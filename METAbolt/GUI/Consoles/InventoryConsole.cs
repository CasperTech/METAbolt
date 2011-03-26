//  Copyright (c) 2008 - 2009, www.metabolt.net
//  All rights reserved.

//  Redistribution and use in source and binary forms, with or without modification, 
//  are permitted provided that the following conditions are met:

//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright notice, 
//    this list of conditions and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution. 
//  * Neither the name METAbolt nor the names of its contributors may be used to 
//    endorse or promote products derived from this software without specific prior 
//    written permission. 

//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
//  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
//  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//  POSSIBILITY OF SUCH DAMAGE.

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SLNetworkComm;
using OpenMetaverse;
using System.Threading;

// Some parts of this code has been adopted from OpenMetaverse.GUI
//
/*
 * Copyright (c) 2007-2008, openmetaverse.org
 * All rights reserved.
*/


namespace METAbolt
{
    public partial class InventoryConsole : UserControl
    {
        InventoryFolder ifolder;

        private GridClient client;
        private SLNetCom netcom;
        private METAboltInstance instance;
        private InventoryItemConsole currentProperties;
        //private AutoChanger autoc;
        private InventoryClipboard clip;
        private InventoryTreeSorter treeSorter = new InventoryTreeSorter();
        private bool ShowAuto = false;
        private string SortBy = "Date";
        ManualResetEvent CurrentlyWornEvent = new ManualResetEvent(false);

        //private frmMain mainForm;
        //private TabsConsole tconsole; 

        //private InventoryManager.FolderUpdatedCallback folderUpdate;
        //private InventoryManager.ItemReceivedCallback itemReceived;
        //private InventoryManager.ObjectOfferedCallback objectOffer;

        // auto changer vars
        private string textfile; // = "Outfit.txt";
        private string path; // = Path.Combine(Environment.CurrentDirectory, "Outfit.txt");
        private int x = 0;
        public bool managerbusy = false;

        public InventoryConsole(METAboltInstance instance)
        {
            InitializeComponent();

            this.instance = instance;
            client = this.instance.Client;
            clip = new InventoryClipboard(client);
            netcom = this.instance.Netcom;
            netcom.NetcomSync = this;

            //mainForm = new frmMain(this.instance);
            //tconsole = mainForm.TabConsole;
            //tconsole = new TabsConsole(this.instance);  

            //ShowAuto = false;
            
            InitializeTree();

            textfile = client.Self.FirstName + "_" + client.Self.LastName + "_" + "Outfit.mtb";
            path = Path.Combine(Environment.CurrentDirectory, textfile);

            ReadTextFile();

            netcom.ClientLoggedOut += new EventHandler(netcom_ClientLoggedOut);
            netcom.ClientDisconnected += new EventHandler<ClientDisconnectEventArgs>(netcom_ClientDisconnected);
            //client.Objects.OnNewAttachment += new ObjectManager.NewAttachmentCallback(Inventory_NewAttachment);
        }

        //Seperate thread
        private void Inventory_OnItemReceived(InventoryItem item)
        {
            BeginInvoke(
                new InventoryManager.ItemReceivedCallback(ReceivedInventoryItem),
                new object[] { item });
        }

        ////Seperate thread
        //private void Inventory_NewAttachment(Simulator sim, Primitive prim, ulong l, ushort u)
        //{
        //    uint lid = prim.LocalID;
        //    //BeginInvoke(
        //    //    new InventoryManager.ItemReceivedCallback(ReceivedAttachement),
        //    //    new object[] { prim });
        //}

        //UI thread
        private void ReceivedAttachement(Primitive prim)
        {
            //UpdateFolder(item.ParentUUID);
        }

        //UI thread
        private void ReceivedInventoryItem(InventoryItem item)
        {
            UpdateFolder(item.ParentUUID);
        }

        //Separate thread
        private bool Inventory_OnInventoryObjectReceived(InstantMessage offer, AssetType type, UUID objectID, bool fromTask)
        {
            BeginInvoke(
                new InventoryManager.ObjectOfferedCallback(ReceivedInventoryOffer),
                new object[] { offer, type, objectID, fromTask });

            return true;
        }

        private void Store_OnInventoryObjectRemoved(InventoryBase obj)
        {
            UpdateFolder(obj.ParentUUID);
        }

        private void Inventory_OnAppearanceSet(bool success)
        {
            RefreshInventory();

            if (managerbusy)
            {
                managerbusy = false;
                client.Appearance.RequestSetAppearance(true);
            }
        }

        //UI thread
        private bool ReceivedInventoryOffer(InstantMessage offer, AssetType type, UUID objectID, bool fromTask)
        {
            if (instance.IsAvatarMuted(offer.FromAgentID))
                return false;

            if (!instance.Config.CurrentConfig.DeclineInv)
            {
                (new frmInvOffered(instance, offer, objectID, type)).Show();

                if (!client.Inventory.Store.Contains(objectID)) return true;

                InventoryBase invObj = client.Inventory.Store[objectID];

                UpdateFolder(invObj.ParentUUID);
                return true;
            }
            else
            {
                if (type != AssetType.Notecard)
                {
                    client.Inventory.RemoveItem(objectID);
                    //client.Self.Chat("Inventory item decline", 0, ChatType.OwnerSay);   
                    return false;
                }
                else
                {
                    if (!client.Inventory.Store.Contains(objectID)) return true;

                    InventoryBase invObj = client.Inventory.Store[objectID];

                    UpdateFolder(invObj.ParentUUID);

                    return true;
                }
            }            
        }

        //Seperate thread
        private void Inventory_OnFolderUpdated(UUID folderID)
        {
            BeginInvoke(
                new InventoryManager.FolderUpdatedCallback(FolderDownloadFinished),
                new object[] { folderID });
        }

        //UI thread
        private void FolderDownloadFinished(UUID folderID)
        {
            //InventoryBase invObj = client.Inventory.Store[folderID];
            //ProcessIncomingObject(invObj);
            UpdateFolder(folderID);
        }

        private void netcom_ClientLoggedOut(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void netcom_ClientDisconnected(object sender, ClientDisconnectEventArgs e)
        {
            CleanUp();
        }

        private void CleanUp()
        {
            ClearCurrentProperties();
            timer1.Enabled = false; 
        }

        public void UpdateFolder(UUID folderID)
        {
            if (this.InvokeRequired) this.BeginInvoke((MethodInvoker)delegate { UpdateFolder(folderID); });
            else
            {
                TreeNode node = null;
                TreeNodeCollection children;

                if (folderID != client.Inventory.Store.RootFolder.UUID)
                {
                    TreeNode[] found = treeView1.Nodes.Find(folderID.ToString(), true);
                    if (found.Length > 0)
                    {
                        node = found[0];

                        string foldername = node.Text;

                        if (foldername.ToLower() == "objects")
                        {
                            // store the UUID
                            instance.Config.CurrentConfig.ObjectsFolder = folderID;
                        }

                        children = node.Nodes;
                    }
                    else
                    {
                        Logger.Log("Received update for unknown TreeView node " + folderID, Helpers.LogLevel.Warning);
                        return;
                    }
                }
                else
                {
                    children = treeView1.Nodes;
                }

                children.Clear();

                List<InventoryBase> contents = client.Inventory.Store.GetContents(folderID);
                
                if (contents.Count == 0)
                {
                    TreeNode add = children.Add(null, "(empty)");
                    add.ForeColor = Color.FromKnownColor(KnownColor.GrayText);
                }
                else
                {
                    foreach (InventoryBase inv in contents)
                    {
                        string key = inv.UUID.ToString();
                        bool isinvfolder = inv is InventoryFolder;

                        try
                        {
                            string worn = string.Empty;

                            if (!isinvfolder)
                            {
                                // Determine all worn clothing items
                                InventoryItem item = (InventoryItem)inv;
                                WearableType wtype;

                                wtype = client.Appearance.IsItemWorn(item);

                                if (wtype != WearableType.Invalid)
                                {
                                    worn = " (WORN)";
                                }
                            }

                            children.Add(key, inv.Name + worn);
                            children[key].Tag = inv;

                            if (isinvfolder)
                            {
                                if (inv.Name.ToLower() == "objects")
                                {
                                    // store the LLUUID
                                    instance.Config.CurrentConfig.ObjectsFolder = inv.UUID;
                                }

                                children[key].Nodes.Add(null, "(loading...)").ForeColor = Color.FromKnownColor(KnownColor.GrayText);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Inventory error: " + ex.Message, Helpers.LogLevel.Error);     
                        }
                    }
                }
            }
        }

        private void InitializeTree()
        {
            try
            {
                //client.Inventory.OnFolderUpdated += new InventoryManager.FolderUpdatedCallback(Inventory_OnFolderUpdated);
                client.Inventory.OnFolderUpdated += new InventoryManager.FolderUpdatedCallback(Inventory_OnFolderUpdated);
                client.Inventory.OnItemReceived += new InventoryManager.ItemReceivedCallback(Inventory_OnItemReceived);
                client.Inventory.OnObjectOffered += new InventoryManager.ObjectOfferedCallback(Inventory_OnInventoryObjectReceived);
                client.Appearance.OnAppearanceSet += new AppearanceManager.AppearanceSetCallback(Inventory_OnAppearanceSet);
                client.Inventory.Store.OnInventoryObjectRemoved += new Inventory.InventoryObjectRemoved(Store_OnInventoryObjectRemoved);

                foreach (ITreeSortMethod method in treeSorter.GetSortMethods())
                {
                    ToolStripMenuItem item = (ToolStripMenuItem)tbtnSort.DropDown.Items.Add(method.Name);
                    item.ToolTipText = method.Description;
                    item.Name = method.Name;
                    item.Click += new EventHandler(SortMethodClick);
                }

                ((ToolStripMenuItem)tbtnSort.DropDown.Items[1]).PerformClick();
                treeView1.TreeViewNodeSorter = treeSorter;

                UpdateFolder(client.Inventory.Store.RootFolder.UUID);
                ((ToolStripMenuItem)tbtnSort.DropDown.Items[1]).PerformClick();
            }
            catch (Exception ex)
            {
                Logger.Log("Inventory error (initialise tree): " + ex.Message, Helpers.LogLevel.Error);     
            }
        }

        //void Inventory_OnFolderUpdated(UUID folderID)
        //{
        //    UpdateFolder(folderID);
        //}

        private void SortMethodClick(object sender, EventArgs e)
        {
            client.Inventory.OnFolderUpdated -= new InventoryManager.FolderUpdatedCallback(Inventory_OnFolderUpdated);

            if (!string.IsNullOrEmpty(treeSorter.CurrentSortName))
                ((ToolStripMenuItem)tbtnSort.DropDown.Items[treeSorter.CurrentSortName]).Checked = false;

            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            treeSorter.CurrentSortName = item.Text;

            treeView1.BeginUpdate();
            treeView1.Sort();
            treeView1.EndUpdate();

            item.Checked = true;

            client.Inventory.OnFolderUpdated += new InventoryManager.FolderUpdatedCallback(Inventory_OnFolderUpdated);
        }

        private void RefreshPropertiesPane()
        {
            if (treeView1.SelectedNode == null) return;

            InventoryBase io = (InventoryBase)treeView1.SelectedNode.Tag;

            if (io is InventoryItem)
            {
                panel2.Visible = false;

                //InventoryImageConsole.vi 
                InventoryItemConsole console = new InventoryItemConsole(instance, (InventoryItem)io);
                console.Dock = DockStyle.Fill;
                splitContainer1.Panel2.Controls.Add(console);

                ClearCurrentProperties();
                //ClearAutoProperties();
                currentProperties = console;
                InventoryItem item = (InventoryItem)io;

                //item.InventoryType 

                try
                {
                    if (item.InventoryType == InventoryType.Wearable || item.InventoryType == InventoryType.Attachment || item.InventoryType == InventoryType.Object)
                    {
                        console.Controls["btnDetach"].Visible = true;
                        console.Controls["btnWear"].Visible = true;
                        console.Controls["btnTP"].Visible = false;
                    }
                    else if (item.InventoryType == InventoryType.Landmark)
                    {
                        console.Controls["btnTP"].Visible = true;
                    }
                    else
                    {
                        console.Controls["btnDetach"].Visible = false;
                        console.Controls["btnWear"].Visible = false;
                        console.Controls["btnTP"].Visible = false;
                    }

                    console.Controls["btnGive"].Visible = true;
                }
                catch (Exception ex)
                {
                    // do nothing
                }
            }
            else
            {
                if (ShowAuto)
                {
                    panel2.Visible = true;
                    textBox2.Text = io.Name.ToString();  
                    ClearCurrentProperties();
                }
                else
                {
                    ClearCurrentProperties();
                }
            }
        }

        private void ClearCurrentProperties()
        {
            if (currentProperties == null) return;

            currentProperties.CleanUp();
            currentProperties.Dispose();
            currentProperties = null;
        }

        private void tmnuDelete_Click(object sender, EventArgs e)
        {
            DeleteItem(treeView1.SelectedNode);
        }

        private void DeleteItem(TreeNode node)
        {
            if (node == null) return;

            InventoryBase io = (InventoryBase)node.Tag;

            if (io is InventoryFolder)
            {
                InventoryFolder folder = (InventoryFolder)io;
                //treeLookup.Remove(folder.UUID);
                client.Inventory.RemoveFolder(folder.UUID);
                folder = null;
            }
            else if (io is InventoryItem)
            {
                InventoryItem item = (InventoryItem)io;
                //treeLookup.Remove(item.UUID);
                client.Inventory.RemoveItem(item.UUID);
                item = null;
            }

            io = null;

            node.Remove();
            node = null;
        }

        private void tmnuCut_Click(object sender, EventArgs e)
        {
            clip.SetClipboardNode(treeView1.SelectedNode, true);
            tmnuPaste.Enabled = true;
        }

        private void tmnuPaste_Click(object sender, EventArgs e)
        {
            clip.PasteTo(treeView1.SelectedNode);
            tmnuPaste.Enabled = false;
        }

        private void tmnuNewFolder_Click(object sender, EventArgs e)
        {
            //string newFolderName = "New Folder";

            ////if (treeView1.SelectedNode == null)
            ////    AddNewFolder(newFolderName, treeView1.Nodes[0]);
            ////else
            ////    AddNewFolder(newFolderName, treeView1.SelectedNode);

            //AddNewFolder(newFolderName, treeView1.SelectedNode);
        }

        private void AddNewFolder(string folderName, TreeNode node)
        {
            //if (node == null) return;

            //InventoryFolder folder = null;
            //TreeNode folderNode = null;

            //if (node.Tag is InventoryFolder)
            //{
            //    folder = (InventoryFolder)node.Tag;
            //    folderNode = node;
            //}
            //else if (node.Tag is InventoryItem)
            //{
            //    folder = (InventoryFolder)node.Parent.Tag;
            //    folderNode = node.Parent;
            //}

            //treeView1.BeginUpdate();

            //UUID newFolderID = client.Inventory.CreateFolder(folder.UUID, folderName, AssetType.Folder);
            //InventoryFolder newFolder = (InventoryFolder)client.Inventory.Store[newFolderID];
            //TreeNode newNode = AddTreeFolder(newFolder, folderNode);

            //treeView1.Sort();
            //treeView1.EndUpdate();
        }

        //private TreeNode AddTreeFolder(InventoryFolder folder, TreeNode node)
        //{
        //    //TreeNode folderNode = node.Nodes.Add(folder.UUID.ToString(), folder.Name);
        //    //folderNode.Tag = folder;
        //    ////folderNode.ImageKey = "ClosedFolder";

        //    //return folderNode;
        //}

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Tag == null)
            {
                InventoryFolder folder = (InventoryFolder)e.Node.Tag;

                if (SortBy == "Date")
                {
                    client.Inventory.RequestFolderContents(folder.UUID, client.Self.AgentID, true, true, InventorySortOrder.ByDate | InventorySortOrder.FoldersByName);
                }
                else
                {
                    client.Inventory.RequestFolderContents(folder.UUID, client.Self.AgentID, true, true, InventorySortOrder.ByName | InventorySortOrder.FoldersByName);
                }
            }
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode aNode = (TreeNode)e.Item;

            InventoryBase io = (InventoryBase)aNode.Tag;
            InventoryItem item = (InventoryItem)io;
            string sItem = string.Empty;
            string sCopy = "False";

            if ((item.Permissions.OwnerMask & PermissionMask.Transfer) != PermissionMask.Transfer)
            {
                return;
            }

            if ((item.Permissions.OwnerMask & PermissionMask.Copy) == PermissionMask.Copy)
            {
                sCopy = "True";
            }
            else
            {
                sCopy = "False";
            }
            
            if (item.AssetType == AssetType.ImageJPEG || item.AssetType == AssetType.ImageTGA || item.AssetType == AssetType.Texture || item.AssetType == AssetType.TextureTGA)
            {
                if ((item.Permissions.OwnerMask & PermissionMask.Modify) != PermissionMask.Modify)
                {
                    return;
                }

                sItem = item.UUID + "," + item.Name + "," + item.AssetType + "," + item.AssetUUID + "," + sCopy;
            }
            else
            {
                sItem = item.UUID + "," + item.Name + "," + item.AssetType + "," + sCopy;
            }

            if ((item.Permissions.OwnerMask & PermissionMask.Transfer) == PermissionMask.Transfer)
            {
                DoDragDrop(new DataObject(DataFormats.FileDrop, sItem),
                   DragDropEffects.Move);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //tbtnNew.Enabled = tbtnOrganize.Enabled = (treeView1.SelectedNode != null);
            tbtnNew.Enabled = tbtnSort.Enabled = tbtnOrganize.Enabled = (treeView1.SelectedNode != null);

            InventoryBase io = (InventoryBase)treeView1.SelectedNode.Tag;
   
            if (e.Node.Tag is InventoryFolder)
            {
                tmnuRename.Enabled = false;
                //return;
            }
            else if (e.Node.Tag is InventoryItem)
            {
                tmnuRename.Enabled = true;
                //return;
            }

            RefreshPropertiesPane();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tbtnSort_Click(object sender, EventArgs e)
        {

        }

        private void tmnuNewNotecard_Click(object sender, EventArgs e)
        {
            string newNotecardName = "New Notecard";
            string newNotecardDescription = String.Format("{0} created with METAbolt {1}", newNotecardName, DateTime.Now); ;
            string newNotecardContent = string.Empty;


            if (treeView1.SelectedNode == null)
                AddNewNotecard(
                    newNotecardName,
                    newNotecardDescription,
                    newNotecardContent,
                    treeView1.Nodes[0]);
            else
                AddNewNotecard(
                    newNotecardName,
                    newNotecardDescription,
                    newNotecardContent,
                    treeView1.SelectedNode);
        }

        private void AddNewNotecard(string notecardName, string notecardDescription, string notecardContent, TreeNode node)
        {
            if (node == null) return;

            InventoryFolder folder = null;
            TreeNode folderNode = null;

            if (node.Tag is InventoryFolder)
            {
                folder = (InventoryFolder)node.Tag;
                folderNode = node;
            }
            else if (node.Tag is InventoryItem)
            {
               
                 folder = (InventoryFolder)node.Parent.Tag;
                 folderNode = node.Parent;
            }

            if (node.Text == "(empty)")
            {
                folder = (InventoryFolder)node.Parent.Tag;
                folderNode = node.Parent;
            }

            ifolder = folder; 
            
            client.Inventory.RequestCreateItem(folder.UUID,
                    notecardName, notecardDescription, AssetType.Notecard, UUID.Random(), InventoryType.Notecard, PermissionMask.All,
                    delegate(bool success, InventoryItem nitem)
                    {
                        if (success) // upload the asset
                            client.Inventory.RequestUploadNotecardAsset(CreateNotecardAsset(""), nitem.UUID, new InventoryManager.InventoryUploadedAssetCallback(OnNoteUpdate));
                    }
                );
        }

        private void TakeSnapshot(string notecardName, string notecardDescription, string notecardContent, TreeNode node)
        {
            
        }

        /// <summary>
        /// </summary>
        /// <param name="body"></param>
        public byte[] CreateNotecardAsset(string body)
        {
            body = body.Trim();

            // Format the string body into Linden text
            string lindenText = "Linden text version 1\n";
            lindenText += "{\n";
            lindenText += "LLEmbeddedItems version 1\n";
            lindenText += "{\n";
            lindenText += "count 0\n";
            lindenText += "}\n";
            lindenText += "Text length " + body.Length + "\n";
            lindenText += body;
            lindenText += "}\n";

            // Assume this is a string, add 1 for the null terminator
            byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(lindenText);
            byte[] assetData = new byte[stringBytes.Length]; //+ 1];
            Array.Copy(stringBytes, 0, assetData, 0, stringBytes.Length);

            return assetData;
        }

        void OnNoteUpdate(bool success, string status, UUID itemID, UUID assetID)
        {
            if (success)
            {
                UpdateFolder(ifolder.UUID);   
            }
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                panel1.Visible = false;
                textBox1.Text = string.Empty;   
            }
            else
            {
                panel1.Visible = true;
                textBox1.Focus();  
            }
        }

        private void tmnuCopy_Click(object sender, EventArgs e)
        {

        }

        private void tmnuRename_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.BeginEdit();   
        }

        private void FindByText()
        {
            Boolean found = false; 
            TreeNodeCollection nodes = treeView1.Nodes;

            foreach (TreeNode n in nodes)
            {
                found = FindRecursive(n);

                if (!found)
                {
                    n.Collapse();
                    found = false;
                }
            }
        }

        private Boolean FindRecursive(TreeNode treeNode)
        {
            string searchstring = textBox1.Text.Trim();
            searchstring = searchstring.ToLower();
            Boolean found = false; 

            foreach (TreeNode tn in treeNode.Nodes)
            {
                // if the text properties match, color the item
                if (tn.Text.ToLower().Contains(searchstring))
                {
                    tn.BackColor = Color.Yellow;
                    tn.ForeColor = Color.Red;  
                    found = true; 
                }

                if (!found)
                {
                    tn.Collapse();
                    found = false;
                }

                FindRecursive(tn);
            }

            return found;
        }

        private void ClearBackColor()
        {
            TreeNodeCollection nodes = treeView1.Nodes;

            foreach (TreeNode n in nodes)
            {
                ClearRecursive(n);
            }
        }

        // called by ClearBackColor function
        private void ClearRecursive(TreeNode treeNode)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                tn.BackColor = Color.White;
                tn.ForeColor = Color.Black;  
                ClearRecursive(tn);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                client.Inventory.OnFolderUpdated -= new InventoryManager.FolderUpdatedCallback(Inventory_OnFolderUpdated);
                treeView1.ExpandAll();
                ClearBackColor();

                //PB1.Visible = true;  
                //Thread.Sleep(7000);
                FindByText();

                client.Inventory.OnFolderUpdated += new InventoryManager.FolderUpdatedCallback(Inventory_OnFolderUpdated);
            }
            catch (Exception ex)
            {
                // do nothing
            }
            //PB1.Visible = false; 
        }

        private void unExpandInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearBackColor();
            treeView1.CollapseAll();  
        }

        private void expandInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();  
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = (textBox1.Text.Trim().Length > 0);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;

            if (textBox1.Text.Trim().Length < 1) return;

            treeView1.ExpandAll();
            ClearBackColor();

            FindByText();
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.CancelEdit) return;

            InventoryBase io = (InventoryBase)treeView1.SelectedNode.Tag;
   
            if (e.Node.Tag is InventoryFolder)
            {
                //InventoryFolder item = (InventoryFolder)io;

                e.CancelEdit = true;
                return;
            }
            else if (e.Node.Tag is InventoryItem)
            {
                //protect against an empty name
                if (e.Label == null || e.Label == string.Empty)
                {
                    e.CancelEdit = true;
                    Logger.Log("Attempt to give inventory item a blank name was foiled!", Helpers.LogLevel.Warning);
                    return;
                }

                ((InventoryItem)e.Node.Tag).Name = e.Label;
                InventoryItem item = (InventoryItem)io;

                if ((item.Permissions.OwnerMask & PermissionMask.Modify) == PermissionMask.Modify)
                {
                    client.Inventory.RequestUpdateItem(item);
                }
                else
                {
                    e.CancelEdit = true;
                    return;
                }
            }

            e.Node.Text = e.Label;

            // Refresh the inventory to reflect the change
            refreshFolderToolStripMenuItem.PerformClick();
        }

        private void refreshFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshInventory(); 
        }

        public void RefreshInventory()
        {
            if (this.InvokeRequired) this.BeginInvoke((MethodInvoker)delegate { RefreshInventory(); });
            else
            {
                TreeNode node = treeView1.SelectedNode;

                if (node == null) return;

                //if (node.Text == "(empty)") return;  

                InventoryFolder folder = null;
                TreeNode folderNode = null;

                if (node.Tag is InventoryFolder)
                {
                    folder = (InventoryFolder)node.Tag;
                    folderNode = node;
                }
                else if (node.Tag is InventoryItem)
                {
                    folder = (InventoryFolder)node.Parent.Tag;
                    folderNode = node.Parent;
                }

                if (node.Text == "(empty)")
                {
                    folder = (InventoryFolder)node.Parent.Tag;
                    folderNode = node.Parent;
                }

                UpdateFolder(folder.UUID);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string clth = string.Empty; // String.Empty;
            clth = "Clothing/" + listBox1.Items[x].ToString();

            try
            {
                UUID cfolder = client.Inventory.FindObjectByPath(client.Inventory.Store.RootFolder.UUID, client.Self.AgentID, clth, 30 * 1000);

                if (cfolder == UUID.Zero)
                {
                    Logger.Log("Outfit changer: outfit path '" + clth + "' not found", Helpers.LogLevel.Warning);
                    return;
                }

                List<InventoryBase> contents = client.Inventory.FolderContents(cfolder, client.Self.AgentID, true, true, InventorySortOrder.ByName, 20 * 1000);
                List<InventoryItem> items = new List<InventoryItem>();

                if (contents == null)
                {
                    Logger.Log("Outfit changer: failed to get contents of '" + clth + "'", Helpers.LogLevel.Warning);
                    return;
                }

                foreach (InventoryBase item in contents)
                {
                    if (item is InventoryItem)
                        items.Add((InventoryItem)item);
                }

                client.Appearance.ReplaceOutfit(items);

                Logger.Log("Outfit changer: Starting to change outfit to '" + clth + "'", Helpers.LogLevel.Info);
                label5.Text = "Currently wearing folder : " + clth;

                double ntime = Convert.ToDouble(trackBar1.Value);
                DateTime nexttime = DateTime.Now;
                nexttime = nexttime.AddMinutes(ntime);
                label6.Text = "Next clothes change @ " + nexttime.ToShortTimeString();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Helpers.LogLevel.Error);
                string exp = ex.Message.ToString();
            }  

            if (x < (listBox1.Items.Count - 1))
            {
                x += 1;
            }
            else
            {
                x = 0;
            }  
        }

        private void ClearCache()
        {
            string folder = client.Settings.ASSET_CACHE_DIR;

            if (!Directory.Exists(folder))
            {
                return;
            }

            string[] files = Directory.GetFiles(@folder);

           
            foreach (string file in files)
                File.Delete(file);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            InventoryBase io = (InventoryBase)treeView1.SelectedNode.Tag;

            if (io is InventoryFolder)
            {
                panel2.Visible = true;
                ClearCurrentProperties();
                ShowAuto = true;
            }
            else
            {
                MessageBox.Show("Select a clothes folder first", "METAbolt");  
            }
        }


        // Auto changer procs start here
        private void button5_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            ShowAuto = false;
        }

        private void AddToFile()
        {
            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter sr = File.CreateText(path))
            {
                foreach (object o in listBox1.Items)
                {
                    // write a line of text to the file
                    sr.WriteLine(o.ToString());
                }

                sr.Close();
                sr.Dispose();
            }
        }

        private void ReadTextFile()
        {
            try
            {
                if (!File.Exists(path))
                {
                    return;
                }

                listBox1.Items.Clear();

                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";

                    while ((s = sr.ReadLine()) != null)
                    {
                        listBox1.Items.Add(s);
                    }

                    sr.Close();
                    sr.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Inventory erro (read text file): " + ex.Message, Helpers.LogLevel.Error);      
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "Start")
            {
                if (trackBar1.Value == 0)
                {
                    MessageBox.Show("Select a frequency from the slider first.", "METAbolt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                x = 0;
                timer1.Interval = ((trackBar1.Value) * 60) * 1000;
                timer1.Enabled = true;
                button3.Enabled = false;
                button4.Text = "Stop";

                double ntime = Convert.ToDouble(trackBar1.Value);
                DateTime nexttime = DateTime.Now;
                nexttime = nexttime.AddMinutes(ntime);
                label6.Text = "Next clothes change @ " + nexttime.ToShortTimeString();

                AddToFile();
            }
            else
            {
                timer1.Enabled = false; 
                button3.Enabled = true;
                button4.Text = "Start";
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label4.Text = "Every " + trackBar1.Value.ToString() + " minutes";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox2.Text);
            listBox1.Sorted = true;
            textBox2.Text = "Select folder from inventory";
            textBox2.Focus(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select an entry to remove first", "METAbolt");
                return;
            }

            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                textBox2.Text = listBox1.SelectedItem.ToString();
            }

            button2.Enabled = listBox1.SelectedIndex > -1;

            if (button4.Text == "Start")
            {
                button3.Enabled = listBox1.SelectedIndex > -1;
            }
            else
            {
                button3.Enabled = false; 
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.SelectAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string clth = string.Empty;
  
            try
            {
                clth = "Clothing/" + listBox1.SelectedItem.ToString();
                UUID cfolder = client.Inventory.FindObjectByPath(client.Inventory.Store.RootFolder.UUID, client.Self.AgentID, clth, 20 * 1000);

                if (cfolder == UUID.Zero)
                {
                    Logger.Log("Outfit changer: outfit path '" + clth + "' not found", Helpers.LogLevel.Warning);
                    return;
                }

                List<InventoryBase> contents = client.Inventory.FolderContents(cfolder, client.Self.AgentID, true, true, InventorySortOrder.ByName, 20 * 1000);
                List<InventoryItem> items = new List<InventoryItem>();

                if (contents == null)
                {
                    Logger.Log("Outfit changer: failed to get contents of '" + clth + "'", Helpers.LogLevel.Warning);
                    return;
                }

                foreach (InventoryBase item in contents)
                {
                    if (item is InventoryItem)
                        items.Add((InventoryItem)item);
                }

                client.Appearance.ReplaceOutfit(items);

                Logger.Log("Outfit changer: Starting to change outfit to '" + clth + "'", Helpers.LogLevel.Info);
                label5.Text = "Currently wearing folder : " + clth;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Helpers.LogLevel.Error);
                string exp = ex.Message.ToString();  
            }
        }

        private void snapshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string newNotecardName = "Snap1";
            //string newNotecardDescription = String.Format("{0} created with METAbolt {1}", newNotecardName, DateTime.Now); ;
            //string newNotecardContent = string.Empty;


            //if (treeView1.SelectedNode == null)
            //    TakeSnapshot(
            //        newNotecardName,
            //        newNotecardDescription,
            //        newNotecardContent,
            //        treeView1.Nodes[0]);
            //else
            //    TakeSnapshot(
            //        newNotecardName,
            //        newNotecardDescription,
            //        newNotecardContent,
            //        treeView1.SelectedNode);
        }

        private void tmnuNewScript_Click(object sender, EventArgs e)
        {
            string newScriptName = "New Script";
            string newScriptDescription = String.Format("{0} created with METAbolt {1}", newScriptName, DateTime.Now); ;
            string newScriptContent = string.Empty;


            if (treeView1.SelectedNode == null)
                AddNewScript(
                    newScriptName,
                    newScriptDescription,
                    newScriptContent,
                    treeView1.Nodes[0]);
            else
                AddNewScript(
                    newScriptName,
                    newScriptDescription,
                    newScriptContent,
                    treeView1.SelectedNode);
        }

        private void AddNewScript(string scriptName, string scriptDescription, string scriptContent, TreeNode node)
        {
            if (node == null) return;

            InventoryFolder folder = null;
            TreeNode folderNode = null;

            if (node.Tag is InventoryFolder)
            {
                folder = (InventoryFolder)node.Tag;
                folderNode = node;
            }
            else if (node.Tag is InventoryItem)
            {

                folder = (InventoryFolder)node.Parent.Tag;
                folderNode = node.Parent;
            }

            if (node.Text == "(empty)")
            {
                folder = (InventoryFolder)node.Parent.Tag;
                folderNode = node.Parent;
            }

            ifolder = folder;

            client.Inventory.RequestCreateItem(folder.UUID,
                    scriptName, scriptDescription, AssetType.LSLText, UUID.Random(), InventoryType.LSL, PermissionMask.All,
                    delegate(bool success, InventoryItem nitem)
                    {
                        if (success) // upload the asset
                        {
                            string scriptbody = "default\n{\n    state_entry()\n    {\n        llSay(0,'Hello METAbolt user');\n    }\n}";
                            client.Inventory.RequestUploadNotecardAsset(CreateScriptAsset(scriptbody), nitem.UUID, new InventoryManager.InventoryUploadedAssetCallback(OnNoteUpdate));
                            //client.Inventory.RequestUploadNotecardAsset(Utils.StringToBytes("Script code..."), nitem.UUID,delegate (bool success2,string status,UUID item_uuid, UUID asset_uuid)
                        }
                    }
                );
        }

        public byte[] CreateScriptAsset(string body)
        {
            body = body.Trim();
             
            // Format the string body into Linden text
            string lindenText = body;

            // Assume this is a string, add 1 for the null terminator
            byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(lindenText);
            byte[] assetData = new byte[stringBytes.Length]; //+ 1];
            Array.Copy(stringBytes, 0, assetData, 0, stringBytes.Length);

            return assetData;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tmnuRename.PerformClick();
        }

        private void expandInventoryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is InventoryItem)
            {
                InventoryBase io = (InventoryBase)treeView1.SelectedNode.Tag;
                InventoryItem item = (InventoryItem)io;

                if (item.InventoryType != InventoryType.Landmark)
                    return;

                UUID landmark = new UUID();

                if (!UUID.TryParse(io.UUID.ToString(), out landmark))
                {
                    MessageBox.Show("Invalid TP LLUID", "Teleport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                ////if (client.Self.Teleport(landmark))
                if (client.Self.Teleport(item.AssetUUID))
                {
                    MessageBox.Show("Teleport successful", "Teleport", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Teleport failed", "Teleport", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

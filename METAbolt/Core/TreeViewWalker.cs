using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenMetaverse;

namespace METAbolt
{
    /// <summary>
    /// Provides a generic mechanism for navigating the nodes in a TreeView control.  Call the ProcessTree method to 
    /// start the navigation process for an entire TreeView.  Call ProcessBranch to navigate only a subset of a TreeView's nodes.
    /// The ProcessNode event will fire for every node in the tree or branch, unless the processing is aborted before reaching the last node.
    /// </summary>
    public class TreeViewWalker
    {
        #region Data

        private GridClient client;
        private METAboltInstance instance;
        private TreeView treeView;
        private bool stopProcessing = false;

        #endregion // Data

        #region Constructors

        /// <summary>
        /// Creates an empty instance.  Set the TreeView property to a TreeView instance before calling ProcessTree.
        /// </summary>
        public TreeViewWalker()
        {
        }

        /// <summary>
        /// Creates an instance which references the specified TreeView.
        /// </summary>
        /// <param name="treeView">The TreeView to navigate.</param>
        public TreeViewWalker(TreeView treeView)
        {
            this.treeView = treeView;
        }

        #endregion // Constructors

        #region Public Interface

        #region ProcessNode [event]

        /// <summary>
        /// This event is raised when the TreeViewWalker navigates to a TreeNode in a TreeView.
        /// </summary>
        public event ProcessNodeEventHandler ProcessNode;

        #endregion // ProcessNode [event]

        #region ProcessBranch

        /// <summary>
        /// Navigates the node branch which starts with the specified node and fires the ProcessNode event for every TreeNode it encounters.
        /// The TreeNode passed to this method does not have to belong to the TreeView assigned to the TreeView property.
        /// </summary>
        /// <param name="rootNode"></param>
        public void ProcessBranch(TreeNode rootNode)
        {
            if (rootNode == null)
                throw new ArgumentNullException("rootNode");

            // Reset the abort flag in case it was previously set.
            this.stopProcessing = false;

            this.WalkNodes(rootNode);
        }

        #endregion // ProcessBranch

        #region ProcessTree

        /// <summary>
        /// Navigates the TreeView and fires the ProcessNode event for every TreeNode it encounters.
        /// </summary>
        public void ProcessTree()
        {
            if (this.TreeView == null)
                throw new InvalidOperationException("The TreeViewWalker must reference a TreeView when ProcessTree is called.");

            foreach (TreeNode node in this.TreeView.Nodes)
            {
                this.ProcessBranch(node);
                if (this.stopProcessing)
                    break;
            }
        }

        #endregion // ProcessTree

        #region TreeView

        /// <summary>
        /// Gets/sets the TreeView control to navigate.
        /// </summary>
        public TreeView TreeView
        {
            get { return this.treeView; }
            set { this.treeView = value; }
        }

        #endregion // TreeView

        #endregion // Public Interface

        #region Protected Interface

        #region OnProcessNode

        /// <summary>
        /// Raises the ProcessNode event.
        /// </summary>
        /// <param name="e">The event argument.</param>
        protected virtual void OnProcessNode(ProcessNodeEventArgs e)
        {
            ProcessNodeEventHandler handler = this.ProcessNode;
            if (handler != null)
                handler(this, e);
        }

        #endregion // OnProcessNode

        #endregion // Protected Interface

        #region Private Helpers

        #region WalkNodes

        private bool WalkNodes(TreeNode node)
        {
            // Fire the ProcessNode event.
            ProcessNodeEventArgs args = ProcessNodeEventArgs.CreateInstance(node);
            this.OnProcessNode(args);

            // Cache the value of ProcessSiblings since ProcessNodeEventArgs is a singleton.
            bool processSiblings = args.ProcessSiblings;

            if (args.StopProcessing)
            {
                this.stopProcessing = true;
            }
            else if (args.ProcessDescendants)
            {
                for (int i = 0; i < node.Nodes.Count; ++i)
                    if (!this.WalkNodes(node.Nodes[i]) || this.stopProcessing)
                        break;
            }

            return processSiblings;
        }

        #endregion // WalkNodes

        #endregion // Private Helpers

        public void LoadInventory(METAboltInstance instance, UUID folderID)
        {
            this.instance = instance;
            this.client = this.instance.Client;
            InventoryFolder rootFolder = this.client.Inventory.Store.RootFolder;
            List<InventoryBase> contents = this.client.Inventory.Store.GetContents(folderID);
            if (folderID != this.client.Inventory.Store.RootFolder.UUID)
            {
                if (this.TreeView.Nodes != null)
                {
                    TreeNode[] array = this.TreeView.Nodes.Find(folderID.ToString(), true);
                    if (array.Length > 0)
                    {
                        TreeNodeCollection nodes = array[0].Nodes;
                        nodes.Clear();
                        if (contents.Count == 0)
                        {
                            nodes.Add(UUID.Zero.ToString(), "(empty)");
                            nodes[UUID.Zero.ToString()].Tag = "empty";
                            nodes[UUID.Zero.ToString()].ForeColor = Color.FromKnownColor(KnownColor.GrayText);
                        }
                        else
                        {
                            List<Primitive> list = this.client.Network.CurrentSim.ObjectsPrimitives.FindAll(delegate(Primitive prim)
                            {
                                bool result;
                                try
                                {
                                    result = (prim.ParentID == instance.Client.Self.LocalID);
                                }
                                catch
                                {
                                    result = false;
                                }
                                return result;
                            });
                            foreach (InventoryBase current in contents)
                            {
                                string key = current.UUID.ToString();
                                bool flag = current is InventoryFolder;
                                try
                                {
                                    string text = string.Empty;
                                    if (!flag)
                                    {
                                        InventoryItem inventoryItem = (InventoryItem)current;
                                        WearableType wearableType = this.client.Appearance.IsItemWorn(inventoryItem);
                                        if (wearableType != WearableType.Invalid)
                                        {
                                            text = " (WORN)";
                                        }
                                        UUID lhs = UUID.Zero;
                                        foreach (Primitive current2 in list)
                                        {
                                            if (current2.NameValues != null)
                                            {
                                                for (int i = 0; i < current2.NameValues.Length; i++)
                                                {
                                                    if (current2.NameValues[i].Name == "AttachItemID")
                                                    {
                                                        lhs = (UUID)current2.NameValues[i].Value.ToString();
                                                        if (lhs == inventoryItem.UUID)
                                                        {
                                                            text = " (WORN)";
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        nodes.Add(key, current.Name + text);
                                        nodes[key].Tag = current;
                                        if (text == " (WORN)")
                                        {
                                            nodes[key].ForeColor = Color.RoyalBlue;
                                        }
                                        string empty = string.Empty;
                                        InventoryType inventoryType = inventoryItem.InventoryType;
                                        switch (inventoryType)
                                        {
                                            case InventoryType.Texture:
                                                nodes[key].ImageKey = "Texture";
                                                continue;
                                            case InventoryType.Sound:
                                            case (InventoryType)4:
                                            case (InventoryType)5:
                                            case InventoryType.Category:
                                            case InventoryType.RootCategory:
                                                break;
                                            case InventoryType.CallingCard:
                                                nodes[key].ImageKey = "CallingCard";
                                                continue;
                                            case InventoryType.Landmark:
                                                nodes[key].ImageKey = "LM";
                                                continue;
                                            case InventoryType.Object:
                                                nodes[key].ImageKey = "Objects";
                                                continue;
                                            case InventoryType.Notecard:
                                                nodes[key].ImageKey = "Notecard";
                                                continue;
                                            case InventoryType.LSL:
                                                nodes[key].ImageKey = "Script";
                                                continue;
                                            default:
                                                if (inventoryType == InventoryType.Snapshot)
                                                {
                                                    nodes[key].ImageKey = "Snapshots";
                                                    continue;
                                                }
                                                if (inventoryType == InventoryType.Wearable)
                                                {
                                                    nodes[key].ImageKey = "Wearable";
                                                    continue;
                                                }
                                                break;
                                        }
                                        nodes[key].ImageKey = "Gear";
                                    }
                                    else
                                    {
                                        nodes.Add(key, current.Name);
                                        nodes[key].Tag = current;
                                        nodes[key].ImageKey = "ClosedFolder";
                                        nodes[key].Nodes.Add(null, "(loading...)").ForeColor = Color.FromKnownColor(KnownColor.GrayText);
                                    }
                                }
                                catch (Exception var_16_4C6)
                                {
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                this.TreeView.Nodes.Clear();
                TreeNode treeNode = this.TreeView.Nodes.Add(rootFolder.UUID.ToString(), "My Inventory");
                treeNode.Tag = rootFolder;
                treeNode.ImageKey = "OpenFolder";
                if (contents.Count == 0)
                {
                    treeNode.Nodes.Add(UUID.Zero.ToString(), "(empty)");
                    treeNode.Nodes[UUID.Zero.ToString()].Tag = "empty";
                    treeNode.Nodes[UUID.Zero.ToString()].ForeColor = Color.FromKnownColor(KnownColor.GrayText);
                }
                else
                {
                    List<Primitive> list = this.client.Network.CurrentSim.ObjectsPrimitives.FindAll(delegate(Primitive prim)
                    {
                        bool result;
                        try
                        {
                            result = (prim.ParentID == instance.Client.Self.LocalID);
                        }
                        catch
                        {
                            result = false;
                        }
                        return result;
                    });
                    foreach (InventoryBase current in contents)
                    {
                        string key = current.UUID.ToString();
                        bool flag = current is InventoryFolder;
                        try
                        {
                            string text = string.Empty;
                            if (!flag)
                            {
                                InventoryItem inventoryItem = (InventoryItem)current;
                                WearableType wearableType = this.client.Appearance.IsItemWorn(inventoryItem);
                                if (wearableType != WearableType.Invalid)
                                {
                                    text = " (WORN)";
                                }
                                UUID lhs = UUID.Zero;
                                foreach (Primitive current2 in list)
                                {
                                    if (current2.NameValues != null)
                                    {
                                        for (int i = 0; i < current2.NameValues.Length; i++)
                                        {
                                            if (current2.NameValues[i].Name == "AttachItemID")
                                            {
                                                lhs = (UUID)current2.NameValues[i].Value.ToString();
                                                if (lhs == inventoryItem.UUID)
                                                {
                                                    text = " (WORN)";
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                treeNode.Nodes.Add(key, current.Name + text);
                                treeNode.Nodes[key].Tag = current;
                                if (text == " (WORN)")
                                {
                                    treeNode.Nodes[key].ForeColor = Color.RoyalBlue;
                                }
                                string empty = string.Empty;
                                InventoryType inventoryType = inventoryItem.InventoryType;
                                switch (inventoryType)
                                {
                                    case InventoryType.Texture:
                                        treeNode.Nodes[key].ImageKey = "Texture";
                                        continue;
                                    case InventoryType.Sound:
                                    case (InventoryType)4:
                                    case (InventoryType)5:
                                    case InventoryType.Category:
                                    case InventoryType.RootCategory:
                                        break;
                                    case InventoryType.CallingCard:
                                        treeNode.Nodes[key].ImageKey = "CallingCard";
                                        continue;
                                    case InventoryType.Landmark:
                                        treeNode.Nodes[key].ImageKey = "LM";
                                        continue;
                                    case InventoryType.Object:
                                        treeNode.Nodes[key].ImageKey = "Objects";
                                        continue;
                                    case InventoryType.Notecard:
                                        treeNode.Nodes[key].ImageKey = "Notecard";
                                        continue;
                                    case InventoryType.LSL:
                                        treeNode.Nodes[key].ImageKey = "Script";
                                        continue;
                                    default:
                                        if (inventoryType == InventoryType.Snapshot)
                                        {
                                            treeNode.Nodes[key].ImageKey = "Snapshots";
                                            continue;
                                        }
                                        if (inventoryType == InventoryType.Wearable)
                                        {
                                            treeNode.Nodes[key].ImageKey = "Wearable";
                                            continue;
                                        }
                                        break;
                                }
                                treeNode.Nodes[key].ImageKey = "Gear";
                            }
                            else
                            {
                                treeNode.Nodes.Add(key, current.Name);
                                treeNode.Nodes[key].Tag = current;
                                treeNode.Nodes[key].ImageKey = "ClosedFolder";
                                treeNode.Nodes[key].Nodes.Add(null, "(loading...)").ForeColor = Color.FromKnownColor(KnownColor.GrayText);
                            }
                        }
                        catch (Exception var_16_4C6)
                        {
                        }
                    }
                    treeNode.Expand();
                }
            }
        }
    }
}
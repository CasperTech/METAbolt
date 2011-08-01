//  Copyright (c) 2008 - 2011, www.metabolt.net (METAbolt)
//  Copyright (c) 2006-2008, Paul Clement (a.k.a. Delta)
//  All rights reserved.

//  Redistribution and use in source and binary forms, with or without modification, 
//  are permitted provided that the following conditions are met:

//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright notice, 
//    this list of conditions and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution. 

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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;

namespace METAbolt
{
    public class InventoryClipboard
    {
        private GridClient client;
        private TreeNode clipNode;
        private InventoryBase clipItem;
        private bool cut = false;

        public InventoryClipboard(GridClient client)
        {
            this.client = client;
        }

        public void SetClipboardNode(TreeNode itemNode, bool ccut)
        {
            clipNode = itemNode;
            clipItem = (InventoryBase)itemNode.Tag;

            this.cut = ccut;

            if (ccut)
            {
                if (clipNode.Parent != null)
                {
                    if (clipNode.Parent.Nodes.Count == 1)
                        clipNode.Parent.Collapse();
                }

                clipNode.Remove();
            }
        }

        public void PasteTo(TreeNode pasteNode)
        {
            if (clipNode == null) return;

            InventoryBase pasteio = (InventoryBase)pasteNode.Tag;

            if (clipItem is InventoryFolder)
            {
                InventoryFolder folder = (InventoryFolder)clipItem;

                if (cut)
                {
                    if (pasteio is InventoryFolder)
                    {
                        client.Inventory.MoveFolder(folder.UUID, pasteio.UUID, folder.Name);
                        pasteNode.Nodes.Add(clipNode);
                    }
                    else if (pasteio is InventoryItem)
                    {
                        client.Inventory.MoveFolder(folder.UUID, pasteio.ParentUUID, folder.Name);
                        pasteNode.Parent.Nodes.Add(clipNode);
                    }
                }
                else
                {
                    if (pasteio is InventoryFolder)
                    {
                        UUID destfolder = client.Inventory.CreateFolder(pasteio.UUID, folder.Name, AssetType.Unknown);

                        List<InventoryBase> contents = client.Inventory.Store.GetContents(folder.UUID);

                        foreach (InventoryItem item in contents)
                        {
                            client.Inventory.RequestCopyItem(item.UUID, destfolder, item.Name, item.OwnerID, (InventoryBase newfolder) => { });
                        }
                    }
                    else if (pasteio is InventoryItem)
                    {
                        UUID destfolder = client.Inventory.CreateFolder(pasteio.ParentUUID, folder.Name, AssetType.Unknown);

                        List<InventoryBase> contents = client.Inventory.Store.GetContents(folder.UUID);

                        foreach (InventoryItem pitem in contents)
                        {
                            client.Inventory.RequestCopyItem(pitem.UUID, destfolder, pitem.Name, pitem.OwnerID, (InventoryBase newfolder) => { });
                        }
                    }
                }

                clipNode.EnsureVisible();
                clipNode = null;
                clipItem = null;
            }
            else if (clipItem is InventoryItem)
            {
                InventoryItem item = (InventoryItem)clipItem;

                if (cut)
                {
                    if (pasteio is InventoryFolder)
                    {
                        client.Inventory.MoveItem(item.UUID, pasteio.UUID, item.Name);
                        pasteNode.Nodes.Add(clipNode);
                    }
                    else if (pasteio is InventoryItem)
                    {
                        client.Inventory.MoveItem(item.UUID, pasteio.ParentUUID, item.Name);
                        pasteNode.Parent.Nodes.Add(clipNode);
                    }
                }
                else
                {
                    if (pasteio is InventoryFolder)
                    {
                        client.Inventory.RequestCopyItem(item.UUID, pasteio.UUID, item.Name, item.OwnerID, (InventoryBase newfolder) => { });
                    }
                    else if (pasteio is InventoryItem)
                    {
                        client.Inventory.RequestCopyItem(item.UUID, pasteio.ParentUUID, item.Name, item.OwnerID, (InventoryBase newfolder) => { });
                    }
                }

                clipNode.EnsureVisible();
                clipNode = null;
                clipItem = null;
            }
        }

        public InventoryBase CurrentClipItem
        {
            get { return clipItem; }
        }

        public TreeNode CurrentClipNode
        {
            get { return clipNode; }
        }
    }
}
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;

namespace METAbolt
{
    public class DateTreeSort : ITreeSortMethod
    {
        private string name = "By Date";
        private string description = "Sorts items in the inventory tree according to date, starting with the newest.";

        #region ITreeSortMethod Members

        public int CompareNodes(InventoryBase x, InventoryBase y, TreeNode nodeX, TreeNode nodeY)
        {
            int returnVal = 0;

            if (x is InventoryItem && y is InventoryItem)
            {
                InventoryItem itemX = (InventoryItem)x;
                InventoryItem itemY = (InventoryItem)y;

                returnVal = -itemX.CreationDate.CompareTo(itemY.CreationDate);
            }
            else if (x is InventoryFolder && y is InventoryFolder)
                returnVal = nodeX.Text.CompareTo(nodeY.Text);
            else if (x is InventoryFolder && y is InventoryItem)
                returnVal = 1;
            else if (x is InventoryItem && y is InventoryFolder)
                returnVal = -1;

            return returnVal;
        }

        public string Name
        {
            get { return name; }
        }

        public string Description
        {
            get { return description; }
        }

        #endregion
    }
}

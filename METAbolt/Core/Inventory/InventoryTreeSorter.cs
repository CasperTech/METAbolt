//  Copyright (c) 2008 - 2013, www.metabolt.net (METAbolt)
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
    public class InventoryTreeSorter : IComparer
    {
        private SafeDictionary<string, ITreeSortMethod> sortMethods = new SafeDictionary<string, ITreeSortMethod>();
        private string currentMethodName;
        private ITreeSortMethod currentMethod;

        public InventoryTreeSorter()
        {
            RegisterSortMethods();
            
            //because the Values property is gay and doesn't have an indexer
            foreach (ITreeSortMethod method in sortMethods.Values)
            {
                currentMethodName = method.Name;
                break;
            }
        }

        private void RegisterSortMethods()
        {
            AddSortMethod(new DateTreeSort());
            AddSortMethod(new NameTreeSort());
        }

        private void AddSortMethod(ITreeSortMethod sort)
        {
            if (!sortMethods.ContainsKey(sort.Name))
            {
            sortMethods.Add(sort.Name, sort);
            }
        }

        public List<ITreeSortMethod> GetSortMethods()
        {
            if (sortMethods.Values.Count == 0) return null;

            List<ITreeSortMethod> methods = new List<ITreeSortMethod>();

            foreach (ITreeSortMethod method in sortMethods.Values)
                methods.Add(method);

            return methods;
        }

        public string CurrentSortName
        {
            get { return currentMethodName; }
            set
            {
                if (!sortMethods.ContainsKey(value))
                    throw new Exception("The specified sort method does not exist.");

                currentMethodName = value;
                currentMethod = sortMethods[currentMethodName];
            }
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            TreeNode nodeX = (TreeNode)x;
            TreeNode nodeY = (TreeNode)y;

            try
            {
                InventoryBase ibX = (InventoryBase)nodeX.Tag;
                InventoryBase ibY = (InventoryBase)nodeY.Tag;

                if (currentMethod == null)
                {
                    currentMethod = sortMethods[currentMethodName];
                }

                try
                {
                    return currentMethod.CompareNodes(ibX, ibY, nodeX, nodeY);
                }
                catch (Exception ex)
                {
                    Logger.Log("Inventory error", Helpers.LogLevel.Error, ex);
                    return 0;
                }
            }
            catch 
            {
                return 0;
            }
        }

        #endregion
    }
}

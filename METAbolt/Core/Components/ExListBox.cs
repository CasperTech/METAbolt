//  Copyright (c) 2008 - 2013, www.metabolt.net (METAbolt)
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;


namespace METAbolt.Core.Components
{
    public class ExListBox : ListBox 
    {
        public ExListBox() : base()
        {
        }

        public bool SortByName;
        public Vector3 location = new Vector3();

        protected override void Sort()
        {
            if (Items.Count > 1)
            {
                bool swapped;
                do
                {
                    int counter = Items.Count - 1;
                    swapped = false;

                    while (counter > 0)
                    {
                        string item1 = this.Items[counter].ToString();
                        string item2 = this.Items[counter - 1].ToString();

                        if (SortByName)
                        {
                            if (item1.CompareTo(item2) == -1)
                            {
                                object temp = Items[counter];
                                this.Items[counter] = this.Items[counter - 1];
                                this.Items[counter - 1] = temp;
                                swapped = true;
                            }
                        }
                        else
                        {
                            ObjectsListItem itemp1 = (ObjectsListItem)this.Items[counter];
                            Vector3 pos1 = new Vector3();
                            pos1 = itemp1.Prim.Position;
                            double dist1 = Math.Round(Vector3.Distance(location, pos1), MidpointRounding.ToEven);

                            ObjectsListItem itemp2 = (ObjectsListItem)this.Items[counter - 1];
                            Vector3 pos2 = new Vector3();
                            pos2 = itemp2.Prim.Position;
                            double dist2 = Math.Round(Vector3.Distance(location, pos2), MidpointRounding.ToEven);

                            if (dist1 < dist2)
                            {
                                object temp = Items[counter];
                                this.Items[counter] = this.Items[counter - 1];
                                this.Items[counter - 1] = temp;
                                swapped = true;
                            }
                        }

                        counter -= 1;
                    }
                }
                while ((swapped == true));
            }
        }

        public void SortList()
        {
            this.Sort();
        } 
    }
}

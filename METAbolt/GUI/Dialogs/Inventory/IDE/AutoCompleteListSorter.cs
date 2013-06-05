//  Copyright (c) 2008-2013, www.metabolt.net
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScintillaNet;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;

namespace METAbolt
{
    #region AutoCompleteListSorter
    public class AutoCompleteStringListSorter : IComparer<string>
    {
        private SortOrder OrderOfSort;
        private CaseInsensitiveComparer ItemComparer;

        public AutoCompleteStringListSorter()
        {
            OrderOfSort = SortOrder.None;
            ItemComparer = new CaseInsensitiveComparer(CultureInfo.CurrentCulture);
        }

        #region IComparer Member

        public int Compare(string x, string y)
        {
            int compareResult = 0;

            String ItemX, ItemY;

            ItemX = x;
            ItemY = y;

            try
            {
                if (ItemX.StartsWith("_", StringComparison.CurrentCultureIgnoreCase) && !ItemY.StartsWith("_", StringComparison.CurrentCultureIgnoreCase))
                {
                    compareResult = 1;
                }
                else if (!ItemX.StartsWith("_", StringComparison.CurrentCultureIgnoreCase) && ItemY.StartsWith("_", StringComparison.CurrentCultureIgnoreCase))
                {
                    compareResult = -1;
                }
                else
                {
                    compareResult = ItemComparer.Compare(ItemX, ItemY);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "AutoCompleteListSorter");
            }

            if (OrderOfSort == SortOrder.Ascending)
            {
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                return (-compareResult);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        public SortOrder SortingOrder
        {
            set
            {
                OrderOfSort = value;
            }

            get
            {
                return OrderOfSort;
            }
        }
    }
    #endregion

    #region SWoDLAutoCompleteItem IComparer Member
    //public class AutoCompleteItemListSorter : IComparer<SWoDLAutoCompleteItem>
    //{
    //    private SortOrder OrderOfSort;
    //    private CaseInsensitiveComparer ItemComparer;

    //    public AutoCompleteItemListSorter()
    //    {

    //        OrderOfSort = SortOrder.None;

    //        ItemComparer = new CaseInsensitiveComparer();
    //    }

    //    #region IComparer Member

    //    public int Compare(SWoDLAutoCompleteItem x, SWoDLAutoCompleteItem y)
    //    {
    //        int compareResult = 0;

    //        SWoDLAutoCompleteItem ItemX, ItemY;

    //        ItemX = x;
    //        ItemY = y;

    //        try
    //        {
    //            if (ItemX.Name.StartsWith("_") && !ItemY.Name.StartsWith("_"))
    //            {
    //                compareResult = 1;
    //            }
    //            else if (!ItemX.Name.StartsWith("_") && ItemY.Name.StartsWith("_"))
    //            {
    //                compareResult = -1;
    //            }
    //            else
    //            {
    //                compareResult = ItemComparer.Compare(ItemX.Name, ItemY.Name);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.ToString(), "AutoCompleteListSorter");
    //        }

    //        if (OrderOfSort == SortOrder.Ascending)
    //        {
    //            return compareResult;
    //        }
    //        else if (OrderOfSort == SortOrder.Descending)
    //        {
    //            return (-compareResult);
    //        }
    //        else
    //        {
    //            return 0;
    //        }
    //    }
    //    #endregion

    //    public SortOrder SortingOrder
    //    {
    //        set
    //        {
    //            OrderOfSort = value;
    //        }

    //        get
    //        {
    //            return OrderOfSort;
    //        }
    //    }
    //}
    #endregion
}

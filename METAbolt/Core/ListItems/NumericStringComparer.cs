//  Copyright (c) 2008 - 2011, www.metabolt.net (METAbolt)
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
using System.Collections;
using System.Windows.Forms;

namespace METAbolt
{
    public sealed class NumericStringComparer : IComparer
    {
        private static readonly IComparer _default = new NumericStringComparer();
        private static readonly IComparer _defaultZeroesFirst = new NumericStringComparer(true);
        private bool zeroesFirst = false;

        private NumericStringComparer()
        { }

        private NumericStringComparer(bool zeroesFirst)
        {
            this.zeroesFirst = zeroesFirst;
        }

        public static IComparer Default
        {
            get { return _default; }
        }

        public static IComparer DefaultZeroesFirst
        {
            get { return _defaultZeroesFirst; }
        }

        public int Compare(object x, object y)
        {
            if (null == x && null == y) return 0;
            if (null == x) return -1;
            if (null == y) return 1;
            if (x is string && y is string)
                return Compare((string)x, (string)y, zeroesFirst);
            return Comparer.Default.Compare(x, y);
        }

        public static int Compare(string s1, string s2)
        {
            return Compare(s1, s2, false);
        }

        public static int Compare(string s1, string s2, bool zeroesFirst)
        {
            //get rid of special cases
            if ((s1 == null) && (s2 == null)) return 0;
            else if (s1 == null) return -1;
            else if (s2 == null) return 1;

            if ((s1.Length <= 0) && (s2.Length <= 0)) return 0;
            else if (s1.Length <= 0) return -1;
            else if (s2.Length <= 0) return 1;

            //special case
            bool sp1 = Char.IsLetterOrDigit(s1[0]);
            bool sp2 = Char.IsLetterOrDigit(s2[0]);
            if (sp1 && !sp2) return 1;
            if (!sp1 && sp2) return -1;

            int i1 = 0, i2 = 0; //current index
            int r = 0; // temp result
            char c1, c2;
            bool letter1, letter2;

            while (true)
            {
                c1 = s1[i1];
                c2 = s2[i2];
                sp1 = Char.IsDigit(c1);
                sp2 = Char.IsDigit(c2);
                if (!sp1 && !sp2)
                {
                    letter1 = Char.IsLetter(c1);
                    letter2 = Char.IsLetter(c2);

                    if (letter1 && letter2)
                    {
                        r = Char.ToUpper(c1).ToString().CompareTo(Char.ToUpper(c2).ToString());
                        if (r != 0) return r;
                    }
                    else if (!letter1 && !letter2)
                    {
                        r = c1.CompareTo(c2);
                        if (r != 0) return r;
                    }
                    else if (!letter1 && letter2)
                    {
                        return -1;
                    }
                    else if (letter1 && !letter2)
                    {
                        return 1;
                    }
                }
                else if (sp1 && sp2)
                {
                    r = CompareNum(s1, ref i1, s2, ref i2, zeroesFirst);
                    if (r != 0) return r;
                }
                else if (sp1)
                {
                    return -1;
                }
                else if (sp2)
                {
                    return 1;
                }
                i1++;
                i2++;
                if ((i1 >= s1.Length) && (i2 >= s2.Length))
                {
                    return 0;
                }
                else if (i1 >= s1.Length)
                {
                    return -1;
                }
                else if (i2 >= s2.Length)
                {
                    return 1;
                }
            }
        }

        private static int CompareNum(string s1, ref int i1, string s2, ref int i2, bool zeroesFirst)
        {
            int nzStart1 = i1, nzStart2 = i2; // nz = non zero
            int end1 = i1, end2 = i2;

            ScanNumEnd(s1, i1, ref end1, ref nzStart1);
            ScanNumEnd(s2, i2, ref end2, ref nzStart2);
            int start1 = i1; i1 = end1 - 1;
            int start2 = i2; i2 = end2 - 1;

            if (zeroesFirst)
            {
                int zl1 = nzStart1 - start1;
                int zl2 = nzStart2 - start2;
                if (zl1 > zl2) return -1;
                if (zl1 < zl2) return 1;
            }

            int nzLength1 = end1 - nzStart1;
            int nzLength2 = end2 - nzStart2;

            if (nzLength1 < nzLength2) return -1;
            else if (nzLength1 > nzLength2) return 1;

            for (int j1 = nzStart1, j2 = nzStart2; j1 <= i1; j1++, j2++)
            {
                int r = s1[j1].CompareTo(s2[j2]);
                if (r != 0) return r;
            }
            // the nz parts are equal
            int length1 = end1 - start1;
            int length2 = end2 - start2;
            if (length1 == length2) return 0;
            if (length1 > length2) return -1;
            return 1;
        }

        //lookahead
        private static void ScanNumEnd(string s, int start, ref int end, ref int nzStart)
        {
            nzStart = start;
            end = start;
            bool countZeros = true;
            while (Char.IsDigit(s, end))
            {
                if (countZeros && s[end].Equals('0'))
                {
                    nzStart++;
                }
                else countZeros = false;
                end++;
                if (end >= s.Length) break;
            }
        }

    }
}

//  Copyright (c) 2008-2011, www.metabolt.net
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

namespace METAbolt
{
    class ArrayX
    {
        /// <summary>
        /// Appends a list of elements to the end of an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="items"></param>
        public static void Append<T>(ref T[] array, params T[] items)
        {
            int oldLength = array.Length;
            //make room for new items
            Array.Resize<T>(ref array, oldLength + items.Length);

            for (int i = 0; i < items.Length; i++)
                array[oldLength + i] = items[i];
        }

        /// <summary>
        /// Remove an Array at a specific Location
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">index to remove at</param>
        /// <param name="list">The Array Object</param>
        public static void RemoveAt<T>(int index, ref T[] list)
        {
            //pre:
            if (index < 0 || list == null | list.Length == 0) return;

            //move everything from the index on to the left one then remove last empty
            if (list.Length > index + 1)
                for (int i = index + 1; i < list.Length; i++)
                    list[i - 1] = list[i];

            Array.Resize<T>(ref list, list.Length - 1);
        }

        /// 
        ///<summary> Thanks to homer.
        /// Remove all elements in an array satisfying a predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The Array Object</param>
        /// <param name="condition">A Predicate when the element shall get removed under.
        /// </param>
        /// <returns>Number of elements removed</returns>
        public static int RemoveAll<T>(ref T[] list, Predicate<T> condition)
        {
            if (null == condition || null == list || 0 == list.Length) return 0;

            int length = list.Length;
            int destinationIndex = 0;
            T[] destinationArray = new T[length];

            for (int i = 0; i < list.Length; i++)
            {
                if (!condition(list[i]))
                {
                    destinationArray[destinationIndex] = list[i];
                    destinationIndex++;
                }
            }

            if (destinationIndex != length)
            {
                Array.Resize<T>(ref destinationArray, destinationIndex);
                list = destinationArray;
            }

            return length - destinationIndex;
        }
    }
}
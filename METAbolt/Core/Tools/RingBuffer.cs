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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace METAbolt
{
    public class RingBufferProtection
    {
        private METAboltInstance instance;
        public int ringbuffmax = 20;
        public List<DateTime> ringbuffer = new List<DateTime>();

        public bool RingBuffer(METAboltInstance instance)
        {
            if (ringbuffmax == 0) return false;

            this.instance = instance;

            if (ringbuffer.Count > 0)
            {
                DateTime ltry = ringbuffer[0];

                TimeSpan tspn = DateTime.Now - ltry;

                if (tspn.TotalSeconds < 1.1)
                {
                    if (ringbuffer.Count == ringbuffmax)
                    {
                        instance.BlockChatIn = true;
                        return true;
                    }
                }
                else
                {
                    try
                    {
                        if (tspn.TotalSeconds < 2.1)
                        {
                            ringbuffer.RemoveAt(ringbuffer.Count() - 1);
                        }
                        else
                        {
                            ringbuffer.Clear();
                        }
                    }
                    catch { ; }
                }
            }

            ringbuffer.Add(DateTime.Now);

            SortDescending(ringbuffer);

            instance.BlockChatIn = false;
            return false;
        }

        public void SetBuffer(int bfr)
        {
            ringbuffmax = bfr;
        }

        private static List<DateTime> SortDescending(List<DateTime> list)
        {
            list.Sort((a, b) => b.CompareTo(a));
            return list;
        }
    }
}

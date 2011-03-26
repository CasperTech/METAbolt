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
using System.Drawing;
using System.Text;
using OpenMetaverse;

namespace METAbolt
{
    public class ImageCache
    {
        private SafeDictionary<UUID, System.Drawing.Image> cache = new SafeDictionary<UUID, System.Drawing.Image>();

        public ImageCache()
        {

        }

        public bool ContainsImage(UUID imageID)
        {
            return cache.ContainsKey(imageID);
        }

        public void AddImage(UUID imageID, System.Drawing.Image image)
        {
            try
            {
                if (!ContainsImage(imageID))
                    cache.Add(imageID, image);
            }
            catch (Exception ex)
            {
                Logger.Log("Image cache: " + ex.Message, Helpers.LogLevel.Error);    
            }
        }

        public void RemoveImage(UUID imageID)
        {
            try
            {
                if (ContainsImage(imageID))
                    cache.Remove(imageID);
            }
            catch (Exception ex)
            {
                Logger.Log("Image cache: " + ex.Message, Helpers.LogLevel.Error);    
            }
        }

        public System.Drawing.Image GetImage(UUID imageID)
        {
            return cache[imageID];
        }
    }
}

//  Copyright (c) 2008 - 2009, www.metabolt.net
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
using System.Text;
using OpenMetaverse;

namespace SLNetworkComm
{
    public class LoadURLEventArgs : EventArgs
    {
        private string objectName;
        private UUID objectID;
        private UUID ownerID;
        private bool ownerIsGroup;
        private string message;
        private string URL;

        public LoadURLEventArgs(string objectName, UUID objectID, UUID ownerID, bool ownerIsGroup, string message, string URL)
        {
            this.objectName = objectName;
            this.objectID = objectID;
            this.ownerID = ownerID;
            this.ownerIsGroup = ownerIsGroup;
            this.message = message;
            this.URL = URL;
        }

        public string ObjectName
        {
            get { return objectName; }
        }

        public UUID ObjectID
        {
            get { return objectID; }
        }

        public UUID OwnerID
        {
            get { return ownerID; }
        }

        public bool OwnerIsGroup
        {
            get { return ownerIsGroup; }
        }

        public string Message
        {
            get { return message; }
        }

        public string url
        {
            get { return URL; }
        }
    }
}

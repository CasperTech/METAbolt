//  Copyright (c) 2008 - 2010, www.metabolt.net (METAbolt)
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
using OpenMetaverse;

namespace SLNetworkComm
{
    public class ChatEventArgs : EventArgs
    {
        private string message;
        private ChatAudibleLevel audible;
        private ChatType type;
        private ChatSourceType sourceType;
        private string fromName;
        private UUID id;
        private UUID ownerid;
        private Vector3 position;

        public ChatEventArgs(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourceType, string fromName, UUID id, UUID ownerid, Vector3 position)
        {
            this.message = message;
            this.audible = audible;
            this.type = type;
            this.sourceType = sourceType;
            this.fromName = fromName;
            this.id = id;
            this.ownerid = ownerid;
            this.position = position;
        }

        public string Message
        {
            get { return message; }
        }

        public ChatAudibleLevel Audible
        {
            get { return audible; }
        }

        public ChatType Type
        {
            get { return type; }
        }

        public ChatSourceType SourceType
        {
            get { return sourceType; }
        }

        public string FromName
        {
            get { return fromName; }
        }

        public UUID ID
        {
            get { return id; }
        }

        public UUID OwnerID
        {
            get { return ownerid; }
        }

        public Vector3 Position
        {
            get { return position; }
        }
    }
}
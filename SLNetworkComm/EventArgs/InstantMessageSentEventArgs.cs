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
using System.Text;
using OpenMetaverse;

namespace SLNetworkComm
{
    public class InstantMessageSentEventArgs : EventArgs
    {
        private string message;
        private UUID targetID;
        private UUID sessionID;
        private DateTime timestamp;
        private Boolean groupIM;

        public InstantMessageSentEventArgs(string message, UUID targetID, UUID sessionID, DateTime timestamp)
        {
            this.message = message;
            this.targetID = targetID;
            this.sessionID = sessionID;
            this.timestamp = timestamp;
        }

        public InstantMessageSentEventArgs(string message, UUID targetID, UUID sessionID, DateTime timestamp, Boolean isGrp)
        {
            this.message = message;
            this.targetID = targetID;
            this.sessionID = sessionID;
            this.timestamp = timestamp;
            this.groupIM = isGrp; 
        }

        public Boolean GroupIM
        {
            get { return groupIM; }
        }

        public string Message
        {
            get { return message; }
        }

        public UUID TargetID
        {
            get { return targetID; }
        }

        public UUID SessionID
        {
            get { return sessionID; }
        }

        public DateTime Timestamp
        {
            get { return timestamp; }
        }
    }
}
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
using System.Collections.Generic;
using System.Text;
using OpenMetaverse;

namespace METAbolt
{
    public class ChatBufferItem
    {
        private DateTime timestamp;
        private string text;
        private ChatBufferTextStyle style;
        private string fromname;
        private UUID fromuuid;
        private string nlink;

        public ChatBufferItem()
        {
            // do nothing
        }

        public ChatBufferItem(DateTime timestamp, string text, ChatBufferTextStyle style)
        {
            this.timestamp = timestamp;
            this.text = text;
            this.style = style;
        }

        public ChatBufferItem(DateTime timestamp, string text, ChatBufferTextStyle style, string fromname)
        {
            this.timestamp = timestamp;
            this.text = text;
            this.style = style;
            this.fromname = fromname; 
        }

        public ChatBufferItem(DateTime timestamp, string text, string link, ChatBufferTextStyle style, string fromname)
        {
            this.timestamp = timestamp;
            this.text = text;
            this.nlink = link;
            this.style = style;
            this.fromname = fromname;
        }

        /// <summary>
        /// Constructor for an item in the ChatBuffer
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <param name="fromname"></param>
        /// <param name="fromuuid">UUID of the agent sending the message</param>
        public ChatBufferItem(DateTime timestamp, string text, ChatBufferTextStyle style, string name, UUID uuid)
        {
            this.timestamp = timestamp;
            this.text = text;
            this.style = style;
            this.fromname = name;
            this.fromuuid = uuid;
        }
        
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string Link
        {
            get { return nlink; }
            set { nlink = value; }
        }

        public string FromName
        {
            get { return fromname; }
            set { fromname = value; }
        }

        public ChatBufferTextStyle Style
        {
            get { return style; }
            set { style = value; }
        }

        /// <summary>
        /// UUID of the object sending the message, strangely this is not the FromAgentId but the IMSessionId in a message from object
        /// </summary>
        public UUID FromUUID
        {
            get { return fromuuid; }
            set { fromuuid = value; }
        }
    }

    public enum ChatBufferTextStyle
    {
        Normal,
        StatusBlue,
        StatusDarkBlue,
        LindenChat,
        ObjectChat,
        StartupTitle,
        Error,
        Alert,
        LoginReply
    }
}

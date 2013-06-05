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
using System.Text;
using METAbolt.FileINI;
using System.Globalization;

namespace METAbolt
{
    public class FileConfig
    {
        private INIFile ini;
        FileConfig config;

        private string filename = string.Empty;
        private Dictionary<string, Dictionary<string, string>> friendgroups = new Dictionary<string, Dictionary<string, string>>(); 

        public FileConfig(string filename)
        {
            this.filename = filename;
            ini = new INIFile(filename);
        }

        public FileConfig Load()
        {
            config = new FileConfig(filename);

            try
            {
                friendgroups = config.ini.m_Sections;
            }
            catch
            {
               ;
            }

            return config;
        }

        public void CreateGroup(string groupname)
        {
            ini.CreateSection(groupname);
            friendgroups = ini.m_Sections;
        }

        public void Save()
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> fr in ini.m_Sections)
            {
                string header = fr.Key.ToString(CultureInfo.CurrentCulture);
                Dictionary<string, string> rec = fr.Value;

                string uuid = string.Empty;
                string name = string.Empty;

                foreach (KeyValuePair<string, string> s in rec)
                {
                    uuid = s.Key.ToString(CultureInfo.CurrentCulture);
                    name = s.Value.ToString(CultureInfo.CurrentCulture);  
                }

                ini.SetValue(header, uuid, name);
            }

            ini.Flush(); 
        }

        public void AddFriendToGroup(string groupname, string friendname, string frienduuid)
        {
            ini.SetValue(groupname, frienduuid, friendname);
            friendgroups = ini.m_Sections;
        }

        public void removeFriendFromGroup(string groupname, string frienduuid)
        {
            ini.RemoveValue(groupname, frienduuid);
            friendgroups = ini.m_Sections;
        }

        public Dictionary<string, Dictionary<string, string>> FriendGroups
        {
            get { return friendgroups; }
            set { friendgroups = value; }
        }
    }
}

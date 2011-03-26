//  Copyright (c) 2008 - 2010, www.metabolt.net
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
using System.Net;
using OpenMetaverse;
using System.Windows.Forms;

namespace METAbolt
{
    class METAproxy
    {
        public void SetProxy(bool UseProxy, string proxy_url, string port, string username, string password)
        {
            if (!UseProxy)
            {
                DisableProxy();
                return;
            }

            if (proxy_url == string.Empty || proxy_url == null)
            {
                UseProxy = false;
                Logger.Log("Proxy Error: A proxy URI has not been specified", Helpers.LogLevel.Warning);   
            }

            if (UseProxy)
            {
                string purl = proxy_url.Trim();

                if (!purl.StartsWith("http://"))
                {
                    purl = @"http://" + purl; 
                }

                try
                {
                    if (port.Length > 1)
                    {
                        purl = purl + ":" + port.Trim() + @"/";
                    }

                    WebProxy proxy = new WebProxy(purl,true);
                    proxy.Credentials = new NetworkCredential(username.Trim(), password.Trim());
                    WebRequest.DefaultWebProxy = proxy;
                }
                catch (Exception ex)
                {
                    Logger.Log("Proxy: " + ex.Message, Helpers.LogLevel.Error);
                    MessageBox.Show(ex.Message); 
                }
            }
            else
            {
                try
                {
                    DisableProxy();
                }
                catch (Exception ex)
                {
                    Logger.Log("Proxy: " + ex.Message, Helpers.LogLevel.Error);
                    MessageBox.Show(ex.Message); 
                }
            }
        }

        private void DisableProxy()
        {
            IWebProxy proxy = new WebProxy();
            proxy.Credentials = CredentialCache.DefaultNetworkCredentials;   // null;
            WebRequest.DefaultWebProxy = proxy;
            return;
        }
    }
}

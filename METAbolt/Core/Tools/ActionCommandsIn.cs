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
using SLNetworkComm;
using OpenMetaverse;
using System.Security.Cryptography;
using MD5library;
using System.Threading;


namespace METAbolt
{
    public class ActionCommandsIn
    {
        private METAboltInstance instance;
        private SLNetCom netcom;
        private GridClient client;
        ManualResetEvent CurrentlyWornEvent = new ManualResetEvent(false);

        public ActionCommandsIn(METAboltInstance instance)
        {
            this.instance = instance;
            client = this.instance.Client;
            netcom = this.instance.Netcom;
        }

        public string ProcessCommand(string command)
        {
            char[] deli = "|".ToCharArray();
            string[] sGrp = command.Split(deli);

            // Check password first and exit if not valid
            string pwd = sGrp[1].Trim();

            if (pwd != GetMD5())
                return string.Empty;

            string cmdtype = sGrp[2].Trim();
            string msg = string.Empty;

            switch (cmdtype)
            {
                case "WEAR":
                    // Format: cmd identifier|password|command type|folder UUID
                    UUID folder = (UUID)sGrp[3].Trim();

                    if (folder == UUID.Zero)
                        return string.Empty;

                    try
                    {
                        List<InventoryBase> contents = client.Inventory.FolderContents(folder, client.Self.AgentID, true, true, InventorySortOrder.ByName, 20 * 1000);
                        List<InventoryItem> items = new List<InventoryItem>();

                        if (contents == null)
                        {
                            Logger.Log("IM Command: Failed to get contents of '" + folder.ToString() + "'", Helpers.LogLevel.Warning);
                            return "Could not get folder contents";
                        }

                        foreach (InventoryBase iitem in contents)
                        {
                            if (iitem is InventoryItem)
                                items.Add((InventoryItem)iitem);
                        }

                        client.Appearance.ReplaceOutfit(items);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("IM WEAR COMMAND: " + ex.Message, Helpers.LogLevel.Error);
                        return string.Empty;
                    }

                    //msg = "Wearing folder: " + folder.ToString();
                    //client.Self.Chat(msg, 0, ChatType.Whisper);     

                    break;

                case "AWAY":
                    instance.State.SetAway(true);
                    break;

                case "NOTAWAY":
                    instance.State.SetAway(false);
                    break;

                case "BUSY":
                    instance.State.SetBusy(true);
                    break;

                case "NOTBUSY":
                    instance.State.SetBusy(false);
                    break;

                case "TP":
                    // Format: cmd identifier|password|command type|SIM|coord x|coord y|coord z
                    string sim = sGrp[3].Trim();
                    float x = float.Parse(sGrp[4].Trim());
                    float y = float.Parse(sGrp[5].Trim());
                    float z = float.Parse(sGrp[6].Trim());

                    if (sim != string.Empty)
                    {
                        netcom.Teleport(sim, new Vector3(x, y, z));
                    }
                    else
                    {
                        Logger.Log("TP COMMAND: SIM name can't be empty. Command has been ignored", Helpers.LogLevel.Info);
                    }
                    break;

                case "SAY":
                    // Format: cmd identifier|password|command type|channel|message|message type
                    int channel = Convert.ToInt32(sGrp[3].Trim());
                    string msgtosay = sGrp[4].Trim();
                    string ctypein = sGrp[5].Trim().ToLower();
                    ChatType ctype = ChatType.Normal;

                    switch (ctypein)
                    {
                        case "normal":
                            ctype = ChatType.Normal;
                            break;
 
                        case "ownersay":
                            ctype = ChatType.OwnerSay;
                            break;

                        case "regionsay":
                            ctype = ChatType.RegionSay;
                            break;

                        case "shout":
                            ctype = ChatType.Shout;
                            break;

                        case "whisper":
                            ctype = ChatType.Whisper;
                            break;

                        default:
                            ctype = ChatType.Normal;
                            break; 
                    }

                    netcom.ChatOut(msgtosay, ctype, channel);
                    break;

                case "GIVE":
                    // Format: cmd identifier|password|command type|item UUID|avatar UUID
                    UUID imitem = (UUID)sGrp[3].Trim();
                    UUID avatar = (UUID)sGrp[4].Trim();

                    if (imitem == UUID.Zero || avatar == UUID.Zero)
                        return string.Empty;

                    // Find the item in inventory
                    InventoryItem item = client.Inventory.FetchItem(imitem, client.Self.AgentID, 15000);

                    if (item == null)
                    {
                        Logger.Log("IM GIVE COMMAND: Item " + imitem.ToString() + " not found in inventory", Helpers.LogLevel.Error);
                        return string.Empty;
                    }

                    try
                    {
                        if ((item.Permissions.OwnerMask & PermissionMask.Transfer) == PermissionMask.Transfer)
                        {
                            if ((item.Permissions.OwnerMask & PermissionMask.Copy) != PermissionMask.Copy)
                            {
                                client.Inventory.GiveItem(item.UUID, item.Name, item.AssetType, avatar, false);
                                client.Inventory.Store.RemoveNodeFor(item);
                            }
                            else
                            {
                                client.Inventory.GiveItem(item.UUID, item.Name, item.AssetType, avatar, false);
                                instance.TabConsole.DisplayChatScreen("Gave inventory item  " + item.Name + " (" + imitem + ") to " + avatar + " via ActionCommand received");  
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("IM GIVE COMMAND: " + ex.Message, Helpers.LogLevel.Error);
                        return string.Empty;
                    }

                    break;

                case "GIVEFOLDER":
                    // Format: cmd identifier|password|command type|folder UUID|folder name|avatar UUID
                    UUID imfolder = (UUID)sGrp[3].Trim();
                    string fname = sGrp[4].Trim();
                    UUID imavatar = (UUID)sGrp[5].Trim();

                    if (imfolder == UUID.Zero || imavatar == UUID.Zero)
                        return string.Empty;

                    try
                    {
                        client.Inventory.GiveFolder(imfolder, fname, AssetType.Folder, imavatar, true);
                        instance.TabConsole.DisplayChatScreen("Gave inventory folder " + fname + " (" + imfolder + ") to " + imavatar + " via ActionCommand received");  
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("ERROR IM GIVEFOLDER COMMAND: " + ex.Message, Helpers.LogLevel.Error);
                    }

                    break;
            }

            return msg;
        }

        private string GetMD5()
        {
            string str = this.instance.Config.CurrentConfig.GroupManPro;
            METAMD5 md5 = new METAMD5();
            return md5.MD5(str);
        }
    }
}
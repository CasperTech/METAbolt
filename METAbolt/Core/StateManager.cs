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
using System.Timers;
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Packets;
using SLNetworkComm;

namespace METAbolt
{
    public class StateManager
    {
        private METAboltInstance instance;
        private GridClient client;
        private SLNetCom netcom;

        private bool typing = false;
        private bool away = false;
        private bool busy = false;
        private bool flying = false;
        private bool alwaysrun = false;
        private bool sitting = false;
        private bool belly = false;
        private bool club = false;
        private bool salsa = false;
        private bool fall = false;
        private bool crouch = false;
        
        private bool pointing = false;
        private UUID pointID = UUID.Zero;
        private UUID beamID = UUID.Zero;
        private UUID beamID1 = UUID.Zero;
        private UUID beamID2 = UUID.Zero;
        private UUID effectID = UUID.Zero;
        private bool looking = false;
        private UUID lookID = UUID.Zero;

        private bool following = false;
        private string followName = string.Empty;
        private float followDistance = 5.0f;
        private SafeDictionary<UUID, String> groupstore = new SafeDictionary<UUID, String>();
        private Dictionary<UUID, Group> groups = new Dictionary<UUID, Group>();
        //private Dictionary<UUID, FriendInfo> avatarfriends = new Dictionary<UUID,FriendInfo>();  
        private List<FriendInfo> avatarfriends;
        private string currenttab = "Chat";
        private int ccntr = 1;

        //private Timer agentUpdateTicker;
        private UUID awayAnimationID = new UUID("fd037134-85d4-f241-72c6-4f42164fedee");
        private UUID busyAnimationID = new UUID("efcf670c2d188128973a034ebc806b67");
        private UUID typingAnimationID = new UUID("c541c47f-e0c0-058b-ad1a-d6ae3a4584d9");
        private UUID bellydanceAnimationID = new UUID("f2c2f006-69a2-089d-64bc-94efe4f3bb23");
        private UUID clubdanceAnimationID = new UUID("cb956f10-cc64-71a3-a36c-61823794f7df");
        private UUID salsaAnimationID = new UUID("6953622f-b308-3c84-4c28-a0cb9d5f9749");
        private UUID fallAnimationID = new UUID("85db9c46-2c49-d4d0-d7eb-b5d954d8d8a3");
        private UUID crouchAnimationID = new UUID(Animations.CROUCH.ToString());
        //private Primitive sitprim = null;
        private UUID sitprim = UUID.Zero;
        private ManualResetEvent PrimEvent = new ManualResetEvent(false);
        private bool groundsitting = false;
        private System.Timers.Timer pointtimer;
        private Vector3d offset = Vector3d.Zero; 
        private Vector3d beamoffset1 = new Vector3d(0, 0, 0.1);
        private Vector3d beamoffset2 = new Vector3d(0, 0.1, 0);
        private Primitive prim = null;
        private Color4 mncolour = new Color4(255, 0, 0, 0);   //new Color4(0, 255, 12, 0); // Green
        private Color4 spcolour = new Color4(0, 0, 255, 0);
        private Color4 tdcolour = new Color4(0, 255, 12, 0);
        private Color4 bkcolour = new Color4(255, 255, 255, 255);
        private Color4 ccolur = new Color4(0, 0, 255, 255);
        private UUID lookattarget = UUID.Zero;  
 

        public StateManager(METAboltInstance instance)
        {
            this.instance = instance;
            netcom = this.instance.Netcom;
            client = this.instance.Client;

            AddNetcomEvents();
            AddClientEvents();
            //InitializeAgentUpdateTimer();

            pointtimer = new System.Timers.Timer();
            pointtimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            // Set the Interval to 10 seconds.
            pointtimer.Interval = 1000;
            pointtimer.Enabled = false;
        }

        private void AddNetcomEvents()
        {
            netcom.ClientLoggedOut += new EventHandler(netcom_ClientLoggedOut);
            netcom.ClientDisconnected += new EventHandler<DisconnectedEventArgs>(netcom_ClientDisconnected);
        }

        private void RemoveNetcomEvents()
        {
            netcom.ClientLoggedOut -= new EventHandler(netcom_ClientLoggedOut);
            netcom.ClientDisconnected -= new EventHandler<DisconnectedEventArgs>(netcom_ClientDisconnected);
        }

        private void netcom_ClientLoggedOut(object sender, EventArgs e)
        {
            TidyUp();            
        }

        private void netcom_ClientDisconnected(object sender, DisconnectedEventArgs e)
        {
            TidyUp();
        }

        private void TidyUp()
        {
            typing = away = busy = false;
            pointtimer.Dispose();
            pointtimer = null;

            RemoveClientEvents();
            RemoveNetcomEvents();
        }

        private void AddClientEvents()
        {
            client.Objects.TerseObjectUpdate += new EventHandler<TerseObjectUpdateEventArgs>(Objects_OnObjectUpdated);
        }

        private void RemoveClientEvents()
        {
            client.Objects.TerseObjectUpdate -= new EventHandler<TerseObjectUpdateEventArgs>(Objects_OnObjectUpdated);
        }

        private void Objects_OnObjectUpdated(object sender, TerseObjectUpdateEventArgs e)
        { 
            if (!e.Update.Avatar) return;
            if (!following) return;

            Avatar av;
            client.Network.CurrentSim.ObjectsAvatars.TryGetValue(e.Update.LocalID, out av);

            if (av == null) return;

            if (av.Name == followName)
            {
                Vector3 pos;

                pos = av.Position;

                if (av.ParentID != 0)
                {
                    // the avatar sat
                    Vector3 avpos = av.Position;
                    uint oID = av.ParentID;

                    if (prim == null)
                    {
                        client.Network.CurrentSim.ObjectsPrimitives.TryGetValue(oID, out prim);
                    }

                    if (prim == null) return;

                    avpos = prim.Position + avpos;

                    pos = avpos;
                }
                else
                {
                    prim = null;
                }

                client.Self.Movement.TurnToward(pos);

                if (prim != null) return;


                if (Vector3.Distance(pos, instance.SIMsittingPos()) > followDistance)
                {
                    client.Self.AutoPilotCancel();
                    ulong followRegionX = e.Simulator.Handle >> 32;
                    ulong followRegionY = e.Simulator.Handle & (ulong)0xFFFFFFFF;
                    ulong xTarget = (ulong)pos.X + followRegionX;
                    ulong yTarget = (ulong)pos.Y + followRegionY;
                    float zTarget = pos.Z - 1f;

                    client.Self.AutoPilot(xTarget, yTarget, zTarget);
                }
                else
                {
                    client.Self.AutoPilotCancel();
                    client.Self.Movement.TurnToward(pos);
                }
            }
        }

        private void InitializeAgentUpdateTimer()
        {
            //agentUpdateTicker = new Timer(60000);
            //agentUpdateTicker.Elapsed += new ElapsedEventHandler(agentUpdateTicker_Elapsed);
        }

        private void agentUpdateTicker_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            //AgentUpdatePacket update = new AgentUpdatePacket();
            //update.Header.Reliable = true;

            //update.AgentData.AgentID = client.Self.AgentID;
            //update.AgentData.SessionID = client.Self.SessionID;
            //update.AgentData.HeadRotation = Quaternion.Identity;
            //update.AgentData.BodyRotation = Quaternion.Identity;
            //client.Network.SendPacket(update, client.Network.CurrentSim);
        }

        public void Follow(string name)
        {
            followName = name;
            following = !string.IsNullOrEmpty(followName);
        }

        public void SetTyping(bool typing)
        {
            Dictionary<UUID, bool> typingAnim = new Dictionary<UUID, bool>();
            typingAnim.Add(typingAnimationID, typing);

            if (!instance.Config.CurrentConfig.DisableTyping)
            {
                client.Self.Animate(typingAnim, false);
            }

            if (typing)
                client.Self.Chat(string.Empty, 0, ChatType.StartTyping);
            else
                client.Self.Chat(string.Empty, 0, ChatType.StopTyping);

            this.typing = typing;
        }

        public void SetAway(bool away)
        {
            Dictionary<UUID, bool> awayAnim = new Dictionary<UUID, bool>();
            awayAnim.Add(awayAnimationID, away);

            client.Self.Animate(awayAnim, true);
            this.away = away;
        }

        public void SetBusy(bool busy)
        {
            Dictionary<UUID, bool> busyAnim = new Dictionary<UUID, bool>();
            busyAnim.Add(busyAnimationID, busy);

            client.Self.Animate(busyAnim, true);
            this.busy = busy;
        }

        public void BellyDance(bool belly)
        {
            Dictionary<UUID, bool> bdanceAnim = new Dictionary<UUID, bool>();
            bdanceAnim.Add(bellydanceAnimationID, belly);

            client.Self.Animate(bdanceAnim, true);
            this.belly = belly;
        }

        public void ClubDance(bool club)
        {
            Dictionary<UUID, bool> cdanceAnim = new Dictionary<UUID, bool>();
            cdanceAnim.Add(clubdanceAnimationID, club);

            client.Self.Animate(cdanceAnim, true);
            this.club = club;
        }

        public void SalsaDance(bool salsa)
        {
            Dictionary<UUID, bool> sdanceAnim = new Dictionary<UUID, bool>();
            sdanceAnim.Add(salsaAnimationID, salsa);

            client.Self.Animate(sdanceAnim, true);
            this.salsa = salsa;
        }

        public void FallOnFace(bool fall)
        {
            Dictionary<UUID, bool> ffallAnim = new Dictionary<UUID, bool>();
            ffallAnim.Add(fallAnimationID, fall);

            client.Self.Animate(ffallAnim, true);
            this.fall = fall;
        }

        public void Crouch(bool crouch)
        {
            Dictionary<UUID, bool> crouchAnim = new Dictionary<UUID, bool>();
            crouchAnim.Add(crouchAnimationID, crouch);

            client.Self.Animate(crouchAnim, true);
            this.crouch = crouch;
        }

        public void SetFlying(bool flying)
        {
            client.Self.Fly(flying);
            this.flying = flying;
        }

        public void SetAlwaysRun(bool alwaysrun)
        {
            client.Self.Movement.AlwaysRun = alwaysrun;
            this.alwaysrun = alwaysrun;
        }

        public void SetSitting(bool sitting, UUID target)
        {
            if (sitting)
            {
                this.sitting = false;
                sitprim = UUID.Zero;
                client.Self.RequestSit(target, Vector3.Zero);
                client.Self.Sit();
            }
            else
            {
                this.sitting = false;
                client.Self.Stand();
                sitprim = UUID.Zero;

                StopAnimations();
            }
        }

        public void SetGroundSit(bool sit)
        {
            groundsitting = sit;
        }

        public void SetStanding()
        {
            client.Self.Stand();
            this.sitting = false;
            this.groundsitting = false;
            sitprim = UUID.Zero;

            StopAnimations();
        }

        public void SetPointing(bool pointing, UUID target)
        {
            this.pointing = pointing;

            if (pointing)
            {
                pointID = UUID.Random();
                beamID = UUID.Random();

                client.Self.SphereEffect(offset, ccolur, 1.1f, effectID);

                client.Self.PointAtEffect(client.Self.AgentID, target, Vector3d.Zero, PointAtType.Select, pointID);
                client.Self.BeamEffect(client.Self.AgentID, target, Vector3d.Zero, mncolour, 1.0f, beamID);
            }
            else
            {
                if (pointID == UUID.Zero || beamID == UUID.Zero) return;

                //client.Self.PointAtEffect(client.Self.AgentID, target, Vector3d.Zero, PointAtType.Clear, pointID);
                client.Self.PointAtEffect(client.Self.AgentID, UUID.Zero, Vector3d.Zero, PointAtType.None, pointID);
                client.Self.BeamEffect(client.Self.AgentID, target, Vector3d.Zero, bkcolour, 0, beamID);
                pointID = UUID.Zero;
                beamID = UUID.Zero;
            }
        }

        public void SetPointing(bool pointing, UUID target, Vector3d offset, Vector3 primposition)
        {
            this.pointing = pointing;

            if (pointing)
            {
                this.offset = offset;
 
                pointID = UUID.Random();
                beamID = UUID.Random();
                effectID = UUID.Random();

                client.Self.SphereEffect(offset, ccolur, 0.80f, effectID);

                client.Self.PointAtEffect(client.Self.AgentID, target, Vector3d.Zero, PointAtType.Select, pointID);

                beamID1 = UUID.Random();                
                beamID2 = UUID.Random();

                client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset, mncolour, 1.0f, beamID);
                client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset + beamoffset1, spcolour, 1.0f, beamID1);
                client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset + beamoffset2, tdcolour, 1.0f, beamID2);

                pointtimer.Enabled = true;
                pointtimer.Start();
            }
            else
            {
                if (pointID == UUID.Zero || beamID == UUID.Zero) return;

                pointtimer.Stop();
                pointtimer.Enabled = false;

                client.Self.PointAtEffect(client.Self.AgentID, UUID.Zero, Vector3d.Zero, PointAtType.None, pointID);
                client.Self.BeamEffect(UUID.Zero, UUID.Zero, Vector3d.Zero, bkcolour, 0.0f, beamID);
                client.Self.SphereEffect(Vector3d.Zero, bkcolour, 0.0f, effectID);

                pointID = UUID.Zero;
                beamID = UUID.Zero;
                effectID = UUID.Zero;

                client.Self.BeamEffect(UUID.Zero, UUID.Zero, Vector3d.Zero, bkcolour, 0.0f, beamID1);
                client.Self.BeamEffect(UUID.Zero, UUID.Zero, Vector3d.Zero, bkcolour, 0.0f, beamID2);

                beamID1 = UUID.Zero;
                beamID2 = UUID.Zero;
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            ccntr += 1;

            if (ccntr > 3) ccntr = 1;

            switch (ccntr)
            {
                case 1:
                    client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset, mncolour, 1.0f, beamID);
                    client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset + beamoffset1, spcolour, 1.0f, beamID1);
                    client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset + beamoffset2, tdcolour, 1.0f, beamID2);
                    break;

                case 2:
                    client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset, spcolour, 1.0f, beamID);
                    client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset + beamoffset1, tdcolour, 1.0f, beamID1);
                    client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset + beamoffset2, mncolour, 1.0f, beamID2);
                    break;

                case 3:
                    client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset, tdcolour, 1.0f, beamID);
                    client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset + beamoffset1, mncolour, 1.0f, beamID1);
                    client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset + beamoffset2, spcolour, 1.0f, beamID2);
                    break;
            }

            
        }

        public void SetPointingTouch(bool pointing, UUID target, Vector3d offset, Vector3 primposition)
        {
            this.pointing = pointing;

            if (pointing)
            {
                this.offset = offset;

                pointID = UUID.Random();
                beamID = UUID.Random();

                client.Self.PointAtEffect(client.Self.AgentID, target, Vector3d.Zero, PointAtType.Select, pointID);

                beamID1 = UUID.Random();
                beamID2 = UUID.Random();

                client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset, mncolour, 1.0f, beamID);
                client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset + beamoffset1, spcolour, 1.0f, beamID1);
                client.Self.BeamEffect(client.Self.AgentID, UUID.Zero, offset + beamoffset2, spcolour, 1.0f, beamID2);

                pointtimer.Start();
            }
            else
            {
                if (pointID == UUID.Zero || beamID == UUID.Zero) return;

                pointtimer.Stop();

                client.Self.PointAtEffect(client.Self.AgentID, UUID.Zero, Vector3d.Zero, PointAtType.None, pointID);
                client.Self.BeamEffect(UUID.Zero, UUID.Zero, Vector3d.Zero, bkcolour, 0.0f, beamID);

                pointID = UUID.Zero;
                beamID = UUID.Zero;

                client.Self.BeamEffect(UUID.Zero, UUID.Zero, Vector3d.Zero, bkcolour, 0.0f, beamID1);
                client.Self.BeamEffect(UUID.Zero, UUID.Zero, Vector3d.Zero, bkcolour, 0.0f, beamID2);

                beamID1 = UUID.Zero;
                beamID2 = UUID.Zero;
            }
        }

        public void LookAt(bool looking, UUID target)
        {
            if (instance.Config.CurrentConfig.DisableLookAt)
                return;

            this.looking = looking;

            if (looking)
            {
                if (lookID == UUID.Zero)
                {
                    lookID = UUID.Random();
                }

                client.Self.LookAtEffect(client.Self.AgentID, target, Vector3d.Zero, LookAtType.Idle, lookID);
                //lookattarget = target;
            }
            else
            {
                //if (lookID == UUID.Zero) return;

                Vector3d lkpos = new Vector3d(new Vector3(3, 0, 0) * Quaternion.Identity);
                client.Self.LookAtEffect(client.Self.AgentID, client.Self.AgentID, lkpos, LookAtType.Idle, lookID);
                //lookID = UUID.Zero;
            }
        }

        public void LookAtObject(bool looking, UUID target)
        {
            if (instance.Config.CurrentConfig.DisableLookAt)
                return;

            this.looking = looking;

            if (looking)
            {
                if (lookID == UUID.Zero)
                {
                    lookID = UUID.Random();
                }

                client.Self.LookAtEffect(client.Self.AgentID, target, Vector3d.Zero, LookAtType.Select, lookID);
                lookattarget = target;
            }
            else
            {
                //if (lookID == UUID.Zero) return;

                Vector3d lkpos = new Vector3d(new Vector3(2, 0, 0) * Quaternion.Identity);
                client.Self.LookAtEffect(client.Self.AgentID, client.Self.AgentID, lkpos, LookAtType.Idle, lookID);
                //lookID = UUID.Zero;
            }
        }

        //private void LookIdle(float dist)
        //{
        //    Vector3d lkpos = new Vector3d(new Vector3(dist, 0, 0) * Quaternion.Identity);
        //    lookID = UUID.Random();

        //    client.Self.LookAtEffect(client.Self.AgentID, client.Self.AgentID, lkpos, LookAtType.Idle, lookID);
        //}

        public DateTime GetTimeStamp(DateTime dte)
        {
            //DateTime dte = item.Timestamp;

            if (instance.Config.CurrentConfig.UseSLT)
            {
                string _timeZoneId = "Pacific Standard Time";
                DateTime startTime = DateTime.UtcNow;
                TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
                dte = TimeZoneInfo.ConvertTime(startTime, TimeZoneInfo.Utc, tst);
            }

            return dte;
        }

        public void StopAnimations()
        {
            client.Self.SignaledAnimations.ForEach((UUID anims) =>
            {
                client.Self.AnimationStop(anims, true);
            });
        }

        public UUID TypingAnimationID
        {
            get { return typingAnimationID; }
            set { typingAnimationID = value; }
        }
        
        public UUID AwayAnimationID
        {
            get { return awayAnimationID; }
            set { awayAnimationID = value; }
        }

        public UUID BusyAnimationID
        {
            get { return busyAnimationID; }
            set { busyAnimationID = value; }
        }

        public UUID BellydanceAnimationID
        {
            get { return bellydanceAnimationID; }
            set { bellydanceAnimationID = value; }
        }

        public UUID ClubdanceAnimationID
        {
            get { return clubdanceAnimationID; }
            set { clubdanceAnimationID = value; }
        }

        public UUID SalsaAnimationID
        {
            get { return salsaAnimationID; }
            set { salsaAnimationID = value; }
        }

        public UUID FallAnimationID
        {
            get { return fallAnimationID; }
            set { fallAnimationID = value; }
        }

        public UUID CrouchAnimationID
        {
            get { return crouchAnimationID; }
            set { crouchAnimationID = value; }
        }

        public bool IsTyping
        {
            get { return typing; }
        }

        public bool IsAway
        {
            get { return away; }
        }

        public bool IsBusy
        {
            get { return busy; }
        }

        public bool IsBelly
        {
            get { return belly; }
        }

        public bool IsFlying
        {
            get { return flying; }
        }

        public bool IsSitting
        {
            get { return sitting; }
            set { sitting = value; }
        }

        public bool IsSittingOnGround
        {
            get { return groundsitting; }
        }

        public bool IsPointing
        {
            get { return pointing; }
        }

        public bool IsFollowing
        {
            get { return following; }
        }

        public string FollowName
        {
            get { return followName; }
            set { followName = value; }
        }

        public float FollowDistance
        {
            get { return followDistance; }
            set { followDistance = value; }
        }

        public SafeDictionary<UUID, String> GroupStore
        {
            get { return groupstore; }
            set { groupstore = value; }
        }

        public Dictionary<UUID, Group> Groups
        {
            get { return groups; }
            set { groups = value; }
        }

        public List<FriendInfo> AvatarFriends
        {
            get { return avatarfriends; }
            set { avatarfriends = value; }
        }

        public string CurrentTab
        {
            get { return currenttab; }
            set { currenttab = value; }
        }

        public UUID SitPrim
        {
            get { return sitprim; }
            set { sitprim = value; } 
        }
    }
}

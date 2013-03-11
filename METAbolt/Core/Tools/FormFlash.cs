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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

public static class FormFlash
{
    [DllImport("user32.dll")]
    public static extern int FlashWindowEx(ref FLASHWINFO pfwi);

    /// <summary>
    /// Flashes the form's taskbar button.
    /// </summary>
    /// <param name="form"></param>
    public static void Flash(Form form)
    {
        FLASHWINFO fw = new FLASHWINFO()
        {
            cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(FLASHWINFO))),
            hwnd = form.Handle,
            dwFlags = (Int32)(FLASHWINFOFLAGS.FLASHW_ALL | FLASHWINFOFLAGS.FLASHW_TIMERNOFG),
            uCount = 3,
            dwTimeout = 0
        };

        FlashWindowEx(ref fw);
        
    }

    /// <summary>
    /// Stops flashing the form's taskbar button.
    /// </summary>
    /// <param name="form"></param>
    public static void Unflash(Form form)
    {
        FLASHWINFO fw = new FLASHWINFO()
        {
            cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(FLASHWINFO))),
            hwnd = form.Handle,
            dwFlags = (Int32)(FLASHWINFOFLAGS.FLASHW_STOP),
            uCount = 0,
            dwTimeout = 0
        };

        FlashWindowEx(ref fw);
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct FLASHWINFO
{
    public UInt32 cbSize;
    public IntPtr hwnd;
    public Int32 dwFlags;
    public UInt32 uCount;
    public Int32 dwTimeout;
}

public enum FLASHWINFOFLAGS
{
    FLASHW_STOP      = 0,
    FLASHW_CAPTION   = 0x00000001,
    FLASHW_TRAY      = 0x00000002,
    FLASHW_ALL       = (FLASHW_CAPTION | FLASHW_TRAY),
    FLASHW_TIMER     = 0x00000004,
    FLASHW_TIMERNOFG = 0x0000000C
}

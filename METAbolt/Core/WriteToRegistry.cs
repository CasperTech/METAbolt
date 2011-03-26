using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace METAbolt
{
    public class WriteToRegistry
    {
        private RegistryKey baseRegistryKey = Registry.LocalMachine;
        private string subKey = "SOFTWARE\\METAbolt";

        public bool Write(string KeyName, object Value)
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.CreateSubKey(subKey);

                // Save the value
                sk1.SetValue(KeyName.ToUpper(), Value);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string Read(string KeyName)
        {
            string svalue = string.Empty;
  
            RegistryKey rk = baseRegistryKey;
            RegistryKey sk1 = rk.OpenSubKey(subKey);

            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    svalue = sk1.GetValue(KeyName.ToUpper()).ToString();
                    return svalue;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }	
    }
}

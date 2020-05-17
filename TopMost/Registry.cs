using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopMost2
{
    public static class Reg
    {
        const string AUTORUN_SUBKEY = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        const string APPCONFIG_SUBKEY = "Software\\TopMost2";

        public static bool IsAutoStart
        {
            get
            {
                string expected = '"' + API.GetExeLocation() + '"' + " --autostart";

                return API.GetValueFromRegistry(AUTORUN_SUBKEY, "TopMost2") == expected;
            }

            set
            {
                if (value)
                    API.SetValueToRegistry(AUTORUN_SUBKEY, "TopMost2", '"' + API.GetExeLocation() + '"' + " --autostart");
                else
                    API.DeleteValueFromRegistry(AUTORUN_SUBKEY, "TopMost2");
            }
        }

        public static bool IsShortcutEnable
        {
            get
            {
                return API.GetValueFromRegistry(APPCONFIG_SUBKEY, "ShortcutEnable") == "1";
            }
            set
            {
                API.SetValueToRegistry(APPCONFIG_SUBKEY, "ShortcutEnable", value ? "1" : "0");
            }
        }

        public static HashSet<Keys> ShortcutCombination
        {
            get
            {
                HashSet<Keys> rtn = new HashSet<Keys>();

                string raw = API.GetValueFromRegistry(APPCONFIG_SUBKEY, "ShortcutKeys");

                if (raw != null)
                {
                    string[] splitted = raw.Split(',');
                    foreach (string token in splitted)
                    {
                        rtn.Add((Keys)Convert.ToInt32(token));
                    }
                }

                if (rtn.Count == 0)
                {
                    // default
                    rtn.Add(Keys.LControlKey);
                    rtn.Add(Keys.LMenu);
                    rtn.Add(Keys.Space);
                }

                return rtn;
            }
            set
            {
                string write = "";

                foreach (Keys k in value)
                {
                    if (write != "") write += ",";
                    write += (int)k;
                }

                API.SetValueToRegistry(APPCONFIG_SUBKEY, "ShortcutKeys", write);
            }
        }
    }
}

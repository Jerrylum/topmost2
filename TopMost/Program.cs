using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

using TopMost2;
using Utilities;

namespace TopMost2
{
    static class Program
    {



        [STAThread]
        static void Main()
        {

            API.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            new OptionsForm(); // Dont use `Application.Run(...);`. We want to hide the form

            Application.Run();
        }

    }
}

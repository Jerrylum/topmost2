using System;
using System.Windows.Forms;

namespace TopMost2
{

    static class Program
    {
        public static OptionsForm OptionsForm;


        [STAThread]
        static void Main(string[] args)
        {

            API.Init();

            ArgsProcess.Handle(args);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            OptionsForm = new OptionsForm(); // Dont use `Application.Run(...);`. We want to hide the form


            Application.Run();
        }

    }
}

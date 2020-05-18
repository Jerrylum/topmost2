using System;

namespace TopMost2
{
    public static class ArgsProcess
    {
        private static string[] args;
        private static int idx = 0;
        private static bool alwaysOpenCheckingFlag = true;
        private static bool keepRunningFlag = false;
        private static bool commandModeFlag = false;

        private static void Eat()
        {
            string mode = args[idx++];

            if (mode == "--autostart")
            {
                alwaysOpenCheckingFlag = false;
                keepRunningFlag = true;
                return;
            }

            string value = args[idx++];

            if (mode == "/S" || mode == "-S" || mode == "--set" || mode == "/R" || mode == "-R" || mode == "--remove")
            {
                IntPtr hwnd = IntPtr.Zero;

                if (value.StartsWith("0x"))
                    hwnd = new IntPtr(Convert.ToInt64(value, 16));

                if (hwnd == IntPtr.Zero)
                    return; // Window not found

                if (mode == "/S" || mode == "-S" || mode == "--set")
                {
                    API.SetTopMost(hwnd, true);
                }
                else if (mode == "/R" || mode == "-R" || mode == "--remove")
                {
                    API.SetTopMost(hwnd, false);
                }

                commandModeFlag = true;
                return;
            }
        }

        public static void Handle(string[] input_args)
        {
            args = input_args;

            try
            {
                while (idx < args.Length)
                    Eat();
            }
            catch
            {
                API.Shutdown(1);
            }




            if (alwaysOpenCheckingFlag && API.IsAlwaysRunning())
                API.Shutdown(1);

            if (!keepRunningFlag && commandModeFlag)
                API.Shutdown(0);
        }
    }
}

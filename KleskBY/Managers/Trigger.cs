using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TopHack.Other;

namespace TopHack.Managers
{
    class Trigger
    {
        public static void Run()
        {
            while (true)
            {
                Thread.Sleep(1);
                if (Reader.PlayerHealth > 0 && Globals.Imports.IsWindowFocues(Globals.Proc.Process))
                {
                    if (Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Keyboard.VK_C)))
                    {
                        if (Reader.InCros != 0) //2 - T; 3 - CT; 4 -hostages; but some servers mix this values so !=0
                        {
                            Globals.Imports.mouse_event(0x0020, 0, 0, 0, 0);
                            Thread.Sleep(10);
                            Globals.Imports.mouse_event(0x0040, 0, 0, 0, 0);
                        }
                    }
                }
            }
        }
    }
}

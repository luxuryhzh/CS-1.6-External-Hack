using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TopHack.Other;

namespace TopHack.Managers
{
    class Air
    {
        public static void Run()
        {
            Random rnd = new Random();
            while (true)
            {
                Thread.Sleep(1);
                if (Reader.PlayerHealth > 0 && Globals.Imports.IsWindowFocues(Globals.Proc.Process))
                {
                    if (Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Keyboard.VK_SPACE)))
                    {
                        if (Reader.OnGround == 1) //random sleep to bypass anticheats
                        {
                            Thread.Sleep(rnd.Next(3, 7));
                            Globals.Imports.mouse_event(0x0800, 0, 0, 120, 0);
                            Thread.Sleep(rnd.Next(9, 15));
                        }
                    }
                    if (Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Keyboard.VK_MENU)))
                    {
                        if (Reader.OnGround == 1)
                        {
                            Globals.Imports.mouse_event(0x0800, 0, 0, -120, 0);
                            Thread.Sleep(15);
                        }
                    }
                }
            }
        }
    }
}

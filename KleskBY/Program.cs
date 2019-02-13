using System;
using System.Collections.Generic;
using TopHack.Other;
using System.Linq;
using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using TopHack.Managers;
using System.Windows.Forms;


namespace TopHack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = $"1488";
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("LOOK FOR HL.EXE");
            Process[] process = Process.GetProcessesByName("hl");
            while (process.Length < 1)
            {
                process = Process.GetProcessesByName("hl");
                Thread.Sleep(250);
            }
            Globals.Proc.Process = process[0];
            Console.WriteLine("PROCCES FOUND!");


            List<string> loadedModules = new List<string>(Globals.Proc.Modules.Length);
            while (loadedModules.Count < Globals.Proc.Modules.Length)
            {
                process = Process.GetProcessesByName("hl");
                if (process.Length < 1) continue;
                foreach (ProcessModule module in process[0].Modules)
                {
                    if (Globals.Proc.Modules.Contains(module.ModuleName) && !loadedModules.Contains(module.ModuleName))
                    {
                        loadedModules.Add(module.ModuleName);

                        switch (module.ModuleName)
                        {
                            case "client.dll":
                                Globals.Proc.Client = module.BaseAddress;
                                break;
                            case "hw.dll":
                                Globals.Proc.HW = module.BaseAddress;
                                break;
                            default:
                                break;
                        }
                    }
                }
                Thread.Sleep(250);
            }
            Console.WriteLine("MODULES FOUND");
            Console.Clear();

            MemoryManager.Initialize(Globals.Proc.Process.Id);
            Console.WriteLine("MEMORY INITIALIZATHION SUCCESFULL");

            ThreadManager.Add("Reader", Reader.Run);
            ThreadManager.Add("Updater", Updater.Run);
            ThreadManager.Add("Trigger", Trigger.Run);
            ThreadManager.Add("Air", Air.Run);

            ThreadManager.ToggleThread("Reader");
            ThreadManager.ToggleThread("Updater");
            ThreadManager.ToggleThread("Air");
            ThreadManager.ToggleThread("Trigger");
            // Application.EnableVisualStyles();
            //  Application.Run(new DirectX_Renderer.Overlay()); 
        }
    }
}

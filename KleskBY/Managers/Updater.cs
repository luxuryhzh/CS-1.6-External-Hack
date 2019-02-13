using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Threading;
using TopHack.Other;


namespace TopHack.Managers
{
    class Updater
    {
        public static void Run()
        {
            while (true) 
            {
                MemoryManager.WriteMemory<Vector3>(0x04A41460, new Vector3(0, 0, 0));
                MemoryManager.WriteMemory<Vector3>(0x04A416B0, new Vector3(0, 0, 0));
                MemoryManager.WriteMemory<Vector3>(0x04A41900, new Vector3(0, 0, 0));
                MemoryManager.WriteMemory<Vector3>(0x04A41B50, new Vector3(0, 0, 0));
                MemoryManager.WriteMemory<Vector3>(0x04A41DA0, new Vector3(0, 0, 0));
                MemoryManager.WriteMemory<Vector3>(0x04A41FF0, new Vector3(0, 0, 0));
                MemoryManager.WriteMemory<Vector3>(0x04A42240, new Vector3(0, 0, 0));
                MemoryManager.WriteMemory<Vector3>(0x04A42490, new Vector3(0, 0, 0));
                MemoryManager.WriteMemory<Vector3>(0x04A426E0, new Vector3(0, 0, 0));
                Thread.Sleep(100);
            }
        }
    }
}

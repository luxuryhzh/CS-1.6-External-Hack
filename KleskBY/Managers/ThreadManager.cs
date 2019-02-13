using System;
using System.Threading;
using System.Collections.Generic;

using TopHack.Other;

namespace TopHack.Managers
{
    internal class ThreadManager
    {
        public static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();
        public static Dictionary<string, Thread> activeThreads = new Dictionary<string, Thread>();
        public static Dictionary<string, Thread> pausedThreads = new Dictionary<string, Thread>();

        public static void Add(string name, ThreadStart function)
        {
            if (threads.TryGetValue(name, out Thread temp)) return;

            threads.Add(name, new Thread(function));
        }

        public static void ToggleThread(string name)
        {
            if (activeThreads.TryGetValue(name, out Thread temp))
            {
                #pragma warning disable CS0618 // Typ oder Element ist veraltet
                temp.Suspend();
                #pragma warning restore CS0618 // Typ oder Element ist veraltet

                activeThreads.Remove(name);

                pausedThreads.Add(name, temp);

                Console.WriteLine($"[ThreadManager][Paused] { name }", true);

                Console.Beep(300, 100);
                Console.Beep(300, 100);
            }
            else
            {
                if (!threads.TryGetValue(name, out temp))
                {
                    Console.WriteLine("[ThreadManager][Error] Could not start { name }", 1500, false);
                    return;
                }

                if (pausedThreads.TryGetValue(name, out Thread temp2))
                {
                    #pragma warning disable CS0618 // Typ oder Element ist veraltet
                    temp2.Resume();
                    #pragma warning restore CS0618 // Typ oder Element ist veraltet

                    pausedThreads.Remove(name);

                    Console.WriteLine($"[ThreadManager][Resumed] { name }", true);

                    Console.Beep(300, 100);
                }
                else
                {
                    temp.Start();

                    Console.WriteLine($"[ThreadManager][Started] { name }", true);
                }

                activeThreads.Add(name, temp);
            }
        }
    }
}

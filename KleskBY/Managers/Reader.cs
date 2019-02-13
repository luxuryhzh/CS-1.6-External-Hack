using System.Threading;
using System;
using System.Collections.Generic;
using TopHack.Other;
using TopHack.Managers;
using System.Numerics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TopHack.Managers
{
    internal class Reader
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int GetAsyncKeyState(int vKey);
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
       


        static int rdardist = 8;
        static int rdardotsize = 8;
        static int rdarcenterX = (Screen.PrimaryScreen.Bounds.Width / 2);
        static int rdarcenterY = (Screen.PrimaryScreen.Bounds.Height / 2);

        public static int PlayerHealth;
        public static int PlayerTeam;
        public static int OnGround;
        public static int InCros;
        public static int IsMenu;
      //  public static float RecoilX;
        public static Vector3 LocalPlayerPos;
        public static Vector3 VecView;
        public static List<string> TargetsModels = new List<string> { };
        public static List<Vector3> Targets = new List<Vector3> { };
        public static List<float> Aimbot = new List<float> { };
        public static float M_PI_F = (180.0f / Convert.ToSingle(System.Math.PI));
        public static bool locker;


        public static Vector3 ClampAngle(Vector3 angle)
        {
            while (angle.Y > 180) angle.Y -= 360;
            while (angle.Y < -180) angle.Y += 360;

            if (angle.X > 89.0f) angle.X = 89.0f;
            if (angle.X < -89.0f) angle.X = -89.0f;

            angle.Z = 0f;

            return angle;
        }

        public static Vector3 NormalizeAngle(Vector3 angle)
        {
            while (angle.X < -180.0f) angle.X += 360.0f;
            while (angle.X > 180.0f) angle.X -= 360.0f;

            while (angle.Y < -180.0f) angle.Y += 360.0f;
            while (angle.Y > 180.0f) angle.Y -= 360.0f;

            while (angle.Z < -180.0f) angle.Z += 360.0f;
            while (angle.Z > 180.0f) angle.Z -= 360.0f;

            return angle;
        }

        public static Vector3 CalcAngle(Vector3 playerPosition, Vector3 enemyPosition, Vector3 aimPunch, Vector3 vecView)
        {
            Vector3 delta = new Vector3(playerPosition.X - enemyPosition.X, playerPosition.Y - enemyPosition.Y, (playerPosition.Z + vecView.Z) - enemyPosition.Z);

            Vector3 tmp = Vector3.Zero;
            tmp.X = Convert.ToSingle(System.Math.Atan(delta.Z / System.Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y))) * 57.295779513082f - aimPunch.X;
            tmp.Y = Convert.ToSingle(System.Math.Atan(delta.Y / delta.X)) * M_PI_F - aimPunch.Y;
            tmp.Z = 0;

            if (delta.X >= 0.0) tmp.Y += 180f;

            tmp = NormalizeAngle(tmp);
            tmp = ClampAngle(tmp);

            return tmp;
        }
        public static void Run()
        {
            while (true)
            {
                Thread.Sleep(1);

                InCros = MemoryManager.ReadMemory<int>((int)Globals.Proc.Client + 0x124224);
                PlayerTeam = MemoryManager.ReadMemory<int>((int)Globals.Proc.Client + 0xFFD04);
                OnGround = MemoryManager.ReadMemory<int>((int)Globals.Proc.HW + 0x122EC94);
                PlayerHealth = MemoryManager.ReadMemory<int>((int)Globals.Proc.HW + 0x1087ACC);
                IsMenu = MemoryManager.ReadMemory<int>((int)Globals.Proc.HW + 0x133484);

                VecView = MemoryManager.ReadMemory<Vector3>((int)Globals.Proc.HW + 0x122ED30);
                LocalPlayerPos = MemoryManager.ReadMemory<Vector3>(0x04A41024);

             //   RecoilX = MemoryManager.ReadMemory<float>((int)Globals.Proc.HW + 0x10FF624);

                string Player1Name = MemoryManager.ReadStringAscii((int)Globals.Proc.HW + 0x12013DC, 10);
                string Player1Model = MemoryManager.ReadStringAscii(0x04A41408, 3);
                Vector3 Player1Pos = MemoryManager.ReadMemory<Vector3>(0x04A41460);

                string Player2Name = MemoryManager.ReadStringAscii(0x04A4162C, 10);
                string Player2Model = MemoryManager.ReadStringAscii(0x04A41658, 3);
                Vector3 Player2Pos = MemoryManager.ReadMemory<Vector3>(0x04A416B0);

                string Player3Name = MemoryManager.ReadStringAscii(0x04A4187C, 10);
                string Player3Model = MemoryManager.ReadStringAscii(0x04A418A8, 3);
                Vector3 Player3Pos = MemoryManager.ReadMemory<Vector3>(0x04A41900);

                string Player4Name = MemoryManager.ReadStringAscii(0x04A41ACC, 10);
                string Player4Model = MemoryManager.ReadStringAscii(0x04A41AF8, 3);
                Vector3 Player4Pos = MemoryManager.ReadMemory<Vector3>(0x04A41B50);

                string Player5Name = MemoryManager.ReadStringAscii(0x04A41D1C, 10);
                string Player5Model = MemoryManager.ReadStringAscii(0x04A41D48, 3);
                Vector3 Player5Pos = MemoryManager.ReadMemory<Vector3>(0x04A41DA0);

                string Player6Name = MemoryManager.ReadStringAscii(0x04A41F6C, 10);
                string Player6Model = MemoryManager.ReadStringAscii(0x04A41F98, 3);
                Vector3 Player6Pos = MemoryManager.ReadMemory<Vector3>(0x04A41FF0);

                string Player7Name = MemoryManager.ReadStringAscii(0x04A421BC, 10);
                string Player7Model = MemoryManager.ReadStringAscii(0x04A421E8, 3);
                Vector3 Player7Pos = MemoryManager.ReadMemory<Vector3>(0x04A42240);

                string Player8Name = MemoryManager.ReadStringAscii(0x04A4240C, 10);
                string Player8Model = MemoryManager.ReadStringAscii(0x04A42438, 3);
                Vector3 Player8Pos = MemoryManager.ReadMemory<Vector3>(0x04A42490);

                string Player9Name = MemoryManager.ReadStringAscii(0x04A4265C, 10);
                string Player9Model = MemoryManager.ReadStringAscii(0x04A42688, 3);
                Vector3 Player9Pos = MemoryManager.ReadMemory<Vector3>(0x04A426E0);

                string Player10Name = MemoryManager.ReadStringAscii(0x04A428AC, 10);
                string Player10Model = MemoryManager.ReadStringAscii(0x04A428D8, 3);
                Vector3 Player10Pos = MemoryManager.ReadMemory<Vector3>(0x04A42930);

                string Player11Name = MemoryManager.ReadStringAscii(0x04A42AFC, 10);
                string Player11Model = MemoryManager.ReadStringAscii(0x04A42B28, 3);
                Vector3 Player11Pos = MemoryManager.ReadMemory<Vector3>(0x04A42B80);

                string Player12Name = MemoryManager.ReadStringAscii(0x04A42D4C, 10);
                string Player12Model = MemoryManager.ReadStringAscii(0x04A42D78, 3);
                Vector3 Player12Pos = MemoryManager.ReadMemory<Vector3>(0x04A42DD0);

                string Player13Name = MemoryManager.ReadStringAscii(0x04A42F9C, 10);
                string Player13Model = MemoryManager.ReadStringAscii(0x04A42FC8, 3);
                Vector3 Player13Pos = MemoryManager.ReadMemory<Vector3>(0x04A43020);

                string Player14Name = MemoryManager.ReadStringAscii(0x04A431EC, 10);
                string Player14Model = MemoryManager.ReadStringAscii(0x04A43218, 3);
                Vector3 Player14Pos = MemoryManager.ReadMemory<Vector3>(0x04A43270);

                locker = true;
                Targets.Clear();
                Targets.Add(Player1Pos);
                Targets.Add(Player2Pos);
                Targets.Add(Player3Pos);
                Targets.Add(Player4Pos);
                Targets.Add(Player5Pos);
                Targets.Add(Player6Pos);
                Targets.Add(Player7Pos);
                Targets.Add(Player8Pos);
                Targets.Add(Player9Pos);
                Targets.Add(Player10Pos);
                Targets.Add(Player11Pos);
                Targets.Add(Player12Pos);
                Targets.Add(Player13Pos);
                Targets.Add(Player14Pos);

                TargetsModels.Clear();
                TargetsModels.Add(Player1Model);
                TargetsModels.Add(Player2Model);
                TargetsModels.Add(Player3Model);
                TargetsModels.Add(Player4Model);
                TargetsModels.Add(Player5Model);
                TargetsModels.Add(Player6Model);
                TargetsModels.Add(Player7Model);
                TargetsModels.Add(Player8Model);
                TargetsModels.Add(Player9Model);
                TargetsModels.Add(Player10Model);
                TargetsModels.Add(Player11Model);
                TargetsModels.Add(Player12Model);
                TargetsModels.Add(Player13Model);
                TargetsModels.Add(Player14Model);

                
                Aimbot.Clear();
                if (PlayerHealth > 0 && Globals.Imports.IsWindowFocues(Globals.Proc.Process))
                {
                    for (int i = 0; i < Targets.Count; i++)
                    {
                        int MyX = (int)LocalPlayerPos.X;
                        int MyY = (int)LocalPlayerPos.Y;

                        int HisTeam = 0;
                        if (TargetsModels[i] == "arc" || TargetsModels[i] == "gue" || TargetsModels[i] == "lee" || TargetsModels[i] == "ter")
                        HisTeam = 1;
                        else HisTeam = 2;

                        if (PlayerTeam == HisTeam) continue;

                        float HisX = Targets[i].X;
                        float HisY = Targets[i].Y;

                        if (HisX == 0) continue;

                        Vector3 AimAngle = CalcAngle(LocalPlayerPos, Targets[i], Vector3.Zero, new Vector3(0, 0, 64));
                        float distance = AimAngle.Y - VecView.Y;
                        Aimbot.Add(distance);

                        double angle = (VecView.Y * Math.PI / 180) + 80;

                        double cos = Math.Cos(angle);
                        double sin = Math.Sin(angle);

                        int XNiceRise = Convert.ToInt32(rdarcenterX + ((HisX - MyX) / rdardist));
                        int YNiceRun = Convert.ToInt32(rdarcenterY - ((HisY - MyY) / rdardist));

                        int dx = XNiceRise - rdarcenterX;
                        int dy = YNiceRun - rdarcenterY;
                        double x = cos * dx - sin * dy + rdarcenterX;
                        double y = sin * dx + cos * dy + rdarcenterY;

                        IntPtr desktopPtr = GetDC(IntPtr.Zero);
                        Graphics g = Graphics.FromHdc(desktopPtr);
                        SolidBrush b = new SolidBrush(Color.Orange);
                        Font drawFont = new Font("Arial", 8);

                        g.FillRectangle(b, new Rectangle((int)Math.Round(x) - rdardotsize / 2, (int)Math.Round(y) - rdardotsize / 2, rdardotsize, rdardotsize));
                        ReleaseDC(IntPtr.Zero, desktopPtr);
                        g.Dispose();
                    }
                    if (!Aimbot.Any()) continue; //Aimbot should not be here cauze this thread is overloaded already

                    float MinValue = 9999999999999;
                    float distance2 = 0;
                    foreach (float value2 in Reader.Aimbot.ToArray())
                    {
                        if (Math.Abs(value2) < 8 || value2 < -330)
                        {
                            if (value2 >= -180)
                            {
                                if (Math.Abs(value2) < MinValue)
                                {
                                    MinValue = Math.Abs(value2);
                                    distance2 = value2;
                                }
                            }
                            else
                            {
                                if (Math.Abs(value2 + 360) < Math.Abs(MinValue + 360))
                                {
                                    MinValue = value2;
                                    distance2 = value2;
                                }
                            }
                        }
                    }
                    if (Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Keyboard.VK_V)))
                    {
                        if (distance2 > -61)
                        {
                            if (distance2 >= 0.5) Globals.Imports.mouse_event(0x0001, -1, 0, 0, 0);
                            if (distance2 <= -0.5) Globals.Imports.mouse_event(0x0001, 1, 0, 0, 0);
                        }
                        else
                        {
                            if (distance2 >= -359.5) Globals.Imports.mouse_event(0x0001, -1, 0, 0, 0);
                            if (distance2 <= -360.5) Globals.Imports.mouse_event(0x0001, 1, 0, 0, 0);
                        }
                    }
                }
            }
        }
    }
}

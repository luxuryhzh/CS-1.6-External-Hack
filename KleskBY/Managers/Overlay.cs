using System;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using TopHack.Managers;
using TopHack.Other;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace DirectX_Renderer
{
    public partial class Overlay_SharpDX : Form
    {
        public static System.Numerics.Vector2 ScreenWH;
        public static RECT rct;
        public static Graphics g;
        public static Pen Health = new Pen(Color.DarkBlue, 2);
        public static Font bigFont = new Font("Choktoff", 16);
        public static Brush mybrush = new SolidBrush(Color.White);
        public static Brush debugbrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
        public static float M_PI_F = (180.0f / Convert.ToSingle(System.Math.PI));


        private IntPtr handle;
        //DllImports
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetClientRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref int[] pMargins);

        [DllImport("user32.dll")]
        private static extern IntPtr SetActiveWindow(IntPtr handle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        #region useless
        public static Rectangle GetScreen(Control referenceControl)
        {
            return Screen.FromControl(referenceControl).Bounds;
        }

        public Overlay_SharpDX()
        {
            this.handle = Handle;
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            Globals.Imports.SetWindowPos(this.Handle, new IntPtr(-1), 0, 0, 0, 0, 0x0001 | 0x0002);
            OnResize(null);

            InitializeComponent();

            
            if (!GetWindowRect(Globals.Proc.Process.MainWindowHandle, out rct))
            {
                MessageBox.Show("ERROR");
                return;
            }
            MessageBox.Show($"{rct.Left}|{rct.Top}|{rct.Right - rct.Left + 1}|{rct.Bottom - rct.Top + 1}");
            this.Left = rct.Left;
            this.Top = rct.Top;
            this.Width = rct.Right - rct.Left + 1;
            this.Height = rct.Bottom - rct.Top + 1;
            ScreenWH = new System.Numerics.Vector2(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
        }

        private void Overlay_SharpDX_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Visible = true;
            timer1.Start();

            Activate();
        }

        public static double GetPlayerDistance(System.Numerics.Vector3 myLoc, System.Numerics.Vector3 enemyLoc)
        {
            double dist = System.Math.Sqrt(System.Math.Pow(enemyLoc.X - myLoc.X, 2) + System.Math.Pow(enemyLoc.Y - myLoc.Y, 2) + System.Math.Pow(enemyLoc.Z - myLoc.Z, 2));
            dist *= 0.01905F;
            return dist;
        }

        public static Vector3 ClampAngle(Vector3 angle)
        {
            while (angle.Y > 180) angle.Y -= 360;
            while (angle.Y < -180) angle.Y += 360;

            if (angle.X > 89.0f) angle.X = 89.0f;
            if (angle.X < -89.0f) angle.X = -89.0f;

            angle.Z = 0f;

            return angle;
        }

        public static float GetDistance3D(Vector3 playerPosition, Vector3 enemyPosition) => Convert.ToSingle(System.Math.Sqrt(System.Math.Pow(enemyPosition.X - playerPosition.X, 2f) + System.Math.Pow(enemyPosition.Y - playerPosition.Y, 2f) + System.Math.Pow(enemyPosition.Z - playerPosition.Z, 2f)));

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

        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        public static IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int WS_EX_TOPMOST = 0x00000008;
        private const int WM_ACTIVATE = 6;
        private const int WA_INACTIVE = 0;
        private const int WM_MOUSEACTIVATE = 0x0021;
        private const int MA_NOACTIVATEANDEAT = 0x0004;




        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams pm = base.CreateParams;
                pm.ExStyle |= 0x80;
                pm.ExStyle |= WS_EX_TOPMOST; // make the form topmost
                pm.ExStyle |= WS_EX_NOACTIVATE; // prevent the form from being activated
                return pm;
            }
        }

        /// <summary>
        /// Makes the form unable to gain focus at all time, 
        /// which should prevent lose focus
        /// </summary>
        /// <summary>
        /// Makes the form unable to gain focus at all time, 
        /// which should prevent lose focus
        /// </summary>
        /*    protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_MOUSEACTIVATE)
                {
                    m.Result = (IntPtr)MA_NOACTIVATEANDEAT;
                    return;
                }
                if (m.Msg == WM_ACTIVATE)
                {
                    if (((int)m.WParam & 0xFFFF) != WA_INACTIVE)
                        if (m.LParam != IntPtr.Zero)
                            SetActiveWindow(m.LParam);
                        else
                            SetActiveWindow(IntPtr.Zero);
                }
                else
                {
                    base.WndProc(ref m);
                }
            }*/
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTTRANSPARENT = -1;
            if (m.Msg == WM_NCHITTEST) m.Result = new IntPtr(HTTRANSPARENT);
            else base.WndProc(ref m);
        }




        #endregion

        private void Overlay_SharpDX_Paint(object sender, PaintEventArgs e)
        {
            if (Globals.Imports.IsWindowFocues(Globals.Proc.Process))
            {
                g = e.Graphics;
                DrawPlayers();
            }
        }

        private void DrawPlayers()
        {
            if (Reader.PlayerHealth > 0 && Globals.Imports.IsWindowFocues(Globals.Proc.Process))
            {
                if (!Reader.locker)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        int MyX = (int)Reader.LocalPlayerPos.X;
                        int MyY = (int)Reader.LocalPlayerPos.Y;

                        int HisTeam = 0;
                        if (Reader.TargetsModels[i] == "arc" || Reader.TargetsModels[i] == "gue"
                        || Reader.TargetsModels[i] == "lee" || Reader.TargetsModels[i] == "ter") HisTeam = 1;
                        else HisTeam = 2;

                        if (Reader.PlayerTeam == HisTeam) continue;

                        float HisX = Reader.Targets[i].X;
                        float HisY = Reader.Targets[i].Y;

                        if (HisX == 0) continue;

                        Vector3 AimAngle = CalcAngle(Reader.LocalPlayerPos, Reader.Targets[i], Vector3.Zero, new Vector3(0, 0, 64));
                        float distance = AimAngle.Y - Reader.VecView.Y;

                        int W = (rct.Right - rct.Left + 1);

                        double percent;
                        if (distance < -180) percent = 1 * (distance + 360) / 45;
                        else percent = 1 * distance / 45;

                        double EpsX = (this.Width / 2) - (W / 2 * percent);

                        int Dist = (int)GetPlayerDistance(Reader.LocalPlayerPos, Reader.Targets[i]);

                        g.DrawRectangle(Health, new Rectangle((int)Math.Round(EpsX) - 3, (this.Height / 2), 6, 6));

                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}


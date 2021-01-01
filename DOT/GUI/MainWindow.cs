using System;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using DOT.GUI.Components;
using DOT.Managers;
using DOT.Managers.KeyboardHook;

namespace DOT.GUI
{
    public partial class MainWindow : Form
    {
        private readonly BrowserManager BrowserManager = BrowserManager.Instance;
        private readonly HookManager KeyHookManager = HookManager.Instance;

        private readonly MetroTabControl Tabs = new MetroTabControl();

        #region "Drag form with header panel"
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private void P_Header_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
            if (e.Clicks == 2)
                MaxMini();
        }
        #endregion
        #region "Minimize fix"
        FormWindowState mLastState;
        protected override void OnClientSizeChanged(EventArgs e)
        {
            if (this.WindowState != mLastState)
            {
                mLastState = this.WindowState;
                OnWindowStateChanged(e);
            }
            base.OnClientSizeChanged(e);
        }
        protected void OnWindowStateChanged(EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                MainFrame.Visible = false;
            }
            else
            {
                MainFrame.Visible = true;
            }
        }
        #endregion
        #region "Header buttons & functions"
        private void MaxMini()
        {
            if (this.GetFormState() == FormExtentions.FormState.Maximized)
                this.SetFormState(FormExtentions.FormState.Normal);
            else
                this.SetFormState(FormExtentions.FormState.Maximized);

            bttn_maxmini.Text = bttn_maxmini.Text == "1" ? "2" : "1";
        }
        private void Bttn_maxmini_Click(object sender, EventArgs e)
        {
            MaxMini();
        }

        private void Bttn_mini_Click(object sender, EventArgs e)
        {
            MainFrame.Visible = false;
            WindowState = FormWindowState.Minimized;
        }

        private void Bttn_fullscreen_Click(object sender, EventArgs e)
        {
            p_header.Visible = false;
            Tabs.SetHeaderVisibility(false);
            this.SetFormState(FormExtentions.FormState.Fullscreen);
            snackbar.Show(this, "Tam ekrandan çıkmak için ESC tuşuna basabilirsin.");
        }

        private void Bttn_exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void P_header_DoubleClick(object sender, EventArgs e)
        {
            MaxMini();
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            KeyHookManager.RegisterKey((int)Keys.Escape, () =>
            {
                if (p_header.Visible) return;
                p_header.Visible = true;
                Tabs.SetHeaderVisibility(true);
                this.SetFormState(FormExtentions.FormState.Normal);
            });

            Tabs.Dock = DockStyle.Fill;
            Tabs.MouseClick += OnTabMouseDown;
            p_tabs.Controls.Add(Tabs);

            BrowserManager.NewPopUpEvent += AddTab;
            BrowserManager.TabCloseEvent += OnTabClose;

            AddTab("https://www.darkorbit.com");
        }

        private void OnTabClose(BrowserTab tab) => CloseTab(tab);

        private void OnTabMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                BrowserTab tab = Tabs.TabPages.Cast<BrowserTab>()
                   .Where((t, i) =>
                    Tabs.GetTabRect(i)
                    .Contains(e.Location)).First();
                CloseTab(tab);
            }
        }

        private void AddTab(string URL = "")
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => { AddTab_(URL); }));
            else
                AddTab_(URL);
            Tabs.SelectedIndex = Tabs.TabPages.Count - 1;
        }

        BrowserTab browserTab;
        private void AddTab_(string url)
        {
            browserTab = BrowserManager.GetNewTab(url);
            Tabs.TabPages.Add(browserTab);
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(300);
                Invoke(new Action(() => { browserTab.chromeBrowser.Visible = true; }));
            });

            Tabs.Refresh();
        }

        private void CloseTab(BrowserTab tab)
        {
            Tabs.TabPages.Remove(tab);
            tab.Dispose();
            if (Tabs.TabPages.Count == 0)
            {
                AddTab("https://darkorbit.com");
            }
        }

        private void Bttn_zoomin_Click(object sender, EventArgs e)
        {
            BrowserTab tab = (BrowserTab)Tabs.SelectedTab;
            tab.SetNextZoomIn();
        }

        private void Bttn_zoomout_Click(object sender, EventArgs e)
        {
            BrowserTab tab = (BrowserTab)Tabs.SelectedTab;
            tab.SetNextZoomOut();
        }

        private void BunifuButton1_Click(object sender, EventArgs e)
        {
            ((BrowserTab)Tabs.SelectedTab).ResetZoomLevel();
        }
    }
}

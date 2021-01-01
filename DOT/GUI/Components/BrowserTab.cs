using System;
using CefSharp;
using CefSharp.WinForms;
using System.Windows.Forms;
using MetroFramework.Controls;
using DOT.Managers;
using System.Drawing;

namespace DOT.GUI.Components
{
    public partial class BrowserTab : MetroTabPage, ILifeSpanHandler
    {
        public ChromiumWebBrowser chromeBrowser;
        private readonly CefManager CefManager = CefManager.Instance;

        public event Action<string> NewPopUpEvent;
        public event Action<BrowserTab> TabCloseEvent;

        private double _zoom;
        public double Zoom { get { return _zoom; } set { _zoom = value >= MaxZoomIn ? MaxZoomIn : value <= MaxZoomOut ? MaxZoomOut : value; } }

        private readonly double MaxZoomIn = +5.0;
        private readonly double MaxZoomOut = -2.5;
        private readonly int ZoomLevels = 5;

        public BrowserTab(string URL)
        {
            CheckForIllegalCrossThreadCalls = false;
            InitiliazeChromium(URL);
            Refresh();
        }

        private void InitiliazeChromium(string URL)
        {
            chromeBrowser = new ChromiumWebBrowser(URL);
            var contx = Cef.GetGlobalRequestContext();
            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                contx.SetPreference("profile.default_content_setting_values.plugins", 1, out string err);
            });

            chromeBrowser.LifeSpanHandler = this;

            chromeBrowser.BackColor = Color.FromArgb(32, 34, 37);
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.TitleChanged += TitleChanged;
            chromeBrowser.Visible = false;
            Controls.Add(chromeBrowser);
        }

        public void SetNextZoomIn()
        {
            chromeBrowser.SetZoomLevel(Zoom += MaxZoomIn / ZoomLevels);
        }

        public void SetNextZoomOut()
        {
            chromeBrowser.SetZoomLevel(Zoom += MaxZoomOut / ZoomLevels);
        }

        public void ResetZoomLevel()
        {
            Zoom = 0;
            chromeBrowser.SetZoomLevel(0);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        private void TitleChanged(object sender, TitleChangedEventArgs e)
        {
            this.Text = e.Title;
        }

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            TabCloseEvent?.Invoke(this);
            return true;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser){ }
        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { }

        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            NewPopUpEvent?.Invoke(targetUrl);

            newBrowser = null;
            return true;
        }
    }
}
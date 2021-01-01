using System;
using System.Collections.Generic;
using DOT.GUI.Components;

namespace DOT.Managers
{
    public class BrowserManager
    {
        public static BrowserManager Instance { get; } = new BrowserManager();

        public event Action<string> NewPopUpEvent;
        public event Action<BrowserTab> TabCloseEvent;

        public Dictionary<int, BrowserTab> TabDictionary = new Dictionary<int, BrowserTab>();

        public BrowserTab GetNewTab(string URL)
        {
            BrowserTab browserTab = new BrowserTab(URL);
            browserTab.NewPopUpEvent += OnNewPopUp;
            browserTab.TabCloseEvent += OnTabClose;
            return browserTab;
        }

        private void OnNewPopUp(string url)
        {
            NewPopUpEvent?.Invoke(url);
        }

        private void OnTabClose(BrowserTab tab)
        {
            TabCloseEvent?.Invoke(tab);
        }
    }
}

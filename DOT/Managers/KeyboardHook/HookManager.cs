using System;

namespace DOT.Managers.KeyboardHook
{
    public class HookManager
    {
        public static HookManager Instance { get; } = new HookManager();

        KeyboardHookManager keyboardHookManager = new KeyboardHookManager();

        public HookManager()
        {
            keyboardHookManager.Start();
        }

        public void RegisterKey(int keyId, Action action)
        {
            keyboardHookManager.RegisterHotkey(keyId, action);
        }

        public void RegisterKey(ModifierKeys modifiers, int keyId, Action action)
        {
            keyboardHookManager.RegisterHotkey(modifiers, keyId, action);
        }

        public void UnRegisterKey(int keyid)
        {
            keyboardHookManager.UnregisterHotkey(keyid);
        }

        public void UnRegisterKey(ModifierKeys[] modifiers, int virtualKeyCode)
        {
            keyboardHookManager.UnregisterHotkey(modifiers, virtualKeyCode);
        }
    }
}

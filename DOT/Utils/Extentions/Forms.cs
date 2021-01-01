using System;
using System.Drawing;

namespace System.Windows.Forms
{
    public static class FormExtentions
    {
        private static FormState formState { get; set; }
        private static Size PrevSize;
        private static Point PrevLoc;

        public enum FormState
        {
            Normal,
            Minimized,
            Maximized,
            Fullscreen
        }

        public static void SetFormState(this Form form, FormState state)
        {
            if (form.WindowState != FormWindowState.Maximized)
            {
                if (form.Size != GetMaxSize(form)) PrevSize = form.Size;
                if (form.Location != new Point(0, 0)) PrevLoc = form.Location;
            }

            switch (state)
            {
                case FormState.Normal:
                    if (form.WindowState == FormWindowState.Maximized) form.WindowState = FormWindowState.Normal;
                    form.Size = PrevSize;
                    formState = state;
                    form.Location = PrevLoc;
                    break;
                case FormState.Maximized:
                    form.Size = GetMaxSize(form);
                    formState = state;
                    form.Location = new Point(0, 0);
                    break;
                case FormState.Minimized:
                    form.WindowState = FormWindowState.Minimized;
                    formState = FormState.Minimized;
                    break;
                case FormState.Fullscreen:
                    form.WindowState = FormWindowState.Maximized;
                    formState = FormState.Fullscreen;
                    break;
            }
        }

        public static Size GetMaxSize(this Form form)
        {
            return new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 1);
        }

        public static FormState GetFormState(this Form form)
        {
            return formState;
        }
    }
}

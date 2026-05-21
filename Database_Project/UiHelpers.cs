using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Database_Project
{
    internal static class UiHelpers
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        public static void SetPlaceholder(TextBox textBox, string text)
        {
            if (textBox == null)
            {
                return;
            }

            SendMessage(textBox.Handle, EM_SETCUEBANNER, (IntPtr)1, text);
        }
    }
}

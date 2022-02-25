using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice_Free
{
    public static class WindowExtensions
    {
        public static void Maximize(this Window window)
        {
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);

            PInvoke.User32.ShowWindow(windowHandle, PInvoke.User32.WindowShowStyle.SW_SHOWMAXIMIZED);
        }
    }
}

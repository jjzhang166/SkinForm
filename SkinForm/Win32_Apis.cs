using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;

namespace Controls.SkinForm
{
    /// <summary>
    /// This class contains some win32 functions needed for nc drawing
    /// </summary>
    internal static class Win32Api
    {
        #region User32.dll

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hWnd, UInt32 wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate,
                                               IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, GWLIndex nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, GWLIndex nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, DCXFlags flags);
        #endregion

        #region Gdi32.dll

        [DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2,
           int cx, int cy);

        #endregion

        #region Shell32.dll

        [DllImport("Shell32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern Int32 SHAppBarMessage(int dwMessage, IntPtr pData);

        #endregion
    }
}

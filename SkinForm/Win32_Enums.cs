using System;
using System.Collections.Generic;

using System.Text;

namespace Controls.SkinForm
{
    #region Enums
    public enum ABMsg
    {
        ABM_NEW = 0,
        ABM_REMOVE = 1,
        ABM_QUERYPOS = 2,
        ABM_SETPOS = 3,
        ABM_GETSTATE = 4,
        ABM_GETTASKBARPOS = 5,
        ABM_ACTIVATE = 6,
        ABM_GETAUTOHIDEBAR = 7,
        ABM_SETAUTOHIDEBAR = 8,
        ABM_WINDOWPOSCHANGED = 9,
        ABM_SETSTATE = 10
    }
    public enum ABEdge
    {
        ABE_LEFT = 0,
        ABE_TOP = 1,
        ABE_RIGHT = 2,
        ABE_BOTTOM = 3
    }


    public enum WindowStyles : uint
    {
        WS_CAPTION = 0x00C00000,
        WS_BORDER = 0x00800000,
    }
    [Flags]
    public enum DCXFlags : long
    {
        DCX_WINDOW = 0x00000001L,
        DCX_CACHE = 0x00000002L,
        DCX_CLIPSIBLINGS = 0x00000010L,
    }
    public enum HitTest
    {
        HTNOWHERE = 0,
        HTCLIENT = 1,
        HTCAPTION = 2,
        HTSYSMENU = 3,
        HTMINBUTTON = 8,
        HTMAXBUTTON = 9,
        HTCLOSE = 20,
    }

    public enum RedrawWindowFlags
    {
        RDW_INVALIDATE = 0x0001,
        RDW_ALLCHILDREN = 0x0080,
        RDW_UPDATENOW = 0x0100,
        RDW_ERASENOW = 0x0200,
        RDW_FRAME = 0x0400,
    }
    public enum WAFlags
    {
        WA_ACTIVE = 1,
        WA_CLICKACTIVE = 2,
    }
    public enum GWLIndex
    {
        GWL_STYLE = (-16),
    }

    [Flags]
    public enum SWPFlags
    {
        SWP_NOSIZE = 0x0001,
        SWP_NOMOVE = 0x0002,
        SWP_NOZORDER = 0x0004,
        SWP_NOREDRAW = 0x0008,
        SWP_NOACTIVATE = 0x0010,
        SWP_FRAMECHANGED = 0x0020,
    }

    /// <summary>
    /// The SysCommands which can be executed by a form button.
    /// </summary>
    public enum SysCommand
    {
        /// <summary>
        /// If no SysCommands should be executed
        /// </summary>
        SC_NONE = 0x0,
        /// <summary>
        /// If the Form should be closed.
        /// </summary>
        SC_CLOSE = 0xf060,
        /// <summary>
        /// If the Form should be maximized
        /// </summary>
        SC_MAXIMIZE = 0xf030,
        /// <summary>
        /// If the Form should be minimized
        /// </summary>
        SC_MINIMIZE = 0xf020,
        /// <summary>
        /// If the form should be restored from the maximize mode.
        /// </summary>
        SC_RESTORE = 0xf120
    }

    /// <summary>
    /// Windows Messages
    /// Defined in winuser.h from Windows SDK v6.1
    /// Documentation pulled from MSDN.
    /// </summary>
    public enum Win32Messages : uint
    {
        /// <summary>
        /// The WM_SIZE message is sent to a window after its size has changed.
        /// </summary>
        SIZE = 0x0005,
        /// <summary>
        /// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately. 
        /// </summary>
        ACTIVATE = 0x0006,
        /// <summary>
        /// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
        /// </summary>
        SHOWWINDOW = 0x0018,
        /// <summary>
        /// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
        /// </summary>
        ACTIVATEAPP = 0x001C,
        GETMINMAXINFO = 0x0024,
        /// <summary>
        /// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        WINDOWPOSCHANGING = 0x0046,
        /// <summary>
        /// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        WINDOWPOSCHANGED = 0x0047,
        /// <summary>
        /// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles
        /// </summary>
        STYLECHANGED = 0x007D,
        /// <summary>
        /// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
        /// </summary>
        NCCALCSIZE = 0x0083,
        /// <summary>
        /// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
        /// </summary>
        NCHITTEST = 0x0084,
        /// <summary>
        /// The WM_NCPAINT message is sent to a window when its frame must be painted. 
        /// </summary>
        NCPAINT = 0x0085,
        /// <summary>
        /// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
        /// </summary>
        NCACTIVATE = 0x0086,
        /// <summary>
        /// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        NCMOUSEMOVE = 0x00A0,
        /// <summary>
        /// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        NCLBUTTONDOWN = 0x00A1,
        /// <summary>
        /// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        NCLBUTTONUP = 0x00A2,
        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.
        /// </summary>
        SYSCOMMAND = 0x0112,
        /// <summary>
        /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        LBUTTONDOWN = 0x0201,
        /// <summary>
        /// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window. 
        /// </summary>
        MDIACTIVATE = 0x0222,
        /// <summary>
        /// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
        /// </summary>
        MOUSELEAVE = 0x02A3,
        /// <summary>
        /// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
        /// </summary>
        NCMOUSELEAVE = 0x02A2,

        MOUSEHOVER = 0x2A1
    }
    #endregion
}

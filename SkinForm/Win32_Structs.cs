using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Controls.SkinForm
{
    #region Structs

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct APPBARDATA
    {
        public int cbSize;
        public IntPtr hWnd;
        public int uCallbackMessage;
        public int uEdge;
        public RECT rc;
        public IntPtr lParam;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct NCCALCSIZE_PARAMS
    {
        public RECT rect0;
        public RECT rect1;
        public RECT rect2;
        public WINDOWPOS IntPtr;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        private int _Left;
        private int _Top;
        private int _Right;
        private int _Bottom;

        public RECT(Rectangle Rectangle)
            : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
        {
        }
        public RECT(int Left, int Top, int Right, int Bottom)
        {
            _Left = Left;
            _Top = Top;
            _Right = Right;
            _Bottom = Bottom;
        }

        public int X
        {
            get { return _Left; }
            set { _Left = value; }
        }
        public int Y
        {
            get { return _Top; }
            set { _Top = value; }
        }
        public int Left
        {
            get { return _Left; }
            set { _Left = value; }
        }
        public int Top
        {
            get { return _Top; }
            set { _Top = value; }
        }
        public int Right
        {
            get { return _Right; }
            set { _Right = value; }
        }
        public int Bottom
        {
            get { return _Bottom; }
            set { _Bottom = value; }
        }
        public int Height
        {
            get { return _Bottom - _Top; }
            set { _Bottom = value - _Top; }
        }
        public int Width
        {
            get { return _Right - _Left; }
            set { _Right = value + _Left; }
        }
        public Point Location
        {
            get { return new Point(Left, Top); }
            set
            {
                _Left = value.X;
                _Top = value.Y;
            }
        }
        public Size Size
        {
            get { return new Size(Width, Height); }
            set
            {
                _Right = value.Height + _Left;
                _Bottom = value.Height + _Top;
            }
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(this.Left, this.Top, this.Width, this.Height);
        }
        public static Rectangle ToRectangle(RECT Rectangle)
        {
            return Rectangle.ToRectangle();
        }
        public static RECT FromRectangle(Rectangle Rectangle)
        {
            return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
        }

        public static implicit operator Rectangle(RECT Rectangle)
        {
            return Rectangle.ToRectangle();
        }
        public static implicit operator RECT(Rectangle Rectangle)
        {
            return new RECT(Rectangle);
        }
        public static bool operator ==(RECT Rectangle1, RECT Rectangle2)
        {
            return Rectangle1.Equals(Rectangle2);
        }
        public static bool operator !=(RECT Rectangle1, RECT Rectangle2)
        {
            return !Rectangle1.Equals(Rectangle2);
        }

        public override string ToString()
        {
            return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
        }

        public bool Equals(RECT Rectangle)
        {
            return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
        }
        public override bool Equals(object Object)
        {
            if (Object is RECT)
            {
                return Equals((RECT)Object);
            }
            else if (Object is Rectangle)
            {
                return Equals(new RECT((Rectangle)Object));
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "#" 
                + this._Left + this._Right +this._Top+this._Bottom).GetHashCode();
        }
    }
    #endregion
}

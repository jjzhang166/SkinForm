
/*
 * basic Skin setting of the SkiningForm
 */

using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using System.ComponentModel;

namespace Controls.SkinForm
{
    /// <summary>
    /// Provides base SystemInfo, can be set if needed
    /// </summary>
    public sealed class SystemInfoBase
    {
        Form _skiningForm;
        public Rectangle ScreenRect { get { return FormExtenders.GetScreenRect(_skiningForm); } }

        public Size BorderSize { get { return FormExtenders.GetBorderSize(_skiningForm); } }

        public int CaptionHeight { get { return FormExtenders.GetCaptionHeight(_skiningForm); } }

        public bool IsHasMenu { get { return FormExtenders.HasMenu(_skiningForm); } }

        public Size CaptionButtonSize { get { return FormExtenders.GetCaptionButtonSize(_skiningForm); } }


        public Size cornerSize { get ;set; }
        public SystemInfoBase(Form form)
        {
            _skiningForm = form;
            cornerSize = new Size(3, 3);
        }
    }
    /// <summary>
    /// Create base Pain tData
    /// </summary>
    sealed class PaintDataBaseParas
    {
        Form _skiningForm;
        internal IntPtr _hdc ;
        internal Graphics _g ;
        internal Region _region;
        internal IntPtr _hrgn;

        // graphics data
        internal readonly BufferedGraphicsContext _bufferContext = BufferedGraphicsManager.Current;
        internal BufferedGraphics _bufferGraphics;
        internal Size _currentCacheSize;

        internal PaintDataBaseParas(Form needDecorateForm)
        {
            _skiningForm = needDecorateForm;
            IntPtr _hdc = (IntPtr)0;
            IntPtr _hrgn = (IntPtr)0;
        }
        internal void Reset()
        {
            _currentCacheSize = Size.Empty;

            if (_bufferGraphics != null)
                _bufferGraphics.Dispose();

            // cleanup data
            if (_hdc != (IntPtr)0)
            {
                Win32Api.SelectClipRgn(_hdc, (IntPtr)0);
                Win32Api.ReleaseDC(_skiningForm.Handle, _hdc);
            }
            if (_region != null && _hrgn != (IntPtr)0)
                _region.ReleaseHrgn(_hrgn);

            if (_region != null)
                _region.Dispose();

            if (_g != null)
                _g.Dispose();
        }

    }
    /// <summary>
    /// Provides a base class for skim used in a form.
    /// Inherit this class to create custom skin, and passed the skin to the Skin property of the SkinForm component.
    /// </summary>
    public abstract class SkinBase : IDisposable
    {
        #region Fields

        Form _skiningForm;

        List<CaptionButton> _captionButtons;

        SystemInfoBase _systemInfoBase;

        PaintDataBaseParas _basePaintParas;
        bool _formIsActive;

        // used for state resetting 
        CaptionButton _pressedButton;
        CaptionButton _hoveredButton;


#region Properties

        protected CaptionButton PressedButton
        {
            get { return _pressedButton; }
            set { _pressedButton = value; }
        }

        protected CaptionButton HoveredButton
        {
            get { return _hoveredButton; }
            set { _hoveredButton = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the form is an active form.
        /// </summary>
        public virtual bool FormIsActive
        {
            get { return _formIsActive; }
            set { _formIsActive = value; }
        }


        /// <summary>
        /// Gets the skinned form.
        /// </summary>
        public virtual Form SkiningForm
        {
            get { return _skiningForm; }
            internal set 
            {
                _skiningForm = value;
                _basePaintParas = new PaintDataBaseParas(_skiningForm);
                _systemInfoBase = new SystemInfoBase(_skiningForm);
            }
        }

        protected internal virtual List<CaptionButton> CaptionButtons
        {
            get{ return _captionButtons; }
            set { _captionButtons = value; }
        }

        protected internal virtual SystemInfoBase SkinSystemInfo
        {
            get{ return _systemInfoBase;}
            set { _systemInfoBase = value; }
        }

        protected internal virtual Size cornerSize
        {
            get { return _systemInfoBase.cornerSize; }
            set { _systemInfoBase.cornerSize = value; }
        }

#endregion
        #endregion
        //--------
        #region Constructor

        public SkinBase()
        {
            _formIsActive = true;
            _captionButtons = new List<CaptionButton>();
        }

        #endregion


        #region protected internal virtual





        /// <summary>
        /// Updates the window style for the parent form.
        /// </summary>
        protected internal virtual void UpdateStyle()
        {
            // remove the border style
            Int32 currentStyle = Win32Api.GetWindowLong(_skiningForm.Handle, GWLIndex.GWL_STYLE);
            if ((currentStyle & (int)(WindowStyles.WS_BORDER)) != 0)
            {
                currentStyle &= ~(int)(WindowStyles.WS_BORDER);
                Win32Api.SetWindowLong(_skiningForm.Handle, GWLIndex.GWL_STYLE, currentStyle);
                Win32Api.SetWindowPos(_skiningForm.Handle, (IntPtr)0, -1, -1, -1, -1,
                                      (int)(SWPFlags.SWP_NOZORDER | SWPFlags.SWP_NOSIZE | SWPFlags.SWP_NOMOVE |
                                             SWPFlags.SWP_FRAMECHANGED | SWPFlags.SWP_NOREDRAW | SWPFlags.SWP_NOACTIVATE));
            }
        }


        /// <summary>
        /// Handles the window sizing
        /// </summary>
        /// <param name="m">The m.</param>
        protected internal virtual void OnSize(Message m)
        {
            UpdateCaption();
            // update form styles on maximize/restore
            if (_skiningForm.MdiParent != null)
            {
                if ((int)m.WParam == 0)
                    UpdateStyle();
                if ((int)m.WParam == 2)
                    _skiningForm.Refresh();
            }

            // update region if needed
            bool wasMaxMin = (_skiningForm.WindowState == FormWindowState.Maximized ||
                _skiningForm.WindowState == FormWindowState.Minimized);

            RECT rect1 = new RECT();
            Win32Api.GetWindowRect(_skiningForm.Handle, ref rect1);

            Rectangle rc = new Rectangle(rect1.Left, rect1.Top, rect1.Right - rect1.Left, rect1.Bottom - rect1.Top - 1);


            if (wasMaxMin && _skiningForm.WindowState == FormWindowState.Normal &&
                rc.Size == _skiningForm.RestoreBounds.Size)
            {
                OnSetRegion(new Size(rect1.Right - rect1.Left, rect1.Bottom - rect1.Top));
                OnNcPaint(true);
            }
        }

        /// <summary>
        /// Gets the command button at the specified position.
        /// </summary>
        /// <param name="point">The position.</param>
        /// <returns>the CaptionButton instance or null if no button was found.</returns>
        protected internal virtual CaptionButton CommandButtonFromPoint(Point point)
        {
            foreach (CaptionButton button in _captionButtons)
                if (button.Bounds.Contains(point)) return button;
            return null;
        }

        /// <summary>
        /// Performs the non client HitTest
        /// </summary>
        /// <param name="m">The Message</param>
        /// <returns>true if the orginal handler should be suppressed otherwise false.</returns>
        protected internal virtual bool OnNcHitTest(ref Message m)
        {
            if (!IsProcessNcArea)
                return false;

            Point point = new Point(m.LParam.ToInt32());
            Rectangle rectScreen = _systemInfoBase.ScreenRect; ;
            Rectangle rect = rectScreen;

            // custom processing
            if (rect.Contains(point))
            {
                Size borderSize = _systemInfoBase.BorderSize;
                rect.Inflate(-borderSize.Width, -borderSize.Height);

                // let form handle hittest itself if we are on borders
                if (!rect.Contains(point))
                    return false;

                Rectangle rectCaption = rect;
                rectCaption.Height = _systemInfoBase.CaptionHeight;

                // not in caption -> client
                if (!rectCaption.Contains(point))
                {
                    m.Result = (IntPtr)(int)HitTest.HTCLIENT;
                    return true;
                }

                // on icon?
                if (_systemInfoBase.IsHasMenu)
                {
                    Rectangle rectSysMenu = rectCaption;
                    rectSysMenu.Size = SystemInformation.SmallIconSize;
                    if (rectSysMenu.Contains(point))
                    {
                        m.Result = (IntPtr)(int)HitTest.HTSYSMENU;
                        return true;
                    }
                }

                // on Button?
                Point pt = new Point(point.X - rectScreen.X, point.Y - rectScreen.Y);
                CaptionButton sysButton = CommandButtonFromPoint(pt);
                if (sysButton != null)
                {
                    m.Result = (IntPtr)sysButton.HitTest;
                    return true;
                }

                // on Caption?
                m.Result = (IntPtr)(int)HitTest.HTCAPTION;
                return true;
            }
            m.Result = (IntPtr)(int)HitTest.HTNOWHERE;
            return true;
        }

        /// <summary>
        /// Redraws the non client area.
        /// </summary>
        /// <param name="invalidateBuffer">if set to <c>true</c> the buffer is invalidated.</param>
        /// <returns>true if the original painting should be suppressed otherwise false.</returns>
        protected internal virtual bool OnNcPaint(bool invalidateBuffer)
        {
            if (!IsProcessNcArea)
                return false;
            bool result = false;
            try
            {
                SkinningFormPaintData paintData = CreatePaintData();

                // create painting meta data for caption buttons
                if (_captionButtons.Count > 0)
                {
                    paintData.CaptionButtons = new CaptionButtonPaintData[_captionButtons.Count];
                    for (int i = 0; i < _captionButtons.Count; i++)
                    {
                        CaptionButton button = _captionButtons[i];
                        CaptionButtonPaintData buttonData = new CaptionButtonPaintData()
                        {
                            BaseData = new PaintDataBase(_basePaintParas._bufferGraphics.Graphics, button.Bounds),
                            Pressed = button.Pressed,
                            Hovered = button.Hovered,
                            Enabled = button.Enabled,
                            HitTest = button.HitTest
                        };
                        paintData.CaptionButtons[i] = buttonData;
                    }
                }

                // paint
                result = OnNcPaint(paintData);

                // render buffered graphics 
                if (_basePaintParas._bufferGraphics != null)
                    _basePaintParas._bufferGraphics.Render(_basePaintParas._g);

            }
            catch (System.Exception )
            {
                result = false;
            }

            _basePaintParas.Reset();
            return result;
        }


        /// <summary>
        /// Updates the caption.
        /// </summary>
        protected internal virtual void UpdateCaption()
        {
            // add command handlers
            foreach (CaptionButton button in _captionButtons)
                button.PropertyChanged += OnCommandButtonPropertyChanged;

            // Calculate Caption Button Bounds
            RECT rectScreen = new RECT();
            Win32Api.GetWindowRect(_skiningForm.Handle, ref rectScreen);
            Rectangle rect = rectScreen.ToRectangle();

            Size borderSize = _systemInfoBase.BorderSize;
            rect.Offset(-rect.Left, -rect.Top);

            Size captionButtonSize = _systemInfoBase.CaptionButtonSize;
            Rectangle buttonRect = new Rectangle(rect.Right - borderSize.Width - captionButtonSize.Width, rect.Top + borderSize.Height,
                                    captionButtonSize.Width, captionButtonSize.Height);

            foreach (CaptionButton button in _captionButtons)
            {
                button.Bounds = buttonRect;
                buttonRect.X -= captionButtonSize.Width;
            }
        }

        /// <summary>
        /// Called when a property of a CommandButton has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnCommandButtonPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // if a button is hovered or pressed invalidate
            if (e.PropertyName == "Pressed" || e.PropertyName == "Hovered")
                OnNcPaint(true);
        }

        /// <summary>
        /// Gets a value indicating whether we should process the nc area.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if we should process the nc area; otherwise, <c>false</c>.
        /// </value>
        protected internal virtual bool IsProcessNcArea
        {
            get
            {
                // check if we should process the nc area
                return
                    SkiningForm.MdiParent == null || SkiningForm.WindowState != FormWindowState.Maximized;
            }
        }

        /// <summary>
        /// Gets the command button with the specified HitTest.
        /// </summary>
        /// <param name="hitTest">The hitTest.</param>
        /// <returns>the CaptionButton instance or null if no button was found.</returns>
        protected internal virtual CaptionButton CommandButtonByHitTest(HitTest hitTest)
        {
            foreach (CaptionButton button in _captionButtons)
                if (button.HitTest == hitTest)
                    return button;

            return null;
        }

        protected internal virtual PaintDataBase CreatePaintDataBase()
        {
            // prepare image bounds
            Size borderSize = _systemInfoBase.BorderSize;
            int captionHeight = _systemInfoBase.CaptionHeight;

            RECT rectScreen = new RECT();
            Win32Api.GetWindowRect(SkiningForm.Handle, ref rectScreen);

            Rectangle rectBounds = rectScreen.ToRectangle();
            rectBounds.Offset(-rectBounds.X, -rectBounds.Y);

            // prepare clipping
            Rectangle rectClip = rectBounds;
            _basePaintParas._region = new Region(rectClip);
            rectClip.Inflate(-borderSize.Width, -borderSize.Height);
            rectClip.Y += captionHeight;
            rectClip.Height -= captionHeight;

            // create graphics handle
            _basePaintParas._hdc = Win32Api.GetDCEx(SkiningForm.Handle, (IntPtr)0,
                (DCXFlags.DCX_CACHE | DCXFlags.DCX_CLIPSIBLINGS | DCXFlags.DCX_WINDOW));
            _basePaintParas._g = Graphics.FromHdc(_basePaintParas._hdc);

            // Apply clipping
            _basePaintParas._region.Exclude(rectClip);
            _basePaintParas._hrgn = _basePaintParas._region.GetHrgn(_basePaintParas._g);
            Win32Api.SelectClipRgn(_basePaintParas._hdc, _basePaintParas._hrgn);

            // create new buffered graphics if needed
            if (_basePaintParas._bufferGraphics == null || _basePaintParas._currentCacheSize != rectBounds.Size)
            {
                if (_basePaintParas._bufferGraphics != null)
                    _basePaintParas._bufferGraphics.Dispose();

                _basePaintParas._bufferGraphics = _basePaintParas._bufferContext.Allocate(_basePaintParas._g, new Rectangle(0, 0,
                            rectBounds.Width, rectBounds.Height));
                _basePaintParas._currentCacheSize = rectBounds.Size;
            }

            // Create painting meta data for form
            PaintDataBase paintDataBase = new PaintDataBase(_basePaintParas._bufferGraphics.Graphics, rectBounds);


            return paintDataBase;
        }

        /// <summary>
        /// Called when the mouse pointer is moved over the non-client area of the form.
        /// </summary>
        protected internal virtual void OnMouseMove(ref Message m)
        {
            // Check for hovered and pressed buttons
            if (Control.MouseButtons != MouseButtons.Left)
            {
                if (_pressedButton != null)
                {
                    _pressedButton.Pressed = false;
                    _pressedButton = null;
                }
            }
            CaptionButton button = CommandButtonByHitTest((HitTest)m.WParam);

            if (_hoveredButton != button && _hoveredButton != null)
                _hoveredButton.Hovered = false;
            if (_pressedButton == null)
            {
                if (button != null)
                    button.Hovered = true;
                _hoveredButton = button;
            }
            else
                _pressedButton.Pressed = (button == _pressedButton);
        }

        /// <summary>
        /// Called when the mouse pointer is over the non-client area of the form and a mouse button is pressed.
        /// </summary>
        protected internal virtual bool OnMouseDown(ref Message m)
        {
            CaptionButton button = CommandButtonByHitTest((HitTest)m.WParam);
            if (_pressedButton != button && _pressedButton != null)
                _pressedButton.Pressed = false;
            if (button != null)
                button.Pressed = true;
            _pressedButton = button;
            if (_pressedButton != null)
            {
                m.Result = (IntPtr)1;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when the mouse pointer is leaving the non-client area of the form.
        /// </summary>
        protected internal virtual void OnMouseLeave()
        {
            if (_pressedButton != null)
                _pressedButton.Pressed = false;
            if (_hoveredButton != null)
            {
                _hoveredButton.Hovered = false;
                _hoveredButton = null;
            }
        }

        /// <summary>
        /// Called when the form need to set its region.
        /// </summary>
        protected internal virtual void OnSetRegion(Size size)
        {
            Size cornerSize = _systemInfoBase.cornerSize; 
            // Create a rounded rectangle using Gdi
            IntPtr hRegion = Win32Api.CreateRoundRectRgn(0, 0, size.Width + 1, size.Height + 1, cornerSize.Width, cornerSize.Height);
            Region region = Region.FromHrgn(hRegion);
            SkiningForm.Region = region;
            region.ReleaseHrgn(hRegion);
        }

        #endregion


        #region protected Internal Functions
        protected internal abstract SkinningFormPaintData CreatePaintData();



        /// <summary>
        /// Called when the mouse pointer is over the non-client area of the form and a mouse button is released.
        /// </summary>
        protected internal abstract bool OnMouseUp( Message m);



        /// <summary>
        /// Called when the non client area of the form needs to be painted.
        /// </summary>
        /// <param name="form">The form which gets drawn.</param>
        /// <param name="paintData">The paint data to use for drawing.</param>
        /// <returns><code>true</code> if the original painting should be suppressed, otherwise <code>false</code></returns>
        protected internal abstract bool OnNcPaint(SkinningFormPaintData paintData);






        #endregion


        #region IDisposable Members
        protected bool _disposed = false;
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
        #endregion
    }


}

using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Controls.SkinForm
{
    class SkiningForm : NativeWindow
    {
        #region Fields

        // form data
        private SkinForm _owner;
        bool _isDetachedForm;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether whe should process the nc area.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if we should process the nc area; otherwise, <c>false</c>.
        /// </value>
        private bool IsProcessNcArea
        {
            get
            {
                // check if we should process the nc area
                return
                    !(_owner == null ||
                      _owner.SkiningForm.MdiParent != null && _owner.SkiningForm.WindowState == FormWindowState.Maximized);
            }
        }

        #endregion

        #region Constructor and Destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinningForm"/> class.
        /// </summary>
        /// <param name="parentForm">The parent form.</param>
        /// <param name="manager">The manager.</param>
        public SkiningForm(SkinForm parentForm)
        {

            _owner = parentForm;
            _isDetachedForm = false;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SkinningForm"/> is reclaimed by garbage collection.
        /// </summary>
        ~SkiningForm()
        {
            UnregisterEventHandlers();
        }
        #endregion

        #region Public Functions
        public void AttachForm()
        {
            if (_owner.SkiningForm != null)
            {
                if (_owner.SkiningForm.Handle != IntPtr.Zero)
                    OnHandleCreated(_owner.SkiningForm, new EventArgs());
                RegisterEventHandlers();
                _isDetachedForm = true;
            }
        }
        public void DetachForm()
        {
            UnregisterEventHandlers();
            _isDetachedForm = false;
        }
        public bool IsDetachedForm { 
            get { return _isDetachedForm; } 
            set { _isDetachedForm = value; } 
        }

        #endregion


        #region Parent Form Handlers

        /// <summary>
        /// Registers all important eventhandlers.
        /// </summary>
        private void RegisterEventHandlers()
        {
            _owner.SkiningForm.HandleCreated += OnHandleCreated;
            _owner.SkiningForm.HandleDestroyed += OnHandleDestroyed;
            _owner.SkiningForm.TextChanged += OnTextChanged;
            _owner.SkiningForm.Disposed += OnParentDisposed;
        }

        /// <summary>
        /// Unregisters all important eventhandlers.
        /// </summary>
        private void UnregisterEventHandlers()
        {
            if (_owner.SkiningForm != null)
            {
                _owner.SkiningForm.HandleCreated -= OnHandleCreated;
                _owner.SkiningForm.HandleDestroyed -= OnHandleDestroyed;
                _owner.SkiningForm.TextChanged -= OnTextChanged;
                _owner.SkiningForm.Disposed -= OnParentDisposed;
            }

        }

        /// <summary>
        /// Called when the handle of the parent form is created.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnHandleCreated(object sender, EventArgs e)
        {
            // this little line allows us to handle the windowMessages of the parent form in this class
            AssignHandle(((Form)sender).Handle);
            if (IsProcessNcArea)
            {
               _owner.Skin.UpdateStyle();
               _owner.Skin.UpdateCaption();
            }
        }

        /// <summary>
        /// Called when the handle of the parent form is destroyed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            // release handle as it is destroyed
            ReleaseHandle();
        }

        /// <summary>
        /// Called when the parent of the parent form is disposed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnParentDisposed(object sender, EventArgs e)
        {
            // unregister events as the parent of the form is disposed
            if (_owner != null)
                UnregisterEventHandlers();
            _owner = null;
        }

        /// <summary>
        /// Called when the text on the parent form has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnTextChanged(object sender, EventArgs e)
        {
            // Redraw on title change
            if (IsProcessNcArea)
                _owner.Skin.OnNcPaint(true);
        }

        #endregion

        #region Skinning Executors

        /// <summary>
        /// Invokes the default window procedure associated with this window.
        /// </summary>
        /// <param name="m">A <see cref="T:System.Windows.Forms.Message"/> that is associated with the current Windows message.</param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            bool supressOriginalMessage = false;
            if (IsProcessNcArea)
                switch ((Win32Messages)m.Msg)
                {
                    // update form data on style change
                    case Win32Messages.STYLECHANGED:
                        _owner.Skin.UpdateStyle();
                        if (_owner.Skin != null)
                        {
                            _owner.Skin.OnSetRegion(_owner.SkiningForm.Size);
                        }
                        break;

                    #region Handle Form Activation
                    case Win32Messages.ACTIVATEAPP:
                        // redraw
                        if (_owner.Skin != null) _owner.Skin.FormIsActive = (int)m.WParam != 0;
                       _owner.Skin.OnNcPaint(true);
                        break;

                    case Win32Messages.ACTIVATE:
                        // Set active state and redraw
                        _owner.Skin.FormIsActive = ((int)WAFlags.WA_ACTIVE == (int)m.WParam || (int)WAFlags.WA_CLICKACTIVE == (int)m.WParam);
                        _owner.Skin.OnNcPaint(true);
                        break;
                    case Win32Messages.MDIACTIVATE:
                        // set active and redraw on activation 
                        if (m.WParam == _owner.SkiningForm.Handle)
                            _owner.Skin.FormIsActive = false;
                        else if (m.LParam == _owner.SkiningForm.Handle)
                            _owner.Skin.FormIsActive = true;
                        _owner.Skin.OnNcPaint(true);
                        break;
                    #endregion

                    #region Handle Mouse Processing
                    // Set Pressed button on mousedown
                    case Win32Messages.NCLBUTTONDOWN:
                        supressOriginalMessage = _owner.Skin.OnMouseDown(ref m);
                        break;
                    // Set hovered button on mousemove
                    case Win32Messages.NCMOUSEMOVE:
                        _owner.Skin.OnMouseMove(ref m);
                        break;
                    // perform button actions if a button was clicked
                    case Win32Messages.NCLBUTTONUP:
                        // Handle button up
                        supressOriginalMessage = _owner.Skin.OnMouseUp( m);

                        break;
                    // restore button states on mouseleave
                    case Win32Messages.NCMOUSELEAVE:
                    case Win32Messages.MOUSELEAVE:
                    case Win32Messages.MOUSEHOVER:
                        _owner.Skin.OnMouseLeave();
                        break;
                    #endregion

                    #region Size Processing

                    // Set region as window is shown                    
                    case Win32Messages.SHOWWINDOW:
                        if (_owner.Skin != null)
                        {
                            _owner.Skin.OnSetRegion(_owner.SkiningForm.Size);
                        }
                        break;
                    // adjust region on resize
                    case Win32Messages.SIZE:
                       _owner.Skin.OnSize(m);
                        break;
                    // ensure that the window doesn't overlap docked toolbars on desktop (like taskbar)
                    case Win32Messages.GETMINMAXINFO:
                        supressOriginalMessage = CalculateMaxSize(ref m);
                        break;
                    // update region on resize
                    case Win32Messages.WINDOWPOSCHANGING:
                        WINDOWPOS wndPos = (WINDOWPOS)m.GetLParam(typeof(WINDOWPOS));
                        if ((wndPos.flags & (int)SWPFlags.SWP_NOSIZE) == 0)
                        {
                            if (_owner.Skin != null)
                            {
                                _owner.Skin.OnSetRegion(new Size(wndPos.cx, wndPos.cy));
                            }

                        }
                        break;
                    // remove region on maximize or repaint on resize
                    case Win32Messages.WINDOWPOSCHANGED:
                        if (_owner.SkiningForm.WindowState == FormWindowState.Maximized)
                            _owner.SkiningForm.Region = null;

                        WINDOWPOS wndPos2 = (WINDOWPOS)m.GetLParam(typeof(WINDOWPOS));
                        if ((wndPos2.flags & (int)SWPFlags.SWP_NOSIZE) == 0)
                        {
                            _owner.Skin.UpdateCaption();
                            _owner.Skin.OnNcPaint(true);
                        }
                        break;
                    #endregion

                    #region Non Client Area Handling
                    // paint the non client area
                    case Win32Messages.NCPAINT:
                        if (_owner.Skin.OnNcPaint(true))
                        {
                            m.Result = (IntPtr)1;
                            supressOriginalMessage = true;
                        }
                        break;
                    // calculate the non client area size
                    case Win32Messages.NCCALCSIZE:
                        if (m.WParam == (IntPtr)1)
                        {
                            if (_owner.SkiningForm.MdiParent != null)
                                break;
                            // add caption height to non client area
                            NCCALCSIZE_PARAMS p = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));
                            p.rect0.Top += FormExtenders.GetCaptionHeight(_owner.SkiningForm);
                            Marshal.StructureToPtr(p, m.LParam, true);
                        }
                        break;
                    // non client hit test
                    case Win32Messages.NCHITTEST:
                        if (_owner.Skin.OnNcHitTest(ref m))
                            supressOriginalMessage = true;
                        break;
                    #endregion
                }

            if (!supressOriginalMessage)
                base.WndProc(ref m);
        }






        /// <summary>
        /// Ensure that the window doesn't overlap docked toolbars on desktop (like taskbar)
        /// </summary>
        /// <param name="m">The message.</param>
        /// <returns></returns>
        private bool CalculateMaxSize(ref Message m)
        {
            if (_owner.SkiningForm.Parent == null)
            {
                // create minMax info for maximize data
                MINMAXINFO info = (MINMAXINFO)m.GetLParam(typeof(MINMAXINFO));
                Rectangle rect = SystemInformation.WorkingArea;
                Size fullBorderSize = new Size(SystemInformation.Border3DSize.Width + SystemInformation.BorderSize.Width,
                    SystemInformation.Border3DSize.Height + SystemInformation.BorderSize.Height);

                info.ptMaxPosition.x = rect.Left - fullBorderSize.Width;
                info.ptMaxPosition.y = rect.Top - fullBorderSize.Height;
                info.ptMaxSize.x = rect.Width + fullBorderSize.Width * 2;
                info.ptMaxSize.y = rect.Height + fullBorderSize.Height * 2;

                info.ptMinTrackSize.y += FormExtenders.GetCaptionHeight(_owner.SkiningForm);


                if (!_owner.SkiningForm.MaximumSize.IsEmpty)
                {
                    info.ptMaxSize.x = Math.Min(info.ptMaxSize.x, _owner.SkiningForm.MaximumSize.Width);
                    info.ptMaxSize.y = Math.Min(info.ptMaxSize.y, _owner.SkiningForm.MaximumSize.Height);
                    info.ptMaxTrackSize.x = Math.Min(info.ptMaxTrackSize.x, _owner.SkiningForm.MaximumSize.Width);
                    info.ptMaxTrackSize.y = Math.Min(info.ptMaxTrackSize.y, _owner.SkiningForm.MaximumSize.Height);
                }

                if (!_owner.SkiningForm.MinimumSize.IsEmpty)
                {
                    info.ptMinTrackSize.x = Math.Max(info.ptMinTrackSize.x, _owner.SkiningForm.MinimumSize.Width);
                    info.ptMinTrackSize.y = Math.Max(info.ptMinTrackSize.y, _owner.SkiningForm.MinimumSize.Height);
                }

                // set wished maximize size
                Marshal.StructureToPtr(info, m.LParam, true);

                m.Result = (IntPtr)0;
                return true;
            }
            return false;
        }





        /// <summary>
        /// Redraws the non client area..
        /// </summary>
        public void Invalidate()
        {
            if (IsProcessNcArea)
                _owner.Skin.OnNcPaint(true);
        }





        #endregion
    }
}

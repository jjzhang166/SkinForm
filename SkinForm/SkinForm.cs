using System;
using System.Collections.Generic;

using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace Controls.SkinForm
{
    public class SkinForm : Component
    {

        #region Fields
        SkinBase _skin = null;
        Form _skiningForm = null;
        SkinFormHook _hook;
        #endregion
        #region Constructor
        public SkinForm()
        {
            _hook = new SkinFormHook(this);
        }
        #endregion
        #region Public Properties
        /// <summary>
        /// Gets or sets the skin used to skinning the form.
        /// </summary>
        public SkinBase Skin
        {
            get { return _skin; }
            set
            {
                if (_skin != value)
                {
                    if (value == null && _hook.IsFormAttached) 
                        _hook.DetachForm();
                    _skin = value;

                    if (_skin != null)
                    {
                        _skin.SkiningForm = _skiningForm;
                        if (!_hook.IsFormAttached) 
                        { 
                            _hook.AttachForm(); 
                        }    
                    }
                    if (_skiningForm != null && _skin != null) 
                        _skiningForm.Invalidate();
                }
            }
        }
        /// <summary>
        /// Gets or sets the form to be skinned.
        /// </summary>
        public Form SkiningForm
        {
            get { return _skiningForm; }
            set
            {
                if (_skiningForm != null && !IsDesignMode)
                    _skiningForm.Disposed -= Form_Disposed;

                _hook.DetachForm();
                _skiningForm = value;
                if (_skin != null)
                {
                    _skin.SkiningForm = _skiningForm;
                }

                if (_skiningForm != null && !IsDesignMode)
                {
                    if (_skin != null)
                        _hook.AttachForm();
                    _hook.Invalidate();
                }
            }
        }

        /// <summary>
        /// Redraws the non client area..
        /// </summary>
        public void Invalidate()
        {
             _hook.Invalidate();
        }

        #endregion
        #region Form Disposed event handler
        private void Form_Disposed(object sender, EventArgs e)
        {
            Dispose();
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_hook != null) _hook.DetachForm();
                if (_skin != null) _skin.Dispose();
            }
            base.Dispose(disposing);
        }
        internal static bool IsDesignMode { get; set; }
    }
}

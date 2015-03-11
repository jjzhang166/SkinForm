
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;


namespace Controls.SkinForm
{
    /// <summary>
    /// This class provides some methods which provide drawing specific data.
    /// </summary>
    public static class FormExtenders
    {
        /// <summary>
        /// Gets a value indicating if the maximize box needs to be drawn on the specified form.
        /// </summary>
        /// <param name="form">The form to check.</param>
        /// <returns></returns>
        public static bool IsDrawMaximizeBox(Form form)
        {
            return form.MaximizeBox && form.FormBorderStyle != FormBorderStyle.SizableToolWindow &&
                   form.FormBorderStyle != FormBorderStyle.FixedToolWindow;
        }
        /// <summary>
        /// Gets a value indicating if the minimize box needs to be drawn on the specified form.
        /// </summary>
        /// <param name="form">The form to check .</param>
        /// <returns></returns>
        public static bool IsDrawMinimizeBox(Form form)
        {
            return form.MinimizeBox && form.FormBorderStyle != FormBorderStyle.SizableToolWindow &&
                   form.FormBorderStyle != FormBorderStyle.FixedToolWindow;
        }

        /// <summary>
        /// Calculates the border size for the given form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        public static Size GetBorderSize(Form form)
        {
            Size border = new Size(0, 0);

            // Check for Caption
            Int32 style = Win32Api.GetWindowLong(form.Handle, GWLIndex.GWL_STYLE);
            bool caption = (style & (int)(WindowStyles.WS_CAPTION)) != 0;
            int factor = SystemInformation.BorderMultiplierFactor - 1;

            OperatingSystem system = Environment.OSVersion;
            bool isVista = system.Version.Major >= 6 && VisualStyleInformation.IsEnabledByUser;

            switch (form.FormBorderStyle)
            {
                case FormBorderStyle.FixedToolWindow:
                case FormBorderStyle.FixedSingle:
                case FormBorderStyle.FixedDialog:
                    border = SystemInformation.FixedFrameBorderSize;
                    break;
                case FormBorderStyle.SizableToolWindow:
                case FormBorderStyle.Sizable:
                    if (isVista)
                        border = SystemInformation.FrameBorderSize;
                    else
                        border = SystemInformation.FixedFrameBorderSize +
                            (caption ? SystemInformation.BorderSize + new Size(factor, factor)
                                : new Size(factor, factor));
                    break;
                case FormBorderStyle.Fixed3D:
                    border = SystemInformation.FixedFrameBorderSize + SystemInformation.Border3DSize;
                    break;
            }

            return border;
        }

        /// <summary>
        /// Gets the size for <see cref="CaptionButton"/> the given form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        public static Size GetCaptionButtonSize(Form form)
        {
            Size buttonSize = form.FormBorderStyle != FormBorderStyle.SizableToolWindow &&
                              form.FormBorderStyle != FormBorderStyle.FixedToolWindow
                                  ? SystemInformation.CaptionButtonSize
                                  : SystemInformation.ToolWindowCaptionButtonSize;
            // looks better with this height
            buttonSize.Height--;
            return buttonSize;

        }

        /// <summary>
        /// Gets the height of the caption.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        public static int GetCaptionHeight(Form form)
        {
            return form.FormBorderStyle != FormBorderStyle.SizableToolWindow &&
                   form.FormBorderStyle != FormBorderStyle.FixedToolWindow
                       ? SystemInformation.CaptionHeight + 2
                       : SystemInformation.ToolWindowCaptionHeight + 1;
        }

        /// <summary>
        /// Gets a value indicating whether the given form has a system menu.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        public static bool HasMenu(Form form)
        {
            return form.FormBorderStyle == FormBorderStyle.Sizable || form.FormBorderStyle == FormBorderStyle.Fixed3D ||
                    form.FormBorderStyle == FormBorderStyle.FixedSingle;
        }

        /// <summary>
        /// Gets the screen rect of the given form
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        public static Rectangle GetScreenRect(Form form)
        {
            return (form.Parent != null) ? form.Parent.RectangleToScreen(form.Bounds) : form.Bounds;
        }

        /// <summary>
        /// Gets the button paint indices.
        /// </summary>
        /// <param name="button">The button paint info.</param>
        /// <param name="active">A value indicating whether the button is active.</param>
        /// <param name="buttonIndex">Index of the button icon image strip.</param>
        /// <param name="rendererIndex">Index of the button background image strip.</param>
        public static void GetButtonData(CaptionButtonPaintData button, bool active, out int buttonIndex, out int rendererIndex)
        {
            
            if (!button.Enabled)
            {
                rendererIndex = -1;
                buttonIndex = 4;
            }
            else if (button.Pressed)
            {
                buttonIndex = active ? 2 : 3;
                rendererIndex = 1;
            }
            else if (button.Hovered)
            {
                buttonIndex = active ? 1 : 3;
                rendererIndex = 0;
            }
            else
            {
                buttonIndex = active ? 0 : 3;
                rendererIndex = -1;
            }
           
        }
    }
}

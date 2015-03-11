using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Resources;

namespace Controls.SkinForm
{

    sealed class SkinSetting
    {
        internal ResourceManager _resourceManager;
        internal ControlPaintHelper _formCaption;
        internal ControlPaintHelper _formBorder;

        internal ControlPaintHelper _formCaptionButton;
        internal ControlPaintHelper _formCaptionButtonSmall;

        internal ImageStrip _formCloseIcon;
        internal ImageStrip _formCloseIconSmall;

        internal ImageStrip _formRestoreIcon;
        internal ImageStrip _formRestoreIconSmall;

        internal ImageStrip _formMaximizeIcon;
        internal ImageStrip _formMaximizeIconSmall;

        internal ImageStrip _formMinimizeIcon;
        internal ImageStrip _formMinimizeIconSmall;

        internal Color _formActiveTitleColor;
        internal Color _formInactiveTitleColor;
        internal bool _formIsTextCentered;
        internal Size _cornerSize;

        internal SkinSetting(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            OnLoad();
        }
        internal SkinSetting(string skinConfigFile)
        {
            OnLoad(skinConfigFile);
        }

        private void OnLoad(string skinConfigFile)
        {
            try
            {
                SkinConfig skin = XmlHelper.ReadConfig<SkinConfig>(skinConfigFile);
                _formBorder = new ControlPaintHelper(PaintHelperData.Read(skin.Border, skin, "FormBorder"));
                _formCaption = new ControlPaintHelper(PaintHelperData.Read(skin.Caption.Backgroud, skin, "FormCaption"));
                Size imageSize = PaintHelperData.StringToSize(skin.Caption.NormalButton.IconSize);
                _formCloseIcon = new ImageStrip(true, imageSize, new Bitmap(skin.Images["CloseIcon"]));
                _formRestoreIcon = new ImageStrip(true, imageSize, new Bitmap(skin.Images["RestoreIcon"]));
                _formMaximizeIcon = new ImageStrip(true, imageSize, new Bitmap(skin.Images["MaximizeIcon"]));
                _formMinimizeIcon = new ImageStrip(true, imageSize, new Bitmap(skin.Images["MinimizeIcon"]));
                _formCaptionButton = new ControlPaintHelper(PaintHelperData.Read(skin.Caption.NormalButton, skin, "FormCaptionButton"));
                imageSize = PaintHelperData.StringToSize(skin.Caption.SmallButton.IconSize);
                _formCloseIconSmall = new ImageStrip(true, imageSize, new Bitmap(skin.Images["CloseIconSmall"]));
                _formRestoreIconSmall = new ImageStrip(true, imageSize, new Bitmap(skin.Images["RestoreIconSmall"]));
                _formMaximizeIconSmall = new ImageStrip(true, imageSize, new Bitmap(skin.Images["MaximizeIconSmall"]));
                _formMinimizeIconSmall = new ImageStrip(true, imageSize, new Bitmap(skin.Images["MinimizeIconSmall"]));
                _formCaptionButtonSmall = new ControlPaintHelper(PaintHelperData.Read(skin.Caption.SmallButton, skin, "FormCaptionButton"));
                _formActiveTitleColor = PaintHelperData.StringToColor(skin.ActiveCaption);
                _formInactiveTitleColor = PaintHelperData.StringToColor(skin.InactiveCation);
                _formIsTextCentered = skin.CenterCaption;
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException("Invalid SkinConfig XML", ex);
            }
            _cornerSize = new Size(9, 9);
        }
        /// <summary>
        /// Called when the skin is loaded.
        /// </summary>
        private void OnLoad()
        {
            if (_resourceManager == null) { return; }

            try
            {
                XmlDocument skinDef = new XmlDocument();
                skinDef.LoadXml(_resourceManager.GetString("SkinDefinition"));

                XmlElement elm = skinDef.DocumentElement;
                XmlNode form = elm["Form"];
                XmlNode captionNode = form["Caption"];
                XmlNode normalButton = captionNode["NormalButton"];
                XmlNode smallButton = captionNode["SmallButton"];

                //Background
                _formBorder = new ControlPaintHelper(PaintHelperData.Read(form["Border"], _resourceManager, "FormBorder"));
                _formCaption = new ControlPaintHelper(PaintHelperData.Read(captionNode["Background"], _resourceManager, "FormCaption"));

                //Big Buttons
                Size imageSize = PaintHelperData.StringToSize(normalButton["IconSize"].InnerText);

                _formCloseIcon = new ImageStrip(true, imageSize, (Bitmap)_resourceManager.GetObject("CloseIcon"));
                _formRestoreIcon = new ImageStrip(true, imageSize, (Bitmap)_resourceManager.GetObject("RestoreIcon"));
                _formMaximizeIcon = new ImageStrip(true, imageSize, (Bitmap)_resourceManager.GetObject("MaximizeIcon"));
                _formMinimizeIcon = new ImageStrip(true, imageSize, (Bitmap)_resourceManager.GetObject("MinimizeIcon"));
                _formCaptionButton = new ControlPaintHelper(PaintHelperData.Read(normalButton, _resourceManager, "FormCaptionButton"));

                //Small Buttons
                imageSize = PaintHelperData.StringToSize(smallButton["IconSize"].InnerText);

                _formCloseIconSmall = new ImageStrip(true, imageSize, (Bitmap)_resourceManager.GetObject("CloseIconSmall"));
                _formRestoreIconSmall = new ImageStrip(true, imageSize, (Bitmap)_resourceManager.GetObject("RestoreIconSmall"));
                _formMaximizeIconSmall = new ImageStrip(true, imageSize, (Bitmap)_resourceManager.GetObject("MaximizeIconSmall"));
                _formMinimizeIconSmall = new ImageStrip(true, imageSize, (Bitmap)_resourceManager.GetObject("MinimizeIconSmall"));
                _formCaptionButtonSmall = new ControlPaintHelper(PaintHelperData.Read(smallButton, _resourceManager, "FormCaptionButton"));

                //General Infos
                _formActiveTitleColor = PaintHelperData.StringToColor(form["ActiveCaption"].InnerText);
                _formInactiveTitleColor = PaintHelperData.StringToColor(form["InactiveCaption"].InnerText);
                _formIsTextCentered = PaintHelperData.StringToBool(form["CenterCaption"].InnerText);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Invalid SkinDefinition XML", e);
            }
        }
    }
    public sealed class CustomSkin : SkinBase
    {

        #region Fields

        #region private

        SkinSetting _skinSetting;

        #endregion


        #endregion

        #region public

        public CustomSkin(string skinConfigFile)
        {
            _skinSetting = new SkinSetting(skinConfigFile);
            CaptionButtons.Add(new CaptionButton(HitTest.HTCLOSE));
            CaptionButtons.Add(new CaptionButton(HitTest.HTMAXBUTTON));
            CaptionButtons.Add(new CaptionButton(HitTest.HTMINBUTTON));
            foreach (CaptionButton button in CaptionButtons)
                button.PropertyChanged += OnCommandButtonPropertyChanged;

        }



        #endregion
        protected internal override SkinningFormPaintData CreatePaintData()
        {

            // Create painting meta data for form
            SkinningFormPaintData paintData = new SkinningFormPaintData()
            {
                BaseData = base.CreatePaintDataBase(),
                Borders = SkinSystemInfo.BorderSize,
                CaptionHeight = SkinSystemInfo.CaptionHeight,
                //CaptionHeight =50,
                Active = FormIsActive,
                HasMenu = SkinSystemInfo.IsHasMenu,
                IconSize = SystemInformation.SmallIconSize,
                IsSmallCaption = false,
                Text = SkiningForm.Text
               
            };
            return paintData;
        }

        #region Overrides




        protected internal override bool OnMouseUp( Message m)
        {
            if (!IsProcessNcArea)
                return false;

            // do we have a pressed button?
            if (PressedButton != null)
            {
                // get button at wparam
                CaptionButton button = CommandButtonByHitTest((HitTest)m.WParam);
                if (button == null)
                    return false;

                if (button.Pressed)
                {
                    switch (button.HitTest)
                    {
                        case HitTest.HTCLOSE:
                            SkiningForm.Close();
                            return true;
                        case HitTest.HTMAXBUTTON:
                            if (SkiningForm.WindowState == FormWindowState.Maximized)
                            {
                                SkiningForm.WindowState = FormWindowState.Normal;
                                SkiningForm.Refresh();
                            }
                            else if (SkiningForm.WindowState == FormWindowState.Normal ||
                                     SkiningForm.WindowState == FormWindowState.Minimized)
                            {
                                SkiningForm.WindowState = FormWindowState.Maximized;
                            }
                            break;
                        case HitTest.HTMINBUTTON:
                            SkiningForm.WindowState = SkiningForm.WindowState == FormWindowState.Minimized
                                                          ? FormWindowState.Normal
                                                          : FormWindowState.Minimized;
                            break;

                    }
                }

                PressedButton.Pressed = false;
                PressedButton.Hovered = false;
                PressedButton = null;
            }
            return false;
        }


        protected internal override bool OnNcPaint(SkinningFormPaintData paintData)
        {
            if (SkiningForm == null) return false;

            bool isMaximized = SkiningForm.WindowState == FormWindowState.Maximized;
            bool isMinimized = SkiningForm.WindowState == FormWindowState.Minimized;

            // prepare bounds
            Rectangle windowBounds = paintData.BaseData.Bounds;
            windowBounds.Location = Point.Empty;

            Rectangle captionBounds = windowBounds;
            Size borderSize = paintData.Borders;
            captionBounds.Height = borderSize.Height + paintData.CaptionHeight;

            Rectangle textBounds = captionBounds;
            Rectangle iconBounds = captionBounds;
            iconBounds.Inflate(-borderSize.Width, 0);
            iconBounds.Y += borderSize.Height;
            iconBounds.Height -= borderSize.Height;

            // Draw Caption
            bool active = paintData.Active;
            _skinSetting._formCaption.Draw(paintData.BaseData.Graphics, captionBounds, active ? 0 : 1);

            // Paint Icon
            if (paintData.HasMenu && SkiningForm.Icon != null)
            {
                iconBounds.Size = paintData.IconSize;
                Icon tmpIcon = new Icon(SkiningForm.Icon, paintData.IconSize);
                iconBounds.Y = captionBounds.Y + (captionBounds.Height - iconBounds.Height) / 2;
                paintData.BaseData.Graphics.DrawIcon(tmpIcon, iconBounds);
                textBounds.X = iconBounds.Right;
                iconBounds.Width -= iconBounds.Right;
            }

            // Paint Icons
            foreach (CaptionButtonPaintData data in paintData.CaptionButtons)
            {
                ControlPaintHelper painter = paintData.IsSmallCaption ? _skinSetting._formCaptionButtonSmall : _skinSetting._formCaptionButton;

                // Get Indices for imagestrip
                int iconIndex = 0;
                int backgroundIndex = 0;
                FormExtenders.GetButtonData(data, paintData.Active, out iconIndex, out backgroundIndex);

                // get imageStrip for button icon
                ImageStrip iconStrip;
                switch (data.HitTest)
                {
                    case HitTest.HTCLOSE:
                        iconStrip = paintData.IsSmallCaption ? _skinSetting._formCloseIconSmall : _skinSetting._formCloseIcon;
                        break;
                    case HitTest.HTMAXBUTTON:
                        if (isMaximized)
                            iconStrip = paintData.IsSmallCaption ? _skinSetting._formRestoreIconSmall : _skinSetting._formRestoreIcon;
                        else
                            iconStrip = paintData.IsSmallCaption ? _skinSetting._formMaximizeIconSmall : _skinSetting._formMaximizeIcon;
                        break;
                    case HitTest.HTMINBUTTON:
                        if (isMinimized)
                            iconStrip = paintData.IsSmallCaption ? _skinSetting._formRestoreIconSmall : _skinSetting._formRestoreIcon;
                        else
                            iconStrip = paintData.IsSmallCaption ? _skinSetting._formMinimizeIconSmall : _skinSetting._formMinimizeIcon;
                        break;
                    default:
                        continue;
                }

                // draw background
                if (backgroundIndex >= 0)
                    painter.Draw(paintData.BaseData.Graphics, data.BaseData.Bounds, backgroundIndex);

                // draw Icon 
                Rectangle b = data.BaseData.Bounds;
                b.Y += 1;
                if (iconIndex >= 0)
                    iconStrip.Draw(paintData.BaseData.Graphics, iconIndex, b, Rectangle.Empty,
                                   DrawingAlign.Center, DrawingAlign.Center);
                // Ensure textbounds
                textBounds.Width -= data.BaseData.Bounds.Width;
            }

            // draw text
            if (!string.IsNullOrEmpty(paintData.Text) && !textBounds.IsEmpty)
            {
                TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.PreserveGraphicsClipping;
                if (_skinSetting._formIsTextCentered)
                    flags = flags | TextFormatFlags.HorizontalCenter;
                Font font = paintData.IsSmallCaption ? SystemFonts.SmallCaptionFont : SystemFonts.CaptionFont;
                TextRenderer.DrawText(paintData.BaseData.Graphics, paintData.Text, font, textBounds,
                    paintData.Active ? _skinSetting._formActiveTitleColor : _skinSetting._formInactiveTitleColor, flags);
            }

            // exclude caption area from painting
            Region region = paintData.BaseData.Graphics.Clip;
            region.Exclude(captionBounds);
            paintData.BaseData.Graphics.Clip = region;

            // Paint borders and corners
            _skinSetting._formBorder.DrawFrame(paintData.BaseData.Graphics, windowBounds, paintData.Active ? 0 : 1);

            paintData.BaseData.Graphics.ResetClip();
            return true;
        }

        #endregion
    }

    
}

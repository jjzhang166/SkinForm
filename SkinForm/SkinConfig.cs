using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Controls.SkinForm
{
    public interface ISkinConfig
    {
        string ImageSize
        {
            get;
            set;
        }
        string ImagePadding
        {
            get;
            set;
        }
    }

    public class SkinConfig
    {
        public SerializableDictionary<string, string> Images = new SerializableDictionary<string, string>();

        string _activeCaption;
        public string ActiveCaption
        {
            get { return _activeCaption; }
            set { _activeCaption = value; }
        }

        string _inactiveCation;
        public string InactiveCation
        {
            get { return _inactiveCation; }
            set { _inactiveCation = value; }
        }

        bool _centerCaption;
        public bool CenterCaption
        {
            get { return _centerCaption; }
            set { _centerCaption = value; }
        }

        CaptionConfig _caption;
        public CaptionConfig Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        BorderConfig _border;
        public BorderConfig Border
        {
            get { return _border; }
            set { _border = value; }
        }
    }

    public class BackgroudConfig : ISkinConfig
    {
        string _imageSize;

        public string ImageSize
        {
            get { return _imageSize; }
            set { _imageSize = value; }
        }
        string _imagePadding;

        public string ImagePadding
        {
            get { return _imagePadding; }
            set { _imagePadding = value; }
        }
    }

    public class NormalButtonConfig : ISkinConfig
    {
        string _iconSize;

        public string IconSize
        {
            get { return _iconSize; }
            set { _iconSize = value; }
        }
        string _imageSize;

        public string ImageSize
        {
            get { return _imageSize; }
            set { _imageSize = value; }
        }
        string _imagePadding;

        public string ImagePadding
        {
            get { return _imagePadding; }
            set { _imagePadding = value; }
        }
    }

    public class SmallButtonConfig : ISkinConfig
    {
        string _iconSize;

        public string IconSize
        {
            get { return _iconSize; }
            set { _iconSize = value; }
        }
        string _imageSize;

        public string ImageSize
        {
            get { return _imageSize; }
            set { _imageSize = value; }
        }
        string _imagePadding;

        public string ImagePadding
        {
            get { return _imagePadding; }
            set { _imagePadding = value; }
        }
    }

    public class CaptionConfig
    {
        BackgroudConfig _backgroud;
        public BackgroudConfig Backgroud
        {
            get { return _backgroud; }
            set { _backgroud = value; }
        }
        NormalButtonConfig _normalButton;

        public NormalButtonConfig NormalButton
        {
            get { return _normalButton; }
            set { _normalButton = value; }
        }
        SmallButtonConfig _smallButton;

        public SmallButtonConfig SmallButton
        {
            get { return _smallButton; }
            set { _smallButton = value; }
        }
    }

    public class BorderConfig : ISkinConfig
    {
        string _imageSize;
        public string ImageSize
        {
            get { return _imageSize; }
            set { _imageSize = value; }
        }
        string _imagePadding;
        public string ImagePadding
        {
            get { return _imagePadding; }
            set { _imagePadding = value; }
        }
    }
}

/*
 * The usage of wrappers which contain all needed painting information,
 * prevents the access to internal used classes and properties.
 */
using System.Drawing;

namespace Controls.SkinForm
{
    /// <summary>
    /// The base class for painting definitions.
    /// </summary>
    public class PaintDataBase
    {
        /// <summary>
        /// Gets or sets the bounds to draw into.
        /// </summary>
        /// <value>The bounds to draw into.</value>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Gets or sets the graphics to draw into.
        /// </summary>
        /// <value>The graphics to draw into.</value>
        public Graphics Graphics { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintDataBase"/> class.
        /// </summary>
        /// <param name="g">The graphics.</param>
        /// <param name="bounds">The bounds.</param>
        public PaintDataBase(Graphics g, Rectangle bounds)
        {
            Graphics = g;
            Bounds = bounds;
        }
        public PaintDataBase()
        {

        }
    }

    /// <summary>
    /// Contains the paint data for caption buttons
    /// </summary>
    public class CaptionButtonPaintData //: PaintDataBase
    {
        public PaintDataBase BaseData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the button is pressed.
        /// </summary>
        /// <value><c>true</c> if pressed; otherwise, <c>false</c>.</value>
        public bool Pressed { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the button is hovered.
        /// </summary>
        /// <value><c>true</c> if hovered; otherwise, <c>false</c>.</value>
        public bool Hovered { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the button is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the hit test result of the button to determine the button icon.
        /// </summary>
        /// <value>The hit test result.</value>
        public HitTest HitTest { get; set; }

        public CaptionButtonPaintData()
        {

        }
    }

    /// <summary>
    /// Contains the paint data for the non client area of the skinned form. 
    /// </summary>
    public class SkinningFormPaintData  
    {

        public PaintDataBase BaseData { get; set; }

        /// <summary>
        /// Gets or sets the list of painting definitions for caption buttons.
        /// </summary>
        /// <value>The caption button paint definitions.</value>
        public CaptionButtonPaintData[] CaptionButtons { get; set; }

        /// <summary>
        /// Gets or sets the form border size.
        /// </summary>
        /// <value>The form border size.</value>
        public Size Borders { get; set; }

        /// <summary>
        /// Gets or sets the size of the icon.
        /// </summary>
        /// <value>The size of the icon.</value>
        public Size IconSize { get; set; }

        /// <summary>
        /// Gets or sets the height of the caption.
        /// </summary>
        /// <value>The height of the caption.</value>
        public int CaptionHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the form is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the form has a system menu.
        /// </summary>
        /// <value><c>true</c> if this instance has menu; otherwise, <c>false</c>.</value>
        public bool HasMenu { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the form as a small caption. (ToolWindows)
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is small caption; otherwise, <c>false</c>.
        /// </value>
        public bool IsSmallCaption { get; set; }

        /// <summary>
        /// Gets or sets the caption text.
        /// </summary>
        /// <value>The caption text.</value>
        public string Text { get; set; }

        public SkinningFormPaintData() { }
    }
}

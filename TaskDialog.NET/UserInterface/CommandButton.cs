using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TaskDialogNet.UserInterface {
  public partial class CommandButton : Button {
    //--------------------------------------------------------------------------------

    #region PRIVATE MEMBERS

    //--------------------------------------------------------------------------------
    private Image ImgArrow1 { get; set; }
    private Image ImgArrow2 { get; set; }

    private const int LeftMargin  = 10;
    private const int TopMargin   = 10;
    private const int ArrowWidth  = 19;

    private enum ButtonState {
      Normal,
      MouseOver,
      Down
    }

    private ButtonState _state = ButtonState.Normal;

    #endregion

    //--------------------------------------------------------------------------------

    #region PUBLIC PROPERTIES

    //--------------------------------------------------------------------------------
    // Override this to make sure the control is invalidated (repainted) when 'Text' is changed
    public override string Text {
      get { return base.Text; }
      set {
        base.Text = value;
        if( _autoHeight )
          Height = GetBestHeight();
        Invalidate();
      }
    }

    // SmallFont is the font used for secondary lines
    public Font SmallFont { get; set; }

    // AutoHeight determines whether the button automatically resizes itself to fit the Text
    private bool _autoHeight = true;

    [Browsable( true )]
    [Category( "Behavior" )]
    [DefaultValue( true )]
    public bool AutoHeight {
      get { return _autoHeight; }
      set {
        _autoHeight = value;
        if( _autoHeight ) Invalidate();
      }
    }

    #endregion

    //--------------------------------------------------------------------------------

    #region CONSTRUCTOR

    //--------------------------------------------------------------------------------
    public CommandButton() {
      InitializeComponent();
      base.Font = new Font( "Arial", 11.75F, FontStyle.Regular, GraphicsUnit.Point, 0 );
      SmallFont = new Font( "Arial", 8F, FontStyle.Regular, GraphicsUnit.Point, 0 );
    }

    #endregion

    //--------------------------------------------------------------------------------

    #region PUBLIC ROUTINES

    //--------------------------------------------------------------------------------
    public int GetBestHeight() {
      return ( TopMargin * 2 ) + (int) GetSmallTextSizeF().Height + (int) GetLargeTextSizeF().Height;
    }

    #endregion

    //--------------------------------------------------------------------------------

    #region PRIVATE ROUTINES

    //--------------------------------------------------------------------------------
    private string GetLargeText() {
      string[] lines = Text.Split( new[] {'\n'} );
      return lines[ 0 ];
    }

    private string GetSmallText() {
      if( Text.IndexOf( '\n' ) < 0 )
        return "";

      string s = Text;
      string[] lines = s.Split( new[] {'\n'} );
      s = "";
      for( int i = 1; i < lines.Length; i++ )
        s += lines[ i ] + "\n";
      return s.Trim( new[] {'\n'} );
    }

    private SizeF GetLargeTextSizeF() {
      const int x         = LeftMargin + ArrowWidth + 5;
      SizeF     mzSize    = new SizeF( Width - x - LeftMargin, 5000.0F ); // presume RIGHT_MARGIN = LEFT_MARGIN
      Graphics  g         = Graphics.FromHwnd( Handle );
      SizeF     textSize  = g.MeasureString( GetLargeText(), base.Font, mzSize );
      return textSize;
    }

    private SizeF GetSmallTextSizeF() {
      string s = GetSmallText();
      if( s == "" ) return new SizeF( 0, 0 );
      const int x         = LeftMargin + ArrowWidth + 8;
      SizeF     mzSize    = new SizeF( Width - x - LeftMargin, 5000.0F ); // presume RIGHT_MARGIN = LEFT_MARGIN
      Graphics  g         = Graphics.FromHwnd( Handle );
      SizeF     textSize  = g.MeasureString( s, SmallFont, mzSize );
      return textSize;
    }

    #endregion

    //--------------------------------------------------------------------------------

    #region OVERRIDEs

    //--------------------------------------------------------------------------------
    protected override void OnCreateControl() {
      base.OnCreateControl();
      ImgArrow1 = Resources.green_arrow1;
      ImgArrow2 = Resources.green_arrow2;
    }

    //--------------------------------------------------------------------------------
    protected override void OnPaint( PaintEventArgs e ) {
      e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
      e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

      LinearGradientBrush brush;
      const LinearGradientMode mode = LinearGradientMode.Vertical;

      Rectangle newRect = new Rectangle( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1,
                                         ClientRectangle.Height - 1 );
      Color textColor = SystemColors.WindowText;

      Image img = ImgArrow1;

      if( Enabled ) {
        switch( _state ) {
          case ButtonState.Normal:
            e.Graphics.FillRectangle( Brushes.White, newRect );
            e.Graphics.DrawRectangle( base.Focused ? new Pen( Color.SkyBlue, 1 ) : new Pen( Color.White, 1 ), newRect );
            textColor = Color.DarkBlue;
            break;

          case ButtonState.MouseOver:
            brush = new LinearGradientBrush( newRect, Color.White, Color.WhiteSmoke, mode );
            e.Graphics.FillRectangle( brush, newRect );
            e.Graphics.DrawRectangle( new Pen( Color.Silver, 1 ), newRect );
            img = ImgArrow2;
            textColor = Color.Blue;
            break;

          case ButtonState.Down:
            brush = new LinearGradientBrush( newRect, Color.WhiteSmoke, Color.White, mode );
            e.Graphics.FillRectangle( brush, newRect );
            e.Graphics.DrawRectangle( new Pen( Color.DarkGray, 1 ), newRect );
            textColor = Color.DarkBlue;
            break;
        }
      } else {
        brush = new LinearGradientBrush( newRect, Color.WhiteSmoke, Color.Gainsboro, mode );
        e.Graphics.FillRectangle( brush, newRect );
        e.Graphics.DrawRectangle( new Pen( Color.DarkGray, 1 ), newRect );
        textColor = Color.DarkBlue;
      }

      string largetext = GetLargeText();
      string smalltext = GetSmallText();

      SizeF szL = GetLargeTextSizeF();
      //e.Graphics.DrawString(largetext, base.Font, new SolidBrush(text_color), new RectangleF(new PointF(LEFT_MARGIN + imgArrow1.Width + 5, TOP_MARGIN), szL));
      TextRenderer.DrawText( e.Graphics, largetext, base.Font,
                             new Rectangle( LeftMargin + ImgArrow1.Width + 5, TopMargin, (int) szL.Width,
                                            (int) szL.Height ), textColor, TextFormatFlags.Default );

      if( smalltext != "" ) {
        SizeF szS = GetSmallTextSizeF();
        e.Graphics.DrawString( smalltext, SmallFont, new SolidBrush( textColor ),
                               new RectangleF(
                                 new PointF( LeftMargin + ImgArrow1.Width + 8, TopMargin + (int) szL.Height ), szS ) );
      }

      e.Graphics.DrawImage( img, new Point( LeftMargin, TopMargin + (int) ( szL.Height / 2 ) - img.Height / 2 ) );
    }

    //--------------------------------------------------------------------------------
    protected override void OnMouseLeave( EventArgs e ) {
      _state = ButtonState.Normal;
      Invalidate();
      base.OnMouseLeave( e );
    }

    //--------------------------------------------------------------------------------
    protected override void OnMouseEnter( EventArgs e ) {
      _state = ButtonState.MouseOver;
      Invalidate();
      base.OnMouseEnter( e );
    }

    //--------------------------------------------------------------------------------
    protected override void OnMouseUp( MouseEventArgs e ) {
      _state = ButtonState.MouseOver;
      Invalidate();
      base.OnMouseUp( e );
    }

    //--------------------------------------------------------------------------------
    protected override void OnMouseDown( MouseEventArgs e ) {
      _state = ButtonState.Down;
      Invalidate();
      base.OnMouseDown( e );
    }

    //--------------------------------------------------------------------------------
    protected override void OnSizeChanged( EventArgs e ) {
      if( _autoHeight ) {
        int h = GetBestHeight();
        if( Height != h ) {
          Height = h;
          return;
        }
      }
      base.OnSizeChanged( e );
    }

    #endregion

    //--------------------------------------------------------------------------------
  }
}
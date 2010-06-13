using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TaskDialog.UserInterface {
  public partial class EmulatedTaskDialog : Form, ITaskDialog {

    #region Events
    public event TaskDialogEventHandler<ButtonClickedArgs> ButtonClicked;
    public new event TaskDialogEventHandler Created;
    public event TaskDialogEventHandler Destroyed;
    public event TaskDialogEventHandler DialogConstructed;
    public event TaskDialogEventHandler ExpandoButtonClicked;
    public event TaskDialogEventHandler Help;
    public event TaskDialogEventHandler<HyperlinkClickedArgs> HyperlinkClicked;
    public event TaskDialogEventHandler Navigated;
    public event TaskDialogEventHandler<ButtonClickedArgs> RadioButtonClicked;
    public event TaskDialogEventHandler<TimerArgs> Timer;
    public event TaskDialogEventHandler VerificationClicked;
    #endregion

    #region Properties
    #region Flags
    public bool EnableHyperlinks { get; set; }
    public bool AllowDialogCancellation { get; set; }
    public bool UseCommandLinks { get; set; }
    public bool UseCommandLinksNoIcon { get; set; }
    public bool ExpandFooterArea { get; set; }
    public bool ExpandedByDefault { get; set; }
    public bool VerificationFlagChecked { get; set; }
    public bool ShowProgressBar { get; set; }
    public bool ShowMarqueeProgressBar { get; set; }
    public bool CallbackTimer { get; set; }
    public bool PositionRelativeToWindow { get; set; }
    public bool RtlLayout { get; set; }
    public bool NoDefaultRadioButton { get; set; }
    public bool CanBeMinimized { get; set; }
    #endregion

    public CommonButtons CommonButtons { get; set; }
    
    public string WindowTitle {
      get { return Name; }
      set { Name = value; }
    }

    public Icon MainIcon { get; set; }
    public System.Drawing.Icon CustomMainIcon { get; set; }
    public string MainInstruction { get; set; }
    
    public string Content {
      get { return contentText.Text; }
      set { contentText.Text = value; }
    }

    public List<TaskDialogButton> Buttons { get; set; }
    public int DefaultButton { get; set; }
    public List<TaskDialogButton> RadioButtons { get; set; }
    public int DefaultRadioButton { get; set; }

    public string VerificationText {
      get { return verifyCheckBox.Text; }
      set { verifyCheckBox.Text = value; }
    }

    public string ExpandedInformation {
      get { return expandedInfoText.Text; }
      set { expandedInfoText.Text = value; }
    }

    public string ExpandedControlText { get; set; }
    public string CollapsedControlText { get; set; }
    public Icon FooterIcon { get; set; }
    public System.Drawing.Icon CustomFooterIcon { get; set; }
    public string Footer { get; set; }
    //public uint Width { get; set; }
    #endregion

    #region Private properties
    private bool IsExpanded { get; set; }
    private Control FocusControl { get; set; }
    private DateTime StartTime { get; set; }

    private int ProgressBarSpeed { get; set; }
    private ProgressBarState ProgressBarState { get; set; }

    private Color _mainInstructionColor = Color.DarkBlue;
    private readonly Font _mainInstructionFont = new Font(
      "Arial",
      11.75F,
      FontStyle.Regular,
      GraphicsUnit.Point,
      0 );
    private int _mainInstructionHeight;
    private int _commandButtonClicked = -1;
    private int _radioButtonClicked = -1;
    private bool _verificationChecked;
    private CommonButtons _cancelButtonId = CommonButtons.None;
    private CommonButtons _acceptButtonId = CommonButtons.None;

    private bool _formBuilt;
    private readonly bool _isVista;
    #endregion



    #region Constructor
    public EmulatedTaskDialog( bool useToolWindowOnXp ) {
      InitializeComponent();

      _isVista = NativeTaskDialog.IsAvailableOnThisOs;
      if( !_isVista && useToolWindowOnXp ) // <- shall we use the smaller toolbar?
        FormBorderStyle = FormBorderStyle.FixedToolWindow;

      Reset();
    }
    #endregion

    #region Methods
    #region Public methods
    /// <summary>
    /// Resets the Task Dialog to the state when first constructed, all properties set to their default value.
    /// </summary>
    private void Reset( ) {
      EnableHyperlinks          = false;
      AllowDialogCancellation   = false;
      UseCommandLinks           = false;
      UseCommandLinksNoIcon     = false;
      ExpandFooterArea          = false;
      ExpandedByDefault         = false;
      VerificationFlagChecked   = false;
      ShowProgressBar           = false;
      ShowMarqueeProgressBar    = false;
      CallbackTimer             = false;
      PositionRelativeToWindow  = false;
      RtlLayout                 = false;
      NoDefaultRadioButton      = false;
      CanBeMinimized            = false;

      CommonButtons             = CommonButtons.None;
      WindowTitle               = null;
      MainIcon                  = global::TaskDialog.UserInterface.Icon.None;
      CustomMainIcon            = null;
      MainInstruction           = null;
      Content                   = null;
      Buttons                   = new List<TaskDialogButton>();
      DefaultButton             = 0;
      RadioButtons              = new List<TaskDialogButton>();
      DefaultRadioButton        = 0;
      VerificationText          = null;
      ExpandedInformation       = null;
      ExpandedControlText       = null;
      CollapsedControlText      = null;
      FooterIcon                = global::TaskDialog.UserInterface.Icon.None;
      CustomFooterIcon          = null;
      Footer                    = null;
      Width                     = 460;

      IsExpanded                = ExpandedByDefault;
      FocusControl              = null;
      StartTime                 = DateTime.Now;
      ProgressBarSpeed          = 30;
      ProgressBarState          = ProgressBarState.Normal;
      _verificationChecked      = VerificationFlagChecked;
    }

    public int Show( IWin32Window owner, out bool verificationFlagChecked, out int radioButtonResult ) {
      BuildForm();

      StartTime = DateTime.Now;

      ShowDialog();

      verificationFlagChecked = _verificationChecked;
      radioButtonResult       = _radioButtonClicked;
      return _commandButtonClicked;
    }
    #endregion

    #region BuildForm

    private void DrawInstructionBackground( Color leftColor, Color rightColor ) {
      Bitmap gradientBitmap = new Bitmap( pnlMainInstruction.Width, pnlMainInstruction.Height );
      Graphics graphics = Graphics.FromImage( gradientBitmap );
      LinearGradientBrush brush = new LinearGradientBrush( new Point( 0, 0 ),
                                                           new Point( pnlMainInstruction.Width,
                                                                      0 ),
                                                           leftColor, rightColor );
      graphics.FillRectangle( brush, 0, 0, pnlMainInstruction.Width, pnlMainInstruction.Height );
      pnlMainInstruction.BackgroundImage = gradientBitmap;
    }

    private void BuildForm() {
      // Setup Content
      if( !string.IsNullOrEmpty( Content ) ) {
        contentText.ConvertLinks();
        contentText.ReadOnly  = true;
        contentText.BackColor = BackColor;
      }

      // Setup Expanded Info and Buttons panels
      if( !string.IsNullOrEmpty( ExpandedInformation ) ) {
        expandedInfoText.ConvertLinks();
        expandedInfoText.ReadOnly   = true;
        expandedInfoText.BackColor  = BackColor;
      }

      progressBar.Minimum = 0;
      progressBar.Maximum = 100;

      // Setup RadioButtons
      if( RadioButtons.Count > 0 ) {
        foreach( TaskDialogButton t in RadioButtons ) {
          RadioButton radioButton = new RadioButton {
            Parent  = pnlRadioButtons,
            Text    = t.ButtonText,
            Tag     = t.ButtonId,
            Checked = ( DefaultRadioButton == t.ButtonId )
          };

          if( radioButton.Checked ) RadioButtonClick( radioButton, EventArgs.Empty );

          radioButton.Click += RadioButtonClick;
        }
      }

      // Setup CommandButtons
      if( Buttons.Count >= 0 ) {
        foreach( TaskDialogButton button in Buttons ) {
          CommandButton commandButton = new CommandButton { Parent = pnlCommandButtons };
          if( _isVista ) {
            commandButton.Font = new Font( commandButton.Font, FontStyle.Regular );
          }
          commandButton.Text  = button.ButtonText;
          commandButton.Tag   = button.ButtonId;
          commandButton.Click += CommandButton_Click;
          if( button.ButtonId == DefaultButton ) {
            FocusControl = commandButton;
          }
        }
      }

      // Setup common buttons
      if( CommonButtons == CommonButtons.None ) {
        CommonButtons = CommonButtons.Ok;
      }

      if( ( CommonButtons & CommonButtons.Yes ) > 0 ) {
        Button button = new Button();
        commonButtonPanel.Controls.Add( button );

        button.Text     = "&Yes";
        button.Tag      = CommonButtons.Yes;
        _acceptButtonId  = CommonButtons.Yes;
      }
      if( ( CommonButtons & CommonButtons.No ) > 0 ) {
        Button button = new Button();
        commonButtonPanel.Controls.Add( button );

        button.Text     = "&No";
        button.Tag      = CommonButtons.No;
        _cancelButtonId  = CommonButtons.No;
      }
      if( ( CommonButtons & CommonButtons.Ok ) > 0 ) {
        Button button = new Button();
        commonButtonPanel.Controls.Add( button );

        button.Text     = "&Ok";
        button.Tag      = CommonButtons.Ok;
        _acceptButtonId  = CommonButtons.Ok;
      }
      if( ( CommonButtons & CommonButtons.Cancel ) > 0 ) {
        Button button = new Button();
        commonButtonPanel.Controls.Add( button );

        button.Text     = "&Cancel";
        button.Tag      = CommonButtons.Cancel;
        _cancelButtonId  = CommonButtons.Cancel;
      }
      if( ( CommonButtons & CommonButtons.Close ) > 0 ) {
        Button button = new Button();
        commonButtonPanel.Controls.Add( button );

        button.Text     = "C&lose";
        button.Tag      = CommonButtons.Close;
        _cancelButtonId  = CommonButtons.Close;
      }
      if( ( CommonButtons & CommonButtons.Retry ) > 0 ) {
        Button button = new Button();
        commonButtonPanel.Controls.Add( button );

        button.Text = "&Retry";
        button.Tag  = CommonButtons.Retry;
      }
      foreach( Button button in commonButtonPanel.Controls ) {
        button.UseVisualStyleBackColor = true;
        button.Click += CommonButtonClick;
      }

      ControlBox = ( ( CommonButtons & CommonButtons.Cancel ) > 0 ||
                     ( CommonButtons & CommonButtons.Close  ) > 0 );
      
      MinimizeBox = CanBeMinimized;

      if( !string.IsNullOrEmpty( Footer ) ) {
        if( EnableHyperlinks ) {
          footerText.ConvertLinks();
        }
        footerText.ReadOnly = true;
        footerText.BackColor = pnlFooter.BackColor;
      }

      ReCalculateLayout();

      _formBuilt = true;

      if( null != DialogConstructed ) DialogConstructed( this, EventArgs.Empty );
    }

    /// <summary>
    /// Recalculates the layout of all elements in the dialog.
    /// </summary>
    private void ReCalculateLayout() {
      int formHeight = 0;
      int mainInstructionMinSize = 41;

      // Setup Main Instruction
      switch( MainIcon ) {
        case global::TaskDialog.UserInterface.Icon.Information:
          imgMain.Image = SystemIcons.Information.ToBitmap();
          contentText.Top = 0;
          break;

        case global::TaskDialog.UserInterface.Icon.Warning:
          imgMain.Image = SystemIcons.Warning.ToBitmap();
          contentText.Top = 0;
          break;

        case global::TaskDialog.UserInterface.Icon.Error:
          imgMain.Image = SystemIcons.Error.ToBitmap();
          contentText.Top = 0;
          break;

        case global::TaskDialog.UserInterface.Icon.SecurityError:
          imgMain.Image = SecurityIcons.Images[ "error" ];
          mainInstructionMinSize += 8;
          contentText.Top = 4;
          DrawInstructionBackground( Color.FromArgb( 172, 1, 0 ), Color.FromArgb( 227, 1, 0 ) );
          _mainInstructionColor = Color.White;
          break;

        case global::TaskDialog.UserInterface.Icon.SecurityShield:
          imgMain.Image = SecurityIcons.Images[ "shield" ];
          mainInstructionMinSize += 8;
          contentText.Top = 4;
          break;

        case global::TaskDialog.UserInterface.Icon.SecurityShieldBlue:
          imgMain.Image = SecurityIcons.Images[ "shield" ];
          mainInstructionMinSize += 8;
          contentText.Top = 4;
          DrawInstructionBackground( Color.FromArgb( 4, 80, 130 ), Color.FromArgb( 28, 120, 133 ) );
          _mainInstructionColor = Color.White;
          break;

        case global::TaskDialog.UserInterface.Icon.SecurityShieldGray:
          imgMain.Image = SecurityIcons.Images[ "shield" ];
          mainInstructionMinSize += 8;
          contentText.Top = 4;
          DrawInstructionBackground( Color.FromArgb( 157, 143, 133 ), Color.FromArgb( 164, 152, 144 ) );
          _mainInstructionColor = Color.White;
          break;

        case global::TaskDialog.UserInterface.Icon.SecuritySuccess:
          imgMain.Image = SecurityIcons.Images[ "success" ];
          mainInstructionMinSize += 8;
          contentText.Top = 4;
          DrawInstructionBackground( Color.FromArgb( 21, 118, 21 ), Color.FromArgb( 57, 150, 63 ) );
          _mainInstructionColor = Color.White;
          break;

        case global::TaskDialog.UserInterface.Icon.SecurityWarning:
          imgMain.Image = SecurityIcons.Images[ "warning" ];
          mainInstructionMinSize += 8;
          contentText.Top = 4;
          DrawInstructionBackground( Color.FromArgb( 242, 177, 0 ), Color.FromArgb( 242, 177, 0 ) );
          _mainInstructionColor = Color.Black;
          break;
      }

      //AdjustLabelHeight(lbMainInstruction);
      //pnlMainInstruction.Height = Math.Max(41, lbMainInstruction.Height + 16);
      if( _mainInstructionHeight == 0 )
        GetMainInstructionTextSizeF();
      pnlMainInstruction.Height = Math.Max( mainInstructionMinSize, _mainInstructionHeight + 16 );

      formHeight += pnlMainInstruction.Height;

      // Setup Content
      pnlContent.Visible = !string.IsNullOrEmpty( Content );
      if( !string.IsNullOrEmpty( Content ) ) {
        AdjustLabelHeight( contentText );
        pnlContent.Height = contentText.Height + 4;
        formHeight += pnlContent.Height;
      }

      bool showVerifyCheckbox = !string.IsNullOrEmpty( verifyCheckBox.Text );
      verifyCheckBox.Visible = showVerifyCheckbox;

      // Setup Expanded Info and Buttons panels
      if( string.IsNullOrEmpty( ExpandedInformation ) ) {
        pnlExpandedInfo.Visible = false;
        showHideDetails.Visible = false;
        verifyCheckBox.Top = 12;
        pnlButtons.Height = 40;

      } else {
        AdjustLabelHeight( expandedInfoText );
        pnlExpandedInfo.Height        = expandedInfoText.Height + 4;
        pnlExpandedInfo.Visible       = IsExpanded;
        showHideDetails.Text        = ( IsExpanded ? String.Format( "        {0}", ExpandedControlText ) : String.Format( "        {0}", CollapsedControlText ) );
        showHideDetails.ImageIndex  = ( IsExpanded ? 0 : 3 );
        if( !showVerifyCheckbox ) {
          pnlButtons.Height = 40;
        }
        if( IsExpanded ) {
          formHeight += pnlExpandedInfo.Height;
        }
      }

      // Setup progress bar
      progressBarPanel.Visible = ( ShowProgressBar || ShowMarqueeProgressBar );
      if( ShowProgressBar || ShowMarqueeProgressBar ) {
        progressBar.Width = expandedInfoText.Width;
        formHeight += progressBarPanel.Height;
      }

      // Setup RadioButtons
      pnlRadioButtons.Visible = ( RadioButtons.Count > 0 );
      if( RadioButtons.Count > 0 ) {
        int pnlHeight = 12;
        int lastHeight = 0;
        foreach( RadioButton radioButton in pnlRadioButtons.Controls.OfType<RadioButton>().OrderBy( r => r.Tag ) ) {
          radioButton.Location = new Point( 60, 4 + lastHeight );
          radioButton.Width    = Width - radioButton.Left - 15;
          pnlHeight   += radioButton.Height;
          lastHeight  += radioButton.Height;
        }
        pnlRadioButtons.Height = pnlHeight;
        formHeight += pnlRadioButtons.Height;
      }

      // Setup CommandButtons
      pnlCommandButtons.Visible = ( Buttons.Count >= 0 );
      if( Buttons.Count >= 0 ) {
        int t = 8;
        int pnlHeight = 16;
        foreach( CommandButton commandButton in pnlCommandButtons.Controls.OfType<CommandButton>().OrderBy( b => b.Tag ) ) {
          commandButton.Location = new Point( 50, t );
          if( _isVista ) {
            commandButton.Font = new Font( commandButton.Font, FontStyle.Regular );
          }
          commandButton.Size = new Size( Width - commandButton.Left - 15, commandButton.GetBestHeight() );
          t         += commandButton.Height;
          pnlHeight += commandButton.Height;
        }
        pnlCommandButtons.Height = pnlHeight;
        formHeight += pnlCommandButtons.Height;
      }

      // Setup common buttons
      pnlButtons.Height = Math.Max( pnlButtons.Height, commonButtonPanel.Top * 2 + commonButtonPanel.Height );

      // Adjust verify checkbox
      if( showVerifyCheckbox ) {
        int       desiredWidth      = commonButtonPanel.Left - verifyCheckBox.Left;
        SizeF     verifyTextBounds  = new SizeF( desiredWidth, 5000.0F );
        Graphics  g                 = Graphics.FromHwnd( verifyCheckBox.Handle );
        SizeF     textSize          = g.MeasureString( verifyCheckBox.Text, verifyCheckBox.Font, verifyTextBounds );

        verifyCheckBox.AutoSize = false;
        verifyCheckBox.Width    = desiredWidth;
        verifyCheckBox.Height   = (int)textSize.Height + 10;
      }

      if( !showVerifyCheckbox && string.IsNullOrEmpty( ExpandedInformation ) && CommonButtons == CommonButtons.None ) {
        pnlButtons.Visible = false;

      } else {
        formHeight += pnlButtons.Height;
      }

      pnlFooter.Visible = !string.IsNullOrEmpty( Footer );
      if( !string.IsNullOrEmpty( Footer ) ) {
        AdjustLabelHeight( footerText );
        pnlFooter.Height = Math.Max( 28, footerText.Height + 16 );
        switch( FooterIcon ) {
          case global::TaskDialog.UserInterface.Icon.Information:
            imgFooter.Image = ResizeBitmap( SystemIcons.Information.ToBitmap(), 16, 16 );
            break;
            
          case global::TaskDialog.UserInterface.Icon.Warning:
            imgFooter.Image = ResizeBitmap( SystemIcons.Warning.ToBitmap(), 16, 16 );
            break;

          case global::TaskDialog.UserInterface.Icon.Error:
            imgFooter.Image = ResizeBitmap( SystemIcons.Error.ToBitmap(), 16, 16 );
            break;

          default:
            throw new ArgumentException( "Not yet implemented.", "FooterIcon" );
        }
        formHeight += pnlFooter.Height;
      }

      ClientSize = new Size( ClientSize.Width, formHeight );
    }

    //--------------------------------------------------------------------------------
    private static Image ResizeBitmap( Image srcImg, int newWidth, int newHeight ) {
      float percentWidth = ( newWidth / (float) srcImg.Width );
      float percentHeight = ( newHeight / (float) srcImg.Height );

      float resizePercent = ( percentHeight < percentWidth ? percentHeight : percentWidth );

      int w = (int) ( srcImg.Width * resizePercent );
      int h = (int) ( srcImg.Height * resizePercent );
      Bitmap b = new Bitmap( w, h );

      using( Graphics g = Graphics.FromImage( b ) ) {
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.DrawImage( srcImg, 0, 0, w, h );
      }
      return b;
    }

    //--------------------------------------------------------------------------------
    // utility function for setting a Label's height
    private static void AdjustLabelHeight( Control lb ) {
      string text = lb.Text;
      Font textFont = lb.Font;
      SizeF layoutSize = new SizeF( lb.ClientSize.Width, 5000.0F );
      Graphics g = Graphics.FromHwnd( lb.Handle );
      SizeF stringSize = g.MeasureString( text, textFont, layoutSize );
      lb.Height = (int) stringSize.Height + 4;
      g.Dispose();
    }
    #endregion

    #region Interface Methods
    /// <summary>
    /// Simulate the action of a button click in the TaskDialog. This can be a DialogResult value 
    /// or the ButtonID set on a TasDialogButton set on TaskDialog.Buttons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be clicked.</param>
    public void ClickButton( int buttonId ) {
      IEnumerable<Button> commonButtons = commonButtonPanel.Controls.OfType<Button>();
      foreach( Button button in commonButtons.Where( button => (int)button.Tag == buttonId ) ) {
        InvokeButtonClicked( this, new ButtonClickedArgs( (int)button.Tag ) );
        return;
      }
      IEnumerable<CommandButton> buttons = pnlCommandButtons.Controls.OfType<CommandButton>();
      foreach( CommandButton button in buttons.Where( button => (int)button.Tag == buttonId ) ) {
        InvokeButtonClicked( this, new ButtonClickedArgs( (int)button.Tag ) );
        return;
      }
    }

    /// <summary>
    /// Simulate the action of a radio button click in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be selected.</param>
    public void ClickRadioButton( int buttonId ) {
      RadioButton button = pnlRadioButtons.Controls.OfType<RadioButton>().Where( b => (int)b.Tag == buttonId ).First();
      if( button != null ) {
        button.Select();
        InvokeRadioButtonClicked( this, new ButtonClickedArgs( buttonId ) );
      }
    }

    /// <summary>
    /// Check or uncheck the verification checkbox in the TaskDialog. 
    /// </summary>
    /// <param name="verificationChecked">The checked state to set the verification checkbox.</param>
    /// <param name="setFocus">True to set the keyboard focus to the checkbox, and false otherwise.</param>
    public void ClickVerification( bool verificationChecked, bool setFocus ) {
      verifyCheckBox.Checked = verificationChecked;
      if( setFocus ) verifyCheckBox.Select();
      else focusButton.Select();
    }

    /// <summary>
    /// Enable or disable a button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.Buttons
    /// or a common button ID.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    /// <param name="enable">Enambe the button if true. Disable the button if false.</param>
    public void EnableButton( int buttonId, bool enable ) {
      IEnumerable<Button> commonButtons = commonButtonPanel.Controls.OfType<Button>();
      foreach( Button button in commonButtons.Where( button => (int)button.Tag == buttonId ) ) {
        button.Enabled = enable;
        return;
      }
      IEnumerable<CommandButton> buttons = pnlCommandButtons.Controls.OfType<CommandButton>();
      foreach( CommandButton button in buttons.Where( button => (int)button.Tag == buttonId ) ) {
        button.Enabled = enable;
        return;
      }
    }

    /// <summary>
    /// Enable a button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.Buttons
    /// or a common button ID.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    public void EnableButton( int buttonId ) {
      EnableButton( buttonId, true );
    }

    /// <summary>
    /// Disable a button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.Buttons
    /// or a common button ID.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    public void DisableButton( int buttonId ) {
      EnableButton( buttonId, false );
    }

    /// <summary>
    /// Enable or disable a radio button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    /// <param name="enable">Enambe the button if true. Disable the button if false.</param>
    public void EnableRadioButton( int buttonId, bool enable ) {
      RadioButton button = pnlRadioButtons.Controls.OfType<RadioButton>().Where( b => (int)b.Tag == buttonId ).First();
      if( button != null ) {
        button.Enabled = enable;
      }
    }

    /// <summary>
    /// Enable a radio button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    public void EnableRadioButton( int buttonId ) {
      EnableRadioButton( buttonId, true );
    }

    /// <summary>
    /// Disable a radio button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    public void DisableRadioButton( int buttonId ) {
      EnableRadioButton( buttonId, false );
    }

    /// <summary>
    /// Recreates a task dialog with new contents, simulating the functionality of a multi-page wizard. 
    /// </summary>
    /// <param name="page">The next page.</param>
    public void NavigatePage( ITaskDialog page ) {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Designate whether a given Task Dialog button or command link should have a User Account Control (UAC) shield icon.
    /// </summary>
    /// <param name="buttonId">ID of the push button or command link to be updated.</param>
    /// <param name="requiresElevation">False to designate that the action invoked by the button does not require elevation;
    /// true to designate that the action does require elevation.</param>
    public void SetButtonElevationRequiredState( int buttonId, bool requiresElevation ) {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Updates the content text.
    /// </summary>
    /// <param name="text">The new value.</param>
    public void SetContentText( string text ) {
      contentText.Text = text;
      contentText.ConvertLinks();
      ReCalculateLayout();
    }

    /// <summary>
    /// Updates the Expanded Information text.
    /// </summary>
    /// <param name="text">The new value.</param>
    public void SetExpandedInformationText( string text ) {
      if( string.IsNullOrEmpty( ExpandedInformation ) ) return;
      expandedInfoText.Text = text;
      expandedInfoText.ConvertLinks();
      ReCalculateLayout();
    }

    /// <summary>
    /// Updates the Footer text.
    /// </summary>
    /// <param name="text">The new value.</param>
    public void SetFooterText( string text ) {
      footerText.Text = text;
      footerText.ConvertLinks();
      ReCalculateLayout();
    }

    /// <summary>
    /// Updates the Main Instruction.
    /// </summary>
    /// <param name="text">The new value.</param>
    public void SetMainInstructionText( string text ) {
      MainInstruction = text;
      pnlMainInstruction.Refresh();
      ReCalculateLayout();
    }

    /// <summary>
    /// Used to indicate whether the hosted progress bar should be displayed in marquee mode or not.
    /// </summary>
    /// <param name="setMarquee">Specifies whether the progress bar sbould be shown in Marquee mode.
    /// A value of true turns on Marquee mode.</param>
    public void SetMarqueeProgressBar( bool setMarquee ) {
      ShowMarqueeProgressBar  = setMarquee;
      ShowProgressBar         = !setMarquee;
      progressBar.Style       = ( setMarquee ) ? System.Windows.Forms.ProgressBarStyle.Marquee : System.Windows.Forms.ProgressBarStyle.Continuous;
    }

    /// <summary>
    /// Sets the animation state of the Marquee Progress Bar.
    /// </summary>
    /// <param name="startMarquee">true starts the marquee animation and false stops it.</param>
    /// <param name="speed">The time in milliseconds between refreshes.</param>
    public void SetProgressBarMarquee( bool startMarquee, uint speed ) {
      if( 0 == speed ) speed = 30;
      progressBar.MarqueeAnimationSpeed = ( startMarquee ) ? (int)speed : int.MaxValue;
      ProgressBarSpeed = progressBar.MarqueeAnimationSpeed;
    }

    /// <summary>
    /// Set the current position for a progress bar.
    /// </summary>
    /// <param name="position">The new position.</param>
    /// <returns>Returns the previous value if successful, or zero otherwise.</returns>
    public int SetProgressBarPosition( int position ) {
      int oldValue = progressBar.Value;
      progressBar.Value = position;
      return oldValue;
    }

    /// <summary>
    /// Set the minimum and maximum values for the hosted progress bar.
    /// </summary>
    /// <param name="minimum">Minimum range value. By default, the minimum value is zero.</param>
    /// <param name="maximum">Maximum range value.  By default, the maximum value is 100.</param>
    public void SetProgressBarRange( int minimum, int maximum ) {
      progressBar.Minimum = minimum;
      progressBar.Maximum = maximum;
    }

    /// <summary>
    /// Sets the state of the progress bar.
    /// </summary>
    /// <param name="state">The state to set the progress bar.</param>
    public void SetProgressBarState( ProgressBarState state ) {
      switch( state ) {
        case ProgressBarState.Normal:
          progressBar.ForeColor = SystemColors.Highlight;
          progressBar.MarqueeAnimationSpeed = ProgressBarSpeed;
          break;

        case ProgressBarState.Pause:
          progressBar.ForeColor = SystemColors.Highlight;
          progressBar.MarqueeAnimationSpeed = int.MaxValue;
          break;

        case ProgressBarState.Error:
          // This is actually unsupported as the ProgressBar ignores ForeColor
          // and PBM_SETSTATE is not supported in XP
          progressBar.ForeColor = Color.Red;
          progressBar.MarqueeAnimationSpeed = int.MaxValue;
          break;
        default:
          throw new ArgumentOutOfRangeException( "state" );
      }
      ProgressBarState = state;
    }

    /// <summary>
    /// Updates the main instruction icon. Note the type (standard via enum or
    /// custom via Icon type) must be used when upating the icon.
    /// </summary>
    /// <param name="icon">The icon to set.</param>
    public void UpdateMainIcon( Icon icon ) {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Updates the footer icon. Note the type (standard via enum or
    /// custom via Icon type) must be used when upating the icon.
    /// </summary>
    /// <param name="icon">Task Dialog standard icon.</param>
    public void UpdateFooterIcon( Icon icon ) {
      throw new NotImplementedException();
    }
    #endregion
    #endregion

    #region Events
    private void CommandButton_Click( object sender, EventArgs e ) {
      int commandButtonClicked = (int) ( (CommandButton) sender ).Tag;
      ButtonClickedArgs buttonClickedArgs = new ButtonClickedArgs( commandButtonClicked );
      if( InvokeButtonClicked( this, buttonClickedArgs ) ) return;
      _commandButtonClicked = commandButtonClicked;
      DialogResult = DialogResult.OK;
    }

    private void CommonButtonClick( object sender, EventArgs e ) {
      Button button = sender as Button;
      if( button != null ) {
        ButtonClickedArgs buttonClickedArgs = new ButtonClickedArgs( (int)button.Tag );
        if( InvokeButtonClicked( this, buttonClickedArgs ) ) return;
        
        DialogResult = (DialogResult)button.Tag;
        Close();
      }
    }

    private bool InvokeButtonClicked( ITaskDialog sender, ButtonClickedArgs e ) {
      if( ButtonClicked != null ) ButtonClicked( sender, e );
      if( e.PreventClosing ) return true;

      return false;
    }

    private void RadioButtonClick( object sender, EventArgs e ) {
      int radioButton = (int)( (RadioButton)sender ).Tag;
      _radioButtonClicked = radioButton;
      if( null != RadioButtonClicked ) RadioButtonClicked( this, new ButtonClickedArgs( radioButton ) );
    }

    private void InvokeRadioButtonClicked( ITaskDialog sender, ButtonClickedArgs e ) {
      if( RadioButtonClicked != null ) RadioButtonClicked( sender, e );
    }

    //--------------------------------------------------------------------------------
    protected override void OnShown( EventArgs e ) {
      if( !_formBuilt )
        throw new Exception( "EmulatedTaskDialog : Please call .BuildForm() before showing the TaskDialog" );
      base.OnShown( e );
      focusButton.Select();
    }

    //--------------------------------------------------------------------------------
    private void LbDetailsMouseEnter( object sender, EventArgs e ) {
      showHideDetails.ImageIndex = ( IsExpanded ? 1 : 4 );
    }

    //--------------------------------------------------------------------------------
    private void LbDetailsMouseLeave( object sender, EventArgs e ) {
      showHideDetails.ImageIndex = ( IsExpanded ? 0 : 3 );
    }

    //--------------------------------------------------------------------------------
    private void LbDetailsMouseUp( object sender, MouseEventArgs e ) {
      showHideDetails.ImageIndex = ( IsExpanded ? 1 : 4 );
    }

    //--------------------------------------------------------------------------------
    private void LbDetailsMouseDown( object sender, MouseEventArgs e ) {
      showHideDetails.ImageIndex = ( IsExpanded ? 2 : 5 );
    }

    //--------------------------------------------------------------------------------
    private void LbDetailsClick( object sender, EventArgs e ) {
      IsExpanded = !IsExpanded;
      pnlExpandedInfo.Visible = IsExpanded;
      showHideDetails.Text = ( IsExpanded ? String.Format( "        {0}", ExpandedControlText ) : String.Format( "        {0}", CollapsedControlText ) );
      if( IsExpanded )
        Height += pnlExpandedInfo.Height;
      else
        Height -= pnlExpandedInfo.Height;
      if( null != ExpandoButtonClicked ) ExpandoButtonClicked( this, EventArgs.Empty );
    }

    //--------------------------------------------------------------------------------
    private const int MainInstructionLeftMargin = 46;
    private const int MainInstructionRightMargin = 8;

    private SizeF GetMainInstructionTextSizeF() {
      SizeF mzSize = new SizeF(
        pnlMainInstruction.Width - MainInstructionLeftMargin - MainInstructionRightMargin, 5000.0F );
      Graphics g = Graphics.FromHwnd( Handle );
      SizeF textSize = g.MeasureString( MainInstruction, _mainInstructionFont, mzSize );
      _mainInstructionHeight = (int) textSize.Height;
      return textSize;
    }

    private void PnlMainInstructionPaint( object sender, PaintEventArgs e ) {
      SizeF szL = GetMainInstructionTextSizeF();
      e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
      e.Graphics.DrawString(
        MainInstruction,
        _mainInstructionFont,
        new SolidBrush( _mainInstructionColor ),
        new RectangleF( new PointF( MainInstructionLeftMargin, 10 ), szL ) );
    }

    //--------------------------------------------------------------------------------

    private void FrmTaskDialogShown( object sender, EventArgs e ) {
      switch( MainIcon ) {
        case global::TaskDialog.UserInterface.Icon.Error:
          System.Media.SystemSounds.Hand.Play();
          break;
        case global::TaskDialog.UserInterface.Icon.Information:
          System.Media.SystemSounds.Asterisk.Play();
          break;
        case global::TaskDialog.UserInterface.Icon.Warning:
          System.Media.SystemSounds.Exclamation.Play();
          break;
      }

      if( FocusControl != null )
        FocusControl.Focus();

      if( null != Created ) Created( this, e );
    }

    private void CallbackTimerTick( object sender, EventArgs e ) {
      if( null != Timer ) Timer( this, new TimerArgs( (uint)DateTime.Now.Subtract( StartTime ).TotalMilliseconds ) );
    }

    private void TaskDialogForm_FormClosed( object sender, FormClosedEventArgs e ) {
      if( null != Destroyed ) Destroyed( this, e );
    }

    private void VerifyCheckedChanged( object sender, EventArgs e ) {
      _verificationChecked = ( (CheckBox)sender ).Checked;
      if( null != VerificationClicked ) VerificationClicked( this, e );
    }
    #endregion

    private void TaskDialogForm_KeyDown( object sender, KeyEventArgs e ) {
      if( e.KeyCode == Keys.F1 && null != Help ) Help( this, EventArgs.Empty );
    }

    private void EmulatedTaskDialog_FormClosing( object sender, FormClosingEventArgs e ) {
      if( e.CloseReason == CloseReason.UserClosing ) {
        // The close button has the same button id as the Yes common button.
        // Kinda weird, but that's how it seems to be.
        ButtonClickedArgs buttonClickedArgs = new ButtonClickedArgs( 2 );
        if( !AllowDialogCancellation || InvokeButtonClicked( this, buttonClickedArgs ) ) e.Cancel = true;
        DialogResult = (DialogResult)_cancelButtonId;
      }
    }

    private void ContentClick( object sender, EventArgs e ) {
      focusButton.Select();
    }

    private void ExpandedInfoClick( object sender, EventArgs e ) {
      focusButton.Select();
    }

    private void FooterClick( object sender, EventArgs e ) {
      focusButton.Select();
    }

    private void ContentMouseDown( object sender, MouseEventArgs e ) {
      focusButton.Select();
    }

    private void ExpandedInfoMouseDown( object sender, MouseEventArgs e ) {
      focusButton.Select();
    }

    private void FooterMouseDown( object sender, MouseEventArgs e ) {
      focusButton.Select();
    }

    private void ContentLinkClicked( object sender, LinkClickedEventArgs e ) {
      if( null != HyperlinkClicked ) HyperlinkClicked( this, new HyperlinkClickedArgs( contentText.GetUrlForLinkText( e.LinkText ) ) );
    }

    private void ExpandedInfoLinkClicked( object sender, LinkClickedEventArgs e ) {
      if( null != HyperlinkClicked ) HyperlinkClicked( this, new HyperlinkClickedArgs( expandedInfoText.GetUrlForLinkText( e.LinkText ) ) );

    }

    private void FooterLinkClicked( object sender, LinkClickedEventArgs e ) {
      if( null != HyperlinkClicked ) HyperlinkClicked( this, new HyperlinkClickedArgs( footerText.GetUrlForLinkText( e.LinkText ) ) );
    }

    //--------------------------------------------------------------------------------
  }
}
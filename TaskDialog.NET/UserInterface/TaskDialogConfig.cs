using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TaskDialogNet.UserInterface {

  public class TaskDialogButton {
    public int ButtonId { get; set; }
    public string ButtonText { get; set; }
  }

  [Flags]
  public enum CommonButtons {
    None = 0x00,
    Ok = 0x01,
    Yes = 0x02,
    No = 0x04,
    Cancel = 0x08,
    Retry = 0x10,
    Close = 0x20
  } 

  public enum CommonIcon {
    None,
    Information,
    Warning,
    Error,
    SecurityWarning,
    SecurityError,
    SecuritySuccess,
    SecurityShield,
    SecurityShieldBlue,
    SecurityShieldGray,
    Custom
  }

  public enum ProgressBarStyle {
    None,
    Continous,
    Marquee
  }

  public enum ProgressBarState {
    Normal,
    Pause,
    Error
  }

  public class TaskDialogFlags : ICloneable {
    /// <summary>
    /// Enables hyperlink processing for the strings specified in the Content, ExpandedInformation
    /// and FooterText members. When enabled, these members may be strings that contain hyperlinks
    /// in the form: <A HREF="executablestring">Hyperlink Text</A>. 
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// </summary>
    public bool EnableHyperLinks { get; set; }

    /// <summary>
    /// Indicates that the dialog should be able to be closed using Alt-F4, Escape and the title bar’s
    /// close button even if no cancel button is specified in either the CommonButtons or Buttons members.
    /// </summary>
    public bool AllowDialogCancellation { get; set; }

    /// <summary>
    /// Indicates that the buttons specified in the Buttons member should be displayed as command links
    /// (using a standard task dialog glyph) instead of push buttons.  When using command links, all
    /// characters up to the first new line character in the ButtonText member (of the TaskDialogButton
    /// structure) will be treated as the command link’s main text, and the remainder will be treated
    /// as the command link’s note. This flag is ignored if the Buttons member has no entires.
    /// </summary>
    public bool UseCommandLinks { get; set; }

    /// <summary>
    /// Indicates that the buttons specified in the Buttons member should be displayed as command links
    /// (without a glyph) instead of push buttons. When using command links, all characters up to the
    /// first new line character in the ButtonText member (of the TaskDialogButton structure) will be
    /// treated as the command link’s main text, and the remainder will be treated as the command link’s
    /// note. This flag is ignored if the Buttons member has no entires.
    /// </summary>
    public bool UseCommandLinksNoIcon { get; set; }

    /// <summary>
    /// Indicates that the string specified by the ExpandedInformation member should be displayed at the
    /// bottom of the dialog’s footer area instead of immediately after the dialog’s content. This flag
    /// is ignored if the ExpandedInformation member is null.
    /// </summary>
    public bool ExpandFooterArea { get; set; }

    /// <summary>
    /// Indicates that the string specified by the ExpandedInformation member should be displayed
    /// when the dialog is initially displayed. This flag is ignored if the ExpandedInformation member
    /// is null.
    /// </summary>
    public bool ExpandedByDefault { get; set; }

    /// <summary>
    /// Indicates that the verification checkbox in the dialog should be checked when the dialog is
    /// initially displayed. This flag is ignored if the VerificationText parameter is null.
    /// </summary>
    public bool VerificationFlagChecked { get; set; }

    /// <summary>
    /// Indicates that a Progress Bar should be displayed.
    /// </summary>
    public bool ShowProgressBar { get; set; }

    /// <summary>
    /// Indicates that a Marquee Progress Bar should be displayed.
    /// </summary>
    public bool ShowMarqueeProgressBar { get; set; }

    /// <summary>
    /// Indicates that the TaskDialog’s callback should be called approximately every 200 milliseconds.
    /// </summary>
    public bool CallbackTimer { get; set; }

    /// <summary>
    /// Indicates that the TaskDialog should be positioned (centered) relative to the owner window
    /// passed when calling Show. If not set (or no owner window is passed), the TaskDialog is
    /// positioned (centered) relative to the monitor.
    /// </summary>
    public bool PositionRelativeToWindow { get; set; }

    /// <summary>
    /// Indicates that the TaskDialog should have right to left layout.
    /// </summary>
    public bool RtlLayout { get; set; }

    /// <summary>
    /// Indicates that the TaskDialog should have no default radio button.
    /// </summary>
    public bool NoDefaultRadioButton { get; set; }

    /// <summary>
    /// Indicates that the TaskDialog can be minimised. Works only if there if parent window is null. Will enable cancellation also.
    /// </summary>
    public bool CanBeMinimized { get; set; }

    public TaskDialogFlags() {
      EnableHyperLinks          = false;
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
    }

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public object Clone() {
      return MemberwiseClone();
    }
  }

  public class TaskDialogConfig : ICloneable {
    public IntPtr Parent { get; set; }
    public TaskDialogFlags Flags { get; set; }

    /// <summary>
    /// Specifies the push buttons displayed in the dialog box. This parameter may be a combination of flags.
    /// If no common buttons are specified and no custom buttons are specified using the Buttons member, the
    /// dialog box will contain the OK button by default.
    /// </summary>
    public CommonButtons CommonButtons { get; set; }

    /// <summary>
    /// The string to be used for the dialog box title. If this parameter is NULL, the filename of the executable program is used.
    /// </summary>
    public string WindowTitle { get; set; }

    /// <summary>
    /// Specifies a built in icon for the main icon in the dialog. If this is set to none
    /// and the CustomMainIcon is null then no main icon will be displayed.
    /// </summary>
    public CommonIcon MainIcon { get; set; }

    /// <summary>
    /// Specifies a custom icon for the main icon in the dialog. If this is set to none
    /// and the CustomMainIcon member is null then no main icon will be displayed.
    /// </summary>
    public Icon CustomMainIcon { get; set; }

    /// <summary>
    /// The string to be used for the main instruction.
    /// </summary>
    public string MainInstruction { get; set; }

    /// <summary>
    /// The string to be used for the dialog’s primary content. If the EnableHyperlinks member is true,
    /// then this string may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>. 
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Specifies the custom push buttons to display in the dialog. Use CommonButtons member for
    /// common buttons; OK, Yes, No, Retry and Cancel, and Buttons when you want different text
    /// on the push buttons.
    /// </summary>
    public List<TaskDialogButton> Buttons { get; set; }

    /// <summary>
    /// Indicates the default button for the dialog. This may be any of the values specified
    /// in ButtonId members of one of the TaskDialogButton structures in the Buttons array,
    /// or one a DialogResult value that corresponds to a buttons specified in the CommonButtons Member.
    /// If this member is zero or its value does not correspond to any button ID in the dialog,
    /// then the first button in the dialog will be the default. 
    /// </summary>
    public int DefaultButton { get; set; }

    /// <summary>
    /// Specifies the radio buttons to display in the dialog.
    /// </summary>
    public List<TaskDialogButton> RadioButtons { get; set; }

    /// <summary>
    /// Indicates the default radio button for the dialog. This may be any of the values specified
    /// in ButtonId members of one of the TaskDialogButton structures in the RadioButtons array.
    /// If this member is zero or its value does not correspond to any radio button ID in the dialog,
    /// then the first button in RadioButtons will be the default.
    /// The property NoDefaultRadioButton can be set to have no default.
    /// </summary>
    public int DefaultRadioButton { get; set; }

    /// <summary>
    /// The string to be used to label the verification checkbox. If this member is null, the
    /// verification checkbox is not displayed in the dialog box.
    /// </summary>
    public string VerificationText { get; set; }

    /// <summary>
    /// The string to be used for displaying additional information. The additional information is
    /// displayed either immediately below the content or below the footer text depending on whether
    /// the ExpandFooterArea member is true. If the EnameHyperlinks member is true, then this string
    /// may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>.
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// </summary>
    public string ExpandedInformation { get; set; }

    /// <summary>
    /// The string to be used to label the button for collapsing the expanded information. This
    /// member is ignored when the ExpandedInformation member is null. If this member is null
    /// and the CollapsedControlText is specified, then the CollapsedControlText value will be
    /// used for this member as well.
    /// </summary>
    public string ExpandedControlText { get; set; }

    /// <summary>
    /// The string to be used to label the button for expanding the expanded information. This
    /// member is ignored when the ExpandedInformation member is null.  If this member is null
    /// and the ExpandedControlText is specified, then the ExpandedControlText value will be
    /// used for this member as well.
    /// </summary>
    public string CollapsedControlText { get; set; }

    /// <summary>
    /// Specifies a built in icon for the icon to be displayed in the footer area of the
    /// dialog box. If this is set to none and the CustomFooterIcon member is null then no
    /// footer icon will be displayed.
    /// </summary>
    public CommonIcon FooterIcon { get; set; }

    /// <summary>
    /// Specifies a custom icon for the icon to be displayed in the footer area of the
    /// dialog box. If this is set to none and the CustomFooterIcon member is null then no
    /// footer icon will be displayed.
    /// </summary>
    public Icon CustomFooterIcon { get; set; }

    /// <summary>
    /// The string to be used in the footer area of the dialog box. If the EnableHyperlinks member
    /// is true, then this string may contain hyperlinks in the form: <A HREF="executablestring">
    /// Hyperlink Text</A>.
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// </summary>
    public string Footer { get; set; }

    /// <summary>
    /// width of the Task Dialog's client area in DLU's. If 0, Task Dialog will calculate the ideal width.
    /// </summary>
    public int Width { get; set; }

    public TaskDialogConfig() {
      Parent                    = IntPtr.Zero;
      Flags                     = new TaskDialogFlags();
      CommonButtons             = CommonButtons.None;
      WindowTitle               = null;
      MainIcon                  = CommonIcon.None;
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
      FooterIcon                = CommonIcon.None;
      CustomFooterIcon          = null;
      Footer                    = null;
      Width                     = 0;
    }

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public object Clone() {
      return MemberwiseClone();
    }
  }
}

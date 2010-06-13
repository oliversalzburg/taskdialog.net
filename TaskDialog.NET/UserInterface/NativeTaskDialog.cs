using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TaskDialog.UserInterface {
  /// <summary>
  /// The TaskDialog common button flags used to specify the builtin bottons to show in the TaskDialog.
  /// </summary>
  [Flags]
  public enum VistaTaskDialogCommonButtons {
    /// <summary>
    /// No common buttons.
    /// </summary>
    None = 0,

    /// <summary>
    /// OK common button. If selected Task Dialog will return DialogResult.OK.
    /// </summary>
    Ok = 0x0001,

    /// <summary>
    /// Yes common button. If selected Task Dialog will return DialogResult.Yes.
    /// </summary>
    Yes = 0x0002,

    /// <summary>
    /// No common button. If selected Task Dialog will return DialogResult.No.
    /// </summary>
    No = 0x0004,

    /// <summary>
    /// Cancel common button. If selected Task Dialog will return DialogResult.Cancel.
    /// If this button is specified, the dialog box will respond to typical cancel actions (Alt-F4 and Escape).
    /// </summary>
    Cancel = 0x0008,

    /// <summary>
    /// Retry common button. If selected Task Dialog will return DialogResult.Retry.
    /// </summary>
    Retry = 0x0010,

    /// <summary>
    /// Close common button. If selected Task Dialog will return this value.
    /// </summary>
    Close = 0x0020,
  }

  /// <summary>
  /// The System icons the TaskDialog supports.
  /// </summary>
  [SuppressMessage( "Microsoft.Design", "CA1028:EnumStorageShouldBeInt32" )] // Type comes from CommCtrl.h
  public enum VistaTaskDialogIcon : uint {
    /// <summary>
    /// No Icon.
    /// </summary>
    None = 0,

    /// <summary>
    /// System warning icon.
    /// </summary>
    //Warning = 0xFFFF, // MAKEINTRESOURCEW(-1)
    /// <summary>
    /// System Error icon.
    /// </summary>
    //Error = 0xFFFE, // MAKEINTRESOURCEW(-2)
    /// <summary>
    /// System Information icon.
    /// </summary>
    //Information = 0xFFFD, // MAKEINTRESOURCEW(-3)
    /// <summary>
    /// Shield icon.
    /// </summary>
    //Shield = 0xFFFC, // MAKEINTRESOURCEW(-4)
    Information = UInt16.MaxValue - 2,
    Warning = UInt16.MaxValue,
    Error = UInt16.MaxValue - 1,
    SecurityWarning = UInt16.MaxValue - 5,
    SecurityError = UInt16.MaxValue - 6,
    SecuritySuccess = UInt16.MaxValue - 7,
    SecurityShield = UInt16.MaxValue - 3,
    SecurityShieldBlue = UInt16.MaxValue - 4,
    SecurityShieldGray = UInt16.MaxValue - 8
  }

  /// <summary>
  /// Task Dialog callback notifications. 
  /// </summary>
  public enum VistaTaskDialogNotification {
    /// <summary>
    /// Sent by the Task Dialog once the dialog has been created and before it is displayed.
    /// The value returned by the callback is ignored.
    /// </summary>
    Created = 0,

    /// <summary>
    /// Sent by the Task Dialog when a navigation has occurred.
    /// The value returned by the callback is ignored.
    /// </summary>   
    Navigated = 1,

    /// <summary>
    /// Sent by the Task Dialog when the user selects a button or command link in the task dialog.
    /// The button ID corresponding to the button selected will be available in the
    /// TaskDialogNotificationArgs. To prevent the Task Dialog from closing, the application must
    /// return true, otherwise the Task Dialog will be closed and the button ID returned to via
    /// the original application call.
    /// </summary>
    ButtonClicked = 2, // wParam = Button ID

    /// <summary>
    /// Sent by the Task Dialog when the user clicks on a hyperlink in the Task Dialog’s content.
    /// The string containing the HREF of the hyperlink will be available in the
    /// TaskDialogNotificationArgs. To prevent the TaskDialog from shell executing the hyperlink,
    /// the application must return TRUE, otherwise ShellExecute will be called.
    /// </summary>
    HyperlinkClicked = 3, // lParam = (LPCWSTR)pszHREF

    /// <summary>
    /// Sent by the Task Dialog approximately every 200 milliseconds when TaskDialog.CallbackTimer
    /// has been set to true. The number of milliseconds since the dialog was created or the
    /// notification returned true is available on the TaskDialogNotificationArgs. To reset
    /// the tickcount, the application must return true, otherwise the tickcount will continue to
    /// increment.
    /// </summary>
    Timer = 4, // wParam = Milliseconds since dialog created or timer reset

    /// <summary>
    /// Sent by the Task Dialog when it is destroyed and its window handle no longer valid.
    /// The value returned by the callback is ignored.
    /// </summary>
    Destroyed = 5,

    /// <summary>
    /// Sent by the Task Dialog when the user selects a radio button in the task dialog.
    /// The button ID corresponding to the button selected will be available in the
    /// TaskDialogNotificationArgs.
    /// The value returned by the callback is ignored.
    /// </summary>
    RadioButtonClicked = 6, // wParam = Radio Button ID

    /// <summary>
    /// Sent by the Task Dialog once the dialog has been constructed and before it is displayed.
    /// The value returned by the callback is ignored.
    /// </summary>
    DialogConstructed = 7,

    /// <summary>
    /// Sent by the Task Dialog when the user checks or unchecks the verification checkbox.
    /// The verificationFlagChecked value is available on the TaskDialogNotificationArgs.
    /// The value returned by the callback is ignored.
    /// </summary>
    VerificationClicked = 8, // wParam = 1 if checkbox checked, 0 if not, lParam is unused and always 0

    /// <summary>
    /// Sent by the Task Dialog when the user presses F1 on the keyboard while the dialog has focus.
    /// The value returned by the callback is ignored.
    /// </summary>
    Help = 9,

    /// <summary>
    /// Sent by the task dialog when the user clicks on the dialog's expando button.
    /// The expanded value is available on the TaskDialogNotificationArgs.
    /// The value returned by the callback is ignored.
    /// </summary>
    ExpandButtonClicked = 10 // wParam = 0 (dialog is now collapsed), wParam != 0 (dialog is now expanded)
  }

  /// <summary>
  /// Progress bar state.
  /// </summary>
  [SuppressMessage( "Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue" )]
  // Comes from CommCtrl.h PBST_* values which don't have a zero.
  public enum VistaProgressBarState {
    /// <summary>
    /// Normal.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// Error state.
    /// </summary>
    Error = 2,

    /// <summary>
    /// Paused state.
    /// </summary>
    Paused = 3
  }

  /// <summary>
  /// A custom button for the TaskDialog.
  /// </summary>
  [SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes" )]
  // Would be unused code as not required for usage.
  [StructLayout( LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1 )]
  public struct VistaTaskDialogButton {
    /// <summary>
    /// The ID of the button. This value is returned by TaskDialog.Show when the button is clicked.
    /// </summary>
    private int buttonId;

    /// <summary>
    /// The string that appears on the button.
    /// </summary>
    [MarshalAs( UnmanagedType.LPWStr )]
    private string buttonText;

    /// <summary>
    /// Initialize the custom button.
    /// </summary>
    /// <param name="id">The ID of the button. This value is returned by TaskDialog.Show when
    /// the button is clicked. Typically this will be a value in the DialogResult enum.</param>
    /// <param name="text">The string that appears on the button.</param>
    public VistaTaskDialogButton( int id, string text ) {
      buttonId = id;
      buttonText = text;
    }

    /// <summary>
    /// The ID of the button. This value is returned by TaskDialog.Show when the button is clicked.
    /// </summary>
    public int ButtonId {
      get { return buttonId; }
      set { buttonId = value; }
    }

    /// <summary>
    /// The string that appears on the button.
    /// </summary>
    public string ButtonText {
      get { return buttonText; }
      set { buttonText = value; }
    }
  }

  /// <summary>
  /// A Task Dialog. This is like a MessageBox but with many more features. TaskDialog requires Windows Longhorn or later.
  /// </summary>
  public class NativeTaskDialog : ITaskDialog {
    #region Events
    public event TaskDialogEventHandler<ButtonClickedArgs> ButtonClicked;
    public event TaskDialogEventHandler Created;
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

    /// <summary>
    /// The flags passed to TaskDialogIndirect.
    /// </summary>
    private UnsafeNativeMethods.TASKDIALOG_FLAGS Flags { get; set; }

    /// <summary>
    /// Creates a default Task Dialog.
    /// </summary>
    public NativeTaskDialog( ) {
      Reset();
    }

    /// <summary>
    /// Returns true if the current operating system supports TaskDialog. If false TaskDialog.Show should not
    /// be called as the results are undefined but often results in a crash.
    /// </summary>
    public static bool IsAvailableOnThisOs {
      get {
        OperatingSystem os = Environment.OSVersion;
        if( os.Platform != PlatformID.Win32NT )
          return false;
        return ( os.Version.CompareTo( RequiredOsVersion ) >= 0 );
      }
    }

    /// <summary>
    /// The minimum Windows version needed to support TaskDialog.
    /// </summary>
    public static Version RequiredOsVersion {
      get { return new Version( 6, 0, 5243 ); }
    }

    #region Properties
    #region Flags
    /// <summary>
    /// Enables hyperlink processing for the strings specified in the Content, ExpandedInformation
    /// and FooterText members. When enabled, these members may be strings that contain hyperlinks
    /// in the form: <A HREF="executablestring">Hyperlink Text</A>. 
    /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
    /// Note: Task Dialog will not actually execute any hyperlinks. Hyperlink execution must be handled
    /// in the callback function specified by Callback member.
    /// </summary>
    public bool EnableHyperlinks {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS, value ); }
    }

    /// <summary>
    /// Indicates that the dialog should be able to be closed using Alt-F4, Escape and the title bar’s
    /// close button even if no cancel button is specified in either the CommonButtons or Buttons members.
    /// </summary>
    public bool AllowDialogCancellation {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION, value ); }
    }

    /// <summary>
    /// Indicates that the buttons specified in the Buttons member should be displayed as command links
    /// (using a standard task dialog glyph) instead of push buttons.  When using command links, all
    /// characters up to the first new line character in the ButtonText member (of the TaskDialogButton
    /// structure) will be treated as the command link’s main text, and the remainder will be treated
    /// as the command link’s note. This flag is ignored if the Buttons member has no entires.
    /// </summary>
    public bool UseCommandLinks {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS, value ); }
    }

    /// <summary>
    /// Indicates that the buttons specified in the Buttons member should be displayed as command links
    /// (without a glyph) instead of push buttons. When using command links, all characters up to the
    /// first new line character in the ButtonText member (of the TaskDialogButton structure) will be
    /// treated as the command link’s main text, and the remainder will be treated as the command link’s
    /// note. This flag is ignored if the Buttons member has no entires.
    /// </summary>
    public bool UseCommandLinksNoIcon {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON, value ); }
    }

    /// <summary>
    /// Indicates that the string specified by the ExpandedInformation member should be displayed at the
    /// bottom of the dialog’s footer area instead of immediately after the dialog’s content. This flag
    /// is ignored if the ExpandedInformation member is null.
    /// </summary>
    public bool ExpandFooterArea {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA, value ); }
    }

    /// <summary>
    /// Indicates that the string specified by the ExpandedInformation member should be displayed
    /// when the dialog is initially displayed. This flag is ignored if the ExpandedInformation member
    /// is null.
    /// </summary>
    public bool ExpandedByDefault {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT, value ); }
    }

    /// <summary>
    /// Indicates that the verification checkbox in the dialog should be checked when the dialog is
    /// initially displayed. This flag is ignored if the VerificationText parameter is null.
    /// </summary>
    public bool VerificationFlagChecked {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED, value ); }
    }

    /// <summary>
    /// Indicates that a Progress Bar should be displayed.
    /// </summary>
    public bool ShowProgressBar {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR, value ); }
    }

    /// <summary>
    /// Indicates that an Marquee Progress Bar should be displayed.
    /// </summary>
    public bool ShowMarqueeProgressBar {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR, value ); }
    }

    /// <summary>
    /// Indicates that the TaskDialog’s callback should be called approximately every 200 milliseconds.
    /// </summary>
    public bool CallbackTimer {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER, value ); }
    }

    /// <summary>
    /// Indicates that the TaskDialog should be positioned (centered) relative to the owner window
    /// passed when calling Show. If not set (or no owner window is passed), the TaskDialog is
    /// positioned (centered) relative to the monitor.
    /// </summary>
    public bool PositionRelativeToWindow {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW, value ); }
    }

    /// <summary>
    /// Indicates that the TaskDialog should have right to left layout.
    /// </summary>
    public bool RtlLayout {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_RTL_LAYOUT ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_RTL_LAYOUT, value ); }
    }

    /// <summary>
    /// Indicates that the TaskDialog should have no default radio button.
    /// </summary>
    public bool NoDefaultRadioButton {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON, value ); }
    }

    /// <summary>
    /// Indicates that the TaskDialog can be minimised. Works only if there if parent window is null. Will enable cancellation also.
    /// </summary>
    public bool CanBeMinimized {
      get { return ( Flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED ) != 0; }
      set { SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED, value ); }
    }
    #endregion


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
    public Icon MainIcon { get; set; }

    /// <summary>
    /// Specifies a custom icon for the main icon in the dialog. If this is set to none
    /// and the CustomMainIcon member is null then no main icon will be displayed.
    /// </summary>
    public System.Drawing.Icon CustomMainIcon { get; set; }

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
    public Icon FooterIcon { get; set; }

    /// <summary>
    /// Specifies a custom icon for the icon to be displayed in the footer area of the
    /// dialog box. If this is set to none and the CustomFooterIcon member is null then no
    /// footer icon will be displayed.
    /// </summary>
    public System.Drawing.Icon CustomFooterIcon { get; set; }

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

    /// <summary>
    /// The window handle of the TaskDialog
    /// </summary>
    [SuppressMessage( "Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources" )] // We don't own the window.
    private IntPtr WindowHandle { get; set; }
    #endregion

    /// <summary>
    /// Reference that is passed to the callback.
    /// </summary>
    private object CallbackData { get; set; }

    #region Methods
    /// <summary>
    /// Resets the Task Dialog to the state when first constructed, all properties set to their default value.
    /// </summary>
    public void Reset( ) {
      Flags                     = 0;

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
      MainIcon                  = Icon.None;
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
      FooterIcon                = Icon.None;
      CustomFooterIcon          = null;
      Footer                    = null;
      Width                     = 0;

      CallbackData              = null;
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show( ) {
      bool verificationFlagChecked;
      int radioButtonResult;
      return Show( IntPtr.Zero, out verificationFlagChecked, out radioButtonResult );
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="owner">Owner window the task Dialog will modal to.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show( IWin32Window owner ) {
      bool verificationFlagChecked;
      int radioButtonResult;
      return Show(
        ( owner == null ? IntPtr.Zero : owner.Handle ),
        out verificationFlagChecked,
        out radioButtonResult );
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show( IntPtr hwndOwner ) {
      bool verificationFlagChecked;
      int radioButtonResult;
      return Show( hwndOwner, out verificationFlagChecked, out radioButtonResult );
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="owner">Owner window the task Dialog will modal to.</param>
    /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
    /// was dismissed.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show( IWin32Window owner, out bool verificationFlagChecked ) {
      int radioButtonResult;
      return Show(
        ( owner == null ? IntPtr.Zero : owner.Handle ),
        out verificationFlagChecked,
        out radioButtonResult );
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
    /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
    /// was dismissed.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show( IntPtr hwndOwner, out bool verificationFlagChecked ) {
      // We have to call a private version or PreSharp gets upset about a unsafe
      // block in a public method. (PreSharp error 56505)
      int radioButtonResult;
      return PrivateShow( hwndOwner, out verificationFlagChecked, out radioButtonResult );
    }

    #region Interface Methods
    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="owner">Owner window the task Dialog will modal to.</param>
    /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
    /// was dismissed.</param>
    /// <param name="radioButtonResult">The radio botton selected by the user.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show( IWin32Window owner, out bool verificationFlagChecked, out int radioButtonResult ) {
      return Show(
        ( owner == null ? IntPtr.Zero : owner.Handle ),
        out verificationFlagChecked,
        out radioButtonResult );
    }

    /// <summary>
    /// Simulate the action of a button click in the TaskDialog. This can be a DialogResult value 
    /// or the ButtonID set on a TasDialogButton set on TaskDialog.Buttons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be clicked.</param>
    public void ClickButton( int buttonId ) {
      // TDM_CLICK_BUTTON                    = WM_USER+102, // wParam = Button ID
      bool success = UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_CLICK_BUTTON,
        (IntPtr)buttonId,
        IntPtr.Zero ) != IntPtr.Zero;
      if( !success ) {
        int lastWin32Error = Marshal.GetLastWin32Error();
        throw new Win32Exception( lastWin32Error, "SendMessage for TDM_CLICK_BUTTON failed." );
      }
    }

    /// <summary>
    /// Simulate the action of a radio button click in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be selected.</param>
    public void ClickRadioButton( int buttonId ) {
      // TDM_CLICK_RADIO_BUTTON = WM_USER+110, // wParam = Radio Button ID
      UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_CLICK_RADIO_BUTTON,
        (IntPtr)buttonId,
        IntPtr.Zero );
    }

    /// <summary>
    /// Check or uncheck the verification checkbox in the TaskDialog. 
    /// </summary>
    /// <param name="verificationChecked">The checked state to set the verification checkbox.</param>
    /// <param name="setFocus">True to set the keyboard focus to the checkbox, and false otherwise.</param>
    public void ClickVerification( bool verificationChecked, bool setFocus ) {
      // TDM_CLICK_VERIFICATION = WM_USER+113, // wParam = 0 (unchecked), 1 (checked), lParam = 1 (set key focus)
      UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_CLICK_VERIFICATION,
        ( verificationChecked ? new IntPtr( 1 ) : IntPtr.Zero ),
        ( setFocus ? new IntPtr( 1 ) : IntPtr.Zero ) );
    }

    /// <summary>
    /// Enable or disable a button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.Buttons
    /// or a common button ID.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    /// <param name="enable">Enambe the button if true. Disable the button if false.</param>
    public void EnableButton( int buttonId, bool enable ) {
      // TDM_ENABLE_BUTTON = WM_USER+111, // lParam = 0 (disable), lParam != 0 (enable), wParam = Button ID
      UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_ENABLE_BUTTON,
        (IntPtr)buttonId,
        (IntPtr)( enable ? 0 : 1 ) );
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
      // TDM_ENABLE_RADIO_BUTTON = WM_USER+112, // lParam = 0 (disable), lParam != 0 (enable), wParam = Radio Button ID
      UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_ENABLE_RADIO_BUTTON,
        (IntPtr)buttonId,
        (IntPtr)( enable ? 0 : 1 ) );
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
      throw new NotImplementedException( "Not yet implemented" );
      // TDM_NAVIGATE_PAGE = WM_USER+101, // wParam = TASKDIALOGCONFIG structure
      ////    UnsafeNativeMethods.SendMessage(
      ////        this.windowHandle,
      ////        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_NAVIGATE_PAGE,
      ////        IntPtr.Zero,
      ////        //a UnsafeNativeMethods.TASKDIALOGCONFIG value);
    }

    /// <summary>
    /// Designate whether a given Task Dialog button or command link should have a User Account Control (UAC) shield icon.
    /// </summary>
    /// <param name="buttonId">ID of the push button or command link to be updated.</param>
    /// <param name="requiresElevation">False to designate that the action invoked by the button does not require elevation;
    /// true to designate that the action does require elevation.</param>
    public void SetButtonElevationRequiredState( int buttonId, bool requiresElevation ) {
      // TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE = WM_USER+115, // wParam = Button ID, lParam = 0 (elevation not required), lParam != 0 (elevation required)
      UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE,
        (IntPtr)buttonId,
        requiresElevation ? new IntPtr( 1 ) : IntPtr.Zero );
    }

    /// <summary>
    /// Updates the content text.
    /// </summary>
    /// <param name="text">The new value.</param>
    public void SetContentText( string text ) {
      // TDE_CONTENT,
      // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
      UnsafeNativeMethods.SendMessageWithString(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
        (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_CONTENT,
        text );
    }

    /// <summary>
    /// Updates the Expanded Information text.
    /// </summary>
    /// <param name="text">The new value.</param>
    public void SetExpandedInformationText( string text ) {
      // TDE_EXPANDED_INFORMATION,
      // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
      UnsafeNativeMethods.SendMessageWithString(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
        (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_EXPANDED_INFORMATION,
        text );
    }

    /// <summary>
    /// Updates the Footer text.
    /// </summary>
    /// <param name="text">The new value.</param>
    public void SetFooterText( string text ) {
      // TDE_FOOTER,
      // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
      UnsafeNativeMethods.SendMessageWithString(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
        (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_FOOTER,
        text );
    }

    /// <summary>
    /// Updates the Main Instruction.
    /// </summary>
    /// <param name="text">The new value.</param>
    public void SetMainInstructionText( string text ) {
      // TDE_MAIN_INSTRUCTION
      // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
      UnsafeNativeMethods.SendMessageWithString(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
        (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_MAIN_INSTRUCTION,
        text );
    }

    /// <summary>
    /// Used to indicate whether the hosted progress bar should be displayed in marquee mode or not.
    /// </summary>
    /// <param name="setMarquee">Specifies whether the progress bar sbould be shown in Marquee mode.
    /// A value of true turns on Marquee mode.</param>
    public void SetMarqueeProgressBar( bool setMarquee ) {
      // TDM_SET_MARQUEE_PROGRESS_BAR        = WM_USER+103, // wParam = 0 (nonMarque) wParam != 0 (Marquee)
      UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_MARQUEE_PROGRESS_BAR,
        ( setMarquee ? (IntPtr)1 : IntPtr.Zero ),
        IntPtr.Zero );
    }

    /// <summary>
    /// Sets the animation state of the Marquee Progress Bar.
    /// </summary>
    /// <param name="startMarquee">true starts the marquee animation and false stops it.</param>
    /// <param name="speed">The time in milliseconds between refreshes.</param>
    public void SetProgressBarMarquee( bool startMarquee, uint speed ) {
      // TDM_SET_PROGRESS_BAR_MARQUEE        = WM_USER+107, // wParam = 0 (stop marquee), wParam != 0 (start marquee), lparam = speed (milliseconds between repaints)
      UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_MARQUEE,
        ( startMarquee ? new IntPtr( 1 ) : IntPtr.Zero ),
        (IntPtr)speed );
    }

    /// <summary>
    /// Set the current position for a progress bar.
    /// </summary>
    /// <param name="position">The new position.</param>
    /// <returns>Returns the previous value if successful, or zero otherwise.</returns>
    public int SetProgressBarPosition( int position ) {
      // TDM_SET_PROGRESS_BAR_POS            = WM_USER+106, // wParam = new position
      return (int)UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_POS,
        (IntPtr)position,
        IntPtr.Zero );
    }

    /// <summary>
    /// Set the minimum and maximum values for the hosted progress bar.
    /// </summary>
    /// <param name="minimum">Minimum range value. By default, the minimum value is zero.</param>
    /// <param name="maximum">Maximum range value.  By default, the maximum value is 100.</param>
    public void SetProgressBarRange( int minimum, int maximum ) {
      // TDM_SET_PROGRESS_BAR_RANGE          = WM_USER+105, // lParam = MAKELPARAM(nMinRange, nMaxRange)
      // #define MAKELPARAM(l, h)      ((LPARAM)(DWORD)MAKELONG(l, h))
      // #define MAKELONG(a, b)      ((LONG)(((WORD)(((DWORD_PTR)(a)) & 0xffff)) | ((DWORD)((WORD)(((DWORD_PTR)(b)) & 0xffff))) << 16))
      IntPtr lparam = (IntPtr)( ( minimum & 0xffff ) | ( ( maximum & 0xffff ) << 16 ) );
      IntPtr previousRange = UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_RANGE,
        IntPtr.Zero,
        lparam );
    }

    /// <summary>
    /// Sets the state of the progress bar.
    /// </summary>
    /// <param name="state">The state to set the progress bar.</param>
    public void SetProgressBarState( ProgressBarState state ) {
      VistaProgressBarState newState = VistaProgressBarState.Normal;
      if( state == ProgressBarState.Error ) {
        newState = VistaProgressBarState.Error;
      } else if( state == ProgressBarState.Pause ) {
        newState = VistaProgressBarState.Paused;
      }
      // TDM_SET_PROGRESS_BAR_STATE          = WM_USER+104, // wParam = new progress state
      bool success = UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_STATE,
        (IntPtr)newState,
        IntPtr.Zero ) != IntPtr.Zero;
      if( !success ) {
        int lastWin32Error = Marshal.GetLastWin32Error();
        throw new Win32Exception( lastWin32Error, "SendMessage for TDM_SET_PROGRESS_BAR_STATE failed." );
      }
    }

    /// <summary>
    /// Updates the main instruction icon. Note the type (standard via enum or
    /// custom via Icon type) must be used when upating the icon.
    /// </summary>
    /// <param name="icon">The icon to set.</param>
    public void UpdateMainIcon( Icon icon ) {
      // TDM_UPDATE_ICON = WM_USER+116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
      UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ICON,
        (IntPtr)UnsafeNativeMethods.TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_MAIN,
        TranslateIcon( icon ) );
    }

    /// <summary>
    /// Updates the footer icon. Note the type (standard via enum or
    /// custom via Icon type) must be used when upating the icon.
    /// </summary>
    /// <param name="icon">Task Dialog standard icon.</param>
    public void UpdateFooterIcon( Icon icon ) {
      // TDM_UPDATE_ICON = WM_USER+116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
      UnsafeNativeMethods.SendMessage(
        WindowHandle,
        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ICON,
        (IntPtr)UnsafeNativeMethods.TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_FOOTER,
        TranslateIcon( icon ) );
    }
    #endregion


    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
    /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
    /// was dismissed.</param>
    /// <param name="radioButtonResult">The radio botton selected by the user.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    public int Show( IntPtr hwndOwner, out bool verificationFlagChecked, out int radioButtonResult ) {
      // We have to call a private version or PreSharp gets upset about a unsafe
      // block in a public method. (PreSharp error 56505)
      return PrivateShow( hwndOwner, out verificationFlagChecked, out radioButtonResult );
    }

    /// <summary>
    /// Translates a bitfield of the generic CommonButtons into an equivalent of of the VistaTaskDialogCommonButtons type.
    /// </summary>
    /// <param name="buttons">The CommonButtons bietfield that should be translated.</param>
    /// <returns>The equivalent VistaTaskDialogCommonButtons type.</returns>
    private static VistaTaskDialogCommonButtons TranslateCommonButtons( CommonButtons buttons ) {
      VistaTaskDialogCommonButtons result = VistaTaskDialogCommonButtons.None;
      if( ( buttons & CommonButtons.Cancel  ) > 0 ) result |= VistaTaskDialogCommonButtons.Cancel;
      if( ( buttons & CommonButtons.Close   ) > 0 ) result |= VistaTaskDialogCommonButtons.Close;
      if( ( buttons & CommonButtons.No      ) > 0 ) result |= VistaTaskDialogCommonButtons.No;
      if( ( buttons & CommonButtons.Ok      ) > 0 ) result |= VistaTaskDialogCommonButtons.Ok;
      if( ( buttons & CommonButtons.Retry   ) > 0 ) result |= VistaTaskDialogCommonButtons.Retry;
      if( ( buttons & CommonButtons.Yes     ) > 0 ) result |= VistaTaskDialogCommonButtons.Yes;
      return result;
    }

    /// <summary>
    /// Takes a list of TaskDialogButton instances and converts them to a list of VistaTaskDialogButton instances.
    /// The latter is required to communicate with the native API.
    /// </summary>
    /// <param name="buttons">The TaskDialogButton instances to translate.</param>
    /// <returns>A list of VistaTaskDialogButton instances.</returns>
    private static List<VistaTaskDialogButton> TranslateCustomButtons( IEnumerable<TaskDialogButton> buttons ) {
      return buttons.Select( button => new VistaTaskDialogButton( button.ButtonId, button.ButtonText ) ).ToList();
    }

    private static IntPtr TranslateIcon( Icon icon ) {
      switch( icon ) {
        case Icon.None:
          return (IntPtr)VistaTaskDialogIcon.None;

        case Icon.Information:
          return (IntPtr)VistaTaskDialogIcon.Information;

        case Icon.Warning:
          return (IntPtr)VistaTaskDialogIcon.Information;

        case Icon.Error:
          return (IntPtr)VistaTaskDialogIcon.Error;

        case Icon.SecurityWarning:
          return (IntPtr)VistaTaskDialogIcon.SecurityWarning;

        case Icon.SecurityError:
          return (IntPtr)VistaTaskDialogIcon.SecurityError;

        case Icon.SecuritySuccess:
          return (IntPtr)VistaTaskDialogIcon.SecuritySuccess;

        case Icon.SecurityShield:
          return (IntPtr)VistaTaskDialogIcon.SecurityShield;

        case Icon.SecurityShieldBlue:
          return (IntPtr)VistaTaskDialogIcon.SecurityShieldBlue;

        case Icon.SecurityShieldGray:
          return (IntPtr)VistaTaskDialogIcon.SecurityShieldGray;

        case Icon.Custom:
          break;
      }
      throw new ArgumentOutOfRangeException( "icon" );
    }

    /// <summary>
    /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
    /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
    /// as specified on the other members of the class before calling Show.
    /// </summary>
    /// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
    /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
    /// was dismissed.</param>
    /// <param name="radioButtonResult">The radio botton selected by the user.</param>
    /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
    private int PrivateShow( IntPtr hwndOwner, out bool verificationFlagChecked, out int radioButtonResult ) {
      verificationFlagChecked = false;
      radioButtonResult = 0;
      int result;
      UnsafeNativeMethods.TASKDIALOGCONFIG config = new UnsafeNativeMethods.TASKDIALOGCONFIG();

      try {
        config.cbSize           = (uint)Marshal.SizeOf( typeof( UnsafeNativeMethods.TASKDIALOGCONFIG ) );
        config.hwndParent       = hwndOwner;
        config.dwFlags          = Flags;
        config.dwCommonButtons  = TranslateCommonButtons( CommonButtons );

        if( !string.IsNullOrEmpty( WindowTitle ) ) {
          config.pszWindowTitle = WindowTitle;
        }

        config.MainIcon = TranslateIcon( MainIcon );
        if( CustomMainIcon != null ) {
          config.dwFlags |= UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_HICON_MAIN;
          config.MainIcon = CustomMainIcon.Handle;
        }

        if( !string.IsNullOrEmpty( MainInstruction ) ) {
          config.pszMainInstruction = MainInstruction;
        }

        if( !string.IsNullOrEmpty( Content ) ) {
          config.pszContent = Content;
        }

        List<VistaTaskDialogButton> customButtons = TranslateCustomButtons( Buttons );
        if( customButtons.Count > 0 ) {
          // Hand marshal the buttons array.
          int elementSize = Marshal.SizeOf( typeof( VistaTaskDialogButton ) );
          config.pButtons = Marshal.AllocHGlobal( elementSize * customButtons.Count );
          for( int i = 0; i < customButtons.Count; i++ ) {
            // Unsafe because of pointer arithmatic.
            unsafe {
              byte* p = (byte*)config.pButtons;
              Marshal.StructureToPtr( customButtons[ i ], (IntPtr)( p + ( elementSize * i ) ), false );
            }

            config.cButtons++;
          }
        }

        List<VistaTaskDialogButton> customRadioButtons = TranslateCustomButtons( RadioButtons );
        if( customRadioButtons.Count > 0 ) {
          // Hand marshal the buttons array.
          int elementSize = Marshal.SizeOf( typeof( VistaTaskDialogButton ) );
          config.pRadioButtons = Marshal.AllocHGlobal( elementSize * customRadioButtons.Count );
          for( int i = 0; i < customRadioButtons.Count; i++ ) {
            // Unsafe because of pointer arithmatic.
            unsafe {
              byte* p = (byte*)config.pRadioButtons;
              Marshal.StructureToPtr( customRadioButtons[ i ], (IntPtr)( p + ( elementSize * i ) ), false );
            }

            config.cRadioButtons++;
          }
        }

        config.nDefaultButton       = DefaultButton;
        config.nDefaultRadioButton  = DefaultRadioButton;

        if( !string.IsNullOrEmpty( VerificationText ) ) {
          config.pszVerificationText = VerificationText;
        }

        if( !string.IsNullOrEmpty( ExpandedInformation ) ) {
          config.pszExpandedInformation = ExpandedInformation;
        }

        if( !string.IsNullOrEmpty( ExpandedControlText ) ) {
          config.pszExpandedControlText = ExpandedControlText;
        }

        if( !string.IsNullOrEmpty( CollapsedControlText ) ) {
          config.pszCollapsedControlText = CollapsedControlText;
        }

        config.FooterIcon = TranslateIcon( FooterIcon );
        if( CustomFooterIcon != null ) {
          config.dwFlags |= UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_HICON_FOOTER;
          config.FooterIcon = CustomFooterIcon.Handle;
        }

        if( !string.IsNullOrEmpty( Footer ) ) {
          config.pszFooter = Footer;
        }

        // Register our callback
        config.pfCallback = new UnsafeNativeMethods.NativeTaskDialogCallback( PrivateCallback );
        
        ////config.lpCallbackData = this.callbackData; // How do you do this? Need to pin the ref?
        config.cxWidth = (uint)Width;

        // The call all this mucking about is here for.
        UnsafeNativeMethods.TaskDialogIndirect(
          ref config,
          out result,
          out radioButtonResult,
          out verificationFlagChecked );

      } finally {
        // Free the unmanged memory needed for the button arrays.
        // There is the possiblity of leaking memory if the app-domain is destroyed in a non clean way
        // and the hosting OS process is kept alive but fixing this would require using hardening techniques
        // that are not required for the users of this class.
        if( config.pButtons != IntPtr.Zero ) {
          int elementSize = Marshal.SizeOf( typeof( VistaTaskDialogButton ) );
          for( int i = 0; i < config.cButtons; i++ ) {
            unsafe {
              byte* p = (byte*)config.pButtons;
              Marshal.DestroyStructure( (IntPtr)( p + ( elementSize * i ) ), typeof( VistaTaskDialogButton ) );
            }
          }

          Marshal.FreeHGlobal( config.pButtons );
        }

        if( config.pRadioButtons != IntPtr.Zero ) {
          int elementSize = Marshal.SizeOf( typeof( VistaTaskDialogButton ) );
          for( int i = 0; i < config.cRadioButtons; i++ ) {
            unsafe {
              byte* p = (byte*)config.pRadioButtons;
              Marshal.DestroyStructure( (IntPtr)( p + ( elementSize * i ) ), typeof( VistaTaskDialogButton ) );
            }
          }

          Marshal.FreeHGlobal( config.pRadioButtons );
        }
      }

      return result;
    }

    /// <summary>
    /// The callback from the native Task Dialog. This prepares the friendlier arguments and calls the simplier callback.
    /// </summary>
    /// <param name="hwnd">The window handle of the Task Dialog that is active.</param>
    /// <param name="msg">The notification. A TaskDialogNotification value.</param>
    /// <param name="wparam">Specifies additional noitification information.  The contents of this parameter depends on the value of the msg parameter.</param>
    /// <param name="lparam">Specifies additional noitification information.  The contents of this parameter depends on the value of the msg parameter.</param>
    /// <param name="refData">Specifies the application-defined value given in the call to TaskDialogIndirect.</param>
    /// <returns>A HRESULT. It's not clear in the spec what a failed result will do.</returns>
    private int PrivateCallback(
      [In] IntPtr hwnd,
      [In] uint msg,
      [In] UIntPtr wparam,
      [In] IntPtr lparam,
      [In] IntPtr refData ) {
      // Prepare arguments for the callback to the user we are insulating from Interop casting sillyness.

      // Future: Consider reusing a single ActiveTaskDialog object and mark it as destroyed on the destry notification.
      //NativeTaskDialog activeDialog = new NativeTaskDialog( hwnd );
      NativeTaskDialogNotificationArgs args = new NativeTaskDialogNotificationArgs {
        Notification =
          (VistaTaskDialogNotification)msg
      };
      switch( args.Notification ) {
        case VistaTaskDialogNotification.ButtonClicked:
        case VistaTaskDialogNotification.RadioButtonClicked:
          args.ButtonId = (int)wparam;
          break;
        case VistaTaskDialogNotification.HyperlinkClicked:
          args.Hyperlink = Marshal.PtrToStringUni( lparam );
          break;
        case VistaTaskDialogNotification.Timer:
          args.TimerTickCount = (uint)wparam;
          break;
        case VistaTaskDialogNotification.VerificationClicked:
          args.VerificationFlagChecked = ( wparam != UIntPtr.Zero );
          break;
        case VistaTaskDialogNotification.ExpandButtonClicked:
          args.Expanded = ( wparam != UIntPtr.Zero );
          break;
      }
      
      switch( args.Notification ) {
        case VistaTaskDialogNotification.Created:
          if( null != Created ) Created( this, EventArgs.Empty );
          break;
          
        case VistaTaskDialogNotification.Navigated:
          if( null != Navigated ) Navigated( this, EventArgs.Empty );
          break;

        case VistaTaskDialogNotification.ButtonClicked:
          if( null != ButtonClicked ) {
            ButtonClickedArgs buttonClickedArgs = new ButtonClickedArgs( args.ButtonId );
            ButtonClicked( this, buttonClickedArgs );
            if( buttonClickedArgs.PreventClosing ) return 1;
          }
          break;

        case VistaTaskDialogNotification.HyperlinkClicked:
          if( null != HyperlinkClicked ) HyperlinkClicked( this, new HyperlinkClickedArgs( args.Hyperlink ) );
          break;

        case VistaTaskDialogNotification.Timer:
          if( null != Timer ) Timer( this, new TimerArgs( args.TimerTickCount ) );
          break;

        case VistaTaskDialogNotification.Destroyed:
          if( null != Destroyed ) Destroyed( this, EventArgs.Empty );
          break;

        case VistaTaskDialogNotification.RadioButtonClicked:
          if( null != RadioButtonClicked ) RadioButtonClicked( this, new ButtonClickedArgs( args.ButtonId ) );
          break;

        case VistaTaskDialogNotification.DialogConstructed:
          WindowHandle = hwnd;
          if( null != DialogConstructed ) DialogConstructed( this, EventArgs.Empty );
          break;

        case VistaTaskDialogNotification.VerificationClicked:
          if( null != VerificationClicked ) VerificationClicked( this, EventArgs.Empty );
          break;

        case VistaTaskDialogNotification.Help:
          if( null != Help ) Help( this, EventArgs.Empty );
          break;

        case VistaTaskDialogNotification.ExpandButtonClicked:
          if( null != ExpandoButtonClicked ) ExpandoButtonClicked( this, EventArgs.Empty );
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }
      return 0; // false;
    }

    /// <summary>
    /// Helper function to set or clear a bit in the flags field.
    /// </summary>
    /// <param name="flag">The Flag bit to set or clear.</param>
    /// <param name="value">True to set, false to clear the bit in the flags field.</param>
    private void SetFlag( UnsafeNativeMethods.TASKDIALOG_FLAGS flag, bool value ) {
      if( value ) {
        Flags |= flag;

      } else {
        Flags &= ~flag;
      }
    }
    #endregion

  }
}
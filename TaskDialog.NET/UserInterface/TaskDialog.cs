using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TaskDialogNet.UserInterface {
  public static class TaskDialog {

    // PUBLIC static values...
    /*
    public static int EmulatedFormWidth = 450;
    public static bool UseToolWindowOnXp = true;
    public static bool PlaySystemSounds = true;
    */
    public static bool ForceEmulationMode;
    public static bool VerificationChecked;
    public static int ButtonResult = -1;
    public static int RadioButtonResult = -1;
    public static int CommandButtonResult = -1;

    /// <summary>
    /// The last used TaskDialogConfig instance.
    /// </summary>
    public static TaskDialogConfig TaskConfig { get; set; }

    #region Events
    public static event TaskDialogEventHandler<ButtonClickedArgs> ButtonClicked;
    public static event TaskDialogEventHandler Created;
    public static event TaskDialogEventHandler Destroyed;
    public static event TaskDialogEventHandler DialogConstructed;
    public static event TaskDialogEventHandler<ExpandoButtonClickedArgs> ExpandoButtonClicked;
    public static event TaskDialogEventHandler Help;
    public static event TaskDialogEventHandler<HyperlinkClickedArgs> HyperlinkClicked;
    public static event TaskDialogEventHandler Navigated;
    public static event TaskDialogEventHandler<ButtonClickedArgs> RadioButtonClicked;
    public static event TaskDialogEventHandler<TimerArgs> Timer;
    public static event TaskDialogEventHandler VerificationClicked;
    
    #region Event Invoker
    private static void InvokeButtonClicked( ITaskDialog sender, ButtonClickedArgs e ) {
      TaskDialogEventHandler<ButtonClickedArgs> handler = ButtonClicked;
      if( handler != null ) handler( sender, e );
    }

    private static void InvokeCreated( ITaskDialog sender, EventArgs e ) {
      TaskDialogEventHandler handler = Created;
      if( handler != null ) handler( sender, e );
    }

    private static void InvokeDestroyed( ITaskDialog sender, EventArgs e ) {
      TaskDialogEventHandler handler = Destroyed;
      if( handler != null ) handler( sender, e );
    }

    private static void InvokeDialogConstructed( ITaskDialog sender, EventArgs e ) {
      TaskDialogEventHandler handler = DialogConstructed;
      if( handler != null ) handler( sender, e );
    }

    private static void InvokeExpandoButtonClicked( ITaskDialog sender, ExpandoButtonClickedArgs e ) {
      TaskDialogEventHandler<ExpandoButtonClickedArgs> handler = ExpandoButtonClicked;
      if( handler != null ) handler( sender, e );
    }

    private static void InvokeHelp( ITaskDialog sender, EventArgs e ) {
      TaskDialogEventHandler handler = Help;
      if( handler != null ) handler( sender, e );
    }

    private static void InvokeHyperlinkClicked( ITaskDialog sender, HyperlinkClickedArgs e ) {
      TaskDialogEventHandler<HyperlinkClickedArgs> handler = HyperlinkClicked;
      if( handler != null ) handler( sender, e );
    }
    
    private static void InvokeNavigated( ITaskDialog sender, EventArgs e ) {
      TaskDialogEventHandler handler = Navigated;
      if( handler != null ) handler( sender, e );
    }
    
    private static void InvokeRadioButtonClicked( ITaskDialog sender, ButtonClickedArgs e ) {
      TaskDialogEventHandler<ButtonClickedArgs> handler = RadioButtonClicked;
      if( handler != null ) handler( sender, e );
    }

    private static void InvokeTimer( ITaskDialog sender, TimerArgs e ) {
      TaskDialogEventHandler<TimerArgs> handler = Timer;
      if( handler != null ) handler( sender, e );
    }

    private static void InvokeVerificationClicked( ITaskDialog sender, EventArgs e ) {
      TaskDialogEventHandler handler = VerificationClicked;
      if( handler != null ) handler( sender, e );
    }
    #endregion
    #endregion

    /// <summary>
    /// Create a TaskDialog from a TaskDialogConfig instance.
    /// </summary>
    /// <param name="taskConfig">A TaskDialogConfig instance that describes the TaskDialog.</param>
    /// <param name="button">The button that was clicked to close the TaskDialog.</param>
    /// <param name="radioButton">The radio button that was selected in the TaskDialog.</param>
    /// <param name="verificationFlagChecked">true if the verification checkbox was checked; false otherwise.</param>
    /// <returns></returns>
    public static int TaskDialogIndirect( TaskDialogConfig taskConfig, out int button, out int radioButton,
                            out bool verificationFlagChecked ) {
      ITaskDialog taskDialog;
      if( NativeTaskDialog.IsAvailableOnThisOs && !ForceEmulationMode ) {
        taskDialog = new NativeTaskDialog();

      } else {
        taskDialog = new EmulatedTaskDialog( false );
      }

      return taskDialog.TaskDialogIndirect( taskConfig, out button, out radioButton, out verificationFlagChecked );
    }

    #region ShowTaskDialogBox
    public static DialogResult ShowTaskDialogBox( IntPtr owner,
                                                  string title,
                                                  string mainInstruction,
                                                  string content,
                                                  string expandedInfo,
                                                  string footer,
                                                  string verificationText,
                                                  string radioButtons,
                                                  string commandButtons,
                                                  CommonButtons buttons,
                                                  CommonIcon mainIcon,
                                                  CommonIcon footerIcon,
                                                  int defaultIndex,
                                                  ProgressBarStyle progressBarStyle ) {
      ITaskDialog taskDialog;
      if( NativeTaskDialog.IsAvailableOnThisOs && !ForceEmulationMode ) {
        taskDialog = new NativeTaskDialog();

      } else {
        taskDialog = new EmulatedTaskDialog( false );
      }

      TaskConfig = new TaskDialogConfig();

      TaskConfig.Parent               = owner;
      TaskConfig.WindowTitle          = title;
      TaskConfig.MainInstruction      = mainInstruction;
      TaskConfig.Content              = content;
      TaskConfig.ExpandedInformation  = expandedInfo;
      TaskConfig.Footer               = footer;

      // Radio Buttons
      if( !string.IsNullOrEmpty( radioButtons ) ) {
        List<TaskDialogButton> lst = new List<TaskDialogButton>();
        string[] arr = radioButtons.Split( new[] {'|'} );
        for( int i = 0; i < arr.Length; i++ ) {
          try {
            TaskDialogButton button = new TaskDialogButton { ButtonId = 1000 + i, ButtonText = arr[ i ] };
            lst.Add( button );
          } catch( FormatException ) {}
        }
        TaskConfig.RadioButtons.AddRange( lst );
        TaskConfig.Flags.NoDefaultRadioButton = ( defaultIndex == -1 );
        if( defaultIndex >= 0 ) {
          TaskConfig.DefaultRadioButton = defaultIndex + 1000;
        } else {
          TaskConfig.DefaultRadioButton = 1000;
        }
      }

      // Custom Buttons
      if( !string.IsNullOrEmpty( commandButtons ) ) {
        List<TaskDialogButton> lst = new List<TaskDialogButton>();
        string[] arr = commandButtons.Split( new[] {'|'} );
        for( int i = 0; i < arr.Length; i++ ) {
          try {
            TaskDialogButton button = new TaskDialogButton { ButtonId = 2000 + i, ButtonText = arr[ i ] };
            lst.Add( button );
          } catch( FormatException ) {}
        }
        TaskConfig.Buttons.AddRange( lst );
        if( defaultIndex >= 0 ) {
          TaskConfig.DefaultButton = defaultIndex;
        }
      }

      TaskConfig.CommonButtons = buttons;
      TaskConfig.MainIcon = mainIcon;
      if( TaskConfig.MainIcon == CommonIcon.Custom ) throw new ArgumentException( "Not supported yet.", "mainIcon" );
      TaskConfig.FooterIcon = footerIcon;
      if( TaskConfig.FooterIcon == CommonIcon.Custom ) throw new ArgumentException( "Not supported yet.", "footerIcon" );

      TaskConfig.Flags.EnableHyperLinks         = true;
      TaskConfig.Flags.ShowProgressBar          = ( progressBarStyle == ProgressBarStyle.Continous ) ? true : false;
      TaskConfig.Flags.ShowMarqueeProgressBar   = ( progressBarStyle == ProgressBarStyle.Marquee ) ? true : false;
      TaskConfig.Flags.AllowDialogCancellation  = ( ( buttons & CommonButtons.Cancel ) >= 0 || 
                                                    ( buttons & CommonButtons.Close  ) >= 0 );
      
      TaskConfig.Flags.CallbackTimer            = true;
      TaskConfig.Flags.ExpandedByDefault        = false;
      TaskConfig.Flags.ExpandFooterArea         = false;
      TaskConfig.Flags.PositionRelativeToWindow = true;
      TaskConfig.Flags.RtlLayout                = false;
      TaskConfig.Flags.CanBeMinimized           = false;
      TaskConfig.Flags.UseCommandLinks          = ( TaskConfig.Buttons.Count > 0 );
      TaskConfig.Flags.UseCommandLinksNoIcon    = false;
      TaskConfig.VerificationText               = verificationText;
      TaskConfig.Flags.VerificationFlagChecked  = false;
      TaskConfig.ExpandedControlText            = "Hide details";
      TaskConfig.CollapsedControlText           = "Show details";

      taskDialog.Created              += TaskDialogCreated;
      taskDialog.ButtonClicked        += TaskDialogButtonClicked;
      taskDialog.HyperlinkClicked     += TaskDialogHyperlinkClicked;
      taskDialog.Timer                += TaskDialogTimer;
      taskDialog.Destroyed            += TaskDialogDestroyed;
      taskDialog.RadioButtonClicked   += TaskDialogRadioButtonClicked;
      taskDialog.DialogConstructed    += TaskDialogDialogConstructed;
      taskDialog.VerificationClicked  += TaskDialogVerificationClicked;
      taskDialog.Help                 += TaskDialogHelp;
      taskDialog.ExpandoButtonClicked += TaskDialogExpandoButtonClicked;
      taskDialog.Navigated            += TaskDialogNavigated;

      DialogResult dialogResult = DialogResult.None;
      int result = taskDialog.TaskDialogIndirect( TaskConfig, out ButtonResult, out RadioButtonResult, out VerificationChecked );

      // if a command button was clicked, then change return result
      // to "DialogResult.OK" and set the CommandButtonResult)
      if( result >= 2000 ) {
        CommandButtonResult = result - 2000;
        dialogResult = DialogResult.OK;
      }
      if( RadioButtonResult >= 1000 ) {
        // deduct the ButtonID start value for radio buttons
        RadioButtonResult -= 1000;
      }

      return dialogResult;
    }

    static void TaskDialogCreated( ITaskDialog sender, EventArgs e ) {
      InvokeCreated( sender, e );
    }

    private static void TaskDialogButtonClicked( ITaskDialog sender, EventArgs e ) {
      InvokeButtonClicked( sender, e as ButtonClickedArgs );
    }

    private static void TaskDialogHyperlinkClicked( ITaskDialog sender, EventArgs e ) {
      InvokeHyperlinkClicked( sender, e as HyperlinkClickedArgs );
    }

    private static void TaskDialogTimer( ITaskDialog sender, TimerArgs e ) {
      InvokeTimer( sender, e );
    }

    private static void TaskDialogDestroyed( ITaskDialog sender, EventArgs e ) {
      InvokeDestroyed( sender, e );
    }

    private static void TaskDialogRadioButtonClicked( ITaskDialog sender, EventArgs e ) {
      InvokeRadioButtonClicked( sender, e as ButtonClickedArgs );
    }

    static void TaskDialogDialogConstructed( ITaskDialog sender, EventArgs e ) {
      InvokeDialogConstructed( sender, e );
    }

    private static void TaskDialogVerificationClicked( ITaskDialog sender, EventArgs e ) {
      InvokeVerificationClicked( sender, e );
    }

    private static void TaskDialogHelp( ITaskDialog sender, EventArgs e ) {
      InvokeHelp( sender, e );
    }

    private static void TaskDialogExpandoButtonClicked( ITaskDialog sender, ExpandoButtonClickedArgs e ) {
      InvokeExpandoButtonClicked( sender, e );
    }

    private static void TaskDialogNavigated( ITaskDialog sender, EventArgs args ) {
      InvokeNavigated( sender, args );
    }

    // Overloaded versions...
    public static DialogResult ShowTaskDialogBox( IntPtr owner,
                                                  string title,
                                                  string mainInstruction,
                                                  string content,
                                                  string expandedInfo,
                                                  string footer,
                                                  string verificationText,
                                                  string radioButtons,
                                                  string commandButtons,
                                                  CommonButtons buttons,
                                                  CommonIcon mainIcon,
                                                  CommonIcon footerIcon,
                                                  ProgressBarStyle progressBarStyle ) {
      return ShowTaskDialogBox( owner, title, mainInstruction, content, expandedInfo, footer, verificationText,
                                radioButtons, commandButtons, buttons, mainIcon, footerIcon, 0, progressBarStyle );
    }

    public static DialogResult ShowTaskDialogBox( string title,
                                                  string mainInstruction,
                                                  string content,
                                                  string expandedInfo,
                                                  string footer,
                                                  string verificationText,
                                                  string radioButtons,
                                                  string commandButtons,
                                                  CommonButtons buttons,
                                                  CommonIcon mainIcon,
                                                  CommonIcon footerIcon,
                                                  ProgressBarStyle progressBarStyle ) {
      return ShowTaskDialogBox( IntPtr.Zero, title, mainInstruction, content, expandedInfo, footer, verificationText,
                                radioButtons, commandButtons, buttons, mainIcon, footerIcon, 0, progressBarStyle );
    }
    #endregion


    #region MessageBox
    public static DialogResult MessageBox( IntPtr owner,
                                           string title,
                                           string mainInstruction,
                                           string content,
                                           string expandedInfo,
                                           string footer,
                                           string verificationText,
                                           CommonButtons buttons,
                                           CommonIcon mainIcon,
                                           CommonIcon footerIcon,
                                           ProgressBarStyle progressBarStyle ) {
      return ShowTaskDialogBox( owner, title, mainInstruction, content, expandedInfo, footer, verificationText, "", "",
                                buttons, mainIcon, footerIcon, progressBarStyle );
    }

    // Overloaded versions...
    public static DialogResult MessageBox( string title,
                                           string mainInstruction,
                                           string content,
                                           string expandedInfo,
                                           string footer,
                                           string verificationText,
                                           CommonButtons buttons,
                                           CommonIcon mainIcon,
                                           CommonIcon footerIcon,
                                           ProgressBarStyle progressBarStyle ) {
      return ShowTaskDialogBox( IntPtr.Zero, title, mainInstruction, content, expandedInfo, footer, verificationText, "", "",
                                buttons, mainIcon, footerIcon, progressBarStyle );
    }

    public static DialogResult MessageBox( IntPtr owner,
                                           string title,
                                           string mainInstruction,
                                           string content,
                                           CommonButtons buttons,
                                           CommonIcon mainIcon,
                                           ProgressBarStyle progressBarStyle ) {
      return MessageBox( owner, title, mainInstruction, content, "", "", "", buttons, mainIcon, CommonIcon.Information, progressBarStyle );
    }

    public static DialogResult MessageBox( string title,
                                           string mainInstruction,
                                           string content,
                                           CommonButtons buttons,
                                           CommonIcon mainIcon,
                                           ProgressBarStyle progressBarStyle ) {
      return MessageBox( IntPtr.Zero, title, mainInstruction, content, "", "", "", buttons, mainIcon, CommonIcon.Information, progressBarStyle );
    }
    #endregion

    #region ShowRadioBox
    public static int ShowRadioBox( IntPtr owner,
                                    string title,
                                    string mainInstruction,
                                    string content,
                                    string expandedInfo,
                                    string footer,
                                    string verificationText,
                                    string radioButtons,
                                    CommonIcon mainIcon,
                                    CommonIcon footerIcon,
                                    int defaultIndex ) {
      DialogResult res = ShowTaskDialogBox( owner, title, mainInstruction, content, expandedInfo, footer,
                                            verificationText,
                                            radioButtons, "", CommonButtons.Ok | CommonButtons.Cancel, mainIcon, footerIcon,
                                            defaultIndex, ProgressBarStyle.None );
      if( res == DialogResult.OK )
        return RadioButtonResult;
      
      return -1;
    }

    // Overloaded versions...
    public static int ShowRadioBox( string title,
                                    string mainInstruction,
                                    string content,
                                    string expandedInfo,
                                    string footer,
                                    string verificationText,
                                    string radioButtons,
                                    CommonIcon mainIcon,
                                    CommonIcon footerIcon,
                                    int defaultIndex ) {
      DialogResult res = ShowTaskDialogBox( IntPtr.Zero, title, mainInstruction, content, expandedInfo, footer,
                                            verificationText,
                                            radioButtons, "", CommonButtons.Ok | CommonButtons.Cancel, mainIcon, footerIcon,
                                            defaultIndex, ProgressBarStyle.None );
      if( res == DialogResult.OK )
        return RadioButtonResult;
      
      return -1;
    }

    public static int ShowRadioBox( IntPtr owner,
                                    string title,
                                    string mainInstruction,
                                    string content,
                                    string expandedInfo,
                                    string footer,
                                    string verificationText,
                                    string radioButtons,
                                    CommonIcon mainIcon,
                                    CommonIcon footerIcon ) {
      return ShowRadioBox( owner, title, mainInstruction, content, expandedInfo, footer, verificationText, radioButtons,
                           CommonIcon.None, CommonIcon.Information, 0 );
    }

    public static int ShowRadioBox( IntPtr owner,
                                    string title,
                                    string mainInstruction,
                                    string content,
                                    string radioButtons,
                                    int defaultIndex ) {
      return ShowRadioBox( owner, title, mainInstruction, content, "", "", "", radioButtons, CommonIcon.None,
                           CommonIcon.Information, defaultIndex );
    }

    public static int ShowRadioBox( IntPtr owner,
                                    string title,
                                    string mainInstruction,
                                    string content,
                                    string radioButtons ) {
      return ShowRadioBox( owner, title, mainInstruction, content, "", "", "", radioButtons, CommonIcon.None,
                           CommonIcon.Information, 0 );
    }

    public static int ShowRadioBox( string title,
                                    string mainInstruction,
                                    string content,
                                    string radioButtons ) {
      return ShowRadioBox( IntPtr.Zero, title, mainInstruction, content, "", "", "", radioButtons, CommonIcon.None,
                           CommonIcon.Information, 0 );
    }
    #endregion

    #region ShowCommandBox
    public static int ShowCommandBox( IntPtr owner,
                                      string title,
                                      string mainInstruction,
                                      string content,
                                      string expandedInfo,
                                      string footer,
                                      string verificationText,
                                      string commandButtons,
                                      bool showCancelButton,
                                      CommonIcon mainIcon,
                                      CommonIcon footerIcon,
                                      ProgressBarStyle progressBarStyle ) {
      DialogResult res = ShowTaskDialogBox( owner, title, mainInstruction, content, expandedInfo, footer,
                                            verificationText,
                                            "", commandButtons,
                                            ( showCancelButton ? CommonButtons.Cancel : CommonButtons.None ),
                                            mainIcon, footerIcon, progressBarStyle );
      if( res == DialogResult.OK )
        return CommandButtonResult;
      
      return -1;
    }

    // Overloaded versions...
    public static int ShowCommandBox( string title,
                                      string mainInstruction,
                                      string content,
                                      string expandedInfo,
                                      string footer,
                                      string verificationText,
                                      string commandButtons,
                                      bool showCancelButton,
                                      CommonIcon mainIcon,
                                      CommonIcon footerIcon,
                                      ProgressBarStyle progressBarStyle ) {
      DialogResult res = ShowTaskDialogBox( IntPtr.Zero, title, mainInstruction, content, expandedInfo, footer,
                                            verificationText,
                                            "", commandButtons,
                                            ( showCancelButton ? CommonButtons.Cancel : CommonButtons.None ),
                                            mainIcon, footerIcon, progressBarStyle );
      if( res == DialogResult.OK )
        return CommandButtonResult;
      
      return -1;
    }

    public static int ShowCommandBox( IntPtr owner, string title, string mainInstruction, string content,
                                      string commandButtons, bool showCancelButton, ProgressBarStyle progressBarStyle ) {
      return ShowCommandBox( owner, title, mainInstruction, content, "", "", "", commandButtons, showCancelButton,
                             CommonIcon.None, CommonIcon.Information, progressBarStyle );
    }

    public static int ShowCommandBox( string title, string mainInstruction, string content, string commandButtons,
                                      bool showCancelButton, ProgressBarStyle progressBarStyle ) {
      return ShowCommandBox( IntPtr.Zero, title, mainInstruction, content, "", "", "", commandButtons, showCancelButton,
                             CommonIcon.None, CommonIcon.Information, progressBarStyle );
    }
    #endregion
  }
}
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
    public static int RadioButtonResult = -1;
    public static int CommandButtonResult = -1;

    #region Events
    public static event TaskDialogEventHandler<ButtonClickedArgs> ButtonClicked;
    public static event TaskDialogEventHandler Created;
    public static event TaskDialogEventHandler Destroyed;
    public static event TaskDialogEventHandler DialogConstructed;
    public static event TaskDialogEventHandler ExpandoButtonClicked;
    public static event TaskDialogEventHandler Help;
    public static event TaskDialogEventHandler<HyperlinkClickedArgs> HyperlinkClicked;
    //public static event TaskDialogEventHandler Navigated;
    public static event TaskDialogEventHandler<ButtonClickedArgs> RadioButtonClicked;
    public static event TaskDialogEventHandler Timer;
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

    private static void InvokeExpandoButtonClicked( ITaskDialog sender, EventArgs e ) {
      TaskDialogEventHandler handler = ExpandoButtonClicked;
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
    /*
    private static void InvokeNavigated( ITaskDialog sender, EventArgs e ) {
      TaskDialogEventHandler handler = Navigated;
      if( handler != null ) handler( sender, e );
    }
    */
    private static void InvokeRadioButtonClicked( ITaskDialog sender, ButtonClickedArgs e ) {
      TaskDialogEventHandler<ButtonClickedArgs> handler = RadioButtonClicked;
      if( handler != null ) handler( sender, e );
    }

    private static void InvokeTimer( ITaskDialog sender, EventArgs e ) {
      TaskDialogEventHandler handler = Timer;
      if( handler != null ) handler( sender, e );
    }

    private static void InvokeVerificationClicked( ITaskDialog sender, EventArgs e ) {
      TaskDialogEventHandler handler = VerificationClicked;
      if( handler != null ) handler( sender, e );
    }
    #endregion
    #endregion

    //--------------------------------------------------------------------------------

    public static void SetProgressBarProgress( int progress ) {
      
    }

    #region ShowTaskDialogBox

    //--------------------------------------------------------------------------------
    public static DialogResult ShowTaskDialogBox( IWin32Window owner,
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

      taskDialog.WindowTitle          = title;
      taskDialog.MainInstruction      = mainInstruction;
      taskDialog.Content              = content;
      taskDialog.ExpandedInformation  = expandedInfo;
      taskDialog.Footer               = footer;

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
        taskDialog.RadioButtons.AddRange( lst );
        taskDialog.NoDefaultRadioButton = ( defaultIndex == -1 );
        if( defaultIndex >= 0 ) {
          taskDialog.DefaultRadioButton = defaultIndex;
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
        taskDialog.Buttons.AddRange( lst );
        if( defaultIndex >= 0 ) {
          taskDialog.DefaultButton = defaultIndex;
        }
      }

      taskDialog.CommonButtons = buttons;
      taskDialog.MainIcon = mainIcon;
      if( taskDialog.MainIcon == CommonIcon.Custom ) throw new ArgumentException( "Not supported yet.", "mainIcon" );
      taskDialog.FooterIcon = footerIcon;
      if( taskDialog.FooterIcon == CommonIcon.Custom ) throw new ArgumentException( "Not supported yet.", "footerIcon" );

      taskDialog.EnableHyperlinks         = true;
      taskDialog.ShowProgressBar          = ( progressBarStyle == ProgressBarStyle.Continous ) ? true : false;
      taskDialog.ShowMarqueeProgressBar   = ( progressBarStyle == ProgressBarStyle.Marquee ) ? true : false;
      taskDialog.AllowDialogCancellation  = ( ( buttons & CommonButtons.Cancel ) >= 0 || 
                                             ( buttons & CommonButtons.Close  ) >= 0 );
      
      taskDialog.CallbackTimer            = true;
      taskDialog.ExpandedByDefault        = false;
      taskDialog.ExpandFooterArea         = false;
      taskDialog.PositionRelativeToWindow = true;
      taskDialog.RtlLayout                = false;
      taskDialog.CanBeMinimized           = false;
      taskDialog.UseCommandLinks          = ( taskDialog.Buttons.Count > 0 );
      taskDialog.UseCommandLinksNoIcon    = false;
      taskDialog.VerificationText         = verificationText;
      taskDialog.VerificationFlagChecked  = false;
      taskDialog.ExpandedControlText      = "Hide details";
      taskDialog.CollapsedControlText     = "Show details";

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

      DialogResult dialogResult = DialogResult.None;
      int result = taskDialog.Show( taskDialog.CanBeMinimized ? null : owner, out VerificationChecked, out RadioButtonResult );

      // if a command button was clicked, then change return result
      // to "DialogResult.OK" and set the CommandButtonResult
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

    private static void TaskDialogTimer( ITaskDialog sender, EventArgs e ) {
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

    private static void TaskDialogExpandoButtonClicked( ITaskDialog sender, EventArgs e ) {
      InvokeExpandoButtonClicked( sender, EventArgs.Empty );
    }

    //--------------------------------------------------------------------------------
    // Overloaded versions...
    //--------------------------------------------------------------------------------
    public static DialogResult ShowTaskDialogBox( IWin32Window owner,
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
      return ShowTaskDialogBox( null, title, mainInstruction, content, expandedInfo, footer, verificationText,
                                radioButtons, commandButtons, buttons, mainIcon, footerIcon, 0, progressBarStyle );
    }

    #endregion

    //--------------------------------------------------------------------------------

    #region MessageBox

    //--------------------------------------------------------------------------------
    public static DialogResult MessageBox( IWin32Window owner,
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

    //--------------------------------------------------------------------------------
    // Overloaded versions...
    //--------------------------------------------------------------------------------
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
      return ShowTaskDialogBox( null, title, mainInstruction, content, expandedInfo, footer, verificationText, "", "",
                                buttons, mainIcon, footerIcon, progressBarStyle );
    }

    public static DialogResult MessageBox( IWin32Window owner,
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
      return MessageBox( null, title, mainInstruction, content, "", "", "", buttons, mainIcon, CommonIcon.Information, progressBarStyle );
    }

    //--------------------------------------------------------------------------------

    #endregion

    //--------------------------------------------------------------------------------

    #region ShowRadioBox

    //--------------------------------------------------------------------------------
    public static int ShowRadioBox( IWin32Window owner,
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

    //--------------------------------------------------------------------------------
    // Overloaded versions...
    //--------------------------------------------------------------------------------
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
      DialogResult res = ShowTaskDialogBox( null, title, mainInstruction, content, expandedInfo, footer,
                                            verificationText,
                                            radioButtons, "", CommonButtons.Ok | CommonButtons.Cancel, mainIcon, footerIcon,
                                            defaultIndex, ProgressBarStyle.None );
      if( res == DialogResult.OK )
        return RadioButtonResult;
      
      return -1;
    }

    public static int ShowRadioBox( IWin32Window owner,
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

    public static int ShowRadioBox( IWin32Window owner,
                                    string title,
                                    string mainInstruction,
                                    string content,
                                    string radioButtons,
                                    int defaultIndex ) {
      return ShowRadioBox( owner, title, mainInstruction, content, "", "", "", radioButtons, CommonIcon.None,
                           CommonIcon.Information, defaultIndex );
    }

    public static int ShowRadioBox( IWin32Window owner,
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
      return ShowRadioBox( null, title, mainInstruction, content, "", "", "", radioButtons, CommonIcon.None,
                           CommonIcon.Information, 0 );
    }

    #endregion

    //--------------------------------------------------------------------------------

    #region ShowCommandBox

    //--------------------------------------------------------------------------------
    public static int ShowCommandBox( IWin32Window owner,
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

    //--------------------------------------------------------------------------------
    // Overloaded versions...
    //--------------------------------------------------------------------------------
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
      DialogResult res = ShowTaskDialogBox( null, title, mainInstruction, content, expandedInfo, footer,
                                            verificationText,
                                            "", commandButtons,
                                            ( showCancelButton ? CommonButtons.Cancel : CommonButtons.None ),
                                            mainIcon, footerIcon, progressBarStyle );
      if( res == DialogResult.OK )
        return CommandButtonResult;
      
      return -1;
    }

    public static int ShowCommandBox( IWin32Window owner, string title, string mainInstruction, string content,
                                      string commandButtons, bool showCancelButton, ProgressBarStyle progressBarStyle ) {
      return ShowCommandBox( owner, title, mainInstruction, content, "", "", "", commandButtons, showCancelButton,
                             CommonIcon.None, CommonIcon.Information, progressBarStyle );
    }

    public static int ShowCommandBox( string title, string mainInstruction, string content, string commandButtons,
                                      bool showCancelButton, ProgressBarStyle progressBarStyle ) {
      return ShowCommandBox( null, title, mainInstruction, content, "", "", "", commandButtons, showCancelButton,
                             CommonIcon.None, CommonIcon.Information, progressBarStyle );
    }

    #endregion

    //--------------------------------------------------------------------------------
  }
}
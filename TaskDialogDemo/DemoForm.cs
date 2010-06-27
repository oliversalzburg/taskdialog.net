using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaskDialogNet.UserInterface;
using ProgressBarStyle = TaskDialogNet.UserInterface.ProgressBarStyle;

namespace TaskDialogDemo {
  public partial class DemoForm : Form {

    private bool ResetCallbackTimer { get; set; }

    public DemoForm() {
      InitializeComponent();
    }

    private void button1_Click( object sender, EventArgs e ) {
      TaskDialog.ButtonClicked        += TaskDialog_ButtonClicked;
      TaskDialog.Created              += TaskDialog_Created;
      TaskDialog.Destroyed            += TaskDialog_Destroyed;
      TaskDialog.DialogConstructed    += TaskDialog_DialogConstructed;
      TaskDialog.ExpandoButtonClicked += TaskDialog_ExpandoButtonClicked;
      TaskDialog.Help                 += TaskDialog_Help;
      TaskDialog.HyperlinkClicked     += TaskDialog_HyperlinkClicked;
      TaskDialog.Navigated            += TaskDialog_Navigated;
      TaskDialog.RadioButtonClicked   += TaskDialog_RadioButtonClicked;
      TaskDialog.Timer                += TaskDialog_Timer;

      TaskDialog.ForceEmulationMode = forceEmulationCheckBox.Checked;
      DialogResult result = TaskDialog.ShowTaskDialogBox( this,
        "TaskDialog.Net Demo",
        "mainInstruction",
        "content",
        "Do you require additional information regarding this matter?",
        "Do you require <A HREF=\"some link\">more information</A>?",
        "This is the so-called verification text. It is quite long to check common button layout.",
        "I like turtles|I leik mudkips",
        "Navigate to another page|Reset callback timer",
        CommonButtons.Ok | CommonButtons.Cancel | CommonButtons.Close | CommonButtons.No | CommonButtons.Retry | CommonButtons.Yes,
        CommonIcon.Information,
        CommonIcon.Information,
        ProgressBarStyle.None );

      TaskDialog.ButtonClicked        -= TaskDialog_ButtonClicked;
      TaskDialog.Created              -= TaskDialog_Created;
      TaskDialog.Destroyed            -= TaskDialog_Destroyed;
      TaskDialog.DialogConstructed    -= TaskDialog_DialogConstructed;
      TaskDialog.ExpandoButtonClicked -= TaskDialog_ExpandoButtonClicked;
      TaskDialog.Help                 -= TaskDialog_Help;
      TaskDialog.HyperlinkClicked     -= TaskDialog_HyperlinkClicked;
      TaskDialog.Navigated            -= TaskDialog_Navigated;
      TaskDialog.RadioButtonClicked   -= TaskDialog_RadioButtonClicked;
      TaskDialog.Timer                -= TaskDialog_Timer;
    }

    void TaskDialog_Timer( ITaskDialog sender, TimerArgs args ) {
      Console.WriteLine( "Timer invoked. Current count: {0}ms", args.TickCount );
      if( ResetCallbackTimer ) {
        ResetCallbackTimer = false;
        args.Reset = true;
      }
    }

    void TaskDialog_RadioButtonClicked( ITaskDialog sender, ButtonClickedArgs args ) {
      Console.WriteLine( "Radio button clicked: {0}", args.Id );
    }

    void TaskDialog_Navigated( ITaskDialog sender, EventArgs args ) {
      Console.WriteLine( "Dialog has navigated to another page." );
    }

    void TaskDialog_HyperlinkClicked( ITaskDialog sender, HyperlinkClickedArgs args ) {
      Console.WriteLine( "Hyperlink clicked: Link: {0}", args.Url );
    }

    void TaskDialog_Help( ITaskDialog sender, EventArgs args ) {
      Console.WriteLine( "Help invoked (F1 pressed)." );
      sender.ClickButton( 2000 );
    }

    void TaskDialog_ExpandoButtonClicked( ITaskDialog sender, ExpandoButtonClickedArgs args ) {
      Console.WriteLine( "Expando button clicked. Currently expanded: {0}", args.IsExpanded );
    }

    void TaskDialog_DialogConstructed( ITaskDialog sender, EventArgs args ) {
      Console.WriteLine( "Dialog constructed." );
    }

    void TaskDialog_Destroyed( ITaskDialog sender, EventArgs args ) {
      Console.WriteLine( "Dialog destroyed." );
    }

    void TaskDialog_Created( ITaskDialog sender, EventArgs args ) {
      Console.WriteLine( "Dialog created." );
    }

    void TaskDialog_ButtonClicked( ITaskDialog sender, ButtonClickedArgs args ) {
      Console.WriteLine( "Button clicked: {0}", args.Id );
      switch( args.Id ) {
        case 2000: {
          args.PreventClosing = true;
          TaskDialogConfig config = new TaskDialogConfig();
          config.CommonButtons = CommonButtons.Ok;
          config.WindowTitle = "Another page";
          config.MainInstruction = "You just navigated to another page.";
          sender.NavigatePage( config );

        }
          break;
        case 2001:
          args.PreventClosing = true;
          ResetCallbackTimer = true;
          break;
      }
    }
  }
}

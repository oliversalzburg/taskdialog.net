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
    public DemoForm() {
      InitializeComponent();
    }

    private void button1_Click( object sender, EventArgs e ) {
      TaskDialog.ButtonClicked += TaskDialog_ButtonClicked;

      //This is the so-called verification text. It is quite long to check common button layout.
      TaskDialog.ForceEmulationMode = forceEmulationCheckBox.Checked;
      DialogResult result = TaskDialog.ShowTaskDialogBox( "title", "mainInstruction", "content", "Do you require additional information regarding this matter?", "footer", "This is the so-called verification text. It is quite long to check common button layout.",
                                                                     "radio1|radio2", "commandButton1|commandButton2|commandButton3|commandButton4|commandButton4|commandButton4", CommonButtons.Ok | CommonButtons.Cancel | CommonButtons.Close | CommonButtons.No | CommonButtons.Retry, CommonIcon.Information,
                                                                     CommonIcon.Information, ProgressBarStyle.None );

      TaskDialog.ButtonClicked -= TaskDialog_ButtonClicked;
      /*
      TaskDialog.MessageBox( "title", "mainInstruction", "content", "expandedInfo", "footer",
                                                      "verificationText", CommonButtons.Ok,
                                                      CommonIcon.Information,
                                                      CommonIcon.Information, ProgressBarStyle.None );
      */
    }

    void TaskDialog_ButtonClicked( ITaskDialog sender, ButtonClickedArgs args ) {
      return;
    }
  }
}

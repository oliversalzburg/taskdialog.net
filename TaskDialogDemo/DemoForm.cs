using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaskDialog.UserInterface;
using ProgressBarStyle = TaskDialog.UserInterface.ProgressBarStyle;

namespace TaskDialogDemo {
  public partial class DemoForm : Form {
    public DemoForm() {
      InitializeComponent();
    }

    private void button1_Click( object sender, EventArgs e ) {
      TaskDialog.UserInterface.TaskDialog.MessageBox( "title", "mainInstruction", "content", "expandedInfo", "footer",
                                                      "verificationText", CommonButtons.Ok,
                                                      TaskDialog.UserInterface.Icon.Information,
                                                      TaskDialog.UserInterface.Icon.Information, ProgressBarStyle.None );
    }
  }
}

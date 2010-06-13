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
      TaskDialog.MessageBox( "title", "mainInstruction", "content", "expandedInfo", "footer",
                                                      "verificationText", CommonButtons.Ok,
                                                      TaskDialogNet.UserInterface.CommonIcon.Information,
                                                      TaskDialogNet.UserInterface.CommonIcon.Information, ProgressBarStyle.None );
    }
  }
}

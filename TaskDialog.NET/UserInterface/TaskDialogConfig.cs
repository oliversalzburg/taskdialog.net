using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaskDialogNet.UserInterface {

  public class TaskDialogFlags {
    public bool EnableHyperLinks { get; set; }
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
  }

  public class TaskDialogConfig {
    public IWin32Window Parent { get; set; }
    public TaskDialogFlags Flags { get; set; }
    public CommonButtons CommonButtons { get; set; }
    public string WindowTitle { get; set; }
    CommonIcon MainIcon { get; set; }
    Icon CustomMainIcon { get; set; }
    string MainInstruction { get; set; }
    string Content { get; set; }
    List<TaskDialogButton> Buttons { get; set; }
    int DefaultButton { get; set; }
    List<TaskDialogButton> RadioButtons { get; set; }
    int DefaultRadioButton { get; set; }
    string VerificationText { get; set; }
    string ExpandedInformation { get; set; }
    string ExpandedControlText { get; set; }
    string CollapsedControlText { get; set; }
    CommonIcon FooterIcon { get; set; }
    Icon CustomFooterIcon { get; set; }
    string Footer { get; set; }
    int Width { get; set; }
  }
}

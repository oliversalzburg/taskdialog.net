using System.Collections.Generic;
using System.Drawing;
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
  }

  public class TaskDialogConfig {
    public IWin32Window Parent { get; set; }
    public TaskDialogFlags Flags { get; set; }
    public CommonButtons CommonButtons { get; set; }
    public string WindowTitle { get; set; }
    public CommonIcon MainIcon { get; set; }
    public Icon CustomMainIcon { get; set; }
    public string MainInstruction { get; set; }
    public string Content { get; set; }
    public List<TaskDialogButton> Buttons { get; set; }
    public int DefaultButton { get; set; }
    public List<TaskDialogButton> RadioButtons { get; set; }
    public int DefaultRadioButton { get; set; }
    public string VerificationText { get; set; }
    public string ExpandedInformation { get; set; }
    public string ExpandedControlText { get; set; }
    public string CollapsedControlText { get; set; }
    public CommonIcon FooterIcon { get; set; }
    public Icon CustomFooterIcon { get; set; }
    public string Footer { get; set; }
    public int Width { get; set; }

    public TaskDialogConfig() {
      Parent                    = null;
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
  }
}

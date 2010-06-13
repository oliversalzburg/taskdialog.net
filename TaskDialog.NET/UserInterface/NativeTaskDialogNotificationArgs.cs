//------------------------------------------------------------------
// <summary>
// A P/Invoke wrapper for TaskDialog. Usability was given preference to perf and size.
// </summary>
//
// <remarks/>
//------------------------------------------------------------------

namespace TaskDialog.UserInterface {
  /// <summary>
  /// Arguments passed to the TaskDialog callback.
  /// </summary>
  public class NativeTaskDialogNotificationArgs {
    /// <summary>
    /// What the TaskDialog callback is a notification of.
    /// </summary>
    public VistaTaskDialogNotification Notification { get; set; }

    /// <summary>
    /// The button ID if the notification is about a button. This a DialogResult
    /// value or the ButtonID member of a TaskDialogButton set in the
    /// TaskDialog.Buttons member.
    /// </summary>
    public int ButtonId { get; set; }

    /// <summary>
    /// The HREF string of the hyperlink the notification is about.
    /// </summary>
    public string Hyperlink { get; set; }

    /// <summary>
    /// The number of milliseconds since the dialog was opened or the last time the
    /// callback for a timer notification reset the value by returning true.
    /// </summary>
    public uint TimerTickCount { get; set; }

    /// <summary>
    /// The state of the verification flag when the notification is about the verification flag.
    /// </summary>
    public bool VerificationFlagChecked { get; set; }

    /// <summary>
    /// The state of the dialog expando when the notification is about the expando.
    /// </summary>
    public bool Expanded { get; set; }
  }
}
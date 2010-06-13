//------------------------------------------------------------------
// <summary>
// A P/Invoke wrapper for TaskDialog. Usability was given preference to perf and size.
// </summary>
//
// <remarks/>
//------------------------------------------------------------------

using System;
using System.Windows.Forms;

namespace TaskDialog.UserInterface {
  /// <summary>
  /// TaskDialog wrapped in a CommonDialog class. This is required to work well in
  /// MMC 3.0. In MMC 3.0 you must use the ShowDialog methods on the MMC classes to
  /// correctly show a modal dialog. This class will allow you to do this and keep access
  /// to the results of the TaskDialog.
  /// </summary>
  public class NativeTaskDialogCommonDialog : CommonDialog {
    /// <summary>
    /// The TaskDialog we will display.
    /// </summary>
    private readonly NativeTaskDialog _taskDialog;

    /// <summary>
    /// The result of the dialog, either a DialogResult value for common push buttons set in the TaskDialog.CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the TaskDialog.Buttons member.
    /// </summary>
    private int _taskDialogResult;

    /// <summary>
    /// The verification flag result of the dialog. True if the verification checkbox was checked when the dialog
    /// was dismissed.
    /// </summary>
    private bool _verificationFlagCheckedResult;

    /// <summary>
    /// TaskDialog wrapped in a CommonDialog class. THis is required to work well in
    /// MMC 2.1. In MMC 2.1 you must use the ShowDialog methods on the MMC classes to
    /// correctly show a modal dialog. This class will allow you to do this and keep access
    /// to the results of the TaskDialog.
    /// </summary>
    /// <param name="taskDialog">The TaskDialog to show.</param>
    public NativeTaskDialogCommonDialog( NativeTaskDialog taskDialog ) {
      if( taskDialog == null ) {
        throw new ArgumentNullException( "taskDialog" );
      }

      _taskDialog = taskDialog;
    }

    /// <summary>
    /// The TaskDialog to show.
    /// </summary>
    public NativeTaskDialog TaskDialog {
      get { return _taskDialog; }
    }

    /// <summary>
    /// The result of the dialog, either a DialogResult value for common push buttons set in the TaskDialog.CommonButtons
    /// member or the ButtonID from a TaskDialogButton structure set on the TaskDialog.Buttons member.
    /// </summary>
    public int TaskDialogResult {
      get { return _taskDialogResult; }
    }

    /// <summary>
    /// The verification flag result of the dialog. True if the verification checkbox was checked when the dialog
    /// was dismissed.
    /// </summary>
    public bool VerificationFlagCheckedResult {
      get { return _verificationFlagCheckedResult; }
    }

    /// <summary>
    /// Reset the common dialog.
    /// </summary>
    public override void Reset() {
      _taskDialog.Reset();
    }

    /// <summary>
    /// The required implementation of CommonDialog that shows the Task Dialog.
    /// </summary>
    /// <param name="hwndOwner">Owner window. This can be null.</param>
    /// <returns>If this method returns true, then ShowDialog will return DialogResult.OK.
    /// If this method returns false, then ShowDialog will return DialogResult.Cancel. The
    /// user of this class must use the TaskDialogResult member to get more information.
    /// </returns>
    protected override bool RunDialog( IntPtr hwndOwner ) {
      _taskDialogResult = _taskDialog.Show( hwndOwner, out _verificationFlagCheckedResult );
      return ( _taskDialogResult != (int) DialogResult.Cancel );
    }
  }
}
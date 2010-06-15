using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TaskDialogNet.UserInterface {

  public class ButtonClickedArgs : EventArgs {
    public int Id { get; private set; }
    public bool PreventClosing { get; set; }

    public ButtonClickedArgs( int id ) {
      Id = id;
    }
  }

  public class HyperlinkClickedArgs : EventArgs {
    public string Url { get; private set; }

    public HyperlinkClickedArgs( string url ) {
      Url = url;
    }
  }

  public class TimerArgs : EventArgs {
    public uint TickCount { get; private set; }

    public TimerArgs( uint tickCount ) {
      TickCount = tickCount;
    }
  }

  public class TaskDialogButton {
    public int ButtonId { get; set; }
    public string ButtonText { get; set; }
  }

  [Flags]
  public enum CommonButtons {
    None = 0x00,
    Ok = 0x01,
    Yes = 0x02,
    No = 0x04,
    Cancel = 0x08,
    Retry = 0x10,
    Close = 0x20
  }

  public enum CommonIcon {
    None,
    Information,
    Warning,
    Error,
    SecurityWarning,
    SecurityError,
    SecuritySuccess,
    SecurityShield,
    SecurityShieldBlue,
    SecurityShieldGray,
    Custom
  }

  public enum ProgressBarStyle {
    None,
    Continous,
    Marquee
  }

  public enum ProgressBarState {
    Normal,
    Pause,
    Error
  }

  public delegate void TaskDialogEventHandler( ITaskDialog sender, EventArgs args );
  public delegate void TaskDialogEventHandler<in T>( ITaskDialog sender, T args );

  public interface ITaskDialog {
    #region Events
    // Events through pfCallback
    // See: http://msdn.microsoft.com/en-us/library/bb760542%28VS.85%29.aspx
    event TaskDialogEventHandler<ButtonClickedArgs> ButtonClicked;
    event TaskDialogEventHandler Created;
    event TaskDialogEventHandler Destroyed;
    event TaskDialogEventHandler DialogConstructed;
    event TaskDialogEventHandler ExpandoButtonClicked;
    event TaskDialogEventHandler Help;
    event TaskDialogEventHandler<HyperlinkClickedArgs> HyperlinkClicked;
    event TaskDialogEventHandler Navigated;
    event TaskDialogEventHandler<ButtonClickedArgs> RadioButtonClicked;
    event TaskDialogEventHandler<TimerArgs> Timer;
    event TaskDialogEventHandler VerificationClicked;
    #endregion

    #region Properties
    #region Flags
    // Properties from dwFlags
    // See: http://msdn.microsoft.com/en-us/library/bb787473%28VS.85%29.aspx
    bool EnableHyperlinks { get; set; }
    bool AllowDialogCancellation { get; set; }
    bool UseCommandLinks { get; set; }
    bool UseCommandLinksNoIcon { get; set; }
    bool ExpandFooterArea { get; set; }
    bool ExpandedByDefault { get; set; }
    bool VerificationFlagChecked { get; set; }
    bool ShowProgressBar { get; set; }
    bool ShowMarqueeProgressBar { get; set; }
    bool CallbackTimer { get; set; }
    bool PositionRelativeToWindow { get; set; }
    bool RtlLayout { get; set; }
    bool NoDefaultRadioButton { get; set; }
    bool CanBeMinimized { get; set; }
    #endregion

    // Additional properties from pTaskConfig
    // See: http://msdn.microsoft.com/en-us/library/bb787473%28VS.85%29.aspx
    CommonButtons CommonButtons { get; set; }
    string WindowTitle { get; set; }
    CommonIcon MainIcon { get; set; }
    Icon CustomMainIcon { get; set; }
    string MainInstruction { get; set; }
    string Content { get; set; }
    List<TaskDialogButton> Buttons { get; }
    int DefaultButton { get; set; }
    List<TaskDialogButton> RadioButtons { get; }
    int DefaultRadioButton { get; set; }
    string VerificationText { get; set; }
    string ExpandedInformation { get; set; }
    string ExpandedControlText { get; set; }
    string CollapsedControlText { get; set; }
    CommonIcon FooterIcon { get; set; }
    Icon CustomFooterIcon { get; set; }
    string Footer { get; set; }
    int Width { get; set; }
    #endregion


    #region Methods
    int Show( IWin32Window owner, out bool verificationFlagChecked, out int radioButtonResult );

    /// <summary>
    /// Create a TaskDialog from a TaskDialogConfig instance.
    /// </summary>
    /// <param name="taskConfig">A TaskDialogConfig instance that describes the TaskDialog.</param>
    /// <param name="button">The button that was clicked to close the TaskDialog.</param>
    /// <param name="radioButton">The radio button that was selected in the TaskDialog.</param>
    /// <param name="verificationFlagChecked">true if the verification checkbox was checked; false otherwise.</param>
    /// <returns></returns>
    int TaskDialogIndirect( TaskDialogConfig taskConfig, out int button, out int radioButton,
                            out bool verificationFlagChecked );


    /// <summary>
    /// Simulate the action of a button click in the TaskDialog. This can be a DialogResult value 
    /// or the ButtonID set on a TasDialogButton set on TaskDialog.Buttons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be clicked.</param>
    void ClickButton( int buttonId );

    /// <summary>
    /// Simulate the action of a radio button click in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be selected.</param>
    void ClickRadioButton( int buttonId );

    /// <summary>
    /// Check or uncheck the verification checkbox in the TaskDialog. 
    /// </summary>
    /// <param name="verificationChecked">The checked state to set the verification checkbox.</param>
    /// <param name="setFocus">True to set the keyboard focus to the checkbox, and false otherwise.</param>
    void ClickVerification( bool verificationChecked, bool setFocus );

    /// <summary>
    /// Enable or disable a button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.Buttons
    /// or a common button ID.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    /// <param name="enable">Enambe the button if true. Disable the button if false.</param>
    void EnableButton( int buttonId, bool enable );

    /// <summary>
    /// Enable a button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.Buttons
    /// or a common button ID.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    void EnableButton( int buttonId );

    /// <summary>
    /// Disable a button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.Buttons
    /// or a common button ID.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    void DisableButton( int buttonId );

    /// <summary>
    /// Enable or disable a radio button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    /// <param name="enable">Enambe the button if true. Disable the button if false.</param>
    void EnableRadioButton( int buttonId, bool enable );

    /// <summary>
    /// Enable a radio button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    void EnableRadioButton( int buttonId );

    /// <summary>
    /// Disable a radio button in the TaskDialog. 
    /// The passed buttonID is the ButtonID set on a TaskDialogButton set on TaskDialog.RadioButtons.
    /// </summary>
    /// <param name="buttonId">Indicates the button ID to be enabled or diabled.</param>
    void DisableRadioButton( int buttonId );

    /// <summary>
    /// Recreates a task dialog with new contents, simulating the functionality of a multi-page wizard. 
    /// </summary>
    /// <param name="page">The next page.</param>
    void NavigatePage( ITaskDialog page );

    /// <summary>
    /// Designate whether a given Task Dialog button or command link should have a User Account Control (UAC) shield icon.
    /// </summary>
    /// <param name="buttonId">ID of the push button or command link to be updated.</param>
    /// <param name="requiresElevation">False to designate that the action invoked by the button does not require elevation;
    /// true to designate that the action does require elevation.</param>
    void SetButtonElevationRequiredState( int buttonId, bool requiresElevation );
    
    /// <summary>
    /// Updates the content text.
    /// </summary>
    /// <param name="text">The new value.</param>
    void SetContentText( string text );

    /// <summary>
    /// Updates the Expanded Information text.
    /// </summary>
    /// <param name="text">The new value.</param>
    void SetExpandedInformationText( string text );

    /// <summary>
    /// Updates the Footer text.
    /// </summary>
    /// <param name="text">The new value.</param>
    void SetFooterText( string text );

    /// <summary>
    /// Updates the Main Instruction.
    /// </summary>
    /// <param name="text">The new value.</param>
    void SetMainInstructionText( string text );

    /// <summary>
    /// Used to indicate whether the hosted progress bar should be displayed in marquee mode or not.
    /// </summary>
    /// <param name="setMarquee">Specifies whether the progress bar sbould be shown in Marquee mode.
    /// A value of true turns on Marquee mode.</param>
    void SetMarqueeProgressBar( bool setMarquee );

    /// <summary>
    /// Sets the animation state of the Marquee Progress Bar.
    /// </summary>
    /// <param name="startMarquee">true starts the marquee animation and false stops it.</param>
    /// <param name="speed">The time in milliseconds between refreshes.</param>
    void SetProgressBarMarquee( bool startMarquee, uint speed );

    /// <summary>
    /// Set the current position for a progress bar.
    /// </summary>
    /// <param name="position">The new position.</param>
    /// <returns>Returns the previous value if successful, or zero otherwise.</returns>
    int SetProgressBarPosition( int position );

    /// <summary>
    /// Set the minimum and maximum values for the hosted progress bar.
    /// </summary>
    /// <param name="minimum">Minimum range value. By default, the minimum value is zero.</param>
    /// <param name="maximum">Maximum range value.  By default, the maximum value is 100.</param>
    void SetProgressBarRange( int minimum, int maximum );

    /// <summary>
    /// Sets the state of the progress bar.
    /// </summary>
    /// <param name="state">The state to set the progress bar.</param>
    void SetProgressBarState( ProgressBarState state );

    /// <summary>
    /// Updates the main instruction icon. Note the type (standard via enum or
    /// custom via Icon type) must be used when upating the icon.
    /// </summary>
    /// <param name="icon">The icon to set.</param>
    void UpdateMainIcon( CommonIcon icon );

    /// <summary>
    /// Updates the main instruction icon. Note the type (standard via enum or
    /// custom via Icon type) must be used when upating the icon.
    /// </summary>
    /// <param name="icon">The icon to set.</param>
    void UpdateMainIcon( Icon icon );

    /// <summary>
    /// Updates the footer icon. Note the type (standard via enum or
    /// custom via Icon type) must be used when upating the icon.
    /// </summary>
    /// <param name="icon">Task Dialog standard icon.</param>
    void UpdateFooterIcon( CommonIcon icon );

    /// <summary>
    /// Updates the footer icon. Note the type (standard via enum or
    /// custom via Icon type) must be used when upating the icon.
    /// </summary>
    /// <param name="icon">Task Dialog standard icon.</param>
    void UpdateFooterIcon( Icon icon );
    #endregion

  }
}

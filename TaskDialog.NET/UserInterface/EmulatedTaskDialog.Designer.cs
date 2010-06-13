using System.Windows.Forms;

namespace TaskDialog.UserInterface
{
  partial class EmulatedTaskDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( EmulatedTaskDialog ) );
      this.pnlButtons = new System.Windows.Forms.Panel();
      this.commonButtonPanel = new System.Windows.Forms.TableLayoutPanel();
      this.verifyCheckBox = new System.Windows.Forms.CheckBox();
      this.showHideDetails = new System.Windows.Forms.Label();
      this.imageList1 = new System.Windows.Forms.ImageList( this.components );
      this.panel2 = new System.Windows.Forms.Panel();
      this.pnlFooter = new System.Windows.Forms.Panel();
      this.imgFooter = new System.Windows.Forms.PictureBox();
      this.panel5 = new System.Windows.Forms.Panel();
      this.panel3 = new System.Windows.Forms.Panel();
      this.pnlCommandButtons = new System.Windows.Forms.Panel();
      this.pnlMainInstruction = new System.Windows.Forms.Panel();
      this.pnlContent = new System.Windows.Forms.Panel();
      this.pnlExpandedInfo = new System.Windows.Forms.Panel();
      this.pnlRadioButtons = new System.Windows.Forms.Panel();
      this.SecurityIcons = new System.Windows.Forms.ImageList( this.components );
      this.callbackTimer = new System.Windows.Forms.Timer( this.components );
      this.focusButton = new System.Windows.Forms.Button();
      this.progressBarPanel = new System.Windows.Forms.Panel();
      this.progressBar = new System.Windows.Forms.ProgressBar();
      this.footerText = new RichTextBoxEx();
      this.expandedInfoText = new RichTextBoxEx();
      this.contentText = new RichTextBoxEx();
      this.imgMain = new TransparentPictureBox();
      this.pnlButtons.SuspendLayout();
      this.pnlFooter.SuspendLayout();
      ( (System.ComponentModel.ISupportInitialize)( this.imgFooter ) ).BeginInit();
      this.pnlMainInstruction.SuspendLayout();
      this.pnlContent.SuspendLayout();
      this.pnlExpandedInfo.SuspendLayout();
      this.progressBarPanel.SuspendLayout();
      ( (System.ComponentModel.ISupportInitialize)( this.imgMain ) ).BeginInit();
      this.SuspendLayout();
      // 
      // pnlButtons
      // 
      this.pnlButtons.BackColor = System.Drawing.Color.WhiteSmoke;
      this.pnlButtons.Controls.Add( this.commonButtonPanel );
      this.pnlButtons.Controls.Add( this.verifyCheckBox );
      this.pnlButtons.Controls.Add( this.showHideDetails );
      this.pnlButtons.Controls.Add( this.panel2 );
      this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlButtons.Location = new System.Drawing.Point( 0, 322 );
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new System.Drawing.Size( 454, 58 );
      this.pnlButtons.TabIndex = 0;
      // 
      // commonButtonPanel
      // 
      this.commonButtonPanel.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.commonButtonPanel.AutoSize = true;
      this.commonButtonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.commonButtonPanel.ColumnCount = 2;
      this.commonButtonPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
      this.commonButtonPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
      this.commonButtonPanel.Location = new System.Drawing.Point( 449, 6 );
      this.commonButtonPanel.Name = "commonButtonPanel";
      this.commonButtonPanel.RowCount = 2;
      this.commonButtonPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
      this.commonButtonPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
      this.commonButtonPanel.Size = new System.Drawing.Size( 0, 0 );
      this.commonButtonPanel.TabIndex = 5;
      // 
      // verifyCheckBox
      // 
      this.verifyCheckBox.AutoSize = true;
      this.verifyCheckBox.Location = new System.Drawing.Point( 13, 34 );
      this.verifyCheckBox.Name = "verifyCheckBox";
      this.verifyCheckBox.Size = new System.Drawing.Size( 353, 17 );
      this.verifyCheckBox.TabIndex = 4;
      this.verifyCheckBox.Text = "Clicking the verification checkbox will fire a VerificationClicked event.";
      this.verifyCheckBox.UseVisualStyleBackColor = false;
      this.verifyCheckBox.Visible = false;
      this.verifyCheckBox.CheckedChanged += new System.EventHandler( this.VerifyCheckedChanged );
      // 
      // showHideDetails
      // 
      this.showHideDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.showHideDetails.ImageIndex = 3;
      this.showHideDetails.ImageList = this.imageList1;
      this.showHideDetails.Location = new System.Drawing.Point( 8, 6 );
      this.showHideDetails.Name = "showHideDetails";
      this.showHideDetails.Size = new System.Drawing.Size( 94, 23 );
      this.showHideDetails.TabIndex = 3;
      this.showHideDetails.Text = "        Show details";
      this.showHideDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.showHideDetails.Click += new System.EventHandler( this.LbDetailsClick );
      this.showHideDetails.MouseDown += new System.Windows.Forms.MouseEventHandler( this.LbDetailsMouseDown );
      this.showHideDetails.MouseEnter += new System.EventHandler( this.LbDetailsMouseEnter );
      this.showHideDetails.MouseLeave += new System.EventHandler( this.LbDetailsMouseLeave );
      this.showHideDetails.MouseUp += new System.Windows.Forms.MouseEventHandler( this.LbDetailsMouseUp );
      // 
      // imageList1
      // 
      this.imageList1.ImageStream = ( (System.Windows.Forms.ImageListStreamer)( resources.GetObject( "imageList1.ImageStream" ) ) );
      this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
      this.imageList1.Images.SetKeyName( 0, "arrow_up_bw.bmp" );
      this.imageList1.Images.SetKeyName( 1, "arrow_up_color.bmp" );
      this.imageList1.Images.SetKeyName( 2, "arrow_up_color_pressed.bmp" );
      this.imageList1.Images.SetKeyName( 3, "arrow_down_bw.bmp" );
      this.imageList1.Images.SetKeyName( 4, "arrow_down_color.bmp" );
      this.imageList1.Images.SetKeyName( 5, "arrow_down_color_pressed.bmp" );
      this.imageList1.Images.SetKeyName( 6, "green_arrow.bmp" );
      // 
      // panel2
      // 
      this.panel2.BackColor = System.Drawing.Color.Gainsboro;
      this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel2.Location = new System.Drawing.Point( 0, 0 );
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size( 454, 1 );
      this.panel2.TabIndex = 0;
      // 
      // pnlFooter
      // 
      this.pnlFooter.BackColor = System.Drawing.Color.WhiteSmoke;
      this.pnlFooter.Controls.Add( this.footerText );
      this.pnlFooter.Controls.Add( this.imgFooter );
      this.pnlFooter.Controls.Add( this.panel5 );
      this.pnlFooter.Controls.Add( this.panel3 );
      this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlFooter.Location = new System.Drawing.Point( 0, 380 );
      this.pnlFooter.Name = "pnlFooter";
      this.pnlFooter.Size = new System.Drawing.Size( 454, 36 );
      this.pnlFooter.TabIndex = 2;
      // 
      // imgFooter
      // 
      this.imgFooter.Location = new System.Drawing.Point( 8, 10 );
      this.imgFooter.Name = "imgFooter";
      this.imgFooter.Size = new System.Drawing.Size( 16, 16 );
      this.imgFooter.TabIndex = 4;
      this.imgFooter.TabStop = false;
      // 
      // panel5
      // 
      this.panel5.BackColor = System.Drawing.Color.White;
      this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel5.Location = new System.Drawing.Point( 0, 1 );
      this.panel5.Name = "panel5";
      this.panel5.Size = new System.Drawing.Size( 454, 1 );
      this.panel5.TabIndex = 2;
      // 
      // panel3
      // 
      this.panel3.BackColor = System.Drawing.Color.Gainsboro;
      this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel3.Location = new System.Drawing.Point( 0, 0 );
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size( 454, 1 );
      this.panel3.TabIndex = 1;
      // 
      // pnlCommandButtons
      // 
      this.pnlCommandButtons.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlCommandButtons.Location = new System.Drawing.Point( 0, 223 );
      this.pnlCommandButtons.Name = "pnlCommandButtons";
      this.pnlCommandButtons.Size = new System.Drawing.Size( 454, 99 );
      this.pnlCommandButtons.TabIndex = 4;
      // 
      // pnlMainInstruction
      // 
      this.pnlMainInstruction.Controls.Add( this.imgMain );
      this.pnlMainInstruction.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlMainInstruction.Location = new System.Drawing.Point( 0, 0 );
      this.pnlMainInstruction.Name = "pnlMainInstruction";
      this.pnlMainInstruction.Size = new System.Drawing.Size( 454, 41 );
      this.pnlMainInstruction.TabIndex = 1;
      this.pnlMainInstruction.Paint += new System.Windows.Forms.PaintEventHandler( this.PnlMainInstructionPaint );
      // 
      // pnlContent
      // 
      this.pnlContent.Controls.Add( this.contentText );
      this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlContent.Location = new System.Drawing.Point( 0, 41 );
      this.pnlContent.Name = "pnlContent";
      this.pnlContent.Size = new System.Drawing.Size( 454, 30 );
      this.pnlContent.TabIndex = 2;
      // 
      // pnlExpandedInfo
      // 
      this.pnlExpandedInfo.Controls.Add( this.expandedInfoText );
      this.pnlExpandedInfo.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlExpandedInfo.Location = new System.Drawing.Point( 0, 71 );
      this.pnlExpandedInfo.Name = "pnlExpandedInfo";
      this.pnlExpandedInfo.Size = new System.Drawing.Size( 454, 30 );
      this.pnlExpandedInfo.TabIndex = 10;
      // 
      // pnlRadioButtons
      // 
      this.pnlRadioButtons.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlRadioButtons.Location = new System.Drawing.Point( 0, 122 );
      this.pnlRadioButtons.Name = "pnlRadioButtons";
      this.pnlRadioButtons.Size = new System.Drawing.Size( 454, 101 );
      this.pnlRadioButtons.TabIndex = 3;
      // 
      // SecurityIcons
      // 
      this.SecurityIcons.ImageStream = ( (System.Windows.Forms.ImageListStreamer)( resources.GetObject( "SecurityIcons.ImageStream" ) ) );
      this.SecurityIcons.TransparentColor = System.Drawing.Color.Transparent;
      this.SecurityIcons.Images.SetKeyName( 0, "error" );
      this.SecurityIcons.Images.SetKeyName( 1, "success" );
      this.SecurityIcons.Images.SetKeyName( 2, "warning" );
      this.SecurityIcons.Images.SetKeyName( 3, "shield" );
      // 
      // callbackTimer
      // 
      this.callbackTimer.Interval = 200;
      this.callbackTimer.Tick += new System.EventHandler( this.CallbackTimerTick );
      // 
      // focusButton
      // 
      this.focusButton.Location = new System.Drawing.Point( 374, 431 );
      this.focusButton.Name = "focusButton";
      this.focusButton.Size = new System.Drawing.Size( 75, 23 );
      this.focusButton.TabIndex = 11;
      this.focusButton.Text = "focus";
      this.focusButton.UseVisualStyleBackColor = true;
      this.focusButton.Visible = false;
      // 
      // progressBarPanel
      // 
      this.progressBarPanel.Controls.Add( this.progressBar );
      this.progressBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
      this.progressBarPanel.Location = new System.Drawing.Point( 0, 101 );
      this.progressBarPanel.Name = "progressBarPanel";
      this.progressBarPanel.Size = new System.Drawing.Size( 454, 21 );
      this.progressBarPanel.TabIndex = 13;
      // 
      // progressBar
      // 
      this.progressBar.Location = new System.Drawing.Point( 50, 3 );
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size( 247, 15 );
      this.progressBar.TabIndex = 13;
      // 
      // footerText
      // 
      this.footerText.BackColor = System.Drawing.Color.WhiteSmoke;
      this.footerText.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.footerText.Cursor = System.Windows.Forms.Cursors.Default;
      this.footerText.DetectUrls = false;
      this.footerText.Location = new System.Drawing.Point( 30, 11 );
      this.footerText.Name = "footerText";
      this.footerText.Size = new System.Drawing.Size( 409, 15 );
      this.footerText.TabIndex = 4;
      this.footerText.Text = "footerText";
      this.footerText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler( this.FooterLinkClicked );
      this.footerText.Click += new System.EventHandler( this.FooterClick );
      this.footerText.MouseDown += new System.Windows.Forms.MouseEventHandler( this.FooterMouseDown );
      // 
      // expandedInfoText
      // 
      this.expandedInfoText.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.expandedInfoText.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.expandedInfoText.Cursor = System.Windows.Forms.Cursors.Default;
      this.expandedInfoText.DetectUrls = false;
      this.expandedInfoText.Location = new System.Drawing.Point( 50, 0 );
      this.expandedInfoText.Name = "expandedInfoText";
      this.expandedInfoText.Size = new System.Drawing.Size( 399, 19 );
      this.expandedInfoText.TabIndex = 0;
      this.expandedInfoText.Text = "expandedInfoText";
      this.expandedInfoText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler( this.ExpandedInfoLinkClicked );
      this.expandedInfoText.Click += new System.EventHandler( this.ExpandedInfoClick );
      this.expandedInfoText.MouseDown += new System.Windows.Forms.MouseEventHandler( this.ExpandedInfoMouseDown );
      // 
      // contentText
      // 
      this.contentText.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.contentText.BackColor = System.Drawing.SystemColors.Window;
      this.contentText.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.contentText.Cursor = System.Windows.Forms.Cursors.Default;
      this.contentText.DetectUrls = false;
      this.contentText.Location = new System.Drawing.Point( 50, 0 );
      this.contentText.Name = "contentText";
      this.contentText.ReadOnly = true;
      this.contentText.Size = new System.Drawing.Size( 399, 19 );
      this.contentText.TabIndex = 0;
      this.contentText.Text = "contentText";
      this.contentText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler( this.ContentLinkClicked );
      this.contentText.Click += new System.EventHandler( this.ContentClick );
      this.contentText.MouseDown += new System.Windows.Forms.MouseEventHandler( this.ContentMouseDown );
      // 
      // imgMain
      // 
      this.imgMain.BackColor = System.Drawing.Color.Transparent;
      this.imgMain.Location = new System.Drawing.Point( 8, 8 );
      this.imgMain.Name = "imgMain";
      this.imgMain.Size = new System.Drawing.Size( 32, 32 );
      this.imgMain.TabIndex = 0;
      this.imgMain.TabStop = false;
      // 
      // EmulatedTaskDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size( 454, 459 );
      this.Controls.Add( this.focusButton );
      this.Controls.Add( this.pnlFooter );
      this.Controls.Add( this.pnlButtons );
      this.Controls.Add( this.pnlCommandButtons );
      this.Controls.Add( this.pnlRadioButtons );
      this.Controls.Add( this.progressBarPanel );
      this.Controls.Add( this.pnlExpandedInfo );
      this.Controls.Add( this.pnlContent );
      this.Controls.Add( this.pnlMainInstruction );
      this.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "EmulatedTaskDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "EmulatedTaskDialog";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.EmulatedTaskDialog_FormClosing );
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.TaskDialogForm_FormClosed );
      this.Shown += new System.EventHandler( this.FrmTaskDialogShown );
      this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.TaskDialogForm_KeyDown );
      this.pnlButtons.ResumeLayout( false );
      this.pnlButtons.PerformLayout();
      this.pnlFooter.ResumeLayout( false );
      ( (System.ComponentModel.ISupportInitialize)( this.imgFooter ) ).EndInit();
      this.pnlMainInstruction.ResumeLayout( false );
      this.pnlContent.ResumeLayout( false );
      this.pnlExpandedInfo.ResumeLayout( false );
      this.progressBarPanel.ResumeLayout( false );
      ( (System.ComponentModel.ISupportInitialize)( this.imgMain ) ).EndInit();
      this.ResumeLayout( false );

    }

    #endregion

    private TransparentPictureBox imgMain;
    private RichTextBoxEx contentText;
    private System.Windows.Forms.Panel pnlButtons;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel pnlFooter;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Panel panel5;
    private System.Windows.Forms.PictureBox imgFooter;
    private RichTextBoxEx footerText;
    private System.Windows.Forms.ImageList imageList1;
    private System.Windows.Forms.Label showHideDetails;
    private System.Windows.Forms.Panel pnlCommandButtons;
    private System.Windows.Forms.CheckBox verifyCheckBox;
    private System.Windows.Forms.Panel pnlMainInstruction;
    private System.Windows.Forms.Panel pnlContent;
    private System.Windows.Forms.Panel pnlExpandedInfo;
    private RichTextBoxEx expandedInfoText;
    private System.Windows.Forms.Panel pnlRadioButtons;
    private System.Windows.Forms.ImageList SecurityIcons;
    private System.Windows.Forms.Timer callbackTimer;
    private System.Windows.Forms.TableLayoutPanel commonButtonPanel;
    private System.Windows.Forms.Button focusButton;
    private System.Windows.Forms.Panel progressBarPanel;
    private ProgressBar progressBar;
  }
}
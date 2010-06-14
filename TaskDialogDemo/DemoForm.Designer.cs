namespace TaskDialogDemo {
  partial class DemoForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing ) {
      if( disposing && ( components != null ) ) {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.button1 = new System.Windows.Forms.Button();
      this.forceEmulationCheckBox = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point( 12, 12 );
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size( 75, 23 );
      this.button1.TabIndex = 0;
      this.button1.Text = "button1";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler( this.button1_Click );
      // 
      // forceEmulationCheckBox
      // 
      this.forceEmulationCheckBox.AutoSize = true;
      this.forceEmulationCheckBox.Location = new System.Drawing.Point( 127, 16 );
      this.forceEmulationCheckBox.Name = "forceEmulationCheckBox";
      this.forceEmulationCheckBox.Size = new System.Drawing.Size( 101, 17 );
      this.forceEmulationCheckBox.TabIndex = 1;
      this.forceEmulationCheckBox.Text = "Force emulation";
      this.forceEmulationCheckBox.UseVisualStyleBackColor = true;
      // 
      // DemoForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size( 396, 188 );
      this.Controls.Add( this.forceEmulationCheckBox );
      this.Controls.Add( this.button1 );
      this.Name = "DemoForm";
      this.Text = "Form1";
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.CheckBox forceEmulationCheckBox;
  }
}


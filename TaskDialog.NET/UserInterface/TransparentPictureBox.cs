using System.Drawing;
using System.Windows.Forms;

namespace TaskDialogNet.UserInterface {
  sealed class TransparentPictureBox : PictureBox {
    public TransparentPictureBox() {
      SetStyle( ControlStyles.SupportsTransparentBackColor, true );
      BackColor = Color.Transparent;
    }

    protected override CreateParams CreateParams {
      get {
        CreateParams cp = base.CreateParams;
        cp.ExStyle |= 0x20;
        return cp;
      }
    }

  }
}

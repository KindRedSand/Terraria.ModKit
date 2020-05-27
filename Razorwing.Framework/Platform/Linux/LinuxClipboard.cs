// using System.Windows.Forms;

namespace Razorwing.Framework.Platform.Linux
{
    public class LinuxClipboard : Clipboard
    {
        public override string GetText()
        {
            return string.Empty;
            // return System.Windows.Forms.Clipboard.GetText(TextDataFormat.UnicodeText);
        }

        public override void SetText(string selectedText)
        {
            //Clipboard.SetText(selectedText);

            //This works within Razorwing but will hang any application you try to paste to afterwards until Razorwing is closed.
            //Likely requires the use of X libraries directly to fix
        }
    }
}

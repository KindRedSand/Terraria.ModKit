namespace Razorwing.Framework.Platform
{
    public abstract class Clipboard
    {
        public abstract string GetText();

        public abstract void SetText(string selectedText);
    }
}

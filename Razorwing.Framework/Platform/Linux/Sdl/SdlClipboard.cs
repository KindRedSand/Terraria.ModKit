using System.Runtime.InteropServices;

namespace Razorwing.Framework.Platform.Linux.Sdl
{
    public class SdlClipboard : Clipboard
    {
        private const string lib = "libSDL2-2.0.so.0";

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetClipboardText", ExactSpelling = true)]
        internal static extern string SDL_GetClipboardText();

        [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetClipboardText", ExactSpelling = true)]
        internal static extern int SDL_SetClipboardText(string text);

        public override string GetText()
        {
            return SDL_GetClipboardText();
        }

        public override void SetText(string selectedText)
        {
            SDL_SetClipboardText(selectedText);
        }
    }
}

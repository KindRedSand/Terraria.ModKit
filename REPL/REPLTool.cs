using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModKit.REPL;
using Terraria.UI;

namespace Terraria.ModKit.Tools.REPL
{
	class REPLTool 
	{
        internal bool visible;
        internal string toggleTooltip;
		internal static REPLBackend replBackend;
		internal static REPLUI toolKit;
		internal static bool EyedropperActive;
        internal UserInterface userInterface;

		internal void Initialize()
		{
			if (replBackend == null)
			    replBackend = new REPLBackend();
			toggleTooltip = "Click to toggle C# REPL";

			if(magicPixel == null)
            {
                magicPixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
                magicPixel.SetData(new Color[] { new Color(1, 1, 1) });
			}
        }

		internal void ClientInitialize()
		{
			Initialize();
			userInterface = new UserInterface();
            toolKit = new REPLUI(userInterface);
            toolKit.Activate();
			userInterface.SetState(toolKit);
		}

        internal void ScreenResolutionChanged()
        {
            userInterface?.Recalculate();
        }

        internal void UIUpdate()
        {
            if (visible)
            {
                userInterface?.Update(Main.gameTimeCache);
            }
        }


		internal void UIDraw()
		{
			if (visible)
            {
				toolKit.Init();
                toolKit.Draw(Main.spriteBatch);
				if (EyedropperActive)
				{
					Point tileCoords = Main.MouseWorld.ToTileCoordinates();
					Vector2 worldCoords = tileCoords.ToVector2() * 16;
					Vector2 screenCoords = worldCoords - Main.screenPosition;

					DrawBorderedRect(Main.spriteBatch, Color.LightBlue * 0.1f, Color.Blue * 0.7f, screenCoords, new Vector2(16,16), 5);

					if(!Main.LocalPlayer.mouseInterface && Main.mouseLeft)
					{
						EyedropperActive = false;

                        toolKit.codeTextBox.Write($"Main.tile[{tileCoords.X},{tileCoords.Y}]");
						// can't get this to work. Tools.REPL.REPLTool.Terraria.ModKitUI.codeTextBox.Focus();
					}

					Main.LocalPlayer.mouseInterface = true;
				}
            }
		}
		internal void Toggled()
		{
			Main.drawingPlayerChat = false;
			if (visible)
			{
				Tools.REPL.REPLTool.toolKit.codeTextBox.Focus();
			}
			if (!visible)
			{
				Tools.REPL.REPLTool.toolKit.codeTextBox.Unfocus();
			}
		}


        internal static Texture2D magicPixel;
		// A helper method that draws a bordered rectangle. 
		public static void DrawBorderedRect(SpriteBatch spriteBatch, Color color, Color borderColor, Vector2 position, Vector2 size, int borderWidth)
		{
            spriteBatch.Draw(magicPixel, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
			spriteBatch.Draw(magicPixel, new Rectangle((int)position.X - borderWidth, (int)position.Y - borderWidth, (int)size.X + borderWidth * 2, borderWidth), borderColor);
			spriteBatch.Draw(magicPixel, new Rectangle((int)position.X - borderWidth, (int)position.Y + (int)size.Y, (int)size.X + borderWidth * 2, borderWidth), borderColor);
			spriteBatch.Draw(magicPixel, new Rectangle((int)position.X - borderWidth, (int)position.Y, (int)borderWidth, (int)size.Y), borderColor);
			spriteBatch.Draw(magicPixel, new Rectangle((int)position.X + (int)size.X, (int)position.Y, (int)borderWidth, (int)size.Y), borderColor);
		}
	}
}

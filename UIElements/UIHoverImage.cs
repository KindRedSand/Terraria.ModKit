using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.ModKit.UIElements
{
	class UIHoverImage : UIImage
	{
		internal string hoverText;

		public UIHoverImage(Asset<Texture2D> texture, string hoverText) : base(texture)
		{
			this.hoverText = hoverText;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			if (IsMouseHovering)
			{
				Main.hoverItemName = hoverText;
			}
		}
	}
}
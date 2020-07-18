using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ModKit
{
    internal class UITinker : DragableUIPanel
    {
        private UIList list;
        internal static bool Visible = true;

        public UITinker()
        {
            Top.Set(300, 0f);
            Left.Set(100, 0f);
            Height.Set(500, 0f);
            Width.Set(200, 0f);
            var tt = new DragableUIPanel();
            tt.Draggable = false;
            tt.Top.Set(50, 0);
            tt.Left.Set(10, 0);
            tt.Width.Set(180, 0);
            tt.Height.Set(400, 0);
            tt.BackgroundColor = new Color(100,44,25);
            Append(tt);
            list = new UIList();
            tt.Append(list);
            //list.Top.Set(50, 0);
            //list.Left.Set(10, 0);
            list.Width.Set(180, 0);
            list.Height.Set(400, 0);
            var el = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f);
            el.HAlign = 0.5f;
            el.Top.Set(-45f, 0.0f);
            el.Left.Set(-10f, 0.0f);
            el.SetPadding(15f);
            list.Add(el);
            //Append(list);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!Visible)
                return;
            base.Draw(spriteBatch);
        }
    }

    internal class DragableUIPanel : UIPanel
    {
        private Vector2 offset;
        public bool dragging;
        public bool Draggable { get; set; } = true;

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            if (Main.mouseLeft && !dragging)
            {
                DragStart(Main.MouseScreen);
            }
            else if(Main.mouseLeftRelease && dragging)
            {
                DragEnd(Main.MouseScreen);
            }
        }


        private void DragStart(Vector2 evt)
        {
            if (!Draggable)
                return;
            offset = new Vector2(evt.X - Left.Pixels, evt.Y - Top.Pixels);
            dragging = true;
        }


        private void DragEnd(Vector2 evt)
        {
            if (!Draggable)
                return;
            dragging = false;
            Left.Set(evt.X - offset.X, 0f);
            Top.Set(evt.Y - offset.Y, 0f);
            Recalculate();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); 
            if (ContainsPoint(Main.MouseScreen)) Main.LocalPlayer.mouseInterface = true;

            if (!Draggable)
                return;

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f);
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }

            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                Recalculate();
            }
        }
    }
}

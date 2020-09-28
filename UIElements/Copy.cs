using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using Razorwing.Framework.Extensions.ColorExtensions;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ModKit.Tools.REPL;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.ModKit
{
    public class Copy : UIState
    {
        private Tile[,] tileCopy;
        private static Point start, end, copyPos, psize;
        internal static bool Visible = false;
        private CopyCaptureMode mode = CopyCaptureMode.disabled;
        private CheatPannel controlPannel;
        private Asset<Texture2D> cursor1, cursor2;

        internal CopyCaptureMode Mode
        {
            get => mode;
            set
            {
                if(mode == value)
                    return;
                if (value == CopyCaptureMode.disabled)
                    Visible = false;
                mode = value;
            }
        }

        internal void initialize()
        {
            tileCopy = null;
            controlPannel = new CheatPannel(110)
            {
                Left = new StyleDimension(-(180) / 2, 0.5f),
                Top = new StyleDimension(-110, 1f),
                Width = new StyleDimension(200, 0f),
                Height = new StyleDimension(55, 0f),
                BackgroundColor = Color.BlanchedAlmond.Opacity(0.3f),
            };

            Append(controlPannel);

            var texture = cursor1 = Main.Assets.Request<Texture2D>(@"Images\UI\Cursor_4");
            cursor2 = Main.Assets.Request<Texture2D>(@"Images\UI\Cursor_5");

            UIHoverImageButton btn = new UIHoverImageButton(texture, texture.Frame(),
                "Set points",
                () =>
                {
                    mode = CopyCaptureMode.select;
                });
            btn.Left.Set(10, 0f);
            controlPannel.Append(btn);


            texture = Main.Assets.Request<Texture2D>(@"Images\UI\Cursor_6");
            btn = new UIHoverImageButton(texture, texture.Frame(),
                "Remove selection",
                () =>
                {
                    mode = CopyCaptureMode.disabled;
                    start = new Point();
                    end = new Point();
                    tileCopy = null;
                    copyPos = new Point();
                });
            btn.Left.Set(60, 0f);
            controlPannel.Append(btn);

            texture = Main.Assets.Request<Texture2D>(@"Images\UI\Cursor_7");
            btn = new UIHoverImageButton(texture, texture.Frame(),
                "Disable selector",
                () => { mode = CopyCaptureMode.idle; });
            btn.Left.Set(110, 0f);
            controlPannel.Append(btn);

            texture = Main.Assets.Request<Texture2D>(@"Images\UI\Cursor_3");
            btn = new UIHoverImageButton(texture, texture.Frame(),
                "Insert",
                () => { mode = CopyCaptureMode.insert; });
            btn.Left.Set(160, 0f);
            controlPannel.Append(btn);
        }



        

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(Visible)
                Update();
        }

        private bool lk, rk;

        internal void Update()
        {
            if (!Visible || Main.netMode != 0)
                return;

            switch (Mode)
            {
                case CopyCaptureMode.idle:

                    break;
                case CopyCaptureMode.@select:

                    if (Main.mouseLeft && !lk && Main.keyState.PressingControl())
                    {
                        start = Main.MouseWorld.ToTileCoordinates();
                        lk = true;
                    }
                    else if (Main.mouseRight && !rk && Main.keyState.PressingControl())
                    {
                        end = Main.MouseWorld.ToTileCoordinates();
                        rk = true;
                    }
                    else if (!Main.mouseLeft && !Main.mouseLeft)
                    {
                        rk = false;
                        lk = false;
                        break;
                    }

                    if (start != end && (start.X != 0 && start.Y != 0) && (end.X != 0 && end.Y != 0))
                    {
                        var pos = new Point((start.X < end.X ? start.X : end.X),
                            (start.Y < end.Y ? start.Y : end.Y));
                        var size = new Point(start.X - end.X, start.Y - end.Y);
                        if (size.X < 0)
                            size.X *= -1;
                        if (size.Y < 0)
                            size.Y *= -1;
                        tileCopy = new Tile[(int) size.X, (int) size.Y];
                        for (int i = 0; i < size.X; i++)
                        {
                            for (int j = 0; j < size.Y; j++)
                            {
                                tileCopy[i, j] = (Tile) Main.tile[pos.X + i, pos.Y + j].Clone();
                            }
                        }
                    }


                    break;

                case CopyCaptureMode.insert:
                    if (Main.mouseLeft && !lk && Main.keyState.PressingControl())
                    {
                        if (start != end && (start.X != 0 && start.Y != 0) && (end.X != 0 && end.Y != 0))
                        {
                            var pos = copyPos = Main.MouseWorld.ToTileCoordinates();
                            psize = new Point(start.X - end.X, start.Y - end.Y);
                            if (psize.X < 0)
                                psize.X *= -1;
                            if (psize.Y < 0)
                                psize.Y *= -1;
                            //tileCopy = new Tile[(int)size.X, (int)size.Y];
                            for (int i = 0; i < psize.X; i++)
                            {
                                for (int j = 0; j < psize.Y; j++)
                                {
                                    Main.tile[(int) pos.X + i, (int) pos.Y + j] = new Tile(tileCopy[i, j]);
                                    //tileCopy[i, j] = (Tile)Main.tile[pos.X + i, pos.Y + j].Clone();
                                }
                            }
                        }

                        lk = true;
                    }
                    else if (Main.mouseLeft && lk)
                    {
                        try
                        {
                            var p = Main.MouseWorld.ToTileCoordinates();
                            int dx = copyPos.X - p.X, dy = copyPos.Y - p.Y;
                            if (dx >= psize.X || dx <= -psize.X)
                            {
                                copyPos.X -= dx > 0 ? psize.X : -psize.X;
                                for (int i = 0; i < psize.X; i++)
                                {
                                    for (int j = 0; j < psize.Y; j++)
                                    {
                                        Main.tile[(int) copyPos.X + i, (int) copyPos.Y + j] = new Tile(tileCopy[i, j]);
                                        //tileCopy[i, j] = (Tile)Main.tile[pos.X + i, pos.Y + j].Clone();
                                    }
                                }
                            }

                            if (dy >= psize.Y || dy <= -psize.Y)
                            {
                                copyPos.Y -= dy > 0 ? psize.Y : -psize.Y;
                                for (int i = 0; i < psize.X; i++)
                                {
                                    for (int j = 0; j < psize.Y; j++)
                                    {
                                        Main.tile[(int) copyPos.X + i, (int) copyPos.Y + j] = new Tile(tileCopy[i, j]);

                                        //tileCopy[i, j] = (Tile)Main.tile[pos.X + i, pos.Y + j].Clone();
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }
                    else if(!Main.mouseLeft)
                    {
                        rk = false;
                        lk = false;
                        break;
                    }
                    break;

            }
        }


        internal void UIDraw()
        {
            if (!Visible || Main.netMode != 0)
                return;
            var s = start.ToWorldCoordinates(0, 0);
            var e = end.ToWorldCoordinates(0,0);
            var pos = new Vector2((s.X < e.X ? s.X : e.X),
                          (s.Y < e.Y ? s.Y : e.Y));
            var size = s - e;
            if (size.X < 0)
                size.X *= -1;
            if (size.Y < 0)
                size.Y *= -1;

            if (mode != CopyCaptureMode.insert)
                DrawBorderedRect(Main.spriteBatch, Color.Gray * 0.2f, Color.White, pos, size, 3);
            else
            {
                pos = Main.MouseWorld.ToTileCoordinates().ToWorldCoordinates(0, 0);//Bind visual to grid
                DrawBorderedRect(Main.spriteBatch, Color.Gray * 0.2f, Color.White, pos, size, 3);
            }
        }

        public void DrawBorderedRect(SpriteBatch spriteBatch, Color color, Color borderColor, Vector2 position, Vector2 size, int borderWidth)
        {

            {
                //var v = start.ToWorldCoordinates();
                //var b = start.ToVector2() * 16;
                Vector2 startV = (start.ToWorldCoordinates(0, 0)) - Main.screenPosition;
                Vector2 s = cursor1.Size();
                float rotation = 1f;
                var e = SpriteEffects.None;
                if (start.X < end.X)
                {
                    e = SpriteEffects.FlipHorizontally;
                }

                if (start.Y < end.Y)
                {
                    if (e == SpriteEffects.None)
                        e = SpriteEffects.FlipVertically;
                    else
                        e &= SpriteEffects.FlipVertically;
                }
                spriteBatch.Draw(cursor1.Value, new Rectangle((int)(startV.X), (int)(startV.Y),
                        (int)(s.X), (int)(s.Y)),
                    null, Color.White, 0f, s/2, e, rotation  );
            }
            {
                Vector2 endV = (end.ToWorldCoordinates(0, 0)) - Main.screenPosition;
                Vector2 s = cursor2.Size();
                float rotation = 1f;
                var e = SpriteEffects.None;
                if (start.X > end.X)
                {
                    e = SpriteEffects.FlipHorizontally;
                }

                if (start.Y > end.Y)
                {
                    if(e == SpriteEffects.None)
                        e = SpriteEffects.FlipVertically;
                    else
                        e &= SpriteEffects.FlipVertically;
                }
                spriteBatch.Draw(cursor2.Value, new Rectangle((int)(endV.X), (int)(endV.Y), 
                        (int)(s.X), (int)(s.Y)),
                    null, Color.White, 0f, s / 2, e, rotation);
            }

            if (start == end || (start.X == 0 && start.Y == 0) || (end.X == 0 && end.Y == 0))
            {
                return;
            }

            position -= Main.screenPosition;

            spriteBatch.Draw(REPLTool.magicPixel, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
            spriteBatch.Draw(REPLTool.magicPixel, new Rectangle((int)position.X - borderWidth, (int)position.Y - borderWidth, (int)size.X + borderWidth * 2, borderWidth), borderColor);
            spriteBatch.Draw(REPLTool.magicPixel, new Rectangle((int)position.X - borderWidth, (int)position.Y + (int)size.Y, (int)size.X + borderWidth * 2, borderWidth), borderColor);
            spriteBatch.Draw(REPLTool.magicPixel, new Rectangle((int)position.X - borderWidth, (int)position.Y, (int)borderWidth, (int)size.Y), borderColor);
            spriteBatch.Draw(REPLTool.magicPixel, new Rectangle((int)position.X + (int)size.X, (int)position.Y, (int)borderWidth, (int)size.Y), borderColor);
        }

        public static string Fill(ushort type)
        {
            try
            {
                var pos = copyPos = Main.MouseWorld.ToTileCoordinates();

                for (int i = 0; i < psize.X; i++)
                {
                    for (int j = 0; j < psize.Y; j++)
                    {
                        Main.tile[(int)pos.X + i, (int)pos.Y + j].type = type;
                        Main.tile[(int) pos.X + i, (int) pos.Y + j].active(true);
                        //tileCopy[i, j] = (Tile)Main.tile[pos.X + i, pos.Y + j].Clone();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Failure";
            }

            return "Done!";
        }

    }

    internal enum CopyCaptureMode
    {
        disabled, idle, select, insert,
    }
}

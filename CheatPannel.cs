using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Razorwing.Framework.Extensions.ColorExtensions;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.ModKit.Tools.REPL;
using Terraria.UI;

namespace Terraria.ModKit
{
    internal class UIHoverImageButton : UIImageFramed
    {
        private readonly Action onPressed;
        internal string HoverText;

        private bool lk;

        public UIHoverImageButton(Asset<Texture2D> texture, Rectangle frame, string hoverText, Action onPress = null) :
            base(texture, frame)
        {
            HoverText = hoverText;
            onPressed = onPress;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (ContainsPoint(Main.MouseScreen))
                Main.instance.MouseTextNoOverride(HoverText);
        }

        public override void Update(GameTime gameTime)
        {
            if(!CheatState.Visible)
                return;
            base.Update(gameTime);
            if (ContainsPoint(Main.MouseScreen) && Main.mouseLeft)
            {
                if (!lk)
                {
                    lk = true;
                    onPressed?.Invoke();
                }
            }
            else
            {
                lk = false;
            }
        }
    }

    class CheatPannel : UIPanel
    {
        private int offset;
        public bool Visible = true;

        public CheatPannel(int off = 65)
        {
            offset = off;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Visible)
                return;
            base.Update(gameTime); // don't remove.

            // Checking ContainsPoint and then setting mouseInterface to true is very common. This causes clicks on this UIElement to not cause the player to use current items. 
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

                Top.Set((Main.screenHeight - offset) , 0f);
            Left.Set(((float) Main.screenWidth / 2 - Width.Pixels / 2) , 0f);
            BackgroundColor = Color.BlanchedAlmond.Opacity(0.1f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;
            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;
            base.DrawSelf(spriteBatch);
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;
            base.DrawChildren(spriteBatch);
        }



    }

    internal class CheatState : UIState
    {
        public static bool Visible = true;

        private UIHoverImageButton cycleMode, flyMode, godMode, changeDifficulty, lockOn;
        private CheatPannel mainPannel, difficultyPannel;
        //private static REPLTool tools;

        public override void OnInitialize()
        {
 

            mainPannel = new CheatPannel();
            mainPannel.Left.Set(((float) Main.screenWidth / 2 - Width.Pixels) / Main.UIScale, 0f);
            mainPannel.Top.Set((Main.screenHeight - 64)/Main.UIScale, 0f);
            mainPannel.Width.Set(310, 0f);
            mainPannel.Height.Set(55f, 0f);
            mainPannel.BackgroundColor = Color.BlanchedAlmond.Opacity(0.3f);
            Append(mainPannel);
            //Main.Assets.Request<Texture2D>(@"Images\UI\Creative\Journey_Toggle");
            var texture = Main.Assets.Request<Texture2D>(@"Images\UI\Creative\Journey_Toggle");
            UIHoverImageButton journeyButton = cycleMode = new UIHoverImageButton(texture, texture.Frame(),
                "Toggle journey mode",
                () =>
                {
                    Entry.CycleMode();
                    
                    cycleMode.Color = Main.GameMode == GameModeData.CreativeMode.Id ? Color.Gold : Color.White;
                });
            journeyButton.Left.Set(0, 0);
            journeyButton.Top.Set(0, 0);
            mainPannel.Append(journeyButton);

            texture = Main.Assets.Request<Texture2D>(@"Images\Item_493");
            journeyButton = flyMode = new UIHoverImageButton(texture, texture.Frame(), "Toggle Fly mode",
                () =>
                {
                    Entry.ChangeFly();
                    flyMode.Color = Entry.fly ? Color.Gold : Color.White;
                });
            journeyButton.Left.Set(50, 0);
            journeyButton.Top.Set(0, 0);
            mainPannel.Append(journeyButton);

            texture = Main.Assets.Request<Texture2D>(@"Images\UI\Creative\Infinite_Powers");
            Point p = CreativePowersHelper.CreativePowerIconLocations.Godmode;
            journeyButton = godMode = new UIHoverImageButton(texture, texture.Frame(21, 1, p.X, p.Y), "God Mode", () =>
            {
                Reflect.Invoke<object>(CreativePowerManager.Instance.GetPower<CreativePowers.GodmodePower>(),
                    "RequestUse");
                Main.LocalPlayer.creativeGodMode = !Main.LocalPlayer.creativeGodMode;
                godMode.Color = Main.LocalPlayer.creativeGodMode ? Color.Gold : Color.White;
            });
            journeyButton.Left.Set(100, 0);
            journeyButton.Top.Set(0, 0);
            mainPannel.Append(journeyButton); 


            texture = Main.Assets.Request<Texture2D>(@"Images\UI\WorldCreation\IconDifficultyMaster");
            journeyButton = changeDifficulty = new UIHoverImageButton(texture, texture.Frame(), "Change Difficulty",
                () =>
                {
                    difficultyPannel.Visible = !difficultyPannel.Visible;
                    changeDifficulty.Color = difficultyPannel.Visible ? Color.Gold : Color.White;
                });
            journeyButton.Left.Set(150, 0);
            journeyButton.Top.Set(0, 0);
            mainPannel.Append(journeyButton);

            texture = Main.Assets.Request<Texture2D>(@"Images\Item_1315");
            journeyButton = lockOn = new UIHoverImageButton(texture, texture.Frame(), "Reveal map",
                () =>
                {
                    if (Main.netMode == 0)
                        Entry.RevealWholeMap();
                    else
                    {
                        Point center = Main.player[Main.myPlayer].Center.ToTileCoordinates();
                        Entry.RevealAroundPoint(center.X, center.Y);
                    }

                });
            journeyButton.Left.Set(200, 0);
            journeyButton.Top.Set(0, 0);
            mainPannel.Append(journeyButton);

            texture = Main.Assets.Request<Texture2D>(@"Images\ui\Settings_Inputs_2");
            journeyButton = new UIHoverImageButton(texture, texture.Frame(2, 2, 1, 0), "Open REPL",
                () =>
                {
                    Entry.tools.visible = !Entry.tools.visible;
                });
            journeyButton.Left.Set(250, 0);
            journeyButton.Top.Set(0, 0);
            mainPannel.Append(journeyButton);


            //var tinker = new UITinker();
            //Append(tinker);


            difficultyPannel = new CheatPannel(65 * 2);
            difficultyPannel.Left.Set((float) Main.screenWidth / 2 - Width.Pixels, 0f);
            difficultyPannel.Top.Set(Main.screenHeight - 65 * 2, 0f);
            difficultyPannel.Width.Set(210, 0f);
            difficultyPannel.Height.Set(55f, 0f);
            difficultyPannel.BackgroundColor = Color.BlanchedAlmond.Opacity(0.3f);
            difficultyPannel.Visible = false;
            Append(difficultyPannel);

            texture = Main.Assets.Request<Texture2D>(@"Images\UI\WorldCreation\IconDifficultyCreative");
            journeyButton = new UIHoverImageButton(texture, texture.Frame(), "Journey",
                () =>
                {
                    Main.GameMode = GameModeData.CreativeMode.Id;
                    Main.LocalPlayer.difficulty = 3;
                });
            journeyButton.Left.Set(0, 0);
            journeyButton.Top.Set(0, 0);
            difficultyPannel.Append(journeyButton);

            texture = Main.Assets.Request<Texture2D>(@"Images\UI\WorldCreation\IconDifficultyNormal");
            journeyButton = new UIHoverImageButton(texture, texture.Frame(), "Classic",
                () =>
                {
                    Main.GameMode = GameModeData.NormalMode.Id;
                    Main.LocalPlayer.difficulty = 0;
                });
            journeyButton.Left.Set(50, 0);
            journeyButton.Top.Set(0, 0);
            difficultyPannel.Append(journeyButton);

            texture = Main.Assets.Request<Texture2D>(@"Images\UI\WorldCreation\IconDifficultyExpert");
            journeyButton = new UIHoverImageButton(texture, texture.Frame(), "Expert",
                () =>
                {
                    Main.GameMode = GameModeData.ExpertMode.Id;
                    Main.LocalPlayer.difficulty = 0;
                });
            journeyButton.Left.Set(100, 0);
            journeyButton.Top.Set(0, 0);
            difficultyPannel.Append(journeyButton);

            texture = Main.Assets.Request<Texture2D>(@"Images\UI\WorldCreation\IconDifficultyMaster");
            journeyButton = new UIHoverImageButton(texture, texture.Frame(), "Master",
                () =>
                {
                    Main.GameMode = GameModeData.MasterMode.Id;
                    Main.LocalPlayer.difficulty = 0;
                });
            journeyButton.Left.Set(150, 0);
            journeyButton.Top.Set(0, 0);
            difficultyPannel.Append(journeyButton);

            base.OnInitialize();
        }

        //public override void Update(GameTime gameTime)
        //{
        //    base.Update(gameTime);
        //    tools.UIUpdate();
        //}

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    base.Draw(spriteBatch);
        //    tools.UIDraw();
        //}
    }
}
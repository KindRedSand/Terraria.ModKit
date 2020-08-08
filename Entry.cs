using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Razorwing.Framework.IO.Stores;
using Razorwing.Framework.Logging;
using Razorwing.Framework.Platform;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModKit.Razorwing.Overrides;
using Terraria.ModKit.Tools.REPL;
using Terraria.UI;
// ReSharper disable PossibleLossOfFraction
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Terraria.ModKit
{
    public class Entry
    {
        private static CreativeInputConfig creativeConfig;

        private static UserInterface inter;
        public static bool fly;
        private static bool firstUpdate;
        private static Dictionary<string, List<string>> inputDic;
        private static Keys[] FlyInput = {Keys.W, Keys.S, Keys.A, Keys.D};
        private static float flyDelta = 5;
        private static UIManageControls controlsUI;

        private static List<string> creativeControlsString = new List<string>
        {
            "CycleMode",
            "FlyMode",
            "UnlockAllItems",
            "UnlockAllBestiary",
            "LockBestiary",
            "IncreaseFlySpeed",
            "DecreaseFlySpeed",
            "InstantRevive"
        };

        private static Keys[] CreativeInput =
        {
            Keys.NumPad0,
            Keys.NumPad4,
            Keys.NumPad5,
            Keys.NumPad3,
            Keys.NumPad7,
            Keys.OemOpenBrackets,
            Keys.OemCloseBrackets,
            Keys.NumPad9
        };

        private static Vector2 delta = Vector2.Zero;
        private static float flyIndex = 3;
        private static int installUI = 5, oldMode;
        private static byte oldPlayerMode;
        private static bool instantRevive;
        internal static CreativeInputConfig Config { get; set; }
        internal static DesktopStorage Storage { get; set; }
        internal static ResourceStore<byte[]> Store { get; private set; }


        public static void Text(string m)
        {
            Console.WriteLine(m);
            if (!Main.gameMenu)
                switch (Main.netMode)
                {
                    case 0:
                    case 1:
                        Main.NewText(m);
                        break;
                }
        }


        public static void Initialise()
        {
            Main.OnTickForInternalCodeOnly += Update;
            Main.versionNumber = "ModKit v0.4\n"+ Main.versionNumber;

            Config = new CreativeInputConfig(Storage = new ModStorage(@"Creative"));

            Store = new ResourceStore<byte[]>(new StorageBackedResourceStore(Storage));
            Store.AddStore(new OnlineStore());

            Logger.Storage = Storage;
            Logger.Level = LogLevel.Debug;

            creativeConfig = new CreativeInputConfig(Storage);
            creativeConfig.Save();
            CreativeInput[0] = creativeConfig.Get<Keys>(InputConfig.CycleMode);
            CreativeInput[1] = creativeConfig.Get<Keys>(InputConfig.FlyMode);
            CreativeInput[2] = creativeConfig.Get<Keys>(InputConfig.UnlockAllItems);
            CreativeInput[3] = creativeConfig.Get<Keys>(InputConfig.UnlockAllBestiary);
            CreativeInput[4] = creativeConfig.Get<Keys>(InputConfig.LockBestiary);
            CreativeInput[5] = creativeConfig.Get<Keys>(InputConfig.IncreaseFlySpeed);
            CreativeInput[6] = creativeConfig.Get<Keys>(InputConfig.DecreaseFlySpeed);


            //InGameUI 36
            inter = new UserInterface();
            inter.SetState(new CheatState());

            tools = new REPLTool();
            tools.ClientInitialize();

        }

        private static void SetupFlySpeed()
        {
            switch (flyIndex)
            {
                case 1:
                    Text("Fly speed now: 1");
                    flyDelta = 1;
                    break;
                case 2:
                    flyDelta = 2.5f;
                    Text("Fly speed now: 2.5");
                    break;
                case 3:
                    flyDelta = 5;
                    Text("Fly speed now: 5");
                    break;
                case 4:
                    flyDelta = 10;
                    Text("Fly speed now: 10");
                    break;
                case 5:
                    flyDelta = 15;
                    Text("Fly speed now: 15");
                    break;
                case 6:
                    flyDelta = 20;
                    Text("Fly speed now: 20");
                    break;
                case 7:
                    flyDelta = 30;
                    Text("Fly speed now: 30");
                    break;
                case 8:
                    flyDelta = 40;
                    Text("Fly speed now: 40");
                    break;
                case 9:
                    flyDelta = 50;
                    Text("Fly speed now: 50");
                    break;
                case 10:
                    flyDelta = 100;
                    Text("Fly speed now: 100");
                    break;
            }
        }

        private static void BindingChanged()
        {
            inputDic = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].WritePreferences();
            if (Enum.TryParse(inputDic["Up"][0], true, out Keys r))
                FlyInput[0] = r;
            if (Enum.TryParse(inputDic["Down"][0], true, out r))
                FlyInput[1] = r;
            if (Enum.TryParse(inputDic["Right"][0], true, out r))
                FlyInput[2] = r;
            if (Enum.TryParse(inputDic["Left"][0], true, out r))
                FlyInput[3] = r;

            var KeyStatus = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus;
            if (!KeyStatus.ContainsKey("CycleMode"))
            {
                KeyStatus.Add("CycleMode", new List<string> {CreativeInput[0].ToString()});
                //CreativeInput[0] = Keys.NumPad0;
            }
            else
            {
                if (Enum.TryParse(KeyStatus["CycleMode"][0], out Keys key))
                {
                    creativeConfig.Set(InputConfig.CycleMode, key);
                    CreativeInput[0] = key;
                }
            }

            if (!KeyStatus.ContainsKey("FlyMode"))
            {
                KeyStatus.Add("FlyMode", new List<string> {CreativeInput[1].ToString()});
                //CreativeInput[1] = Keys.NumPad4;
            }
            else
            {
                if (Enum.TryParse(KeyStatus["FlyMode"][0], out Keys key))
                {
                    creativeConfig.Set(InputConfig.FlyMode, key);
                    CreativeInput[1] = key;
                }
            }

            if (!KeyStatus.ContainsKey("UnlockAllItems"))
            {
                KeyStatus.Add("UnlockAllItems", new List<string> {CreativeInput[2].ToString()});
                //CreativeInput[2] = Keys.NumPad5;
            }
            else
            {
                if (Enum.TryParse(KeyStatus["UnlockAllItems"][0], out Keys key))
                {
                    creativeConfig.Set(InputConfig.UnlockAllItems, key);
                    CreativeInput[2] = key;
                }
            }

            if (!KeyStatus.ContainsKey("UnlockAllBestiary"))
            {
                KeyStatus.Add("UnlockAllBestiary", new List<string> {CreativeInput[3].ToString()});
                //CreativeInput[3] = Keys.NumPad3;
            }
            else
            {
                if (Enum.TryParse(KeyStatus["UnlockAllBestiary"][0], out Keys key))
                {
                    creativeConfig.Set(InputConfig.UnlockAllBestiary, key);
                    CreativeInput[3] = key;
                }
            }

            if (!KeyStatus.ContainsKey("LockBestiary"))
            {
                KeyStatus.Add("LockBestiary", new List<string> {CreativeInput[4].ToString()});
                //CreativeInput[4] = Keys.NumPad7;
            }
            else
            {
                if (Enum.TryParse(KeyStatus["LockBestiary"][0], out Keys key))
                {
                    creativeConfig.Set(InputConfig.LockBestiary, key);
                    CreativeInput[4] = key;
                }
            }

            if (!KeyStatus.ContainsKey("IncreaseFlySpeed"))
            {
                KeyStatus.Add("IncreaseFlySpeed", new List<string> {CreativeInput[5].ToString()});
                //CreativeInput[5] = Keys.OemOpenBrackets;
            }
            else
            {
                if (Enum.TryParse(KeyStatus["IncreaseFlySpeed"][0], out Keys key))
                {
                    creativeConfig.Set(InputConfig.IncreaseFlySpeed, key);
                    CreativeInput[5] = key;
                }
            }

            if (!KeyStatus.ContainsKey("DecreaseFlySpeed"))
            {
                KeyStatus.Add("DecreaseFlySpeed", new List<string> {CreativeInput[6].ToString()});
                //CreativeInput[6] = Keys.OemCloseBrackets;
            }
            else
            {
                if (Enum.TryParse(KeyStatus["DecreaseFlySpeed"][0], out Keys key))
                {
                    creativeConfig.Set(InputConfig.DecreaseFlySpeed, key);
                    CreativeInput[6] = key;
                }
            }

            if (!KeyStatus.ContainsKey("InstantRevive"))
            {
                KeyStatus.Add("InstantRevive", new List<string> {CreativeInput[7].ToString()});
                //CreativeInput[6] = Keys.OemCloseBrackets;
            }
            else
            {
                if (Enum.TryParse(KeyStatus["InstantRevive"][0], out Keys key))
                {
                    creativeConfig.Set(InputConfig.InstantRespawn, key);
                    CreativeInput[7] = key;
                }
            }
        }


        internal static REPLTool tools;
        private static void Update()
        {
            if (Main.gameMenu) return;

            if (installUI > 0) //Skip some frames before access layers
            {
                installUI--;
                if (installUI == 0)
                {
                    var layers = Reflect.GetF<List<GameInterfaceLayer>>(Main.instance, "_gameInterfaceLayers");
                    layers.Insert(36, new LegacyGameInterfaceLayer("Creative mod: Custom UI", () =>
                    {
                        if (Main.gameMenu)
                            return false;
                        if (CheatState.Visible)
                            inter.Draw(Main.spriteBatch, new GameTime());
                        if(tools.visible)
                            tools.UIDraw();
                        return true;
                    }));
                }
            }

            if (controlsUI == null && Main.InGameUI.CurrentState != null)
            {
                if (Main.InGameUI.CurrentState is UIManageControls ui)
                {
                    Console.WriteLine("Hijack UIManageControls. Adding own input panel...");
                    controlsUI = ui;
                    try
                    {
                        var pan = Reflect.Invoke<UISortableElement>(controlsUI, "CreateBindingGroup", 3,
                            creativeControlsString, InputMode.Keyboard);
                        Reflect.GetF<List<UIElement>>(controlsUI, "_bindsKeyboard").Add(pan);
                        Reflect.GetF<List<UIElement>>(controlsUI, "_bindsKeyboardUI").Add(pan);

                        var KeyStatus = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus;


                        if (!KeyStatus.ContainsKey("CycleMode"))
                            KeyStatus.Add("CycleMode", new List<string> {CreativeInput[0].ToString()});
                        if (!KeyStatus.ContainsKey("FlyMode"))
                            KeyStatus.Add("FlyMode", new List<string> {CreativeInput[1].ToString()});
                        if (!KeyStatus.ContainsKey("UnlockAllItems"))
                            KeyStatus.Add("UnlockAllItems", new List<string> {CreativeInput[2].ToString()});
                        if (!KeyStatus.ContainsKey("UnlockAllBestiary"))
                            KeyStatus.Add("UnlockAllBestiary", new List<string> {CreativeInput[3].ToString()});
                        if (!KeyStatus.ContainsKey("LockBestiary"))
                            KeyStatus.Add("LockBestiary", new List<string> {CreativeInput[4].ToString()});
                        if (!KeyStatus.ContainsKey("IncreaseFlySpeed"))
                            KeyStatus.Add("IncreaseFlySpeed", new List<string> {CreativeInput[5].ToString()});
                        if (!KeyStatus.ContainsKey("DecreaseFlySpeed"))
                            KeyStatus.Add("DecreaseFlySpeed", new List<string> {CreativeInput[6].ToString()});
                        if (!KeyStatus.ContainsKey("InstantRevive"))
                            KeyStatus.Add("InstantRevive", new List<string> {CreativeInput[7].ToString()});
                        if (!KeyStatus.ContainsKey("InstantRevive"))
                            KeyStatus.Add("InstantRevive", new List<string> { CreativeInput[7].ToString() });


                        PlayerInput.OnBindingChange += BindingChanged;

                        var method2 = controlsUI.GetType()
                            .GetMethod("FillList", BindingFlags.NonPublic | BindingFlags.Instance);
                        //We use catch statement to notify if injection failed
                        // ReSharper disable once PossibleNullReferenceException
                        method2.Invoke(controlsUI, new object[] { });

                        

                        Console.WriteLine("Successively added new panel");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Reflection failed!");
                        Console.WriteLine(e);
                    }
                }
            }

            if (!firstUpdate)
            {
                firstUpdate = true;
                BindingChanged();
            }

#if DEBUG
            if (Main.keyState.IsKeyDown(Keys.R) && Main.oldKeyState.IsKeyUp(Keys.R) && Main.keyState.PressingControl())
            {
                tools.ClientInitialize();
            }
#endif
            //All our ui get broken when UI Scale != 1
            Main.UIScale = 1f;


            if (tools.visible)
                tools.UIUpdate();

            if (Main.LocalPlayer.creativeTracker.ItemSacrifices.SacrificesCountByItemIdCache.Count < 1000 &&
                Main.keyState.IsKeyDown(CreativeInput[2]) && Main.oldKeyState.IsKeyUp(CreativeInput[2]))
            {
                Console.WriteLine("Unlocking all items...");
                for (int i = 0; i < 5079; i++)
                {
                    try
                    {
                        Main.LocalPlayer.creativeTracker.ItemSacrifices.RegisterItemSacrifice(i, 999);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                Text("All items in journey mode unlocked");
            }

            if (Main.keyState.IsKeyDown(CreativeInput[0]) && Main.oldKeyState.IsKeyUp(CreativeInput[0]))
            {
                //CycleMode();
                CheatState.Visible = !CheatState.Visible;
            }

            if (Main.keyState.IsKeyDown(CreativeInput[3]) && Main.oldKeyState.IsKeyUp(CreativeInput[3]))
            {
                Console.WriteLine("Processing Bestiary...");
                try
                {
                    foreach (var it in ContentSamples.NpcBestiaryCreditIdsByNpcNetIds)
                    {
                        Console.WriteLine($"Registering NPCID: {it.Key} -> {it.Value}");
                        Main.BestiaryTracker.Kills.SetKillCountDirectly(it.Value, 9999);
                        Main.BestiaryTracker.Chats.SetWasChatWithDirectly(it.Value);
                        Main.BestiaryTracker.Sights.SetWasSeenDirectly(it.Value);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            if (Main.keyState.IsKeyDown(CreativeInput[4]) && Main.oldKeyState.IsKeyUp(CreativeInput[4]))
            {
                Console.WriteLine("Clearing Bestiary...");
                try
                {
                    Main.BestiaryTracker.Kills.Reset();
                    Main.BestiaryTracker.Chats.Reset();
                    Main.BestiaryTracker.Sights.Reset();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            if (Main.keyState.IsKeyDown(CreativeInput[1]) && Main.oldKeyState.IsKeyUp(CreativeInput[1]))
            {
                ChangeFly();
            }

            if (Main.keyState.IsKeyDown(CreativeInput[5]) && Main.oldKeyState.IsKeyUp(CreativeInput[5]))
            {
                if (flyIndex < 10)
                {
                    flyIndex++;
                    SetupFlySpeed();
                }
            }

            if (Main.keyState.IsKeyDown(CreativeInput[6]) && Main.oldKeyState.IsKeyUp(CreativeInput[6]))
            {
                if (flyIndex > 1)
                {
                    flyIndex--;
                    SetupFlySpeed();
                }
            }

            if (Main.keyState.IsKeyDown(CreativeInput[7]) && Main.oldKeyState.IsKeyUp(CreativeInput[7]))
            {
                instantRevive = !instantRevive;
            }


            //Logic
            delta = Main.LocalPlayer.velocity - Main.LocalPlayer.oldVelocity;

            if (fly)
            {
                if (delta.Length() < 1)
                {
                    Main.LocalPlayer.velocity = -delta;
                    Main.LocalPlayer.position += -delta;
                }
                else
                {
                    Main.LocalPlayer.velocity = Vector2.Zero;
                    Main.LocalPlayer.oldVelocity = Vector2.Zero;
                }

                Main.LocalPlayer.fallStart = (int)(Main.LocalPlayer.position.Y / 16f);

                if (Main.keyState.IsKeyDown(FlyInput[0]))
                    Main.LocalPlayer.position.Y -= flyDelta;
                if (Main.keyState.IsKeyDown(FlyInput[1]))
                    Main.LocalPlayer.position.Y += flyDelta;
                if (Main.keyState.IsKeyDown(FlyInput[2]))
                    Main.LocalPlayer.position.X += flyDelta;
                if (Main.keyState.IsKeyDown(FlyInput[3]))
                    Main.LocalPlayer.position.X -= flyDelta;
            }

            if (instantRevive && Main.LocalPlayer.respawnTimer > 0)
            {
                fly = false;
                Main.LocalPlayer.respawnTimer = 0;
            }

            if (Main.mapFullscreen)
            {
                if (Main.mouseRight && Main.keyState.IsKeyUp(Keys.LeftControl))
                {
                    int mapWidth = Main.maxTilesX * 16;
                    int mapHeight = Main.maxTilesY * 16;
                    Vector2 cursorPosition = new Vector2(Main.mouseX, Main.mouseY);

                    cursorPosition.X -= Main.screenWidth / 2;
                    cursorPosition.Y -= Main.screenHeight / 2;

                    Vector2 mapPosition = Main.mapFullscreenPos;
                    Vector2 cursorWorldPosition = mapPosition;

                    cursorPosition /= 16;
                    cursorPosition *= 16 / Main.mapFullscreenScale;
                    cursorWorldPosition += cursorPosition;
                    cursorWorldPosition *= 16;

                    Player player = Main.player[Main.myPlayer];
                    cursorWorldPosition.Y -= player.height;
                    if (cursorWorldPosition.X < 0) cursorWorldPosition.X = 0;
                    else if (cursorWorldPosition.X + player.width > mapWidth)
                        cursorWorldPosition.X = mapWidth - player.width;
                    if (cursorWorldPosition.Y < 0) cursorWorldPosition.Y = 0;
                    else if (cursorWorldPosition.Y + player.height > mapHeight)
                        cursorWorldPosition.Y = mapHeight - player.height;

                    if (Main.netMode == 0) // single
                    {
                        player.Teleport(cursorWorldPosition, 1);
                        player.position = cursorWorldPosition;
                        player.velocity = Vector2.Zero;
                        player.fallStart = (int) (player.position.Y / 16f);
                    }
                    else // 1, client
                    {
                        NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, cursorWorldPosition.X,
                            cursorWorldPosition.Y, 1);
                        player.Teleport(cursorWorldPosition, 1);
                        player.position = cursorWorldPosition;
                        player.velocity = Vector2.Zero;
                        player.fallStart = (int)(player.position.Y / 16f);
                    }
                }
            }

            inter.Update(new GameTime());
        }

        public static void RevealWholeMap()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (WorldGen.InWorld(i, j))
                        Main.Map.Update(i, j, 255);
                }
            }
            Main.refreshMap = true;
        }

        public static void RevealAroundPoint(int x, int y)
        {
            const int MapRevealSize = 300;
            for (int i = x - MapRevealSize / 2; i < x + MapRevealSize / 2; i++)
            {
                for (int j = y - MapRevealSize / 2; j < y + MapRevealSize / 2; j++)
                {
                    if (WorldGen.InWorld(i, j))
                        Main.Map.Update(i, j, 255);
                }
            }
            Main.refreshMap = true;
        }

        public static void ChangeFly()
        {
            fly = !fly;
            string status = fly ? "enabled" : "disabled";
            Text($"Fly mode now: {status}");
        }

        public static void CycleMode()
        {
            Console.WriteLine("Changing difficulty...");
            if (Main.LocalPlayer.difficulty == 3)
            {
                Main.GameMode = oldMode;
                Main.LocalPlayer.difficulty = oldPlayerMode;
                Console.WriteLine("Journey mode disabled");
            }
            else
            {
                oldMode = Main.GameMode;
                oldPlayerMode = Main.LocalPlayer.difficulty;
                Main.GameMode = GameModeData.CreativeMode.Id;
                Main.LocalPlayer.difficulty = 3;
                if (!Main.playerInventory)
                    Main.playerInventory = true;
                Main.CreativeMenu.ToggleMenu();
                Console.WriteLine("Journey Mode Enabled");
            }
        }
    }
}
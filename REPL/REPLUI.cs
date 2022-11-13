﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria.ID;
using System.Linq;
using System.Text;
using Terraria.ModKit.UIElements;
using Terraria.ModKit.Tools;
using Terraria.ModKit.Tools.REPL;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using ReLogic.Content;
using Terraria.Graphics;

namespace Terraria.ModKit.REPL
{
	class REPLUI : UIState
	{
		public static bool visible = false;
		public UIElements.FixedUIScrollbar keyboardScrollbar;
		public UIPanel keyboardPanel;
		public UIList replOutput;
		public NewUITextBoxMultiLine codeTextBox;
		public static string filterText = "";
		private UserInterface userInterface;

		public REPLUI(UserInterface userInterface)
		{
			this.userInterface = userInterface;
		}

		public override void OnInitialize()
		{
			keyboardPanel = new UIPanel();
			keyboardPanel.SetPadding(8);
			keyboardPanel.Left.Set(-550f, 1f);
			keyboardPanel.Top.Set(-370f, 1f);
			keyboardPanel.Width.Set(500f, 0f);
			keyboardPanel.Height.Set(300f, 0f);
			keyboardPanel.BackgroundColor = new Color(73, 94, 171);

			codeTextBox = new NewUITextBoxMultiLine("Type code here", 1f);
			codeTextBox.SetUnfocusKeys(false, false);
			codeTextBox.BackgroundColor = Color.Transparent;
			codeTextBox.BorderColor = Color.Transparent;
			codeTextBox.Left.Pixels = 0;
			codeTextBox.Top.Pixels = 0;
			codeTextBox.Width.Set(-20, 1f);
			//filterTextBox.OnTextChanged += () => { filterText = filterTextBox.Text; updateneeded = true; };
			codeTextBox.OnEnterPressed += EnterAction;
			codeTextBox.OnTabPressed += TabAction;
			codeTextBox.OnUpPressed += UpAction;
			//keyboardPanel.Append(codeTextBox);

			replOutput = new UIList();
			replOutput.Width.Set(-25f, 1f); // left spacing plus scrollbar
			//replOutput.Height.Set(-codeTextBox.GetDimensions().Height - 32, 1f);
			replOutput.Height.Set(-26, 1f);
			replOutput.Left.Set(0, 0f);
			//replOutput.Top.Set(codeTextBox.GetDimensions().Height, 0f);
			replOutput.Top.Set(0, 0f);
			replOutput.ListPadding = 10f;
			replOutput.Add(codeTextBox);
			keyboardPanel.Append(replOutput);

			keyboardScrollbar = new UIElements.FixedUIScrollbar(userInterface);
			keyboardScrollbar.SetView(100f, 1000f);
			keyboardScrollbar.Top.Pixels = codeTextBox.GetDimensions().Height;
			keyboardScrollbar.Height.Set(-26, 1f);
			keyboardScrollbar.Left.Set(-4, 0f);
			keyboardScrollbar.HAlign = 1f;
			keyboardPanel.Append(keyboardScrollbar);

			replOutput.SetScrollbar(keyboardScrollbar);

            Asset<Texture2D> texture = Main.Assets.Request<Texture2D>("Images/UI/Cursor_2");
            UIImageButton eyeDropperButton = new UIImageButton(texture);
			eyeDropperButton.Height.Pixels = 20;
			//eyeDropperButton.Width.Pixels = 20;
			eyeDropperButton.OnLeftClick += EyeDropperButton_OnClick;
			eyeDropperButton.Top.Set(-26, 1f);
			keyboardPanel.Append(eyeDropperButton);

            texture = Main.Assets.Request<Texture2D>("Images/UI/ButtonSeed");
			UIHoverImageButton openText = new UIHoverImageButton(texture, texture.Frame(), "Open External Editor");
			openText.OnLeftClick += OpenTextButton_OnClick;
			openText.Top.Set(-26 / Main.UIScale, 1f);
			openText.Left.Set(26 / Main.UIScale, 0f);
			keyboardPanel.Append(openText);

            texture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay");
            UIHoverImageButton runText = new UIHoverImageButton(texture, texture.Frame(), "Execute External Code");
			runText.OnLeftClick += RunTextButton_OnClick;
			runText.Top.Set(-26, 1f);
			runText.Left.Set(52, 0f);
			keyboardPanel.Append(runText);

			Append(keyboardPanel);
		}

		private void EyeDropperButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			REPLTool.EyedropperActive = !REPLTool.EyedropperActive;
			if (REPLTool.EyedropperActive)
			{
				Main.NewText("Click to select an item/npc/player/dust/tile from the game world");
			}
		}

		private void OpenTextButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			string filename = "Terraria.Code.cs";
			string folder = Path.Combine(Main.SavePath, "Mods", "Cache");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

			string path = Path.Combine(folder, filename);
			if (!File.Exists(path))
			{
				File.WriteAllText(path, "// Write code statements here");
			}
			Process.Start(path);
		}

		private void RunTextButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			string filename = "Terraria.Code.cs";
			string folder = Path.Combine(Main.SavePath, "Mods", "Cache");
			string path = Path.Combine(folder, filename);
			if (File.Exists(path))
			{
				string code = File.ReadAllText(path);
				codeTextBox.SetText(code);
				EnterAction();
			}
			else
			{
				Main.NewText("File does not exist");
			}
			
		}

		public void EnterAction()
		{
			if (Main.oldKeyState.PressingShift())
			{
				codeTextBox.Write("\n");
				//codeTextBox.SetText(codeTextBox.Text + "\n", 1f, false);
				//codeTextBox.Height.Pixels += 20;
				Main.drawingPlayerChat = false;
				return;
			}
			//codeTextBox.Height.Pixels = 20;
			if (codeTextBox.Text == "clear")
			{
				pendingClear = true;
				//replOutput.Clear();
				codeTextBox.SetText("");
				Main.drawingPlayerChat = false;
				return;
			}
			if (codeTextBox.Text == "reset")
			{
				pendingClear = true;
				//replOutput.Clear();
				codeTextBox.SetText("");
				Main.drawingPlayerChat = false;
				REPLTool.replBackend.Reset();
				return;
			}
			AddChunkedLine(codeTextBox.Text, CodeType.Input);
			//replOutput.Add(new UICodeEntry(codeTextBox.Text, CodeType.Input));
			REPLTool.replBackend.Action(codeTextBox.Text);
			codeTextBox.SetText("");
			//Main.chatRelease = false;
			Main.drawingPlayerChat = false;
		}

		public void UpAction()
		{
			foreach (var item in replOutput)
			{
                if (item is UICodeEntry codeEntry && codeEntry.codeType == CodeType.Input)
				{
					codeTextBox.SetText(codeEntry.Text);
					break;
				}
			}
		}

		public void TabAction()
		{
			string[] completions = REPLTool.replBackend.GetCompletions(codeTextBox.Text);
			if (codeTextBox.Text.StartsWith("using "))
			{
				string pre = codeTextBox.Text.Substring(6);
				completions = REPLTool.replBackend.namespaces.ToArray().Where(x => x.StartsWith(pre)).Select(x => x.Substring(pre.Length)).ToArray();
			}
			if (completions == null) return;
			string prefix = LongestCommonPrefix2(completions);
			if (prefix == "")
			{
				if (completions.Length > 0 && completions[0] != "")
				{
					string hint = "{" + string.Join(", ", completions) + "}";
					if (hint.Length > 400)
					{
						hint = hint.Substring(0, 400) + "...";
					}
					AddChunkedLine(hint, CodeType.Hint);
					//replOutput.Add(new UICodeEntry(hint, CodeType.Hint));
				}
			}
			else
			{
				codeTextBox.Write(prefix);
			}
		}

		private bool updateneeded;



		internal void UpdateCheckboxes()
		{
			if (!updateneeded) { return; }
			updateneeded = false;
        }

		private List<UIElement> pendingAdd = new List<UIElement>();
		private bool pendingClear;
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (pendingAdd.Count > 0)
			{
				replOutput.AddRange(pendingAdd);
				pendingAdd.Clear();
			}
			if (pendingClear)
			{
				replOutput.Clear();
				replOutput.Add(codeTextBox);
				pendingClear = false;
			}
			UpdateCheckboxes();
			if (keyboardPanel.ContainsPoint(Main.MouseScreen))
			{
				Main.player[Main.myPlayer].mouseInterface = true;
			}
		}

		public string LongestCommonPrefix2(string[] strs)
		{
			if (strs.Length == 0) return String.Empty;
			Array.Sort(strs);

			var first = strs[0];
			var last = strs[strs.Length - 1];
			var strbuilder = new StringBuilder();
			for (int i = 0; i < first.Length; i++)
			{
				if (first[i] != last[i])
				{
					break;
				}
				strbuilder.Append(first[i]);
			}

			return strbuilder.ToString();
		}

		internal void AddChunkedLine(string buffer, CodeType codeType)
		{
            StringBuilder sb = new StringBuilder();
			int chunkSize = 55;
			int stringLength = buffer.Length;
			int chunks = (int)Math.Ceiling((float)stringLength / chunkSize);
			for (int i = 0; i < chunks; i++)
			{
				int len = chunkSize;
				int start = i * chunkSize;
				if (i == chunks - 1)
				{
					len = stringLength % chunkSize;
				}
				//Console.WriteLine(str.Substring(i, chunkSize));
				//replOutput.Add(new UICodeEntry(buffer.Substring(start, len), codeType));
				sb.Append(buffer.Substring(start, len));
				if (i < chunks - 1) sb.Append("\n");
			}
			//sb.Append(buffer);
			pendingAdd.Add(new UICodeEntry(sb.ToString(), codeType));
		}
	}

	public static class UIListExtensions
    {
        public static void AddRange(this UIList list, IEnumerable<UIElement> range)
        {
            foreach (var it in range)
            {
                list.Add(it);
            }
        }
    }
}

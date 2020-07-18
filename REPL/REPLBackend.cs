using Microsoft.Xna.Framework;
using Terraria;
using Mono.CSharp;
using System.Linq;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using Terraria.ModKit.Tools;
using Terraria.ModKit.Tools.REPL;
using Terraria.ModKit.UIElements;

namespace Terraria.ModKit.REPL
{
	public class REPLBackend
	{
		CompilerContext compilerContext;
		Evaluator evaluator;
		internal List<string> namespaces;

		public REPLBackend()
		{
			Reset();
		}

		public void Reset()
		{
			if (Main.netMode == 2)
			{
				compilerContext = new CompilerContext(new CompilerSettings(), new ConsoleReportPrinter(new ConsoleTextWriter()));
			}
			else
			{
				compilerContext = new CompilerContext(new CompilerSettings(), new ConsoleReportPrinter(new MainNewTextTextWriter()));
			}
			evaluator = new Evaluator(compilerContext);
			namespaces = new List<string>();

			foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly == null)
				{
					continue;
				}
				try
				{
					if (assembly.FullName.Contains("mscorlib") || assembly.FullName.Contains("System.Core,") || assembly.FullName.Contains("System,") || assembly.FullName.Contains("System"))
					{

					}
					else
					{
                        try
                        {
						    var topLevel = assembly.GetTypes()
						       //.Select(t => GetTopLevelNamespace(t))
						       .Select(t => t.Namespace)
						       .Distinct();
						    foreach (string ns in topLevel)
						    {
							    if (ns != null && !ns.StartsWith("<"))
							    {
								    if (!namespaces.Contains(ns))
								    {
									    namespaces.Add(ns);
								    }
							    }
						    }

                            evaluator.ReferenceAssembly(assembly);
                        }
                        catch (Exception e)
                        {
                            
                        }

						//namespaces.AddRange(topLevel);
					}
				}
				catch (NullReferenceException e)
				{
				}
			}
			try
			{
				evaluator.Run("using Terraria");
			}
			catch (Exception)
			{
			}
		}

		static string GetTopLevelNamespace(Type t)
		{
			string ns = t.Namespace ?? "";
			int firstDot = ns.IndexOf('.');
			return firstDot == -1 ? ns : ns.Substring(0, firstDot);
		}

		static string GetNamespace(Type t)
		{
			string ns = t.Namespace ?? "";
			int firstDot = ns.IndexOf('.');
			return firstDot == -1 ? ns : ns.Substring(0, firstDot);
		}

		public string[] GetCompletions(string input)
		{
			string prefix;
			var results = evaluator.GetCompletions(input, out prefix);
			//.NewText("Prefix: " + prefix);
			return results;
		}

		public void Action(string line)
		{
			if (line == null)
				return;

			object result;
			bool result_set;
			evaluator.Evaluate(line, out result, out result_set);
			if (result_set)
			{
				if (result == null)
				{
					result = "<null>";
				}
				//Console.WriteLine(result);
				//Main.NewText(result.ToString());
				if (Main.dedServ)
				{
					Console.WriteLine(result.ToString());
				}
				else
				{
					REPLTool.toolKit.AddChunkedLine(result.ToString(), CodeType.Output);
				}
				//Terraria.ModKit.instance.Terraria.ModKitUI.replOutput.Add(new UICodeEntry(result.ToString(), CodeType.Output));
			}
			else
			{

            }

		}
	}

	public class ConsoleTextWriter : TextWriter
	{
		public ConsoleTextWriter()
		{
		}

		string buffer = "";
		public override void Write(char value)
		{
			if (value == '\n')
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(buffer);
				buffer = "";
			}
			else
			{
				buffer += value;
			}
		}

		public override Encoding Encoding
		{
			get { return System.Text.Encoding.UTF8; }
		}
	}

	public class MainNewTextTextWriter : TextWriter
	{
		public MainNewTextTextWriter()
		{
		}

		string buffer = "";
		public override void Write(char value)
		{
			if (value == '\n')
			{
				REPLTool.toolKit.AddChunkedLine(buffer, CodeType.Error);
                buffer = "";
			}
			else
			{
				buffer += value;
			}
		}

		public string SafeSubstring(string text, int start, int length)
		{
			return text.Length <= start ? ""
				: text.Length - start <= length ? text.Substring(start)
				: text.Substring(start, length);
		}

		public override Encoding Encoding
		{
			get { return System.Text.Encoding.UTF8; }
		}
	}

}

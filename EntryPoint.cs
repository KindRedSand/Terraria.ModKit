using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Terraria.ModKit
{
    internal class EntryPoint
    {
        private static void Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs sargs)
                {
                    bool mono = false;
                    string resourceName = new AssemblyName(sargs.Name).Name + ".dll";
                    string textx = Array.Find<string>(typeof(Program).Assembly.GetManifestResourceNames(), (string element) => element.EndsWith(resourceName));
                    if (textx == null)
                    {
                        textx = Array.Find<string>(typeof(EntryPoint).Assembly.GetManifestResourceNames(), (string element) => element.EndsWith(resourceName));
                        if (textx == null)
                        {
                            return null;
                        }

                        mono = true;
                    }
                    Assembly resultt;
                    if (!mono)
                    {
                        using (Stream manifestResourceStream = typeof(Program).Assembly.GetManifestResourceStream(textx))
                        {
                            byte[] array = new byte[manifestResourceStream.Length];
                            manifestResourceStream.Read(array, 0, array.Length);
                            resultt = Assembly.Load(array);
                        }
                    }
                    else
                    {
                        using (Stream manifestResourceStream = typeof(EntryPoint).Assembly.GetManifestResourceStream(textx))
                        {
                            byte[] array = new byte[manifestResourceStream.Length];
                            manifestResourceStream.Read(array, 0, array.Length);
                            resultt = Assembly.Load(array);
                        }
                    }
                    return resultt;
                };

                //Force load Newtonsoft.Json to memory
                string text = Array.Find<string>(typeof(Program).Assembly.GetManifestResourceNames(), (string element) => element.EndsWith("Newtonsoft.Json.dll"));
                Assembly result;
                using (Stream manifestResourceStream = typeof(Program).Assembly.GetManifestResourceStream(text))
                {
                    byte[] array = new byte[manifestResourceStream.Length];
                    manifestResourceStream.Read(array, 0, array.Length);
                    result = AppDomain.CurrentDomain.Load(array);
                    //result = Assembly.Load(array); 
                }
                //Newtonsoft.Json.JsonConvert.SerializeObject()
                var s = (string)Reflect.InvokeS(result, "Newtonsoft.Json.JsonConvert", "SerializeObject", new object[] { new List<string>{"Terraria.ModKit", "v0.4"}});
                Console.WriteLine(s);
                //Program.LaunchGame(args);
                Terraria.Main.OnEngineLoad += Entry.Initialise;
                Reflect.Invoke(typeof(WindowsLaunch), "Main", BindingFlags.Static | BindingFlags.NonPublic,
                    args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }
}
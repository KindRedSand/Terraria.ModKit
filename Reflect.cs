using System;
using System.Linq;
using System.Reflection;

namespace Terraria.ModKit
{
    static class Reflect
    {
        public static T Invoke<T>(object target, string methodName, params object[] param)
        {
            return (T)target?.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(target, param);
        }

        public static object Invoke(Type target, string methodName, params object[] param)
        {
            return target.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(null, param);
        }

        public static object InvokeS(Assembly assembly,string typename, string methodName, params object[] param)
        {
            var type = assembly.GetType(typename);
            var method = type.GetMethod(methodName, param.Select((x) => x.GetType()).ToArray());
            return method?.Invoke(null, param);
        }

        public static T Invoke<T>(object target, string methodName, BindingFlags flags= BindingFlags.NonPublic | BindingFlags.Instance, params object[] param)
        {
            return (T)target?.GetType().GetMethod(methodName, flags)?.Invoke(target, param);
        }

        public static object Invoke(Type target, string methodName, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance, params object[] param)
        {
            var method = target.GetMethod(methodName, flags);
            return method?.Invoke(null,new object[]{param});
        }

        public static T GetF<T>(object target, string fieldName)
        {
            return (T)target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(target);
        }

        public static void SetF<T>(object target, string fieldName, T value)
        {
            target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(target, value);
        }

        public static T GetP<T>(object target, string propertyName)
        {
            return (T)target.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(target);
        }

        public static void SetP<T>(object target, string propertyName, T value)
        {
            target.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(target, value);
        }

        //internal static Assembly ApplyPatch(string filePath = "Terraria.exe")
        //{
        //    AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(filePath);
        //    var module = assembly.MainModule;
            
        //    var type = module.GetAllTypes().First(t => t.Name == "LaunchInitializer");
        //    type.Module.ImportReference(typeof(Entry));
        //    var method = type.GetMethods().First(t => t.Name == "LoadParameters");
        //    var processor = method.Body.GetILProcessor();
        //    //var targetMethod = ModuleDefinition.ReadModule(typeof(Entry).Assembly.Location).GetAllTypes().First(t => t.Name == "Entry").GetMethods().First(t => t.Name == "Initialise");
        //    //Entry.Initialise
        //    var targetMethod = typeof(Entry).GetMethod("Initialise", BindingFlags.Static | BindingFlags.Public);
        //    var newInstruction = processor.Create(OpCodes.Call, method.Module.ImportReference(targetMethod));
        //    var firstInstruction = method.Body.Instructions[0];
        //    var ldstr = processor.Create(OpCodes.Ldstr, method.Name);
        //    processor.InsertBefore(firstInstruction, ldstr);
        //    firstInstruction = method.Body.Instructions[1];
        //    processor.InsertAfter(firstInstruction, newInstruction);

        //    //if(File.Exists("Terraria.Creative.exe"))
        //    //    using (FileStream wr = new FileStream("Terraria.Creative.exe", FileMode.Truncate))
        //    //    {
        //    //        assembly.Write(wr);
        //    //    }
        //    //else
        //        using (FileStream wr = new FileStream("Terraria.Creative.exe", FileMode.Create))
        //        {
        //            assembly.Write(wr);
        //        }
        //    return Assembly.Load("Terraria.Creative.exe");
        //}
    }
}

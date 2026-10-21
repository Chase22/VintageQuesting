using System;
using System.Reflection;
using System.Linq;

class DllInspector
{
    static void Main()
    {
        try
        {
            // Set up assembly resolution for Vintage Story dependencies
            var vintageStoryPath = Environment.GetEnvironmentVariable("VINTAGE_STORY");
            if (!string.IsNullOrEmpty(vintageStoryPath))
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    var name = new AssemblyName(args.Name);
                    var path = System.IO.Path.Combine(vintageStoryPath, "Lib", name.Name + ".dll");
                    if (!System.IO.File.Exists(path))
                    {
                        path = System.IO.Path.Combine(vintageStoryPath, name.Name + ".dll");
                    }
                    return System.IO.File.Exists(path) ? Assembly.LoadFrom(path) : null;
                };
            }

            var dllPath = @"C:\Users\Lukas\source\repos\VintageQuesting\VintageQuesting\bin\Debug\Mods\VintageQuesting.dll";
            var asm = Assembly.LoadFrom(dllPath);

            Console.WriteLine($"Assembly: {asm.FullName}");
            Console.WriteLine($"Version: {asm.GetName().Version}");
            Console.WriteLine($"\nPublic Types ({asm.GetExportedTypes().Length}):");
            foreach (var type in asm.GetExportedTypes())
            {
                Console.WriteLine($"  - {type.FullName}");
                if (type.IsClass || type.IsInterface)
                {
                    var members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    foreach (var member in members.Take(5))
                    {
                        Console.WriteLine($"    - {member.MemberType}: {member.Name}");
                    }
                    if (members.Length > 5)
                        Console.WriteLine($"    ... and {members.Length - 5} more");
                }
            }

            Console.WriteLine($"\nAll Types ({asm.GetTypes().Length}):");
            foreach (var type in asm.GetTypes())
            {
                Console.WriteLine($"  - {type.FullName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack: {ex.StackTrace}");
        }
    }
}

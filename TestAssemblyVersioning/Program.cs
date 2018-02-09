using System;
using System.Diagnostics;
using System.Reflection;

namespace TestAssemblyVersioning
{
    class Program
    {
        static void Main(string[] args)
        {
            var ass = Assembly.GetExecutingAssembly(); // AssemblyVersion
            var ver = ass.GetName(); // AssemblyFileVersion
            var fvi = FileVersionInfo.GetVersionInfo(ass.Location); // AssemblyInformationalVersion

            var version = $@"
ass.GetName().Version = AssemblyVersion              = {ver.Version}
fvi.FileVersion       = AssemblyFileVersion          = {fvi.FileVersion}
fvi.ProductVersion    = AssemblyInformationalVersion = { fvi.ProductVersion }
";
            Console.WriteLine(version);
            Debug.WriteLine(version);
        }
    }
}

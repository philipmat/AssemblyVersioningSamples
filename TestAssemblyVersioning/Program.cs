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

            Debug.WriteLine($@"
ass.GetName().Version = AssemblyVersion              = {ver.Version}
fvi.FileVersion       = AssemblyFileVersion          = {fvi.FileVersion}
fvi.ProductVersion    = AssemblyInformationalVersion = { fvi.ProductVersion }
");
        }
    }
}

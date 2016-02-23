using Gtk;
using Projects.main;
using Projects.main.backend;
using System;
using System.IO;
using System.Windows.Forms;
using Application = Gtk.Application;

namespace Projects
{
    internal class Program
    {
        /// <summary>
        /// Main entry-point to the application
        /// </summary>
        /// <param name="args">command-line arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            // find the OS version and store the value
            OS.Version = Environment.OSVersion.Platform.ToString();

#if DEBUG
			//write debug info
			Console.WriteLine($"Projects is running on {OS.Version}\nWindows: {OS.isWindows()}");
#endif

            try
            {
                //initialize GLIB
                Application.Init();

                // if the operating system is windows
                if (OS.isWindows())
                {
                    // and the program is 64 bit..
                    if (Environment.Is64BitProcess)
                    {
                        // set the path to the 64-bit GTK version
                        Environment.SetEnvironmentVariable("PATH",
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\gtk64\bin"));
                    }
                    else // otherwise
                    {
                        // default to the 32 bit version
                        Environment.SetEnvironmentVariable("PATH",
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\gtk32\bin"));
                    }

                    // set the font to the system font and use anti-aliasing
                    var font = System.Drawing.SystemFonts.DefaultFont;
                    Settings.Default.FontName = font.Name + " " + font.SizeInPoints;
                    Settings.Default.XftAntialias = 1;
                    Settings.Default.XftRgba = "rgb";
                }

                // if command line arguments have been passed to the application
                if (args.Length != 0)
                {
                    // set file to the first argument set
                    var file = args[0];

                    // get the extension and store it
                    var extension = Path.GetExtension(file);

                    // when the extension isn't null, and the file exists and uses the projects file extension
                    // and a lock file does not exist...
                    if (extension != null &&
                        ((File.Exists(file) && extension.Equals(".prf")) && !File.Exists(file + ".lk")))
                    {
                        // load the file
                        Console.Write(file);
                        var window = new ProjectWindow(file);
                        window.Show();
                    }
                    else
                    {
                        // when conditions are not met continue to normal startup.
                        Startup();
                    }
                    // Run GLIB application
                    Application.Run();
                }
                else
                {
                    // startup normally if no arguments are set
                    Startup();
                }
            }
            catch (BadImageFormatException)
            {
                if (OS.isWindows())
                {
                    MessageBox.Show((Environment.Is64BitProcess) ? "You are running a 64-bit version of Projects but have a 32-bit version of GTK has been detected." :
                        "You are running a 32-bit version of Projects but a 64-bit version of GTK has been detected.",
                            "Wrong architecture", MessageBoxButtons.OK, MessageBoxIcon.Error); //fall back to winforms
                } else
                {
                    Console.WriteLine((Environment.Is64BitProcess) ? "You are running a 64-bit version of Projects but have a 32-bit version of GTK has been detected." :
                        "You are running a 32-bit version of Projects but a 64-bit version of GTK has been detected.");
                }
            }
            catch (DllNotFoundException ex)
            {
                if (OS.isWindows())
                {
                    if (!ex.Message.EndsWith(".dll"))
                    {
                        MessageBox.Show($"A file required by Projects is missing:\n\n{ex.Message} .",
                            "Missing prerequisite", MessageBoxButtons.OK, MessageBoxIcon.Error); //fall back to winforms
                    }
                    else if (ex.Message.EndsWith(".dll"))
                    {
                        MessageBox.Show($"A DLL file required by Projects is missing: {ex.Message}.",
                            "Missing library", MessageBoxButtons.OK, MessageBoxIcon.Error); //fall back to winforms
                    }
                }
                else
                {
                    Console.WriteLine($"A file required by Projects is missing: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Start up this instance.
        /// </summary>
        private static void Startup()
        {
            // check if the program is supposed to load a file at startup
            if (Properties.Settings.Default.LoadOnStartup)
            {
                // if so, retrieve the full path to the file
                var file = Properties.Settings.Default.FileOnStartup;

                // if the file exists and there is no lock file
                if (File.Exists(file) && !File.Exists(file + ".lk"))
                {
                    //load the application with the file
                    var window = new ProjectWindow(file);
                    window.Show();
                }
                else
                {
                    // if a lock file does not exist
                    if (!File.Exists(file + ".lk"))
                    {
                        // the file can no longer be located so remove the settings
                        Properties.Settings.Default.LoadOnStartup = false;
                        Properties.Settings.Default.FileOnStartup = null;
                        Properties.Settings.Default.Save();
                    }

                    // load application normally
                    var window = new MainWindow();
                    window.Show();
                }
            }
            else
            {
                // load application normally when the settings have not been set
                var window = new MainWindow();
                window.Show();
            }

            // run GLIB application
            Application.Run();
        }
    }
}
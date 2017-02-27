using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Gtk;
using Projects.main;
using Projects.main.backend;
using Application = Gtk.Application;

namespace Projects
{
    /// <summary>
    ///     Main program class.
    /// </summary>
    internal class Program
    {
        // use for single threaded applications (GTK requires this, otherwise the program will crash)  
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                if (!PrjHandler.IsUnix)
                    Environment.SetEnvironmentVariable("PATH",
                        Environment.Is64BitOperatingSystem
                            ? @"C:\msys64\mingw64\bin"
                            : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\gtk32\bin")
                        );

                Application.Init();

                // store system's default font in variable
                var font = SystemFonts.DefaultFont;
                // set the font to the default font
                Settings.Default.FontName = font.Name + " " + font.SizeInPoints;
                // force usage of smooth font
                Settings.Default.XftAntialias = 1;
                Settings.Default.XftRgba = "rgb";

                // if arguments have been passed to the program
                if (args.Length != 0)
                {
                    // store first argument
                    var file = args[0];

                    // get the file's extension 
                    var extension = Path.GetExtension(file);
                    //Console.WriteLine(extension);

                    // if the extension isn't null, the file exists and is of the right extension, and the lock file
                    // does not exist,
                    // load the main application with the argument passed to the program
                    if (extension != null && File.Exists(file) && extension.Equals(".prj") && !File.Exists(file + ".lk"))
                        new ProjectWindow(file).Show();
                    // if the requirements are not met, then proceed with the startup process
                    else
                        Startup();
                    Application.Run();
                }
                // if no arguments have been passed then go to the startup process
                else
                    Startup();
            }
            catch (DllNotFoundException dnfe)
            {
                if (!dnfe.Message.EndsWith(".dll"))
                {
                    // fall back to winforms messagebox if gtk dependency is missing 
                    MessageBox.Show($"A file required by Projects is missing:\n\n{dnfe.Message} .",
                        "Missing prerequisite", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (dnfe.Message.EndsWith(".dll"))
                {
                    MessageBox.Show($"A DLL file required by Projects is missing: {dnfe.Message}.",
                        "Missing library", MessageBoxButtons.OK, MessageBoxIcon.Error); //fall back to winforms
                }
            }
        }

        /// <summary>
        ///     The start-up process logic executed on program launch
        /// </summary>
        private static void Startup()
        {
            // if the application has been set to load an application on start-up 
            if (Properties.Settings.Default.LoadOnStartup)
            {
                // store the location of the file in a variable
                var file = Properties.Settings.Default.FileOnStartup;

                // check to see if the file exists, and a lock file does not exist 
                // load the main application with the startup file
                if (File.Exists(file) && !File.Exists(file + ".lk"))
                    new ProjectWindow(file).Show();
                else
                {
                    // if a lockfile does not exist for the startup program 
                    if (!File.Exists(file + ".lk"))
                    {
                        // reset the the application settings for loading the file on startup
                        // and save the changes made.
                        Properties.Settings.Default.LoadOnStartup = false;
                        Properties.Settings.Default.FileOnStartup = null;
                        Properties.Settings.Default.Save();
                    }

#if DEBUG
                        Console.WriteLine(
                            $"{Properties.Settings.Default.LoadOnStartup} \n {Properties.Settings.Default.FileOnStartup}");
#endif
                    // show the welcome screen.
                    new MainWindow().Show();
                }
            }
            // if there isn't a set file tot load on startup show the welcome screen
            else new MainWindow().Show();

            // run the GTK application
            Application.Run();

            PrjHandler.UnlockFile();
#if DEBUG
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
#endif
        }
    }
}
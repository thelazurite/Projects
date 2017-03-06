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
            // if the user is not running a Unix Based OS - set the enviroment var path accordingly
            if (!PrjHandler.IsUnix)
                Environment.SetEnvironmentVariable("PATH",
                    Environment.Is64BitOperatingSystem
                        ? @"C:\msys64\mingw64\bin"
                        : @"C:\msys32\mingw32\bin"
                    );
            try
            {
                Application.Init();
            }
            catch (DllNotFoundException e)
            {
                var str = $"A file required by Projects is missing:\n\n{e.Message}.";

                if (!PrjHandler.IsUnix)
                    MessageBox.Show(str, "Missing prerequisite", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else Console.WriteLine(str);

                Environment.Exit(0);
            }
            catch (TypeInitializationException e)
            {
                var str = $"An issue occurred while trying to load projects:\n\n{e.Message}\n\nIs Gtk installed?";

                if (!PrjHandler.IsUnix)
                    MessageBox.Show(str, "Check Install", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else Console.WriteLine(str);

                Environment.Exit(0);
            }

            // store system's default font in variable
            var font = SystemFonts.DefaultFont;
            // set the font to the default font
            Gtk.Settings.Default.FontName = font.Name + " " + font.SizeInPoints;
            // force usage of smooth font
            Gtk.Settings.Default.XftAntialias = 1;
            Gtk.Settings.Default.XftRgba = "rgb";

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

        /// <summary>
        ///     The start-up process logic executed on program launch
        /// </summary>
        private static void Startup()
        {
            // if the application has been set to load an application on start-up 
            if (main.backend.Settings.Default.LoadOnStartup)
            {
                // store the location of the file in a variable
                var file = main.backend.Settings.Default.FileOnStartup;

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
                        main.backend.Settings.Default.LoadOnStartup = false;
                        main.backend.Settings.Default.FileOnStartup = null;
                        main.backend.Settings.Default.Save();
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
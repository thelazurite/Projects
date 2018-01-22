
// MIT License
//
// Copyright (c) 2017 Dylan Eddies
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Projects.Gtk.main;
using Projects.Gtk.main.backend;
using Application = Gtk.Application;

namespace Projects.Gtk
{
    /// <summary>
    ///     Main program class.
    /// </summary>
    internal static class Program
    {
        // use for single threaded applications (GTK requires this, otherwise the program will crash)  
        [STAThread]
        public static void Main(String[] args)
        {
            // if the user is not running a Unix Based OS - set the enviroment var path accordingly
            if (!ApplicationHelper.IsUnix)
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

                if (!ApplicationHelper.IsUnix)
                    MessageBox.Show(str, "Missing prerequisite", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else Console.WriteLine(str);

                Environment.Exit(0);
            }
            catch (TypeInitializationException e)
            {
                var str = $"An issue occurred while trying to load projects:\n\n{e.Message}\n\nIs Gtk installed?";

                if (!ApplicationHelper.IsUnix)
                    MessageBox.Show(str, "Check Install", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else Console.WriteLine(str);

                Environment.Exit(0);
            }

            // store system's default font in variable
            Font font = SystemFonts.DefaultFont;
            // set the font to the default font
            global::Gtk.Settings.Default.FontName = font.Name + " " + font.SizeInPoints;
            // force usage of smooth font
            global::Gtk.Settings.Default.XftAntialias = 1;
            global::Gtk.Settings.Default.XftRgba = "rgb";

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
            if (Settings.Default.LoadOnStartup)
            {
                // store the location of the file in a variable
                var file = Settings.Default.FileOnStartup;

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
                        Settings.Default.LoadOnStartup = false;
                        Settings.Default.FileOnStartup = null;
                        Settings.Default.Save();
                    }

#if DEBUG
                        Console.WriteLine(
                            $"{Settings.Default.LoadOnStartup} \n {Settings.Default.FileOnStartup}");
#endif
                    // show the welcome screen.
                    new MainWindow().Show();
                }
            }
            // if there isn't a set file tot load on startup show the welcome screen
            else new MainWindow().Show();

            // run the GTK application
            Application.Run();

            ApplicationHelper.UnlockFile();
#if DEBUG
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
#endif
        }
    }
}
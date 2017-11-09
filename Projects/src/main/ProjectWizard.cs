
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
using System.IO;
using System.Windows.Forms;
using Gtk;
using Projects.Gtk.main.backend;
using Projects.Dal;

namespace Projects.Gtk.main
{
    public partial class ProjectWizard : Window
    {
        /// <summary>
        ///     The project creation wizard.
        /// </summary>
        public ProjectWizard() : base(WindowType.Toplevel)
        {
            // initialize interface and display to user
            BuildInterface();
            //hide the create file item
            _createButton.Visible = false;
            _filepathEntry.Text = backend.Settings.Default.PreviousBrowseFolder;
        }

        private void _selectPathButton_Clicked(Object sender, EventArgs e)
        {
            var path = String.Empty;

            if (!ApplicationHelper.IsUnix)
            {
                // folder browser dialog implements dispose(), so using statement can be used here
                using (var browser = new FolderBrowserDialog())
                {
                    // if the previously browsed folder still exists, set the browser window to it.
                    if (Directory.Exists(backend.Settings.Default.PreviousBrowseFolder))
                        browser.SelectedPath = backend.Settings.Default.PreviousBrowseFolder;
                    // allow users to create new folders from the inteface
                    browser.ShowNewFolderButton = true;

                    // show the dialog
                    browser.ShowDialog();
                    path = browser.SelectedPath;
                }
            }
            else
            {
                using (var browser = new FileChooserDialog("Select Folder", this, FileChooserAction.SelectFolder))
                {
                    browser.AddButton("Open", ResponseType.Ok);
                    browser.AddButton("Cancel", ResponseType.Close);
                    ApplicationHelper.SetCurrentFolder(Directory.Exists(backend.Settings.Default.PreviousBrowseFolder)
                        ? backend.Settings.Default.PreviousBrowseFolder
                        : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), browser.Handle);
                    if (browser.Run() == (Int32)ResponseType.Ok)
                        path = browser.File.ParsedName;
                    browser.Destroy();
                }
            }
            // set the filepath to the select path from the file browser
            _filepathEntry.Text = path;
            // set the previously browsed folder to the selected path and save 
            // property changes.
            backend.Settings.Default.PreviousBrowseFolder = path;
            backend.Settings.Default.Save();
        }

        // validates information input 
        private void _fileEntry_Changed(Object sender, EventArgs e) => CheckValues();
        private void _filepathEntry_Changed(Object sender, EventArgs e) => CheckValues();

        private void CheckValues()
        {
            // if both required values aren't empty - show the create button. 
            if (!String.IsNullOrWhiteSpace(_filepathEntry.Text) && !String.IsNullOrWhiteSpace(_fileEntry.Text))
                _createButton.Visible = true;
            // otherwise, do not
            else
                _createButton.Visible = false;
        }

        /// <summary>
        ///     Triggered once the user presses the create button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _createButton_Clicked(Object sender, EventArgs e)
        {
            var path = _filepathEntry.Text;
            var file = _fileEntry.Text + ".prj";
            var full = System.IO.Path.Combine(path, file);
            var cont = true;
//            Console.WriteLine("Before: " + path + ", " + file);

            if (File.Exists(full))
            {
                // create a dialog asking the user if they wish to overwrite the file
                // implements IDisposable
                using (
                    var overwriteConfirm = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Question,
                        ButtonsType.YesNo,
                        $"File: {_fileEntry.Text} already exists in the desired path.\nDo you want to overwrite it?")
                    )
                {
                    // if the user presses Yes
                    if (overwriteConfirm.Run() == (Int32) ResponseType.Yes)
                    {
                        // check if the lockfile exists and delete the file if it does not
                        if (!File.Exists(full + ".lk"))
                        {
                            try
                            {
                                File.Delete(full);
                                //cont = true;
                            }
                            catch (IOException ioException)
                            {
                                using (
                                    var error = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error,
                                        ButtonsType.Ok, $"IoException: {ioException.Message}"))
                                {
                                    error.Run();
                                    error.Destroy();
                                }
                                cont = false;
                            }
                            overwriteConfirm.Destroy();
                        }
                        // otherwise show an error message saying the file has been locked
                        else
                        {
                            //implements IDisposable
                            using (var error = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error,
                                ButtonsType.Ok, "The file has been locked for editing and cannot be deleted"))
                            {
                                error.Run();
                                error.Destroy();
                                overwriteConfirm.Destroy();
                            }
                            cont = false;
                        }
                    }
                    else
                    {
                        overwriteConfirm.Destroy();
                        cont = false;
                    }
                }
            }
            if (!cont) return;
#if DEBUG
            Console.WriteLine($"LoadOnStart: {_loadOnStartButton.Active}");
#endif
            // if the user has checked the load on startup tick box,
            if (_loadOnStartButton.Active)
            {
                // modify the application properties which are used to determine the file to load
                // at start-up and then save the properties.
                backend.Settings.Default.LoadOnStartup = true;
                backend.Settings.Default.FileOnStartup = full;
                backend.Settings.Default.Save();
#if DEBUG
                Console.WriteLine(
                    $"{backend.Settings.Default.FileOnStartup} \n {backend.Settings.Default.FileOnStartup}");

#endif
            }

            // destroy the wizard window
            Destroy();
            // create the Project window
            new ProjectWindow(path, file).Show();
        }

        private void _cancelButton_Clicked(Object sender, EventArgs e)
        {
            // Destroy the current window
            Destroy();
            // go back to the welcome screen
            new MainWindow().Show();
        }
    }
}
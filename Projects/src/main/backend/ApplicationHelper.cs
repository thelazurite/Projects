
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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GLib;
using Gtk;

namespace Projects.Gtk.main.backend
{
    /// <summary>
    /// Projects File and Operations handling class
    /// </summary>
    public static class ApplicationHelper
    {
        // check the platform version. 
        public static Boolean IsUnix
        {
            get
            {
                var p = (Int32) Environment.OSVersion.Platform;
                return (p == 4 || p == 6 || p == 128);
            }
        }

        public static String PathToFile;
        public static String PathToLock;
        private static StreamWriter _lock;

        [DllImport("libgtk-3-0.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean gtk_file_chooser_set_current_folder(IntPtr raw, IntPtr filename);
        public static Boolean SetCurrentFolder(String filename, IntPtr handle)
        {
            IntPtr num = Marshaller.StringToFilenamePtr(filename);
            var flag = gtk_file_chooser_set_current_folder(handle, num);
            Marshaller.Free(num);
            return flag;
        }

        public static Boolean Open(Widget parent)
        {
            var file = String.Empty;
            if (!IsUnix)
                using (var openDialog = new OpenFileDialog())
                {
                    // check file exists
                    openDialog.CheckFileExists = true;
                    // If the previously browsed directory exists, then direct the user to it, otherwise direct to home directory.
                    openDialog.InitialDirectory = Directory.Exists(Settings.Default.PreviousBrowseFolder)
                        ? Settings.Default.PreviousBrowseFolder
                        : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    // Set the filter to show only 'prf' files
                    openDialog.Filter = "Projects file (*.prj)|*.prj|Lock file (*.prj.lk) |*.prj.lk|All Files (*.*)|*.*";
                    openDialog.FilterIndex = 1;
                    //RestoreDirectory = true,
                    openDialog.ShowReadOnly = false;
                    openDialog.ShowDialog();

                    file = openDialog.FileName;
                    openDialog.Reset();
                }
            else
            {
                using (var openDialog = new FileChooserDialog("Open File", parent as Window, FileChooserAction.Open))
                {
                    openDialog.AddButton("Open", ResponseType.Ok);
                    openDialog.AddButton("Cancel", ResponseType.Close);
                    SetCurrentFolder(Directory.Exists(Settings.Default.PreviousBrowseFolder)
                        ? Settings.Default.PreviousBrowseFolder
                        : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), openDialog.Handle);

                    using (var openFilter = new FileFilter())
                    {
                        openFilter.Name = "Projects File";
                        openFilter.AddMimeType("Projects File");
                        openFilter.AddPattern("*.prj");
                        openDialog.AddFilter(openFilter);
                    }
                    using (var openFilter = new FileFilter())
                    {
                        openFilter.Name = "Projects Lock File";
                        openFilter.AddMimeType("Projects Lock File");
                        openFilter.AddPattern("*.prj.lk");
                        openDialog.AddFilter(openFilter);
                    }
                    using (var openFilter = new FileFilter())
                    {
                        openFilter.Name = "All files";
                        openFilter.AddMimeType("All");
                        openFilter.AddPattern("*.*");
                        openDialog.AddFilter(openFilter);
                    }

                    if (openDialog.Run() == (Int32) ResponseType.Ok)
                        file = openDialog.File.ParsedName;
                    openDialog.Destroy();
                }
            }

            // if no file has been provided / the file does not exist - do not continue
            if (String.IsNullOrEmpty(file) || !File.Exists(file)) return false;
            //Console.WriteLine(Path.GetExtension(file));

            // ensure that the correct file extension is being used
            if (Path.GetExtension(file) != ".prj")
            {
                using (var md = new MessageDialog(parent as Window, DialogFlags.Modal, MessageType.Error,
                    ButtonsType.Close, "The selected file is not recognized by Projects."))
                {
                    md.Run();
                    md.Destroy();
                }
                return false;
            }
            // get the path from the selected file, by finding the last occurrence of a directory separator and save it into 
            // the "PreviousBrowseFolder" Setting
            Settings.Default.PreviousBrowseFolder = file.Substring(0,
                file.LastIndexOf(Path.DirectorySeparatorChar));

            // save settings
            Settings.Default.Save();

            if (File.Exists(file + ".lk"))
            {
                using (
                    var dialog = new MessageDialog(parent as Window, DialogFlags.DestroyWithParent,
                        MessageType.Error,
                        ButtonsType.Ok,
                        $"The file is currently in use. If you're sure that this isn't the case, please delete the following file:\n {file}.lk")
                    )
                {
                    dialog.Run();
                    dialog.Destroy();
                }
                return false;
            }

            var window = new ProjectWindow(file);
            window.Show();
            parent.Destroy();
            return true;

        }

        // create the lock file
        public static void LockFile()
        {
            using (_lock = new StreamWriter(PathToLock)) 
                _lock.Write("[[PROJECTS LOCKFILE]]");
        }

        // remove the lock file
        public static void UnlockFile()
        {
            _lock?.Close();
            if (PathToLock == null) return;
            File.Delete(PathToLock);
        }
    }
}
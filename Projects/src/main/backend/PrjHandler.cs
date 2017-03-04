using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GLib;
using Gtk;
using Settings = Projects.Properties.Settings;

namespace Projects.main.backend
{
    /// <summary>
    /// Projects File and Operations handling class
    /// </summary>
    public static class PrjHandler
    {
        // check the platform version. 
        public static bool IsUnix
        {
            get
            {
                var p = (int) Environment.OSVersion.Platform;
                return (p == 4 || p == 6 || p == 128);
            }
        }

        public static string PathToFile;
        public static string PathToLock;
        private static StreamWriter _lock;

        [DllImport("libgtk-3-0.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool gtk_file_chooser_set_current_folder(IntPtr raw, IntPtr filename);
        public static bool SetCurrentFolder(string filename, IntPtr handle)
        {
            var num = Marshaller.StringToFilenamePtr(filename);
            var flag = gtk_file_chooser_set_current_folder(handle, num);
            Marshaller.Free(num);
            return flag;
        }

        public static bool Open(Widget parent)
        {
            var file = string.Empty;
            if(!IsUnix)
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
                    using (var openFilter = new FileFilter())
                    {
                        openDialog.AddButton("Open", ResponseType.Ok);
                        openDialog.AddButton("Cancel", ResponseType.Close);
                        SetCurrentFolder(Directory.Exists(Settings.Default.PreviousBrowseFolder)
                        ? Settings.Default.PreviousBrowseFolder
                        : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),openDialog.Handle);

                        openFilter.AddMimeType("Projects File");
                        openFilter.AddPattern("*.prj");

                        openDialog.Filter = openFilter;
                        if (openDialog.Run() == (int) ResponseType.Ok)
                            file = openDialog.File.ParsedName;
                        openDialog.Destroy();
                    }
                }
            }

            Console.WriteLine(file);
            // if no file has been provided / the file does not exist - do not continue
            if (string.IsNullOrEmpty(file) || !File.Exists(file)) return false;
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
using System;
using System.IO;
using System.Windows.Forms;
using Gtk;
using Settings = Projects.Properties.Settings;

namespace Projects.main.backend
{
    public static class PrjHandler
    {
        public static string PathToFile;

        public static string PathToLock;

        private static StreamWriter _lock;

        public static bool Open(Widget parent)
        {
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

                var file = openDialog.FileName;

                openDialog.Reset();

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
        }

        public static void LockFile()
        {
            _lock = new StreamWriter(PathToLock);
            _lock.Write("[[PROJECTS LOCKFILE]]");
        }

        public static void UnlockFile()
        {
            _lock?.Close();
            if (PathToLock == null) return;
            File.Delete(PathToLock);
        }
    }
}
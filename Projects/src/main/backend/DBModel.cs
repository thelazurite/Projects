using Gtk;
using System;
using System.IO;
using System.Windows.Forms;

namespace Projects.main.backend
{
    /// <summary>
    /// Operating system information
    /// </summary>
    public struct OS
    {
        public static string Version;

        /// <summary>
        /// Checks if the operating system running the application is Windows.
        /// </summary>
        /// <returns><c>true</c>If windows <c>false</c> otherwise.</returns>
        public static bool isWindows()
        {
            //check if the version name contains these values
            if (Version.Contains("Windows") || Version.Contains("Win32NT"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Opens a file.
    /// </summary>
    public static class OpenFile
    {
        public static string PathToFile;
        public static string PathToLock;

        private static StreamWriter _lock;

        public static void Open(Widget parent)
        {
            string file = string.Empty;

            //check to see if the OS is windows
            if (OS.isWindows())
            {
                // show the file dialog
                var openDialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    Filter = "Projects file (*.prf)|*.prf",
                    FilterIndex = 2,
                    RestoreDirectory = true,
                    ShowReadOnly = false
                };

                openDialog.ShowDialog();

                file = openDialog.FileName;
            }
            else
            {
                // use the gtk file dialog
                var openDialog = new FileChooserDialog(
                                     "Open", parent as Window, FileChooserAction.Open,
                                     "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept
                                 );
                // filter down to prf files
                openDialog.Filter = new FileFilter();
                openDialog.Filter.AddPattern("*.prf");

                // set the file path
                if (openDialog.Run() == (int)ResponseType.Accept)
                    file = openDialog.File.Path;
                // destroy the dialog
                openDialog.Destroy();
            }
            Console.WriteLine(file);

            // if the file doesnt exist - do not continue
            if (!File.Exists(file)) return;

            // if a lock file exists
            if (File.Exists(file + ".lk"))
            {
                //show an error and do not continue
                var dialog = new MessageDialog(parent as Window, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok,
                    $"The file is currently in use. If you're sure that this isn't the case, please delete the lock file:\n {file}.lk");
                dialog.Run();
                dialog.Destroy();
                return;
            }

            // load the file
            var window = new ProjectWindow(file);
            window.Show();
            parent.Destroy();
        }

        // creates a the lock file
        public static void LockFile()
        {
            _lock = new StreamWriter(PathToLock);
        }

        // removes the lock file created
        public static void UnlockFile()
        {
            _lock.Close();
            File.Delete(PathToLock);
        }
    }

    // Model classes
    public class ToDoItem
    {
        public ToDoItem(string id, string name, string description, string category, string priority, DateTime start, DateTime end)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Priority = priority;
            Start = start;
            Finish = end;
        }

        public string Id;
        public string Name;
        public string Description;
        public string Category;
        public string Priority;
        public DateTime Start;
        public DateTime Finish;
    }

    public class Category
    {
        public Category(string id, string name, string description, bool active)
        {
            Id = id;
            CategoryName = name;
            CategoryDescription = description;
            CategoryActive = active;
        }

        public string Id;
        public string CategoryName;
        public string CategoryDescription;
        public bool CategoryActive;
    }
}
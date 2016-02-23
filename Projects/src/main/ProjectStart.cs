using Gtk;
using Projects.main.backend;
using System;
using System.IO;
using System.Windows.Forms;

namespace Projects.main
{
    public partial class ProjectStart : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Projects.main.ProjectStart"/> class.
        /// </summary>
        public ProjectStart() : base(WindowType.Toplevel)
        {
            BuildInterface();
            _createButton.Visible = false;
        }

        /// <summary>
        /// Selects the path for the file to be stored
        /// </summary>
        private void _selectPathButton_Clicked(object sender, EventArgs e)
        {
            // check if running windows
            if (OS.isWindows())
            {
                // show the folder browser
                var browser = new FolderBrowserDialog { ShowNewFolderButton = true };
                browser.ShowDialog();
                _filepathEntry.Text = browser.SelectedPath;
            }
            else
            {
                // show the gtk file chooser dialog
                var openDialog = new FileChooserDialog(
                    "Open", this, FileChooserAction.SelectFolder,
                    "Cancel", ResponseType.Cancel, "Select", ResponseType.Accept
                );

                if (openDialog.Run() == (int)ResponseType.Accept)
                    _filepathEntry.Text = openDialog.File.Path;
                openDialog.Destroy();
            }
        }

        private void _fileEntry_Changed(object sender, EventArgs e)
        {
            CheckValues();
        }

        private void _filepathEntry_Changed(object sender, EventArgs e)
        {
            CheckValues();
        }

        private void CheckValues()
        {
            if (!string.IsNullOrWhiteSpace(_filepathEntry.Text) && !string.IsNullOrWhiteSpace(_fileEntry.Text))
                _createButton.Visible = true;
            else
                _createButton.Visible = false;
        }

        private void _createButton_Clicked(object sender, EventArgs e)
        {
            // combine the path and file
            var path = _filepathEntry.Text;
            var file = _fileEntry.Text + ".prf";
            var full = System.IO.Path.Combine(path, file);

            // make sure a file does not exist with the same name already
            if (File.Exists(full))
            {
                var error = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close,
                    "File already exists!");
                error.Run();
                error.Destroy();
                return;
            }

#if DEBUG
            Console.WriteLine($"LoadOnStart: {_loadOnStartButton.Active}");
#endif

            // if the user checks the load on startup button save the values to the properties.
            if (_loadOnStartButton.Active)
            {
                Properties.Settings.Default.LoadOnStartup = true;
                Properties.Settings.Default.FileOnStartup = full;
                Properties.Settings.Default.Save();

#if DEBUG
                Console.WriteLine(
                    $"{Properties.Settings.Default.FileOnStartup} \n {Properties.Settings.Default.FileOnStartup}");

#endif
            }

            //show the project window and destroy this window
            var window = new ProjectWindow(path, file);
            window.Show();
            Destroy();
        }

        private void _cancelButton_Clicked(object sender, EventArgs e)
        {
            // destroy this window and open the main window
            Destroy();
            var window = new MainWindow();
            window.Show();
        }
    }
}
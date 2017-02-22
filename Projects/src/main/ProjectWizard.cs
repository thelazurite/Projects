using System;
using System.IO;
using System.Windows.Forms;
using Gtk;

namespace Projects.main
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

            _filepathEntry.Text = Properties.Settings.Default.PreviousBrowseFolder;
        }

        private void _selectPathButton_Clicked(object sender, EventArgs e)
        {
            // folder browser dialog implements dispose(), so using statement can be used here
            using (var browser = new FolderBrowserDialog())
            {
                // if the previously browsed folder still exists, set the browser window to it.
                if (Directory.Exists(Properties.Settings.Default.PreviousBrowseFolder))
                    browser.SelectedPath = Properties.Settings.Default.PreviousBrowseFolder;
                // allow users to create new folders from the inteface
                browser.ShowNewFolderButton = true;

                // show the dialog
                browser.ShowDialog();

                // set the filepath to the select path from the file browser
                _filepathEntry.Text = browser.SelectedPath;

                // set the previously browsed folder to the selected path and save 
                // property changes.
                Properties.Settings.Default.PreviousBrowseFolder = _filepathEntry.Text;
                Properties.Settings.Default.Save();
            }
        }

        // validates information input 
        private void _fileEntry_Changed(object sender, EventArgs e) => CheckValues();
        private void _filepathEntry_Changed(object sender, EventArgs e) => CheckValues();

        private void CheckValues()
        {
            // if both required values aren't empty - show the create button. 
            if (!string.IsNullOrWhiteSpace(_filepathEntry.Text) && !string.IsNullOrWhiteSpace(_fileEntry.Text))
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
        private void _createButton_Clicked(object sender, EventArgs e)
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
                    if (overwriteConfirm.Run() == (int) ResponseType.Yes)
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
                Properties.Settings.Default.LoadOnStartup = true;
                Properties.Settings.Default.FileOnStartup = full;
                Properties.Settings.Default.Save();
#if DEBUG
                Console.WriteLine(
                    $"{Properties.Settings.Default.FileOnStartup} \n {Properties.Settings.Default.FileOnStartup}");

#endif
            }

            // destroy the wizard window
            Destroy();
            // create the Project window
            new ProjectWindow(path, file).Show();
        }

        private void _cancelButton_Clicked(object sender, EventArgs e)
        {
            // Destroy the current window
            Destroy();
            // go back to the welcome screen
            new MainWindow().Show();
        }
    }
}
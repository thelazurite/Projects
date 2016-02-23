using Gtk;
using Projects.main.backend;
using System;

namespace Projects.main
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Projects.main.MainWindow"/> class.
        /// </summary>
        public MainWindow() : base(WindowType.Toplevel)
        {
            // creates the interface
            BuildInterface();
        }

        /// <summary>
        /// When the open button is clicked
        /// </summary>
        private void OpenButton_Clicked(object sender, EventArgs e)
        {
            // OpenFile command (projects.main.backend).
            // this windows is the parent
            OpenFile.Open(this);
        }

        /// <summary>
        /// When the "new" button is clicked
        /// </summary>
        private void NewButton_Clicked(object sender, EventArgs e)
        {
            // opens the project creation wizard and destroys the main window
            var window = new ProjectStart();
            window.Show();

            Destroy();
        }
    }
}
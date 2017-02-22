using System;
using Gtk;
using Projects.main.backend;

namespace Projects.main
{
    public partial class MainWindow : Window
    {
        public MainWindow() : base(WindowType.Toplevel)
        {
            BuildInterface();
        }

        // Display the Open file interface, declaring this window as the parent
        private void OpenButton_Clicked(object sender, EventArgs e) => PrjHandler.Open(this);

        private void NewButton_Clicked(object sender, EventArgs e)
        {
            // open project creation wizard
            new ProjectWizard().Show();
            Destroy();
        }
    }
}
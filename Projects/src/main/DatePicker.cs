using System;
using Gtk;

namespace Projects.main
{
    public partial class DatePicker : Window
    {
        public DatePicker() : base(WindowType.Popup)
        {
            BuildInterface();
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Destroy();
        }
    }
}
using Gtk;

namespace Projects.Gtk.main
{
    public static class DialogHelper
    {
        public static void DisplayError(string message, Window parent)
        {
            using (var md = new MessageDialog(parent, DialogFlags.Modal, MessageType.Error,
                ButtonsType.Close, message))
            {
                md.Run();
                md.Destroy();
            }
        }
    }
}
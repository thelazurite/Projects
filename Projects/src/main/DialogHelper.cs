using Gtk;

namespace Projects.Gtk.main
{
    public sealed partial class TaskItemTab
    {
        private void DisplayError(string message)
        {
            using (var md = new MessageDialog(_parent as global::Gtk.Window, DialogFlags.Modal, MessageType.Error,
                ButtonsType.Close, message))
            {
                md.Run();
                md.Destroy();
            }
        }
    }
}
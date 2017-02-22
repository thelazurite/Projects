using System;
using GLib;
using Gtk;
using Projects.main.backend;
using Window = Gdk.Window;

namespace Projects.main
{
    public sealed partial class CategoryTab : VBox
    {
        private readonly IWrapper _parent;

        /// <summary>
        ///     Create a category Tab
        /// </summary>
        /// <param name="parent">reference to the parent window</param>
        public CategoryTab(IWrapper parent)
        {
            _parent = parent;
            ParentWindow = _parent as Window;
            BuildInterface();
        }

        public event Action<object, EventArgs> AddCategoryHandler;

        private void CancelButton_Clicked(object sender, EventArgs e) => Destroy();

        /// <summary>
        ///     logic executed once the user attempts to add a category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_nameEntry.Text))
            {
                using (var md = new MessageDialog(_parent as Gtk.Window, DialogFlags.Modal, MessageType.Error,
                    ButtonsType.Close, "No category name entered!"))
                {
                    md.Run();
                    md.Destroy();
                    return;
                }
            }

            var category = new Category(Guid.NewGuid().ToString(), _nameEntry.Text, _descView.Buffer.Text, true);
            AddCategory(category, EventArgs.Empty);
            Destroy();
        }

        /// <summary>
        ///     Invoke "AddCategory" event handler, as long as it isn't null
        /// </summary>
        /// <param name="sender">provides the category object back to the handler</param>
        /// <param name="e">empty</param>
        public void AddCategory(object sender, EventArgs e) => AddCategoryHandler?.Invoke(sender, e);
    }
}
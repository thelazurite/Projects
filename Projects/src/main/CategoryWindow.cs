using Gtk;
using Projects.main.backend;
using System;

namespace Projects.main
{
    public sealed partial class CategoryWindow : Window
    {
        public event Action<Category, EventArgs> AddCategoryHandler;

        public CategoryWindow() : base(WindowType.Toplevel)
        {
            BuildInterface();
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Destroy();
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_nameEntry.Text))
            {
                KeepAbove = false;

                var md = new MessageDialog(this, DialogFlags.Modal, MessageType.Error,
                    ButtonsType.Close, "No category name entered!");
                md.Run();
                md.Destroy();
                KeepAbove = true;
                return;
            }

            var category = new Category(Guid.NewGuid().ToString(), _nameEntry.Text, _descView.Buffer.Text, true);
            AddCategory(category, EventArgs.Empty);
            Destroy();
        }

        public void AddCategory(Category sender, EventArgs e)
        {
            AddCategoryHandler?.Invoke(sender, e);
        }
    }
}
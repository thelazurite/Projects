using System;
using Gtk;
using Projects.main.backend;

namespace Projects.main
{
    public sealed partial class CategoryWindow : Window
    {
        public event Action<object, EventArgs> AddCategoryHandler;

        public CategoryWindow() : base(WindowType.Toplevel)
        {
            BuildInterface();
        }
        
        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Destroy();
        }

        /// <summary>
        /// logic executed once the user attempts to add a category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Invoke "AddCategory" event handler, as long as it isn't null  
        /// </summary>
        /// <param name="sender">provides the category object back to the handler</param>
        /// <param name="e">empty</param>
        public void AddCategory(object sender, EventArgs e) => AddCategoryHandler?.Invoke(sender, e);
    }
}

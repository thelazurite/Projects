using System;
using System.Globalization;
using Gdk;
using GLib;
using Gtk;
using Projects.main.backend;
using DateTime = System.DateTime;

namespace Projects.main
{
    public sealed partial class TaskTab : VBox
    {
        public event Action<object, EventArgs> AddTaskHandler;
        private readonly ListStore _categories;
        private DatePicker _window;
        private DateTime _start;
        private DateTime _end;
        private IWrapper _parent;

        /// <summary>
        /// Constructor for the Task Creation tab
        /// </summary>
        /// <param name="categories">List to be provided to the window</param>
        /// <param name="parent">The parent Widget/Window</param>
        public TaskTab(ListStore categories, IWrapper parent)
        {
            _parent = parent;
            ParentWindow = _parent as Gdk.Window;
            // set the category list to the currently available categories
            _categories = categories;
            // builds the interface and displays it
            BuildInterface();
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            // destroy/close the window
            Destroy();
        }

        /// <summary>
        /// Logic executed once the add button is pressed
        /// </summary>
        private void AddButton_Clicked(object sender, EventArgs e)
        {

            // validate the data being provided 

            // if there has been no entry for a to-do item's name provided
            if (string.IsNullOrWhiteSpace(_nameEntry.Text))
            {
                //display an error message to the user
                using (var md = new MessageDialog(_parent as Gtk.Window, DialogFlags.Modal, MessageType.Error,
                    ButtonsType.Close, "No name entered!"))
                {
                    // display the message box
                    md.Run();
                    // wait for user input before destroying/closing the message box.
                    md.Destroy();
                }
                // stop the function
                return;
            }

            // logic run after data provided has been validated

            // get active item from list
            TreeIter iter;
            _categoryBox.GetActiveIter(out iter);
            var item = _categoryBox.Model.GetValue(iter, 0);

            //convert object to that of the category class
            var category = item as Category;
            
            // get the currently seleted priority item
            var priority = _priorityBox.Active;

            // createes the to-do Item
            var todo = new Task
            (
                // create a new guid and convert it to a string
                Guid.NewGuid().ToString(),
                // get the text stored in the name entry field
                _nameEntry.Text, 
                _descView.Buffer.Text,
                // if the currently selected category id isn't null, select it - otherwise use an empty GUID value
                category != null ? category.Id : Guid.Empty.ToString(),
                // if the value provided is out of range, then select the last available item (No Priority)
                _values[priority == -1 ? 3 : priority],
                // start and end dates
                _start, _end
            );
            AddTodo(todo, EventArgs.Empty);

            //Console.WriteLine(_categoryBox.Active);
            Destroy();
        }

        private void StartPicker_Clicked(object sender, EventArgs e)
        {
            _window = new DatePicker();
            _window.AcceptButton.Clicked += StartPicker_Selected;
            _window.Show();
        }

        private void StartPicker_Selected(object sender, EventArgs e)
        {
            var sent = _window.Calendar.Date;
            _start = new DateTime(sent.Year, sent.Month, sent.Day, _window.HoursSpin.ValueAsInt,
                _window.MinutesSpin.ValueAsInt, _window.SecondsSpin.ValueAsInt);

            _startEntry.Text = _start.ToString(CultureInfo.CurrentCulture);
            _window.Destroy();
        }

        private void EndPicker_Clicked(object sender, EventArgs e)
        {
            _window = new DatePicker();
            _window.AcceptButton.Clicked += EndPicker_Selected;
            _window.Show();
        }

        private void EndPicker_Selected(object sender, EventArgs e)
        {
            var sent = _window.Calendar.Date;
            _end = new DateTime(sent.Year, sent.Month, sent.Day, _window.HoursSpin.ValueAsInt,
                _window.MinutesSpin.ValueAsInt, _window.SecondsSpin.ValueAsInt);

            _endEntry.Text = _end.ToString(CultureInfo.CurrentCulture);
            _window.Destroy();
        }

        public void AddTodo(object sender, EventArgs e) => AddTaskHandler?.Invoke(sender, e);

        private static void Func(ICellLayout cellLayout, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
                var item = (Category)model.GetValue(iter, 0);
                var value = cell as CellRendererText;
                if (value != null) value.Text = item.CategoryName;
        }
    }
}

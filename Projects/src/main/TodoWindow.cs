using Gtk;
using Projects.main.backend;
using System;
using System.Globalization;
using Window = Gtk.Window;
using WindowType = Gtk.WindowType;

namespace Projects.main
{
    public sealed partial class TodoWindow : Window
    {
        public event Action<ToDoItem, EventArgs> AddTodoHandler;

        private readonly ListStore _categories;
        private DatePicker _window;
        private DateTime _start;
        private DateTime _end;

        public TodoWindow(ListStore categories) : base(WindowType.Toplevel)
        {
            _categories = categories;
            BuildInterface();
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Destroy();
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            // checks to see if a name is entered
            if (string.IsNullOrWhiteSpace(_nameEntry.Text))
            {
                KeepAbove = false;

                var md = new MessageDialog(this, DialogFlags.Modal, MessageType.Error,
                    ButtonsType.Close, "No name entered!");
                md.Run();
                md.Destroy();
                KeepAbove = true;
                return;
            }

            // gets the selected item from the drop-down
            TreeIter iter;
            _categoryBox.GetActiveIter(out iter);
            var item = _categoryBox.Model.GetValue(iter, 0);
            var category = item as Category;

            // gets the active priority
            var val = _priorityBox.Active;

            // gets the values selected and error checks the category and priority values
            var todo = new ToDoItem(Guid.NewGuid().ToString(), _nameEntry.Text, _descView.Buffer.Text,
                (category != null) ? category.Id : Guid.Empty.ToString(),
                _values[(val == -1) ? 3 : val], _start, _end);

            // starts the custom event
            AddTodo(todo, EventArgs.Empty);

            // destroys the window
            Destroy();
        }

        private void StartPicker_Clicked(object sender, EventArgs e)
        {
            // opens the date picker
            _window = new DatePicker();
            _window.AcceptButton.Clicked += StartPicker_Selected;
            _window.Show();
        }

        private void StartPicker_Selected(object sender, EventArgs e)
        {
            // creates the datetime based on what values were selected in the datepicker
            var sent = _window.Calendar.Date;
            _start = new DateTime(sent.Year, sent.Month, sent.Day, _window.HoursSpin.ValueAsInt,
                _window.MinutesSpin.ValueAsInt, _window.SecondsSpin.ValueAsInt);

            //readable value is converted to a string based on the current culture of the client
            _startEntry.Text = _start.ToString(CultureInfo.CurrentCulture);

            //destroys the datepicker
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

        public void AddTodo(ToDoItem sender, EventArgs e)
        {
            //calls on the event - if it exists
            AddTodoHandler?.Invoke(sender, e);
        }

        private static void Func(ICellLayout cellLayout, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                var item = (Category)model.GetValue(iter, 0);
                var value = cell as CellRendererText;
                if (value != null) value.Text = item.CategoryName;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Null reference has occurred with category object name");
            }
        }
    }
}
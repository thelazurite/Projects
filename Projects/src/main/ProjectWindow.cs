using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using GLib;
using Gtk;
using Projects.main.backend;
using static Projects.main.backend.PrjHandler;
using Application = Gtk.Application;
using DateTime = System.DateTime;

namespace Projects.main
{
    public sealed partial class ProjectWindow : Window
    {
        private readonly List<DbItem> _sqlItems;
        private SQLiteConnection _dbConnection;


        /// <summary>
        ///     Constructor used when creating a new projects file
        /// </summary>
        /// <param name="pathToFile">Where the file will be stored.</param>
        /// <param name="fileName">The file name</param>
        public ProjectWindow(string pathToFile, string fileName) : base(WindowType.Toplevel)
        {
            BuildInterface();

            PathToFile = System.IO.Path.Combine(pathToFile, fileName);
            PathToLock = PathToFile + ".lk";

            LockFile();

            _sqlItems = new List<DbItem>();

            if (!File.Exists(PathToFile))
            {
                SQLiteConnection.CreateFile(PathToFile);
                _dbConnection = new SQLiteConnection($"Data Source={PathToFile};Version=3;");
                _dbConnection.Open();

                // creates the category table
                var execute =
                    new SQLiteCommand(
                        "create table tblCategories (categoryId varchar(36), category varchar(150), description varchar(1024))",
                        _dbConnection);
                execute.ExecuteNonQuery();

                // creates a unique identifier on the categoryId varchar(36) column (used to store a GUID)
                execute = new SQLiteCommand("create unique index catUniqueId on tblCategories (categoryId)",
                    _dbConnection);

                execute.ExecuteNonQuery();

                // creats the to-do item table
                execute = new SQLiteCommand
                    ("create table tblTodoItems(todoId varchar(36), todo varchar(150), description varchar(1024)," +
                     " itemPriority varchar(50), category varchar(50), startDate datetime, dueDate datetime)",
                        _dbConnection);

                execute.ExecuteNonQuery();

                // creates unique identifier on the todoId varchar(36) column
                execute = new SQLiteCommand("create unique index todoUniqueId on tblTodoItems (todoId)", _dbConnection);

                execute.ExecuteNonQuery();

                // creates the default category
                execute = new SQLiteCommand("insert into tblCategories(categoryId, category, description)" +
                                            "values('00000000-0000-0000-0000-000000000000','None','For list items with no category')",
                    _dbConnection);
                execute.ExecuteNonQuery();

                _dbConnection.Close();

                RefreshData();
            }
            else
            {
                Destroy();
                var window = new ProjectWindow(PathToFile);
                window.Show();
            }
            Title = "Projects - " + PathToFile;
        }

        public ProjectWindow(string fullPath) : base(WindowType.Toplevel)
        {
            PathToFile = fullPath;
            PathToLock = PathToFile + ".lk";

            LockFile();

            BuildInterface();
            Title = "Projects - " + PathToFile;

            _dbConnection = new SQLiteConnection("Data Source=" + PathToFile + ";Version=3;");
            RefreshData();
            _sqlItems = new List<DbItem>();
        }

        private bool ChangesMade => _sqlItems?.Count > 0;

        private void UpdateCalendarMarks()
        {
            Calendar.ClearMarks();


            TaskStore.Foreach(delegate(ITreeModel taskModel, TreePath path, TreeIter taskIter)
            {
                var taskItem = (Task) taskModel.GetValue(taskIter, 0);

                if ((Calendar.Month == taskItem.DueDate.Month - 1) && (Calendar.Year == taskItem.DueDate.Year))
                    Calendar.MarkDay((uint) taskItem.DueDate.Day);

                return false;
            });
        }

        private void RefreshData()
        {
            // clear data before refreshing
            CategoryStore.Clear();
            TaskStore.Clear();

            // open database connection
            _dbConnection.Open();

            // select all information from both tables
            const string selectCategories = "select * from tblCategories";
            const string selectRecords = "select * from tblTodoItems";

            // set query to selct categories from the data context
            var execute = new SQLiteCommand(selectCategories, _dbConnection);
            var reader = execute.ExecuteReader();

            // while reading through each item, check for nulls and append to the list store
            while (reader.Read())
            {
                var category = new Category(reader["categoryId"]?.ToString() ?? "null",
                    DbItem.RestoreSpecialChars(reader["category"]?.ToString()) ?? "null",
                    DbItem.RestoreSpecialChars(reader["description"]?.ToString()) ?? "null", true);

                CategoryStore.AppendValues(category);
            }

            // next get the to-do items.
            execute = new SQLiteCommand(selectRecords, _dbConnection);
            reader = execute.ExecuteReader();

            while (reader.Read())
            {
                // display date information in the current localisation 
                DateTime start;
                DateTime.TryParse(reader["startDate"].ToString(), CultureInfo.CurrentCulture,
                    DateTimeStyles.AssumeLocal, out start);

                DateTime end;
                DateTime.TryParse(reader["dueDate"].ToString(), CultureInfo.CurrentCulture,
                    DateTimeStyles.AssumeLocal, out end);

                var item = new Task(reader["todoId"]?.ToString() ?? "null",
                    DbItem.RestoreSpecialChars(reader["todo"]?.ToString()) ?? "null",
                    DbItem.RestoreSpecialChars(reader["description"]?.ToString()) ?? "null",
                    reader["category"]?.ToString() ?? "null", reader["itemPriority"]?.ToString() ?? "null", start, end);

                TaskStore.AppendValues(item);
            }

            // close the connection
            _dbConnection.Close();

            // Update calendar marks
            UpdateCalendarMarks();
        }

        private void OpenActionOnActivated(object sender, EventArgs eventArgs)
        {
            if (ChangesMade)
                switch (ConfirmClose())
                {
                    case 1:
                        SaveFile();
                        if (Open(this))
                            UnlockFile();
                        break;
                    case 2:
                        if (Open(this))
                            UnlockFile();
                        break;
                    default:
                        return;
                }
            else if (Open(this))
                UnlockFile();
        }


        private void FileNewMenuItem_OnActivated(object sender, EventArgs eventArgs)
        {
            _dbConnection.Close();
            _dbConnection = new SQLiteConnection();
            UnlockFile();
            Destroy();
            new ProjectWizard().Show();
        }

        private void AddCategory_Clicked(object sender, EventArgs e)
        {
            var window = new CategoryTab(this);
            window.Destroyed += CategoryWindow_Destroyed;
            window.AddCategoryHandler += Window_AddCategoryHandler;
            var win = _noteBook.AppendPage(window, new Label("New Category"));
            _noteBook.CurrentPage = win;
            var windowNoteChild = (Notebook.NotebookChild) _noteBook[window];
            windowNoteChild.Detachable = false;
            windowNoteChild.TabFill = false;
        }

        private void Window_AddCategoryHandler(object sender, EventArgs e)
        {
            var category = sender as Category;
            if (category == null) return;
            category.AddToDb();
            CategoryStore.AppendValues(category);
            _sqlItems.Add(category);
        }

        private void CategoryWindow_Destroyed(object sender, EventArgs e)
        {
            _addCategoryAction.UnblockActivate();
            _removeCategoryAction.UnblockActivate();
        }

        private void AddTaskItem_Clicked(object sender, EventArgs args)
        {
            var window = new TaskTab(CategoryStore, this);
            window.Destroyed += TaskWindow_Destroyed;
            window.AddTaskHandler += Window_AddTaskHandler;
            var win = _noteBook.AppendPage(window, new Label("New Task"));
            _noteBook.CurrentPage = win;
            var windowNoteChild = (Notebook.NotebookChild) _noteBook[window];
            windowNoteChild.Detachable = false;
            windowNoteChild.TabFill = false;
        }

        /// <summary>
        ///     Handles the addition of task items.
        /// </summary>
        /// <param name="sender">The task item object</param>
        /// <param name="e">empty.</param>
        private void Window_AddTaskHandler(object sender, EventArgs e)
        {
            var task = sender as Task;
            if (task == null) return;
            task.AddToDb();
            TaskStore.AppendValues(task);
            _sqlItems.Add(task);
            UpdateCalendarMarks();
        }

        private void TaskWindow_Destroyed(object sender, EventArgs e)
        {
            _addTaskItemAction.UnblockActivate();
            _removeTaskItemAction.UnblockActivate();
        }

        private void SaveItem_OnActivated(object sender, EventArgs eventArgs) => SaveFile();

        private void SaveFile()
        {
            _fileActionProgBar.Visible = true;
            var amt = _sqlItems.Count;
            var current = 0;
            _dbConnection.Open();

            foreach (var item in _sqlItems.ToList())
            {
                _fileActionProgBar.Window.ProcessUpdates(true);

                while (Application.EventsPending())
                    Application.RunIteration(true);

                current++;
                Console.WriteLine(_fileActionProgBar.Fraction);

                SQLiteCommand sql = null;

                if (item.IsAdding)
                    sql = item.SqlAdd;
                else if (item.IsDeleting)
                    sql = item.SqlDelete;
                else if (item.IsModifying)
                    sql = item.SqlModify;

                if (sql != null)
                {
                    sql.Connection = _dbConnection;
                    sql.ExecuteNonQuery();
                }

                _sqlItems.Remove(item);
                _fileActionProgBar.Fraction = (float) current/amt;
            }

            _fileActionProgBar.Fraction = 1;
            _fileActionProgBar.Visible = false;
            _fileActionProgBar.Fraction = 0;

            _dbConnection.Close();
            RefreshData();
        }

        public void categoryItemToggleCell_Toggled(object sender, ToggledArgs args)
        {
            TreeIter iter;
            if (!CategoryStore.GetIter(out iter, new TreePath(args.Path))) return;
            var toggle = (bool) CategoryStore.GetValue(iter, 3);
            CategoryStore.SetValue(iter, 3, !toggle);
        }

        private bool CheckList(DbItem item)
        {
            var cnt = true;

            // if the item wanting to be added into the pending list is already in the list:
            //  check to see if they are going to be added and deleted or modified and deleted :
            //      if so remove then from the pending list.
            foreach (
                var ite in
                    _sqlItems.ToList().Where(i => i.Id == item.Id)
                        .Where(ite => item.IsAdding && item.IsDeleting || item.IsModifying && item.IsDeleting))
            {
                _sqlItems.Remove(ite);
                cnt = false;
            }

            // return the result
            return cnt;
        }

        public void deleteCategory_Clicked(object sender, EventArgs e)
        {
            ITreeModel model;
            TreeIter iter;
            var selection = _categoryTreeView.Selection;

            if (!selection.GetSelected(out model, out iter)) return;

            var item = model.GetValue(iter, 0);

            var category = item as Category;

            // ensure category isn't null before continuing.
            if (category == null) return;
            if (new Guid(category.Id) == Guid.Empty) return;

            var del = 0;

            // check through Task list if there are any tasks which fall under the category
            TaskStore.Foreach(delegate(ITreeModel taskModel, TreePath path, TreeIter taskIter)
            {
                var taskItem = (Task) taskModel.GetValue(taskIter, 0);
#if DEBUG
                Console.WriteLine(taskItem.Category);
                Console.WriteLine(category.Id);
#endif
                // Check Category id is matching an id used by a task item.
                if (taskItem.Category == category.Id)
                    ++del;
                return false;
            });

            // if there any tasks which meet the above criteria, stop user from deleting it
            if (del > 0)
            {
                using (var md = new MessageDialog(this, DialogFlags.Modal, MessageType.Error,
                    ButtonsType.Close,
                    $"Cannot delete category '{category.CategoryName}'.\nThere are task items which use it."))
                {
                    md.Run();
                    md.Destroy();
                }
                return;
            }

            category.DeleteFromDb();

            if (CheckList(category))
                _sqlItems.Add(category);

            CategoryStore.Remove(ref iter);
        }

        private void DeleteTask_Clicked(object sender, EventArgs eventArgs)
        {
            ITreeModel model;
            TreeIter iter;
            var selection = _mainView.Selection;

            if (!selection.GetSelected(out model, out iter)) return;

            var item = model.GetValue(iter, 0);

            var record = item as Task;

            if (record == null) return;

            record.DeleteFromDb();

            if (CheckList(record))
                _sqlItems.Add(record);

            TaskStore.Remove(ref iter);
            UpdateCalendarMarks();
        }

        private void fileExitMenuItem_Clicked(object sender, EventArgs args)
        {
            if (ChangesMade)
                switch (ConfirmClose())
                {
                    case 1:
                        SaveFile();
                        UnlockFile();
                        Application.Quit();
                        break;
                    case 2:
                        UnlockFile();
                        Application.Quit();
                        break;
                    default:
                        return;
                }
            else
            {
                UnlockFile();
                Application.Quit();
            }
        }

        private void categoryItemNameCell_Edited(object sender, EditedArgs args)
        {
            TreeIter iter;
            CategoryStore.GetIter(out iter, new TreePath(args.Path));
            CategoryStore.SetValue(iter, 1, args.NewText);
        }

        private void CategoryItem_Toggled(object sender, ToggledArgs args)
        {
            TreeIter iter;
            CategoryStore.GetIter(out iter, new TreePath(args.Path));
            Console.WriteLine(iter);

            var item = (Category) CategoryStore.GetValue(iter, 0);
            item.CategoryActive = !item.CategoryActive;
        }

        private static void RenderCategoryId(TreeViewColumn treeColumn, CellRenderer cell, ITreeModel model,
            TreeIter iter)
        {
            var item = (Category) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value != null) value.Text = item.Id;
        }

        private static void RenderCategoryName(TreeViewColumn treeColumn, CellRenderer cell, ITreeModel model,
            TreeIter iter)
        {
            var item = (Category) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value != null) value.Text = item.CategoryName;
        }

        private static void RenderCategoryToggle(TreeViewColumn treeColumn, CellRenderer cell, ITreeModel model,
            TreeIter iter)
        {
            var item = (Category) model.GetValue(iter, 0);
            var value = cell as CellRendererToggle;
            if (value != null) value.Active = item.CategoryActive;
        }

        private static void RenderTaskItemId(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            var item = (Task) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value != null) value.Text = item.Id;
        }

        private static void RenderTaskItemName(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            var item = (Task) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value != null) value.Text = item.Name;
        }

        private void RenderTaskItemCategory(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            var item = (Task) model.GetValue(iter, 0);
            var value = cell as CellRendererText;

            if (value == null) return;
            if (new Guid(item.Category) == Guid.Empty)
                value.Text = "None";
            else
                CategoryStore.Foreach(delegate(ITreeModel categoryModel, TreePath path, TreeIter categoryIter)
                {
                    var categoryItem = (Category) categoryModel.GetValue(categoryIter, 0);
                    if (categoryItem.Id == item.Category)
                    {
                        value.Text = categoryItem.CategoryName;
                        return true;
                    }
                    value.Text = "None";
                    return false;
                });
        }

        private static void RenderTaskItemPriority(TreeViewColumn column, CellRenderer cell, ITreeModel model,
            TreeIter iter)
        {
            var item = (Task) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value != null) value.Text = item.Priority;
        }

        private static void RenderTaskItemStart(TreeViewColumn column, CellRenderer cell, ITreeModel model,
            TreeIter iter)
        {
            var item = (Task) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value == null) return;
            value.Text = item.StartDate.Date != DateTime.MinValue.Date ? CheckDate(item.StartDate) : "Not set";
        }

        private static void RenderTaskItemFinish(TreeViewColumn column, CellRenderer cell, ITreeModel model,
            TreeIter iter)
        {
            var item = (Task) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value == null) return;

            if (item.DueDate.Date != DateTime.MinValue.Date)
            {
                var render = item.DueDate;
                var current = DateTime.Now;

                if (render < current)
                    value.Text = "Overdue since " + CheckDate(render);
                else
                    value.Text = CheckDate(render);
            }
            else
                value.Text = "Not set";
        }

        private static string CheckDate(DateTime date)
        {
            string output;
            if (date.Date == DateTime.Now.Date)
                output = $"Today at {date:hh:mm:ss tt}";
            else if (date.Date == DateTime.Today.AddDays(-1))
                output = $"Yesterday at {date:hh:mm:ss tt)}";
            else
                output = date.ToString(CultureInfo.CurrentCulture);
            return output;
        }

        private static void ProjectWindow_DestroyEvent(object o, DestroyEventArgs args)
        {
            UnlockFile();
            args.RetVal = true;
        }

        [ConnectBefore]
        // check if user wants to save before closing window
        private void OnDeleteEvent(object sender, DeleteEventArgs args)
        {
            if (ChangesMade)
            {
                switch (ConfirmClose())
                {
                    case 1:
                        args.RetVal = false;
                        SaveFile();
                        UnlockFile();
                        Application.Quit();
                        break;
                    case 2:
                        args.RetVal = false;
                        UnlockFile();
                        Application.Quit();
                        break;
                    default:
                        args.RetVal = true;
                        break;
                }
            }
            else
            {
                args.RetVal = false;
                UnlockFile();
                Application.Quit();
            }
        }

        /// <summary>
        ///     Method used to confirm if user wants to close the Project Window
        /// </summary>
        /// <returns></returns>
        private int ConfirmClose()
        {
            using (
                var closeConfirm = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Question,
                    ButtonsType.YesNo,
                    "Do you want to save your changes?")
                )
            {
                // if the user presses Yes
                closeConfirm.AddButton("_Cancel", ResponseType.Cancel);

                int pars;
                switch (closeConfirm.Run())
                {
                    case (int) ResponseType.Yes:
                        pars = 1;
                        break;
                    case (int) ResponseType.No:
                        pars = 2;
                        break;
                    default:
                        pars = 0;
                        break;
                }
                closeConfirm.Destroy();
                return pars;
            }
        }

        private void Calendar_MonthChanged(object sender, EventArgs e) => UpdateCalendarMarks();

        private void CategoryTreeView_RowActivated(object o, RowActivatedArgs args)
        {
            ITreeModel model;
            TreeIter iter;
            var selection = _categoryTreeView.Selection;

            if (!selection.GetSelected(out model, out iter)) return;

            var item = model.GetValue(iter, 0);

            var category = item as Category;

            if (category != null)
                _categoryDescription.Buffer.Text = string.IsNullOrWhiteSpace(category.CategoryDescription)
                    ? $"No description for {category.CategoryName}"
                    : category.CategoryDescription;
        }
    }
}
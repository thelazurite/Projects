using Gtk;
using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Globalization;
using System.IO;

using Projects.main.backend;
using static Projects.main.backend.OpenFile;
using Application = Gtk.Application;

namespace Projects.main
{
    public sealed partial class ProjectWindow : Window
    {
        //TODO: this comment is ironic

        // create a read-only instance of the database connection
		private readonly SqliteConnection _dbConnection;

        // list used to store sql commands before they are saved to the database
        private List<string> _sqlList;

        /// <summary>
        /// Initializes a new instance of the <see cref="Projects.main.ProjectWindow"/> class.
        /// </summary>
        /// <param name="pathToFile">Path to store new file.</param>
        /// <param name="fileName">Name of the new file.</param>
        public ProjectWindow(string pathToFile, string fileName) : base(WindowType.Toplevel)
        {
            // create the interface
            BuildInterface();

            // path to files used
            PathToFile = System.IO.Path.Combine(pathToFile, fileName);
            PathToLock = PathToFile + ".lk";

            // create the lockfile
            LockFile();

            // initialize the list
            _sqlList = new List<string>();

            // make sure the method has not been called when the file aleready exists
            if (!File.Exists(PathToFile))
            {
                // Create the new file, create a connection and then open the connection
                SqliteConnection.CreateFile(PathToFile);
                _dbConnection = new SqliteConnection("Data Source=" + PathToFile + ";Version=3;");
                _dbConnection.Open();

                // the structure of the database to be created
                const string categoryTbl = "create table tblCategories (categoryId varchar(36), category varchar(150), description varchar(1024))";
                const string categoryIndex = "create unique index catUniqueId on tblCategories (categoryId)";
                const string recordsTbl = "create table tblTodoItems (todoId varchar(36), todo varchar(150), description varchar(1024), itemPriority varchar(50), category varchar(50), " +
                                          "startDate datetime, finishDate datetime)";
                const string recordsIndex = "create unique index todoUniqueId on tblTodoItems (todoId)";

                // add tables and indexes to the database
                var execute = new SqliteCommand(categoryTbl, _dbConnection);
                execute.ExecuteNonQuery();
                execute = new SqliteCommand(categoryIndex, _dbConnection);
                execute.ExecuteNonQuery();
                execute = new SqliteCommand(recordsTbl, _dbConnection);
                execute.ExecuteNonQuery();
                execute = new SqliteCommand(recordsIndex, _dbConnection);
                execute.ExecuteNonQuery();

                // close the connection
                _dbConnection.Close();
            }
            else
            {
                // if the file exists open it instead
                Destroy();
                var window = new ProjectWindow(PathToFile);
                window.Show();
            }

            // set window title
            Title = "Projects - " + PathToFile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Projects.main.ProjectWindow"/> class.
        /// </summary>
        /// <param name="fullPath">Full path to existing file.</param>
        public ProjectWindow(string fullPath) : base(WindowType.Toplevel)
        {
            // path to files used
            PathToFile = fullPath;
            PathToLock = PathToFile + ".lk";

            // create lockfile
            LockFile();

            // build the interface
            BuildInterface();
            Title = "Projects - " + PathToFile;

            // commands for selecting all records from tables
            const string selectCategories = "select * from tblCategories";
            const string selectRecords = "select * from tblTodoItems";

            // create a connection and open it
            _dbConnection = new SqliteConnection("Data Source=" + PathToFile + ";Version=3;");
            _dbConnection.Open();

            // execute command for reading categories table
            var execute = new SqliteCommand(selectCategories, _dbConnection);
            var reader = execute.ExecuteReader();

            // while looping through records
            while (reader.Read())
            {
                // convert each record to a category item, null checking each value
                var category = new Category(reader["categoryId"]?.ToString() ?? "null",
                    reader["category"]?.ToString() ?? "null",
                    reader["description"]?.ToString() ?? "null", true);

                // append the category to the table model
                CategoryStore.AppendValues(category);
            }

            // execute command for reading to-do records table
            execute = new SqliteCommand(selectRecords, _dbConnection);
            reader = execute.ExecuteReader();

            while (reader.Read())
            {
                // parse dates from the database and convert it to the format used on the client's
                // machine
                DateTime start;
                DateTime.TryParse(reader["startDate"].ToString(), CultureInfo.CurrentCulture,
                    DateTimeStyles.AssumeLocal, out start);

                DateTime end;
                DateTime.TryParse(reader["finishDate"].ToString(), CultureInfo.CurrentCulture,
                    DateTimeStyles.AssumeLocal, out end);

                // null check values and convert records to ToDoItems
                var item = new ToDoItem(reader["todoId"]?.ToString() ?? "null",
                    reader["todo"]?.ToString() ?? "null", reader["description"]?.ToString() ?? "null",
                    reader["category"]?.ToString() ?? "null", reader["itemPriority"]?.ToString() ?? "null", start, end);

                // append ToDoItems to the table model
                TodoStore.AppendValues(item);
            }

            // close the connection
            _dbConnection.Close();

            // initialize the temporary list store
            _sqlList = new List<string>();
        }

        private void OpenActionOnActivated(object sender, EventArgs eventArgs)
        {
            //open the file selection window
            Open(this);
        }

        private void FileNewMenuItem_OnActivated(object sender, EventArgs eventArgs)
        {
            // destroy the window and unlock the open file
            Destroy();
            UnlockFile();

            // show the project wizard
            var window = new ProjectStart();
            window.Show();
        }

        private void AddCategory_Clicked(object sender, EventArgs e)
        {
            /*categorySidebar.Add(new Test());
            Show();*/

            // Show the new category window
            var window = new CategoryWindow();
            window.Show();

            // Handlers controlled by this class.
            window.Destroyed += CategoryWindow_Destroyed;
            window.AddCategoryHandler += Window_AddCategoryHandler;

            // stop the user from opening multiple category windows and deleting values
            _addCategoryAction.BlockActivate();
            _removeCategoryAction.BlockActivate();
        }

        /// <summary>
        /// When the user pressess the add category button in the add category
        /// window.
        /// </summary>
        /// <param name="sender">The values the user input before pressing the add button</param>
        /// <param name="e">event arguments</param>
        private void Window_AddCategoryHandler(Category sender, EventArgs e)
        {
            // append it to the table model
            CategoryStore.AppendValues(sender);

            // temorarily store command for add record to DB in list
            _sqlList.Add("insert into tblCategories(categoryId, category, description)" +
                         $"values('{sender.Id}','{sender.CategoryName}','{sender.CategoryDescription}')");
        }

        private void CategoryWindow_Destroyed(object sender, EventArgs e)
        {
            // allow the user to add/remove categories again
            _addCategoryAction.UnblockActivate();
            _removeCategoryAction.UnblockActivate();
        }

        private void AddTodoItem_Clicked(object sender, EventArgs args)
        {
            // opens the todo window
            var window = new TodoWindow(CategoryStore);
            window.Show();

            // handlers controlled by this class
            window.Destroyed += TodoWindow_Destroyed;
            window.AddTodoHandler += Window_AddTodoHandler;

            // block addition/removal of todo items
            _addToDoItemAction.BlockActivate();
            _removeToDoItemAction.BlockActivate();
        }

        private void Window_AddTodoHandler(ToDoItem sender, EventArgs e)
        {
            // store values in table model
            TodoStore.AppendValues(sender);

            // store the command to add to db temporarily
            // convert the dates to an acceptable format by the DB
            _sqlList.Add(
                "insert into tblTodoItems(todoId, todo, description, itemPriority,category, startDate, finishDate)" +
                $"values('{sender.Id}', '{sender.Name}', '{sender.Description}','{sender.Priority}','{sender.Category}'," +
                $"'{sender.Start.ToString("yyyy-MM-ddTHH:mm:ss")}','{sender.Finish.ToString("yyyy-MM-ddTHH:mm:ss")}')"
            );
        }

        private void TodoWindow_Destroyed(object sender, EventArgs e)
        {
            // unblock addition/removal of todo items
            _addToDoItemAction.UnblockActivate();
            _removeToDoItemAction.UnblockActivate();
        }

        private void SaveItem_OnActivated(object sender, EventArgs eventArgs)
        {
			Save();
        }

		private void Save()
		{
			//show the progress bar
			_fileActionProgBar.Visible = true;

			// amount of actions to complete == how many commands were stored in list
			var amt = _sqlList.Count;
			var current = 0;

			// open database connection
			_dbConnection.Open();

			foreach (var command in _sqlList)
			{
				// force progress bar to update visuall
				_fileActionProgBar.Window.ProcessUpdates(true);

				// force application to run loop events ASAP
				while (Application.EventsPending())
					Application.RunIteration(true);

				current++;

				// convert list item to sql command for the current connection
				// and execute it
				var sql = new SqliteCommand(command, _dbConnection);
				sql.ExecuteNonQuery();

				// update visual representation of progress
				_fileActionProgBar.Fraction = (float)current / amt;
			}

			// close database and reset values
			_dbConnection.Close();
			_sqlList.Clear();

			_fileActionProgBar.Fraction = 0;
			_fileActionProgBar.Visible = false;
		}

        public void categoryItemToggleCell_Toggled(object sender, ToggledArgs args)
        {
            // get the currently selected tree view item, and change the visibility
            // checkbox value based on its previous value
            TreeIter iter;
            if (!CategoryStore.GetIter(out iter, new TreePath(args.Path))) return;

            var toggler = (bool)CategoryStore.GetValue(iter, 3);
            CategoryStore.SetValue(iter, 3, !toggler);
        }

        public void deleteCategory_Clicked(object sender, EventArgs e)
        {
            try
            {
                ITreeModel model;
                TreeIter iter;
                var selection = _categoryTreeView.Selection;

                // out the values, and check if nothing has been selected
                if (!selection.GetSelected(out model, out iter)) return;

                // item is the currently selected iteration in the model
                var item = model.GetValue(iter, 0);

                // switch the item back into the category class
                var category = (item as Category);

                // check for null and then remove the item from the table
                // and add the sql command to the list
                if (category != null)
                {
                    _sqlList.Add($"delete from tblCategories where categoryId = '{category.Id}'");
                    CategoryStore.Remove(ref iter);
                }
            }
            catch (Exception exception)
            {
                var md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok,
                    $"An error has occurred: {exception.Message}. \nat {exception.Source}");
                md.Run();
                md.Destroy();
            }
        }

        private void DeleteToDoItem_Clicked(object sender, EventArgs eventArgs)
        {
            try
            {
                ITreeModel model;
                TreeIter iter;
                var selection = _todoListTreeView.Selection;

                if (!selection.GetSelected(out model, out iter)) return;

                var item = model.GetValue(iter, 0);

                var record = (item as ToDoItem);

                if (record != null) Console.WriteLine(record.Id);

                if (record != null)
                {
                    _sqlList.Add($"delete from tblTodoItems where todoId = '{record.Id}'");
                    TodoStore.Remove(ref iter);
                }
            }
            catch (Exception exception)
            {
#if DEBUG
                Console.WriteLine("Tried to access protected memory. " + exception.Message);
#endif
                var md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok,
                    $"An error has occurred: {exception.Message}. \nat {exception.Source}");
                md.Run();
                md.Destroy();
            }
        }

        private void fileExitMenuItem_Clicked(object sender, EventArgs args)
        {
            // close the program
            Destroy();
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

            var item = (Category)CategoryStore.GetValue(iter, 0);
            item.CategoryActive = !item.CategoryActive;
        }

        private static void RenderCategoryId(TreeViewColumn treeColumn, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                var item = (Category)model.GetValue(iter, 0);
                var value = cell as CellRendererText;
                if (value != null) value.Text = item.Id;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Null reference has occurred with category object id");
            }
        }

        private static void RenderCategoryName(TreeViewColumn treeColumn, CellRenderer cell, ITreeModel model, TreeIter iter)
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

        private static void RenderCategoryToggle(TreeViewColumn treeColumn, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                var item = (Category)model.GetValue(iter, 0);
                var value = cell as CellRendererToggle;
                if (value != null) value.Active = item.CategoryActive;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Null reference has occurred with category object toggle");
            }
        }

        private void CategoryTreeView_RowActivated(object o, RowActivatedArgs args)
        {
            ITreeModel model;
            TreeIter iter;
            var selection = _categoryTreeView.Selection;

            if (!selection.GetSelected(out model, out iter))
                return;

            var item = model.GetValue(iter, 0);

            var category = (item as Category);

            // renders the category description
            if (category != null)
                _categoryDescription.Buffer.Text = (string.IsNullOrWhiteSpace(category.CategoryDescription)) ?
                    $"No description for {category.CategoryName}" : category.CategoryDescription;
        }

        private static void RenderToDoItemId(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                var item = (ToDoItem)model.GetValue(iter, 0);
                var value = cell as CellRendererText;
                if (value != null) value.Text = item.Id;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Null reference has occurred to do object id");
            }
        }

        private static void RenderToDoItemName(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                var item = (ToDoItem)model.GetValue(iter, 0);
                var value = cell as CellRendererText;
                if (value != null) value.Text = item.Name;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Null reference has occurred (name)");
            }
        }

        private void RenderToDoItemCategory(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                // get the current item and render it as text
                var item = (ToDoItem)model.GetValue(iter, 0);
                var value = cell as CellRendererText;

                // check if the value is null
                if (value == null) return;

                // if the conversion from GUID is empty (zeroes)
                if (new Guid(item.Category) == Guid.Empty)
                {
                    //display value as "None"
                    value.Text = "None";
                }
                else
                {
                    // find the matching ID from the category table
                    CategoryStore.Foreach(
                        delegate (ITreeModel categoryModel, TreePath path, TreeIter categoryIter)
                        {
                            var categoryItem = (Category)categoryModel.GetValue(categoryIter, 0);
                            if (categoryItem.Id == item.Category)
                            {
                                value.Text = categoryItem.CategoryName;
                                return true;
                            }

                            value.Text = "None";
                            return false;
                        }
                    );
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Null reference has occurred (category)");
            }
        }

        private static void RenderToDoItemPriority(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                var item = (ToDoItem)model.GetValue(iter, 0);
                var value = cell as CellRendererText;
                if (value != null) value.Text = item.Priority;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Null reference has occurred (priority)");
            }
        }

        private static void RenderToDoItemStart(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                var item = (ToDoItem)model.GetValue(iter, 0);
                var value = cell as CellRendererText;
                if (value == null) return;

                //if the value isn't empty check the date using the method
                value.Text = item.Start.Date != DateTime.MinValue.Date ? CheckDate(item.Start) : "Not set";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Null reference has occurred (start)");
            }
        }

        private static void RenderToDoItemFinish(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                var item = (ToDoItem)model.GetValue(iter, 0);
                var value = cell as CellRendererText;
                if (value == null) return;

                if (item.Finish.Date != DateTime.MinValue.Date)
                {
                    var render = item.Finish;
                    var current = DateTime.Now;

                    // if the finish date is older than the current date
                    if (render < current)
                    {
                        //the item is overdue
                        value.Text = "Overdue since " + CheckDate(render);
                    }
                    else
                    {
                        value.Text = CheckDate(render);
                    }
                }
                else
                {
                    value.Text = "Not set";
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Null reference has occurred (finish)");
            }
        }

        private static string CheckDate(DateTime date)
        {
            // declare output string
            string output;

            if (date.Date == DateTime.Now.Date) //if the date is the same
            {
                output = $"Today at {date.ToString("hh:mm:ss tt")}";
            }
            else if (date.Date == DateTime.Today.AddDays(-1)) //if the date is the same as the current day -1
            {
                output = $"Yesterday at {date.ToString("hh:mm:ss tt")}";
            }
            else
            {
                output = date.ToString(CultureInfo.CurrentCulture);
            }

            return output;
        }

        private static void ProjectWindow_DestroyEvent(object o, DestroyEventArgs args)
        {
        }

        private void OnDeleteEvent(object sender, DeleteEventArgs a)
        {
			//if there are any items waiting to be added to/removed from the database
			if(_sqlList.Count != 0)
			{
				var md = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Would you like to save changes");
				if(md.Run() == (int)ResponseType.Yes)
				{
					Save();

					md.Destroy();
					Destroy();
					UnlockFile();

					// quit
					a.RetVal = true;
					Application.Quit();
				}
				else
				{
					// destroy and unlock
					md.Destroy();
					Destroy();
					UnlockFile();

					// quit
					a.RetVal = true;
					Application.Quit();
				}
			}
           
        }
    }
}
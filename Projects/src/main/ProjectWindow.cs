
// MIT License
//
// Copyright (c) 2017 Dylan Eddies
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using GLib;
using Gtk;
using Projects.Gtk.main.backend;
using Projects.Dal;
using Application = Gtk.Application;
using DateTime = System.DateTime;
using Object = System.Object;

namespace Projects.Gtk.main
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
        public ProjectWindow(String pathToFile, String fileName) : base(WindowType.Toplevel)
        {
            BuildInterface();

            ApplicationHelper.PathToFile = System.IO.Path.Combine(pathToFile, fileName);
            ApplicationHelper.PathToLock = ApplicationHelper.PathToFile + ".lk";

            ApplicationHelper.LockFile();

            _sqlItems = new List<DbItem>();

            if (!File.Exists(ApplicationHelper.PathToFile))
            {
                SQLiteConnection.CreateFile(ApplicationHelper.PathToFile);
                _dbConnection = new SQLiteConnection($"Data Source={ApplicationHelper.PathToFile};Version=3;");
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
                var window = new ProjectWindow(ApplicationHelper.PathToFile);
                window.Show();
            }
            Title = "Projects - " + ApplicationHelper.PathToFile;
        }

        public ProjectWindow(String fullPath) : base(WindowType.Toplevel)
        {
            ApplicationHelper.PathToFile = fullPath;
            ApplicationHelper.PathToLock = ApplicationHelper.PathToFile + ".lk";

            ApplicationHelper.LockFile();

            BuildInterface();
            Title = "Projects - " + ApplicationHelper.PathToFile;

            _dbConnection = new SQLiteConnection("Data Source=" + ApplicationHelper.PathToFile + ";Version=3;");
            RefreshData();
            _sqlItems = new List<DbItem>();
        }

        private Boolean ChangesMade => _sqlItems?.Count > 0;

        private void UpdateCalendarMarks()
        {
            _calendar.ClearMarks();


            _taskStore.Foreach(delegate(ITreeModel taskModel, TreePath path, TreeIter taskIter)
            {
                var taskItem = (TaskItem) taskModel.GetValue(taskIter, 0);

                if ((_calendar.Month == taskItem.DueDate.Month - 1) && (_calendar.Year == taskItem.DueDate.Year))
                    _calendar.MarkDay((UInt32) taskItem.DueDate.Day);

                return false;
            });
        }

        private void RefreshData()
        {
            // clear data before refreshing
            _categoryStore.Clear();
            _taskStore.Clear();

            // open database connection
            _dbConnection.Open();

            // select all information from both tables
            const String selectCategories = "select * from tblCategories";
            const String selectRecords = "select * from tblTodoItems";

            // set query to selct categories from the data context
            var execute = new SQLiteCommand(selectCategories, _dbConnection);
            var reader = execute.ExecuteReader();

            // while reading through each item, check for nulls and append to the list store
            while (reader.Read())
            {
                var category = new Category(reader["categoryId"]?.ToString() ?? "null",
                    DbItem.RestoreSpecialChars(reader["category"]?.ToString()) ?? "null",
                    DbItem.RestoreSpecialChars(reader["description"]?.ToString()) ?? "null", true);

                _categoryStore.AppendValues(category);
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

                var item = new TaskItem(reader["todoId"]?.ToString() ?? "null",
                    DbItem.RestoreSpecialChars(reader["todo"]?.ToString()) ?? "null",
                    DbItem.RestoreSpecialChars(reader["description"]?.ToString()) ?? "null",
                    reader["category"]?.ToString() ?? "null", reader["itemPriority"]?.ToString() ?? "null", start, end);

                _taskStore.AppendValues(item);
            }

            // close the connection
            _dbConnection.Close();

            // Update calendar marks
            UpdateCalendarMarks();
        }

        private void OpenActionOnActivated(Object sender, EventArgs eventArgs)
        {
            if (ChangesMade)
                switch (ConfirmClose())
                {
                    case 1:
                        SaveFile();
                        if (ApplicationHelper.Open(this))
                            ApplicationHelper.UnlockFile();
                        break;
                    case 2:
                        if (ApplicationHelper.Open(this))
                            ApplicationHelper.UnlockFile();
                        break;
                    default:
                        return;
                }
            else if (ApplicationHelper.Open(this))
                ApplicationHelper.UnlockFile();
        }


        private void FileNewMenuItem_OnActivated(Object sender, EventArgs eventArgs)
        {
            _dbConnection.Close();
            _dbConnection = new SQLiteConnection();
            ApplicationHelper.UnlockFile();
            Destroy();
            new ProjectWizard().Show();
        }

        private void AddCategory_Clicked(Object sender, EventArgs e)
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

        private void Window_AddCategoryHandler(Object sender, EventArgs e)
        {
            var category = sender as Category;
            if (category == null) return;
            category.AddToDb();
            _categoryStore.AppendValues(category);
            _sqlItems.Add(category);
        }

        private void CategoryWindow_Destroyed(Object sender, EventArgs e)
        {
            _addCategoryAction.UnblockActivate();
            _removeCategoryAction.UnblockActivate();
        }

        private void AddTaskItem_Clicked(Object sender, EventArgs args)
        {
            var taskTab = new TaskItemTab(_categoryStore, this);
            taskTab.Destroyed += TaskItemWindow_Destroyed;
            taskTab.AddTaskItemHandler += Window_AddTaskItemHandler;
            var win = _noteBook.AppendPage(taskTab, new Label("New TaskItem"));
            _noteBook.CurrentPage = win;
            var windowNoteChild = (Notebook.NotebookChild) _noteBook[taskTab];
            windowNoteChild.Detachable = false;
            windowNoteChild.TabFill = false;
        }

        /// <summary>
        ///     Handles the addition of task items.
        /// </summary>
        /// <param name="sender">The task item object</param>
        /// <param name="e">empty.</param>
        private void Window_AddTaskItemHandler(Object sender, EventArgs e)
        {
            var task = sender as TaskItem;
            if (task == null) return;
            task.AddToDb();
            _taskStore.AppendValues(task);
            _sqlItems.Add(task);
            UpdateCalendarMarks();
        }

        private void TaskItemWindow_Destroyed(Object sender, EventArgs e)
        {
            _addTaskItemAction.UnblockActivate();
            _removeTaskItemAction.UnblockActivate();
        }

        private void SaveItem_OnActivated(Object sender, EventArgs eventArgs) => SaveFile();

        private void SaveFile()
        {
            _fileActionProgBar.Visible = true;
            var amt = _sqlItems.Count;
            var current = 0;
            _dbConnection.Open();

            foreach (DbItem item in _sqlItems.ToList())
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
                _fileActionProgBar.Fraction = (Single) current/amt;
            }

            _fileActionProgBar.Fraction = 1;
            _fileActionProgBar.Visible = false;
            _fileActionProgBar.Fraction = 0;

            _dbConnection.Close();
            RefreshData();
        }

        public void categoryItemToggleCell_Toggled(Object sender, ToggledArgs args)
        {
            TreeIter iter;
            if (!_categoryStore.GetIter(out iter, new TreePath(args.Path))) return;
            var toggle = (Boolean) _categoryStore.GetValue(iter, 3);
            _categoryStore.SetValue(iter, 3, !toggle);
        }

        private Boolean CheckList(DbItem item)
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

        public void deleteCategory_Clicked(Object sender, EventArgs e)
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

            // check through TaskItem list if there are any tasks which fall under the category
            _taskStore.Foreach(delegate(ITreeModel taskModel, TreePath path, TreeIter taskIter)
            {
                var taskItem = (TaskItem) taskModel.GetValue(taskIter, 0);
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

            _categoryStore.Remove(ref iter);
        }

        private void ModifyTaskItemActionOnActivated(Object sender, EventArgs eventArgs)
        {
            var taskTab = new TaskItemTab(_categoryStore, this);
            taskTab.Destroyed += TaskItemWindow_Destroyed;

            TaskItem record = RetrieveTreeViewSelection<TaskItem>(_mainView.Selection).Result;

            if (record == null) return;

            taskTab.LoadTodoItem(record);

            var win = _noteBook.AppendPage(taskTab, new Label($"Modifying {record.Name}"));
            _noteBook.CurrentPage = win;

            var windowNoteChild = (Notebook.NotebookChild)_noteBook[taskTab];
            windowNoteChild.Detachable = false;
            windowNoteChild.TabFill = false;
        }

        private static TreeViewSelection<T> RetrieveTreeViewSelection<T>(TreeSelection selection) where T : class
        {
            ITreeModel model;
            TreeIter iter;

            if (!selection.GetSelected(out model, out iter)) return TreeViewSelection<T>.Empty();

            var result = model.GetValue(iter, 0) as T;
            var item = new TreeViewSelection<T>
            {
                Iter = iter,
                Result = result
            };

            return item;
        }

        private void DeleteTaskItem_Clicked(Object sender, EventArgs eventArgs)
        {
            var value = RetrieveTreeViewSelection<TaskItem>(_mainView.Selection);
            TaskItem record = value.Result;
            TreeIter iter = value.Iter;

            if (value.Result == null) return;

            record.DeleteFromDb();

            if (CheckList(record))
                _sqlItems.Add(record);

            _taskStore.Remove(ref iter);
            UpdateCalendarMarks();
        }

        private void fileExitMenuItem_Clicked(Object sender, EventArgs args)
        {
            if (ChangesMade)
                switch (ConfirmClose())
                {
                    case 1:
                        SaveFile();
                        ApplicationHelper.UnlockFile();
                        Application.Quit();
                        break;
                    case 2:
                        ApplicationHelper.UnlockFile();
                        Application.Quit();
                        break;
                    default:
                        return;
                }
            else
            {
                ApplicationHelper.UnlockFile();
                Application.Quit();
            }
        }

        private void CategoryItemNameCell_Edited(Object sender, EditedArgs args)
        {
            TreeIter iter;
            _categoryStore.GetIter(out iter, new TreePath(args.Path));
            _categoryStore.SetValue(iter, 1, args.NewText);
        }

        private void CategoryItem_Toggled(Object sender, ToggledArgs args)
        {
            TreeIter iter;
            _categoryStore.GetIter(out iter, new TreePath(args.Path));
            Console.WriteLine(iter);

            var item = (Category) _categoryStore.GetValue(iter, 0);
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
            var item = (TaskItem) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value != null) value.Text = item.Id;
        }

        private static void RenderTaskItemName(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            var item = (TaskItem) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value != null) value.Text = item.Name;
        }

        private void RenderTaskItemCategory(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            var item = (TaskItem) model.GetValue(iter, 0);
            var value = cell as CellRendererText;

            if (value == null) return;
            if (new Guid(item.Category) == Guid.Empty)
                value.Text = "None";
            else
                _categoryStore.Foreach(delegate(ITreeModel categoryModel, TreePath path, TreeIter categoryIter)
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
            var item = (TaskItem) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value != null) value.Text = item.Priority;
        }

        private static void RenderTaskItemStart(TreeViewColumn column, CellRenderer cell, ITreeModel model,
            TreeIter iter)
        {
            var item = (TaskItem) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value == null) return;
            value.Text = item.StartDate.Date != DateTime.MinValue.Date ? CheckDate(item.StartDate) : "Not set";
        }

        private static void RenderTaskItemFinish(TreeViewColumn column, CellRenderer cell, ITreeModel model,
            TreeIter iter)
        {
            var item = (TaskItem) model.GetValue(iter, 0);
            var value = cell as CellRendererText;
            if (value == null) return;

            if (item.DueDate.Date != DateTime.MinValue.Date)
            {
                DateTime render = item.DueDate;
                DateTime current = DateTime.Now;

                if (render < current)
                    value.Text = "Overdue since " + CheckDate(render);
                else
                    value.Text = CheckDate(render);
            }
            else
                value.Text = "Not set";
        }

        private static String CheckDate(DateTime date)
        {
            String output;
            if (date.Date == DateTime.Now.Date)
                output = $"Today at {date:hh:mm:ss tt}";
            else if (date.Date == DateTime.Today.AddDays(-1))
                output = $"Yesterday at {date:hh:mm:ss tt)}";
            else
                output = date.ToString(CultureInfo.CurrentCulture);
            return output;
        }

        private static void ProjectWindow_DestroyEvent(Object o, DestroyEventArgs args)
        {
            ApplicationHelper.UnlockFile();
            args.RetVal = true;
        }

        [ConnectBefore]
        // check if user wants to save before closing window
        private void OnDeleteEvent(Object sender, DeleteEventArgs args)
        {
            if (ChangesMade)
            {
                switch (ConfirmClose())
                {
                    case 1:
                        args.RetVal = false;
                        SaveFile();
                        ApplicationHelper.UnlockFile();
                        Application.Quit();
                        break;
                    case 2:
                        args.RetVal = false;
                        ApplicationHelper.UnlockFile();
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
                ApplicationHelper.UnlockFile();
                Application.Quit();
            }
        }

        /// <summary>
        ///     Method used to confirm if user wants to close the Project Window
        /// </summary>
        /// <returns></returns>
        private Int32 ConfirmClose()
        {
            using (
                var closeConfirm = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Question,
                    ButtonsType.YesNo,
                    "Do you want to save your changes?")
                )
            {
                // if the user presses Yes
                closeConfirm.AddButton("_Cancel", ResponseType.Cancel);

                Int32 pars;
                switch (closeConfirm.Run())
                {
                    case (Int32) ResponseType.Yes:
                        pars = 1;
                        break;
                    case (Int32) ResponseType.No:
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

        private void Calendar_MonthChanged(Object sender, EventArgs e) => UpdateCalendarMarks();

        private void CategoryTreeView_RowActivated(Object o, RowActivatedArgs args)
        {

            var retrieval = RetrieveTreeViewSelection<Category>(_categoryTreeView.Selection);
            Category category = retrieval.Result;

            if (category != null)
                _categoryDescription.Buffer.Text = String.IsNullOrWhiteSpace(category.CategoryDescription)
                    ? $"No description for {category.CategoryName}"
                    : category.CategoryDescription;
        }
    }
}
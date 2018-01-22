
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
using System.Globalization;
using GLib;
using Gtk;
using Projects.Dal;
using DateTime = System.DateTime;
using Window = Gdk.Window;

namespace Projects.Gtk.main
{
    public sealed partial class TaskItemTab : VBox
    {
        private readonly ListStore _categories;
        private readonly IWrapper _parent;
        private DateTime _end;
        private DateTime _start;
        private DatePicker _window;

        /// <summary>
        ///     Constructor for the Task Creation tab
        /// </summary>
        /// <param name="categories">List to be provided to the window</param>
        /// <param name="parent">The parent Widget/Window</param>
        public TaskItemTab(ListStore categories, IWrapper parent)
        {
            
            _parent = parent;
            ParentWindow = _parent as Window;
            _categories = categories;
            BuildInterface();
        }

        public void LoadTodoItem(Activity task)
        {
            if (task == null) return;

            _categories.Foreach(delegate(ITreeModel model, TreePath path, TreeIter iter)
            {
                var category = (Category) model.GetValue(iter, 0);
                if (category.Id == task.Category)
                    _categoryBox.SetActiveIter(iter);
                return false;
            });

            _nameEntry.Text = task.Name;
            _descView.Buffer.Text = task.Description;
            _startEntry.Text = task.StartDate.ToString(CultureInfo.CurrentCulture);
            _endEntry.Text = task.DueDate.ToString(CultureInfo.CurrentCulture);
        }

        public event Action<object, EventArgs> AddTaskItemHandler, ActivityNameChangeHandler;

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Destroy();
        }

        /// <summary>
        ///     Logic executed once the add button is pressed
        /// </summary>
        private void AddButton_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(_nameEntry.Text))
            {
                const String message = "No name entered!";
                DialogHelper.DisplayError(message, _parent as global::Gtk.Window);
                return;
            }

            _categoryBox.GetActiveIter(out var iter);
            
            var category = _categoryBox.Model.GetValue(iter, 0) as Category;
            var priority = _priorityBox.Active;

            var todo = new Activity
            (
                Guid.NewGuid().ToString(),
                _nameEntry.Text,
                _descView.Buffer.Text,
                category != null ? category.Id : Guid.Empty.ToString(),
                _values[priority == -1 ? 3 : priority],
                _start, _end
            );

            AddTodo(todo, EventArgs.Empty);

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

        private void AddTodo(object sender, EventArgs e) => AddTaskItemHandler?.Invoke(sender, e);
        private void ReportNameChange(object sender, EventArgs e) => ActivityNameChangeHandler?.Invoke(sender, e);

        private static void Func(ICellLayout cellLayout, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            var item = (Category) model.GetValue(iter, 0);
            if (cell is CellRendererText value) 
                value.Text = item.CategoryName;
        }

        
        private void NameTextChanged()
        {
            ReportNameChange(_nameEntry.Text, EventArgs.Empty);
        }
    }
}
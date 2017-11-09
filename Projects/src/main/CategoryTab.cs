
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
using GLib;
using Gtk;
using Projects.Dal;
using Object = System.Object;

namespace Projects.Gtk.main
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
            ParentWindow = _parent as Gdk.Window;
            BuildInterface();
        }

        public event Action<Object, EventArgs> AddCategoryHandler;

        private void CancelButton_Clicked(Object sender, EventArgs e) => Destroy();

        /// <summary>
        ///     logic executed once the user attempts to add a category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Clicked(Object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(_nameEntry.Text))
            {
                using (var md = new MessageDialog(_parent as Window, DialogFlags.Modal, MessageType.Error,
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
        public void AddCategory(Object sender, EventArgs e) => AddCategoryHandler?.Invoke(sender, e);
    }
}
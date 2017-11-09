
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
using Gtk;
using Projects.Gtk.main.backend;
using Button = Gtk.Button;
using Label = Gtk.Label;

namespace Projects.Gtk.main
{
    public sealed partial class CategoryTab
    {
        private Button _addButton;

        private HBox _buttonsHBox;
        private Button _cancelButton;

        private Fixed _descFixed;
        private Label _descLabel;
        private TextView _descView;
        private ScrolledWindow _descWindow;
        private Entry _nameEntry;

        private Fixed _nameFixed;
        private Label _nameLabel;

        private void BuildInterface()
        {
            Gui.Initialize(this);
            Name = "CategoryTab";


            _nameFixed = new Fixed
            {
                Name = "nameFixed",
                HasWindow = false
            };

            _nameLabel = new Label
            {
                Name = "nameLabel",
                Text = "Name"
            };

            _nameEntry = new Entry
            {
                Name = "nameEntry"
            };

            _descFixed = new Fixed
            {
                Name = "descFixed",
                HasWindow = false
            };

            _descLabel = new Label
            {
                Name = "descLabel",
                Text = "Description"
            };

            _descWindow = new ScrolledWindow
            {
                Name = "descWindow",
                ShadowType = ShadowType.None,
                HscrollbarPolicy = PolicyType.Never,
                VscrollbarPolicy = PolicyType.Automatic
            };

            _descView = new TextView
            {
                Name = "descView",
                BorderWidth = 2,
                WrapMode = WrapMode.WordChar,
                HscrollPolicy = ScrollablePolicy.Natural
            };

            _buttonsHBox = new HBox
            {
                Name = "buttonsFixed",
                HasWindow = false
            };

            _addButton = new Button
            {
                Name = "addButton",
                Label = "Create"
            };

            _cancelButton = new Button
            {
                Name = "cancelButton",
                Label = "Cancel"
            };


            _descWindow.HScrollbar.Visible = false;
            _descWindow.VScrollbar.Visible = true;


            _addButton.WidthRequest = 100;
            _cancelButton.WidthRequest = 100;

            Add(_nameFixed);

            var nameFixedChild = (BoxChild) this[_nameFixed];
            nameFixedChild.Expand = false;
            nameFixedChild.Padding = 5;

            _nameFixed.Add(_nameLabel);
            var nameLabelChild = (Fixed.FixedChild) _nameFixed[_nameLabel];
            nameLabelChild.X = 5;
            nameLabelChild.Y = 5;

            Add(_nameEntry);

            var nameEntryChild = (BoxChild) this[_nameEntry];
            nameEntryChild.Expand = false;

            Add(_descFixed);
            var descFixedChild = (BoxChild) this[_descFixed];
            descFixedChild.Expand = false;
            descFixedChild.Padding = 5;

            _descFixed.Add(_descLabel);

            var descLabelChild = (Fixed.FixedChild) _descFixed[_descLabel];
            descLabelChild.X = 5;
            descLabelChild.Y = 5;

            Add(_descWindow);

            var descViewChild = (BoxChild) this[_descWindow];
            descViewChild.Expand = false;

            _descWindow.AddWithViewport(_descView);

            Add(_buttonsHBox);

            var buttonsChild = (BoxChild) this[_buttonsHBox];
            buttonsChild.Expand = false;
            buttonsChild.Padding = 5;

            _buttonsHBox.Add(_addButton);

            var addChild = (BoxChild) _buttonsHBox[_addButton];
            addChild.Expand = false;
            addChild.Padding = 5;

            _buttonsHBox.Add(_cancelButton);
            var cancelChild = (BoxChild) _buttonsHBox[_cancelButton];
            cancelChild.Expand = false;
            cancelChild.Padding = 5;

            Margin = 10;

            ShowAll();

            DeleteEvent += OnDeleteEvent;
            _addButton.Clicked += AddButton_Clicked;
            _cancelButton.Clicked += CancelButton_Clicked;
        }

        private void OnDeleteEvent(Object o, DeleteEventArgs args)
        {
            args.RetVal = true;
            Destroy();
        }
    }
}
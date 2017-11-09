
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
using Gdk;
using Gtk;
using Projects.Gtk.main.backend;

namespace Projects.Gtk.main
{
    public partial class ProjectWizard
    {
        private Button _cancelButton;
        private Fixed _containerFixed;

        private Button _createButton;
        private Entry _fileEntry;
        private Label _fileLabel;

        private Entry _filepathEntry;

        private CheckButton _loadOnStartButton;

        private Label _pathLabel;

        private Button _selectPathButton;

        private void BuildInterface()
        {
            Gui.Initialize(this);
            Icon = new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, !ApplicationHelper.IsUnix ? @"Content\img\todo.png" : @"Content/img/todo.png"));

            Name = "ProjectWizard";
            Title = "Project Creation Wizard";
            WindowPosition = WindowPosition.Center;
            Resizable = false;
            SetSizeRequest(300, 140);

            _containerFixed = new Fixed
            {
                Name = "containerFixed",
                HasWindow = false
            };

            _pathLabel = new Label
            {
                Name = "pathLabel",
                Text = "Location"
            };

            _containerFixed.Add(_pathLabel);

            var pathLabelChild = (Fixed.FixedChild) _containerFixed[_pathLabel];
            pathLabelChild.X = 10;
            pathLabelChild.Y = 45;

            _filepathEntry = new Entry
            {
                Name = "filePathEntry",
                IsEditable = false
            };

            _filepathEntry.Changed += _filepathEntry_Changed;
            _filepathEntry.SetSizeRequest(200, 25);

            _containerFixed.Add(_filepathEntry);

            var pathChild = (Fixed.FixedChild) _containerFixed[_filepathEntry];
            pathChild.X = 65;
            pathChild.Y = 40;

            _selectPathButton = new Button
            {
                Name = "selectPathButton",
                Label = "..."
            };
            _selectPathButton.Clicked += _selectPathButton_Clicked;
            _selectPathButton.SetSizeRequest(25, 20);
            _containerFixed.Add(_selectPathButton);

            var pathButtonChild = (Fixed.FixedChild) _containerFixed[_selectPathButton];
            pathButtonChild.X = 265;
            pathButtonChild.Y = 40;

            _fileLabel = new Label
            {
                Name = "fileLabel",
                Text = "File name"
            };

            _containerFixed.Add(_fileLabel);
            var fileLabelChild = (Fixed.FixedChild) _containerFixed[_fileLabel];
            fileLabelChild.X = 10;
            fileLabelChild.Y = 85;

            _fileEntry = new Entry
            {
                Name = "fileEntry"
            };

            _fileEntry.Changed += _fileEntry_Changed;

            _fileEntry.SetSizeRequest(225, 25);

            _containerFixed.Add(_fileEntry);

            var fileChild = (Fixed.FixedChild) _containerFixed[_fileEntry];
            fileChild.X = 65;
            fileChild.Y = 80;

            _loadOnStartButton = new CheckButton("Load file on program start")
            {
                Name = "loadFileOnStart",
                Active = true
            };

            _containerFixed.Add(_loadOnStartButton);

            var loadChild = (Fixed.FixedChild) _containerFixed[_loadOnStartButton];
            loadChild.X = 10;
            loadChild.Y = 110;

            _createButton = new Button
            {
                Name = "createButton",
                Label = "Create"
            };

            _createButton.Clicked += _createButton_Clicked;

            _containerFixed.Add(_createButton);

            var createChild = (Fixed.FixedChild) _containerFixed[_createButton];
            createChild.X = 190;
            createChild.Y = 110;

            _cancelButton = new Button
            {
                Name = "cancelButton",
                Label = "Cancel"
            };
            _cancelButton.Clicked += _cancelButton_Clicked;

            _containerFixed.Add(_cancelButton);

            var cancelChild = (Fixed.FixedChild) _containerFixed[_cancelButton];
            cancelChild.X = 240;
            cancelChild.Y = 110;

            Add(_containerFixed);

            ShowAll();
            DeleteEvent += OnDeleteEvent;
        }

        private static void OnDeleteEvent(Object o, DeleteEventArgs args)
        {
            args.RetVal = true;
            Application.Quit();
        }
    }
}
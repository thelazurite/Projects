
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
    public partial class MainWindow
    {
        private Image _backgroundImage;
        private Fixed _containerFixed;

        private Button _newButton;
        private Button _openButton;

        private void BuildInterface()
        {
            Gui.Initialize(this);

            Name = "MainWindow";
            Title = "Projects " + typeof (MainWindow).Assembly.GetName().Version;
            WindowPosition = WindowPosition.Center;
            Icon =
                new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    !ApplicationHelper.IsUnix ? @"Content\img\todo.png" : @"Content/img/todo.png"));
            DefaultWidth = 553;
            DefaultHeight = 194;
            Resizable = false;

            _containerFixed = new Fixed
            {
                Name = "containerFixed",
                HasWindow = false
            };


            _backgroundImage = new Image
            {
                Name = "backgroundImage",
                Pixbuf =
                    new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        !ApplicationHelper.IsUnix ? @"Content\img\splash.png" : @"Content/img/splash.png"))
            };

            _containerFixed.Add(_backgroundImage);
            var backgroundChild = (Fixed.FixedChild) _containerFixed[_backgroundImage];

            backgroundChild.X = 0;
            backgroundChild.Y = 0;

            _openButton = new Button("_Open")
            {
                Name = "openButton"
            };

            _openButton.Clicked += OpenButton_Clicked;
            _containerFixed.Add(_openButton);
            var openChild = (Fixed.FixedChild) _containerFixed[_openButton];

            openChild.X = 480;
            openChild.Y = 80;

            _newButton = new Button("_New")
            {
                Name = "newButton"
            };
            _newButton.Clicked += NewButton_Clicked;

            _containerFixed.Add(_newButton);
            var newChild = (Fixed.FixedChild) _containerFixed[_newButton];

            newChild.X = 480;
            newChild.Y = 120;

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
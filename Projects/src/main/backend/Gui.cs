
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
using System.IO;
using Gdk;
using Gtk;

namespace Projects.Gtk.main.backend
{
    internal class Gui
    {
        private static Boolean _initialized;

        /// <summary>
        ///     Initialize a widget
        /// </summary>
        /// <param name="widget">Widget to initialize</param>
        internal static void Initialize(Widget widget)
        {
            if (_initialized) return;

            if (_initialized)
                _initialized = false;

            _initialized = true;

            var factory = new IconFactory();

            var save =
                new IconSet(
                    new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, !ApplicationHelper.IsUnix ? @"Content\img\CircledSave.png": @"Content/img/CircledSave.png")));
            factory.Add("CircledSave", save);
            var add =
                new IconSet(
                    new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, !ApplicationHelper.IsUnix ? @"Content\img\CircledPlus.png": @"Content/img/CircledPlus.png")));
            factory.Add("CircledPlus", add);
            var remove =
                new IconSet(
                    new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, !ApplicationHelper.IsUnix ? @"Content\img\CircledMinus.png" : @"Content/img/CircledMinus.png")));
            factory.Add("CircledMinus", remove);
            var calendar =
                new IconSet(new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, !ApplicationHelper.IsUnix ? @"Content\img\calendar.png": @"Content/img/calendar.png")));
            factory.Add("Calendar", calendar);

            factory.AddDefault();
        }
    }
}
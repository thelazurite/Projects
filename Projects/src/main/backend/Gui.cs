using System;
using System.IO;
using Gdk;
using Gtk;

namespace Projects.main.backend
{
    internal class Gui
    {
        private static bool _initialized;

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
                    new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\CircledSave.png")));
            factory.Add("CircledSave", save);
            var add =
                new IconSet(
                    new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\CircledPlus.png")));
            factory.Add("CircledPlus", add);
            var remove =
                new IconSet(
                    new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\CircledMinus.png")));
            factory.Add("CircledMinus", remove);
            var calendar =
                new IconSet(new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\calendar.png")));
            factory.Add("Calendar", calendar);

            factory.AddDefault();
        }
    }
}
using Gdk;
using Gtk;
using System;
using System.IO;

namespace Projects.main.backend
{
    internal class Gui
    {
        /// <summary>
        /// Initialize a widget
        /// </summary>
        /// <param name="widget">Widget to initialize</param>
        internal static void Initialize(Gtk.Window widget)
        {
            var factory = new IconFactory();
            if (OS.isWindows())
            {
                widget.Icon = new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\todo.png"));

                var save = new IconSet(new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\CircledSave.png")));
                factory.Add("CircledSave", save);
                var add = new IconSet(new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\CircledPlus.png")));
                factory.Add("CircledPlus", add);
                var remove = new IconSet(new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\CircledMinus.png")));
                factory.Add("CircledMinus", remove);
                var calendar = new IconSet(new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\calendar.png")));
                factory.Add("Calendar", calendar);
            }
            else
            {
                widget.Icon = new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content/img/todo.png"));

                var save = new IconSet(new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content/img/CircledSave.png")));
                factory.Add("CircledSave", save);
                var add = new IconSet(new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content/img/CircledPlus.png")));
                factory.Add("CircledPlus", add);
                var remove = new IconSet(new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content/img/CircledMinus.png")));
                factory.Add("CircledMinus", remove);
                var calendar = new IconSet(new Pixbuf(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content/img/calendar.png")));
                factory.Add("Calendar", calendar);
            }

            factory.AddDefault();
        }
    }

    internal class ActionGroups
    {
        public static ActionGroup GetActionGroup(Type type)
        {
            return GetActionGroup(type.FullName);
        }

        public static ActionGroup GetActionGroup(string name)
        {
            return null;
        }
    }
}
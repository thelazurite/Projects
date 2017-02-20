using System;
using Gtk;

namespace Projects.main.backend
{
    internal class ActionGroups
    {
        public static ActionGroup GetActionGroup(Type type) => GetActionGroup(type.FullName);
        public static ActionGroup GetActionGroup(string name) => null;
    }
}
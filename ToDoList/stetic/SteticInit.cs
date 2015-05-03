using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Gtk;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Globalization;

namespace Stetic
{
	/// <summary>
	/// Initialize Stetic
	/// </summary>

	internal class SteticInit
	{
		private static bool initialized;

		internal static void Initialize(Gtk.Widget libraryInitializer)
		{				
			Console.WriteLine("checkInit? " + SteticInit.initialized);
			if (SteticInit.initialized == true) {
				Stetic.SteticInit.initialized = false;
			}
			if (Stetic.SteticInit.initialized == false)
			{
				try
				{
					SteticInit.initialized = true;
	
					global::Gtk.IconFactory w1 = new global::Gtk.IconFactory ();
	
					global::Gtk.IconSet w2 = new global::Gtk.IconSet (new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, ".\\CircledSave.png")));
					w1.Add ("CircledSave", w2);
					global::Gtk.IconSet w3 = new global::Gtk.IconSet (new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, ".\\CircledPlus.png")));
					w1.Add ("CircledPlus", w3);
					global::Gtk.IconSet w4 = new global::Gtk.IconSet (new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, ".\\CircledMinus.png")));
					w1.Add ("CircledMinus", w4);
					global::Gtk.IconSet w5 = new global::Gtk.IconSet (new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, ".\\calendar.png")));
					w1.Add ("Calendar", w5);
					
					w1.AddDefault ();
				} catch (Exception e) {
					if(!e.Message.Contains(".dll")){
					   	MessageBox.Show("Initializing hass failed. It is possible that the resource files located in: "
					   	                + global::System.AppDomain.CurrentDomain.BaseDirectory
					                	+ " are missing.\n\nConsole output: " + e.Message
					                );
				   } else {
					   	MessageBox.Show("Dll file to load images is missing.\nConsole Output: " 
					   	                + e.Message);
				   }
				}
				
			}
		}
	}
	internal class ActionGroup
	{
		public static Gtk.ActionGroup GetActionGroup (System.Type type)
		{
			return Stetic.ActionGroups.GetActionGroup(type.FullName);
		}
		public static Gtk.ActionGroup GetActionGroup (string name)
		{
			return null;
		}
	}
}

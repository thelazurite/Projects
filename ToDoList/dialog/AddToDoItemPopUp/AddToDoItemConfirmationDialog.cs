// ToDoList - A simple To-Do item manager
// Copyright (C) 2015 DylanEddies
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Gtk;

namespace ToDoList
{
	public partial class AddToDoItemConfirmationDialog : Gtk.Window
	{
		public AddToDoItemConfirmationDialog () : base(Gtk.WindowType.Toplevel)
		{
			BuildConfirmationDialog();
		}
	
		public void OnDeleteEvent(object sender, DeleteEventArgs e)
		{
			this.Destroy();
		}
		public void yesButton_Clicked(object sender, EventArgs e)
		{
			GlobalGuiVars.AddToDoItemPopUp_ConfirmClose = true;
			this.Destroy ();
		}
		public void noButton_Clicked(object sender, EventArgs e)
		{
			GlobalGuiVars.AddToDoItemPopUp_ConfirmClose = false;
			this.Destroy();
		}
	}
}
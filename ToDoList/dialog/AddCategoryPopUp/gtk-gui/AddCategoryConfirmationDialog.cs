// ToDoList - A simple To-Do item manager
// Copyright (C) 2014 Dylan Eddies
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gtk;

namespace ToDoList
{
	public partial class AddCategoryConfirmationDialog
	{
		private global::Gtk.VBox messageContainer;
		private global::Gtk.Label message;
		private global::Gtk.HSeparator hsep;
		private global::Gtk.HBox buttonContainer;
		private global::Gtk.Button yesbutton;
		private global::Gtk.Button nobutton;

		protected virtual void BuildConfirmationDialog()
		{
			Console.WriteLine ("Building pop-up");

			this.Name = "AddCategoryConfirmationDialog";
			this.Title = "Confirm";
			Console.WriteLine ("Window Created: " + this.Title);
			this.WindowPosition = global::Gtk.WindowPosition.Center;

			this.messageContainer =  new global::Gtk.VBox();
			this.messageContainer.Name = "messageContainer";

			this.message = new global::Gtk.Label ();
			this.message.Name = "message";
			this.message.Text = "You have modified the form.\nAre you sure you wish to cancel?";
			this.messageContainer.Add (message);
			global::Gtk.Box.BoxChild msgmc = ((global::Gtk.Box.BoxChild)(this.messageContainer[this.message]));

			this.hsep = new global::Gtk.HSeparator ();
			this.messageContainer.Add (hsep);
			global::Gtk.Box.BoxChild hsmc = ((global::Gtk.Box.BoxChild)(this.messageContainer [this.hsep]));

			this.buttonContainer = new global::Gtk.HBox ();
			this.buttonContainer.Name = "buttonContainer";

			this.messageContainer.Add (buttonContainer);
			global::Gtk.Box.BoxChild bcmc = ((global::Gtk.Box.BoxChild)(this.messageContainer[this.buttonContainer]));

			this.yesbutton = new global::Gtk.Button ();
			this.yesbutton.Name = "yesbutton";
			this.yesbutton.Label = "Yes";
			this.buttonContainer.Add (yesbutton);
			this.yesbutton.Clicked += yesbutton_Clicked;
			global::Gtk.Box.BoxChild ybbc = ((global::Gtk.Box.BoxChild)(this.buttonContainer [this.yesbutton]));

			this.nobutton = new global::Gtk.Button();
			this.nobutton.Name = "nobutton";
			this.nobutton.Label = "No";
			this.buttonContainer.Add (nobutton);
            this.nobutton.Clicked += nobutton_Clicked;
			global::Gtk.Box.BoxChild nbbc = ((global::Gtk.Box.BoxChild)(this.buttonContainer [this.nobutton]));


			this.Add (messageContainer);
			
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}

			this.DefaultWidth = 100;
			this.DefaultHeight = 75;
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			Console.WriteLine ("Pop-up built.");
		}

	}
}


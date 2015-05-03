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

namespace ToDoList
{
	public partial class ConfirmCategoryDelete
	{
		private global::Gtk.VBox vbox;

		private global::Gtk.Label question;
		private global::Gtk.HSeparator hsep;

		private global::Gtk.HBox buttonContainer;

		private global::Gtk.Button yesButton;
		private global::Gtk.Button noButton;

		protected virtual void BuildConfirmCategoryDelete()
		{
			this.Name = "ConfirmCategoryDelete";
			this.Title = "Confirm Category Deletion";
			this.WindowPosition = global::Gtk.WindowPosition.Center;

			this.vbox = new global::Gtk.VBox ();
			this.vbox.Name = "vbox";

			this.question = new global::Gtk.Label ();
			this.question.Name = "question";

			this.vbox.Add (question);
			global::Gtk.Box.BoxChild ql = ((global::Gtk.Box.BoxChild)(this.vbox [this.question]));

			ql.Fill = false;
			ql.Expand = false;

			this.hsep = new global::Gtk.HSeparator ();
			this.hsep.Name = "hsep";

			this.vbox.Add (hsep);
			global::Gtk.Box.BoxChild hsvb = ((global::Gtk.Box.BoxChild)(this.vbox [this.hsep]));

			this.buttonContainer = new global::Gtk.HBox ();
			this.buttonContainer.Name = "buttonContainer";

			this.yesButton = new global::Gtk.Button ();
			this.yesButton.Name = "yesButton";
			this.yesButton.Label = "Yes";
			this.yesButton.Clicked += yesButton_Clicked;

			this.buttonContainer.Add (this.yesButton);
			global::Gtk.Box.BoxChild ybbc = ((global::Gtk.Box.BoxChild)(this.buttonContainer [this.yesButton]));

			this.noButton = new global::Gtk.Button ();
			this.noButton.Name = "noButton";
			this.noButton.Label = "No";
			this.noButton.Clicked += noButton_Clicked;
			this.buttonContainer.Add (noButton);
			global::Gtk.Box.BoxChild nbbc = ((global::Gtk.Box.BoxChild)(this.buttonContainer [this.noButton]));

			this.vbox.Add (this.buttonContainer);
			global::Gtk.Box.BoxChild vbhb = ((global::Gtk.Box.BoxChild)(this.vbox[this.buttonContainer]));

			this.Add (vbox);

			if((this.Child != null))
			{
				this.Child.ShowAll ();
			}
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		}
	}
}


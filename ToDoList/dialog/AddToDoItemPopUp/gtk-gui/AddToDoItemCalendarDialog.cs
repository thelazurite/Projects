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
using System.IO;
using Gtk;
using System.Reflection;

namespace ToDoList
{
	public partial class AddToDoItemCalendarDialog
	{
		private global::Gtk.VBox WindowContainer;
		private global::Gtk.HBox ButtonContainer;
		private global::Gtk.Label dateLabel;
		private global::Gtk.Calendar Calendar;
		private global::Gtk.Button AcceptButton;
		private global::Gtk.Button NowButton;
		private global::Gtk.Button CancelButton;


		protected virtual void BuildCalendarDialog() 
		{
			this.Name = "AddToDoItemCalendarDialog" + GlobalGuiVars.calendarDateType;

			Console.WriteLine (GlobalGuiVars.calendarDateType + " -> " + this.Name);
			if (GlobalGuiVars.calendarDateType == 1) {
				this.Title = "Pick start date";
			} else if (GlobalGuiVars.calendarDateType == 2) {
				this.Title = "Pick end date";
			}

			this.Resizable = false;

			this.WindowContainer = new VBox();

			this.Calendar = new Calendar();
			this.Calendar.Name = "Calendar";
			this.Calendar.DaySelected += CalendarItem_Selected;

			this.WindowContainer.Add (this.Calendar);
			global::Gtk.Box.BoxChild calWC = ((Gtk.Box.BoxChild)(this.WindowContainer [this.Calendar]));

			this.dateLabel = new Label ();
			this.dateLabel.Name = "dateLabel";
			this.dateLabel.Visible = false;

			this.WindowContainer.Add (this.dateLabel);
			global::Gtk.Box.BoxChild dlbc = ((Gtk.Box.BoxChild)(this.WindowContainer [this.dateLabel]));

			this.ButtonContainer = new HBox();

			this.AcceptButton = new Button ();
			this.AcceptButton.Name = "AcceptButton";
			this.AcceptButton.Label = "Ok";
			this.AcceptButton.Clicked += AcceptButton_Clicked;

			this.ButtonContainer.Add (this.AcceptButton);
			global::Gtk.Box.BoxChild abbc = ((Gtk.Box.BoxChild)(this.ButtonContainer [this.AcceptButton]));

			if (GlobalGuiVars.calendarDateType == 1) {
				this.NowButton = new Button ();
				this.NowButton.Name = "NowButton";
				this.NowButton.Label = "Current Time";
				this.NowButton.Clicked += NowButton_Clicked;

				this.ButtonContainer.Add (this.NowButton);
				global::Gtk.Box.BoxChild nbbc = ((Gtk.Box.BoxChild)(this.ButtonContainer [this.NowButton]));
			}

			this.CancelButton = new Button ();
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Label = "Cancel";
			this.CancelButton.Clicked += CancelButton_Clicked;

			this.ButtonContainer.Add (this.CancelButton);
			global::Gtk.Box.BoxChild cbbc = ((Gtk.Box.BoxChild)(this.ButtonContainer [this.CancelButton]));

			this.WindowContainer.Add (this.ButtonContainer);
			global::Gtk.Box.BoxChild wcbc = ((Gtk.Box.BoxChild)(this.WindowContainer [this.ButtonContainer]));

			this.Add (this.WindowContainer);

			if (this.Child != null) {
				this.Child.ShowAll ();
			}
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);

		}
	}
}


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
	public partial class AddToDoItemCalendarDialog : Gtk.Window
	{
		public AddToDoItemCalendarDialog () : base (Gtk.WindowType.Toplevel)
		{
			BuildCalendarDialog ();
			this.dateLabel.Hide ();
		}
		private void CalendarItem_Selected (object sender, EventArgs e)
		{
			if (GlobalGuiVars.calendarDateType == 1) 
			{
				GlobalGuiVars.TempStartDate = this.Calendar.Date;
			} 
			else if (GlobalGuiVars.calendarDateType == 2) 
			{
				GlobalGuiVars.TempFinishDate = this.Calendar.Date;
				Console.WriteLine ("Finish date is before the start date");
				dateLabel.Text = "Error: " + GlobalGuiVars.TempFinishDate + "\nIs before the start date";
			}
			if (GlobalGuiVars.TempStartDate > GlobalGuiVars.TempFinishDate)
			{

			} 
			GlobalGuiVars.dateToAdd = Convert.ToString(this.Calendar.Date);
			this.dateLabel.Text = GlobalGuiVars.dateToAdd;
			if (this.dateLabel.Visible.Equals (false)) {
				this.dateLabel.Visible = true;
				this.dateLabel.Show ();
			}
		}
		bool errorCheck;
		private void AcceptButton_Clicked (object sender, EventArgs e)
		{


			if (GlobalGuiVars.calendarDateType == 1) {
				errorCheck = true;
			} 
			else if (GlobalGuiVars.calendarDateType == 2) 
			{
				if (GlobalGuiVars.TempStartDate > GlobalGuiVars.TempFinishDate)
				{
					dateLabel.Text = "Error: " + GlobalGuiVars.TempFinishDate + "\nIs before the start date";
				} 
				else 
				{
					errorCheck = true;
				}
			}

			if (errorCheck == true)
			{
				GlobalGuiVars.calendarJustClosed = true;
				GlobalGuiVars.calendarOpen = false;
				this.Destroy ();
			} 
			else
			{
				dateLabel.Text = "Error: " + GlobalGuiVars.TempFinishDate + "\nIs before the start date";
			}
		}
		private void NowButton_Clicked (object sender, EventArgs e)
		{
			GlobalGuiVars.dateToAdd = Convert.ToString (System.DateTime.Today);
			this.Calendar.SelectMonth (Convert.ToUInt32 (DateTime.Today.Month) - 1, Convert.ToUInt32 (DateTime.Today.Year));
			this.Calendar.SelectDay (Convert.ToUInt32 (DateTime.Today.Day));
			this.dateLabel.Text = GlobalGuiVars.dateToAdd;
			this.dateLabel.Visible = true;
			this.dateLabel.Show ();

		}
		private void CancelButton_Clicked (object sender, EventArgs e)
		{
			this.Destroy ();
			GlobalGuiVars.calendarOpen = false;
			GlobalGuiVars.calendarDateType = 0;
		}
		private void OnDeleteEvent (object sender, DeleteEventArgs e)
		{
			GlobalGuiVars.calendarOpen = false;

		}
	}
}


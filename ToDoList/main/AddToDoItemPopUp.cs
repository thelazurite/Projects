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

/// <summary>
/// Add to do item pop up. currently unimplemented
/// </summary>
namespace ToDoList
{
	public partial class AddToDoItemPopUp : Gtk.Window
	{
		public string resultFile = System.IO.Path.GetTempFileName();
        public string resultDir = System.IO.Path.GetTempPath();

        public bool fileWritten = false;
        public bool formModified = false;
		
		public AddToDoItemPopUp () : base (Gtk.WindowType.Toplevel)
		{
			BuildToDoItemPopup ();
			
			Stetic.SteticInit.Initialize(this);
			this.DialogWidgetsContainerBox5.Hide ();
	
		}

		void checkForDateAddition()
		{
			while (GlobalGuiVars.calendarJustClosed == false) {
				if (GlobalGuiVars.n12 == false) 
                {
					Console.WriteLine ("Waiting for user input");
					GlobalGuiVars.n12 = true;
				}
			}
			while (GlobalGuiVars.calendarJustClosed == true) {
				if (GlobalGuiVars.calendarDateType == 1) {
					this.ItemStartDateValueContainer.Text = GlobalGuiVars.dateToAdd;
					GlobalGuiVars.dateToAdd = "";
					GlobalGuiVars.calendarDateType = 0;
					if(ItemStartDateValueContainer.Text != "")
					{
						this.DialogWidgetsContainerBox5.Show ();
					}
					GlobalGuiVars.calendarJustClosed = false;
					
				} else if (GlobalGuiVars.calendarDateType == 2) {
					this.ItemEndDateValueContainer.Text = GlobalGuiVars.dateToAdd;
					GlobalGuiVars.dateToAdd = "";
					GlobalGuiVars.calendarDateType = 0;
					GlobalGuiVars.calendarJustClosed = false;
				}
			}
		}

		protected void OkayButton_Clicked(object sender, EventArgs e)
		{
			Console.WriteLine ("Add.Notimplemented");
		}
		protected void CancelButton_Clicked(object sender, EventArgs e)
		{
			this.Destroy ();
		}
		private object locker = new object ();
		Exception closed = new Exception("Closed");
		public void StartButton_Clicked(object sender, EventArgs e)
		{
			if (GlobalGuiVars.calendarOpen == false) {
				GlobalGuiVars.calendarDateType = 1;
				GlobalGuiVars.calendarOpen = true;
				AddToDoItemCalendarDialog startCalendar = new AddToDoItemCalendarDialog ();
				startCalendar.Show ();
				startCalendar.WindowPosition = WindowPosition.Center;
				Thread calOpen = new Thread (new ThreadStart (checkForDateAddition));
				calOpen.Start ();
				lock (locker)
				{
					if (GlobalGuiVars.calendarOpen == false)
					{
						calOpen.Abort ();
						throw closed;
					} else {
						Console.WriteLine ("Running");
					}
				}
				Console.WriteLine ("unlocked");
			} else if (GlobalGuiVars.calendarOpen == true) {
				Console.WriteLine ("Already Open: " + GlobalGuiVars.calendarDateType);
			}


		}
		protected void EndButton_Clicked(object sender, EventArgs e)
		{
			if (GlobalGuiVars.calendarOpen == false) {
				GlobalGuiVars.calendarDateType = 2;
				GlobalGuiVars.calendarOpen = true;
				AddToDoItemCalendarDialog endCalendar = new AddToDoItemCalendarDialog ();
				endCalendar.Show ();
				Thread calOpen = new Thread (new ThreadStart (checkForDateAddition));
				calOpen.Start ();
			} else if (GlobalGuiVars.calendarOpen == true) {
				Console.WriteLine ("Already Open: " + GlobalGuiVars.calendarDateType);
			}
		}
		protected void OnDeleteEvent(object sender, DeleteEventArgs e)
		{
			this.Destroy();
		}
	}
}


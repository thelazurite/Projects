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
	public partial class AddToDoItemPopUp
	{
		private global::Gtk.UIManager UIManager;
		private global::Gdk.Pixbuf cal;
		private global::Gtk.ActionGroup actionGrp;
		private global::Gtk.ListStore priorityData;
		
		#region WindowContainer
		private global::Gtk.VBox WindowContainer;

			#region DialogWidgetsContainer(s)
			
				#region DW1
				private global::Gtk.HBox DialogWidgetsContainerBox1;
				private global::Gtk.Label ItemNameLabel;
				private global::Gtk.Entry ItemNameTextBox;
				#endregion
				
				#region DW2
				private global::Gtk.HBox DialogWidgetsContainerBox2;
				private global::Gtk.Label ItemDescriptionLabel;
				private global::Gtk.Entry ItemDescrtiptionTextBox;
				#endregion
				
				#region DW3
				private global::Gtk.HBox DialogWidgetsContainerBox3;
				private global::Gtk.Label ItemPriorityLabel;
				private global::Gtk.ComboBox ItemPriorityComboBox;
				#endregion
					
				#region DW4
				private global::Gtk.HBox DialogWidgetsContainerBox4;
				private global::Gtk.Label ItemStartDateLabel;
				private global::Gtk.Entry ItemStartDateValueContainer;
				private global::Gtk.Button ItemStartDateCalendarPicker;
				#endregion
				
				#region DW5
				private global::Gtk.HBox DialogWidgetsContainerBox5;
				private global::Gtk.Label ItemEndDateLabel;
				private global::Gtk.Entry ItemEndDateValueContainer;
				private global::Gtk.Button ItemEndDateCalendarPicker;
				#endregion
			
			#endregion
			
			#region DialogButtonsContainer
				private global::Gtk.HBox DialogButtonsContainer;
				private global::Gtk.Button OkayButton;
				private global::Gtk.Button CancelButton;
			#endregion
		#endregion



		#region DialogDiagram
		/*
		 * Dialog Contents:
		 * ==
 		 * Window Container
 		 * ==
		 * > DialogWidgetsContainerBox1
		 * > > ItemNameLabel
		 * > > ItemNameTextBox
		 * > DialogWidgetsContainerBox2
		 * > > ItemDescriptionLabel
		 * > > ItemDescriptionTextBox
		 * > DialogWidgetsContainerBox3
		 * > > ItemPriorityLabel
		 * > > ItemPriorityComboBox
		 * > DialogWidgetsContainerBox4
		 * > > ItemStartDateLabel
		 * > > ItemStartDateValueContainer < TextBox
		 * > > ItemStartDateCalendarPicker < Button: text: 
		 * ...					" ..."  Onclickevent( show calendar window );
		 * ...					Gdk.Pixbuf ( calendar.png ) ;
		 * > DialogWidgetsContainerBox5
		 * > > ItemEndDateLabel
		 * > > ItemEndDateValueContainer < TextBox
		 * > > ItemEndDateValueContainer < Button: 
		 * ==
		 * DialogButtonsContainer
		 * ==
		 * >  OkayButton
		 * >  CancelButton
		 */
		#endregion

		protected virtual void BuildToDoItemPopup()
		{

			//Assembly newassembly = Assembly.Load(global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, ".\\ImageLoader.dll"));
			//Console.WriteLine(newassembly.GetManifestResourceInfo("CircledCritical").ToString());
			//Stetic.SteticInit.Initialize(this);
			
			try 
			{
				this.cal = new global::Gdk.Pixbuf 
					(
						global::System.IO.Path.Combine 
						(
							global::System.AppDomain.CurrentDomain.BaseDirectory,
							".\\calendar_16.png"
						)
					);
			} 
			catch 
			{
				Console.WriteLine("NotFoundErr");
			}

			Image calendar = new Image(cal);
			Image calenb = new Image(cal);
			
			this.UIManager = new UIManager();

			this.actionGrp = new ActionGroup("Default");
			
			this.UIManager.InsertActionGroup (actionGrp, 0);
			this.AddAccelGroup (this.UIManager.AccelGroup);

			this.priorityData = new global::Gtk.ListStore (typeof(string), typeof(int));
			this.priorityData.AppendValues("High", 1);
			this.priorityData.AppendValues("Medium", 2);
			this.priorityData.AppendValues("low", 3);

            this.Name = "AddToDoItemPopUp";
            this.Title = "Add To-do Item";
			this.Resizable = false;

            Console.WriteLine("Window created: " + this.Title);

			this.WindowPosition = global::Gtk.WindowPosition.Center;

			
			#region WindowContainer
				this.WindowContainer = new global::Gtk.VBox();
				this.WindowContainer.Name = "WindowContainer";
				this.WindowContainer.Spacing = 6;
				
				#region DialogWidgetsContainers
							
					#region DW1
					this.DialogWidgetsContainerBox1 = new Gtk.HBox();
					
					this.ItemNameLabel = new Gtk.Label();
					this.ItemNameLabel.Name = "ItemNameLabel";
					this.ItemNameLabel.Text = "Name: ";
					
					this.ItemNameTextBox = new Gtk.Entry();
					this.ItemNameTextBox.Name = "ItemNameTextBox";
//					this.ItemNameTextBox.Changed += ItemNameTextBox_Changed;
					
					this.DialogWidgetsContainerBox1.Add(this.ItemNameLabel);
					global::Gtk.Box.BoxChild dwinl = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox1[this.ItemNameLabel]));
					
					this.DialogWidgetsContainerBox1.Add(this.ItemNameTextBox);
					global::Gtk.Box.BoxChild dwint = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox1[this.ItemNameTextBox]));
					
					#endregion

					#region DW2
					this.DialogWidgetsContainerBox2 = new Gtk.HBox();
					
					this.ItemDescriptionLabel = new Gtk.Label();
					this.ItemDescriptionLabel.Name = "ItemDescriptionLabel";
					this.ItemDescriptionLabel.Text = "Description: ";
					
					this.ItemDescrtiptionTextBox = new Gtk.Entry();
					this.ItemDescrtiptionTextBox.Name = "ItemDescriptionTextBox";
//					this.ItemDescrtiptionTextBox.Changed += ItemDescriptionTextBox_Changed;
					
					this.DialogWidgetsContainerBox2.Add(this.ItemDescriptionLabel);
					global::Gtk.Box.BoxChild dwdl = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox2[this.ItemDescriptionLabel]));
					
					this.DialogWidgetsContainerBox2.Add(this.ItemDescrtiptionTextBox);
					global::Gtk.Box.BoxChild dwdtb = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox2[this.ItemDescrtiptionTextBox]));
					#endregion
					
					#region DW3
					this.DialogWidgetsContainerBox3 = new Gtk.HBox();
					
					this.ItemPriorityLabel = new Gtk.Label();
					this.ItemPriorityLabel.Name = "ItemPriorityLabel";
					this.ItemPriorityLabel.Text = "Priority: ";
					
					
					this.ItemPriorityComboBox = new Gtk.ComboBox();
					this.ItemPriorityComboBox.Name = "ItemPriorityComboBox";

					Gtk.CellRendererText priorityCell = new Gtk.CellRendererText();
					this.ItemPriorityComboBox.Model = this.priorityData;
					this.ItemPriorityComboBox.PackStart(priorityCell, false);
					this.ItemPriorityComboBox.AddAttribute (priorityCell, "text", 0);
							
					this.DialogWidgetsContainerBox3.Add(this.ItemPriorityLabel);
					global::Gtk.Box.BoxChild dwipl = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox3[this.ItemPriorityLabel]));
					
					this.DialogWidgetsContainerBox3.Add(this.ItemPriorityComboBox);
					global::Gtk.Box.BoxChild dwipcb = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox3[this.ItemPriorityComboBox]));
					#endregion
					
					#region DW4
					this.DialogWidgetsContainerBox4 = new Gtk.HBox();
					
					this.ItemStartDateLabel = new Label();
					this.ItemStartDateLabel.Name = "ItemStartDateLabel";
					this.ItemStartDateLabel.Text = "Start Date: ";
					
					this.ItemStartDateValueContainer = new Entry();
					this.ItemStartDateValueContainer.Name = "ItemStartDateValueContainer";
				this.ItemStartDateValueContainer.IsEditable = false;
					
					this.ItemStartDateCalendarPicker = new Button();
					this.ItemStartDateCalendarPicker.Add(calendar);
					this.ItemStartDateCalendarPicker.Clicked += StartButton_Clicked;
					
					this.DialogWidgetsContainerBox4.Add(this.ItemStartDateLabel);
					global::Gtk.Box.BoxChild dwisdl = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox4[this.ItemStartDateLabel]));
				
					this.DialogWidgetsContainerBox4.Add(this.ItemStartDateValueContainer);
					global::Gtk.Box.BoxChild dwisvc = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox4[this.ItemStartDateValueContainer]));
					
					this.DialogWidgetsContainerBox4.Add(this.ItemStartDateCalendarPicker);
					global::Gtk.Box.BoxChild dwiscp = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox4[this.ItemStartDateCalendarPicker]));
					#endregion
					
					#region DW5
					this.DialogWidgetsContainerBox5 = new Gtk.HBox();
					
					this.ItemEndDateLabel = new Label();
					this.ItemEndDateLabel.Name = "ItemEndDateLabel";
					this.ItemEndDateLabel.Text = "End Date";
					
					this.ItemEndDateValueContainer = new Entry();
					this.ItemEndDateValueContainer.Name = "ItemEndDateValueContainer";
					this.ItemEndDateValueContainer.IsEditable = false;
					
					this.ItemEndDateCalendarPicker = new Button();
					this.ItemEndDateCalendarPicker.Add(calenb);
					this.ItemEndDateCalendarPicker.Clicked += EndButton_Clicked;
					
					this.DialogWidgetsContainerBox5.Add(this.ItemEndDateLabel);
					global::Gtk.Box.BoxChild dwiedl = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox5[this.ItemEndDateLabel]));
					
					this.DialogWidgetsContainerBox5.Add(this.ItemEndDateValueContainer);
					global::Gtk.Box.BoxChild dwievc = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox5[this.ItemEndDateValueContainer]));
					
					this.DialogWidgetsContainerBox5.Add(this.ItemEndDateCalendarPicker);
					global::Gtk.Box.BoxChild dwiecp = ((global::Gtk.Box.BoxChild)(this.DialogWidgetsContainerBox5[this.ItemEndDateCalendarPicker]));
					
					#endregion
					
				#endregion
				
				#region DWCLinks
				this.WindowContainer.Add(DialogWidgetsContainerBox1);
				global::Gtk.Box.BoxChild link1 = ((Gtk.Box.BoxChild)(this.WindowContainer[this.DialogWidgetsContainerBox1]));
				                                  
				this.WindowContainer.Add(DialogWidgetsContainerBox2);
				global::Gtk.Box.BoxChild link2 = ((Gtk.Box.BoxChild)(this.WindowContainer[this.DialogWidgetsContainerBox2]));
				
				this.WindowContainer.Add(DialogWidgetsContainerBox3);
				global::Gtk.Box.BoxChild link3 = ((Gtk.Box.BoxChild)(this.WindowContainer[this.DialogWidgetsContainerBox3]));
				
				this.WindowContainer.Add(DialogWidgetsContainerBox4);
				global::Gtk.Box.BoxChild link4 = ((Gtk.Box.BoxChild)(this.WindowContainer[this.DialogWidgetsContainerBox4]));
				
				this.WindowContainer.Add(DialogWidgetsContainerBox5);
				global::Gtk.Box.BoxChild link5 = ((Gtk.Box.BoxChild)(this.WindowContainer[this.DialogWidgetsContainerBox5]));
				#endregion
				
				#region DialogButtonsContainer
				this.DialogButtonsContainer = new Gtk.HBox();
				this.DialogButtonsContainer.Name = "DialogButtonsContainer";
				
				this.OkayButton = new Button();
				this.OkayButton.Name = "OkayButton";
				this.OkayButton.Label = "Add";
				this.OkayButton.Clicked += OkayButton_Clicked;
				
				this.CancelButton = new Button();
				this.CancelButton.Name = "CancelButton";
				this.CancelButton.Label = "Cancel";
				this.CancelButton.Clicked += CancelButton_Clicked;
				
				this.DialogButtonsContainer.Add(this.OkayButton);
				global::Gtk.Box.BoxChild dbcob = ((global::Gtk.Box.BoxChild)(this.DialogButtonsContainer[this.OkayButton]));
				
				this.DialogButtonsContainer.Add(this.CancelButton);
				global::Gtk.Box.BoxChild dbccb = ((global::Gtk.Box.BoxChild)(this.DialogButtonsContainer[this.CancelButton]));
				#endregion
				
				this.WindowContainer.Add(this.DialogButtonsContainer);
				global::Gtk.Box.BoxChild wcdbc = ((global::Gtk.Box.BoxChild)(this.WindowContainer[this.DialogButtonsContainer]));
			#endregion
			
			this.Add(this.WindowContainer);
			

			this.WindowPosition = global::Gtk.WindowPosition.Center;

			if((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Show();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
			
			Console.WriteLine("GUI Built");
			
		}
	}
}


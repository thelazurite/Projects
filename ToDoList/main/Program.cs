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

using System.Windows.Forms;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Resources;
using System.Globalization;
using System.Reflection;
using System.Collections.Concurrent;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Gtk;

namespace ToDoList
{
	/// <summary>
	/// Main class. Partialy implemented. Very Buggy.
	/// </summary>

	class MainClass
	{
		private global::Gtk.UIManager UIManager;

		private global::Gtk.VBox windowContainer;

		private global::Gtk.Toolbar catToolbar;
		private global::Gtk.Toolbar todoToolbar;
		
		private global::Gtk.HPaned WindowPane;

		#region notimplemented
		/*
		 * TODO: CREATE and IMPLEMENT the unused ITEMS
		 * 
		 * private global::Gtk.Menu editMenu; //quick access to editing functions
		 */
		#endregion

		#region menubar
		private global::Gtk.MenuBar mainMenu;
		private global::Gtk.Menu fileMenu;
		
		private global::Gtk.MenuItem fileMenuItem;

		private global::Gtk.MenuItem fileExitMenuItem;
		private global::Gtk.MenuItem fileNewMenuItem;
		private global::Gtk.HSeparator hsepfilemenu;
		private global::Gtk.MenuItem fileOpenMenuItem;
		private global::Gtk.MenuItem fileSaveMenuItem;
		
		private global::Gtk.Menu viewMenu;
		
		private global::Gtk.MenuItem viewMenuItem;
		private global::Gtk.MenuItem viewToggleMenuItem;
		#endregion

		#region toolbar_actions
		//<toolbar>
		private global::Gtk.Action saveAction;
		private global::Gtk.Action openAction;
		private global::Gtk.Action newTabAction;
		//<separator />
		private global::Gtk.Action addCategoryAction;
		private global::Gtk.Action removeCategoryAction;
		//<separator />
		private global::Gtk.Action addToDoItemAction;
		private global::Gtk.Action removeToDoItemAction;
		private global::Gtk.Action modifyToDoItemAction;
		//</toolbar>
		#endregion
		
        private global::Gtk.VBox categorySidebar;
		private global::Gtk.VBox categoryExpanderContainer;
        private global::Gtk.Calendar calendar;

		private global::Gtk.Label CalendarExpanderLabel;

        private global::Gtk.Expander CategoryExpander;

		private global::Gtk.Expander CalendarExpander;

        private global::Gtk.ScrolledWindow CategoryContainer;
        private global::Gtk.TreeView categoryItems;

        private global::Gtk.TreeViewColumn categoryItemID;
        private global::Gtk.CellRendererText categoryItemIDCell;

        private global::Gtk.TreeViewColumn categoryItemName;
        private global::Gtk.CellRendererText categoryItemNameCell;

        private global::Gtk.TreeViewColumn categoryItemDescription;
        private global::Gtk.CellRendererText categoryItemDescriptionCell;

        private global::Gtk.TreeViewColumn categoryItemToggle;
        private global::Gtk.CellRendererToggle categoryItemToggleCell; 

        private global::Gtk.ListStore categoryItemsStore;

        private global::Gtk.Label CategoryContainerLabel;

		private global::Gtk.VBox ToDoTableListContainer;

        private global::Gtk.ScrolledWindow GtkScrolledWindoactionGrp;

        private global::Gtk.TreeView todoListItems;

        private global::Gtk.TreeViewColumn todoItemId;
        private global::Gtk.TreeViewColumn todoItemName;
      //  private global::Gtk.TreeViewColumn todoItemDescription;
        private global::Gtk.TreeViewColumn todoItemPriority;
		private global::Gtk.TreeViewColumn todoItemCategory;
        private global::Gtk.TreeViewColumn todoItemStartDate;
        private global::Gtk.TreeViewColumn todoItemFinishDate;

        private global::Gtk.ProgressBar fileActionProgBar;

		private global::Gtk.Adjustment hadjustment;
		private global::Gtk.Adjustment vadjustment;
		private global::Gtk.Adjustment hadjustment1;
		private global::Gtk.Adjustment vadjustment1;
		private global::Gtk.Window window;

		public List<ToDoItem> ToDoStore;

		public static void Main ()
		{
		
			Console.WriteLine ("Application Initializing");

			try{
				
				Gtk.Application.Init ();

				GlobalGuiVars.TotalCategories = 0;

				new MainClass ();

				Console.WriteLine ("\nApplication running");

				Gtk.Application.Run ();


			} 
			catch (DllNotFoundException dnfe)
			{
				if(!dnfe.Message.EndsWith(".dll"))
                {
                    System.Windows.Forms.MessageBox.Show ("For this application to run you must have" +
                        " the GTK runtime installed.\nError message provided: \n\n" + dnfe.Message + ".", "Missing Libraries", MessageBoxButtons.OK, MessageBoxIcon.Error); //fall back to winforms
                }
                else if (dnfe.Message.EndsWith(".dll")) 
                {
                    System.Windows.Forms.MessageBox.Show("For this application to run you must have" +
                        " the GTK runtime installed.\nDLL Missing: " + dnfe.Message + ".", "Missing Libraries", MessageBoxButtons.OK, MessageBoxIcon.Error); //fall back to winforms
                }
			}
		}

		private void RenderToDoItemId(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			try{
				ToDoItem item = (ToDoItem)model.GetValue (iter, 0);
				if (item != null) 
				{
					(cell as Gtk.CellRendererText).Text = item.ID;
				}
				else 
				{
					Console.WriteLine ("The ID value was null");
					item.ID = "no value";
					(cell as Gtk.CellRendererText).Text = (string)item.ID;
				}
			} 
			catch (System.NullReferenceException) 
			{
				Console.WriteLine ("Null reference has occured (id)");
			}
		}

		private void RenderToDoItemName(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			try
			{
				ToDoItem item = (ToDoItem)model.GetValue (iter, 0);
				
				if (item != null) 
				{
					(cell as Gtk.CellRendererText).Text = item.Name;
				}
				else 
				{
					Console.WriteLine ("The name value was null");
					item.Name = "no value";
					(cell as Gtk.CellRendererText).Text = item.Name;
				}
			}
			catch (System.NullReferenceException)
			{
				Console.WriteLine ("Null reference has occured (name)");
			}
		}

		private void RenderToDoItemCategory(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			try
			{
				ToDoItem item = (ToDoItem)model.GetValue (iter, 0);
				if (item != null)
				{
					(cell as Gtk.CellRendererText).Text = item.Category;
				} 
				else 
				{
					Console.WriteLine ("The category value was null");
					item.Category = "no value";
					(cell as Gtk.CellRendererText).Text = item.Category;
				}
			}
			catch (System.NullReferenceException) {
				Console.WriteLine ("Null reference has occured (category)");
			}
		}

		private void RenderToDoItemPriority(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			try{
				ToDoItem item = (ToDoItem)model.GetValue (iter, 0);
				if (item != null) {
					(cell as Gtk.CellRendererText).Text = item.Priority;
				} 
				else 
				{
					Console.WriteLine ("The Priority value was null");
					item.Priority = "no value";
					(cell as Gtk.CellRendererText).Text = item.Priority;
				}
			} 
			catch (System.NullReferenceException) 
			{
				Console.WriteLine ("Null reference has occured (priority)");
			}
		}

		private void RenderToDoItemStart(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			try 
			{
				ToDoItem item = (ToDoItem)model.GetValue (iter, 0);
				
				if (item != null)
				{
					(cell as Gtk.CellRendererText).Text = item.Start;
					
					Console.WriteLine(item.Start);
					
				} else
				{
					Console.WriteLine ("The start date value was null");
					item.Start = "no value";
					(cell as Gtk.CellRendererText).Text = item.Start;
					Console.WriteLine(item.Start);
				}
			}
			catch (System.NullReferenceException) 
			{
				Console.WriteLine ("Null reference has occured (start)");
			}
		}

		private void RenderToDoItemFinish(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			try{
				ToDoItem item = (ToDoItem)model.GetValue (iter, 0);
				if (item != null) 
				{
					(cell as Gtk.CellRendererText).Text = item.Finish;
				}
				else 
				{
					Console.WriteLine ("The finish date value was null");
					item.Finish = "no value";
					(cell as Gtk.CellRendererText).Text = item.Finish;
				}
			} 
			catch (System.NullReferenceException) 
			{
				Console.WriteLine ("Null reference has occured (finish)");
			}
		}
		
		public MainClass ()
		{

        	
			window = new Gtk.Window("To-Do list");

			Stetic.SteticInit.Initialize(window);

			ToDoStore = new List<ToDoItem>();

			UIManager = new global::Gtk.UIManager ();
			global::Gtk.ActionGroup actionGrp = new global::Gtk.ActionGroup ("Default");

			mainMenu = new global::Gtk.MenuBar();
			mainMenu.Name = "mainMenu";

			fileMenu = new global::Gtk.Menu ();

			fileMenuItem = new global::Gtk.MenuItem ("File");
			fileMenuItem.Name = "fileMenuItem";

			fileMenuItem.Submenu = fileMenu;

			fileNewMenuItem = new global::Gtk.MenuItem ("New");
			fileNewMenuItem.Activated += fileNewMenuItem_Clicked;
			fileMenu.Append (fileNewMenuItem);

			fileOpenMenuItem = new global::Gtk.MenuItem ("Open");
			fileMenu.Append (fileOpenMenuItem);

			fileSaveMenuItem = new global::Gtk.MenuItem ("Save");
			fileMenu.Append (fileSaveMenuItem);

			hsepfilemenu = new global::Gtk.HSeparator ();
			fileMenu.Append (hsepfilemenu);

			fileExitMenuItem = new global::Gtk.MenuItem ("Exit");
			fileExitMenuItem.Activated += fileExitMenuItem_Clicked;
			fileMenu.Append (fileExitMenuItem);

			viewMenu = new global::Gtk.Menu ();
			viewMenu.Name = "viewMenu";
			
			viewMenuItem = new Gtk.MenuItem ("View");
			viewMenuItem.Name = "viewMenuItem";
			viewMenuItem.Submenu = viewMenu;
			
			viewToggleMenuItem = new Gtk.MenuItem ("Toggle single window mode");
			viewToggleMenuItem.Name = "viewToggleMenuItem";
			viewMenu.Append(viewToggleMenuItem);
			
			WindowPane = new Gtk.HPaned();

			mainMenu.Append (fileMenuItem);
			mainMenu.Append (viewMenuItem);

			//<Toolbar>
			saveAction = new global::Gtk.Action ("saveAction",
				"Save file", "Save file", "gtk-save");
			actionGrp.Add (saveAction, null);

			openAction = new global::Gtk.Action ("openAction",
				"Open File", "Open file", "gtk-open");
			actionGrp.Add (openAction, null);

			newTabAction = new global::Gtk.Action ("newTabAction",
				"New", "New Application tab", "gtk-new");
			actionGrp.Add (newTabAction, null);

			//<seperator />
			addCategoryAction = new global::Gtk.Action ("addCategoryAction",
				"New Category", "Create a new category", "CircledPlus");
			addCategoryAction.Activated += addCategory_Clicked;
			actionGrp.Add (addCategoryAction, null);

			removeCategoryAction = new global::Gtk.Action ("removeCategoryAction",
				"Remove Category", "Remove the selected category", "CircledMinus");
			removeCategoryAction.Activated += deleteCategory_Clicked;
			actionGrp.Add (removeCategoryAction, null);

			//<separator />
			addToDoItemAction = new global::Gtk.Action ("addToDoItemAction",
				"Add To-Do Item", "Add a new To-Do Item", "CircledPlus");
			addToDoItemAction.Activated += addTodoItem_Clicked;
			actionGrp.Add (addToDoItemAction, null);

			modifyToDoItemAction = new global::Gtk.Action ("modifyToDoItemAction", 
				"Modify Item", "Modify To-Do Item", "gtk-edit");
			actionGrp.Add (modifyToDoItemAction, null);

			removeToDoItemAction = new global::Gtk.Action ("removeToDoItemAction",
				"Remove Item", "Remove To-Do Item", "CircledMinus");
			actionGrp.Add (removeToDoItemAction, null);
			//</ toolbar>

			UIManager.InsertActionGroup (actionGrp, 0);

			window.AddAccelGroup (UIManager.AccelGroup);

			window.Name = "TodoList";
			window.Title = "To-do List";
            Console.WriteLine("Window created: " + window.Title);

			window.WindowPosition = global::Gtk.WindowPosition.Center;
			windowContainer = new global::Gtk.VBox ();
			windowContainer.Name = "mainwindowcontainer";
			windowContainer.Spacing = 3;
			//	windowContainer.Spacing = 6;

            categorySidebar = new global::Gtk.VBox();
            categorySidebar.Name = "categorySidebar";
            categorySidebar.Spacing = 6;
			categorySidebar.ResizeMode = Gtk.ResizeMode.Parent;

			UIManager.AddUiFromString (
				"<ui>" +
					"<toolbar name='toolbar2'>" +
						"<toolitem name='saveAction' action='saveAction'/>" +
						"<toolitem name='openAction' action='openAction'/>" +
						"<toolitem name='newTabAction' action='newTabAction'/>" +
					"</toolbar>" +
				"</ui>"
			);//xml used to create the Toolbar NOT USED

			UIManager.AddUiFromString (
				"<ui>" +
				"<toolbar name='categoryToolbar'>" +
				"<toolitem name='addCategoryAction' action='addCategoryAction'/>" +
				"<toolitem name='removeCategoryAction' action='removeCategoryAction'/>" +
				"</toolbar>" +
				"</ui>");
			catToolbar = ((global::Gtk.Toolbar)(UIManager.GetWidget ("/categoryToolbar")));
			catToolbar.Events = ((global::Gdk.EventMask)(8992));
			catToolbar.Name = "catToolbar";
			catToolbar.ShowArrow = false;

			windowContainer.Add(mainMenu);
			global::Gtk.Box.BoxChild mmmw =  ((global::Gtk.Box.BoxChild)(windowContainer[mainMenu]));
			mmmw.Fill = false;
			mmmw.Expand = false;

			CalendarExpander = new global::Gtk.Expander(null);
			CalendarExpander.CanFocus = true;
			CalendarExpander.Name = "CategoryExpander";
			CalendarExpander.Expanded = true;

            calendar = new global::Gtk.Calendar();
            calendar.CanFocus = true;
            calendar.Name = "calendar";
            calendar.DisplayOptions = ((global::Gtk.CalendarDisplayOptions)(35));

			CalendarExpander.Add(calendar);
			CalendarExpanderLabel = new global::Gtk.Label();
			CalendarExpanderLabel.Name = "CalendarExpanderLabel";
			CalendarExpanderLabel.LabelProp = "Calendar";
			CalendarExpanderLabel.UseUnderline = true;
			CalendarExpander.LabelWidget = CalendarExpanderLabel;

			categorySidebar.Add(CalendarExpander);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(categorySidebar[CalendarExpander]));
            w3.Position = 0;
            w3.Expand = false;
            w3.Fill = false;

            CategoryExpander = new global::Gtk.Expander(null);
            CategoryExpander.CanFocus = true;
            CategoryExpander.Name = "CategoryExpander";
            CategoryExpander.Expanded = true;

			categoryExpanderContainer = new global::Gtk.VBox();
			categoryExpanderContainer.Name = "categoryExpanderContainer";
			categoryExpanderContainer.Spacing = 3;

			categoryExpanderContainer.Add (catToolbar);
			global::Gtk.Box.BoxChild ctvb = ((global::Gtk.Box.BoxChild)(categoryExpanderContainer [catToolbar]));
			ctvb.Expand = false;
			ctvb.Fill = false;

			CategoryContainer = new global::Gtk.ScrolledWindow();
            CategoryContainer.Name = "CategoryContainer";
            CategoryContainer.ShadowType = Gtk.ShadowType.EtchedIn;
            CategoryContainer.SetScrollAdjustments(hadjustment, vadjustment);
            CategoryContainer.HScrollbar.Visible = true;
            CategoryContainer.VScrollbar.Visible = true;

            categoryItems = new global::Gtk.TreeView();
            categoryItems.CanFocus = true;
            categoryItems.Name = "categoryListItems";
			categoryExpanderContainer.Add (categoryItems);
			global::Gtk.Box.BoxChild cicec = ((global::Gtk.Box.BoxChild)
				(categoryExpanderContainer [categoryItems]));
			//CategoryContainer.Add (categoryExpanderContainer);

            CategoryExpander.Add(CategoryContainer);
            CategoryContainerLabel = new global::Gtk.Label();
            CategoryContainerLabel.Name = "CategoryContainerLabel";
            CategoryContainer.AddWithViewport(categoryExpanderContainer);
            CategoryContainer.HscrollbarPolicy = Gtk.PolicyType.Always;

            CategoryContainer.VscrollbarPolicy = Gtk.PolicyType.Always;

            CategoryContainerLabel.LabelProp = "Categories";
            CategoryContainerLabel.UseUnderline = true;
            CategoryExpander.LabelWidget = CategoryContainerLabel;

            categorySidebar.Add(CategoryExpander);
            global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(categorySidebar[CategoryExpander]));
            w6.Position = 1;

            WindowPane.Add(categorySidebar);

            global::Gtk.Paned.PanedChild actionGrp8 = ((global::Gtk.Paned.PanedChild)(WindowPane[categorySidebar]));

			ToDoTableListContainer = new global::Gtk.VBox();
            ToDoTableListContainer.Name = "ToDoTableListContainer";
			ToDoTableListContainer.Spacing = 3;

			UIManager.AddUiFromString (
				"<ui>" +
				"<toolbar name='todoToolbar'>" +
				"<toolitem name='addToDoItemAction' action='addToDoItemAction'/>" +
				//"<toolitem name='modifyToDoItemAction' action='modifyToDoItemAction'/>" +
				"<toolitem name='removeToDoItemAction' action='removeToDoItemAction'/>" +
				"</toolbar>" +
				"</ui>");

			todoToolbar = ((global::Gtk.Toolbar)(UIManager.GetWidget ("/todoToolbar")));
			todoToolbar.Events = ((global::Gdk.EventMask)(8992));
			todoToolbar.Name = "todoToolbar";
			todoToolbar.ShowArrow = false;

			ToDoTableListContainer.Add (todoToolbar);
			global::Gtk.Box.BoxChild tdtbvb = ((global::Gtk.Box.BoxChild)(ToDoTableListContainer [todoToolbar]));
			tdtbvb.Fill = false;
			tdtbvb.Expand = false;

            GtkScrolledWindoactionGrp = new global::Gtk.ScrolledWindow();
            GtkScrolledWindoactionGrp.AddWithViewport(todoListItems);
            GtkScrolledWindoactionGrp.Name = "GtkScrolledWindoactionGrp";
            GtkScrolledWindoactionGrp.ShadowType = ((global::Gtk.ShadowType)(1));
			GtkScrolledWindoactionGrp.SetScrollAdjustments (hadjustment1, vadjustment1);
			GtkScrolledWindoactionGrp.VscrollbarPolicy = Gtk.PolicyType.Always;
			GtkScrolledWindoactionGrp.HscrollbarPolicy = Gtk.PolicyType.Always;


            Console.Write("Gui widgets loaded.");



			#region categoryTable_properties
            categoryItemID = new global::Gtk.TreeViewColumn();
            categoryItemID.Title = "ID";
			categoryItemID.Resizable = true;
			categoryItemID.Visible = false;
            categoryItemIDCell = new global::Gtk.CellRendererText();
			
			categoryItemID.PackStart(categoryItemIDCell, true);

            
            categoryItemName = new global::Gtk.TreeViewColumn();
            categoryItemName.Title = "Category";
			categoryItemName.Resizable = true;
            categoryItemNameCell = new global::Gtk.CellRendererText();
			categoryItemNameCell.Editable = true;
			categoryItemNameCell.Edited += categoryItemNameCell_Edited;
			
            categoryItemName.PackStart(categoryItemNameCell, true);


            categoryItemDescription = new global::Gtk.TreeViewColumn();
            categoryItemDescription.Title = "Description";
			categoryItemDescription.Resizable = true;
            categoryItemDescriptionCell = new global::Gtk.CellRendererText();
			categoryItemDescriptionCell.Editable = true;
			categoryItemDescriptionCell.Edited += categoryItemDescriptionCell_Edited;

			categoryItemDescription.PackStart (categoryItemDescriptionCell, true);


			categoryItemToggle = new global::Gtk.TreeViewColumn ();
			categoryItemToggle.Title = "";
			categoryItemToggleCell = new global::Gtk.CellRendererToggle ();
			categoryItemToggle.PackStart (categoryItemToggleCell, true);

			categoryItemToggleCell.Activatable = true;
			categoryItemToggleCell.Toggled += categoryItemToggleCell_Toggled;

            categoryItems.AppendColumn(categoryItemID);
            categoryItems.AppendColumn(categoryItemName);
            categoryItems.AppendColumn(categoryItemDescription);
			categoryItems.AppendColumn (categoryItemToggle);
			
			categoryItemID.AddAttribute(categoryItemIDCell, "text", 0);
			categoryItemName.AddAttribute(categoryItemNameCell, "text", 1);
			categoryItemDescription.AddAttribute(categoryItemDescriptionCell, "text", 2);
			categoryItemToggle.AddAttribute(categoryItemToggleCell, "active", 3);

			categoryItemsStore = new global::Gtk.ListStore(typeof(string), typeof(string), typeof(string), typeof(bool));

            categoryItems.Model = categoryItemsStore;
			#endregion

            Console.WriteLine("\nData tables created");

            fileActionProgBar = new global::Gtk.ProgressBar();
			fileActionProgBar.Name = "fileActionProgBar";

            ToDoTableListContainer.Add(fileActionProgBar);

            global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(ToDoTableListContainer[fileActionProgBar]));
            w21.Position = 1;
            w21.Expand = false;
            w21.Fill = false;

			#region todoTable_properties
			todoListItems = new global::Gtk.TreeView();
			GtkScrolledWindoactionGrp.Add(todoListItems);

			ToDoTableListContainer.Add(GtkScrolledWindoactionGrp);
			global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(ToDoTableListContainer[GtkScrolledWindoactionGrp]));

			todoItemId = new global::Gtk.TreeViewColumn();
			todoItemId.Title = "ID";
			Gtk.CellRendererText todoIDCell = new Gtk.CellRendererText ();
			todoItemId.PackStart(todoIDCell, true);
			todoItemId.SetCellDataFunc(todoIDCell, new Gtk.TreeCellDataFunc(RenderToDoItemId));

			todoItemId.Visible = false;

			todoItemName = new global::Gtk.TreeViewColumn();
			todoItemName.Title = "Title";
			todoItemName.Resizable = true;
			Gtk.CellRendererText todoNameCell = new Gtk.CellRendererText ();
			todoItemName.PackStart(todoNameCell, true);
			todoItemName.SetCellDataFunc(todoNameCell, new Gtk.TreeCellDataFunc (RenderToDoItemName));

			todoItemCategory = new global::Gtk.TreeViewColumn();
			todoItemCategory.Title = "Category";
			todoItemCategory.Resizable = true;
			Gtk.CellRendererText todoCategoryCell = new Gtk.CellRendererText ();
			todoItemCategory.PackStart(todoCategoryCell, true);
			todoItemCategory.SetCellDataFunc(todoCategoryCell, new Gtk.TreeCellDataFunc (RenderToDoItemCategory));

			todoItemPriority = new global::Gtk.TreeViewColumn();
			todoItemPriority.Title = "Priority";
			todoItemPriority.Resizable = true;
			Gtk.CellRendererText todoPriorityCell = new Gtk.CellRendererText ();
			todoItemPriority.PackStart(todoPriorityCell, true);
			todoItemPriority.SetCellDataFunc(todoPriorityCell, new Gtk.TreeCellDataFunc (RenderToDoItemPriority));

			todoItemStartDate = new global::Gtk.TreeViewColumn();
			todoItemStartDate.Title = "Start";
			todoItemStartDate.Resizable = true;
			Gtk.CellRendererText todoStartCell = new Gtk.CellRendererText ();
			todoItemStartDate.PackStart(todoStartCell, true);
			todoItemStartDate.SetCellDataFunc(todoStartCell, new Gtk.TreeCellDataFunc (RenderToDoItemStart));

			todoItemFinishDate = new global::Gtk.TreeViewColumn();
			todoItemFinishDate.Title = "Finish";
			todoItemFinishDate.Resizable = true;
			CellRendererText todoFinishCell = new CellRendererText();
			todoItemFinishDate.PackStart(todoFinishCell, true);
			todoItemFinishDate.SetCellDataFunc(todoFinishCell, new Gtk.TreeCellDataFunc(RenderToDoItemFinish));

			GlobalGuiVars.todoListItemsStore = new global::Gtk.ListStore(typeof(ToDoItem));

			todoListItems.Model = GlobalGuiVars.todoListItemsStore;

			todoListItems.AppendColumn(todoItemId);
			todoListItems.AppendColumn(todoItemName);
			todoListItems.AppendColumn(todoItemCategory);
			todoListItems.AppendColumn(todoItemPriority);
			todoListItems.AppendColumn(todoItemStartDate);
			todoListItems.AppendColumn(todoItemFinishDate);

			#endregion

			WindowPane.Add (ToDoTableListContainer);
			global::Gtk.Paned.PanedChild w22 = ((global::Gtk.Paned.PanedChild)(WindowPane[ToDoTableListContainer]));

			windowContainer.Add (WindowPane);
			global::Gtk.Box.BoxChild mwcbc = ((global::Gtk.Box.BoxChild)(windowContainer[WindowPane]));
			
			window.Add (windowContainer);

            window.SetSizeRequest(1078,739);
            window.DeleteEvent += new global::Gtk.DeleteEventHandler(OnDeleteEvent);
            Console.Write("GUI built\n");

			window.ShowAll ();
			fileActionProgBar.Visible = false;
		}
		void addCategory_Clicked(object sender, EventArgs e)
		{

			if (GlobalGuiVars.AddCategoryOpened == false)
			{
				AddCategoryPopUp acpu = new AddCategoryPopUp();
				acpu.Show();
				Thread cfca = new Thread(new ThreadStart(checkForCategoryAddition));
				cfca.Start();
				GlobalGuiVars.AddCategoryOpened = true;
				GuiWorker.Worker(2, true);

			}
			else
			{
				Console.WriteLine("already open");
			}

		}

		void checkForCategoryAddition()
		{

			while (GlobalGuiVars.AddCategoryPopUp_JustClosedSecondPhase == false)
			{
				if (GlobalGuiVars.n11 == false)
				{
					Console.WriteLine("Waiting for input...");
					GlobalGuiVars.n11 = true;
				}

			}
			while (GlobalGuiVars.AddCategoryPopUp_JustClosedSecondPhase == true)
			{
				GlobalGuiVars.hasBeenGenerated++;
				Console.WriteLine (GlobalGuiVars.hasBeenGenerated);
				if (GlobalGuiVars.hasBeenGenerated == 1) {

					categoryItemsStore.AppendValues (GlobalGuiVars.tempCatIdStore,
						GlobalGuiVars.tempCatNameStore,
						GlobalGuiVars.tempCatDescriptionStore, true
					);

					GlobalGuiVars.tempCatNameStore = "";
					GlobalGuiVars.tempCatDescriptionStore = "";
					GlobalGuiVars.hasBeenGenerated++;
					Console.WriteLine (GlobalGuiVars.hasBeenGenerated);
				} else {
					Console.WriteLine ("Err: already generated");
				}

				GlobalGuiVars.AddCategoryPopUp_JustClosedSecondPhase = false;
				GlobalGuiVars.AddCategoryPopUp_JustClosed = false;
				GlobalGuiVars.AddCategoryOpened = false;

				GlobalGuiVars.n10 = false;
				GlobalGuiVars.n11 = false;

				GuiWorker.Worker (1, false);
				GuiWorker.Worker (2, false);
				GuiWorker.Worker (3, false);

			}
		}
		public void categoryItemToggleCell_Toggled(object sender, ToggledArgs socio)
		{
			TreeIter iter;
			if (categoryItemsStore.GetIter (out iter, new TreePath (socio.Path))) 
			{
				bool toggler = (bool)categoryItemsStore.GetValue (iter, 3);
				categoryItemsStore.SetValue (iter, 3, !toggler);
			}
		}
		public void deleteCategory_Clicked(object sender, EventArgs e)
		{
			TreeIter iter;
			int Res;


			categoryItemsStore.GetIter (out iter, new TreePath());
            try
            {
                string tempID = (string)categoryItemsStore.GetValue(iter, 0);
                string tempName = (string)categoryItemsStore.GetValue(iter, 1);

                int.TryParse(tempID, out Res);
                categoryItemsStore.Remove(ref iter);
            }
            catch
            {
                MessageBox.Show("An unknown error has occurred");
            }
		}
		void modifyCategory_Clicked(object sender, EventArgs e)
		{
			ModifyCategoryPopUp mcpu = new ModifyCategoryPopUp ();
			mcpu.Show ();
		}
		void fileNewMenuItem_Clicked(object sender, EventArgs e)
		{
			MainClass newWindow = new MainClass ();
		}
		void fileExitMenuItem_Clicked(object sender, EventArgs e)
		{
			Gtk.Application.Quit ();
		}
		void addTodoItem_Clicked(object sender, EventArgs e)
		{
			AddToDoItemPopUp atdipu = new AddToDoItemPopUp();
			atdipu.Show();

			//ToDoStore.Add(new ToDoItem(Convert.ToString(i), "Test " + i, "N/A", "High", "1", "2"));
			//GlobalGuiVars.todoListItemsStore.AppendValues (ToDoStore[i])
		}
		private void categoryItemNameCell_Edited(object sender, EditedArgs socio)
		{

			TreeIter iter;
			categoryItemsStore.GetIter (out iter, new TreePath (socio.Path));

			//CategoryName name = (CategoryName)categoryItemsStore.GetValue (iter, 1);
			GlobalGuiVars.tempCatNameStore = categoryItemNameCell.Text;

			string name = (string)categoryItemsStore.GetValue(iter,1);
			categoryItemsStore.SetValue(iter, 1, socio.NewText);
		}
		private void categoryItemDescriptionCell_Edited(object sender, EditedArgs socio) 
		{

			TreeIter iter;
			categoryItemsStore.GetIter (out iter, new TreePath (socio.Path));

			string name = (string)categoryItemsStore.GetValue(iter,2);
			categoryItemsStore.SetValue(iter, 2, socio.NewText);


		}
		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Environment.Exit (0); //makes sure that if there are any threads still running, they are closed with the program
		}

	}
}
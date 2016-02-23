using Gdk;
using Gtk;
using Projects.main.backend;
using Action = Gtk.Action;
using Label = Gtk.Label;
using Menu = Gtk.Menu;
using MenuItem = Gtk.MenuItem;
using ProgressBar = Gtk.ProgressBar;
using TreeView = Gtk.TreeView;

namespace Projects.main
{
    public sealed partial class ProjectWindow
    {
        private Toolbar _categoryToolbar;

        private Action _addCategoryAction;
        private Action _removeCategoryAction;

        private Action _addToDoItemAction;
        private Action _removeToDoItemAction;

        private TreeView _categoryTreeView;
        private TreeViewColumn _categoryItemId;
        private TreeViewColumn _categoryItemName;
        private TreeViewColumn _categoryItemToggle;

        private Label _categoryDescriptionLabel;
        private TextView _categoryDescription;

        private TreeView _todoListTreeView;
        private TreeViewColumn _todoItemId;
        private TreeViewColumn _todoItemName;
        private TreeViewColumn _todoItemPriority;
        private TreeViewColumn _todoItemCategory;
        private TreeViewColumn _todoItemStartDate;
        private TreeViewColumn _todoItemFinishDate;

        private VBox categorySidebar;
        public ListStore CategoryStore;
        public ListStore TodoStore;

        private ProgressBar _fileActionProgBar;

        private void BuildInterface()
        {
            Gui.Initialize(this);

            Name = "TodoList";
            Title = "Projects";
            WindowPosition = WindowPosition.Center;

            var uiManager = new UIManager();

            var actionGrp = new ActionGroup("Default");

            var mainMenu = new MenuBar
            {
                Name = "mainMenu"
            };

            var fileMenu = new Menu();

            var fileMenuItem = new MenuItem("File")
            {
                Name = "fileMenuItem",
                Submenu = fileMenu,
            };

            #region Menubar items

            var fileNewMenuItem = new MenuItem("New");
            fileNewMenuItem.Activated += FileNewMenuItem_OnActivated;
            fileMenu.Append(fileNewMenuItem);

            var fileOpenMenuItem = new MenuItem("Open");
            fileOpenMenuItem.Activated += OpenActionOnActivated;
            fileMenu.Append(fileOpenMenuItem);

            var fileSaveMenuItem = new MenuItem("Save");
            fileMenu.Append(fileSaveMenuItem);
            fileSaveMenuItem.Activated += SaveItem_OnActivated;

            var hsepfilemenu = new SeparatorMenuItem { Name = "FileMenuSeparator" };
            fileMenu.Append(hsepfilemenu);

            var fileExitMenuItem = new MenuItem("Exit");
            fileExitMenuItem.Activated += fileExitMenuItem_Clicked;
            fileMenu.Append(fileExitMenuItem);

            var windowPane = new HPaned();

            mainMenu.Append(fileMenuItem);

            _addCategoryAction = new Action("addCategoryAction",
                "New Category", "Create a new category", "CircledPlus");
            _addCategoryAction.Activated += AddCategory_Clicked;
            actionGrp.Add(_addCategoryAction, null);

            _removeCategoryAction = new Action("removeCategoryAction",
                "Remove Category", "Remove the selected category", "CircledMinus");
            _removeCategoryAction.Activated += deleteCategory_Clicked;
            actionGrp.Add(_removeCategoryAction, null);

            _addToDoItemAction = new Action("addToDoItemAction",
                "Add To-Do Item", "Add a new To-Do Item", "CircledPlus");
            _addToDoItemAction.Activated += AddTodoItem_Clicked;
            actionGrp.Add(_addToDoItemAction, null);

            _removeToDoItemAction = new Action("removeToDoItemAction",
                "Remove Item", "Remove To-Do Item", "CircledMinus");
            _removeToDoItemAction.Activated += DeleteToDoItem_Clicked;
            actionGrp.Add(_removeToDoItemAction, null);

            uiManager.InsertActionGroup(actionGrp, 0);

            AddAccelGroup(uiManager.AccelGroup);

            #endregion Menubar items

            var windowContainer = new VBox
            {
                Name = "mainWindowContainer",
                Spacing = 3
            };

            categorySidebar = new VBox
            {
                Name = "categorySidebar",
                Spacing = 6,
                ResizeMode = ResizeMode.Parent
            };

            categorySidebar.WidthRequest = 210;

            uiManager.AddUiFromString(
                "<ui>" +
                "<toolbar name='categoryToolbar'>" +
                "<toolitem name='addCategoryAction' action='addCategoryAction'/>" +
                "<toolitem name='removeCategoryAction' action='removeCategoryAction'/>" +
                "</toolbar>" +
                "</ui>");

            _categoryToolbar = ((Toolbar)(uiManager.GetWidget("/categoryToolbar")));
            _categoryToolbar.Events = ((EventMask)(8992));
            _categoryToolbar.Name = "catToolbar";
            _categoryToolbar.ShowArrow = false;

            var calendarExpander = new Expander(null)
            {
                CanFocus = true,
                Name = "CategoryExpander",
                Expanded = true
            };

            var calendar = new Calendar
            {
                CanFocus = true,
                Name = "calendar",
                DisplayOptions = ((CalendarDisplayOptions)(35))
            };

            calendarExpander.Add(calendar);

            var calendarExpanderLabel = new Label
            {
                Name = "CalendarExpanderLabel",
                LabelProp = "Calendar",
                UseUnderline = true
            };

            calendarExpander.LabelWidget = calendarExpanderLabel;

            var categoryExpander = new Expander(null)
            {
                CanFocus = true,
                Name = "CategoryExpander",
                Expanded = true
            };

            var categoryExpanderContainer = new VBox
            {
                Name = "categoryExpanderContainer",
                Spacing = 3
            };

            var categoryPane = new VPaned();

            var categoryContainer = new ScrolledWindow()
            {
                Name = "CategoryContainer",
                ShadowType = ShadowType.None,
                HscrollbarPolicy = PolicyType.Automatic,
                VscrollbarPolicy = PolicyType.Automatic
            };

            categoryContainer.HScrollbar.Visible = true;
            categoryContainer.VScrollbar.Visible = true;

            var categoryContainerLabel = new Label()
            {
                Name = "CategoryContainerLabel"
            };

            var categoryDescriptionScroll = new ScrolledWindow
            {
                Name = "categoryDescriptionScroll",
                ShadowType = ((ShadowType)(1)),
                VscrollbarPolicy = PolicyType.Automatic,
                HscrollbarPolicy = PolicyType.Automatic
            };

            var descriptionHBox = new VBox
            {
                Name = "descriptionHBox",
                HasWindow = false
            };

            var descriptionFixed = new Fixed
            {
                Name = "descriptionFixed",
                HasWindow = false
            };

            _categoryDescriptionLabel = new Label
            {
                Name = "categoryDescriptionLabel",
                Text = "Description"
            };

            _categoryDescription = new TextView
            {
                Name = "categoryDescription",
                BorderWidth = 2,
                Editable = false,
                WrapMode = WrapMode.WordChar,
                HscrollPolicy = ScrollablePolicy.Natural,
            };

            _categoryDescription.Buffer.Text = "No category selected";

            var toDoTableListContainer = new VBox
            {
                Name = "ToDoTableListContainer",
                Spacing = 3
            };

            uiManager.AddUiFromString(
                "<ui>" +
                "<toolbar name='todoToolbar'>" +
                "<toolitem name='addToDoItemAction' action='addToDoItemAction'/>" +
                "<toolitem name='removeToDoItemAction' action='removeToDoItemAction'/>" +
                "</toolbar>" +
                "</ui>");

            var todoToolbar = ((Toolbar)(uiManager.GetWidget("/todoToolbar")));
            todoToolbar.Events = ((EventMask)(8992));
            todoToolbar.Name = "todoToolbar";
            todoToolbar.ShowArrow = false;

            var recordsWindow = new ScrolledWindow
            {
                Name = "recordsWindow",
                ShadowType = ((ShadowType)(1)),
                VscrollbarPolicy = PolicyType.Automatic,
                HscrollbarPolicy = PolicyType.Automatic
            };

            #region Category table properties

            _categoryTreeView = new TreeView();

            _categoryItemId = new TreeViewColumn
            {
                Title = "ID",
                Resizable = true,
#if !DEBUG
                Visible = false
#endif
#if DEBUG
                Visible = true
#endif
            };

            var categoryIdCell = new CellRendererText();
            _categoryItemId.PackStart(categoryIdCell, false);
            _categoryItemId.SetCellDataFunc(categoryIdCell, RenderCategoryId);

            _categoryItemName = new TreeViewColumn { Title = "Category", Resizable = true };
            var categoryNameCell = new CellRendererText();
            _categoryItemName.PackStart(categoryNameCell, false);
            _categoryItemName.SetCellDataFunc(categoryNameCell, RenderCategoryName);
            categoryNameCell.Edited += categoryItemNameCell_Edited;

            _categoryItemToggle = new TreeViewColumn { Title = "Show", Resizable = true };
            var categoryToggleCell = new CellRendererToggle();
            _categoryItemToggle.PackStart(categoryToggleCell, false);
            _categoryItemToggle.SetCellDataFunc(categoryToggleCell, RenderCategoryToggle);
            categoryToggleCell.Toggled += CategoryItem_Toggled;

            CategoryStore = new ListStore(typeof(Category));
            _categoryTreeView.Model = CategoryStore;

            _categoryTreeView.AppendColumn(_categoryItemId);
            _categoryTreeView.AppendColumn(_categoryItemName);
            _categoryTreeView.AppendColumn(_categoryItemToggle);
            _categoryTreeView.RowActivated += CategoryTreeView_RowActivated;

            #endregion Category table properties

            #region Todo table properties

            _todoListTreeView = new TreeView();

            _todoItemId = new TreeViewColumn
            {
                Title = "ID",
                Resizable = true,
#if !DEBUG
                Visible = false
#endif
#if DEBUG
                Visible = true
#endif
            };

            var todoIdCell = new CellRendererText();
            _todoItemId.PackStart(todoIdCell, false);
            _todoItemId.SetCellDataFunc(todoIdCell, RenderToDoItemId);

            _todoItemName = new TreeViewColumn { Title = "Title", Resizable = true };
            var todoNameCell = new CellRendererText();
            _todoItemName.PackStart(todoNameCell, false);
            _todoItemName.SetCellDataFunc(todoNameCell, RenderToDoItemName);

            _todoItemCategory = new TreeViewColumn { Title = "Category", Resizable = true };
            var todoCategoryCell = new CellRendererText();
            _todoItemCategory.PackStart(todoCategoryCell, false);
            _todoItemCategory.SetCellDataFunc(todoCategoryCell, RenderToDoItemCategory);

            _todoItemPriority = new TreeViewColumn { Title = "Priority", Resizable = true };
            var todoPriorityCell = new CellRendererText();
            _todoItemPriority.PackStart(todoPriorityCell, false);
            _todoItemPriority.SetCellDataFunc(todoPriorityCell, RenderToDoItemPriority);

            _todoItemStartDate = new TreeViewColumn { Title = "Start", Resizable = true };
            var todoStartCell = new CellRendererText();
            _todoItemStartDate.PackStart(todoStartCell, false);
            _todoItemStartDate.SetCellDataFunc(todoStartCell, RenderToDoItemStart);

            _todoItemFinishDate = new TreeViewColumn { Title = "Finish", Resizable = true };
            var todoFinishCell = new CellRendererText();
            _todoItemFinishDate.PackStart(todoFinishCell, true);
            _todoItemFinishDate.SetCellDataFunc(todoFinishCell, RenderToDoItemFinish);

            TodoStore = new ListStore(typeof(ToDoItem));

            _todoListTreeView.Model = TodoStore;

            _todoListTreeView.AppendColumn(_todoItemId);
            _todoListTreeView.AppendColumn(_todoItemName);
            _todoListTreeView.AppendColumn(_todoItemCategory);
            _todoListTreeView.AppendColumn(_todoItemPriority);
            _todoListTreeView.AppendColumn(_todoItemStartDate);
            _todoListTreeView.AppendColumn(_todoItemFinishDate);

            #endregion Todo table properties

            _fileActionProgBar = new ProgressBar()
            {
                Name = "fileActionProgBar",
                Visible = false
            };

            windowContainer.Add(mainMenu);

            var menuChild = ((Box.BoxChild)(windowContainer[mainMenu]));
            menuChild.Fill = false;
            menuChild.Expand = false;

            categorySidebar.Add(calendarExpander);

            var sidebarChild = ((Box.BoxChild)(categorySidebar[calendarExpander]));
            sidebarChild.Position = 0;
            sidebarChild.Expand = false;
            sidebarChild.Fill = false;

            categoryExpanderContainer.Add(_categoryToolbar);
            categoryExpanderContainer.Add(_categoryTreeView);

            var toolbarChild = ((Box.BoxChild)(categoryExpanderContainer[_categoryToolbar]));
            toolbarChild.Expand = false;
            toolbarChild.Fill = false;

            categoryExpander.Add(categoryPane);

            categoryPane.Add(categoryContainer);

            var paneContainerChild = ((Paned.PanedChild)(categoryPane[categoryContainer]));
            paneContainerChild.Resize = true;

            categoryContainer.AddWithViewport(categoryExpanderContainer);
            categoryContainerLabel.LabelProp = "Categories";
            categoryContainerLabel.UseUnderline = true;
            categoryExpander.LabelWidget = categoryContainerLabel;

            categoryPane.Add(descriptionHBox);

            var descriptionContainerChild = ((Paned.PanedChild)(categoryPane[descriptionHBox]));
            descriptionContainerChild.Resize = true;

            descriptionHBox.Add(descriptionFixed);

            var descriptionFixedChild = ((Box.BoxChild)(descriptionHBox[descriptionFixed]));
            descriptionFixedChild.Expand = false;
            descriptionFixedChild.Padding = 5;

            descriptionFixed.Add(_categoryDescriptionLabel);

            var descriptionLabelChild = ((Fixed.FixedChild)(descriptionFixed[_categoryDescriptionLabel]));
            descriptionLabelChild.X = 5;
            descriptionLabelChild.Y = 0;

            descriptionHBox.Add(categoryDescriptionScroll);

            var descriptionViewChild = ((Box.BoxChild)(descriptionHBox[categoryDescriptionScroll]));
            descriptionViewChild.Expand = true;

            categoryDescriptionScroll.Add(_categoryDescription);

            categoryDescriptionScroll.AddWithViewport(_categoryDescription);

            categorySidebar.Add(categoryExpander);

            var sidebarchild = ((Box.BoxChild)(categorySidebar[categoryExpander]));
            sidebarchild.Position = 1;

            windowPane.Add(categorySidebar);

            toDoTableListContainer.Add(todoToolbar);

            var todobarChild = ((Box.BoxChild)(toDoTableListContainer[todoToolbar]));
            todobarChild.Fill = false;
            todobarChild.Expand = false;

            recordsWindow.Add(_todoListTreeView);

            recordsWindow.AddWithViewport(_todoListTreeView);

            toDoTableListContainer.Add(recordsWindow);

            toDoTableListContainer.Add(_fileActionProgBar);

            var barChild = ((Box.BoxChild)(toDoTableListContainer[_fileActionProgBar]));
            barChild.Expand = false;
            barChild.Fill = false;

            windowPane.Add(toDoTableListContainer);

            windowContainer.Add(windowPane);

            Add(windowContainer);

            SetSizeRequest(640, 480);
            DefaultWidth = 800;
            DefaultHeight = 600;

            DestroyEvent += ProjectWindow_DestroyEvent;
            DeleteEvent += OnDeleteEvent;
            ShowAll();

            _fileActionProgBar.Visible = false;
        }
    }
}
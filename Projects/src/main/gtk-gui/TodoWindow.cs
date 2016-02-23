using Gtk;
using Projects.main.backend;

namespace Projects.main
{
    public sealed partial class TodoWindow
    {
        private readonly string[] _values = { "High", "Medium", "Low", "None" };

        private VBox _containerVBox;

        private Fixed _nameFixed;
        private Label _nameLabel;
        private Entry _nameEntry;

        private Fixed _descFixed;
        private Label _descLabel;
        private ScrolledWindow _descWindow;
        private TextView _descView;

        private Fixed _categoryFixed;
        private Label _categoryLabel;
        private ComboBox _categoryBox;

        private Fixed _priorityFixed;
        private Label _priorityLabel;
        private ComboBox _priorityBox;

        private Fixed _startFixed;
        private Label _startLabel;

        private HBox _startHBox;
        private Entry _startEntry;
        private Button _startPicker;

        private Fixed _endFixed;
        private Label _endLabel;

        private HBox _endHBox;
        private Entry _endEntry;
        private Button _endPicker;

        private HBox _buttonsHBox;
        private Button _addButton;
        private Button _cancelButton;

        private void BuildInterface()
        {
            Gui.Initialize(this);

            Name = "ToDoWindow";
            Title = "New item";
            WindowPosition = WindowPosition.Center;
            KeepAbove = true;
            Resizable = false;

            _containerVBox = new VBox
            {
                Name = "containerFixed",
                HasWindow = false
            };

            _nameFixed = new Fixed
            {
                Name = "nameFixed",
                HasWindow = false
            };

            _nameLabel = new Label
            {
                Name = "nameLabel",
                Text = "Name"
            };

            _nameEntry = new Entry
            {
                Name = "nameEntry"
            };

            _descFixed = new Fixed
            {
                Name = "descFixed",
                HasWindow = false
            };

            _descLabel = new Label
            {
                Name = "descLabel",
                Text = "Description"
            };

            _descWindow = new ScrolledWindow
            {
                Name = "descWindow",
                ShadowType = ShadowType.None,
                HscrollbarPolicy = PolicyType.Never,
                VscrollbarPolicy = PolicyType.Automatic
            };

            _descView = new TextView
            {
                Name = "descView",
                BorderWidth = 2,
                WrapMode = WrapMode.WordChar,
                HscrollPolicy = ScrollablePolicy.Natural
            };

            _categoryFixed = new Fixed
            {
                Name = "categoryFixed",
                HasWindow = false
            };

            _categoryLabel = new Label
            {
                Name = "categoryLabel",
                Text = "Category"
            };

            _categoryBox = new ComboBox
            {
                Name = "categoryBox",
                Model = _categories
            };
            var categoryRenderer = new CellRendererText();
            _categoryBox.PackStart(categoryRenderer, true);
            _categoryBox.SetCellDataFunc(categoryRenderer, Func);
            _categoryBox.IdColumn = 0;

            _priorityFixed = new Fixed
            {
                Name = "priorityFixed",
                HasWindow = false
            };

            _priorityLabel = new Label
            {
                Name = "priorityLabel",
                Text = "Priority"
            };

            _priorityBox = new ComboBox(_values)
            {
                Name = "priorityBox",
            };

            _startFixed = new Fixed
            {
                Name = "startFixed",
                HasWindow = false
            };

            _startLabel = new Label
            {
                Name = "startLabel",
                Text = "Start"
            };

            _startHBox = new HBox
            {
                Name = "startHBox",
                HasWindow = false
            };

            _startEntry = new Entry
            {
                Name = "startEntry",
                IsEditable = false
            };

            _startPicker = new Button
            {
                Name = "startPicker"
            };
            _startPicker.RenderIconPixbuf("Calendar", IconSize.Button);

            _endFixed = new Fixed
            {
                Name = "endFixed",
                HasWindow = false
            };

            _endLabel = new Label
            {
                Name = "endLabel",
                Text = "End"
            };

            _endHBox = new HBox
            {
                Name = "endHBox",
                HasWindow = false
            };

            _endEntry = new Entry
            {
                Name = "endEntry",
                IsEditable = false
            };

            _endPicker = new Button
            {
                Name = "endPicker"
            };
            _endPicker.RenderIconPixbuf("Calendar", IconSize.Button);

            _buttonsHBox = new HBox
            {
                Name = "buttonsFixed",
                HasWindow = false
            };

            _addButton = new Button
            {
                Name = "addButton",
                Label = "Create"
            };

            _cancelButton = new Button
            {
                Name = "cancelButton",
                Label = "Cancel",
            };

            _descWindow.HScrollbar.Visible = false;
            _descWindow.VScrollbar.Visible = true;

            _descWindow.SetSizeRequest(300, 100);
            _descView.SetSizeRequest(300, 100);
            _nameEntry.SetSizeRequest(300, 25);

            _addButton.WidthRequest = 100;
            _cancelButton.WidthRequest = 100;
            _priorityBox.WidthRequest = 100;

            _containerVBox.Add(_nameFixed);

            var nameFixedChild = ((Box.BoxChild)(_containerVBox[_nameFixed]));
            nameFixedChild.Expand = true;
            nameFixedChild.Padding = 5;

            _nameFixed.Add(_nameLabel);
            var nameLabelChild = ((Fixed.FixedChild)(_nameFixed[_nameLabel]));
            nameLabelChild.X = 5;
            nameLabelChild.Y = 5;

            _containerVBox.Add(_nameEntry);

            var nameEntryChild = ((Box.BoxChild)(_containerVBox[_nameEntry]));
            nameEntryChild.Expand = true;

            _containerVBox.Add(_descFixed);
            var descFixedChild = ((Box.BoxChild)(_containerVBox[_descFixed]));
            descFixedChild.Expand = true;
            descFixedChild.Padding = 5;

            _descFixed.Add(_descLabel);

            var descLabelChild = ((Fixed.FixedChild)(_descFixed[_descLabel]));
            descLabelChild.X = 5;
            descLabelChild.Y = 5;

            _containerVBox.Add(_descWindow);

            var descViewChild = ((Box.BoxChild)(_containerVBox[_descWindow]));
            descViewChild.Expand = true;

            _descWindow.AddWithViewport(_descView);

            _containerVBox.Add(_categoryFixed);

            var catFixedChild = ((Box.BoxChild)(_containerVBox[_categoryFixed]));
            catFixedChild.Expand = true;
            catFixedChild.Padding = 5;

            _categoryFixed.Add(_categoryLabel);

            var catLabelChild = ((Fixed.FixedChild)(_categoryFixed[_categoryLabel]));
            catLabelChild.X = 5;
            catLabelChild.Y = 5;

            _containerVBox.Add(_categoryBox);

            var catBoxChild = ((Box.BoxChild)(_containerVBox[_categoryBox]));
            catBoxChild.Expand = true;

            _containerVBox.Add(_priorityFixed);

            var priFixedChild = ((Box.BoxChild)(_containerVBox[_priorityFixed]));
            priFixedChild.Expand = true;
            priFixedChild.Padding = 5;

            _priorityFixed.Add(_priorityLabel);

            var priLabelChild = ((Fixed.FixedChild)(_priorityFixed[_priorityLabel]));
            priLabelChild.X = 5;
            priLabelChild.Y = 5;

            _containerVBox.Add(_priorityBox);

            var priBoxChild = ((Box.BoxChild)(_containerVBox[_priorityBox]));
            priBoxChild.Expand = true;

            _containerVBox.Add(_startFixed);

            var startFixedChild = ((Box.BoxChild)(_containerVBox[_startFixed]));
            startFixedChild.Expand = true;
            startFixedChild.Padding = 5;

            _startFixed.Add(_startLabel);

            var startLabelChild = ((Fixed.FixedChild)(_startFixed[_startLabel]));
            startLabelChild.X = 5;
            startLabelChild.Y = 5;

            _containerVBox.Add(_startHBox);

            var startBoxChild = ((Box.BoxChild)(_containerVBox[_startHBox]));
            startBoxChild.Expand = true;
            startBoxChild.Padding = 5;

            _startHBox.Add(_startEntry);

            var startEntryChild = ((Box.BoxChild)(_startHBox[_startEntry]));
            startEntryChild.Expand = false;

            _startHBox.Add(_startPicker);

            var startPickerChild = ((Box.BoxChild)(_startHBox[_startPicker]));
            startPickerChild.Expand = false;

            _containerVBox.Add(_endFixed);

            var endFixedChild = ((Box.BoxChild)(_containerVBox[_endFixed]));
            endFixedChild.Expand = true;
            endFixedChild.Padding = 5;

            _endFixed.Add(_endLabel);

            var endLabelChild = ((Fixed.FixedChild)(_endFixed[_endLabel]));
            endLabelChild.X = 5;
            endLabelChild.Y = 5;

            _containerVBox.Add(_endHBox);

            var endBoxChild = ((Box.BoxChild)(_containerVBox[_endHBox]));
            endBoxChild.Expand = true;
            endBoxChild.Padding = 5;

            _endHBox.Add(_endEntry);

            var endEntryChild = ((Box.BoxChild)(_endHBox[_endEntry]));
            endEntryChild.Expand = false;

            _endHBox.Add(_endPicker);

            var endPickerChild = ((Box.BoxChild)(_endHBox[_endPicker]));
            endPickerChild.Expand = false;

            _containerVBox.Add(_buttonsHBox);

            var buttonsChild = ((Box.BoxChild)(_containerVBox[_buttonsHBox]));
            buttonsChild.Expand = false;
            buttonsChild.Padding = 5;

            _buttonsHBox.Add(_addButton);

            var addChild = ((Box.BoxChild)(_buttonsHBox[_addButton]));
            addChild.Expand = false;
            addChild.Padding = 5;

            _buttonsHBox.Add(_cancelButton);
            var cancelChild = ((Box.BoxChild)(_buttonsHBox[_cancelButton]));
            cancelChild.Expand = false;
            cancelChild.Padding = 5;

            Add(_containerVBox);

            ShowAll();

            DeleteEvent += OnDeleteEvent;
            _startPicker.Clicked += StartPicker_Clicked;
            _endPicker.Clicked += EndPicker_Clicked;
            _addButton.Clicked += AddButton_Clicked;
            _cancelButton.Clicked += CancelButton_Clicked;
        }

        private void OnDeleteEvent(object o, DeleteEventArgs args)
        {
            args.RetVal = true;
            Destroy();
        }
    }
}
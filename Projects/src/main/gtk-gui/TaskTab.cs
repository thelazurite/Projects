using Gtk;

namespace Projects.main
{
    public sealed partial class TaskTab
    {
        private readonly string[] _values = {"High", "Medium", "Low", "None"};
        private Button _addButton;

        private HBox _buttonsHBox;
        private Button _cancelButton;
        private ComboBox _categoryBox;

        private Fixed _categoryFixed;
        private Label _categoryLabel;

        private Fixed _descFixed;
        private Label _descLabel;
        private TextView _descView;
        private ScrolledWindow _descWindow;
        private Entry _endEntry;

        private Fixed _endFixed;

        private HBox _endHBox;
        private Label _endLabel;
        private Button _endPicker;
        private Entry _nameEntry;

        private Fixed _nameFixed;
        private Label _nameLabel;
        private ComboBox _priorityBox;

        private Fixed _priorityFixed;
        private Label _priorityLabel;
        private Entry _startEntry;

        private Fixed _startFixed;

        private HBox _startHBox;
        private Label _startLabel;
        private Button _startPicker;

        private void BuildInterface()
        {
            //Gui.Initialize(this);
            Name = "TaskTab";

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
                Name = "priorityBox"
            };

            _startFixed = new Fixed
            {
                Name = "startFixed",
                HasWindow = false
            };

            _startLabel = new Label
            {
                Name = "startLabel",
                Text = "Start Date"
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
                Text = "Due Date"
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
                Label = "Cancel"
            };


            _descWindow.HScrollbar.Visible = false;
            _descWindow.VScrollbar.Visible = true;

            _descWindow.SetSizeRequest(300, 100);
            _descView.SetSizeRequest(300, 100);
            _nameEntry.SetSizeRequest(300, 25);

            _addButton.WidthRequest = 100;
            _cancelButton.WidthRequest = 100;
            _priorityBox.WidthRequest = 100;

            Add(_nameFixed);

            var nameFixedChild = (BoxChild) this[_nameFixed];
            nameFixedChild.Expand = true;
            nameFixedChild.Padding = 5;

            _nameFixed.Add(_nameLabel);
            var nameLabelChild = (Fixed.FixedChild) _nameFixed[_nameLabel];
            nameLabelChild.X = 5;
            nameLabelChild.Y = 5;

            Add(_nameEntry);

            var nameEntryChild = (BoxChild) this[_nameEntry];
            nameEntryChild.Expand = true;

            Add(_descFixed);
            var descFixedChild = (BoxChild) this[_descFixed];
            descFixedChild.Expand = true;
            descFixedChild.Padding = 5;

            _descFixed.Add(_descLabel);

            var descLabelChild = (Fixed.FixedChild) _descFixed[_descLabel];
            descLabelChild.X = 5;
            descLabelChild.Y = 5;

            Add(_descWindow);

            var descViewChild = (BoxChild) this[_descWindow];
            descViewChild.Expand = true;

            _descWindow.AddWithViewport(_descView);

            Add(_categoryFixed);

            var catFixedChild = (BoxChild) this[_categoryFixed];
            catFixedChild.Expand = true;
            catFixedChild.Padding = 5;

            _categoryFixed.Add(_categoryLabel);

            var catLabelChild = (Fixed.FixedChild) _categoryFixed[_categoryLabel];
            catLabelChild.X = 5;
            catLabelChild.Y = 5;

            Add(_categoryBox);

            var catBoxChild = (BoxChild) this[_categoryBox];
            catBoxChild.Expand = true;

            Add(_priorityFixed);

            var priFixedChild = (BoxChild) this[_priorityFixed];
            priFixedChild.Expand = true;
            priFixedChild.Padding = 5;

            _priorityFixed.Add(_priorityLabel);

            var priLabelChild = (Fixed.FixedChild) _priorityFixed[_priorityLabel];
            priLabelChild.X = 5;
            priLabelChild.Y = 5;

            Add(_priorityBox);

            var priBoxChild = (BoxChild) this[_priorityBox];
            priBoxChild.Expand = true;

            Add(_startFixed);

            var startFixedChild = (BoxChild) this[_startFixed];
            startFixedChild.Expand = true;
            startFixedChild.Padding = 5;

            _startFixed.Add(_startLabel);

            var startLabelChild = (Fixed.FixedChild) _startFixed[_startLabel];
            startLabelChild.X = 5;
            startLabelChild.Y = 5;

            Add(_startHBox);

            var startBoxChild = (BoxChild) this[_startHBox];
            startBoxChild.Expand = true;
            startBoxChild.Padding = 5;

            _startHBox.Add(_startEntry);

            var startEntryChild = (BoxChild) _startHBox[_startEntry];
            startEntryChild.Expand = false;

            _startHBox.Add(_startPicker);

            var startPickerChild = (BoxChild) _startHBox[_startPicker];
            startPickerChild.Expand = false;

            Add(_endFixed);

            var endFixedChild = (BoxChild) this[_endFixed];
            endFixedChild.Expand = true;
            endFixedChild.Padding = 5;

            _endFixed.Add(_endLabel);

            var endLabelChild = (Fixed.FixedChild) _endFixed[_endLabel];
            endLabelChild.X = 5;
            endLabelChild.Y = 5;

            Add(_endHBox);

            var endBoxChild = (BoxChild) this[_endHBox];
            endBoxChild.Expand = true;
            endBoxChild.Padding = 5;

            _endHBox.Add(_endEntry);

            var endEntryChild = (BoxChild) _endHBox[_endEntry];
            endEntryChild.Expand = false;

            _endHBox.Add(_endPicker);

            var endPickerChild = (BoxChild) _endHBox[_endPicker];
            endPickerChild.Expand = false;

            Add(_buttonsHBox);

            var buttonsChild = (BoxChild) this[_buttonsHBox];
            buttonsChild.Expand = false;
            buttonsChild.Padding = 5;

            _buttonsHBox.Add(_addButton);

            var addChild = (BoxChild) _buttonsHBox[_addButton];
            addChild.Expand = false;
            addChild.Padding = 5;

            _buttonsHBox.Add(_cancelButton);
            var cancelChild = (BoxChild) _buttonsHBox[_cancelButton];
            cancelChild.Expand = false;
            cancelChild.Padding = 5;

            //Add(this);

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
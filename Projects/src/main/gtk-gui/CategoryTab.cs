using Gtk;
using Projects.main.backend;

namespace Projects.main
{
    public sealed partial class CategoryTab
    {
        private Button _addButton;

        private HBox _buttonsHBox;
        private Button _cancelButton;

        private Fixed _descFixed;
        private Label _descLabel;
        private TextView _descView;
        private ScrolledWindow _descWindow;
        private Entry _nameEntry;

        private Fixed _nameFixed;
        private Label _nameLabel;

        private void BuildInterface()
        {
            Gui.Initialize(this);
            Name = "CategoryTab";


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


            _addButton.WidthRequest = 100;
            _cancelButton.WidthRequest = 100;

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

            ShowAll();

            DeleteEvent += OnDeleteEvent;
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
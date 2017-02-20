using System;
using Gdk;
using Gtk;
using Projects.main.backend;

namespace Projects.main
{
    public sealed partial class CategoryWindow
    {
        private VBox _containerVBox;

        private Fixed _nameFixed;
        private Label _nameLabel;
        private Entry _nameEntry;

        private Fixed _descFixed;
        private Label _descLabel;
        private ScrolledWindow _descWindow;
        private TextView _descView;

        private HBox _buttonsHBox;
        private Button _addButton;
        private Button _cancelButton;

        private void BuildInterface()
        {
            Gui.Initialize(this);
            Name = "CategoryWindow";
            Title = "New Category";
            Icon = new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\todo.png"));
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

            _descWindow = new ScrolledWindow()
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
                HscrollPolicy = ScrollablePolicy.Natural,
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
                Label = "Cancel",
            };


            _descWindow.HScrollbar.Visible = false;
            _descWindow.VScrollbar.Visible = true;

            _descWindow.SetSizeRequest(300, 100);
            _descView.SetSizeRequest(300, 100);
            _nameEntry.SetSizeRequest(300, 25);

            _addButton.WidthRequest = 100;
            _cancelButton.WidthRequest = 100;

            _containerVBox.Add(_nameFixed);

            var nameFixedChild = (Box.BoxChild)_containerVBox[_nameFixed];
            nameFixedChild.Expand = true;
            nameFixedChild.Padding = 5;

            _nameFixed.Add(_nameLabel);
            var nameLabelChild = (Fixed.FixedChild)_nameFixed[_nameLabel];
            nameLabelChild.X = 5;
            nameLabelChild.Y = 5;

            _containerVBox.Add(_nameEntry);

            var nameEntryChild = (Box.BoxChild)_containerVBox[_nameEntry];
            nameEntryChild.Expand = true;

            _containerVBox.Add(_descFixed);
            var descFixedChild = (Box.BoxChild) _containerVBox[_descFixed];
            descFixedChild.Expand = true;
            descFixedChild.Padding = 5;

            _descFixed.Add(_descLabel);

            var descLabelChild = (Fixed.FixedChild)_descFixed[_descLabel];
            descLabelChild.X = 5;
            descLabelChild.Y = 5;

            _containerVBox.Add(_descWindow);

            var descViewChild = (Box.BoxChild) _containerVBox[_descWindow];
            descViewChild.Expand = true;

            _descWindow.AddWithViewport(_descView);

            _containerVBox.Add(_buttonsHBox);

            var buttonsChild = (Box.BoxChild) _containerVBox[_buttonsHBox];
            buttonsChild.Expand = false;
            buttonsChild.Padding = 5;
            
            _buttonsHBox.Add(_addButton);

            var addChild = (Box.BoxChild) _buttonsHBox[_addButton];
            addChild.Expand = false;
            addChild.Padding = 5;

            _buttonsHBox.Add(_cancelButton);
            var cancelChild = (Box.BoxChild)_buttonsHBox[_cancelButton];
            cancelChild.Expand = false;
            cancelChild.Padding = 5;

            Add(_containerVBox);

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

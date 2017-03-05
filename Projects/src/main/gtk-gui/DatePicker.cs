using System;
using Gdk;
using Gtk;
using Projects.main.backend;

namespace Projects.main
{
    public sealed partial class DatePicker
    {
        private HBox _buttonBox;
        private Button _cancelButton;
        private VBox _containerVBox;

        private VBox _hoursBox;
        private Label _hoursLabel;

        private VBox _minutesBox;
        private Label _minutesLabel;

        private VBox _secondsBox;
        private Label _secondsLabel;
        private HBox _timeBox;
        public Button AcceptButton;
        public Calendar Calendar;
        public SpinButton HoursSpin;
        public SpinButton MinutesSpin;
        public SpinButton SecondsSpin;

        private void BuildInterface()
        {
            Gui.Initialize(this);
            Icon = new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, !PrjHandler.IsUnix ? @"Content\img\todo.png": @"Content/img/todo.png"));
            Name = "DatePicker";
            Title = "Set date";
            WindowPosition = WindowPosition.Mouse;

            KeepAbove = true;
            Modal = true;
            Resizable = false;

            _containerVBox = new VBox
            {
                Name = "containerVBox",
                HasWindow = false
            };

            Calendar = new Calendar();

            _timeBox = new HBox
            {
                Name = "timeBox",
                HasWindow = false
            };

            _hoursBox = new VBox
            {
                Name = "hoursBox",
                HasWindow = false
            };

            _hoursLabel = new Label
            {
                Name = "hoursLabel",
                Text = "Hours"
            };

            HoursSpin = new SpinButton(0f, 1f, 1f)
            {
                Name = "hoursSpin"
            };
            HoursSpin.SetRange(0, 23);

            _minutesBox = new VBox
            {
                Name = "minutesBox",
                HasWindow = false
            };

            _minutesLabel = new Label
            {
                Name = "minutesLabel",
                Text = "Minutes"
            };

            MinutesSpin = new SpinButton(0f, 1f, 1f)
            {
                Name = "minutesSpin"
            };
            MinutesSpin.SetRange(0, 59);

            _secondsBox = new VBox
            {
                Name = "secondsBox",
                HasWindow = false
            };

            _secondsLabel = new Label
            {
                Name = "secondsLabel",
                Text = "Seconds"
            };

            SecondsSpin = new SpinButton(0f, 1f, 1f)
            {
                Name = "secondsSpin"
            };
            SecondsSpin.SetRange(0, 59);

            _buttonBox = new HBox
            {
                Name = "buttonBox",
                HasWindow = false
            };

            AcceptButton = new Button
            {
                Name = "acceptButton",
                Label = "OK"
            };

            _cancelButton = new Button
            {
                Name = "cancelButton",
                Label = "Cancel"
            };

            _containerVBox.Add(Calendar);

            var calendarChild = (Box.BoxChild) _containerVBox[Calendar];
            calendarChild.Padding = 5;
            calendarChild.Expand = true;

            _containerVBox.Add(_timeBox);

            var timeChild = (Box.BoxChild) _containerVBox[_timeBox];
            timeChild.Padding = 5;
            timeChild.Expand = true;

            _timeBox.Add(_hoursBox);
            var hoursBoxChild = (Box.BoxChild) _timeBox[_hoursBox];
            hoursBoxChild.Padding = 5;
            hoursBoxChild.Expand = true;

            _hoursBox.Add(_hoursLabel);

            var hoursLabelChild = (Box.BoxChild) _hoursBox[_hoursLabel];
            hoursLabelChild.Padding = 5;
            hoursLabelChild.Expand = true;

            _hoursBox.Add(HoursSpin);

            var hoursSpinChild = (Box.BoxChild) _hoursBox[HoursSpin];
            hoursSpinChild.Padding = 5;
            hoursSpinChild.Expand = true;

            _timeBox.Add(_minutesBox);
            var minutesBoxChild = (Box.BoxChild) _timeBox[_minutesBox];
            minutesBoxChild.Padding = 5;
            minutesBoxChild.Expand = true;

            _minutesBox.Add(_minutesLabel);

            var minutesLabelChild = (Box.BoxChild) _minutesBox[_minutesLabel];
            minutesLabelChild.Padding = 5;
            minutesLabelChild.Expand = true;

            _minutesBox.Add(MinutesSpin);

            var minutesSpinChild = (Box.BoxChild) _minutesBox[MinutesSpin];
            minutesSpinChild.Padding = 5;
            minutesSpinChild.Expand = true;

            _timeBox.Add(_secondsBox);
            var secondsBoxChild = (Box.BoxChild) _timeBox[_secondsBox];
            secondsBoxChild.Padding = 5;
            secondsBoxChild.Expand = true;

            _secondsBox.Add(_secondsLabel);

            var secondsLabelChild = (Box.BoxChild) _secondsBox[_secondsLabel];
            secondsLabelChild.Padding = 5;
            secondsLabelChild.Expand = true;

            _secondsBox.Add(SecondsSpin);

            var secondsSpinChild = (Box.BoxChild) _secondsBox[SecondsSpin];
            secondsSpinChild.Padding = 5;
            secondsSpinChild.Expand = true;

            _containerVBox.Add(_buttonBox);

            var boxchild = (Box.BoxChild) _containerVBox[_buttonBox];
            boxchild.Padding = 5;
            boxchild.Expand = true;

            _buttonBox.Add(AcceptButton);

            var acceptChild = (Box.BoxChild) _buttonBox[AcceptButton];
            acceptChild.Padding = 5;

            _buttonBox.Add(_cancelButton);

            var cancelChild = (Box.BoxChild) _buttonBox[_cancelButton];
            cancelChild.Padding = 5;

            Add(_containerVBox);

            ShowAll();

            _cancelButton.Clicked += CancelButton_Clicked;

            DeleteEvent += OnDeleteEvent;
        }


        private void OnDeleteEvent(object o, DeleteEventArgs args)
        {
            args.RetVal = true;
            Destroy();
        }
    }
}
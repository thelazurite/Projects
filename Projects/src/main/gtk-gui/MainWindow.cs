using System;
using Gdk;
using Gtk;
using Projects.main.backend;

namespace Projects.main
{
    public partial class MainWindow
    {
        private Image _backgroundImage;
        private Fixed _containerFixed;

        private Button _newButton;
        private Button _openButton;

        private void BuildInterface()
        {
            Gui.Initialize(this);

            Name = "MainWindow";
            Title = "Welcome to Projects!";
            WindowPosition = WindowPosition.Center;
            Icon = new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\todo.png"));
            DefaultWidth = 553;
            DefaultHeight = 194;
            Resizable = false;

            _containerFixed = new Fixed
            {
                Name = "containerFixed",
                HasWindow = false
            };


            _backgroundImage = new Image
            {
                Name = "backgroundImage",
                Pixbuf =
                    new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\splash.png"))
            };

            _containerFixed.Add(_backgroundImage);
            var backgroundChild = (Fixed.FixedChild) _containerFixed[_backgroundImage];

            backgroundChild.X = 0;
            backgroundChild.Y = 0;

            _openButton = new Button("_Open")
            {
                Name = "openButton"
            };

            _openButton.Clicked += OpenButton_Clicked;
            _containerFixed.Add(_openButton);
            var openChild = (Fixed.FixedChild) _containerFixed[_openButton];

            openChild.X = 500;
            openChild.Y = 80;

            _newButton = new Button("_New")
            {
                Name = "newButton"
            };
            _newButton.Clicked += NewButton_Clicked;

            _containerFixed.Add(_newButton);
            var newChild = (Fixed.FixedChild) _containerFixed[_newButton];

            newChild.X = 500;
            newChild.Y = 120;

            Add(_containerFixed);
            ShowAll();

            DeleteEvent += OnDeleteEvent;
        }

        private static void OnDeleteEvent(object o, DeleteEventArgs args)
        {
            args.RetVal = true;
            Application.Quit();
        }
    }
}
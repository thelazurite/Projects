using Gdk;
using Gtk;
using Projects.main.backend;
using System;
using Image = Gtk.Image;

namespace Projects.main
{
    public partial class MainWindow
    {
        // Widget declaration
        private Fixed _containerFixed;

        private Image _backgroundImage;
        private Button _openButton;
        private Button _newButton;

        /// <summary>
        /// Builds the interface.
        /// </summary>
        private void BuildInterface()
        {
            // initialize the window
            Gui.Initialize(this);

            Name = "MainWindow";
            Title = "Welcome to Projects!";
            WindowPosition = WindowPosition.Center;

            DefaultWidth = 553;
            DefaultHeight = 194;
            Resizable = false;

            _containerFixed = new Fixed
            {
                Name = "containerFixed",
                HasWindow = false,
            };

            _backgroundImage = new Image
            {
                Name = "backgroundImage",
            };

            if (OS.isWindows())
            {
                _backgroundImage.Pixbuf = new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\img\splash.png"));
            }
            else
            {
                _backgroundImage.Pixbuf = new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content/img/splash.png"));
            }

            _containerFixed.Add(_backgroundImage);
            var backgroundChild = ((Fixed.FixedChild)(_containerFixed[_backgroundImage]));

            backgroundChild.X = 0;
            backgroundChild.Y = 0;

            _openButton = new Button
            {
                Name = "openButton",
                Label = "Open"
            };

            _openButton.Clicked += OpenButton_Clicked;
            _containerFixed.Add(_openButton);
            var openChild = ((Fixed.FixedChild)(_containerFixed[_openButton]));

            openChild.X = 500;
            openChild.Y = 80;

            _newButton = new Button
            {
                Name = "newButton",
                Label = "New"
            };
            _newButton.Clicked += NewButton_Clicked;

            _containerFixed.Add(_newButton);
            var newChild = ((Fixed.FixedChild)(_containerFixed[_newButton]));

            newChild.X = 500;
            newChild.Y = 120;

            Add(_containerFixed);
            ShowAll();

            DeleteEvent += OnDeleteEvent;
        }

        /// <summary>
        /// When the delete event has been raised.
        /// </summary>
        private static void OnDeleteEvent(object o, DeleteEventArgs args)
        {
            // quit the application
            args.RetVal = true;
            Application.Quit();
        }
    }
}
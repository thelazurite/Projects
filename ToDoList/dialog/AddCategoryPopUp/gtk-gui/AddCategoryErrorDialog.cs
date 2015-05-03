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
using System.Threading.Tasks;
using Gtk;
using Gdk;

namespace ToDoList
{
    public partial class AddCategoryErrorDialog
    {
        private global::Gtk.UIManager UIManager;
        private global::Gtk.VBox errorMessageContainer;
        private global::Gtk.Label error;
        private global::Gtk.HSeparator hsep;
        private global::Gtk.Button okButton;

        protected virtual void BuildDialog() 
        {
            this.UIManager = new global::Gtk.UIManager();
            
           /* this.actGrp = new global::Gtk.ActionGroup("Default");
            this.ActionD = new global::Gtk.Action("Action", null, null, null);
            this.actGrp.Add(ActionD, null);
            this.UIManager.InsertActionGroup(actGrp, 0);
            */
            this.AddAccelGroup(this.UIManager.AccelGroup);
            this.Name = "AddCategoryErrorDialog";
            this.Title = "Error!";
            this.WindowPosition = global::Gtk.WindowPosition.Center;
            this.errorMessageContainer = new global::Gtk.VBox();
            this.errorMessageContainer.Name = "errorMessageContainer";
            this.error = new global::Gtk.Label();
            this.error.Name = "errorLabel";
            this.error.Text = "Category Name has not been added.\nPlease add one before continuing.";
            this.errorMessageContainer.Add(error);
            global::Gtk.Box.BoxChild er = ((global::Gtk.Box.BoxChild)(this.errorMessageContainer[this.error]));

            this.hsep = new global::Gtk.HSeparator();
            this.hsep.Name = "hsep";
            this.errorMessageContainer.Add(hsep);
            global::Gtk.Box.BoxChild hs = ((global::Gtk.Box.BoxChild)(this.errorMessageContainer[this.hsep]));

            this.okButton = new global::Gtk.Button();
            this.okButton.Name = ("ok");
            this.okButton.Label = ("OK");
            this.okButton.Clicked += okButton_Clicked;
            this.errorMessageContainer.Add(this.okButton);
            global::Gtk.Box.BoxChild ok = ((global::Gtk.Box.BoxChild)(this.errorMessageContainer[this.okButton]));
            
            this.Add(errorMessageContainer);
            
            if ((this.Child != null))
            {
                this.Child.ShowAll();
            }

            this.DefaultWidth = 75;
            this.DefaultHeight = 50;
            this.Resizable = false;
            this.Show();
            this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent); 
        }


    }
}

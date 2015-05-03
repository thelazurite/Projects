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
    public partial class AddCategoryPopUp
    {
        private global::Gtk.UIManager UIManager;
        
        private global::Gtk.VBox TableHolder;
        
        private global::Gtk.Table TableFormDetails;

        private global::Gtk.Entry CategoryName;

        private global::Gtk.Label CategoryNameLabel;

        private global::Gtk.Entry CategoryDescription;

        private global::Gtk.Label CategoryDescriptionLabel;

        private global::Gtk.Table TableFormButtons;

        private global::Gtk.Button OkayButton;

        private global::Gtk.Button CancelButton;
        

        protected virtual void BuildACPopUp()
        {
            Console.WriteLine("Building Pop-up Gui");

            this.UIManager = new global::Gtk.UIManager();
           

            this.AddAccelGroup(this.UIManager.AccelGroup);
            this.Name = "AddCategoryPopUp";
            this.Title = "Add Category";
            Console.WriteLine("Window created: " + this.Title);

            this.WindowPosition = global::Gtk.WindowPosition.Center;
            this.TableHolder = new global::Gtk.VBox();
            this.TableHolder.Name = "TableHolder";

            this.TableFormDetails = new global::Gtk.Table((uint)(2), (uint)(2), true);

            this.TableFormDetails.Name = "TableFormDetails";
            this.TableFormDetails.RowSpacing = ((uint)(6));
            this.TableFormDetails.ColumnSpacing = ((uint)(6));

            this.CategoryNameLabel = new global::Gtk.Label();
            this.CategoryNameLabel.Name = "CategoryNameLabel";
            this.CategoryNameLabel.Text = "Category: ";
            this.TableFormDetails.Add(this.CategoryNameLabel);
            global::Gtk.Table.TableChild cnl = ((global::Gtk.Table.TableChild)(this.TableFormDetails[this.CategoryNameLabel]));
            cnl.LeftAttach = ((uint)(1));
            cnl.RightAttach = ((uint)(1));
            cnl.XOptions = ((global::Gtk.AttachOptions)(4));
            cnl.YOptions = ((global::Gtk.AttachOptions)(4));

            this.CategoryName = new global::Gtk.Entry();
            this.CategoryName.Name = "CategoryName";
            this.CategoryName.CanFocus = true;
            this.CategoryName.TextInserted += TextInserted;
            this.CategoryName.TextDeleted += TextDeleted;
            this.TableFormDetails.Add(this.CategoryName);
            global::Gtk.Table.TableChild cn = ((global::Gtk.Table.TableChild)(this.TableFormDetails[this.CategoryName]));
            cn.LeftAttach = ((uint)(1));
            cn.RightAttach = ((uint)(2));
            cn.XOptions = ((global::Gtk.AttachOptions)(4));
            cn.YOptions = ((global::Gtk.AttachOptions)(4));

            this.CategoryDescriptionLabel = new global::Gtk.Label();
            this.CategoryDescriptionLabel.Name = "CategoryDescriptionLabel";
            this.CategoryDescriptionLabel.Text = "Description: ";
            this.TableFormDetails.Add(this.CategoryDescriptionLabel);
            global::Gtk.Table.TableChild cdl = ((global::Gtk.Table.TableChild)(this.TableFormDetails[this.CategoryDescriptionLabel]));
            cdl.BottomAttach = ((uint)(2));
            cdl.TopAttach = ((uint)(1));

            cdl.LeftAttach = ((uint)(2));
            cdl.RightAttach = ((uint)(1));
          
            cdl.XOptions = ((global::Gtk.AttachOptions)(4));
            cdl.YOptions = ((global::Gtk.AttachOptions)(4));

            this.CategoryDescription = new global::Gtk.Entry();
            this.CategoryDescription.Name = "CategoryDescription";
            this.CategoryDescription.CanFocus = true;
            this.CategoryDescription.TextInserted += TextInserted;
            this.CategoryDescription.TextDeleted += TextDeleted;
            this.TableFormDetails.Add(this.CategoryDescription);
            global::Gtk.Table.TableChild cd = ((global::Gtk.Table.TableChild)(this.TableFormDetails[this.CategoryDescription]));
            cd.BottomAttach = ((uint)(2));
            cd.TopAttach = ((uint)(1)); 
            cd.LeftAttach = ((uint)(2));
            cd.RightAttach = ((uint)(2));
            cd.XOptions = ((global::Gtk.AttachOptions)(4));
            cd.YOptions = ((global::Gtk.AttachOptions)(4));
            
            
            this.TableHolder.Add(this.TableFormDetails);
            global::Gtk.Box.BoxChild thbbc = ((global::Gtk.Box.BoxChild)(this.TableHolder[this.TableFormDetails]));
            thbbc.Position = 1;
            thbbc.Expand = false;
            thbbc.Fill = false;

            this.TableFormButtons = new global::Gtk.Table(1, 2, true);
            this.TableFormButtons.Name = "TableFormButtons";
            this.TableFormButtons.ColumnSpacing = 6;
            this.TableFormButtons.RowSpacing = 6;

            this.OkayButton = new global::Gtk.Button();
            this.OkayButton.Name = "OkayButton";
            this.OkayButton.CanFocus = true;
            this.OkayButton.UseUnderline = true;
            this.OkayButton.Label = "Add";
            this.OkayButton.Clicked += OkayButton_Clicked;
            this.TableFormButtons.Add(this.OkayButton);
            global::Gtk.Table.TableChild ob = ((global::Gtk.Table.TableChild)(this.TableFormButtons[this.OkayButton]));
            ob.LeftAttach = ((uint)(1));
            ob.RightAttach = ((uint)(1));

            this.CancelButton = new global::Gtk.Button();
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.CanFocus = true;
            this.CancelButton.UseUnderline = true;
            this.CancelButton.Label = "cancel";
            this.CancelButton.Clicked += CancelButton_Clicked;
            this.TableFormButtons.Add(this.CancelButton);
            global::Gtk.Table.TableChild cb = ((global::Gtk.Table.TableChild)(this.TableFormButtons[this.CancelButton]));
            cb.LeftAttach = ((uint)(1));
            cb.RightAttach = ((uint)(2));

            this.TableHolder.Add(this.TableFormButtons);
            global::Gtk.Box.BoxChild tfbbc = ((global::Gtk.Box.BoxChild)(this.TableHolder[this.TableFormButtons]));
            tfbbc.Position = 2;
            tfbbc.Expand = false;
            tfbbc.Fill = false;
            
            this.Add(this.TableHolder);
            if((this.Child != null))
            {
                this.Child.ShowAll();
            }
            this.Show();
            this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);

            Console.WriteLine("Pop-up Gui built.");
        }
    }
}

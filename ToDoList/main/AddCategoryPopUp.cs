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
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Gtk;
using System.Reflection;

/// <summary>
/// Add category pop up. fully implemented,
/// could do with redesigning.
/// </summary>

namespace ToDoList
{

    public partial class AddCategoryPopUp : Gtk.Window
    {
    	
        public string resultFile = System.IO.Path.GetTempFileName();
        public string resultDir = System.IO.Path.GetTempPath();

        public bool fileWritten = false;
        public bool formModified = false;
      
        public AddCategoryPopUp() : base (Gtk.WindowType.Toplevel) 
        {
            BuildACPopUp();

            createTempFiles();
            
        }
        public void AddCategoryDialog()
        {
            AddCategoryErrorDialog aced = new AddCategoryErrorDialog();
            aced.Show();
        }
        public void createTempFiles()
        {
            
            Console.WriteLine(resultDir);
            
            Directory.CreateDirectory(resultDir);

            
            try
            {
               // File.Create(resultFile);
                Console.WriteLine(resultFile);
            } catch (DirectoryNotFoundException direx)
            {
                Console.WriteLine("Dir not found... " + direx.Message);
            }
        }
        void OkayButton_Clicked(object sender, EventArgs e)
        {
            if(CategoryName.Text == "")
            {
                AddCategoryDialog();
            }
            else
            {
                WriteFile();
                this.Destroy();
                del();
            }
        }


        void CancelButton_Clicked(object sender, EventArgs e)
        {
			Thread cfct = new Thread(new ThreadStart(CheckForConfirm));
			AddCategoryConfirmationDialog accd = new AddCategoryConfirmationDialog();

			if (formModified == true) {
				GuiWorker.Worker (3, true);
				accd.Show();
				cfct.Start ();
			} else if (formModified == false) {
				this.Destroy ();
				GlobalGuiVars.AddCategoryOpened = false;
				GuiWorker.Worker (1, false);
				GuiWorker.Worker (2, false);
			}

        }

		void CheckForConfirm ()
		{
			while (GlobalGuiVars.AddCategoryPopUp_ConfirmClose == false) {
				if (GlobalGuiVars.notclosing == true) {
					break;
				}

			} if (GlobalGuiVars.AddCategoryPopUp_ConfirmClose == true) {
				GuiWorker.Worker (2, false);
				GuiWorker.Worker (1, false);
				GlobalGuiVars.AddCategoryPopUp_ConfirmClose = false;
				GlobalGuiVars.AddCategoryOpened = false;
				this.Destroy ();
			}
			if (GlobalGuiVars.notclosing == true) {
				GuiWorker.Worker (3, false);
				GlobalGuiVars.notclosing = false;
			}

		}

	
        public void OnDeleteEvent(object sender, DeleteEventArgs e)
        {
			this.Destroy ();
			GlobalGuiVars.AddCategoryOpened = false;
			GuiWorker.Worker (1, false);
			GuiWorker.Worker (2, false);
        }
        public void del(){
            GlobalGuiVars.AddCategoryOpened = false;
            GlobalGuiVars.AddCategoryPopUp_JustClosed = true;
            GlobalGuiVars._AddCategoryTemporaryFileLocation = resultFile;
            GuiWorker.Worker(1, true);
        }   
        void WriteFile()
        {
			GlobalGuiVars.hasBeenGenerated = 0;
			Console.WriteLine (GlobalGuiVars.hasBeenGenerated);
            var fileOutput = "<Category>\n\t<ID>"
                + GlobalGuiVars.TotalCategories
                + "</ID>\n\t<name>"
                + CategoryName.Text + "</name>\n\t<description>"
                + CategoryDescription.Text
                + "</description>\n</Category>";

            try
            {
                File.WriteAllText(resultFile, fileOutput);
                Console.WriteLine(resultFile + ":\n===========================\n" + fileOutput);
            }
            catch (FileLoadException fle)
            {
                Console.WriteLine("Found but cannot be loaded" + fle.Message + "\n=======" + fle.StackTrace + "\n" + fle.Data);
            }

            GlobalGuiVars.TotalCategories++;
            GlobalGuiVars._AddCategoryTemporaryFileLocation = resultFile;
        }
        void TextInserted(object o, TextInsertedArgs args)
        {
            if(this.CategoryName.Text != "" || this.CategoryDescription.Text != "")
            {
                this.Title = " Add Category ( * )";
                formModified = true;
            }
        }
        void TextDeleted(object o, TextDeletedArgs args)
        {
            if (this.CategoryName.Text == "" && this.CategoryDescription.Text == "")
            {
                this.Title = " Add Category";
                formModified = false;
            }
        }
    }

}

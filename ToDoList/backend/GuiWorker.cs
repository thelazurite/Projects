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
using System.Threading;
using System.IO;

namespace ToDoList
{
    public static class GuiWorker
    {

        public static void Worker(int jobId, bool enable)
        {
			if (jobId == 1) {
				Thread checker = new Thread (new ThreadStart (GuiWorker.CheckAddClosed));
				if (enable == true) {
					checker.Start ();
				} else if (enable == false) {
					checker.Abort ();
					Console.WriteLine ("stopped");
				}
			} else if (jobId == 2) {
				Thread catPopup = new Thread (new ThreadStart (GuiWorker.OpenAddCategoryPopUp));
				if (enable == true) {
					catPopup.Start ();
				} else if (enable == false) {
					catPopup.Abort ();
					Console.WriteLine ("stopped");
				}
			} else if (jobId == 3) {
				Thread chkConfirm = new Thread (new ThreadStart (GuiWorker.CheckConfirmation));
				if (enable == true) {
					chkConfirm.Start ();
				} else if (enable == false) {
					chkConfirm.Abort ();
					Console.WriteLine ("stopped");
				}
			} else {
				Console.WriteLine ("Error: JobID Does not Exist");
			}

            
        }
        public static void OpenAddCategoryPopUp()
        {
            Console.WriteLine("== Category Pop up thread ==");

        }
        public static void CheckAddClosed()
		{
			Console.WriteLine ("== checking thread started ==");
            
			while (GlobalGuiVars.AddCategoryPopUp_JustClosed == true) {
				if (GlobalGuiVars.n10 == false) {
					Console.WriteLine ("=== ADD CATEGORY DIALOG JUST CLOSED  ===");
                    
					string[] lines = File.ReadAllLines (GlobalGuiVars._AddCategoryTemporaryFileLocation);
					if (lines [0].Equals ("<Category>")) {

						GlobalGuiVars.tempCatIdStore = lines [1];
						GlobalGuiVars.tempCatIdStore = GlobalGuiVars.tempCatIdStore.Replace ("<ID>", "");
						GlobalGuiVars.tempCatIdStore = GlobalGuiVars.tempCatIdStore.Replace ("</ID>", "");
						GlobalGuiVars.tempCatIdStore = GlobalGuiVars.tempCatIdStore.Replace ("\t", "");

						GlobalGuiVars.tempCatNameStore = lines [2];
						GlobalGuiVars.tempCatNameStore = GlobalGuiVars.tempCatNameStore.Replace ("<name>", "");
						GlobalGuiVars.tempCatNameStore = GlobalGuiVars.tempCatNameStore.Replace ("</name>", "");
						GlobalGuiVars.tempCatNameStore = GlobalGuiVars.tempCatNameStore.Replace ("\t", "");

						GlobalGuiVars.tempCatDescriptionStore = lines [3];
						GlobalGuiVars.tempCatDescriptionStore = GlobalGuiVars.tempCatDescriptionStore.Replace ("<description>", "");
						GlobalGuiVars.tempCatDescriptionStore = GlobalGuiVars.tempCatDescriptionStore.Replace ("</description>", "");
						GlobalGuiVars.tempCatDescriptionStore = GlobalGuiVars.tempCatDescriptionStore.Replace ("\t", "");

						Console.WriteLine (GlobalGuiVars.tempCatIdStore 
						                   + " : " 
						                   + GlobalGuiVars.tempCatNameStore 
						                   + "\n============================\n" 
						                   + GlobalGuiVars.tempCatDescriptionStore 
						                   + "\n============================\n"
					    );
						
						File.Delete (GlobalGuiVars._AddCategoryTemporaryFileLocation);
						Console.WriteLine ("File: " + GlobalGuiVars._AddCategoryTemporaryFileLocation + " deleted.");

						GlobalGuiVars._AddCategoryTemporaryFileLocation = "";
                        
						GlobalGuiVars.AddCategoryPopUp_JustClosed = false;
						
						for (int i = 0; i < 5; i++) {
							Console.Write (lines [i]);
							lines [i].Remove (i);
							Console.Write ("\nremoved line from array\n");
						}
						
					}
					GlobalGuiVars.n10 = true;
				}
			}
			if (GlobalGuiVars.AddCategoryPopUp_JustClosed == false) {
				GuiWorker.Worker (1, false);

				Console.WriteLine (GlobalGuiVars.tempCatIdStore
				+ " : "
				+ GlobalGuiVars.tempCatNameStore
				+ "\n============================\n"
				+ GlobalGuiVars.tempCatDescriptionStore
				+ "\n============================\n"
				);

				GlobalGuiVars.AddCategoryPopUp_JustClosedSecondPhase = true;
			}
            
		}
        public static void CheckConfirmation()
        {
			while (GlobalGuiVars.AddCategoryPopUp_ConfirmClose == false) {

			} if (GlobalGuiVars.AddCategoryPopUp_ConfirmClose == true) {
				GuiWorker.Worker (3, false);
			}
        }
	}
}


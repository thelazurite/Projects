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
using System.IO;

namespace ToDoList
{
    public static class GlobalGuiVars
    {
		public static volatile int hasBeenGenerated;

        static volatile int _Categories;
        
        public static int TotalCategories
        {
            get
            {
                return _Categories;
            }
            set
            {
                _Categories = value;
            }
        }

        public static volatile bool AddCategoryOpened = false;
        public static volatile bool AddCategoryPopUp_JustClosed = false;
        public static volatile bool AddCategoryPopUp_JustClosedSecondPhase = false;

        public static volatile bool AddCategoryPopUp_ConfirmClose = false;
		public static volatile bool notclosing = false;

        public static volatile bool n10 = false;
        public static volatile bool n11 = false;
		public static volatile bool n12 = false;

        public static volatile string _AddCategoryTemporaryFileLocation;
        public static volatile string tempTextStore;
        public static volatile string tempCatIdStore;
        public static volatile string tempCatNameStore;
        public static volatile string tempCatDescriptionStore;
        
        public static volatile string tempToDoIdStore;
        public static volatile string tempToDoNameStore;
        public static volatile string tempToDoCatStore;
        public static volatile string tempToDoPriorityStore;
        public static volatile string tempToDoStartStore;
        public static volatile string tempToDoFinishStore;

		public static volatile string idOfItemToModify;
		public static volatile string nameOfItemToModify;
		public static volatile string descriptionOfItemToModify;

		public static volatile global::Gtk.ListStore todoListItemsStore;

		public static volatile bool addToDoItemOpen = false;
		
        public static volatile bool AddToDoItemPopUp_JustClosed = false;
        public static volatile bool AddToDoItemPopUp_JustClosedSecondPhase = false;
        
        public static volatile bool AddToDoItemPopUp_ConfirmClose = false;
        
		public static volatile bool calendarOpen = false;
		public static volatile bool calendarJustClosed = false;
		
		public static volatile string dateToAdd = " ";
		public static volatile int calendarDateType = 0;

		public static DateTime TempStartDate;
		public static DateTime TempFinishDate;
    }
}

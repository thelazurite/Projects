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
	public class ToDoItem
	{
		public ToDoItem(string id, string name, string category, string priority, string start, string end)
		{
			this.ID = id;
			this.Name = name;
			this.Category = category;
			this.Priority = priority;
			this.Start = start;
			this.Finish = end;
		}
		
		public string ID;
		public string Name;
		public string Category;
		public string Priority;
		public string Start;
		public string Finish;
	}
	public class Category 
	{
		public Category(string id, string name, string description, bool active){
			this.CategoryID = id;
			this.CategoryName = name;
			this.CategoryDescription = description;
			this.CategoryActive = active;
		}
		
		public string CategoryID;
		public string CategoryName;
		public string CategoryDescription;
		public bool CategoryActive;
	}
}

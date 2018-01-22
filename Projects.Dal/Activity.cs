
// MIT License
//
// Copyright (c) 2017 Dylan Eddies
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Data.SQLite;

namespace Projects.Dal
{
    public class Activity : DbItem
    {
        public String Category { get; set; }
        public String Description { get; set; }
        public String Name { get; set; }
        public String Priority { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }

        public Activity(String id, String name, String description, String category, String priority,
            DateTime startDate,
            DateTime end)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Priority = priority;
            StartDate = startDate;
            DueDate = end;
        }

        public override void Add()
        {
            SqlAdd = new SQLiteCommand(
                $@"insert into tblTodoItems(todoId, todo, description, itemPriority,category, startDate, dueDate)
                values(@id, @taskName, @taskDescription, @priority, @category, @startDate, @dueDate)");

            SqlAdd.Parameters.Add(new SQLiteParameter("@id", Id));
            SqlAdd.Parameters.Add(new SQLiteParameter("@taskName", Name));
            SqlAdd.Parameters.Add(new SQLiteParameter("@taskDescription", Description));
            SqlAdd.Parameters.Add(new SQLiteParameter("@priority", Priority));
            SqlAdd.Parameters.Add(new SQLiteParameter("@category", Category));
            SqlAdd.Parameters.Add(new SQLiteParameter("@startDate", StartDate.ToString("yyyy-MM-ddTHH:mm:ss")));
            SqlAdd.Parameters.Add(new SQLiteParameter("@dueDate", DueDate.ToString("yyyy-MM-ddTHH:mm:ss")));

            base.Add();
        }

        public override void Delete()
        {
            SqlDelete = new SQLiteCommand("delete from tblTodoItems where todoId = @id");
            SqlDelete.Parameters.Add(new SQLiteParameter("@id", Id));

            base.Delete();
        }
    }
}
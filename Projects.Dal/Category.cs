
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
    public class Category : DbItem
    {
        public Boolean CategoryActive;
        public String CategoryDescription;

        public String CategoryName;

        public Category(String id, String name, String description, Boolean active)
        {
            Id = id;
            CategoryName = name;
            CategoryDescription = description;
            CategoryActive = active;
        }

        public override void AddToDb()
        {
            SqlAdd = new SQLiteCommand("insert into tblCategories(categoryId, category, description)" +
                                       "values(@id, @categoryName, @categoryDescription)");
            SqlAdd.Parameters.Add(new SQLiteParameter("@id", Id));
            SqlAdd.Parameters.Add(new SQLiteParameter("@categoryName", CategoryName));
            SqlAdd.Parameters.Add(new SQLiteParameter("@categoryDescription", CategoryDescription));
            base.AddToDb();
        }

        public override void DeleteFromDb()
        {
            SqlDelete = new SQLiteCommand("delete from tblCategories where categoryId = @id");
            SqlDelete.Parameters.Add(new SQLiteParameter("@id", Id));
            base.DeleteFromDb();
        }
    }
}

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
using System.Collections.Generic;

namespace Projects.Dal.Sqlite
{
    public static class SqliteSchema
    {
        private const Int32 SchemaVersion = 1;

        private static readonly Dictionary<string, string> DatabaseSchema = new Dictionary<string, string>
        {
            {
                "Create_Version_Table", @"create table tblSchemaVersion 
                                            (id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            version int, dateUpdated datetime)"
            },

            {
                "Create_Category_Table", @"create table tblCategories 
                                            (categoryId varchar(36), category varchar(150),
                                            description varchar(1024))"
            },

            {
                "Create_Category_FK", @"create unique index catUniqueId 
                                            on tblCategories (categoryId)"
            },

            {
                "Create_Activity_Table", @"create table tblTodoItems
                                            (todoId varchar(36), todo varchar(150), 
                                            description varchar(1024), itemPriority varchar(50), 
                                            startDate datetime, dueDate datetime)"
            },

            {
                "Create_Activity_FK", @"create unique index todoUniqueId
                                            on tblTodoItems (todoId)"
            },

            {
                "Create_Activity_Category_Link", @"create table lnkActivityCategory
                                                    (id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                                    todoId varchar(36), categoryId varchar(36),
                                                    FOREIGN KEY (todoId) REFERENCES tbltodoitems(todoId),
                                                    FOREIGN KEY (categoryId) REFERENCES tblcategories(categoryId))"
            },
            
            {
                "Insert_Category_Defaults", $@"insert into tblCategories
                                                (categoryId, category, description) 
                                                values('{Guid.Empty}','None','No Category')"
            },
            
            {
                "Insert_SchemaVersion_Defaults", $@"insert into tblschemaversion (version,dateUpdated) 
                                                        values ({SchemaVersion}, '{DateTime.UtcNow}')"
            }
        };

        public static Dictionary<String, String> GetSchema() => DatabaseSchema;
    }
}

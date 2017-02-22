using System;
using System.Data.SQLite;

namespace Projects.main.backend
{
    public class Task : DbItem
    {
        public string Category;
        public string Description;
        public DateTime DueDate;

        public string Name;
        public string Priority;
        public DateTime StartDate;

        public Task(string id, string name, string description, string category, string priority, DateTime startDate,
            DateTime end) : base("Task")
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Priority = priority;
            StartDate = startDate;
            DueDate = end;
        }

        public override void AddToDb()
        {
            SqlAdd = new SQLiteCommand(
                "insert into tblTodoItems(todoId, todo, description, itemPriority,category, startDate, dueDate)" +
                "values(@id, @taskName, @taskDescription, @priority, @category, @startDate, @dueDate)");

            SqlAdd.Parameters.Add(new SQLiteParameter("@id", Id));
            SqlAdd.Parameters.Add(new SQLiteParameter("@taskName", Name));
            SqlAdd.Parameters.Add(new SQLiteParameter("@taskDescription", Description));
            SqlAdd.Parameters.Add(new SQLiteParameter("@priority", Priority));
            SqlAdd.Parameters.Add(new SQLiteParameter("@category", Category));
            SqlAdd.Parameters.Add(new SQLiteParameter("@startDate", StartDate.ToString("yyyy-MM-ddTHH:mm:ss")));
            SqlAdd.Parameters.Add(new SQLiteParameter("@dueDate", DueDate.ToString("yyyy-MM-ddTHH:mm:ss")));

            base.AddToDb();
        }

        public override void DeleteFromDb()
        {
            SqlDelete = new SQLiteCommand("delete from tblTodoItems where todoId = @id");
            SqlDelete.Parameters.Add(new SQLiteParameter("@id", Id));

            base.DeleteFromDb();
        }
    }
}
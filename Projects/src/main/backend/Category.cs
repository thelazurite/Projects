using System.Data.SQLite;

namespace Projects.main.backend
{
    public class Category : DbItem
    {
        public bool CategoryActive;
        public string CategoryDescription;

        public string CategoryName;

        public Category(string id, string name, string description, bool active) : base("Category")
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
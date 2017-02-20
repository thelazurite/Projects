using System.Data.SQLite;

namespace Projects.main.backend
{
    public class DbItem
    {
        public string Type;
        public string Id;

        public bool BeingEdited;
        public bool IsAdding { get; set; }
        public bool IsDeleting { get; set; }
        public bool IsModifying { get; set; }

        public SQLiteCommand SqlAdd { get; protected set; }
        public SQLiteCommand SqlDelete { get; protected set; }
        public SQLiteCommand SqlModify { get; protected set; }

        /// <summary>
        /// Creates a database Item. 
        /// </summary>
        /// <param name="type">The type of database item being created</param>  
        public DbItem(string type)
        {
            Type = type;
            IsAdding = false;
            IsDeleting = false;
            IsModifying = false;
        }

        /// <summary>
        /// Handles database item addition logic
        /// </summary>
        public virtual void AddToDb()
        {
            IsAdding = true;
        }

        /// <summary>
        /// Handles database item deletion logic
        /// </summary>
        public virtual void DeleteFromDb()
        {
            IsDeleting = true;
        }

        /// <summary>
        /// Handles database item modification
        /// </summary>
        internal void ModifyInDb()
        {
            IsModifying = true;
        }

        internal void Complete()
        {
            IsAdding = false;
            IsDeleting = false;
            IsModifying = false;
        }

        /// <summary>
        /// Replaces Special characters so the DB doesn't crash/explode
        /// </summary>
        /// <param name="str">String to have characters replaced</param>
        /// <returns>String without special characters</returns>
        public static string ReplaceSpecialChars(string str)
            => str.Replace("'", "&#39;").Replace("/", "&frasl;").Replace("\\", "&#92;");

        /// <summary>
        /// Restores Special characters so the user doesn't panic.
        /// </summary>
        /// <param name="str">String to have characters restored</param>
        /// <returns>String with special characters</returns>
        public static string RestoreSpecialChars(string str)
            => str.Replace("&#39;", "'").Replace("&frasl;", "/").Replace("&#92;", "\\");

    }
}

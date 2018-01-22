
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
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using Projects.Dal.Sqlite;

namespace Projects.Dal.SQlite
{
    public class SqliteContext : IDisposable
    {
        private readonly string _filePath; 
        private readonly SQLiteConnection _connection;
        private SchemaVersion _schemaVersion; 

        /// <summary>
        /// creates a context of the Projects Database.
        /// </summary>
        /// <param name="filePath"></param>
        public SqliteContext(string filePath)
        {
            _filePath = filePath;
            _connection = new SQLiteConnection($"Data Source={filePath};Version=3;");
        }

        private SchemaVersion GetSchemaVersion()
        {
            _connection.Open();

            var version = Fetch(SchemaVersion());

            _connection.Close();
            return version;
        }

        private SQLiteDataReader SchemaVersion()
        {
            const string sqlSmt = @"select max(dateUpdated) as maxDateUpdated , version
                            from tblschemaversion
                            group by dateUpdated  limit 1;";

            var execute = new SQLiteCommand(sqlSmt, _connection);
            var reader = execute.ExecuteReader();
            return reader;
        }

        private static SchemaVersion Fetch(IDataReader reader)
        {
            var version = new SchemaVersion();
            while (reader.Read())
            {
                Int32.TryParse(reader["version"]?.ToString(), out var versionNumber);
                DateTime.TryParse(reader["dateUpdated"].ToString(), CultureInfo.CurrentCulture,
                    DateTimeStyles.AssumeUniversal, out var dateUpdated);

                version.VersionNumber = versionNumber;
                version.DateUpdated = dateUpdated;
            }

            return version;
        }

        public void CreateSchema()
        {
            SQLiteConnection.CreateFile(_filePath);
            
            _connection.Open();
            foreach (var sql in SqliteSchema.GetSchema().Values)
            {
                var execute = new SQLiteCommand(sql, _connection);
                execute.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

         ~SqliteContext()
        {
            Dispose();
        }
    }
}
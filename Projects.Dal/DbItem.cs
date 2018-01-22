
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

﻿using System;
using System.Data.SQLite;

namespace Projects.Dal
{
    public class DbItem
    {
        public String Id;

        public Boolean IsAdding { get; set; }
        public Boolean IsDeleting { get; set; }
        public Boolean IsModifying { get; set; }

        public SQLiteCommand SqlAdd { get; protected set; }
        public SQLiteCommand SqlDelete { get; protected set; }
        public SQLiteCommand SqlModify { get; protected set; }

        /// <summary>
        ///     Handles database item addition logic
        /// </summary>
        public virtual void Add()
        {
            IsAdding = true;
        }

        /// <summary>
        ///     Handles database item deletion logic
        /// </summary>
        public virtual void Delete()
        {
            IsDeleting = true;
        }

        /// <summary>
        ///     Handles database item modification
        /// </summary>
        internal void Modify()
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
        ///     Replaces Special characters so the DB doesn't crash/explode
        /// </summary>
        /// <param name="str">String to have characters replaced</param>
        /// <returns>String without special characters</returns>
        public static String ReplaceSpecialChars(String str)
            => str.Replace("'", "&#39;").Replace("/", "&frasl;").Replace("\\", "&#92;");

        /// <summary>
        ///     Restores Special characters so the user doesn't panic.
        /// </summary>
        /// <param name="str">String to have characters restored</param>
        /// <returns>String with special characters</returns>
        public static String RestoreSpecialChars(String str)
            => str.Replace("&#39;", "'").Replace("&frasl;", "/").Replace("&#92;", "\\");
    }
}
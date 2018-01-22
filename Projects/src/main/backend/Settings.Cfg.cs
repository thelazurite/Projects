
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
using System.Configuration;

namespace Projects.Gtk.main.backend {
    internal sealed partial class Settings : ApplicationSettingsBase
    {
        public static Settings Default { get; } = (Settings)Synchronized(new Settings());

        /// <summary>
        /// should a file be loaded at startup? 
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean LoadOnStartup {
            get => (Boolean)this["LoadOnStartup"];
            set => this["LoadOnStartup"] = value;
        }
        
        /// <summary>
        /// file to be loaded on startup
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public String FileOnStartup {
            get => (String)this["FileOnStartup"];
            set => this["FileOnStartup"] = value;
        }
        
        /// <summary>
        /// Is the application multi window?
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean MultiWindow {
            get => (Boolean)this["MultiWindow"];
            set => this["MultiWindow"] = value;
        }
        
        /// <summary>
        /// Previously browsed file within the application
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public String PreviousBrowseFolder {
            get => (String)this["PreviousBrowseFolder"];
            set => this["PreviousBrowseFolder"] = value;
        }
        
        /// <summary>
        /// files which have been opened in the application before
        /// </summary>
        [UserScopedSetting]
        public System.Collections.Specialized.StringCollection PreviouslyOpenedFiles {
            get => (System.Collections.Specialized.StringCollection)this["PreviouslyOpenedFiles"];
            set => this["PreviouslyOpenedFiles"] = value;
        }
    }
}

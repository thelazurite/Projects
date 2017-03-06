using System.Configuration;

namespace Projects.main.backend {
    internal sealed partial class Settings : ApplicationSettingsBase
    {
        public static Settings Default { get; } = (Settings)Synchronized(new Settings());

        /// <summary>
        /// should a file be loaded at startup? 
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public bool LoadOnStartup {
            get {
                return (bool)this["LoadOnStartup"];
            }
            set {
                this["LoadOnStartup"] = value;
            }
        }
        
        /// <summary>
        /// file to be loaded on startup
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string FileOnStartup {
            get {
                return (string)this["FileOnStartup"];
            }
            set {
                this["FileOnStartup"] = value;
            }
        }
        
        /// <summary>
        /// Is the application multi window?
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public bool MultiWindow {
            get {
                return (bool)this["MultiWindow"];
            }
            set {
                this["MultiWindow"] = value;
            }
        }
        
        /// <summary>
        /// Previously browsed file within the application
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string PreviousBrowseFolder {
            get {
                return (string)this["PreviousBrowseFolder"];
            }
            set {
                this["PreviousBrowseFolder"] = value;
            }
        }
        
        /// <summary>
        /// files which have been opened in the application before
        /// </summary>
        [UserScopedSetting]
        public System.Collections.Specialized.StringCollection PreviouslyOpenedFiles {
            get {
                return (System.Collections.Specialized.StringCollection)this["PreviouslyOpenedFiles"];
            }
            set {
                this["PreviouslyOpenedFiles"] = value;
            }
        }
    }
}

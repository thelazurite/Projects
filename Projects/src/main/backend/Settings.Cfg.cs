using System.Configuration;

namespace Projects.main.backend {
    internal sealed partial class Settings : ApplicationSettingsBase
    {
        public static Settings Default { get; } = ((Settings)(Synchronized(new Settings())));

        [UserScopedSetting()]
        [DefaultSettingValue("False")]
        public bool LoadOnStartup {
            get {
                return ((bool)(this["LoadOnStartup"]));
            }
            set {
                this["LoadOnStartup"] = value;
            }
        }
        
        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string FileOnStartup {
            get {
                return ((string)(this["FileOnStartup"]));
            }
            set {
                this["FileOnStartup"] = value;
            }
        }
        
        [UserScopedSetting()]
        [DefaultSettingValue("False")]
        public string[] MultiWindow {
            get {
                return ((string[])(this["MultiWindow"]));
            }
            set {
                this["MultiWindow"] = value;
            }
        }
        
        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string PreviousBrowseFolder {
            get {
                return ((string)(this["PreviousBrowseFolder"]));
            }
            set {
                this["PreviousBrowseFolder"] = value;
            }
        }
        
        [UserScopedSetting()]
        public System.Collections.Specialized.StringCollection PreviouslyOpenedFiles {
            get {
                return ((System.Collections.Specialized.StringCollection)(this["PreviouslyOpenedFiles"]));
            }
            set {
                this["PreviouslyOpenedFiles"] = value;
            }
        }
    }
}

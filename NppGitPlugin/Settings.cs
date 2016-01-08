using System.Diagnostics;
using System.IO;

namespace NppGit
{
    public static class Settings
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static IniFile _file;

        #region "Get/Set"
        // Загрузка/сохранение происходит по имени класса и свойства
        // Имена получаются через стек и рефлексию
        private static void Set<T>(T value)
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var className = mth.ReflectedType.Name;
            var propName = mth.Name.Replace("set_", "");
            logger.Debug("Save: Section={0}, Key={1}, Value={2}", className, propName, value);
            _file.SetValue(className, propName, value);
        }
        private static T Get<T>(T defaultValue)
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var className = mth.ReflectedType.Name;
            var propName = mth.Name.Replace("get_", "");
            T value = _file.GetValue(className, propName, defaultValue);
            logger.Debug("Load: Section={0}, Key={1}, Value={2}", className, propName, value);
            return value;
        }
        #endregion

        #region "Common"

        public static void Init()
        {
            logger.Debug("Init settings");
            var iniPath = Path.Combine(PluginUtils.ConfigDir, Properties.Resources.PluginName + ".ini");
            logger.Debug("Plugin setting file: {0}", iniPath);
            _file = new IniFile(iniPath);
        }
        #endregion

        #region "Settings classes"
        public static class Functions
        {
            public static bool ShowBranch
            {
                get { return Get(false); }
                set { Set(value); }
            }

            public static byte SHACount
            {
                get { return Get((byte)6); }
                set { Set(value); }
            }

            public static bool OpenFileInOtherView
            {
                get { return Get(false); }
                set { Set(value); }
            }
        }

        public static class Panels
        {
            public static bool StatusPanelVisible
            {
                get { return Get(false); }
                set { Set(value); }
            }
        }

        public static class TortoiseGitProc
        {
            public static string Path
            {
                get { return Get(""); }
                set { Set(value); }
            }
            public static bool ShowToolbar
            {
                get { return Get(false); }
                set { Set(value); }
            }
            public static uint ButtonMask
            {
                get { return Get(0u); }
                set { Set(value); }
            }
        }

        public static class InnerSettings
        {
            public static bool IsSetDefaultShortcut
            {
                get { return Get(false); }
                set { Set(value); }
            }
        }
        #endregion
    }
}

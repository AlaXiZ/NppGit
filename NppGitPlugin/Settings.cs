using System.Diagnostics;
using System.IO;

namespace NppGit
{
    public class Settings
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static Settings _inst = new Settings();
        private static IniFile _file;

        #region "Get/Set"
        // Загрузка/сохранение происходит по имени класса и свойства
        // Имена получаются через стек и рефлексию
        protected static void Set<T>(T value)
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var className = mth.ReflectedType.Name;
            var propName = mth.Name.Replace("set_", "");
            logger.Debug("Save: Section={0}, Key={1}, Value={2}", className, propName, value);
            _file.SetValue(className, propName, value);
        }
        protected static T Get<T>(T defaultValue)
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
        private Settings()
        {
            FuncSet = new Functions();
            PanelsSet = new Panels();
            TortoiseGit = new TortoiseGitProc();
        }

        public static Settings Instance { get { return _inst; } }

        public static void Init()
        {
            logger.Debug("Init settings");
            var iniPath = Path.Combine(PluginUtils.ConfigDir, Properties.Resources.PluginName + ".ini");
            logger.Debug("Plugin setting file: {0}", iniPath);
            _file = new IniFile(iniPath);
        }
        #endregion

        #region "Settings classes"
        public class Functions
        {
            public bool ShowBranch
            {
                get { return Get(false); }
                set { Set(value); }
            }
        }

        public class Panels
        {
            public bool StatusPanelVisible
            {
                get { return Get(false); }
                set { Set(value); }
            }
        }

        public class TortoiseGitProc
        {
            public string Path
            {
                get { return Get(""); }
                set { Set(value); }
            }
            public bool ShowToolbar
            {
                get { return Get(false); }
                set { Set(value); }
            }
            public uint ButtonMask
            {
                get { return Get(0u); }
                set { Set(value); }
            }
        }
        #endregion

        #region "Properties"
        public Functions FuncSet { get; protected set; }
        public Panels PanelsSet { get; protected set; }
        public TortoiseGitProc TortoiseGit { get; protected set; }
        #endregion

    }
}

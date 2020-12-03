namespace UIFrame
{
    public enum UIModuleType
    {
        SingleControl,//单窗口控制模式
        MultipleControl//多窗口控制模式
    }
    public class SystemDefine
    {
        /// <summary>
        /// 模块路径配置表
        /// </summary>
        public const string UIPanelConfigPath = "Configuration/ModulePathConfig";
        public const string SkillDataConfigPath = "Configuration/SkillDataJson";
        public const string UITextLocalizationConfigPath = "Configuration/LocalizationTexts";
        public const string UIWidgetConfigPath = "Configuration/WidgetPathConfig";
        //重要UI元素的末尾标记
        public static string[] IMPROTANT_WIDGET_TOKEN;
        public const string SECONDWIDGETS_TOKEN = "~";
        public const string PLAYERPREFS_LANGUAGEID = "LanguagesID";
        public const bool UseLocalization = false;
        /// <summary>
        /// 不参与本地化的头部标记
        /// </summary>
        public const string NotInvolvedInLocalization = "$";
        static SystemDefine()
        {
            IMPROTANT_WIDGET_TOKEN = new string[] { "#", "!", "*" ,"_F"};
        }
    }
}
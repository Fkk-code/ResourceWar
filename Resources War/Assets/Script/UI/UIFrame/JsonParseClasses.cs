using System;
namespace UIFrame
{
    [Serializable]
    public class UIModulePathModel
    {
        public UIModuleMsg[] UIPanels;
    }
    [Serializable]
    public class UIModuleMsg
    {
        public string PanelName;
        public string PanelPath;
    }
    [Serializable]
    public class UILocaliztionTextModel
    {
        public LZText[] localizationTexts;
    }
    [Serializable]
    public class LZText
    {
        public string[] texts;
    }

    [Serializable]
    public class UIWidgetPathModel
    {
        public UIWidgetMsg[] UIWidgetMsgs;
    }
    [Serializable]
    public class UIWidgetMsg
    {
        public string WidgetName;
        public string WidgetPath;
    }
    [Serializable]
    public class SkillData
    {
        public SkillDataArray[] dataArrays;
    }
    [Serializable]
    public class SkillDataArray
    {
        public int SkillID;
        public int SkillLevel;
    }

}
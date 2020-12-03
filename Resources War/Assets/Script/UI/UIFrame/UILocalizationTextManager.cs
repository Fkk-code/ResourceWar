using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilty;

namespace UIFrame
{
    public class UILocalizationTextManager : Singleton<UILocalizationTextManager>
    {
       private UILocalizationTextManager() { }
       private Action<int> changeLanguageActon;
        public void AddTextActionListener(Action<int> action)
        {
            changeLanguageActon += action;
        }
        public void RemoveTextActionListener(Action<int> action)
        {
            changeLanguageActon -= action;
        }
        public void LoacalizationTextChange(int languageID)
        {
            PlayerPrefs.SetInt(SystemDefine.PLAYERPREFS_LANGUAGEID,languageID);
            if (changeLanguageActon != null)
            {
                changeLanguageActon(languageID);
            }
        }
    }
}

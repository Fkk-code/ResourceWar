using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFrame
{
    [RequireComponent(typeof(Text))]
    public class UILocalText : MonoBehaviour
    {
        private Text _text;
        [Header("当前文本字符串名称【英文】")]
        public string textString;
        private void Awake()
        {
            _text = GetComponent<Text>();
        }
        private void OnEnable()
        {
            UILocalizationTextManager.GetInstance().AddTextActionListener(ChangeLanguage);
        }
        private void OnDisable()
        {
            UILocalizationTextManager.GetInstance().RemoveTextActionListener(ChangeLanguage);
        }
        public void ChangeLanguage(int languageID)
        {
            //当前文本为Key，传目标语言id，返回目标语言文本
           _text.text = UIConfigurationManager.GetInstance().
                GetTextStringByLanguageID(textString,languageID);
        }
    }
}

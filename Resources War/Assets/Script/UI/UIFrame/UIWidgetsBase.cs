using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace UIFrame
{
    public class UIWidgetsBase : MonoBehaviour
    {
        /// <summary>
        /// 当前模块
        /// </summary>
        public UIModuleBase _currentModule;

        #region Field
        private Transform _transform;
        private RectTransform _rectTransform;
        private Text _text;
        private Image _image;
        private RawImage _rawImage;

        private Button _button;
        private InputField _inputField;
        private Slider _slider;
        private Dropdown _dropdown;
        private Toggle _toggle;
        private Scrollbar _scrollbar;
        
        private ContentSizeFitter _sizeFitter;
        #endregion

        #region Prop
        public Transform Transform { get => _transform;  }
        public Text Text { get => _text;  }
        public Image Image { get => _image;  }
        public RawImage RawImage { get => _rawImage;  }
        public Button Button { get => _button;  }
        public InputField InputField { get => _inputField; }
        public Slider Slider { get => _slider;  }
        public Dropdown Dropdown { get => _dropdown; }
        public Toggle Toggle { get => _toggle;  }
        public Scrollbar Scrollbar { get => _scrollbar;  }
        
        public ContentSizeFitter SizeFitter { get => _sizeFitter;  }
        public RectTransform RectTransform { get => _rectTransform; }
        #endregion

        private void Awake()
        {
            _transform = transform;
            try
            {
                _text = GetComponent<Text>();
                _image = GetComponent<Image>();
                _rawImage = GetComponent<RawImage>();
                _button = GetComponent<Button>();
                _inputField = GetComponent<InputField>();
                _slider = GetComponent<Slider>();
                _dropdown = GetComponent<Dropdown>();
                _toggle = GetComponent<Toggle>();
                _scrollbar = GetComponent<Scrollbar>();
               
                _rectTransform = GetComponent<RectTransform>();
                _sizeFitter = GetComponent<ContentSizeFitter>();
            }
            catch (System.Exception)
            {

            }
           
        }

        //protected virtual void OnDestroy()
        //{
        //    _currentModule.UnRegisterWidget();
        //}

    }

}
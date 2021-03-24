using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace JFrame
{

    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class TextLoader : MonoBehaviour
    {
#if STRING_HEADER_CREATED
        public H_Str.eTab Tab;
        public string Key;

        public bool Refresh;
        public bool FixedFont;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void Awake()
        {
            if (FixedFont == false)
            {
                UnityEngine.UI.Text _t = gameObject.GetComponent<UnityEngine.UI.Text>();
                _t.font = JFrame.StrMgr.Instance.CurrFont;
            }
            
            //####  Set Text
            if (string.IsNullOrEmpty(Key) == false)
            {
                string _str = JFrame.StrMgr.Instance.GetString(Tab, Key);
                ApplyText(_str);
            }

        }

        void ApplyText(string _text)
        {
            if (_text == null)
            {
                return;
            }

            UnityEngine.UI.Text _textComponent = gameObject.GetComponent<UnityEngine.UI.Text>();
            if (_textComponent == null)
            {
                return;
            }

            _textComponent.text = _text;
        }



        void OnValidate()
        {
            if (Refresh == true)
            {
                Refresh = false;

                string _strJson = JFrame.DataMGR.ReadText("Text/ko/" + Tab.ToString());
                if (string.IsNullOrEmpty(_strJson) == true)
                {
                    return;
                }

                JsonData _strData = JsonMapper.ToObject(_strJson);
                if (_strData == null)
                {
                    return;
                }

                if (JFrame.ComLib.HasKeyInJson(_strData, Key) == false)
                {
                    return;
                }


                string _value = _strData[Key].ToString();
                ApplyText(_value);
            }

        }

        public virtual void Redraw ()
        {

        }
#endif
    }



}

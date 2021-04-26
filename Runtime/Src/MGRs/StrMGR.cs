using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

using LitJson;

namespace JFrame
{



    public class StrMgr : Singleton<StrMgr>
    {
        public string Locale { get; set; }
        JsonData StrData;
        public Font CurrFont { get; set; }

        void Start()
        {
            //Init by Phone setting
            
        }

        public void LoadFont (int _index)
        {
            CurrFont = DataMGR.Instance.Load<Font>(_index) as Font;

            RefreshTextLoaders();
        }

        void RefreshTextLoaders ()
        {
            //SceneMGR.Instance.OnChangeFont();
            //FloatingUIMGR.Instance.OnChangeFont();
        }

        public void Reset (string _locale = "ko")
        {
            SetLocale(_locale);
        }

        public void InitStringByUserLocale()
        {
            SetLocale("ko");
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SetLocale(string _locale)
        {
            if (Locale == _locale)
            {
                return;
            }
            Locale = _locale;
            LoadStrings();

            RefreshTextLoaders();
        }

        public string GetString (H_Str.eTab _tab, string strIdName, bool _bConvertPP = true)
        {
            string _ret = GetString((int)_tab, strIdName);

            if (_bConvertPP)
            {
                _ret = ConvertPP(_ret);
            }

            return _ret;

        }

        public string GetString (int tabId, string strIdName)
        {
            if (StrData == null || string.IsNullOrEmpty(strIdName) == true)
            {
                return null;
            }

            if (StrData.Count <= tabId)
            {
                return null;
            }

            if (StrData[tabId] == null)
            {
                return null;
            }

            string _ret = StrData[tabId][strIdName].ToString();

            
            if (Locale.ToUpper().CompareTo("HIN") == 0)
            {
                _ret = changeHindiGlyph(_ret);
            }
            return _ret;
        }

        string changeHindiGlyph(string originalText)
        {
            //https://stackoverflow.com/questions/21188046/writing-hindi-fonts-with-gd-library-do-not-render-as-desired/27502129#27502129
            string[] words = originalText.Split(new char[] { ' ' });

            for (int k = 0; k < words.Length; k++)
            {
                if (words[k].Contains("\u093f")) // check if the word contains "i" vowel
                {
                    char[] arr = words[k].ToCharArray();
                    for (int i = 0; i < arr.Length - 1; i++)
                    {
                        //interchange the order of "i" vowel
                        if (arr[i] == '\u093f')
                        {
                            arr[i] = arr[i - 1];
                            arr[i - 1] = '\u093f';
                        }
                    }

                    words[k] = new string(arr);
                }
            }

            originalText = string.Join(" ", words);

            return originalText;
        }

        public string GetString(int tabId, int strId)
        {
            if (StrData == null || strId < 0)
            {
                return null;
            }

            if (StrData.Count <= tabId)
            {
                return null;
            }

            if (StrData[tabId] == null)
            {
                return null;
            }


            return StrData[tabId][strId].ToString();
        }

        

        private void LoadStrings()
        {
            StrData = null;
            StrData = new JsonData();

            foreach (string _name in System.Enum.GetNames(typeof(H_Str.eTab)))
            //for (int i = 0; i < H_Str.eTab.Length; i++)
            {
                string _path = string.Format("Text/{0}/{1}", Locale, _name);
                JsonData _strSheet = DataMGR.Instance.ReadJson(_path);
                StrData.Add(_strSheet);
            }
        }

        public static string ConvertPP(string _text)
        {
            int _indexOfPP = _text.IndexOf("\\s00");

            while (_indexOfPP != -1)
            {
                char _t = _text[_indexOfPP - 1];

                string _strId = "S00_YI";
                if ((_t - 44032) % 28 == 0)
                {
                    _strId = "S00_GA";
                }
                string _pp = StrMgr.Instance.GetString(H_Str.eTab.Common, _strId);

                _text = _text.Replace("\\s00", _pp);
                _indexOfPP = _text.IndexOf("\\s00");
            }

            _indexOfPP = _text.IndexOf("\\s01");

            while (_indexOfPP != -1)
            {
                char _t = _text[_indexOfPP - 1];

                string _strId = "S01_UL";
                if ((_t - 44032) % 28 == 0)
                {
                    _strId = "S01_RUL";
                }
                string _pp = StrMgr.Instance.GetString(H_Str.eTab.Common, _strId);

                _text = _text.Replace("\\s01", _pp);
                _indexOfPP = _text.IndexOf("\\s01");
            }

            _indexOfPP = _text.IndexOf("\\s02");

            while (_indexOfPP != -1)
            {
                char _t = _text[_indexOfPP - 1];

                string _strId = "S02_UN";
                if ((_t - 44032) % 28 == 0)
                {
                    _strId = "S02_NUN";
                }
                string _pp = StrMgr.Instance.GetString(H_Str.eTab.Common, _strId);

                _text = _text.Replace("\\s02", _pp);
                _indexOfPP = _text.IndexOf("\\s02");
            }

            return _text;
        }

        public static string GetNumberWithCurrency(long _value)
        {
            //decimal parsed = decimal.Parse(_value.ToString(), CultureInfo.InvariantCulture);
            //CultureInfo hindi = new CultureInfo("hi-IN");
            //string text = string.Format(hindi, "{0:c}", parsed);
            //string text = string.Format(hindi, "{0:#,#}", parsed);
            //_value = 1111;
            string _postfixForDegree = "";
            int _valueForUnderPoint = 0;

            float _valueConverted = (float)_value;

            if (_valueConverted > 1000000000000L)
            {
                //use LCr
                _postfixForDegree = "LCr";
                _valueConverted = _valueConverted / 1000000000000L;
            }
            else if (_valueConverted > 10000000L)
            {
                //use Cr
                _postfixForDegree = "Cr";
                _valueConverted /= 10000000L;
            }
            else if (_valueConverted > 100000)
            {
                //Use L
                _postfixForDegree = "L";
                _valueConverted /= 100000;
            }

            StringBuilder stringBuilder = new StringBuilder();

            float _base3digit = ((int)((_valueConverted % 1000) * 100)) / 100f;

            int _valueInt = (int)_valueConverted / 1000;

            if (_valueInt > 0)
            {
                stringBuilder.AppendFormat("{0:000.##}{1}", _base3digit, _postfixForDegree);
            }
            else
            {
                stringBuilder.AppendFormat("{0:#.##}{1}", _base3digit, _postfixForDegree);
            }




            while (_valueInt > 0)
            {
                int _current2digit = _valueInt % 100;

                if (_valueInt / 100 > 0)
                {
                    stringBuilder.Insert(0, string.Format("{0:00}", _valueInt % 100) + ",");
                }
                else
                {
                    stringBuilder.Insert(0, (_valueInt % 100) + ",");
                }

                _valueInt /= 100;
            }


            return "₹" + stringBuilder.ToString();
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using LitJson;


using UnityEngine.UI;

namespace JFrame
{


    public class Debugger : Singleton<Debugger>
    {
        InputField _InputField;
        public void Reset()
        {
            if (_InputField == null)
            {
                GameObject _go = Resources.Load("Debug_InputField") as GameObject;

                if (_go != null)
                {
                    GameObject _goIf = Instantiate(_go) as GameObject;
                    _InputField = _goIf.GetComponent<InputField>();

                    if (_InputField != null)
                    {
                        _InputField.transform.parent = transform;
                        _InputField.gameObject.SetActive(false);
                        _InputField.onEndEdit.AddListener((_t) => { OnEndEdit(_t); });

                        //    .AddListener(() => { OnButtonSeat(_buttonParam); });
                        //_InputField.OnEndEdit += { } => OnEndEdit(_InputField.text);
                    }
                }
            }
        }

        void OnEndEdit(string _command)
        {
            _InputField.text = "";
            _InputField.transform.parent = transform;
            _InputField.gameObject.SetActive(false);

            string[] _toks = _command.Split(new char[] { ' ', '\r', '\n', '\t' });
            if (_toks == null || _toks.Length == 0)
            {
                return;
            }

            string _strCommand = _toks[0];
            string[] _strParams = null;

            if (_toks.Length > 1)
            {
                _strParams = new string[_toks.Length - 1];
                for (int i = 0; i < _toks.Length - 1; i++)
                {
                    _strParams[i] = _toks[i + 1];
                }
            }

            Cheat(_strCommand, _strParams);
        }


        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return) == true)
            {
                if (_InputField != null)
                {
                    if (_InputField.IsActive() == false)
                    {
                        Canvas _activeCanvas = GameObject.FindObjectOfType<Canvas>();
                        if (_activeCanvas != null)
                        {
                            _InputField.transform.parent = _activeCanvas.transform;
                            _InputField.transform.localPosition = Vector3.zero;
                            _InputField.transform.localScale = Vector3.one;
                            _InputField.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        OnEndEdit(_InputField.text);
                    }

                }

            }
        }

        void Cheat(string _cmd, string[] _params)
        {
            if (string.IsNullOrEmpty(_cmd) == true)
            {
                return;
            }

            switch (_cmd)
            {
                case "aaaa":
                    break;
            }   //end of switch

        }
    }
}

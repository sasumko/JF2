using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

using NLua;
using LitJson;

namespace JFrame
{
    

    public class PopupMGR : Singleton<PopupMGR>
    {
        Canvas CanvasInstance;

        List<CPopupBase> _listPopups;
        List<CPopupInfo> PopupInfos;

        public void Reset()
        {
            if (CanvasInstance == null)
            {
                CanvasInstance = gameObject.AddComponent<Canvas>();
                CanvasInstance.sortingOrder = 100;
                CanvasInstance.renderMode = RenderMode.ScreenSpaceOverlay;

                gameObject.AddComponent<UnityEngine.UI.CanvasScaler>();
                gameObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            }
            //#### Load PopupInfos
           PopupInfos = DataMGR.CreateList<CPopupInfo>("Json/PopupInfo");
        }

        public bool OnBackButtonEvent()
        {
            return CloseLastPopup();
        }

        public void NotifiedClosePopup (CPopupBase _p)
        {
            if (_p == null || _listPopups == null || _listPopups.Contains(_p) == false)
            {
                return;
            }

            _listPopups.Remove(_p);
        }

        public bool CloseLastPopup ()
        {
            if (_listPopups == null)
            {
                return false;
            }

            CPopupBase _lastPopup = _listPopups[_listPopups.Count - 1];
            if (_lastPopup != null && _lastPopup.OnBackButtonEvent() == true)
            {
                _lastPopup.OnClickClose();
                return true; //True : event triggered! 
            }

            return false;
        }

        public Action MakeDelegateWithClose (Action _delegate)
        {
            return () => { _delegate(); CloseLastPopup(); };

        }

        public T CreatePopup<T> (int _popupId) where T : CPopupBase, new()
        {
            T _ret = default(T);

            
            if (PopupInfos == null)
            {
                return _ret;
            }

            CPopupInfo _info = PopupInfos.Find(_item => _item.Idx == _popupId);

            if (_info == null)
            {
                return _ret;
            }

            _ret = (T) CPopupBase.CreateWithInfo(_info);

            if (_ret == null)
            {
                return _ret;
            }


            if (_listPopups == null)
            {
                _listPopups = new List<CPopupBase>();
            }
            _listPopups.Add(_ret);
            
            SetCanvasDepth(_ret.gameObject, H_DEFINES.POPUP_DEPTH_NORMAL + _listPopups.Count);
            _ret.transform.parent = transform;
            _ret.transform.localPosition = Vector3.zero;


            return _ret;
        }

        public void DismissPopups ()
        {
            if (_listPopups == null)
            {
                return;
            }

            foreach (CPopupBase _popup in _listPopups)
            {
                if (_popup == null)
                {
                    continue;
                }

                _popup.OnClickClose();
            }
        }

        public void DismissPopup<T> () where T : CPopupBase
        {
            if (_listPopups == null)
            {
                return;
            }

            List< CPopupBase> _removes = _listPopups.FindAll(_item => _item.GetType() == typeof(T));

            if (_removes == null)
            {
                return;
            }
            
            foreach (CPopupBase _popup in _removes)
            {
                if (_popup == null)
                {
                    continue;
                }

                _popup.OnClickClose();
            }
        }

        GameObject _goLoading = null;

        public void SetLoadingPopupOnOff (bool _bOnOff)
        {
            if (_goLoading == null)
            {
                _goLoading = DataMGR.Instance.Instantiate<GameObject>("LoadingPopup");
                if (_goLoading != null)
                {
                    _goLoading.transform.parent = transform;
                    SetCanvasDepth(_goLoading, H_DEFINES.POPUP_DEPTH_LOADING);
                }
            }

            if (_goLoading == null)
            {
                return;
            }

            _goLoading.SetActive(_bOnOff);
        }

        public static void SetCanvasDepth (GameObject _go, int _order)
        {
            if (_go == null)
            {
                return;
            }

            Canvas _c = _go.GetComponentInChildren<Canvas>();

            if (_c == null)
            {
                return;
            }

            _c.sortingOrder = _order;
        }

        public void DispatchMessage(string msg, object param = null)
        {
            if (_listPopups == null)
            {
                return;
            }

            CPopupBase _lastPopup = _listPopups[_listPopups.Count - 1];
            if (_lastPopup != null)
            {
                _lastPopup.SendMessage(msg, param, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    
}
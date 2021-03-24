using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JFrame
{


    public class CPopupBase : MonoBehaviour
    {
        public Text BodyTxt;
        public Button[] Buttons;
        public float Lifetime { get; set; }
        
        public bool IsNotListenToBackbuttonEvent = false;

        //private void Awake()
        //{
        //    Init();
        //}

        //protected virtual void Init ()
        //{
            
        //}
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutines();
            //_actionWhenClose = null;

        }

        protected void StartCoroutines ()
        {
            if (Lifetime > 0)
            {
                StartCoroutine(coroutineLifetimeElapse());
            }

            StartCoroutine(coroutineBindingButtons());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator coroutineBindingButtons()
        {
            yield return null;
            InitButtons();
        }

        protected virtual void InitButtons ()
        {

        }

        public void SetBodyText (string _txt)
        {
            JFrame.ComLib.SetLabel(BodyTxt, _txt);
        }

        public void SetButtons(int _index, string _caption, fnBtnEvent _cb = null, bool _bIncludeClose = true)
        {
            if (Buttons == null || _index < 0 || _index >= Buttons.Length)
            {
                return;
            }

            Button _b = Buttons[_index];

            if (_b == null)
            {
                return;
            }

            if (_cb == null || _caption == null)
            {
                _b.gameObject.SetActive(false);
                return;
            }

            //_caption == "" -> don't change
            if (_caption != string.Empty)
            {
                UnityEngine.UI.Text _t = _b.GetComponentInChildren<Text>();
                JFrame.ComLib.SetLabel(_t, _caption);
            }

            _b.onClick.AddListener(() =>
                                    {
                                        _cb();
                                    });

            if (_bIncludeClose == true)
            {
                _b.onClick.AddListener(() =>
                {
                    OnClickClose();
                });
            }
        }

        IEnumerator coroutineLifetimeElapse()
        {
            Debug.Log("Coroutine for lifetime" + gameObject.name);
            yield return new WaitForSeconds(Lifetime);

            CloseByLifeTime();
        }

        private void OnDestroy()
        {
            StopCoroutine("coroutineLifetimeElapse");
        }

        public virtual bool OnBackButtonEvent()
        {
            if (gameObject.activeSelf == false
                || IsNotListenToBackbuttonEvent == true)
            {
                return false;
            }

            OnClickClose();

            return true;    //True : event triggered! 
        }


        public delegate void fnBtnEvent();
        fnBtnEvent _actionWhenClose = null;

        public void CloseByLifeTime()
        {
            //Request to start close sequence
            //if popup closed by lifetime, this method should be called

            if (_actionWhenClose != null)
            {
                _actionWhenClose();
            }
            OnClickClose();
        }



        //##########################################
        //####  Button Events
        //##########################################



        public virtual void OnClick0()
        {
            Debug.Log("--- ok clicked");
        }
        public virtual void OnClick1()
        {
            Debug.Log("--- yes clicked");
        }

        public virtual void OnClick2()
        {
            Debug.Log("--- no clicked");
        }
        
        public virtual void OnClickClose()
        {
            //Debug.Log("--- close clicked");
            //Do something
            Destroy(gameObject);
            PopupMGR.Instance.NotifiedClosePopup(this);
        }

 

        public static CPopupBase CreateWithInfo (CPopupInfo _info)
        {
            if (_info == null)
            {
                return null;
            }

            CPopupBase _ret;

            GameObject _prefab = DataMGR.Instance.Instantiate<GameObject>(_info.ResId);

            if (_prefab == null)
            {
                return null;
            }

            _ret = _prefab.GetComponent<CPopupBase>();

            if (_ret == null)
            {
                Destroy(_prefab);
            }
            
            if (_info.LifeTime > 0)
            {
                _ret.Lifetime = _info.LifeTime;
            }
            return _ret;
        }
    }
}

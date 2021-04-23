using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace JFrame
{


    [RequireComponent(typeof(ScrollRect))]
    public class UIComponent_ScrollRect : MonoBehaviour
    {
        public CPopupBase ParentPopup;

        public eDir Direction = eDir.VERTICAL;
        public enum eDir
        {
            HORIZONTAL,
            VERTICAL
        };

        public float Spacing = 0.0f;

        List<object> Items;
        List<GameObject> ItemPrefabs;

        [SerializeField]
        private ScrollRect ScrRectAssigned = null;

        [SerializeField]
        private RectTransform RtViewportAssigned = null;
        public RectTransform RtContentAssigned = null;

        public RectTransform ItemGO;

        bool _isRefershNeeded = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        void LateUpdate()
        {
            if (_isRefershNeeded == true)
            {
                _isRefershNeeded = false;

                if (RtContentAssigned != null && ItemGO != null)
                {
                    if (Direction == eDir.HORIZONTAL)
                    {
                        RtContentAssigned.sizeDelta = new Vector2((Items == null ? 0 : (Items.Count * ItemGO.sizeDelta.x + (Items.Count - 1) * Spacing)), ItemGO.sizeDelta.y);
                    }
                    else
                    {
                        RtContentAssigned.sizeDelta = new Vector2(ItemGO.sizeDelta.x, (Items == null ? 0 : (Items.Count * ItemGO.sizeDelta.y + (Items.Count - 1) * Spacing)));
                    }
                    
                }
            }

            Rect _rectView = RtViewportAssigned.GetWorldRectInGUIForm();

            if (ItemPrefabs != null)
            {
                foreach (GameObject _g in ItemPrefabs)
                {
                    RectTransform _rt = _g.GetComponent<RectTransform>();
                    if (_rt == null)
                    {
                        continue;
                    }

                    Rect _rect = _rt.GetWorldRectInGUIForm();
                    bool _isInRect = _rect.Overlaps(_rectView) == true;
                    _rt.gameObject.SetActive(_isInRect);

                    if (_isInRect == true)
                    {
                        IUIListItem _item = _g.GetComponent<IUIListItem>();
                        if (_item != null)
                        {
                            _item.Redraw();
                        }
                    }
                }
            }
        }

        public void Add(object _itemToAdd, bool _allowDuplication = false)
        {
            if (_itemToAdd == null)
            {
                return;
            }

            if (Items == null)
            {
                Items = new List<object>();
            }

            object _exist = Items.Find(_item => _item == _itemToAdd);

            int _indexToAdd = -1;

            if (_exist != null)
            {
                if (_allowDuplication == false)
                {
                    return;
                }
                else
                {
                    _indexToAdd = Items.IndexOf(_exist);
                    if (_indexToAdd != -1)
                    {
                        Items.RemoveAt(_indexToAdd);
                    }
                }
            }

            if (_indexToAdd == -1)
            {
                Items.Add(_itemToAdd);
                _indexToAdd = Items.Count - 1;
            }
            else
            {
                Items.Insert(_indexToAdd, _itemToAdd);
            }

            if (ItemPrefabs == null)
            {
                ItemPrefabs = new List<GameObject>();
            }
            GameObject _go = Instantiate(ItemGO.gameObject) as GameObject;
            _go.transform.parent = RtContentAssigned;
            _go.transform.localScale = Vector3.one;
            if (Direction == eDir.HORIZONTAL)
            {
                _go.transform.localPosition = new Vector3(_indexToAdd * (ItemGO.sizeDelta.x + Spacing), 0, 0);
            }
            else
            {
                _go.transform.localPosition = new Vector3(0, - _indexToAdd * (ItemGO.sizeDelta.y + Spacing), 0);
            }
            
            ItemPrefabs.Add(_go);
            _isRefershNeeded = true;
            IUIListItem _uiItem = _go.GetComponent<IUIListItem>();
            if (_uiItem != null)
            {
                _uiItem.SetData(_itemToAdd);
                _uiItem.SetParent(this);
            }
        }

        public void Remove()
        {

        }

        public void Refresh()
        {
            _isRefershNeeded = true;
        }

        public void OnNotifiedItemClicked(IUIListItem _itemClicked)
        {
            if (ItemPrefabs == null)
            {
                return;
            }

            foreach (GameObject _itemObj in ItemPrefabs)
            {
                if (_itemObj == null)
                {
                    continue;
                }

                IUIListItem _item = _itemObj.GetComponent<IUIListItem>();
                if (_item == null)
                {
                    continue;
                }

                _item.SetSelected(_item == _itemClicked);
            }

            Refresh();
        }

        public void SendMessageToPopup (string _msg, object _value)
        {
            if (ParentPopup == null)
            {
                return;
            }

            ParentPopup.SendMessage(_msg, _value);
        }
    }
}
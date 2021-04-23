using UnityEngine;
using System.Collections;

namespace JFrame
{
    public interface IUIListItem
    {
        void SetData(object _data);
        void Redraw();

        void SetParent(UIComponent_ScrollRect _sRect);
        void SetSelected(bool _bSelected);
    }

}

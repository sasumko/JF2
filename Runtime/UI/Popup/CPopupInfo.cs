using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrame
{
    public class CPopupInfo : CInfoBase
    {
        public int Idx;
        public string ResId;
        public float LifeTime;
        public int DefaultCloseActionType;
    }

    public enum ePopupButtonEvent
    {
        NONE = -1,
        CLOSE,
        YES,
        NO,
        OK,
        CANCEL
    }
}

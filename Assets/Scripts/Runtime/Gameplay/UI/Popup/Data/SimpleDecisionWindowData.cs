using System;
using Core.UI;

namespace Runtime.Gameplay.UI.Popup.Data
{
    public class SimpleDecisionWindowData : BaseWindowData
    {
        public Action PressOkEvent;
        public string Message;
    }
}
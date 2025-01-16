using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using Harris.GPC;
using System;

namespace TowerDefense
{

    public class UIEvent : Harris.GPC.Event
    {         
        public DateTime OccuredTime { get; private set; }

        public override void Notify()
        {
            OccuredTime = DateTime.Now;
            base.Notify();
        }
    }

    public class UIButtonEvent : UIEvent
    {
        private TowerButton _button;

        public UIButtonEvent(TowerButton button)
        {
            _button = button;
        }
    }


    public class UIEventTypes : EventTypes
    {
        public static readonly UIEventTypes TURRETBUTTONCLICK =  new UIEventTypes("TURRETBUTTONCLICK",  new UIButtonEvent(UIManager.Instance.MachineGunButton));
        public static readonly UIEventTypes CANNONBUTTONCLICK =  new UIEventTypes("CANNONBUTTONCLICK",  new UIButtonEvent(UIManager.Instance.CannonButton));
        public static readonly UIEventTypes GRIDTOWERBUTTONCLICK =  new UIEventTypes("GRIDTOWERBUTTONCLICK",  new UIButtonEvent(UIManager.Instance.GridTowerButton));

        private int id = -1;

        public UIEventTypes (string value,  UIEvent ev): base(value, ev)
        {

        }
    }

    public class UIEventBus : EventBus<UIEventTypes> 
    {

    }
}
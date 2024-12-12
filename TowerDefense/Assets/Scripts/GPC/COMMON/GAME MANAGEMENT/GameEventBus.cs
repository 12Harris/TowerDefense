using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using Harris.GPC;
using System;

namespace Harris.GPC
{

    public class GameEvent : Harris.GPC.Event
    {         
        public DateTime OccuredTime { get; private set; }

        public override void Notify()
        {
            OccuredTime = DateTime.Now;
            base.Notify();
        }
    }

    public abstract class EventTypes : EnumBase
    {
        private Event _event = null;

        public EventTypes (string value, Event _ev)
            : this(value)
        {
            _event = _ev;
        }

        private EventTypes (string value)
            : base(value)
        {
 
        }

        public Event GetEvent()
        {
            if(_event == null)
                _event = new GameEvent();
            
            return _event;
        }

    }

    public class GameEventTypes : EventTypes
    {
        public static readonly GameEventTypes START =  new GameEventTypes("START",  new GameEvent());
        public static readonly GameEventTypes PAUSE = new GameEventTypes("PAUSE", new GameEvent());
        public static readonly GameEventTypes STOP = new GameEventTypes("STOP", new GameEvent());
        public static readonly GameEventTypes FINISH = new GameEventTypes("FINISH", new GameEvent());
        public static readonly GameEventTypes PESTART = new GameEventTypes("RESTART", new GameEvent());
        public static readonly GameEventTypes QUIT = new GameEventTypes("QUIT", new GameEvent());
        private int id = -1;

        public GameEventTypes (string value,  GameEvent ev): base(value, ev)
        {

        }
    }

    public class GameEventBus : EventBus<GameEventTypes> 
    {

    }
}
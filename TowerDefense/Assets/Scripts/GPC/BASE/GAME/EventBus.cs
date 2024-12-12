using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Harris.GPC
{
    public class EventBus<T> where T: EventTypes                        
    {
        private static readonly 
            IDictionary<T, Event> 
            Events = new Dictionary<T, Event>();

            public static void Subscribe (T eventType, IEventListener listener) {
            
            Event thisEvent;

            if (Events.TryGetValue(eventType, out thisEvent)) {
                thisEvent.Attach(listener);
            }
            else {
   
                thisEvent = eventType.GetEvent();
                thisEvent.Attach(listener);
                Events.Add(eventType, thisEvent);
            }
        }

        public static void Unsubscribe 
            (T eventType, IEventListener listener) {

            Event thisEvent;

            if (Events.TryGetValue(eventType, out thisEvent)) {
                thisEvent.Detach(listener);
            }
        }

        public static void Execute(T eventType) {

            Event thisEvent;

            if (Events.TryGetValue(eventType, out thisEvent)) {
                thisEvent.Notify();
            }
        }
    }
}
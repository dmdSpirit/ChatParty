using System;
using System.Collections.Generic;

/// <summary>
/// EventBus implementation.
/// </summary>
public static class EventBus {
	public delegate void ActionDelegate<T> (T eventData);
	static Dictionary <Type, Delegate> eventsDictionary = new Dictionary<Type, Delegate> ();

    /// <summary>
    /// Subscribe callback to be raised on specific event.
    /// </summary>
    /// <param name="eventAction">Callback.</param>
	public static void Subscribe<T> (ActionDelegate<T> eventAction){
		if(eventAction != null){
			var eventType = typeof(T);
			Delegate delegateList;
			eventsDictionary.TryGetValue (eventType, out delegateList);
            eventsDictionary [eventType] = (delegateList as ActionDelegate<T>) + eventAction;
        }
    }

    /// <summary>
    /// Publish event.
    /// </summary>
    /// <param name="eventMessage">Event message.</param>
	public static void Publish<T>(T eventMessage){
		var eventType = typeof(T);
		Delegate delegateList;
		eventsDictionary.TryGetValue (eventType, out delegateList);
		var list = delegateList as ActionDelegate<T>;
		if(list != null){
			list (eventMessage);
		}
	}

    /// <summary>
    /// Unsubscribe callback.
    /// </summary>
    /// <param name="eventAction">Event action.</param>
    /// <param name="keepEvent">Clear only callback and keep event.</param>
	public static void Unsubscribe<T> (ActionDelegate<T> eventAction, bool keepEvent = false){
		if(eventAction != null){
			var eventType = typeof(T);
			Delegate delegateList;
			if(eventsDictionary.TryGetValue(eventType, out delegateList)){
				var list = (delegateList as ActionDelegate<T>) - eventAction;
				if (list == null && !keepEvent) {
					eventsDictionary.Remove (eventType);
				} else
					eventsDictionary [eventType] = list;
			}
		}
	}

    /// <summary>
    /// Unsubscribe all callbacks from event.
    /// </summary>
    /// <param name="keepEvent">Clear only callbacks and keep events.</param>
	public static void UnsubscribeAll<T> (bool keepEvent = false){
		var eventType = typeof(T);
		Delegate delegateList;
		if(eventsDictionary.TryGetValue(eventType, out delegateList)){
			if (keepEvent)
				eventsDictionary [eventType] = null;
			else
				eventsDictionary.Remove (eventType);
		}
	}

    /// <summary>
    /// Clear everything.
    /// </summary>
	public static void ClearAll(){
		eventsDictionary.Clear ();
	}
}

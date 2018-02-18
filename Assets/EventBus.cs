using System;
using System.Collections.Generic;

public static class EventBus {
	public delegate void ActionDelegate<T> (T eventData);
	static Dictionary <Type, Delegate> eventsDictionary = new Dictionary<Type, Delegate> ();

	public static void Subscribe<T> (ActionDelegate<T> eventAction){
		if(eventAction != null){
			var eventType = typeof(T);
			Delegate rawList;
			eventsDictionary.TryGetValue (eventType, out rawList);
			//eventsDictionary [eventType] += eventAction;
		}
	}

	public static void Publish<T>(T eventMessage){
		var eventType = typeof(T);
		Delegate rawList;
		eventsDictionary.TryGetValue (eventType, out rawList);
		var list = rawList as ActionDelegate<T>;
		if(list != null){
			list (eventMessage);
		}
	}

	public static void Unsubscribe<T> (ActionDelegate<T> eventAction, bool keepEvent = false){
		if(eventAction != null){
			var eventType = typeof(T);
			Delegate rawList;
			if(eventsDictionary.TryGetValue(eventType, out rawList)){
				var list = (rawList as ActionDelegate<T>) - eventAction;
				if (list == null && !keepEvent) {
					eventsDictionary.Remove (eventType);
				} else
					eventsDictionary [eventType] = list;
			}
		}
	}

	public static void UnsubscribeAll<T> (bool keepEvent = false){
		var eventType = typeof(T);
		Delegate rawList;
		if(eventsDictionary.TryGetValue(eventType, out rawList)){
			if (keepEvent)
				eventsDictionary [eventType] = null;
			else
				eventsDictionary.Remove (eventType);
		}
	}

	public static void ClearAll(){
		eventsDictionary.Clear ();
	}
}

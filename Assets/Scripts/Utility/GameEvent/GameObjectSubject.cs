using System.Collections.Generic;

public class GameObjectSubject
{
    static public Dictionary<GameObjectType, Dictionary<int, object>> gameEventHandlerList = null;

    static public void RegisterValue(GameObjectType objectType, object SendObject, int addedList = 0)
    {
        if (null == gameEventHandlerList)
            gameEventHandlerList = new Dictionary<GameObjectType, Dictionary<int, object>>();

        if(!gameEventHandlerList.TryGetValue(objectType, out var value))
        {
            value = new Dictionary<int, object>();
            gameEventHandlerList.Add(objectType, value);
        }

        if (value.ContainsKey(addedList)) value[addedList] = SendObject;
        else value.Add(addedList, SendObject);
    }

    static public void UnregisterHandler(GameObjectType objectType, int addedList = 0)
    {
        if (gameEventHandlerList == null)
            return;

        if (!gameEventHandlerList.TryGetValue(objectType, out var value))
            return;

        value.Remove(addedList);
    }

    static public object GetObjectSubject(GameObjectType objectType, int addedList = 0)
    {
        if (gameEventHandlerList == null)
            return null;

        if (!gameEventHandlerList.TryGetValue(objectType, out var value))
            return null;

        if (!value.TryGetValue(addedList, out var result))
            return null;

        return result;
    }
}

using UnityEngine;
using System.Collections.Generic;

public interface EventUpdate
{
    void UpdateAllInfo();
    void UnregisterGameEvent();
}

public abstract class GameEventUpdate : MonoBehaviour, EventUpdate
{
    public abstract void UnregisterGameEvent();

    public abstract void UpdateAllInfo();
}

public abstract class GameEventHandler : GameEventUpdate
{
    private Dictionary<GameObjectType, int> handlingObjectTypeList = null;

    // 실제로 이벤트를 받을 자식클래스(game object 등)에서 반드시 override 되어야 함.
    public abstract void HandleGameEvent(GameEvent ge);
    public sealed override void UpdateAllInfo()
    {
        UnregisterAllGameEvent();
        foreach (var inEventType in EventList)
            GameEventSubject.RegisterHandler(inEventType, HandleGameEvent);

        ChildUpdateAllInfo();
    }

    protected virtual void ChildUpdateAllInfo() { }
    protected abstract List<GameEventType> EventList { get; }

    private void UnregisterAllGameEvent()
    {
        ChildUnregisterGameEvent();
        foreach (var inEventType in EventList)
            GameEventSubject.UnregisterHandler(inEventType, HandleGameEvent);
    }

    public void RegisterGameEvent(GameObjectType inEventType, object SendObject, int addedList = 0)
    {
        if (null == handlingObjectTypeList)
            handlingObjectTypeList = new Dictionary<GameObjectType, int>();

        handlingObjectTypeList[inEventType] = addedList;
        GameObjectSubject.RegisterValue(inEventType, SendObject, addedList);
    }

    protected void UnregisterAllObjectType()
    {
        if (null == handlingObjectTypeList || handlingObjectTypeList.Count == 0)
            return;

        var itr = handlingObjectTypeList.GetEnumerator();
        while(itr.MoveNext())
            GameObjectSubject.UnregisterHandler(itr.Current.Key, itr.Current.Value);
        itr.Dispose();

        handlingObjectTypeList.Clear();
    }

    public sealed override void UnregisterGameEvent()
    {
        UnregisterAllGameEvent();
        UnregisterAllObjectType();
    }

    protected virtual void ChildUnregisterGameEvent() { }

    protected virtual void OnDestroy()
    {
        UnregisterGameEvent();
        handlingObjectTypeList = null;
    }
}

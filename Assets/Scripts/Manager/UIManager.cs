using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Enum;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField] PopupCanvas popupCanvas = null;
    List<BaseUI> UIList = new List<BaseUI>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ShowUI(DynamicUI ui, params object[] data)
    {
        var idx = UIList.FindIndex(a => a.uiType == ui);
        if(idx < 0)
        {
            var UIObj = Resources.Load<BaseUI>(Common.GetUIPath(ui));
            var obj = Instantiate(UIObj, popupCanvas.gameObject.transform);
            obj.Init(data);
            obj.SetEnumData(ui);
            UIList.Add(obj);
        }
        else
        {
            if (UIList[idx].gameObject.activeInHierarchy == false)
                UIList[idx].Init(data);
        }
    }
}

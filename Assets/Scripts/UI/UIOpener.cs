using UnityEngine;
using UnityEngine.UI;
using App.Enum;

public class UIOpener : MonoBehaviour
{
    [SerializeField] DynamicUI uiType = DynamicUI.NONE;

    Button button = null;
    private void Start()
    {
        button = this.GetComponentInChildren<Button>();
        if (button == null)
            return;
        button.onClick.AddListener(Click_Button);
    }
    public void Click_Button()
    {
        UIManager.instance.ShowUI(uiType);
        SoundManager.Instance.PlayEffect("Sounds/UI/Button");
    }
    private void OnDestroy()
    {
        if (button == null)
            return;
        button.onClick.RemoveListener(Click_Button);
    }
}

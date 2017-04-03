
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public MenuButton SelectedButton;
    public MainMenuAnims Anim;
    public EventTrigger[] Triggers;

#if UNITY_EDITOR
    void OnValidate()
    {
        Triggers = GetComponentsInChildren<EventTrigger>();
        for (int i = 0; i < Triggers.Length; i++)
            Triggers[i].enabled = false;
    }
#endif // UNITY_EDITOR

    protected virtual void OnEnable()
    {
        if (SelectedButton)
            SelectedButton.OnActivate();
        for (int i = 0; i < Triggers.Length; i++)
            Triggers[i].enabled = true;
    }

    protected virtual void OnDisable()
    {
        var curButton = MainMenuManager.Instance.CurrentButton;
        if (MainMenuManager.Instance.MovingForward)
            SelectedButton = curButton;
        if (curButton)
            curButton.OnDeactivate();
        for (int i = 0; i < Triggers.Length; i++)
            Triggers[i].enabled = false;
    }
}

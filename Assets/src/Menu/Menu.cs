using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MenuAction))]
public class Menu : MonoBehaviour
{
    public Menu LastMenu;
    public MenuButton DefaultButton;

    public MenuAction MenuAction;

    public MenuAnimator[] Animators;

    public float ComingDuration;
    public float LeavingDuration;

    public MenuButton SelectedButton
    {
        set
        {
            if (value != null)
                m_lastSelected = value;
        }
        get
        {
            return m_lastSelected;
        }
    }

    public static Menu CurrentMenu
    {
        get { return s_currentMenu; }
    }

    void Update()
    {
        if (m_effectiveTimer < ComingDuration)
        {
            m_effectiveTimer += Time.deltaTime;
            return ;
        }

        if (BackAction())
        {
            MenuAction.OnHideBack();
        }
    }

    public void DisableToMenu(Menu nextMenu)
    {
        m_effectiveTimer = 0;
        gameObject.SetActive(true);
        StartCoroutine(DisableCoroutine(nextMenu));
    }

    public void EnableMenu()
    {
        s_currentMenu = this;

        m_effectiveTimer = 0;

        if (DefaultButton != null)
        {
            DefaultButton.Select();
            SelectedButton = DefaultButton;
        }
    }

    private IEnumerator DisableCoroutine(Menu nextMenu)
    {
        foreach (MenuAnimator animator in Animators)
        {
            animator.PlayLeavingAnimation();
        }

        yield return new WaitForSeconds(LeavingDuration);

        if (nextMenu != null)
        {
            nextMenu.MenuAction.OnDraw();
        }
        
        gameObject.SetActive(false);
    }

    private static bool BackAction()
    {
        return ControllerManager.Instance.GetKeyDown("back") || Input.GetKeyDown(KeyCode.Backspace);
    }

    private static Menu s_currentMenu;

    private MenuButton m_lastSelected;
    private float m_effectiveTimer;
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public Menu DefaultMenu;
    public Menu[] Menus;

    public Animator Animator;
    public Menu CurrentMenu;
    public bool MovingForward;

    public MenuButton CurrentButton
    {
        get { return m_currentButton; }
        set
        {
            if (m_currentButton)
                m_currentButton.OnDeactivate();
            m_currentButton = value;
            if (m_currentButton)
                m_currentButton.OnActivate();
        }
    }
    private MenuButton m_currentButton;
    [NonSerialized]
    public NavigationSelector[] Navigators = new NavigationSelector[ControllerManager.c_maxControllers];
    public int NavigatorsCount;

    public GameObject[] EnableAtStart;
    public bool EnableBack = true;

    private List<Menu> m_enteredMenus = new List<Menu>(6);

    void Awake()
    {
        if (!enabled)
            return;
        Instance = this;

        CurrentMenu = DefaultMenu;
        foreach (GameObject go in EnableAtStart)
        {
            go.SetActive(true);
        }
    }

    void Start()
    {
        AudioManager.Instance.Play("menu", true);
        ControllerManager.Instance.OnMenuInput += OnMenuInputCB;
    }

    private void OnDestroy()
    {
        ControllerManager.Instance.OnMenuInput -= OnMenuInputCB;
    }

    public void OnMenuInputCB(MenuInput _input, int _controller)
    {
        if (_controller == 0)
        {
            CurrentButton.UpdateInput(_input);
            if (_input == MenuInput.Validation && CurrentButton.NextMenu != null)
            {
                // back button
                if (m_enteredMenus.Contains(CurrentButton.NextMenu))
                    BackToMenu(CurrentButton.NextMenu);
                else
                    MoveToMenu(CurrentButton.NextMenu);
            }
            else if (_input == MenuInput.Cancel && NavigatorsCount > 0 && ! Navigators[0].Locked)
            {
                if (m_enteredMenus.Count == 0)
                {
                    AudioManager.Instance.Play("errorMenu");
                }
                else
                {
                    var lastMenu = m_enteredMenus[m_enteredMenus.Count - 1];
                    m_enteredMenus.Remove(lastMenu);
                    BackToMenu(lastMenu);
                }
            }
        }
        
        for (int i = 0; i < NavigatorsCount; i++)
        {
            var navigator = Navigators[i];
            navigator.UpdateInput(_input, _controller);
        }
    }

    public void MoveToMenu(Menu _newMenu)
    {
        MovingForward = true;
        Animator.SetFloat("Mirror", 1);
        m_enteredMenus.Add(CurrentMenu);
        Animator.Play((int)_newMenu.Anim, 0, 0.01f);
        SetNewMenu(_newMenu);
    }

    void BackToMenu(Menu _newMenu)
    {
        MovingForward = false;
        m_enteredMenus.Remove(_newMenu);
        Animator.SetFloat("Mirror", -1);
        Animator.Play((int)CurrentMenu.Anim, 0, 0.99f);
        SetNewMenu(_newMenu);
    }

    void SetNewMenu(Menu _newMenu)
    {
        CurrentMenu.enabled = false;
        CurrentMenu = _newMenu;
        _newMenu.enabled = true;
    }
}

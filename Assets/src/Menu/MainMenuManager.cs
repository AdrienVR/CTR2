using System;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public Menu DefaultMenu;
    public GameObject[] GoToEnable;

    [Serializable]
    public class FirstAnimator
    {
        public Animator Animator;
        public string StateName;
        public RuntimeAnimatorController ReplacingController;

        public void PlayAnimation()
        {
            Animator.Play(StateName);
            if (ReplacingController != null)
                Animator.runtimeAnimatorController = ReplacingController;
        }
    }

    public FirstAnimator[] FirstAnimations;

    // Use this for initialization
    void Awake()
    {
        m_menus = GetComponentsInChildren<Menu>();
        
        foreach (Menu menu in m_menus)
        {
            if (menu == DefaultMenu)
            {
                DefaultMenu.gameObject.SetActive(true);
            }
            else
            {
                menu.gameObject.SetActive(false);
            }
        }

        foreach (GameObject go in GoToEnable)
        {
            go.SetActive(true);
        }
        DefaultMenu.MenuAction.OnDraw();
    }

    void Start()
    {
        AudioManager.Instance.Play("menu", true);

        foreach (FirstAnimator anim in FirstAnimations)
        {
            anim.PlayAnimation();
        }
    }

    private Menu[] m_menus;
}

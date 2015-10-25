using System;
using System.Collections;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Menu LastMenu;
    public MenuButton DefaultButton;

    public MenuAnimator[] Animators;
    public FirstAnimator[] FirstAnimations;

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

    // Use this for initialization
    void Start()
    {
        if (s_currentMenu == null)
        {
            EnableMenu();
            foreach (FirstAnimator anim in FirstAnimations)
            {
                anim.PlayAnimation();
            }
        }
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
            if (LastMenu == null)
            {
                AudioManager.Instance.Play("errorMenu");
            }
            else
            {
                AudioManager.Instance.Play("cancelMenu");
                DisableToMenu(LastMenu);
            }
        }
    }

    public void DisableToMenu(Menu nextMenu)
    {
        m_effectiveTimer = 0;
        StartCoroutine(DisableCoroutine(nextMenu));
    }

    public void EnableMenu()
    {
        s_currentMenu = this;
        gameObject.SetActive(true);

        if (s_currentMenu != null)
        {
            foreach (MenuAnimator animator in Animators)
            {
                animator.PlayComingAnimation();
            }
        }

        m_effectiveTimer = 0;

        DefaultButton.Select();
        SelectedButton = DefaultButton;
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
            nextMenu.EnableMenu();
        }

        s_lastMenu = this;
        gameObject.SetActive(false);
    }

    private static bool BackAction()
    {
        return ControllerManager.Instance.GetKeyDown("back") || Input.GetKeyDown(KeyCode.Backspace);
    }

    private static Menu s_currentMenu;
    private static Menu s_lastMenu;

    private MenuButton m_lastSelected;
    private float m_effectiveTimer;

    [Serializable]
    public class MenuAnimator
    {
        public Animator Animator;
        public MenuAnimationType Type;

        public void PlayComingAnimation()
        {
            switch (Type)
            {
                case MenuAnimationType.Left:
                    Animator.Play("SlideLeft");
                    break;
                case MenuAnimationType.Right:
                    Animator.Play("SlideRight");
                    break;
                case MenuAnimationType.Top:
                    Animator.Play("SlideTop");
                    break;
                case MenuAnimationType.Bottom:
                    Animator.Play("SlideBottom");
                    break;
                default:
                    break;
            }
        }

        public void PlayLeavingAnimation()
        {
            switch (Type)
            {
                case MenuAnimationType.Left:
                    Animator.Play("SlideLeftBack");
                    break;
                case MenuAnimationType.Right:
                    Animator.Play("SlideRightBack");
                    break;
                case MenuAnimationType.Top:
                    Animator.Play("SlideTopBack");
                    break;
                case MenuAnimationType.Bottom:
                    Animator.Play("SlideBottomBack");
                    break;
                default:
                    break;
            }
        }
    }

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

    public enum MenuAnimationType
    {
        Left,
        Right,
        Top,
        Bottom
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EventTrigger))]
public class MenuButton : NavigationCross
{
    public Menu ParentMenu;
    public CrossMenuButton Cross;
    public Menu NextMenu;

    public bool Active
    {
        get
        {
            return ParentMenu.SelectedButton == this;
        }
        set
        {
            if (value == true)
            {
                ParentMenu.SelectedButton = this;
                ButtonSelect();
            }
            else
                Deselect();
        }
    }

    void Awake()
    {
        m_callback = GetComponent<Button>().onClick;
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            if (IsHighLighted() == false)
                m_animator.Play("Highlighted");

            m_timer += Time.deltaTime;
            if (m_timer < 0.05f)
                return;

            if (ValidateAction())
            {
                Validate();
            }
            if (Cross.Up != null && UpAction())
            {
                Cross.Up.OnUp();
            }
            if (Cross.Down != null && DownAction())
            {
                Cross.Down.OnDown();
            }
            if (Cross.Left != null && LeftAction())
            {
                Cross.Left.OnLeft();
            }
            if (Cross.Right != null && RightAction())
            {
                Cross.Right.OnRight();
            }
        }
        else
        {
            if (IsHighLighted())
                Deselect();
            m_timer = 0;
        }
    }

    public override void OnLeft()
    {
        ((MenuButton)Cross.Right).Active = false;
        Active = true;
    }
    public override void OnRight()
    {
        ((MenuButton)Cross.Left).Active = false;
        Active = true;
    }
    public override void OnUp()
    {
        ((MenuButton)Cross.Down).Active = false;
        Active = true;
    }
    public override void OnDown()
    {
        ((MenuButton)Cross.Up).Active = false;
        Active = true;
    }

    public void Validate()
    {
        AudioManager.Instance.Play("validateMenu");
        if (m_callback != null)
        {
            m_callback.Invoke();
        }
        if (NextMenu != null)
        {
            Menu.CurrentMenu.DisableToMenu(NextMenu);
        }
    }

    public void Select()
    {
        m_animator.Play("Highlighted");
    }

    private void Deselect()
    {
        m_animator.Play("Normal");
    }

    public void ButtonSelect()
    {
        AudioManager.Instance.Play("downMenu");
        Select();
    }

    public void MouseSelect()
    {
        ParentMenu.SelectedButton = this;
        ButtonSelect();
    }

    public void MouseClick()
    {
        Validate();
    }

    private bool IsHighLighted()
    {
        return m_animator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted");
    }

    private static bool UpAction()
    {
        return ControllerManager.Instance.GetKeyDown("up") || Input.GetKeyDown(KeyCode.UpArrow);
    }

    private static bool DownAction()
    {
        return ControllerManager.Instance.GetKeyDown("down") || Input.GetKeyDown(KeyCode.DownArrow);
    }

    private static bool LeftAction()
    {
        return ControllerManager.Instance.GetKeyDown("left") || Input.GetKeyDown(KeyCode.LeftArrow);
    }

    private static bool RightAction()
    {
        return ControllerManager.Instance.GetKeyDown("right") || Input.GetKeyDown(KeyCode.RightArrow);
    }

    private static bool ValidateAction()
    {
        return ControllerManager.Instance.GetKeyDown("validate") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
    }

    private UnityEvent m_callback;
    private Animator m_animator;

    private bool m_selected;
    private float m_timer;
}


using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class NavigationSelector : NavigationCross
{
    public SelectorManager SelectorManager;
    public ShowRoom ShowRoom;
    public Sprite ValidatedSprite;
    public int PlayerIndex;

    void Start()
    {
        m_image = GetComponent<Image>();
        m_availableSprite = m_image.sprite;
        m_x = s_x++;
        m_y = 0;// s_y++;
    }

    protected override void Update()
    {
        if (m_locked == false)
            base.Update();

        if (BackAction())
        {
            OnBack();
        }
    }

    // Deplacements :
    public override void OnLeft()
    {
        transform.SetParent(SelectorManager.GetLeftParent(ref m_x, ref m_y), false);
        ShowRoom.ShowModelFromRight(transform.parent.name, PlayerIndex);
    }
    public override void OnRight()
    {
        transform.SetParent(SelectorManager.GetRightParent(ref m_x, ref m_y), false);
		ShowRoom.ShowModelFromLeft(transform.parent.name, PlayerIndex);
    }
    public override void OnUp()
    {
        transform.SetParent(SelectorManager.GetUpParent(ref m_x, ref m_y), false);
		ShowRoom.ShowModelFromDown(transform.parent.name, PlayerIndex);
    }
    public override void OnDown()
    {
        transform.SetParent(SelectorManager.GetDownParent(ref m_x, ref m_y), false);
		ShowRoom.ShowModelFromUp(transform.parent.name, PlayerIndex);
    }

    // Actions
    public override void OnValidateAction()
    {
        m_image.sprite = ValidatedSprite;
        m_locked = true;
    }

    public void OnBack()
    {
        m_image.sprite = m_availableSprite;
        m_locked = false;
    }

    protected override bool UpAction()
    {
        return ControllerManager.Instance.GetKeyDown("up", PlayerIndex);
    }

    protected override bool DownAction()
    {
        return ControllerManager.Instance.GetKeyDown("down", PlayerIndex);
    }

    protected override bool LeftAction()
    {
        return ControllerManager.Instance.GetKeyDown("left", PlayerIndex);
    }

    protected override bool RightAction()
    {
        return ControllerManager.Instance.GetKeyDown("right", PlayerIndex);
    }

    protected override bool ValidateAction()
    {
        return ControllerManager.Instance.GetKeyDown("validate", PlayerIndex);
    }

    protected bool BackAction()
    {
        return ControllerManager.Instance.GetKeyDown("action", PlayerIndex);
    }

    private bool m_locked = false;

    private int m_x;
    private int m_y;

    private Image m_image;
    private Sprite m_availableSprite;

    private static int s_x;
    private static int s_y;
}

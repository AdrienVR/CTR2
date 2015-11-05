
public class PlayerMenuAction : MenuAction
{
    public MenuAnimator Logo;
    public MenuAnimator Title;

    public override void OnHideBack()
    {
        Logo.PlayComingAnimation();
        Title.PlayComingAnimation();
        base.OnHideBack();
    }

    public override void OnDraw()
    {
        Logo.PlayLeavingAnimation();
        Title.PlayLeavingAnimation();
        base.OnHideBack();
    }
}

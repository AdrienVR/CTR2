using UnityEngine;

[RequireComponent(typeof(Menu))]
public class MenuAction : MonoBehaviour
{
    public Menu Menu;

    public virtual void OnHideBack()
    {
        Menu.DisableToMenu(Menu.LastMenu);
    }

    public virtual void OnHideNext(Menu NextMenu)
    {
        Menu.DisableToMenu(NextMenu);
    }

    public virtual void OnDraw()
    {
        Menu.gameObject.SetActive(true);

        foreach (MenuAnimator animator in Menu.Animators)
        {
            animator.PlayComingAnimation();
        }

        Menu.EnableMenu();
    }
}

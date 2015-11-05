using System;
using UnityEngine;

public enum MenuAnimationType
{
    Left,
    Right,
    Top,
    Bottom
}

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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    public Image ParentSprite;
    public Color EnabledColor;
    public Color DisabledColor;

    public void SwitchState()
    {
        state = (state == false);
        WeaponManager.Instance.SetBattleWeaponActivation(transform.parent.name, state);

        if (state == true)
            ParentSprite.color = EnabledColor;
        else
            ParentSprite.color = DisabledColor;
    }

    private bool state = true;
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Video : MonoBehaviour
{
    public Sprite[] Images;
    public Image TargetImage;

    void Start()
    {
        m_endOfFrame = new WaitForSeconds(1.0f/25f);
        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        for (int i = 0; ; i++)
        {
            yield return m_endOfFrame;

            TargetImage.sprite = Images[i];

            if (i + 1 >= Images.Length)
            {
                i = -1;
            }
        }
    }

    private WaitForSeconds m_endOfFrame;
}

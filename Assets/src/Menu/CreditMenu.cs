
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CreditTitle
{
    public string m_title;
    public string m_name;
}

public class CreditMenu : Menu
{
    public Text[] m_titleTexts;
    public Text[] m_nameTexts;
    public CreditTitle[] m_credits;
    public float m_creditDuration;

    private Coroutine m_coroutine;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_coroutine = StartCoroutine(SwitchCredit());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);
    }

    IEnumerator SwitchCredit()
    {
        int curIndex = 0;
        while(true)
        {
            var curCredit = m_credits[curIndex];
            for (int i = 0; i < m_titleTexts.Length; ++i)
            {
                m_titleTexts[i].text = curCredit.m_title;
                m_nameTexts[i].text = curCredit.m_name;
            }
            curIndex++;
            if (curIndex >= m_credits.Length)
                curIndex = 0;
            yield return Yielders.Get(m_creditDuration);
        }
    }
}

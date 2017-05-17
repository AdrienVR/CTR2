using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderReplacer : MonoBehaviour
{
    public Shader m_old;
    public Shader m_new;

    [ContextMenu("Replace")]
    void Replace()
    {
        var rends = FindObjectsOfType<Renderer>();
        foreach(var rend in rends)
        {
            foreach (var mat in rend.sharedMaterials)
            {
                if (mat != null && mat.shader == m_old)
                {
                    mat.shader = m_new;
                }
            }
        }
    }
}

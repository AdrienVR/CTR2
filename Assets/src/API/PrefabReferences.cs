using UnityEngine;

public class PrefabReferences : MonoBehaviour
{
    // Singleton
    public static PrefabReferences Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = (Resources.Load("PrefabReferences") as GameObject).GetComponent<PrefabReferences>();
            }
            return s_instance;
        }

    }
    private static PrefabReferences s_instance;

    // public references :
    public AudioCategoryManager AudioCategoryManager;
    public GameObject WeaponManager;
    public GameObject CameraConfig;
    public GameObject PlayerManager;

}

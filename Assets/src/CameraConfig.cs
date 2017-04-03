
using System;
using UnityEngine;

public class CameraConfig : MonoBehaviour
{
    // Singleton
    public static CameraConfig Instance;

    public GameObject ReferenceCanvas;

    [Serializable]
    public class CameraArray
    {
        public GameObject[] Cameras;
    }

    [SerializeField]
    public CameraArray[] OrthographicCameras;
    [SerializeField]
    public CameraArray[] PerspectiveCameras;

    void Awake()
    {
        if (!enabled)
            return;
        Instance = this;
    }

    [ContextMenu("SetCameraArray")]
    void SetCameraArray()
    {
        OrthographicCameras = new CameraArray[4];
        PerspectiveCameras = new CameraArray[4];
        for (int i = 0; i < 4; i++)
        {
            int size = i + 1;
            OrthographicCameras[i] = new CameraArray();
            OrthographicCameras[i].Cameras = new GameObject[size];
            PerspectiveCameras[i] = new CameraArray();
            PerspectiveCameras[i].Cameras = new GameObject[size];
            for (int j = 0; j < size; j++)
            {
                OrthographicCameras[i].Cameras[j] = GameObject.Find("CameraOrtho_" + (j + 1) + "_" + (i + 1));
                PerspectiveCameras[i].Cameras[j] = GameObject.Find("CameraPers_" + (j + 1) + "_" + (i + 1));
            }
        }
    }

    public CameraController InitializePlayer(PlayerController player, int playerIndex, int totalPlayers)
    {
        GameObject cameraGo = Instantiate(OrthographicCameras[totalPlayers - 1].Cameras[playerIndex]);
        Camera camera = cameraGo.GetComponent<Camera>();
        GameObject canvasGo = Instantiate(ReferenceCanvas);

        UIPlayerManager playerUI = canvasGo.GetComponent<UIPlayerManager>();
        player.UIPlayerManager = playerUI;
        playerUI.Player = player;

        SetLayerRecursively(canvasGo, Consts.Layer.layer2d_j1 + playerIndex);
        canvasGo.GetComponent<Canvas>().worldCamera = camera;
        canvasGo.transform.SetParent(cameraGo.transform);

        cameraGo = Instantiate(PerspectiveCameras[totalPlayers - 1].Cameras[playerIndex]);
        cameraGo.transform.SetParent(player.transform);
        return cameraGo.GetComponent<CameraController>();
    }

    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
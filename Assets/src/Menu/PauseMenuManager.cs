using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PauseMenuButton
{
    public PauseMenuButtonType m_type;
    public GUITexture m_backGround;
    public GUIText m_text;
}

[System.Serializable]
public class PauseMenuPanel
{
    public PauseMenuPanelType m_panel;
    public PauseMenuButton[] m_buttons;
}

public enum PauseMenuButtonType
{
    Back,
    Continue,
    Restart,
    BackCharacter,
    BackLevel,
    BackConfig,
    Options,
    Leave,
    VolumeControl,
    InputType,
}

public enum PauseMenuPanelType
{
    Pause,
    Options,
    Settings,
    End,
}

public class PauseMenuManager : MonoBehaviour
{
    public PauseMenuPanel[] m_panels;
    public PauseMenuButton m_currentButton;

    public int m_index;

    public Texture normal;
    public Texture hover;
    public Texture triVolume3;
    public Texture triVolume2;
    public Texture triVolume1;
    public static GameObject cameraMenu;
    private GameObject greyT;
    private float normalTime;
    private int heightLabel = 100;
    private int position = 0;
    private int positionH = 1;
    private static GameObject titreAffiche;
    private static GameObject nameMap;
    private static GameObject triangleFond;
    private static GameObject triangleVolume;
    private static GameObject textPlayer;
    private static GameObject fleches;
    private static List<GameObject> flechesD = new List<GameObject>();
    private static List<GameObject> textureAffichees = new List<GameObject>();
    private static List<GameObject> textAffiches = new List<GameObject>();
    private static List<GameObject> controlAffiches = new List<GameObject>();
    private static Dictionary<int, string> menuCourant = new Dictionary<int, string>();
    public Main main;
    public Kart winner;
    public List<Kart> loosers = new List<Kart>();
    public GameObject cadre1;
    public GameObject cadre5;

    private bool readyToMove = true;
    private int j = 5;
    public bool falseok = true;
    public int numSelection = 1;
    private static List<GameObject> tempAffiches = new List<GameObject>();
    private static List<bool> configWeaponsStates = new List<bool>();
    private static List<string> listMapForMenu = new List<string>() { "Parking", "Skull Rock" };

    // variables statiques pour la config d'une battle
    private static List<string> persos = new List<string>();
    private static List<string> weapons = new List<string>();
    private static string map;
    private static int nbPts = 8;

    delegate void InputDelegate(MenuInput _input);

    private Dictionary<PauseMenuButtonType, InputDelegate> m_inputDelegates;

    void Awake()
    {
        m_inputDelegates = new Dictionary<PauseMenuButtonType, InputDelegate>
        {
            {PauseMenuButtonType.Back,          Back},
            {PauseMenuButtonType.Continue,      Continue},
            {PauseMenuButtonType.Restart,       Restart},
            {PauseMenuButtonType.BackCharacter, BackCharacter},
            {PauseMenuButtonType.BackLevel,     BackLevel},
            {PauseMenuButtonType.BackConfig,    BackConfig},
            {PauseMenuButtonType.Options,       Options},
            {PauseMenuButtonType.Leave,         Leave},
            {PauseMenuButtonType.VolumeControl, VolumeControl},
            {PauseMenuButtonType.InputType,     InputType},
        };
    }

    public void Pause()
    {
        m_currentButton = m_panels[0].m_buttons[0];
        ControllerManager.Instance.OnMenuInput += OnMenuInput;
    }

    public void Resume()
    {
        ControllerManager.Instance.OnMenuInput -= OnMenuInput;
    }

    private void OnMenuInput(MenuInput _input, int _index)
    {
        NavigateMenu(_input);
        m_inputDelegates[m_currentButton.m_type](_input);
    }

    //void testEnd()
    //{
    //    foreach (Kart k in main.players)
    //    {
    //        if (k.isWinner)
    //        {
    //            k.isWinner = false;
    //            winner = k;
    //            greyT = (GameObject)GameObject.Instantiate(Resources.Load("guiBlack"));
    //            inPause = true;
    //            StartCoroutine(waitAndPause());
    //        }
    //        if (winner != null)
    //        {
    //            k.c2d.GetComponent<Camera>().enabled = false;
    //            if (k != winner)
    //            {
    //                k.camera.GetComponent<Camera>().rect = new Rect();
    //                if (loosers.IndexOf(k) == -1)
    //                    loosers.Add(k);
    //            }
    //        }
    //    }
    //}

    //IEnumerator waitAndPause()
    //{
    //    Instantiate(Resources.Load("videoFond"));
    //    KartController.stop = true;

    //    StartCoroutine(interpolateCamera());
    //    yield return new WaitForSeconds(1);
    //    displayMenu(menuFin);

    //    Destroy(greyT);
    //    GameObject j1 = (GameObject)Instantiate(Resources.Load("textTitreMenu"), new Vector3(0.27f, 0.35f, 0), Quaternion.identity);
    //    j1.GetComponent<GUIText>().text = "Joueur " + winner.numeroJoueur + " : " + winner.nbPoints + " Pts";
    //    j1.GetComponent<GUIText>().color = Color.yellow;
    //    loosers = loosers.OrderBy(x => x.nbPoints).ToList();
    //    loosers.Reverse();
    //    foreach (Kart k in loosers)
    //    {
    //        GameObject j = (GameObject)Instantiate(Resources.Load("textTitreMenu"), new Vector3(0.27f, 0.35f - 0.08f * (loosers.IndexOf(k) + 1), 0), Quaternion.identity);
    //        j.GetComponent<GUIText>().text = "Joueur " + k.numeroJoueur + " : " + k.nbPoints + " Pts";
    //    }
    //    KartController.stop = false;
    //    winner.kart_script.GetTransform().position = main.listRespawn[0].position;
    //    winner.kart_script.GetTransform().rotation = main.listRespawn[0].rotation;
    //    AI.kart = winner.kart_script.gameObject;
    //    //AI.children = winner.kc.wheels ["wheelAL"];
    //    AI.wheels = winner.kart_script.wheels;
    //    //main.executeIA winner.kc.wheels ();
    //}


    IEnumerator interpolateCamera()
    {
        Rect obj = new Rect(0.04f, 0.5f, 0.5f, 0.4f);
        float ellapsed = 0;
        float total = 200f;
        float lastTime = Time.time;
        float percent = 0f;
        while (ellapsed < total)
        {
            percent = ellapsed / total;
            ellapsed += (Time.time - lastTime);
            yield return new WaitForSeconds(0.01f);
            winner.camera.GetComponent<Camera>().rect = Lerp(winner.camera.GetComponent<Camera>().rect, obj, percent);
        }
    }

    Rect Lerp(Rect a, Rect b, float f)
    {
        Rect s = new Rect(a.xMin + f * (b.xMin - a.xMin), a.yMin + f * (b.yMin - a.yMin),
                           a.width + f * (b.width - a.width), a.height + f * (b.height - a.height));

        return s;
    }

    void VolumeControl(MenuInput _input)
    {
        Vector3 pos = new Vector3();// 0.5f, 0.5f + (((float)heightLabel / 2) / (float)Screen.height) * (menu.Count / 2 - i), -1);
        textureAffichees.Add((GameObject)Instantiate(Resources.Load("menuButton"), pos, Quaternion.identity));
        triangleFond = (GameObject)Instantiate(Resources.Load("menuBackgroundTriangle"), new Vector3(pos.x, pos.y, 2), Quaternion.identity);
        triangleVolume = (GameObject)Instantiate(Resources.Load("menuVolumeTriangle"), new Vector3(pos.x, pos.y, 3), Quaternion.identity);
        Rect t = new Rect(triangleVolume.GetComponent<GUITexture>().pixelInset.x, triangleVolume.GetComponent<GUITexture>().pixelInset.y, 250 * AudioListener.volume * 1.42f, 25 * AudioListener.volume * 1.42f);
        triangleVolume.GetComponent<GUITexture>().pixelInset = t;
        GameObject textbutton = (GameObject)Instantiate(Resources.Load("textButton"), new Vector3(pos.x - (float)((float)400 / (float)((float)Screen.width * (float)3)), pos.y, 0), Quaternion.identity);
        textbutton.GetComponent<GUIText>().text = "";
        textAffiches.Add(textbutton);
    }

    void Back(MenuInput _input)
    {
        if (_input == MenuInput.Validation)
            PauseMenu.Instance.Resume();
    }

    void Continue(MenuInput _input)
    {
        if (_input == MenuInput.Validation)
            PauseMenu.Instance.Resume();
    }
    
    void Restart(MenuInput _input)
    {
        if (_input == MenuInput.Validation)
        {
            PauseMenu.Instance.Resume();
            Main.Restart();
        }
    }

    void BackCharacter(MenuInput _input)
    {
        if (_input == MenuInput.Validation)
        {
            PauseMenu.Instance.Resume();
        }
    }

    void BackLevel(MenuInput _input)
    {
        if (_input == MenuInput.Validation)
        {
            PauseMenu.Instance.Resume();
        }
    }

    void BackConfig(MenuInput _input)
    {
        if (_input == MenuInput.Validation)
        {
            PauseMenu.Instance.Resume();
        }
    }

    void Options(MenuInput _input)
    {
        if (_input == MenuInput.Validation)
        {
            PauseMenu.Instance.Resume();
        }
    }

    void Leave(MenuInput _input)
    {
        if (_input == MenuInput.Validation)
        {
            PauseMenu.Instance.Resume();
            Application.Quit();
        }
    }

    void InputType(MenuInput _input)
    {
        if (_input == MenuInput.Validation)
        {
            PauseMenu.Instance.Resume();
        }
    }

    void NavigateMenu(MenuInput _input)
    {
    }
}

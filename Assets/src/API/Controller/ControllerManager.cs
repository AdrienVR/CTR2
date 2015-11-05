using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ControllerManager : MonoBehaviour
{
    // Singleton
    public static ControllerManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject go = new GameObject("ControllerManager");
                DontDestroyOnLoad(go);
                s_instance = go.AddComponent<ControllerManager>();
            }
            return s_instance;
        }
    }

    private static ControllerManager s_instance;

    void Awake()
    {
        if (s_instance == null)
            s_instance = this;
        else
            Destroy(gameObject);

        m_allControllers = new List<ControllerBase>();
        m_allControllers.Add(new ControllerBase("Keyboard1"));
        m_allControllers.Add(new ControllerBase("Keyboard2"));
        m_allControllers.Add(new ControllerBase("Keyboard3"));
        while (Input.GetJoystickNames().Length != m_instantiatedControllers)
        {
            Update();
        }
    }

    void Update()
    {
        if (Input.GetJoystickNames().Length != m_instantiatedControllers)
            InitJoysticks();
        foreach (ControllerBase controller in m_allControllers)
        {
            controller.UpdateInternal();
        }
    }

    public int NumberOfController
    {
        get { return Instance.m_instantiatedControllers; }
    }

    public ControllerBase GetController(int i)
    {
        if (i > m_allControllers.Count - 1)
            return m_allControllers[m_allControllers.Count - 1];
        return m_allControllers[i];
    }

    private void InitJoysticks()
    {
        m_nControllers = Input.GetJoystickNames().Length;

        if (m_instantiatedControllers > m_nControllers)
        {
            m_allControllers.RemoveAt(m_instantiatedControllers--);
            ControllerResources.controllers = m_instantiatedControllers;
        }
        else if (m_instantiatedControllers < m_nControllers)
        {
            //if (Input.GetJoystickNames()[m_instantiatedControllers].Trim() == "")
              //  return;
            m_allControllers.Insert(m_instantiatedControllers, new ControllerBase(Input.GetJoystickNames()[m_instantiatedControllers]));
            m_instantiatedControllers++;
            ControllerResources.controllers = m_instantiatedControllers;
        }

        Debug.Log("Now " + m_nControllers + " controllers detected");
    }


    public float GetAxis(string actionName)
    {
        foreach (ControllerBase controller in m_allControllers)
        {
            if (controller.GetAxis(actionName) != 0)
            {
                return controller.GetAxis(actionName);
            }
        }
        return 0;
    }

    public float GetAxis(string actionName, int controllerIndex)
    {
        return GetController(controllerIndex).GetAxis(actionName);
    }

    public bool GetKey(string actionName)
    {
        bool result = false;
        foreach (ControllerBase controller in m_allControllers)
        {
            result |= controller.GetKey(actionName);
        }
        return result;
    }

    public bool GetKey(string actionName, int controllerIndex)
    {
        return GetController(controllerIndex).GetKey(actionName);
    }

    public bool GetKeyDown(string actionName)
    {
        bool result = false;
        foreach (ControllerBase controller in m_allControllers)
        {
            result |= controller.GetKeyDown(actionName);
        }
        return result;
    }

    public bool GetKeyDown(string actionName, int controllerIndex)
    {
        return GetController(controllerIndex).GetKeyDown(actionName);
    }

    public bool GetKeyUp(string actionName)
    {
        bool result = false;
        foreach (ControllerBase controller in m_allControllers)
        {
            result |= controller.GetKeyUp(actionName);
        }
        return result;
    }

    public bool GetKeyUp(string actionName, int controllerIndex)
    {
        return GetController(controllerIndex).GetKeyUp(actionName);
    }

    // TODO
    public static void Test(int i)
    {
        var dictionary = new Dictionary<string, string>();
        dictionary["perls"] = "dot";
        dictionary["net"] = "perls";
        dictionary["dot"] = "net";
        Instance.Write(dictionary, Path.Combine(Application.dataPath, "controller_" + i + ".bin"));

        dictionary = Read(Path.Combine(Application.dataPath, "controller_" + i + ".bin"));
        foreach (var pair in dictionary)
        {
            Debug.Log(pair);
        }
    }

    private void Write(Dictionary<string, string> dictionary, string file)
    {
        using (FileStream fs = File.OpenWrite(file))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            // Put count.
            writer.Write(dictionary.Count);
            // Write pairs.
            foreach (var pair in dictionary)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
        }
    }

    static Dictionary<string, string> Read(string file)
    {
        var result = new Dictionary<string, string>();
        using (FileStream fs = File.OpenRead(file))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            // Get count.
            int count = reader.ReadInt32();
            // Read in all pairs.
            for (int i = 0; i < count; i++)
            {
                string key = reader.ReadString();
                string value = reader.ReadString();
                result[key] = value;
            }
        }
        return result;
    }
    
    private List<ControllerBase> m_allControllers = new List<ControllerBase>();

    private int m_nControllers;
    private int m_instantiatedControllers;

}

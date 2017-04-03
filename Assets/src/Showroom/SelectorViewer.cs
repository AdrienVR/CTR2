using UnityEngine;
using System;

public class SelectorViewer : MonoBehaviour
{
    public int Length;
    public int Height;

    public ShowCamera[] CameraViewers;

    [Serializable]
    public class Model
    {
        public string name;
        public int x;
        public int y;
    }

    public Model[] Models;

    [Serializable]
    public class LineTransform
    {
        public Transform[] ModelLineTransforms;
    }
    public LineTransform[] ModelTransforms;

    public Model FindModel(string name)
    {
        foreach (Model model in Models)
        {
            if (model.name == name)
                return model;
        }
        return null;
    }

    public void ShowModelFromLeft(string modelName, int cameraIndex = 0)
    {
        Model model = FindModel(modelName);
        int x = model.x + 1;
        int y = model.y + 1;
        Vector3 modelPosition = ModelTransforms[y].ModelLineTransforms[x].position;

        if (model.x + 1 > Length)
        {
            Vector3 rightDestination = ModelTransforms[y].ModelLineTransforms[Length + 1].position;
            Vector3 leftDestination = ModelTransforms[y].ModelLineTransforms[0].position;
            CameraViewers[cameraIndex].GoTo(
                new Vector3[3] {
                    rightDestination,
                    leftDestination,
                    modelPosition
            });
        }
        else
        {
            CameraViewers[cameraIndex].GoTo(new Vector3[1] { modelPosition });
        }
    }

    public void ShowModelFromRight(string modelName, int cameraIndex = 0)
    {
        Model model = FindModel(modelName);
        int x = model.x + 1;
        int y = model.y + 1;
        Vector3 modelPosition = ModelTransforms[y].ModelLineTransforms[x].position;

        if (model.x + 2 - 1 < 1)
        {
            Vector3 leftDestination = ModelTransforms[y].ModelLineTransforms[0].position;
            Vector3 rightDestination = ModelTransforms[y].ModelLineTransforms[Length + 1].position;
            CameraViewers[cameraIndex].GoTo(
                new Vector3[3] {
                    leftDestination,
                    rightDestination,
                    modelPosition
            });
        }
        else
        {
            CameraViewers[cameraIndex].GoTo(new Vector3[1] { modelPosition });
        }
    }

    public void ShowModelFromUp(string modelName, int cameraIndex = 0)
    {
        Model model = FindModel(modelName);
        int x = model.x + 1;
        int y = model.y + 1;
        Vector3 modelPosition = ModelTransforms[y].ModelLineTransforms[x].position;

        if (model.y + 1 > Height)
        {
            Vector3 downDestination = ModelTransforms[Height + 1].ModelLineTransforms[x].position;
            Vector3 upDestination = ModelTransforms[0].ModelLineTransforms[x].position;
            CameraViewers[cameraIndex].GoTo(
                new Vector3[3] {
                    downDestination,
                    upDestination,
                    modelPosition
            });
        }
        else
        {
            CameraViewers[cameraIndex].GoTo(new Vector3[1] { modelPosition });
        }
    }

    public void ShowModelFromDown(string modelName, int cameraIndex = 0)
    {
        Model model = FindModel(modelName);
        int x = model.x + 1;
        int y = model.y + 1;
        Vector3 modelPosition = ModelTransforms[y].ModelLineTransforms[x].position;

        if (model.y + 2 - 1 < 1)
        {
            Vector3 leftDestination = ModelTransforms[0].ModelLineTransforms[x].position;
            Vector3 rightDestination = ModelTransforms[Height + 1].ModelLineTransforms[x].position;
            CameraViewers[cameraIndex].GoTo(
                new Vector3[3] {
                    leftDestination,
                    rightDestination,
                    modelPosition
            });
        }
        else
        {
            CameraViewers[cameraIndex].GoTo(new Vector3[1] { modelPosition });
        }
    }

    private int Mod(int x, int m)
    {
        if (m == 0)
            return x;
        return (x % m + m) % m;
    }

    private int m_index;
    private Transform m_currentModel;
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShowRoom : MonoBehaviour
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
        x = model.x + 1 + 1;
        y = model.y + 1;

        if (x > Length)
        {
            Vector3 destination = ModelTransforms[y].ModelLineTransforms[Length + 1].position;
            CameraViewers[cameraIndex].GoTo(
                new Vector3[3] {
                    modelPosition,
                    modelPosition,
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
        int x = model.x + 1 - 1;
        int y = model.y + 1;

        if (x < 1)
        {
            Vector3[] positions = new Vector3[3]
            {
                ModelTransforms[y].ModelLineTransforms[0].position,
                ModelTransforms[y].ModelLineTransforms[Length + 1].position,
                ModelTransforms[y].ModelLineTransforms[Length].position
            };
            CameraViewers[cameraIndex].GoTo(ModelTransforms[y].ModelLineTransforms[1], positions);
        }
        else
        {
            Transform destination = ModelTransforms[y].ModelLineTransforms[x];
            CameraViewers[cameraIndex].GoTo(destination);
        }
    }

    public void ShowModelFromUp(string modelName, int cameraIndex = 0)
    {
        Model model = FindModel(modelName);
        int x = model.x + 1;
        int y = model.y + 1 - 1;

        if (y < 1)
        {
            Vector3[] positions = new Vector3[3]
            {
                ModelTransforms[y].ModelLineTransforms[x].position,
                ModelTransforms[y].ModelLineTransforms[x].position,
                ModelTransforms[y].ModelLineTransforms[x].position
            };
            CameraViewers[cameraIndex].GoTo(ModelTransforms[y].ModelLineTransforms[1], positions);
        }
        else
        {
            Transform destination = ModelTransforms[y].ModelLineTransforms[x];
            CameraViewers[cameraIndex].GoTo(destination);
        }
    }

    public void ShowModelFromDown(string modelName, int cameraIndex = 0)
    {
        Model model = FindModel(modelName);
        int x = model.x + 1;
        int y = model.y + 1 - 1;

        if (y < 1)
        {
            Vector3[] positions = new Vector3[3]
            {
                ModelTransforms[y].ModelLineTransforms[x].position,
                ModelTransforms[y].ModelLineTransforms[x].position,
                ModelTransforms[y].ModelLineTransforms[x].position
            };
            CameraViewers[cameraIndex].GoTo(ModelTransforms[y].ModelLineTransforms[1], positions);
        }
        else
        {
            Transform destination = ModelTransforms[y].ModelLineTransforms[x];
            CameraViewers[cameraIndex].GoTo(destination);
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

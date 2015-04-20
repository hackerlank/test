using UnityEngine;
using System.Collections;
using System;
namespace Common
{
    public interface IModule
    {
        void Start();
        void Update();
        void OnGUI();
        void LateUpdate();
        void OnApplicationFocus();
    }
}
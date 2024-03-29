﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Color
{
    public class ObjectColor : MonoBehaviour
    {
        public UnityEngine.Color color;
        private Renderer renderer;

        private void OnEnable()
        {
            renderer = GetComponent<Renderer>();
            renderer.material.color = color;
        }
    }
}

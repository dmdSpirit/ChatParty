﻿using UnityEngine;

namespace dmdSpirit {
    /// <summary>
    /// Component for storing borders positions.
    /// </summary>
    public class BackgroundController : MonoSingleton<BackgroundController> {
        public Transform leftBorder;
        public Transform rightBorder;
    }
}

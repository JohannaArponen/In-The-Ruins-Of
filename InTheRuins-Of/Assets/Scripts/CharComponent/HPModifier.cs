using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CharacterComponentSystem {
  public class HPModifier : MonoBehaviour {
    [Tooltip("Priority of modifications applied by this modifier")]
    public float priority = 0;
  }
}
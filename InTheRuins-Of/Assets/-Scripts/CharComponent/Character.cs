using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
  public string displayName = "Untitled Character";

  [Range(0, 1)]
  public float goodness = 1;
}

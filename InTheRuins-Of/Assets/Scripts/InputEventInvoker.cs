using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEventInvoker : MonoBehaviour {
  public

  struct InputEvent {

  }

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    if (Input.GetKeyDown(attack))
      attackDown = true;
  }

  // Update is called once per frame
  void FixedUpdate() {
    if (attackDown) {
      attackDown = false;
    }
  }
}

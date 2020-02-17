using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class OnTriggerEvent : MonoBehaviour {
  [Tooltip("Invoked when " + nameof(OnTriggerEnter) + " is triggered")]
  public TriggerUnityEvent enterEvent;
  [Tooltip("Invoked when " + nameof(OnTriggerStay) + " is triggered")]
  public TriggerUnityEvent stayEvent;
  [Tooltip("Invoked when " + nameof(OnTriggerExit) + " is triggered")]
  public TriggerUnityEvent exitEvent;

  [System.Serializable]
  public class TriggerUnityEvent : UnityEvent<Collider> { }

  // Start is called before the first frame update
  void Start() {
    foreach (var col in GetComponents<Collider>()) {
      if (col.isTrigger) return;
    }
    throw new UnityException($"{nameof(OnTriggerEvent)} requires a trigger collider to exist on the GameObject");
  }

  public void Test(Collider col) {
    print(col);
  }

  void OnTriggerEnter(Collider col) => enterEvent.Invoke(col);
  void OnTriggerExit(Collider col) => exitEvent.Invoke(col);
  void OnTriggerStay(Collider col) => stayEvent.Invoke(col);
}

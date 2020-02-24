﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class OnCollisionEvent : MonoBehaviour {

  [Tooltip("Require collider to have specific tag for it to trigger the events")]
  public bool filterTag;

  [MyBox.ConditionalField(nameof(filterTag))]
  [MyBox.Tag]
  [Tooltip("The required tag for colliders")]
  public string filteredTag;

  [Tooltip("Invoked when " + nameof(OnCollisionEnter) + " is triggered")]
  public TriggerUnityEvent enterEvent;
  [Tooltip("Invoked when " + nameof(OnCollisionStay) + " is triggered")]
  public TriggerUnityEvent stayEvent;
  [Tooltip("Invoked when " + nameof(OnCollisionExit) + " is triggered")]
  public TriggerUnityEvent exitEvent;

  [System.Serializable]
  public class TriggerUnityEvent : UnityEvent<Collision> { }

  // Start is called before the first frame update
  void Start() {
    foreach (var col in GetComponents<Collider>()) if (!col.isTrigger) return;
    throw new UnityException($"{nameof(OnCollisionEvent)} requires a non trigger collider to exist on the GameObject");
  }

  public void Test(Collider col) {
    print(col);
  }

  void OnCollisionEnter(Collision col) {
    if (!filterTag || col.gameObject.tag == filteredTag) {
      stayEvent.Invoke(col);
    }
  }
  void OnCollisionExit(Collision col) {
    if (!filterTag || col.gameObject.tag == filteredTag) {
      stayEvent.Invoke(col);
    }
  }
  void OnCollisionStay(Collision col) {
    if (!filterTag || col.gameObject.tag == filteredTag) {
      stayEvent.Invoke(col);
    }
  }
}

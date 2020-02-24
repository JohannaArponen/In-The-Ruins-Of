using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace InteractionSystem {
  public class Interaction {

    public readonly Interactor source;
    public readonly Interactable target;

    /// <summary> The original distance between source and targe </summary>
    public readonly float distance;

    public readonly float startTime;
    /// <summary> Current duration of interaction </summary>
    public float duration { get => ended ? endTime - startTime : Time.time - startTime; }

    public bool ended { get; protected set; }
    public float endTime { get; protected set; }

    public static implicit operator bool(Interaction interaction) => interaction != null;

    public Interaction(Interactor source, Interactable target) => new Interaction(source, target, Time.time);
    public Interaction(Interactor source, Interactable target, float startTime) {
      this.source = source;
      this.target = target;
      this.distance = math.distance(source.transform.position, target.transform.position);
      this.startTime = startTime;
    }

    public void End(float time) {
      if (ended) {
        Debug.LogWarning("Tried to end an interaction multiple times");
        return;
      }
      ended = true;
      endTime = time;
    }
    public void End() {
      if (ended) {
        Debug.LogWarning("Tried to end an interaction multiple times");
        return;
      }
      ended = true;
      endTime = Time.time;
    }
  }
}
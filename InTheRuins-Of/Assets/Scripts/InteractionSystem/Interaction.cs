using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem {
  public class Interaction {

    public readonly Interactor source;
    public readonly Interactable target;
    public readonly float startDistance;
    public readonly float startTime;

    public Vector3 sourcePos { get => source.transform.position; }
    public Vector3 targetPos { get => target.transform.position; }

    public float distance { get => Vector3.Distance(sourcePos, targetPos); }

    public float duration { get => ended ? endTime - startTime : Time.time - startTime; }

    public bool ended { get; private set; }
    public float endTime { get; private set; }

    public static implicit operator bool(Interaction interaction) => interaction != null;

    public Interaction(Interactor source, Interactable target) => new Interaction(source, target, Time.time);
    public Interaction(Interactor source, Interactable target, float startTime) {
      this.source = source;
      this.target = target;
      this.startDistance = Vector3.Distance(source.transform.position, target.transform.position);
      this.startTime = startTime;
    }

    public void End() => End(Time.time);
    public void End(float time) {
      if (ended) {
        Debug.LogWarning("Tried to end an interaction multiple times");
        return;
      }
      ended = true;
      endTime = time;
    }
  }
}
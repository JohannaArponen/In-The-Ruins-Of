using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using MyBox;


namespace InteractionSystem {
  [RequireComponent(typeof(Rigidbody))]
  [RequireComponent(typeof(Interactable))]
  public class Movable : MonoBehaviour {

    [Tooltip(
      "The maximum amount of samples to take from previous positions for velocity calculation.\n\n" +
      "Higher values reduce the chance of small movements affecting the throw in an unwanted manner."
    )]
    public int sampleCount = 2;

    [
      Tooltip("Keep distance from the " + nameof(Interactor) + " within this range (from 0 to maximum interaction distance)"),
      MinMaxRange(0, 1)
    ]
    public FloatRange distanceRange = new FloatRange(0, 1);

    [Tooltip("Move " + nameof(Interactable) + " to the center of the " + nameof(Interactor) + "'s ray")]
    public bool restrictCenter;

    public bool3 restrictRotation;
    public bool3 restrictPosition;


    private Interactable interactable;
    private Rigidbody rb;

    private CircularBuffer<Sample> samples;
    private class Sample {
      public Vector3 pos; public float delta;
      public Sample(Vector3 pos, float delta) { this.pos = pos; this.delta = delta; }
    }

    private Interaction interaction;
    private float targetDistance;
    private bool usedGravity;


    void OnValidate() {
      sampleCount = math.max(2, sampleCount);
      samples = new CircularBuffer<Sample>(sampleCount);
    }

    void Start() {
      OnValidate();
      rb = GetComponent<Rigidbody>();
      interactable = GetComponent<Interactable>();
      interactable.AddActivationEventListeners(OnActivate, OnActive, OnDeactive);
    }

    public void OnActivate(Interaction inter) {
      if (interaction && !interaction.ended) inter.End();
      interaction = inter;
      usedGravity = rb.useGravity;
      rb.useGravity = false;
      var maxDir = inter.dir.SetLen(inter.source.maxDistance);
      Line line = new Line(
        Vector3.Lerp(inter.sourcePos, inter.sourcePos + maxDir, distanceRange.min),
        Vector3.Lerp(inter.sourcePos, inter.sourcePos + maxDir, distanceRange.max)
      );
      var closestPoint = line.ClampToLine(inter.targetPos);
      targetDistance = Vector3.Distance(inter.sourcePos, closestPoint);
      samples.Clear();
    }

    public void OnActive(Interaction inter) {
      samples.Add(new Sample(transform.position, Time.deltaTime));
      var targetPos = inter.sourcePos + (inter.dir.SetLenSafe(targetDistance).SetDirSafe(inter.source.transform.forward));
      rb.MovePosition(targetPos);
    }

    public void OnDeactive(Interaction inter) {
      rb.useGravity = usedGravity;
      var count = 0;
      Sample merged = new Sample(Vector3.zero, 0);
      foreach (var sample in samples) {
        if (sample != null) {
          count++;
          if (count == 1) continue;
          merged.pos += sample.pos;
          merged.delta += sample.delta;
        }
      }
      // Need atleast 2 samples
      if (count >= 2) {
        Sample avg = new Sample(merged.pos / count, merged.delta / count);
        var oldest = samples[samples.Length - 1].pos;
        rb.velocity = (oldest - avg.pos) / avg.delta;
      }
    }
  }
}

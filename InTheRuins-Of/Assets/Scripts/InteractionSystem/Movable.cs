﻿using System.Collections;
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
    public int sampleCount = 3;

    [
      Tooltip("Keep distance from the " + nameof(Interactor) + " within this range (from 0 to maximum interaction distance)"),
      MinMaxRange(0, 1)
    ]
    public FloatRange distanceRange = new FloatRange(0, 1);

    [Tooltip("Move " + nameof(Interactable) + " to the center of the " + nameof(Interactor) + "'s ray")]
    public bool restrictCenter;

    [Tooltip("Multiplier of movement towards desired point")]
    public float moveVelocityWhenColliding = 20;

    [Tooltip("Enable collision when no longer colliding with the " + nameof(Interactor) + " instead of immediately")]
    public bool waitCollisionEnd = true;

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
    private List<Collider> awaitingCols = new List<Collider>();

    void OnValidate() {
      sampleCount = math.max(3, sampleCount);
      samples = new CircularBuffer<Sample>(sampleCount);
    }

    void Start() {
      OnValidate();
      rb = GetComponent<Rigidbody>();
      interactable = GetComponent<Interactable>();
      interactable.AddActivationEventListeners(OnActivate, OnActive, OnDeactive);
    }

    void Update() {
      if (waitCollisionEnd && awaitingCols.Count > 0) {
        var col = GetComponent<Collider>();

        // We must unignore before sweeping or we dont hit the colliders
        foreach (var awaitCol in awaitingCols) Physics.IgnoreCollision(col, awaitCol, false);
        var cols = rb.SweepTestAll(Vector3.forward, 0).Map(hit => hit.collider);
        foreach (var awaitCol in awaitingCols) Physics.IgnoreCollision(col, awaitCol, true);

        foreach (var awaitingCol in awaitingCols) {
          if (cols.IndexOfItem(awaitingCol) == -1) {
            Physics.IgnoreCollision(rb.GetComponent<Collider>(), interaction.source.associatedCollider, false);
            awaitingCols.Remove(awaitingCol);
          }
        }
      }
    }

    public void OnActivate(Interaction inter) {
      if (interaction && !interaction.ended) inter.End();
      interaction = inter;
      usedGravity = rb.useGravity;
      if (inter.source.associatedCollider)
        Physics.IgnoreCollision(rb.GetComponent<Collider>(), interaction.source.associatedCollider);
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
      var dir = targetPos - transform.position;
      if (rb.SweepTest(dir.normalized, out var hit, dir.magnitude * 1.1f)) {
        rb.velocity = dir * moveVelocityWhenColliding;
      } else {
        rb.velocity = Vector3.zero;
        rb.MovePosition(targetPos);
      }
    }

    public void OnDeactive(Interaction inter) {
      rb.useGravity = usedGravity;
      var assCol = interaction.source.associatedCollider;
      if (assCol && waitCollisionEnd) {
        if (!awaitingCols.Contains(assCol)) awaitingCols.Add(assCol);
      }
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
      // Need atleast 3 valid samples
      if (count >= 3) {
        var oldest = samples[samples.Length - 1].pos;
        var vel = -(oldest - merged.pos / (count - 1)) / merged.delta;
        rb.velocity = vel;
      } else {
        rb.velocity = Vector3.zero;
      }
    }
  }
}

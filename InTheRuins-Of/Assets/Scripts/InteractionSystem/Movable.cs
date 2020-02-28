using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


namespace InteractionSystem {
  [RequireComponent(typeof(Interactable))]
  // NAMETO: Adjustable
  public class Movable : MonoBehaviour {

    [Tooltip("Note that you wont be able to throw things if they dont have a rigidbody or if the global position is preserved")]
    public bool throwable = true;
    [
      Tooltip(
        "The maximum amount of samples to take from previous positions for velocity calculation.\n\n" +
        "Higher values reduce the chance of small movements affecting the throw in an unwanted manner."
      ),
      MyBox.ConditionalField(nameof(throwable))
    ]
    public int sampleCount = 1;

    [Tooltip(
      "None: Rotation is not affected\n\n" +
      "Relative: Rotation is preversed in relation to the interactor\n\n" +
      "Look: Look at interactor\n\n" +
      "Global: Rotation is preserved in global space\n\n" +
      "Local: Rotation is preserved in local space (relative to parent)"
    )]
    public RotationMode rotationMode = RotationMode.relative;
    public enum RotationMode { none, relative, look, global, local, }

    [Tooltip(
      "None: Position is not affected\n\n" +
      "Relative: Position is preversed in relation to the interactor\n\n" +
      "Global: Position is preserved in global space\n\n" +
      "Local: Position is preserved in local space (relative to parent)"
    )]
    public PositionModes positionMode = PositionModes.relative;
    public enum PositionModes { none, relative, look, global, local, }

    [Tooltip("Preserve position in relation to the interactor")]
    public bool preserveOffset = false;

    private Interactable interactable;
    private Rigidbody rb;
    private CircularBuffer<Sample> samples;
    private class Sample {
      public Vector3 pos; public float delta;
      public Sample(Vector3 pos, float delta) { this.pos = pos; this.delta = delta; }
    }

    private Interaction interaction;

    private Vector3 pos;
    private Vector3 rot;
    private Vector3 relatorPos;
    private Vector3 relatorRot;

    void OnValidate() {
      sampleCount = math.max(2, sampleCount);
      samples = new CircularBuffer<Sample>(sampleCount);
    }

    void Start() {
      OnValidate();
      interactable = GetComponent<Interactable>();
      interactable.AddActivationEventListeners(OnActivate, OnActive, OnDeactive);
      rb = GetComponent<Rigidbody>();
    }

    public void OnActivate(Interaction interaction) {
      if (interaction && !interaction.ended) interaction.End();
      this.interaction = interaction;
      var distance = interaction.distance;
      // ABSOLUTE GONK
      pos = transform.position;
      rot = transform.eulerAngles;
      relatorPos = transform.position;
      relatorRot = transform.eulerAngles;
      switch (rotationMode) {
        case RotationMode.none: break;
        case RotationMode.relative: relatorRot = interaction.source.transform.InverseTransformDirection(transform.eulerAngles); break;
        case RotationMode.look: break;
        case RotationMode.global: relatorRot = transform.eulerAngles; break;
        case RotationMode.local: relatorRot = transform.localEulerAngles; break;
      }
      samples.Clear();
    }
    public void OnActive(Interaction _) {
      if (rb) {
        switch (rotationMode) {
          case RotationMode.none: break;
          case RotationMode.relative: relatorRot = transform.forward + relatorRot; break;
          case RotationMode.look: transform.LookAt(interaction.source.transform, Vector3.up); break;
          case RotationMode.global: transform.eulerAngles = relatorRot; break;
          case RotationMode.local: transform.localEulerAngles = relatorRot; break;
        }
      } else {
        switch (rotationMode) {
          case RotationMode.none: break;
          case RotationMode.relative: transform.eulerAngles = relatorRot - interaction.source.transform.eulerAngles; break;
          case RotationMode.look: transform.LookAt(interaction.source.transform, Vector3.up); break;
          case RotationMode.global: transform.eulerAngles = relatorRot; break;
          case RotationMode.local: transform.localEulerAngles = relatorRot; break;
        }
      }
      samples.Add(new Sample(transform.position, Time.deltaTime));
    }

    public void OnDeactive(Interaction inter) {
      if (rb) {
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
        Sample avg = new Sample(merged.pos / count, merged.delta / count);
        rb.velocity = (avg.pos - samples[0].pos) * avg.delta;
      }
    }
  }
}

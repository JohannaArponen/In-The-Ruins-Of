using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


namespace InteractionSystem {
  [RequireComponent(typeof(Interactable))]
  [RequireComponent(typeof(Rigidbody))]
  public class Movable : MonoBehaviour {

    [MyBox.PositiveValueOnly, GetSet("sampleCount"), Tooltip(
      "The maximum amount of samples to take from previous positions for velocity calculation. " +
      "Higher values reduce the chance of small movements affecting the throw in an unvanted manner."
    )]
    private int _sampleCount = 1;
    protected int sampleCount {
      get => _sampleCount;
      private set {
        var @new = new Stack<float3>();
        for (int i = 0; i < samples.Count; i++) {
          @new.Push(samples.Pop());
        }
      }
    }
    private Stack<float3> samples = new Stack<float3>();

    protected Interactable interactable;
    protected Rigidbody rb;

    void Start() {
      interactable = GetComponent<Interactable>();
      rb = GetComponent<Rigidbody>();
    }

    public void Hold() {

    }
    public void Throw() {

    }
  }
}

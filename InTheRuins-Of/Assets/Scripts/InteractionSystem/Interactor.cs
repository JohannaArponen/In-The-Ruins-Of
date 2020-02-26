﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem {
  public class Interactor : MonoBehaviour {

    [Tooltip("The maximum distance to " + nameof(Interactable) + "'s collider. (Calculated with nearest point)")]
    public float maxDistance = 2;

    [Tooltip("The maximum distance to the hit point of the raycast")]
    public float rayLength = 3f;

    [Tooltip("Layer mask used for raycast")]
    public LayerMask mask;

    [MyBox.SearchableEnum]
    public KeyCode key = KeyCode.E;

    [Tooltip(
      nameof(Type.Hold) + ": Activate on press and deactivate on release.\n\n" +
      nameof(Type.Toggle) + ": Activate on press and deactivate when pressed again.\n\n" +
      nameof(Type.Instant) + ": Activate on press and deactivate immediately."
    )]
    public Type type = Type.Hold;
    public enum Type { Hold, Toggle, Instant }

    public DeactivationRules restrictions;
    [System.Serializable]
    public class DeactivationRules {
      [Tooltip("Deactivate if the raycast no longer hits the " + nameof(Interactable))]
      public bool sigth = true;

      [Tooltip("Deactivate " + nameof(Interactable) + " if the maximum distance is exeeded")]
      public bool distance = true;

      [Tooltip("Deactivate if the angle towards " + nameof(Interactable) + " exeeds this"), Range(0, 180)]
      public float angle = 180f;
    }


    protected Interactable interactable;
    protected Interaction interaction;

    void Update() {
      // If an interaction is happening
      if (interaction) {
        // Maybe the interaction was ended somewhere else?
        if (interaction.ended) {
          interaction = null;
          // Recall Update so new interactions can be immediately recognized
          Update();
          return;
        }
        if (type == Type.Toggle ? Input.GetKeyDown(key) : !Input.GetKey(key) || !Complies(interaction)) {
          interactable.Deactivate();
          return;
        }
        interactable.Active();
      } else {
        var pos = transform.position;
        // Check if targetting something valid and do appropriate things
        Debug.DrawRay(pos, transform.forward * rayLength);
        RaycastHit hit;
        var prevInteractable = interactable;
        if (Physics.Raycast(pos, transform.forward, out hit, rayLength, mask)) {
          // If hit interactable object
          if (hit.collider.TryGetComponent(out interactable)) {
            // Check if within the required distance
            if (hit.distance < maxDistance || Vector3.Distance(pos, hit.collider.ClosestPoint(pos)) < maxDistance) {
              if (Input.GetKeyDown(key)) {
                // Pressed. Activate the interactable
                if (type == Type.Instant) {
                  interactable.Activate(this);
                  interactable.Deactivate();
                } else {
                  interaction = interactable.Activate(this);
                  interactable.Active();
                }
              } else {
                // Not pressed. Target if necessary
                if (!interactable.targeted) {
                  interactable.Target();
                }
              }
            } else {
              // No Interactable was hit. Untarget if necessary
              if (prevInteractable && prevInteractable.targeted) {
                prevInteractable.Untarget();
              }
            }
          }
        } else {
          // Nothing was hit. Untarget if necessary
          if (prevInteractable && prevInteractable.targeted) {
            prevInteractable.Untarget();
          }
        }
      }
    }

    bool Complies(Interaction interaction) {
      if (restrictions.sigth) {
        var len = restrictions.distance ? maxDistance : float.PositiveInfinity;
        if (Physics.Raycast(interaction.sourcePos, transform.forward, out var hit, len, mask)) {
          if (hit.collider.gameObject != interaction.target.gameObject) return false;
        } else {
          return false; // well if it didnt hit anything it definitely didnt hit the target
        }
      }
      // Distance checking is done with the raycast if sigth is enabled!
      if (restrictions.distance && !restrictions.sigth) {
        if (interaction.distance > maxDistance) return false;
      }

      if (restrictions.angle < 180) {
        var to = this.interaction.source.transform.forward;
        var from = (this.interaction.targetPos - this.interaction.sourcePos).normalized;
        if (Vector3.Angle(to, from) > restrictions.angle) return false;
      }

      return true;
    }
  }
}
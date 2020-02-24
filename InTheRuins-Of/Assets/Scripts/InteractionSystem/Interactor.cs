using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem {
  public class Interactor : MonoBehaviour {

    [Tooltip("The maximum distance of an interactable. (To the collision point, not the GameObject)")]
    public float distance = 1;

    [MyBox.SearchableEnum]
    public KeyCode key = KeyCode.E;

    [Tooltip(
      nameof(Type.Hold) + ": Activate on press and unactivate on release.\n\n" +
      nameof(Type.Toggle) + ": Activate on press and unactivate when pressed again.\n\n" +
      nameof(Type.Instant) + ": Activate on press and unactivate immediately."
    )]
    public Type type = Type.Hold;
    public enum Type { Hold, Toggle, Instant }

    public LayerMask mask;

    protected Interactable interactable;
    protected Interaction interaction;

    // Update is called once per frame
    void Update() {
      // If an interaction is happening
      if (interaction) {
        // Maybe it got deactivated somewhere else?
        if (interaction.ended) {
          interaction = null;
          // Recall Update so new interactions can be immediately recognized
          Update();
          return;
        }
        if (type == Type.Toggle ? Input.GetKeyDown(key) : !Input.GetKey(key)) {
          interactable.Deactivate();
          return;
        }
        interactable.Active();
      } else {
        // Check if targetting something valid and do appropriate things
        Debug.DrawRay(transform.position, transform.forward * distance);
        RaycastHit hit;
        var prevInteractable = interactable;
        if (Physics.Raycast(transform.position, transform.forward * distance, out hit, distance, mask)) {
          // If hit interactable object
          if (hit.collider.TryGetComponent(out interactable)) {
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
        } else {
          // Nothing was hit. Untarget if necessary
          if (prevInteractable && prevInteractable.targeted) {
            prevInteractable.Untarget();
          }
        }
      }
    }
  }
}
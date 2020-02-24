using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem {
  [RequireComponent(typeof(Collider2D))]
  public class Interactable : MonoBehaviour {

    [Tooltip("When the interactable gains focus")]
    public InteractableEvent onTarget;
    [Tooltip("When the interactable loses focus")]
    public InteractableEvent onUntarget;

    [Tooltip("When the interaction is started")]
    public InteractionEvent onActivate;
    [Tooltip("When the interaction is ongoing")]
    public InteractionEvent onActive;
    [Tooltip("When the interaction is ended")]
    public InteractionEvent onDeactivate;

    [System.Serializable]
    public class InteractableEvent : UnityEvent<Interactable> { };
    [System.Serializable]
    public class InteractionEvent : UnityEvent<Interaction> { };

    /// <summary> Whether or not the interactable is being targeted </summary>
    public bool targeted { get; protected set; }

    /// <summary> The current interaction or null </summary>
    public Interaction interaction { get; protected set; }

    public void Target() {
      if (targeted) {
        Debug.LogWarning($"Trying to target an {nameof(Interactable)} multiple times");
        return;
      }
      targeted = true;
      onTarget.Invoke(this);
    }
    public void Untarget() {
      if (!targeted) {
        Debug.LogWarning($"Trying to untarget an {nameof(Interactable)} multiple times");
        return;
      }
      targeted = false;
      onUntarget.Invoke(this);
    }

    /// <summary>
    /// Activates the Interactable.  
    /// If the Interactable is targeted, it is untargeted.  
    /// </summary>
    public Interaction Activate(Interactor source) {
      if (targeted) Untarget();
      if (interaction) {
        interaction.End();
        onDeactivate.Invoke(interaction);
      }
      interaction = new Interaction(source, this);
      onActivate.Invoke(interaction);
      return interaction;
    }

    /// <summary> Invokes the onActivate listeners </summary>
    public void Active() {
      onActive.Invoke(interaction);
    }

    /// <summary> Deactivates the Interactable </summary>
    public void Deactivate() {
      if (interaction.ended) return;
      interaction.End();
      onDeactivate.Invoke(interaction);
      interaction = null;
    }
  }
}
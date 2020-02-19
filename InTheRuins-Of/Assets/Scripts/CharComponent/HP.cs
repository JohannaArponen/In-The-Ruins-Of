using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HP : MonoBehaviour {
  public float health = 1;
  public bool invulnerable;

  [Tooltip("After add or substract health. (" + nameof(HP) + " component, real change, wanted change)")]
  public PostHealthAdd postHealthAdd;

  [Tooltip("After set health. (" + nameof(HP) + " component, original value)")]
  public OnHealthSetEvent onHealthSet;

  [Tooltip("Invoked after any change to health. Values passed: (" + nameof(HP) + " component, original value)")]
  public OnHealthEvent onHealth;

  private List<Modifier> modifiers = new List<Modifier>();


  [System.Serializable] public class PostHealthAdd : UnityEvent<HP, float, float> { }
  [System.Serializable] public class OnHealthSetEvent : UnityEvent<HP, float> { }
  [System.Serializable] public class OnHealthEvent : UnityEvent<HP, float> { }

  #region Parameter Implementation

  public delegate float ModifierHandler(HP hp, float change);
  private class Modifier {
    public Modifier(float priority, ModifierHandler handler) {
      this.priority = priority;
      this.handler = handler;
    }

    public float priority = 0;
    public ModifierHandler handler;
  }

  private bool invoking = false;

  #endregion


  #region Methods

  public void AddHealth(float change) {
    if (invoking) {
      health = health + change;
      return;
    } else if (invulnerable) return;

    var old = health;

    var modChange = change;

    foreach (var cb in modifiers) modChange = cb.handler(this, modChange);

    invoking = true;
    postHealthAdd.Invoke(this, modChange, change);
    invoking = false;

    onHealth.Invoke(this, old);
  }

  public void SetHealth(float value) {
    var old = health;
    health = value;
    onHealthSet.Invoke(this, old);

    onHealth.Invoke(this, old);
  }

  /// <summary> Modifies the change value when using AddHealth <summary/>
  public void AddModifier(float priority, ModifierHandler handler) {
    var modifier = new Modifier(priority, handler);
    for (int i = 0; i < modifiers.Count; i++) {
      var mod = modifiers[i];
      if (mod.priority <= priority) {
        modifiers.Insert(i, modifier);
        return;
      }
    }
    modifiers.Add(modifier);
  }

  #endregion

}

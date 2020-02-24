using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour {
  [SerializeField, Candlelight.PropertyBackingField]
  protected float _stamina = 1;
  public float stamina { get => _stamina; protected set => _stamina = value; }
  public float maximum = 1;
  public float regenPerSec = 1;
  public float regenDelay = 0.5f;

  protected float lastUse;

  public static implicit operator float(Stamina s) => s.stamina;

  void Update() {
    // Enforce delay
    if (Time.time < lastUse + regenDelay) return;
    stamina += regenPerSec * Time.deltaTime;
    stamina = Mathf.Min(stamina, maximum);
  }

  public bool HasStamina(float amount) => stamina >= amount;

  public bool TryUse(float stamina) {
    if (HasStamina(stamina)) {
      this.stamina -= stamina;
      lastUse = Time.time;
      return true;
    }
    return false;
  }

  public void ForceUse(float stamina, bool clampToZero = true) {
    this.stamina -= stamina;
    lastUse = Time.time;
    if (clampToZero && this.stamina < 0) this.stamina = 0;
  }

  public void Add(float stamina) {
    this.stamina += stamina;
    stamina = Mathf.Min(stamina, maximum);
  }

}

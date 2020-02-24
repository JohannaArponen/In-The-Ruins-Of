using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HP))]
public class DamageImmunityDuration : HPModifier {

  [Tooltip("The duration of immunity after receiving damage")]
  public float duration = 1;

  [Tooltip("Allow bigger damage instances to pass through immunity (and refresh immunity). Only the additional damage is applied.")]
  public bool highDamageOverride = true;

  public HP hp;
  public bool isImmune { get => Time.time <= lastDamageTime + duration; }

  protected float lastDamageTime;
  protected float lastDamage;


  void Start() {
    if (hp == null) hp = GetComponent<HP>();
    if (hp == null) throw new UnityException("No health component specified and none found on the GameObject");
    hp.damageModifiers.Add((damage, original) => {

      if (!highDamageOverride) {
        if (!isImmune) {
          lastDamageTime = Time.time;
          lastDamage = damage;
          return damage;
        }
        return 0;
      }

      if (!isImmune) {
        lastDamageTime = Time.time;
        lastDamage = damage;
        return damage;
      }

      if (lastDamage < damage) {
        var diff = damage - lastDamage;
        lastDamageTime = Time.time;
        lastDamage = damage;
        return diff;
      }

      return 0;
    });
  }
}

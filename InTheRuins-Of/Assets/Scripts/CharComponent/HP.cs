﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HP : MonoBehaviour {

  [SerializeField] protected float _health = 1;
  public float health { get => _health; protected set => _health = value; }

  public float prevHealth { get; protected set; }

  [Tooltip("Invoked in LateUpdate if health was changed. HP")]
  public OnChangeEvent onChange;
  [System.Serializable] public class OnChangeEvent : UnityEvent<HP> { }


  [Tooltip("Invoked after taking damage. HP, Damage, Filtered damage")]
  public OnDamageEvent onDamage;
  [System.Serializable] public class OnDamageEvent : UnityEvent<HP, float, float> { }

  [Tooltip("Invoked after any heal. HP, Heal, Filtered heal")]
  public OnHealEvent onHeal;
  [System.Serializable] public class OnHealEvent : UnityEvent<HP, float, float> { }

  [Tooltip("Invoked after health being set. HP, Value, Filtered value")]
  public OnSetEvent onSet;
  [System.Serializable] public class OnSetEvent : UnityEvent<HP, float, float> { }


  public FilterList<Func<HP, float, float>> damageFilters = new FilterList<Func<HP, float, float>>();
  public FilterList<Func<HP, float, float>> healFilters = new FilterList<Func<HP, float, float>>();
  public FilterList<Func<HP, float, float>> setFilters = new FilterList<Func<HP, float, float>>();

  public class Filter<T> where T : Delegate {
    public Filter(float priority, T function) {
      this.priority = priority;
      this.function = function;
    }
    public float priority = 0;
    public T function;
  }

  void Start() {
    prevHealth = _health;
  }

  void LateUpdate() {
    if (prevHealth == health) return;
    onChange.Invoke(this);
    prevHealth = health;
  }


  public void Damage(float damage) {
    var filtered = damage;
    foreach (var filter in damageFilters) filtered = filter.function(this, filtered);
    health -= filtered;
    onDamage.Invoke(this, damage, filtered);
  }

  public void Heal(float healing) {
    var filtered = healing;
    foreach (var filter in healFilters) filtered = filter.function(this, filtered);
    health += filtered;
    onHeal.Invoke(this, healing, filtered);
  }

  public void Set(float value) {
    var filtered = value;
    foreach (var filter in setFilters) filtered = filter.function(this, filtered);
    health = filtered;
    onSet.Invoke(this, value, filtered);
  }
}

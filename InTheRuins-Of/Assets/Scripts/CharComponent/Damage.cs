using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {
  public float damage = 1;
  [Tooltip("You can hit multiple targets but not the same target twice during this cooldown")]
  public bool perTargetCooldown;
  [Tooltip("Duration of cooldown after hitting a target")]
  public float cooldown;
  // Start is called before the first frame update
  void Start() {

  }


  void DamageObject(Collider col, float damage) => DamageObject(col.gameObject, damage);
  void DamageObject(GameObject col, float damage) {
    var hp = GetComponent<HP>();
    if (!hp) return;
    hp.Damage(damage);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterComponentSystem {
  [RequireComponent(typeof(HP))]
  public class IntervalHeal : MonoBehaviour {
    public float interval = 1;
    public float heal = 1;
    public HP hp;

    protected float lastHeal;

    // Start is called before the first frame update
    void Start() {
      lastHeal = Time.time;
      if (hp == null) hp = GetComponent<HP>();
      if (hp == null) throw new UnityException("No health component specified and none found on the GameObject");
    }

    // Update is called once per frame
    void Update() {
      while (Time.time >= lastHeal + interval) {
        lastHeal += interval;
        hp.Heal(heal);
      }
    }
  }
}
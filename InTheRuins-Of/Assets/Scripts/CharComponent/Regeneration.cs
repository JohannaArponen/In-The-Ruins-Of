using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterComponentSystem {
  [RequireComponent(typeof(HP))]
  public class Regeneration : MonoBehaviour {

    public float healPerSec = 1;
    public HP hp;

    // Start is called before the first frame update
    void Start() {
      if (hp == null) hp = GetComponent<HP>();
      if (hp == null) throw new UnityException("No health component specified and none found on the GameObject");
    }

    // Update is called once per frame
    void Update() {
      hp.Heal(healPerSec * Time.deltaTime);
    }
  }
}
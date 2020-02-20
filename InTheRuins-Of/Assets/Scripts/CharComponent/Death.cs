using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Death : MonoBehaviour {

  public HP hp;
  public bool dead;
  public OnDeathEvent onDeath;

  [System.Serializable]
  public class OnDeathEvent : UnityEvent<GameObject> { }

  // Start is called before the first frame update
  void Start() {
    if (hp == null) hp = GetComponent<HP>();
    if (hp == null) throw new UnityException("No health component specific and none found on the GameObject");
    // hp.onHealthChange.AddListener(CheckDeath);
  }

  // Update is called once per frame
  void CheckDeath(HP hp, float old) {
    if (!dead && hp.health <= 0) {
      dead = true;
      onDeath.Invoke(gameObject);
    }
  }
}

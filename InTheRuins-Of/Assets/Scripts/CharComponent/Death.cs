using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Death : MonoBehaviour {

  public HP hp;
  [SerializeField]
  private bool _dead = false;
  public bool dead {
    get => _dead; protected set {
      if (_dead = value) return;
      if (value) onDeath.Invoke(gameObject);
      else onRevive.Invoke(gameObject);
      _dead = value;
    }
  }
  public OnDeathStateChangeEvent onDeath;
  public OnDeathStateChangeEvent onRevive;

  [System.Serializable]
  public class OnDeathStateChangeEvent : UnityEvent<GameObject> { }

  // Start is called before the first frame update
  void Start() {
    if (hp == null) hp = GetComponent<HP>();
    if (hp == null) throw new UnityException("No health component specific and none found on the GameObject");
    hp.onChange.AddListener(CheckDeath);
  }

  void CheckDeath(HP hp) { if (!dead && hp.health <= 0) Kill(); }

  /// <summary> Kills the unit if alive. Health is set to 0 if it is not negative </summary>
  /// <returns> Whether or not the unit was killed. False if already dead </returns>
  public bool Kill() {
    if (!dead) {
      if (hp.health > 0) hp.Set(0);
      dead = true;
      onDeath.Invoke(gameObject);
      return true;
    } else {
      return false;
    }
  }

  /// <summary> Revives the unit if dead. Health is set to 1 if it is not positive </summary>
  /// <returns> Whether or not the unit was revived. False if already alive </returns>
  public bool Revive() {
    if (dead) {
      if (hp.health <= 0) hp.Set(1);
      dead = false;
      onRevive.Invoke(gameObject);
      return true;
    } else {
      return false;
    }
  }
}

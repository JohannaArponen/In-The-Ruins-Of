using UnityEngine;

[CreateAssetMenu(fileName = "Untitled Enemy", menuName = "NPC/Enemy")]
public class Enemy : ScriptableObject {
  public new string name;
  public float health;
  public float armor;
  public float damage;

  public float speed;
}
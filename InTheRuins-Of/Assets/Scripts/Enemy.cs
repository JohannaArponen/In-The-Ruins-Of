using UnityEngine;

[CreateAssetMenu(fileName = "Untitled Enemy", menuName = "NPC/Enemy")]
public class Enemy : ScriptableObject {
  public string npcName = "Untitled Enemy";

  public float health = 1;
  public float armor = 0;
  public float damage = 1;

  public float sightRange = 5;
}
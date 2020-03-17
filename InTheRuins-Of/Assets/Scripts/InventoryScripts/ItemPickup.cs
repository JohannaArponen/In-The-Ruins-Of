using UnityEngine;

public class ItemPickup : Inter_actableS{

    public Item item;
    public override void Interact() {
        base.Interact();
        PickUP();
    }

    void PickUP() {
        Debug.Log("Picking up " + item.name);
        bool wasPickUp = Inventory.instance.Add(item);

        if(wasPickUp)
        Destroy(gameObject);
    }
}

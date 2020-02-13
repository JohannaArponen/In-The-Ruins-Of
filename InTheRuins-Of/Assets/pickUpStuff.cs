using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpStuff : MonoBehaviour
{
    public int distanceToItem;
    // Star t is called before the first frame update
    void Start()
    {

    }
}

    
   /* // Update is called once per frame
    void Update()
    {
        Collect();
    }
    void Collect()
    {
        if (Input.GetMouseButtonUp(1))(
            RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition){

            if (Physics.Raycast(ray, out hit, distanceToItem))
                if (hit.collider.gameObject.name == "item")
        }

    }
}
 */
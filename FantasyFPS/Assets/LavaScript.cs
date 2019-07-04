using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{

    public DealDamage damage;
    // Start is called before the first frame update
    void Start()
    {
        damage.IsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

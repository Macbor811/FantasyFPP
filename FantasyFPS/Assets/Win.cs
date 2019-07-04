using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Win : MonoBehaviour
{
    public GameObject winText;
    private bool win = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(win)
        {
            winText.SetActive(true);
            GetComponent<FirstPersonController>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WinArea")
            win = true;
    }
}

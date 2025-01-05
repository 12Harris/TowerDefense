using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurretCollider : MonoBehaviour
{

    public event Action _onTreeDetected;

    public event Action _onTreeLeft;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.SendMessage("AddDamage");
        }*/
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("on trigger enter turret");
        if(other.gameObject.layer == 8)
        {
            _onTreeDetected?.Invoke();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            _onTreeLeft?.Invoke();
        }
    }

    public void Enable(bool value)
    {
        GetComponent<BoxCollider>().enabled = value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    public GameObject ArrowPrefab; 

    public override void OnAttack(Vector3 OwnerPosition, Vector3 Direction)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        base.MyCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

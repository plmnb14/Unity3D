using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public enum PartsType { RightHand, LeftHand, Helmet, BodyArmor, Boots, Cape, PartsTypeEnd };

    private PartsType partType { get; set; } = PartsType.RightHand;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneCornerFinder))]
public class BuoyPlacer : MonoBehaviour
{

    public GameObject buoyPref;
    // Start is called before the first frame update
    void Start()
    {
        PlaneCornerFinder cornerFinder = GetComponent<PlaneCornerFinder>();

        //GameObject instbuoy = Instantiate(buoyPref, )
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public float density;

    private BoxCollider waterCollider;
    private TransformHelper waterPlane;

    private void Start() {
        this.waterPlane = this.transform.GetChild(0).GetComponent<TransformHelper>() ;
        this.waterCollider = GetComponent<BoxCollider>();
        //this.waterCollider.center = new Vector3(0, GetWaterLevel()/2, 0);
        //this.waterCollider.size = new Vector3(10, GetWaterLevel(), 10);
    }

    public float GetWaterLevel() {
        return waterPlane.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        ILiquidReactive absF = other.gameObject.GetComponent<ILiquidReactive>();
        if (absF != null) {
            absF.SetWater(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ILiquidReactive absF = other.gameObject.GetComponent<ILiquidReactive>();
        if (absF != null){
            absF.UnsetWater(this);
        }
    }
}

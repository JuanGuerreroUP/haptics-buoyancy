using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public float density;

    private BoxCollider waterCollider;
    private TransformHelper waterPlane;

    private void Awake() {
        Transform waterTransform = this.transform.GetChild(0);
        this.waterPlane = waterTransform.GetComponent<TransformHelper>();
        PlaneCornerFinder cornerFinder = waterTransform.GetComponent<PlaneCornerFinder>();
        this.waterCollider = GetComponent<BoxCollider>();
        this.waterCollider.center = new Vector3(0, -50 + GetWaterLevel(), 0);
        this.waterCollider.size = new Vector3(cornerFinder.GetWidth(), 100, cornerFinder.GetDepth()); 
    }

    public float GetWaterLevel() {
        return waterPlane.Position.y;
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

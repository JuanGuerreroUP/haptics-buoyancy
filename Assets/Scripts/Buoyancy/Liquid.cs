using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public float density;
    [SerializeField]
    private float depth = 100;

    private BoxCollider waterCollider;
    private TransformHelper waterPlane;


    private void Awake() {
        Transform waterTransform = this.transform.GetChild(0);
        this.waterPlane = waterTransform.GetComponent<TransformHelper>();
        PlaneCornerFinder cornerFinder = waterTransform.GetComponent<PlaneCornerFinder>();
        this.waterCollider = GetComponent<BoxCollider>();
        float halfDepth = depth / 2;
        this.waterCollider.center = new Vector3(0, -halfDepth + GetWaterLevel(), 0);
        this.waterCollider.size = new Vector3(cornerFinder.GetWidth(), depth, cornerFinder.GetDepth()); 
    }

    public float GetWaterLevel() {
        return waterPlane.Position.y;
    }
    public float GetDepth(float y)
    {
        return Mathf.Abs(GetWaterLevel() - y);
    }
    public float GetFloorLevel()
    {
        return GetWaterLevel() - depth;
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

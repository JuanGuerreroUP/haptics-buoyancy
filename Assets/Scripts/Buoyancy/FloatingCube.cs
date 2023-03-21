using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(RotatedCubeHelper))]
public class FloatingCube : AbstractFloatingObject {

    private RotatedCubeHelper rotatedCubeHelper;

    protected override void OnAwake()
    {
        this.rotatedCubeHelper = GetComponent<RotatedCubeHelper>();
    }

    public override float GetObjVolume()
    {
        return this.transformHelper.Scale.x * this.transformHelper.Scale.y * this.transformHelper.Scale.z;
    }

    public override void SetWater(Liquid water)
    {
        base.SetWater(water);
        this.rotatedCubeHelper.waterLevel = water.GetWaterLevel();
    }

    public override float GetDisplacedVolume()
    {
        if(this.transformHelper.Rotation.eulerAngles == Vector3.zero)
        {
            return this.transformHelper.Scale.x * this.transformHelper.Scale.z * GetH();
        }
        return this.rotatedCubeHelper.GetVolume();
    }
}

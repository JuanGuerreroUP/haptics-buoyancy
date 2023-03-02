using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FloatingCube : AbstractFloatingObject {

    public override float GetObjVolume()
    {
        return this.transformHelper.Scale.x * this.transformHelper.Scale.y * this.transformHelper.Scale.z;
    }
    //olozada@up.edu.mx

    public override float GetDisplacedVolume()
    {
        return this.transformHelper.Scale.x* this.transformHelper.Scale.z*GetH();
    }
}

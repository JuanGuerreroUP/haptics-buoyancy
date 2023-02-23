using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FloatingCube : AbstractFloatingObject {

    public override float GetObjVolume()
    {
        return this.transformHelper.scale.x * this.transformHelper.scale.y * this.transformHelper.scale.z;
    }
    //olozada@up.edu.mx

    public override float GetDisplacedVolume()
    {
        return this.transformHelper.scale.x* this.transformHelper.scale.z*GetH();
    }
}

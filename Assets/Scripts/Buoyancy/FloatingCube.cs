using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FloatingCube : AbstractFloatingObject<BoxCollider> {

    public override float GetObjVolume()
    {
        Vector3 size = this.GetCollider().size;
        return size.x * size.y * size.z;
    }
    //olozada@up.edu.mx

    public override float GetDisplacedVolume()
    {
        Vector3 size = this.GetCollider().size;
        return size.x*size.z*GetH();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFloatingObject: MonoBehaviour {
    public abstract float CalcVolume(float waterLevel);

    public float getH(float waterLevel){
        float maxH = this.transform.localScale.y;
        float bottom = this.transform.position.y - (maxH / 2f);
        float h = waterLevel - bottom;
        if (h >= maxH) {
            return maxH;
        } else if( h <= 0) {
            return 0;
        }
        return h;
    }
}

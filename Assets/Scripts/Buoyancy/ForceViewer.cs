using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ForceViewer : MonoBehaviour
{
    private Vector3 bouyantForce;
    private Vector3 weight;
    private Vector3 dragForce;
    private Vector3 netForce;

    [SerializeField]private TMP_Text bouyantForceLbl;
    [SerializeField]private TMP_Text weigthLbl;
    [SerializeField]private TMP_Text dragForceLbl;
    [SerializeField]private TMP_Text netForceLbl;

    private const string baseText = "Bouyant Force: {0}\nWeight: {1}\nNet Force: {2}\nDrag Force: {3}\n";

    public void SetForces(Vector3 bouyantForce, Vector3 weight, Vector3 dragForce, Vector3 netForce)
    {
        this.bouyantForce = bouyantForce;
        this.weight = weight;
        this.dragForce = dragForce;
        this.netForce = netForce;

    }   

    private void Update()
    {
        bouyantForceLbl.text = "Bouyant force: " + bouyantForce.ToString();
        weigthLbl.text = "Weight: " + weight.ToString();
        dragForceLbl.text = "Drag force: " + dragForce.ToString();
        netForceLbl.text = "Net force: " + netForce.ToString();
    }
}

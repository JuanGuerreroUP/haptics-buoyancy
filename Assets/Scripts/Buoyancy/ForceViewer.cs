using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ForceViewer : MonoBehaviour
{

    private BuoyantState state = new BuoyantState();
    private string currentObject = "None";
    public AbstractFloatingObject initialWatchingObject;

    public void SetWatchingObject(AbstractFloatingObject obj){
        this.currentObject = obj.gameObject.name;
        this.state = obj.State;
    }

    

    [SerializeField]private TMP_Text forcesLabel;

    private const string baseText = 
        "Bouyant Force: {0:0000.0000}N\n"+
        "Weight: {1:0000.0000}N\n"+
        "Net Force: {2:0000.0000}N\n"+
        "Drag Force: {3:0000.0000}N\n"+
        "Displaced Volume: {4:0000.0000}u<sup>3</sup>\n"+
        "Target: {5}";

    public void SetForces(Vector3 bouyantForce, Vector3 weight, Vector3 dragForce, Vector3 netForce)
    {
        this.state.BouyantForce = bouyantForce;
        this.state.Weight = weight;
        this.state.DragForce = dragForce;
        this.state.NetForce = netForce;

    }
    private void Start(){
        if(this.initialWatchingObject != null){
            this.SetWatchingObject(this.initialWatchingObject);
        }
        
    }

    private void Update(){
        forcesLabel.text = string.Format(
            baseText,
            this.state.BouyantForce.y,
            this.state.Weight.y,
            this.state.NetForce.y,
            this.state.DragForce.y,
            this.state.DisplacedVolume*1000,
            this.currentObject
        );

        Debug.Log(string.Format(
            "Volume: ({0:0.000},{1:0.000})\t Force ({2:0.000},{3:0.000})",
            this.state.displacedVolumeLimits.GetMin(),
            this.state.displacedVolumeLimits.GetMax(),
            this.state.netForceLimits.GetMin(),
            this.state.netForceLimits.GetMax()
        ));
    }
}

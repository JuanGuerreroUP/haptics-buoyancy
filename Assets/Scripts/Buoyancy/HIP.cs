using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIP : MonoBehaviour
{
    // establish Haptic Manager and IHIP objects
    public GameObject hapticManager;
    public GameObject IHIP;

    // get haptic device information from the haptic manager
    private AbstractHM myHapticManager;

    // haptic device number
    public int hapticDevice;
    // haptic device variables
    private Vector3 position;
    private bool button0;
    private bool button1;
    private bool button2;
    private bool button3;
    public float mass;
    private float radius;
    private Material material;
    private Rigidbody rigidBody;

    [Header("Stiffness Fator")]
    // stiffness coefficient
    public float Kp = 1000; // [N/m]

    [Header("Damping Factors")]
    // damping term
    public float Kv = 20; // [N/m]
    public float Kvr = 10;
    public double Kvg = 10;

    // object in the scene that was hitted
    private bool isTouching;
    private float objectMass;
    private Vector3 HIPCollidingPosition;
    private Vector3 objectCollidingPosition;

    // Called when the script instance is being loaded
    void Awake()
    {
        position = new Vector3(0, 0, 0);
        button0 = false;
        button1 = false;
        button2 = false;
        button3 = false;
        material = IHIP.GetComponent<Renderer>().material;
        rigidBody = GetComponent<Rigidbody>();
        isTouching = false;
    }

    // Use this for initialization
    void Start()
    {
        //rigidBodies = GameObject.FindGameObjectsWithTag("Rigid Body");
        myHapticManager = GetComponent<AbstractHM>();
    }

    private void UpdateHapticDevice(){
        int hapticsFound = myHapticManager.GetHapticDevicesFound();
        hapticDevice = (hapticDevice > -1 && hapticDevice < hapticsFound) ? hapticDevice : hapticsFound - 1;
    }
    private boolean IsButtonPressed(int buttonID){
        return myHapticManager.GetButtonState(hapticDevice, buttonID);
    }
    private Vector3 GetPosition(){
        return myHapticManager.GetPosition(hapticDevice);
    }

    private void UpdateDampingFactors(){
        Kv = (Kv > 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 6)) ? 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 6) : Kv;
        Kvr = (Kvr > 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 7)) ? 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 7) : Kvr;
        Kvg = (Kvr > 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 8)) ? 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 8) : Kvg;
    }
    private void UpdateHapticDeviceMass(){
        mass = (mass > 0) ? mass : 0.0f;
        rigidBody.mass = mass;
    }

    private void UpdateRaduis(){
        radius = (IHIP.GetComponent<Renderer>().bounds.extents.magnitude) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHapticDevice();
        UpdateRaduis();
        UpdateHapticDeviceMass();
        if (isTouching) {
            IHIP.transform.position = HIPCollidingPosition;
            transform.position = position;
        }
        else {
            IHIP.transform.position = position;
            transform.position = position;
        }
        UpdateDampingFactors();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
            return;
        // HIP is touching an object
        isTouching = true;

        // calculate the collision point
        objectCollidingPosition = position + (collision.contacts[0].normal * Mathf.Abs(collision.contacts[0].separation));

        // obtain colliding object mass
        objectMass = collision.rigidbody.mass;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody == null)
            return;
        // update IHIP position according to colliding position
        if (Mathf.Abs(collision.contacts[0].separation) > radius)
        {
            HIPCollidingPosition = collision.contacts[0].point + (Mathf.Abs(collision.contacts[0].separation) * collision.contacts[0].normal);
        }
        else
        {
            HIPCollidingPosition = collision.contacts[0].point + (radius * collision.contacts[0].normal);
        }

        // uodate collision point
        objectCollidingPosition = position + (collision.contacts[0].normal * Mathf.Abs(collision.contacts[0].separation));

        // obtain colliding object mass
        objectMass = collision.rigidbody.mass;

        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.black);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isTouching = false;
    }

    public bool HipIsColliding()
    {
        return isTouching;
    }

    public Vector3 CollidingObjectPosition()
    {
        return objectCollidingPosition;
    }

    public float CollidingObjectMass()
    {
        return objectMass;
    }
}
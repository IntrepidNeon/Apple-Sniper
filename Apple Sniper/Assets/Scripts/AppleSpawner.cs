using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawner : MonoBehaviour 
{
    [SerializeField] private GameObject childObj;
    [SerializeField] private float dropInterval;
    [Space]

    private bool modeLR;
    private bool modeUpdateBool;

    private float yRot;
    private float yPos;
    private float yPosPhase;

    private Vector3 wishdir;
    private Vector3 motionVector;

    [SerializeField] private Transform forwardDirection;
    [Space]
    [SerializeField] private float spawnerHeight;
    [SerializeField] private float heightRange;
    [SerializeField] private float distRange;
    [Space]
    [SerializeField] private float speed;

    [SerializeField] private Rigidbody appleSpawner;
    // Start is called before the first frame update
    void Start()
    {
        motionVector = new Vector3 (0f, 0f, 1f);
        transform.position = new Vector3 (0f,spawnerHeight,0f);
        Invoke("dropObject", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        forwardDirection.localRotation = Quaternion.Euler(0f, yRot, 0f);
        wishdir = forwardDirection.transform.TransformDirection(motionVector);
        move();
        navigate();
    }
    void dropObject()
    {
        GameObject apple = Instantiate<GameObject>(childObj);
        apple.transform.position = transform.position;
        Invoke("dropObject", dropInterval);
    }
    private void move()
    {
        appleSpawner.velocity = wishdir * speed;
        appleSpawner.position = new Vector3(transform.position.x, yPos+spawnerHeight, transform.position.z);
    }
    private void navigate()
    {
        yPosPhase += 0.1f * appleSpawner.velocity.magnitude * Time.deltaTime;
        yPosPhase %= 2f * Mathf.PI;
        yPos = spawnerHeight + heightRange * Mathf.Sin(yPosPhase);
        if (transform.position.x > distRange || transform.position.x < -distRange || transform.position.z > distRange || transform.position.z < -distRange)
        {
            if (!modeLR)
            {
                yRot -= 0.25f;
            }
            if (modeLR)
            {
                yRot += 0.25f;
            }
            if(!modeUpdateBool)
            {
                modeUpdateBool = true;
            }
            
        }
        else
        {
            if (modeUpdateBool)
            {
                modeLR = !modeLR;
                modeUpdateBool = false;
            }
            if (!modeLR)
            {
                yRot -= 0.0125f;
            }
            if (modeLR)
            {
                yRot += 0.0125f;
            }
        }
        

    }
}

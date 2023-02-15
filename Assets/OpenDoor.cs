using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Quaternion originalRotation;
    [SerializeField] Quaternion goalRotation;
    
    float timeCount;

    public bool rotateTime;

    void Awake()
    {
        originalRotation = door.transform.rotation;
    }

    void Update()
    {
        if (rotateTime)
            door.transform.rotation = Quaternion.Slerp(originalRotation, goalRotation, timeCount);

        timeCount = timeCount + Time.deltaTime;
    }

    public void RotateTime() =>
        rotateTime = true;
}

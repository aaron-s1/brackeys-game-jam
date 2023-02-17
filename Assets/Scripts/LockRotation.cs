using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    [SerializeField] Quaternion rotation;
    
    void FixedUpdate() =>
        gameObject.transform.eulerAngles = new Vector3(rotation.x, rotation.y, rotation.z);
}

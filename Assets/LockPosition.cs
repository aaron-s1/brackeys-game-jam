using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    [SerializeField] Vector3 position;

    void FixedUpdate() =>
        transform.localPosition = position;
}

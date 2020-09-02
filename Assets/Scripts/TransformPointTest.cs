using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPointTest : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToMove;

    [SerializeField]
    private Vector3 offset;

    private void Update()
    {
        objectToMove.transform.position = transform.TransformPoint(offset);
    }
}

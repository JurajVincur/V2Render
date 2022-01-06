using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    void Update()
    {
        float ammount = Time.deltaTime * 50;
        transform.Rotate(new Vector3(ammount, ammount, ammount));
    }
}

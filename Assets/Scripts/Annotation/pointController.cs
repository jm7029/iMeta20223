using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointController : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    public Vector3 ReturnPosition()
    {
        Vector3 Position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        return Position;

    }
    // Update is called once per frame
}

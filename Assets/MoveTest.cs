using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    private Transform tf;
    private Vector3 input;
    public float speed = 0.01f;
    // tart is called before the first frame
    // update
    void Start()
    {

        tf = GetComponent<Transform>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


        transform.position += speed * input; 


                
    }
}

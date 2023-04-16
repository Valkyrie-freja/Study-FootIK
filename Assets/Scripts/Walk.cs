using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rigidbody;
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void Move(float x, float z)
    {
        if (x == 0 && z == 0)
        {
            animator.SetBool("isWalking", false);
            rigidbody.velocity = Vector3.zero;
            return;
        }
        
        // move
        const float speed = 5.0f;
        float move_length = Mathf.Sqrt(x * x + z * z);
        float length_inv = 1f / move_length;
        animator.SetBool("isWalking", true);
        rigidbody.velocity = new Vector3(x*length_inv, 0, z*length_inv) * speed;
        
        // rotate
        Vector3 targetDirection = new Vector3(x, 0, z);
        if(targetDirection.magnitude > 0.1)
        {
            const float smooth = 10.0f;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);
        }
    }
}

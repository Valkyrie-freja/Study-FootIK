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

    void Move(float x_input, float z_input)
    {
        if (x_input == 0 && z_input == 0)
        {
            animator.SetBool("isWalking", false);
            rigidbody.velocity = Vector3.zero;
            return;
        }
        
        // move
        const float speed = 5.0f;
        float move_length = Mathf.Sqrt(x_input * x_input + z_input * z_input);
        float length_inv = 1f / move_length;
        animator.SetBool("isWalking", true);

        Vector3 forceInput = new Vector3(x_input, 0, z_input);
        // 粘性抵抗
        Vector3 resistViscosity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, rigidbody.velocity.z);
        // 慣性抵抗
        Vector3 resistInertia = new Vector3(
            rigidbody.velocity.x * rigidbody.velocity.x,
            rigidbody.velocity.y * rigidbody.velocity.y,
            rigidbody.velocity.z * rigidbody.velocity.z);
        Vector3 forceEffective = forceInput - resistViscosity - resistInertia;
        rigidbody.AddForce(forceEffective);
        
        // rotate
        Vector3 targetDirection = new Vector3(x_input, 0, z_input);
        if(targetDirection.magnitude > 0.1)
        {
            const float smooth = 10.0f;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);
        }
    }
}

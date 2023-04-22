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
        const float x_max = 25.0f;
        const float z_max = 25.0f;
        if (transform.position.x > x_max)
        {
            Transform current_transform = this.transform;
            Vector3 new_position = current_transform.position;
            new_position.x = 0;
            current_transform.position = new_position;
        }

        if (transform.position.z > z_max)
        {
            Transform current_transform = this.transform;
            Vector3 new_position = current_transform.position;
            new_position.z = 0;
            current_transform.position = new_position;
        }
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
        animator.SetBool("isWalking", true);
        Vector3 velocityInput = new Vector3(x_input, 0, z_input);
        float move_length_inv = 1f / velocityInput.magnitude;
        Vector3 velocityInput_norm = velocityInput * move_length_inv;
        const float speed = 5.0f;
        Vector3 velocityEffective = velocityInput_norm * speed;
        rigidbody.velocity = velocityEffective;
        
        // rotate
        Vector3 targetDirection = velocityInput;
        if(targetDirection.magnitude > 0.1)
        {
            const float smooth = 10.0f;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);
        }
    }
}

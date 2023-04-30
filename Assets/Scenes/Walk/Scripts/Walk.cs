using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Walk : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rigidbody;

    private GameObject mainCamera;

    private VectorQuaternion vector_controller;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main.gameObject;
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
        Vector3 input_vector = new Vector3(x_input, 0, z_input);
        Vector3 player_look_vector = vector_controller.ToObjectStandardVector(mainCamera, input_vector);
        
        const float speed = 5.0f;
        Vector3 velocityEffective = player_look_vector * speed;
        rigidbody.velocity = velocityEffective;
        
        // rotate
        Vector3 inputDirection = input_vector;
        if(inputDirection.magnitude > 0.1)
        {
            const float smooth = 10.0f;
            Quaternion currentRotation = transform.rotation;
            Quaternion nextRotation = Quaternion.LookRotation(inputDirection);
            currentRotation = Quaternion.Lerp(currentRotation, nextRotation, Time.deltaTime * smooth);
        }
    }
}

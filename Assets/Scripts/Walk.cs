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
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main.gameObject;
        
        // test
        for(float x = -1.0f; x <= 1.0f ; x += 0.1f)
        {
            for(float z = -1.0f; z <= 1.0f; z += 0.1f)
            {
                Vector3 input_vector3 = new Vector3(x, 0, z);
                Vector3 camera_standard_vector = ToCameraStandardVector(input_vector3);
                Debug.Log($"{input_vector3}, {camera_standard_vector}");
            }
        }
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

    private Vector3 ToCameraStandardVector(Vector3 _input_vector)
    {
        Quaternion input_rotate = Quaternion.FromToRotation(Vector3.forward/*(0, 0, 1)*/, _input_vector);
        Vector3 camera_look_vector = mainCamera.transform.forward;
        // use only x and z
        camera_look_vector.y = 0.0f;
        return input_rotate * camera_look_vector.normalized;
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
        Vector3 player_look_vector = ToCameraStandardVector(input_vector);
        
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

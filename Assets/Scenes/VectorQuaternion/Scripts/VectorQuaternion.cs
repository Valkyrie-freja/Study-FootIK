using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorQuaternion : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject arrow_camera;
    [SerializeField] private GameObject arrow_input;
    [SerializeField] private GameObject arrow_player_look;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 controller_input_vector = new Vector3(
            Input.GetAxis("Horizontal"),
            0.0f,
            Input.GetAxis("Vertical"));
        Vector3 player_look_vector = 
            ToCameraStandardVector(controller_input_vector);
    }

    private void SetArrowStatus(GameObject _arrow, float _scale, Quaternion _rotate)
    {
        _arrow.transform.localScale = _scale * Vector3.one;
        _arrow.transform.rotation = _rotate;
    }
    
    private void SetArrowStatus(GameObject _arrow, float _scale, Vector3 _vector3)
    {
        _arrow.transform.localScale = _scale * Vector3.one;
        Quaternion _rotate = Quaternion.Euler(_vector3);
        _arrow.transform.rotation = _rotate;
    }
    
    private Vector3 ToCameraStandardVector(Vector3 _input_vector)
    {
        Quaternion input_rotate = 
            Quaternion.FromToRotation(Vector3.forward, _input_vector);
        // when (0, 0, 0) -> (0, 0, -1), not to be rotate = 0
        if (_input_vector.x == 0 && _input_vector.z < 0) {
            input_rotate = Quaternion.AngleAxis(180, Vector3.up);
        }
        SetArrowStatus(arrow_input, _input_vector.magnitude, input_rotate);

        Vector3 camera_look_vector = camera.transform.forward;
        // use only x and z
        camera_look_vector.y = 0.0f;
        SetArrowStatus(arrow_camera, 1.0f, camera_look_vector);
        
        return input_rotate * camera_look_vector.normalized;
    }
}

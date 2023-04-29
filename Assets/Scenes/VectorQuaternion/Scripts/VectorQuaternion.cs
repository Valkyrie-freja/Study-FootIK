using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class VectorQuaternion : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject arrow_camera;
    [SerializeField] private GameObject arrow_input;
    [SerializeField] private GameObject arrow_player_look;

    void Update()
    {
        Vector3 controller_input_L_vector = new Vector3(Input.GetAxis("Horizontal"),   0.0f, Input.GetAxis("Vertical"));
        Vector3 controller_input_R_vector = new Vector3(Input.GetAxis("Horizontal_R"), 0.0f, Input.GetAxis("Vertical_R"));
        Vector3 player_look_vector = ToCameraStandardVector(controller_input_L_vector);
        SetArrowStatus(arrow_player_look, player_look_vector);
        camera.transform.rotation = ToQuaternion(controller_input_R_vector);
    }

    private void SetArrowStatus(GameObject _arrow, float _scale, Quaternion _rotate)
    {
        _arrow.transform.localScale = _scale * Vector3.one;
        _arrow.transform.rotation = _rotate;
    }
    
    private void SetArrowStatus(GameObject _arrow, Vector3 _vector3)
    {
        _arrow.transform.localScale = _vector3.magnitude * Vector3.one;
        Quaternion rotate = ToQuaternion(_vector3);
        _arrow.transform.rotation = rotate;
    }

    // 世界座標の正面に対する _vector の回転を返す
    private Quaternion ToQuaternion(Vector3 _vector)
    {
        Quaternion rotate = 
            Quaternion.FromToRotation(Vector3.forward, _vector);
        // when (0, 0, 0) -> (0, 0, -1), not to be rotate = 0
        if (_vector.x == 0 && _vector.z < 0) {
            rotate = Quaternion.AngleAxis(180, Vector3.up);
        }

        return rotate;
    }
    
    private Vector3 ToCameraStandardVector(Vector3 _input_vector)
    {
        Quaternion input_rotate = ToQuaternion(_input_vector);
        SetArrowStatus(arrow_input, _input_vector.magnitude, input_rotate);

        Vector3 camera_look_vector = camera.transform.forward;
        // use only x and z
        camera_look_vector.y = 0.0f;
        SetArrowStatus(arrow_camera, camera_look_vector);
        
        return _input_vector.magnitude * (input_rotate * camera_look_vector);
    }
}

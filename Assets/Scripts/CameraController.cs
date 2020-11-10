using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private float sensitivity = 6.0f;


	void Update ()
    {
        transform.position += transform.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime * sensitivity;
        Vector3 direction = transform.forward;
        direction.y = 0.0f;
        direction *= 2.0f;
        transform.position += direction * Input.GetAxisRaw("Vertical") * Time.deltaTime * sensitivity;
    }
}

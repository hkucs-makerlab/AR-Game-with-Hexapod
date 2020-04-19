using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayer : MonoBehaviour
{
    public Joystick joystick;
    public float speed;

    private void Awake() {
        joystick = GameManager.Instance.joystick;
    }

    private void Update()
    {
        //Vector3 direction = transform.GetChild(0).GetChild(0).forward * joystick.Vertical + transform.GetChild(0).GetChild(0).right * joystick.Horizontal;
        //transform.position += direction.normalized * speed;
    }
}

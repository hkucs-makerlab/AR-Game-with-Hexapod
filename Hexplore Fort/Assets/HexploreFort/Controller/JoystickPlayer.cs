using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayer : MonoBehaviour
{
    MovementJoystick joystick;
    //private float walkingSpeed = 0.01f;
    //private float rotationSpeed = 15;
    private float walkingSpeed = 0.05f;
    private float rotationSpeed = 30;

    private void Start() {
        joystick = GameManager.Instance.movementJoystick;

        StartCoroutine(SyncWithHexapod());
    }

    private IEnumerator SyncWithHexapod() {
        while (true) {
            yield return new WaitForSeconds(0.2f);

            if (joystick.walking || joystick.running) {
                float multiplier = joystick.walking ? 1f : 2f;
                if (joystick.Horizontal != 0 || joystick.Vertical != 0) {
                    if (Mathf.Abs(joystick.Horizontal) > Mathf.Abs(joystick.Vertical)) {
                        transform.Rotate(new Vector3(0, joystick.Horizontal * rotationSpeed * multiplier, 0));
                    } else {
                        Vector3 direction = joystick.Vertical * transform.forward;
                        transform.localPosition += direction.normalized * walkingSpeed * multiplier;
                    }
                }
            }
        }
    }
}

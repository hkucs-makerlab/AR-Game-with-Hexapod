using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour {
    [SerializeField]
    private bool front;
    public List<GameObject> detectedWall;

    private void Start() {
        detectedWall = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Wall") {
            detectedWall.Add(other.gameObject);
            GameManager.Instance.movementJoystick.DetectedWall(front, true);
        } else if (other.gameObject.tag == "Door") {
            Door door = other.gameObject.GetComponent<Door>();
            if (GameManager.Instance.player.OpenDoor(door.type)) {
                StartCoroutine(door.Open());
            } else {
                detectedWall.Add(other.gameObject);
                GameManager.Instance.movementJoystick.DetectedWall(front, true);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Door") {
            if (detectedWall.Contains(other.gameObject)) {
                detectedWall.Remove(other.gameObject);
            }

            if (detectedWall.Count == 0) {
                GameManager.Instance.movementJoystick.DetectedWall(front, false);
            }
        }
    }
}

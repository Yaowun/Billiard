using UnityEngine;

public class PlayerConctroller : MonoBehaviour
{
    Camera cam;
    GameObject cueStick;
    GameObject cueBall;
    public float rotateSensitive;
    public float zoomSensitive;
    public float strokeSensitive;
    public float distanceOfCamera;
    public float distanceOfCueStick;


    void Start()
    {
        cam = Camera.main;
        cueStick = GameObject.Find("CueStick");
        cueBall = GameObject.Find("CueBall");
        // Reset(cueStick.GetComponent<Transform>(), cueBall.GetComponent<Transform>(), new Vector3(0f, 90f, 0f), distanceOfCueStick);
        // Reset(cam.GetComponent<Transform>(), cueBall.GetComponent<Transform>(), new Vector3(5f, 90f, 0f), distanceOfCamera);
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
    }

    void Reset(Transform gameObject, Transform target, Vector3 direction, float distance)
    {
        gameObject.localEulerAngles = direction;
        gameObject.position = target.position - gameObject.forward * distance;
    }
}

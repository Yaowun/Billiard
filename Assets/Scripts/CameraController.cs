using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;

    [SerializeField] float minZoomDistance;
    [SerializeField] float maxZoomDistance;

    [SerializeField] float resetDistance;

    BallController ballController;
    CueStickController cueStickController;

    float rotateSensitive;
    float zoomSensitive;

    float rotationX;
    float rotationY;

    float mouseX;
    float mouseY;
    float mouseCenter;

    float currentDistance;
    Vector3 direction;

    public bool resetted { get; private set; }
    bool hittedBall;

    void Start()
    {
        ballController = GameObject.Find("BallController").GetComponent<BallController>();
        cueStickController = GameObject.Find("CueStick").GetComponent<CueStickController>();

        rotateSensitive = GameObject.Find("PlayerController").GetComponent<PlayerConctroller>().rotateSensitive;
        zoomSensitive = GameObject.Find("PlayerController").GetComponent<PlayerConctroller>().zoomSensitive;

        rotationX = transform.localEulerAngles.x;
        rotationY = transform.localEulerAngles.y;

        resetted = true;
    }

    void Update()
    {
        hittedBall = cueStickController.hittedBall;

        if (!hittedBall && resetted)
        {
            Rotate();
            Zoom();
        }

        else if (!ballController.IsAllBallStop())
        {
            Track();
        }

        else
        {
            resetted = false;
        }

        if (!resetted)
        {
            Reset();
        }
    }

    void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            currentDistance = (target.position - transform.position).magnitude;
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            rotationY += mouseX * rotateSensitive;
            rotationX += -mouseY * rotateSensitive;
            rotationX = Mathf.Clamp(rotationX, minPitch, maxPitch);

            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
            transform.position = target.position - transform.forward * currentDistance;
        }
    }

    void Zoom()
    {
        mouseCenter = Input.GetAxis("Mouse ScrollWheel");
        if (mouseCenter > 0 || mouseCenter < 0)
        {
            currentDistance = (target.position - transform.position).magnitude;
            direction = (target.position - transform.position).normalized;

            if ((currentDistance <= minZoomDistance && mouseCenter > 0) || (currentDistance >= maxZoomDistance && mouseCenter < 0))
            {
                return;
            }

            transform.position += direction * mouseCenter * zoomSensitive;
        }
    }

    void Track()
    {
        transform.LookAt(target);
    }

    void Reset()
    {
        // transform.position = target.position - transform.forward * resetDistance;
        if (Mathf.Abs((transform.position - target.position).magnitude - resetDistance) < 0.001f)
        {
            Debug.Log("resetted");
            rotationY = transform.localEulerAngles.y;
            rotationX = transform.localEulerAngles.x;
            resetted = true;
            return;
        }
        float step = 3f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position - transform.forward * resetDistance, step);
    }
}

using System;
using UnityEngine;

public class CueStickController : MonoBehaviour
{
    [SerializeField] Transform ball;
    [SerializeField] float pitch;
    [SerializeField] float resetDistance;
    [SerializeField] float maxCueStickVelocity;

    CameraController cam;
    BallController ballController;

    Rigidbody rb;
    Rigidbody rbCueBall;

    float previousMouseYPosition;
    float currentMouseYPosition;

    float rotateSensitive;
    float strokeSensitive;
    float rotationY;
    float mouseX;

    float currentDistance;
    float speedThreshold;

    public bool hittedBall { get; private set; }
    bool hittingBall;
    bool resetted;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();
        ballController = GameObject.Find("BallController").GetComponent<BallController>();

        rb = GetComponent<Rigidbody>();
        rbCueBall = GameObject.Find("CueBall").GetComponent<Rigidbody>();

        rotateSensitive = GameObject.Find("PlayerController").GetComponent<PlayerConctroller>().rotateSensitive;
        strokeSensitive = GameObject.Find("PlayerController").GetComponent<PlayerConctroller>().strokeSensitive;

        rotationY = transform.localEulerAngles.y;
        speedThreshold = ballController.speedThreshold;

        hittedBall = false;
        hittingBall = false;
    }

    void Update()
    {
        resetted = cam.resetted;

        if (!hittedBall && resetted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                previousMouseYPosition = Input.mousePosition.y;
                hittingBall = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                hittingBall = false;
            }

            Rotate();
        }

        else if (ballController.IsAllBallStop())
        {
            NewHit();
        }
    }

    void FixedUpdate()
    {
        if (hittingBall)
        {
            HitBall();
        }
        else
        {
            rb.velocity = new Vector3();
        }
    }

    void HitBall()
    {
        if (Input.GetMouseButton(0))
        {
            currentMouseYPosition = Input.mousePosition.y;

            float direction = currentMouseYPosition > previousMouseYPosition ? 1 : -1;
            float speed = Mathf.Abs(currentMouseYPosition - previousMouseYPosition) * strokeSensitive;
            if (speed != 0)
            {
                speed = Mathf.Pow(speed, 0.5f) + 0.6f;
            }
            Vector3 velocity = transform.forward * direction * speed;
            velocity = Vector3.ClampMagnitude(velocity, maxCueStickVelocity);
            rb.velocity = velocity;

            // float speed = (currentMouseYPosition - previousMouseYPosition) * strokeSensitive;
            // Vector3 targetVelocity = transform.forward * speed;
            // Vector3 velocityChange = targetVelocity - rb.velocity;
            // Vector3 acceleration = velocityChange / Time.fixedDeltaTime;
            // acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);
            // rb.AddForce(acceleration, ForceMode.Acceleration);

            previousMouseYPosition = currentMouseYPosition;
        }

        else
        {
            rb.velocity = new Vector3();
        }
    }

    void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            currentDistance = (ball.position - transform.position).magnitude;
            mouseX = Input.GetAxis("Mouse X");

            rotationY += mouseX * rotateSensitive;

            transform.localEulerAngles = new Vector3(pitch, rotationY, 0f);
            transform.position = ball.position - transform.forward * currentDistance;
        }
    }

    void NewHit()
    {
        Debug.Log("NewHit");
        hittedBall = false;
        GetComponent<Collider>().enabled = true;
        Reset();
    }

    void Reset()
    {
        transform.localEulerAngles = new Vector3(pitch, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z);
        transform.position = ball.position - transform.forward * resetDistance;
        rotationY = transform.localEulerAngles.y;
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.name == "CueBall" && (rb.velocity.magnitude > speedThreshold || rbCueBall.velocity.magnitude > speedThreshold))
        {
            Debug.Log("hittedBall");
            hittedBall = true;
            hittingBall = false;
            GetComponent<Collider>().enabled = false;
        }
    }
}

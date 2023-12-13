using UnityEngine;

public class BallController : MonoBehaviour
{
    public CueBall cueBall;
    public NumberBall[] numberBalls;
    public float speedThreshold {get; private set;} = 0.001f;

    void Start()
    {
        cueBall = GameObject.Find("CueBall").GetComponent<CueBall>();
        numberBalls = new NumberBall[15];
        for (int i = 0; i < numberBalls.Length; i++)
        {
            numberBalls[i] = GameObject.Find("NumberBall/Ball" + (i + 1)).GetComponent<NumberBall>();
            numberBalls[i].ballNumber = i + 1;
        }

        InitializeBallPosition();
    }

    void InitializeBallPosition()
    {
        cueBall.InitializeCueBallPosition();
        for (int i = 0; i < numberBalls.Length; i++)
        {
            numberBalls[i].InitializeNumberBallPosition();
        }
    }

    public bool IsAllBallStop()
    {
        if (cueBall.isActiveAndEnabled && cueBall.GetComponent<Rigidbody>().velocity.magnitude > speedThreshold)
        {
            return false;
        }
        foreach (NumberBall ball in numberBalls)
        {
            if (ball.isActiveAndEnabled && ball.GetComponent<Rigidbody>().velocity.magnitude > speedThreshold)
            {
                return false;
            }
        }
        return true;
    }
}

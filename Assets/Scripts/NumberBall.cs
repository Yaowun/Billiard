using UnityEngine;
using System.Collections.Generic;

public class NumberBall : MonoBehaviour
{
    public int ballNumber { get; set; }

    private static readonly Dictionary<int, Vector3> offsetCounts = new Dictionary<int, Vector3>
    {
        // Level 1
        { 9, new Vector3(-2f, 0f, 0f) },
        // Level 2
        { 7, new Vector3(-1f, 0f, -1f) },
        { 12, new Vector3(-1f, 0f, 1f) },
        // Level 3
        { 15, new Vector3(0f, 0f, -2f) },
        { 8, new Vector3(0f, 0f, 0f) },
        { 1, new Vector3(0f, 0f, 2f) },
        // Level 4
        { 6, new Vector3(1f, 0f, -3f) },
        { 10, new Vector3(1f, 0f, -1f) },
        { 3, new Vector3(1f, 0f, 1f) },
        { 14, new Vector3(1f, 0f, 3f) },
        // Level 5
        { 11, new Vector3(2f, 0f, -4f) },
        { 2, new Vector3(2f, 0f, -2f) },
        { 13, new Vector3(2f, 0f, 0f) },
        { 4, new Vector3(2f, 0f, 2f) },
        { 5, new Vector3(2f, 0f, 4f) },
    };

    public void InitializeNumberBallPosition()
    {
        float diameter = 0.0285f;
        if (offsetCounts.ContainsKey(ballNumber))
        {
            Vector3 currentOffsetCount = offsetCounts[ballNumber];
            transform.position = new Vector3(0.6f + currentOffsetCount.x * diameter * Mathf.Sqrt(3f), 0.7915657f, currentOffsetCount.z * diameter);
        }
        else
        {
            Debug.LogError("Position for ballNumber " + ballNumber + " is not defined.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "ColliderPot")
        {
            gameObject.SetActive(false);
        }
    }
}

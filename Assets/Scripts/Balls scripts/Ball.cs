using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Creates : Pool ball
/// </summary>
public class Ball : MonoBehaviour
{ 
    Rigidbody2D ballRB2D;
    int ballNumber = 0;
    string ballType = "empty";
    float ballDecelerationIndex;
    GameObject ballText;

    // Get number of ball
    public int BallNumber { get { return ballNumber; } }
    // Get type of ball
    public string BallType { get { return ballType; } }

    // Create ball with number and type
    public void BallSetup(string ballTypeSetup, int ballNumberSetup)
    {
        ballType = ballTypeSetup;
        ballNumber = ballNumberSetup;
        if (ballTypeSetup != "ballWhite")
        {
            ballText = Instantiate(Resources.Load<GameObject>("Ball/BallText"), transform.position + new Vector3(0, 0, -0.2f), transform.rotation);
            ballText.transform.SetParent(transform);
            ballText.GetComponent<TextMeshPro>().text = ballNumberSetup.ToString();
        }
    }

    virtual protected void Awake()
    {
        ballNumber = 0;
        ballDecelerationIndex = 0.9975f;
        ballRB2D = GetComponent<Rigidbody2D>();
        //add shadows to ball
        GameObject ballUpperShadow = Instantiate(Resources.Load<GameObject>("Ball/BallUpperShadow"), transform.position + new Vector3(0, 0, -0.1f), transform.rotation);
        ballUpperShadow.transform.parent = transform;
        GameObject ballLowerShadow = Instantiate(Resources.Load<GameObject>("Ball/BallLowerShadow"), transform.position + new Vector3(0, 0, 0.1f), transform.rotation);
        ballLowerShadow.transform.parent = transform;
    }

    virtual protected void Update()
    {
        ballRB2D.velocity *= ballDecelerationIndex;
        ballRB2D.angularVelocity *= ballDecelerationIndex;
        if (ballRB2D.velocity.magnitude <= 0.01) ballRB2D.velocity *= 0;
        if (ballRB2D.angularVelocity <= 0.01) ballRB2D.angularVelocity *= 0;
    }
}

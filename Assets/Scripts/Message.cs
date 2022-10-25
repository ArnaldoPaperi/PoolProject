using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    List<GameObject> ballSolidDestroyList;
    List<GameObject> ballStripeDestroyList;
    int ballSolidNumber;
    int ballStripeNumber;

    void Start()
    {
        ballSolidDestroyList = new();
        ballStripeDestroyList = new();
        // add listener for points added event
        EventManager.AddBallDestroyListener(BallDestroy);
        EventManager.AddCheckBallNumberListener(CheckBallNumber);
    }

    public void CheckBallNumber(int ballSolidNumber, int ballStripeNumber)
    {
        this.ballSolidNumber = ballSolidNumber;
        this.ballStripeNumber = ballStripeNumber;
    }

    public void BallDestroy(GameObject ballDestroy)
    {
        Ball ball = ballDestroy.GetComponent<Ball>();
        switch (ball.BallType)
        {
            case "solid":
                ballSolidNumber--;
                ballSolidDestroyList.Add(ballDestroy);
                if (ballSolidDestroyList.Count == 1) ballSolidDestroyList[0].transform.SetPositionAndRotation(new Vector3(-17, 9.5f, -0.5f), Quaternion.identity);
                else ballSolidDestroyList[^1].transform.SetPositionAndRotation(ballSolidDestroyList[^2].transform.position + new Vector3(0.6f, 0, 0), Quaternion.identity);
                break;
            case "stripe":
                ballStripeNumber--;
                ballStripeDestroyList.Add(ballDestroy);
                if (ballStripeDestroyList.Count == 1) ballStripeDestroyList[0].transform.SetPositionAndRotation(new Vector3(-17, 8.9f, -0.5f), Quaternion.identity);
                else ballStripeDestroyList[^1].transform.SetPositionAndRotation(ballStripeDestroyList[^2].transform.position + new Vector3(0.6f, 0, 0), Quaternion.identity);
                break;
            case "ball8":
                if (ballSolidNumber == 0) print("Congratulation solid");
                else if (ballStripeNumber == 0) print("Congratulation stripe");
                else print("8 ball lost, game over");
                Time.timeScale = 0;
                break;
            case "ballWhite":
                ballDestroy.transform.position = new Vector2(30, 30);
                ballDestroy.SetActive(false);
                print("lost white ball");
                break;

        }
    }
}

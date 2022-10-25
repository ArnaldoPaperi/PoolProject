using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallsSpawner : MonoBehaviour
{
    //Number of total balls (WITHOUT 8 BALL)
    [SerializeField]
    int ballTotalNumber;
    int ballTotalSolidNumber;
    int ballTotalStripeNumber;
    GameObject ballWhite;
    CheckBallNumberEvent checkBallNumberEvent;

    void Start()
    {
        checkBallNumberEvent = new();

        // Get radius and diameter of ball
        GameObject ballTemporary = Instantiate(Resources.Load<GameObject>("Ball/Ball"));
        CircleCollider2D tempBallCircelCollider2D = ballTemporary.GetComponent<CircleCollider2D>();
        float ballRadius = tempBallCircelCollider2D.radius;
        float ballDiameter = ballRadius * 2;
        Destroy(ballTemporary);

        // Number of total solid balls
        ballTotalSolidNumber = ballTotalNumber / 2;;
        // Number of total sprite balls (if total ball is odd then there will be 1 more stripe ball)
        if (ballTotalNumber != 1)
        {
            if (ballTotalNumber % 2 == 0)
            {
                ballTotalStripeNumber = ballTotalSolidNumber;
            }
            else
            {
                ballTotalStripeNumber = ballTotalSolidNumber + 1;
            }
        }
        else
        {
            ballTotalStripeNumber = 0;
        }

        // Calculate where 8 ball must stay
        int ball8TotalNumber = ballTotalNumber;
        int ballCollumNumber = 1;
        int ball8Index = 0;
        int ball8Collum = 0;
        while (ball8TotalNumber > 0)
        {
            ball8TotalNumber -= ballCollumNumber + 1;
            ballCollumNumber++;
        }
        while (ball8Collum < ballCollumNumber / 2)
        {
            ball8Index += ball8Collum + 1;
            ball8Collum++;
        }
        ball8Index += (ball8Collum + 1) / 2;

        // List of all balls
        List<GameObject> listBalls = new();
        List<GameObject> listBallsSolid = new();
        List<GameObject> listBallsStripe = new();
        // Calculate how many balls are in there in group of 7 balls (it will first create 7 solid ball, then 7 stripe and then repeat)
        int ballSolidNumberCount = ballTotalSolidNumber / 7;
        int solidCount = 0;
        int ballStripeNumberCount = ballTotalStripeNumber / 7;
        int stripeCount = 0;
        if (ballSolidNumberCount * 7 != ballTotalSolidNumber)
        {
            ballSolidNumberCount++;
        }
        if (ballStripeNumberCount * 7 != ballTotalStripeNumber)
        {
            ballStripeNumberCount++;
        }

        if (ballTotalNumber > 1)
        {
            // Populate solid list
            for (int i = 0; i < ballSolidNumberCount; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    solidCount++;
                    if (solidCount <= ballTotalSolidNumber)
                    {
                        if (solidCount != 1)
                        {
                            listBallsSolid.Add(CreateBall("solid", j, i));
                        }
                    }
                }
            }
            // Populate stripe list
            for (int i = 0; i < ballStripeNumberCount; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    stripeCount++;
                    if (stripeCount <= ballTotalStripeNumber)
                    {
                        listBallsStripe.Add(CreateBall("stripe", j, i));
                    }
                }
            }
            // Fuse list
            listBalls.AddRange(listBallsSolid);
            listBalls.AddRange(listBallsStripe);
            // Shuffle list
            for (int i = 0; i < listBalls.Count; i++)
            {
                GameObject ballCurrentIndex = listBalls[i];
                int randomIndex = Random.Range(i, listBalls.Count);
                listBalls[i] = listBalls[randomIndex];
                listBalls[randomIndex] = ballCurrentIndex;
            }
            // Ball1 must stay at closest corner
            listBalls.Insert(0, CreateBall("solid", 0, 0));
            // Ball8 must stay at center of grid
            listBalls.Insert(ball8Index, CreateBall("ball8", 0, 0));
        }
        // Various condition to make sure the grid doesn't broke
        else if (ballTotalNumber == 1)
        {
            listBalls.Insert(0, CreateBall("solid", 0, 0));
            listBalls.Insert(1, CreateBall("ball8", 0, 0));
        }
        else
        {
            listBalls.Insert(ball8Index, CreateBall("ball8", 0, 0));
        }

        // Create triangle shape grid
        Vector2 topBallPosition = new(Mathf.Sqrt(Mathf.Pow(ballDiameter, 2) - Mathf.Pow(ballRadius, 2)), ballRadius);
        Vector2 lowerBallPosition = new(0, ballDiameter);
        int ballIndex = 1;
        int ballColl = 1;

        // Positionate first ball
        listBalls[0].transform.position = new(4, 0);

        // Creates rest of balls with random placement
        while (ballIndex < ballTotalNumber + 1)
        {
            listBalls[ballIndex].transform.position = listBalls[ballIndex - ballColl].transform.position + (Vector3)topBallPosition;
            ballIndex++;
            for (int i = 0; i < ballColl; i++)
            {
                // Creates collum of balls
                if (ballIndex < ballTotalNumber + 1)
                {
                    listBalls[ballIndex].transform.position = listBalls[ballIndex - 1].transform.position - (Vector3)lowerBallPosition;
                    ballIndex++;
                }
            }
            ballColl++;
        }
        // Creates white ball
        ballWhite = CreateBall("ballWhite", 0, 0);

        EventManager.AddCheckBallNumberInvoker(this);
        checkBallNumberEvent.Invoke(ballTotalSolidNumber, ballTotalStripeNumber);
    }

    // Update is called once per frame
    private void Update()
    {
        // Create new white ball (only if last one got destroyed)
        if (Input.GetKeyDown(KeyCode.Space) && !ballWhite.GetComponent<Renderer>().isVisible)
        {
            ballWhite.SetActive(true);
            ballWhite.transform.position = new Vector2(-6.5f, 0);
            print("New white ball");
        }
        if (Input.GetKey(KeyCode.T))
        {
            Time.timeScale = 2;
        }
        else if (Input.GetKey(KeyCode.R))
        {
            Time.timeScale = 1;
        }
    }

    // Create ball with type and number
    private GameObject CreateBall(string ballType, int ballNumber, int ballIndex1)
    {
        GameObject ball = Instantiate(Resources.Load<GameObject>("Ball/Ball"));
        switch (ballType)
        {
            case "solid":
                if (ballIndex1 == 0)
                {
                    ball.AddComponent<Ball>().BallSetup(ballType, ballNumber + 1);
                }
                else
                {
                    ball.AddComponent<Ball>().BallSetup(ballType, ballNumber + 2 + ballIndex1 * 14);
                }
                ball.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSolidSprites/BallSolid" + SpriteNumberConverter(ballNumber));
                break;
            case "stripe":
                if (ballIndex1 == 0)
                {
                    ball.AddComponent<Ball>().BallSetup(ballType, ballNumber + 9);
                }
                else
                {
                    ball.AddComponent<Ball>().BallSetup(ballType, ballNumber + 9 + ballIndex1 * 14);
                }
                ball.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallStripeSprites/BallStripe" + SpriteNumberConverter(ballNumber));
                break;
            case "ball8":
                ball.AddComponent<Ball>().BallSetup(ballType, 8);
                ball.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Ball8");
                break;
            case "ballWhite":
                ball.AddComponent<Ball>().BallSetup(ballType, 0);
                ball.AddComponent<BallHitter>();
                ball.tag = "BallWhite";
                ball.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallWhite");
                ball.transform.position = new Vector2(-6.5f, 0);
                break;

        }
        return ball;
    }

    //    // make sure that when the number arrives at cap (7 in this case) it return back to 0
    //    // (7 = 0, 8 = 1, 9 = 2 ....... 13 = 7, 14 = 0, 15 = 1 etc. etc.)
    private int SpriteNumberConverter(int number)
    {
        int num1 = number / 7;
        int num2 = num1 * 7;
        int num3 = number - num2;
        return num3;
    }

    public void AddCheckBallNumberListener(UnityAction<int, int> listener) { checkBallNumberEvent.AddListener(listener); }
}

using UnityEngine;

/// <summary>
/// Creates various indicator and gives you the possibility to hit the ball
/// </summary>
public class BallHitter : MonoBehaviour
{
    Rigidbody2D ballRigidbody2D;
    GameObject cue;
    GameObject indicator1;
    GameObject indicator2;
    GameObject indicator3;
    GameObject ballIndicator;

    void Start()
    {
        // Gets ball component
        ballRigidbody2D = GetComponent<Rigidbody2D>();
        // Create arrow indicator
        cue = Instantiate(Resources.Load<GameObject>("Cue"));
        // Create line renderer indicator
        indicator1 = Instantiate(Resources.Load<GameObject>("Ball/BallWhite/BallIndicator1"));
        indicator2 = Instantiate(Resources.Load<GameObject>("Ball/BallWhite/BallIndicator2"));
        indicator3 = Instantiate(Resources.Load<GameObject>("Ball/BallWhite/BallIndicator3"));
        ballIndicator = Instantiate(Resources.Load<GameObject>("Ball/BallWhite/BallRayCast"));

    }

    void Update()
    {
        // Calculate direction to hit ball (based on mouse position)
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float ballAngle = AngleCalculator(transform.position, mousePosition);
        Vector3 ballDirection = DirectionCalculator(ballAngle);
        // Calculate force to hit ball (based on mouse distance)
        float force = Vector3.Distance(transform.position, mousePosition) - 10;
        float forceClamped = Mathf.Clamp(force, 0, 2);
        // Check if all balls are still
        bool ballsSteady = false;
        GameObject[] ballsList = GameObject.FindGameObjectsWithTag("Ball");
        for (int i = 0; i < ballsList.Length; i++)
        {
            if (ballsList[i].GetComponent<Rigidbody2D>().IsSleeping() && ballRigidbody2D.IsSleeping()) ballsSteady = true;
            else break;
        }
        // Ball can be hit only if: All balls aren't moving,n seconds have passed from last hit and mouse is within range. (arrow is show only when everything is true)
        bool indicatorShow;
        SpriteRenderer ballSpriteRenderer = GetComponent<SpriteRenderer>();
        Color ballColor = ballSpriteRenderer.color;
        Color learpedColor;
        if (force == forceClamped && ballsSteady && GetComponent<Renderer>().isVisible)
        {
            indicatorShow = true;
            if (force >= 0 && force < 1) learpedColor = Color.Lerp(Color.green, Color.yellow, force);
            else learpedColor = Color.Lerp(Color.yellow, Color.red, force - 1);
            if (ballColor != learpedColor) ballColor = Color.Lerp(ballColor, learpedColor, 4 * Time.deltaTime);
            if (Input.GetMouseButtonDown(0)) ballRigidbody2D.AddForce(20 * forceClamped * ballDirection, ForceMode2D.Impulse);
        }
        else
        {
            indicatorShow = false;
            if (ballColor != Color.white) ballColor = Color.Lerp(ballColor, Color.white, 4 * Time.deltaTime);
        }
        ballSpriteRenderer.color = ballColor;

        // Calculate angle and position of arrow (with slight offset from the ball)
        cue.transform.position = transform.position - ballDirection;
        cue.transform.eulerAngles = new Vector3(0, 0, ballAngle * Mathf.Rad2Deg);
        SpriteRenderer cueSpriteRenderer = cue.GetComponent<SpriteRenderer>();

        LineRenderer lineRenderer1 = indicator1.GetComponent<LineRenderer>();
        LineRenderer lineRenderer2 = indicator2.GetComponent<LineRenderer>();
        LineRenderer lineRenderer3 = indicator3.GetComponent<LineRenderer>();
        // Mask the second and third ray
        int LayerHole = 7;
        int LayerBall = 8;
        int layerMask1 = 1 << LayerHole;
        int layerMask2 = (1 << LayerHole) | (1 << LayerBall);
        // Creates three ray to indicate the ball angle and bounce
        float ballRadius = GetComponent<CircleCollider2D>().radius;
        RaycastHit2D raycast1 = Physics2D.CircleCast(transform.position + 2 * ballRadius * ballDirection, ballRadius, ballDirection, Mathf.Infinity, ~layerMask1);
        RaycastHit2D raycast2 = Physics2D.Raycast(ballIndicator.transform.position, Vector2.Reflect(ballDirection, raycast1.normal), Mathf.Infinity, ~layerMask2);
        RaycastHit2D raycast3 = Physics2D.Raycast(ballIndicator.transform.position, raycast1.normal * -1, Mathf.Infinity, ~layerMask2);
        ballIndicator.transform.position = raycast1.point + raycast1.normal * ballRadius;
        // Create line 1
        lineRenderer1.SetPosition(0, transform.position + ballDirection * ballRadius);
        lineRenderer1.SetPosition(1, ballIndicator.transform.position - ballDirection * ballRadius);
        // Create line 2
        Vector2 raycast2Direction = DirectionCalculator(AngleCalculator(ballIndicator.transform.position, raycast2.point));
        lineRenderer2.SetPosition(0, ballIndicator.transform.position - (Vector3)raycast2Direction * ballRadius);
        lineRenderer2.SetPosition(1, raycast2.point);
        // Create line 3
        Vector2 raycast3Direction = DirectionCalculator(AngleCalculator(ballIndicator.transform.position, raycast3.point));
        lineRenderer3.SetPosition(0, ballIndicator.transform.position - (Vector3)raycast3Direction * ballRadius);
        lineRenderer3.SetPosition(1, raycast3.point);
        if (raycast1.collider.CompareTag("PoolTable"))
        {
            indicator2.SetActive(true);
            indicator3.SetActive(false);
        }
        else if (raycast1.collider.CompareTag("Ball"))
        {
            indicator2.SetActive(false);
            indicator3.SetActive(true);
        }

        if (indicatorShow)
        {
            cue.SetActive(true);
            indicator1.SetActive(true);
            indicator2.SetActive(true);
            indicator3.SetActive(true);
            ballIndicator.SetActive(true);
            cueSpriteRenderer.color = FadeIn(cueSpriteRenderer.color);
            lineRenderer1.startColor = FadeIn(lineRenderer1.startColor);
            lineRenderer1.endColor = FadeIn(lineRenderer1.endColor);
        }
        else
        {
            if (cueSpriteRenderer.color.a > 0)
            {
                cueSpriteRenderer.color = FadeOut(cueSpriteRenderer.color);
                lineRenderer1.startColor= FadeOut(lineRenderer1.startColor);
                lineRenderer1.endColor = FadeOut(lineRenderer1.endColor);
            }
            else
            {
                cue.SetActive(false);
                indicator1.SetActive(false);
                indicator2.SetActive(false);
                indicator3.SetActive(false);
                ballIndicator.SetActive(false);
            }
        }
    }

    // Get angle in degree beetween two point
    private float AngleCalculator(Vector3 origin, Vector3 end)
    {
        float angle = Mathf.Atan2(origin.y - end.y, origin.x- end.x);
        return angle;
    }

    // Get an angle as a Vector
    private Vector3 DirectionCalculator(float angle)
    {
        Vector3 angleDirection = new(Mathf.Cos(angle), Mathf.Sin(angle));
        return angleDirection;
    }

    private Color FadeIn(Color color)
    {
        if (color.a <= 1) color.a += 4 * Time.deltaTime;
        return color;
    }

    private Color FadeOut(Color color)
    {
        if (color.a >= 0) color.a -= 4 * Time.deltaTime;
        return color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperBallShadow : MonoBehaviour
{
    #region Unity Methods

    // Update is called once per frame
    virtual protected void Update()
    {
        // rotate shadow with light direction
        float angle = Mathf.Atan2(transform.position.y, transform.position.x);
        float degreeAngle = Mathf.Rad2Deg * angle;
        transform.eulerAngles = new Vector3(0, 0, degreeAngle + 180);
    }

    #endregion
}

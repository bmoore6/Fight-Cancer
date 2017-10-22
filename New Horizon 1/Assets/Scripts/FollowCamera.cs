using UnityEngine;
using System.Collections;

/// <summary>
/// http://answers.unity3d.com/questions/29183/2d-camera-smooth-follow.html
/// Jennifer1
/// </summary>
public class FollowCamera : MonoBehaviour
{
    // This will be the player object that the camera will follow
    [SerializeField]
    GameObject target;

    float interpVelocity;
    float minDistance;
    float followDistance;
    Vector3 offset = new Vector3(0.0f, 0.8f, 0.0f);
    Vector3 targetPos;
    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 5f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

        }
    }
}
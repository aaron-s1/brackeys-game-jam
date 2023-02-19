using UnityEngine;

public class TestPlatformMove : MonoBehaviour
{
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 endPosition = new Vector3(0, 5, 0);
    [SerializeField] float lerpTime = 3f;

    float currentTime = 0.0f;
    bool isLerpingToEndPosition = true;

    void Awake() 
    {
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x, endPosition.y, startPosition.z);
    }        

    private void Update()
    {
        Vector3 targetPosition = isLerpingToEndPosition ? endPosition : startPosition;

        float t = Mathf.Clamp01(currentTime / lerpTime);

        transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        
        currentTime += Time.deltaTime;

        if (currentTime >= lerpTime)
        {
            isLerpingToEndPosition = !isLerpingToEndPosition;
            currentTime -= lerpTime;
        }
    }
}
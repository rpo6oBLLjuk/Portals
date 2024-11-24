using UnityEngine;

public class PlayerRotationFix : MonoBehaviour
{
    [SerializeField] private float rotateTime;
    [SerializeField] private AnimationCurve rotateCurve;

    private Quaternion incorrectRotation;
    private Quaternion correctRotation;
    [SerializeField] private float currentRotateTime = -1;


    public void FixRotation()
    {
        incorrectRotation = transform.rotation;
        correctRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        currentRotateTime = 0;
    }

    void Update()
    {
        if (currentRotateTime < 0)
            return;

        if (currentRotateTime < rotateTime)
        {
            currentRotateTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(incorrectRotation, correctRotation, rotateCurve.Evaluate(currentRotateTime / rotateTime));
        }
        else
        {
            currentRotateTime = -1;
        }
    }
}

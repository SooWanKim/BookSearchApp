using UnityEngine;

public class LoadingCircleUI : MonoBehaviour
{
    public RectTransform progressRectComponent;
    readonly float RotateSpeed = 200f;


    private void Update()
    {
        progressRectComponent.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);
    }
}

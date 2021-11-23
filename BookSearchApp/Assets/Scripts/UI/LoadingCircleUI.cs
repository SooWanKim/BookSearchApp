using UnityEngine;

public class LoadingCircleUI : MonoBehaviour
{
    public RectTransform progressRectComponent;
    private float rotateSpeed = 200f;


    private void Update()
    {
        progressRectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}
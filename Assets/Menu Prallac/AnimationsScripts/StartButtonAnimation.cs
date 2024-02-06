using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
    private Button button;
    private Vector3 originalScale;

    private void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
    }

    public void AnimateButton()
    {
        // Scale up the button
        transform.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutQuad).OnComplete(ResetButtonScale);
    }

    private void ResetButtonScale()
    {
        // Scale down the button to its original size
        transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad);
    }
}
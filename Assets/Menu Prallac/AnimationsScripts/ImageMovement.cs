
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageMovement : MonoBehaviour
{
    public Image image;
    public float moveDistance = 200f;
    public float moveDuration = 2f;

    private Vector3 originalPos;

    private void Start()
    {
        // Сохраняем исходную позицию картинки
        originalPos = image.rectTransform.position;

        // Запускаем плавное перемещение картинки вправо
        image.rectTransform.DOMoveX(originalPos.x + moveDistance, moveDuration)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
using UnityEngine;
using DG.Tweening;

public class MenuAnimation : MonoBehaviour
{

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float speed;

    void Start()
    {
        // Перемещаем каждый элемент меню
        foreach (Transform menuElement in transform)
        {
            // Создаем пустую последовательность
            Sequence sequence = DOTween.Sequence();

            // Добавляем движение от начальной точки до конечной
            sequence.Append(menuElement.DOMove(new Vector3(minX, minY, 0), speed));

            // Добавляем паузу на конечной точке
            sequence.AppendInterval(speed);

            // Добавляем движение от конечной точки до начальной
            sequence.Append(menuElement.DOMove(new Vector3(maxX, maxY, 0), speed));

            // Добавляем паузу на начальной точке
            sequence.AppendInterval(speed);

            // Зацикливаем движение
            sequence.SetLoops(-1);
        }
    }


}
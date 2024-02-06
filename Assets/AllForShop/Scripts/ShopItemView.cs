using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour, IPointerClickHandler
{
    public event Action<ShopItemView> Click;

    [SerializeField] private Sprite _standartBackground;
    [SerializeField] private Sprite _hightlightBackground;

    [SerializeField] private Image _contentImage;
    [SerializeField] private Image _lockImage;

    [SerializeField] private Image _selectionText;

    private Image _backgroundImage;


    public void OnPointerClick(PointerEventData eventData) => Click?.Invoke(this);
    
}

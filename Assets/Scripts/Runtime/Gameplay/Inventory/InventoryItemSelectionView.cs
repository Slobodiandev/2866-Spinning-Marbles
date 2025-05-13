using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.Inventory
{
    public class InventoryItemSelectionView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        private int _id;
    
        public Sprite Sprite => _image.sprite;
        public int Id => _id;
    
        public event Action<int, Sprite> OnPressed;
    
        public void Initialize(Sprite sprite, int itemID)
        {
            _id = itemID;
            _image.sprite = sprite;
        
            _button.onClick.AddListener(() => OnPressed?.Invoke(itemID, sprite));
        }
    }
}

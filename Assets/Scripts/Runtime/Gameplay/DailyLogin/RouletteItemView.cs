using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.DailyLogin
{
    public class RouletteItemView : MonoBehaviour
    {
        [SerializeField] private Image _prizeImage;

        private RouletteItemModel _rouletteItemModel;
    
        public RouletteItemModel RouletteItemModel => _rouletteItemModel;
    
        public void Initialize(RouletteItemModel rouletteItemModel)
        {
            _rouletteItemModel = rouletteItemModel;
            _prizeImage.sprite = rouletteItemModel.Sprite;
        }
    }
}

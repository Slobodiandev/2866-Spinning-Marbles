using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Runtime.Gameplay.Services.IAP
{
    [CreateAssetMenu(fileName = "IAPConfig", menuName = "Config/IAPConfig")]
    public class IAPConfig : BaseConfig
    {
        [SerializeField] private List<ProductData> _products;

        public List<ProductData> Products => _products;
    }
}
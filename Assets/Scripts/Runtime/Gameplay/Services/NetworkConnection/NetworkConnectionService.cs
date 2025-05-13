using UnityEngine;

namespace Runtime.Gameplay.Services.NetworkConnection
{
    public class NetworkConnectionService : INetworkConnectionService
    {
        bool INetworkConnectionService.IsInternetReachable()
        {
            return UnityEngine.Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}
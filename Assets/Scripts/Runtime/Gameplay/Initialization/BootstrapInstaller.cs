using Core.StateMachine;
using Runtime.Gameplay.Game;
using Runtime.Gameplay.Game.Controllers;
using Runtime.Gameplay.Initialization.Controllers;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Initialization
{
    [CreateAssetMenu(fileName = "BootstrapInstaller", menuName = "Installers/BootstrapInstaller")]
    public class BootstrapInstaller : ScriptableObjectInstaller<BootstrapInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Bootstrapper>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BootstrapStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
            Container.Bind<StateMachine>().AsTransient();

            Container.Bind<AudioSettingsBootstrapController>().AsSingle();
            Container.Bind<ApplicationStateChangeController>().AsSingle();
        }
    }
}
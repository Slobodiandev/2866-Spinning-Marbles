using System.Collections.Generic;
using System.Linq;
using Runtime.Gameplay.Gameplay.Interfaces;

namespace Runtime.Gameplay.Gameplay.SystemManagers
{
    public abstract class BaseSystemsManager
    {
        protected List<IEnableable> _enableableSystems = new ();
        protected List<IResettable> _resettableSystems = new ();

        public void RegisterSystem(object system)
        {
            if(system is IEnableable enableable)
                _enableableSystems.Add(enableable);
        
            if(system is IResettable resettable)
                _resettableSystems.Add(resettable);
        }

        public void EnableAllSystems(bool enable) => _enableableSystems.ForEach(system => system.Enable(enable));
        public void ResetAllSystems() => _resettableSystems.ForEach(system => system.Reset());

        public void EnableSystem<T>(bool enable) where T : class, IEnableable
        {
            var system = _enableableSystems.OfType<T>().FirstOrDefault();
            if(system != null)
                system.Enable(enable);
        }
    }
}

using Runtime.Gameplay.Gameplay.Interfaces;

namespace Runtime.Gameplay.Gameplay.Systems
{
    public abstract class BaseSystem : IEnableable, IResettable
    {
        protected bool Enabled = false;
    
        public virtual void Enable(bool enable)
        {
            Enabled = enable;
        }

        public abstract void Reset();
    }
}

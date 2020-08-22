
namespace Actor_Components.State_Machine.States
{
    public abstract class BaseState
    {
        public abstract FighterStateMachine.States Update(float delaTime);
    }
}

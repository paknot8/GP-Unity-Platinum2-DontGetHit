public abstract class PlayerBaseState
{
    public abstract void EnterState(Game_Manager player);
    public abstract void ExitState(Game_Manager player);
    public abstract void UpdateState(Game_Manager player);
}
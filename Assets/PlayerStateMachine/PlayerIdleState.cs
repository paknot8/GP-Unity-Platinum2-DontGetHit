using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(Game_Manager player)
    {
        Debug.Log("Idle State");
    }

    public override void ExitState(Game_Manager player)
    {
        
    }

    public override void UpdateState(Game_Manager player)
    {
        player.Movement();
        if(player.vector != Vector2.zero){
            player.SwitchState(player.moveState);
        }
    }
}
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public override void EnterState(Game_Manager player)
    {
        Debug.Log("Moving State");
    }

    public override void ExitState(Game_Manager player)
    {
        
    }

    public override void UpdateState(Game_Manager player)
    {
        player.Movement();
        if(player.vector == Vector2.zero){
            player.SwitchState(player.idleState);
        }
    }
}
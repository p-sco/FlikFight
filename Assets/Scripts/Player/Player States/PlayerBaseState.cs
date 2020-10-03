using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerController player);
    public abstract void Update(PlayerController player);
    public abstract void OnCollisionEnter2D(PlayerController player);

}

using UnityEngine;

public class AuraWeapon : Weapon
   
{
    protected override bool NeedsTarget => false;
    protected override void Fire()
    {
        throw new System.NotImplementedException();
    }
    
}

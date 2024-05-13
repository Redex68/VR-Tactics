using Fusion;
using UnityEngine;

public class PlayerGunFiringSync : NetworkBehaviour
{
    [SerializeField] PlayerTypeVariable playerType;

    private BNG.RaycastWeapon _weapon;

    [Networked] private int firedGun { get; set; } = 0;
    private ChangeDetector _changeDetector;

    override public void Spawned()
    {
        _weapon = GetComponent<BNG.RaycastWeapon>();
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        if(playerType.value == PlayerType.VR) _weapon.onShootEvent.AddListener(OnShoot);
        else
        {
            _weapon.ReloadMethod = BNG.ReloadType.InfiniteAmmo;
            _weapon.InternalAmmo = _weapon.MaxInternalAmmo;
            _weapon.AutoChamberRounds = true;
            _weapon.MustChamberRounds = false;
        }
    }

    public override void Render()
    {
        foreach(var change in _changeDetector.DetectChanges(this))
        {
            switch(change)
            {
                case nameof(firedGun):
                    if(playerType.value != PlayerType.VR) _weapon.Shoot();
                    break;
            }
        }
    }

    private void OnShoot()
    {
        firedGun++;
    }

}

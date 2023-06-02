namespace Grubs;

[Prefab]
public partial class FireEmitGadgetComponent : GadgetComponent
{
	[Prefab, Net]
	public float FireSpeed { get; set; } = 850f;

	public override void OnUse( Weapon weapon, int charge )
	{
		FireHelper.StartFiresWithDirection( weapon.GetStartPosition().WithY( 0f ), (Grub.EyeRotation.Forward.Normal * Grub.Facing * FireSpeed).WithY( 0f ), 1 );
		Gadget.Delete();
	}
}
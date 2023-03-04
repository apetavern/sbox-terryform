namespace Grubs;

[Prefab, Category( "Gadget" )]
public partial class Gadget : AnimatedEntity
{
	public Grub Grub => Owner as Grub;

	[Prefab]
	public bool ShouldUseModelCollision { get; set; } = false;

	[Prefab]
	public float CollisionRadius { get; set; } = 1.0f;

	[Prefab, Net]
	public bool ShouldCameraFollow { get; set; } = true;

	public override void Spawn()
	{
		Transmit = TransmitType.Always;
		EnableLagCompensation = true;
		Health = 1;

		if ( ShouldUseModelCollision )
			// TODO: This doesn't seem to work.
			SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
		else
			SetupPhysicsFromSphere( PhysicsMotionType.Keyframed, Position, CollisionRadius );
	}

	public void OnUse( Grub grub, Weapon weapon, int charge )
	{
		Owner = grub;
		grub.Player.Gadgets.Add( this );

		Position = (Grub.EyePosition + (Grub.Facing * 20f)).WithY( 0 );

		foreach ( var component in Components.GetAll<GadgetComponent>() )
		{
			component.OnUse( weapon, charge );
		}
	}

	public override void Simulate( IClient client )
	{
		foreach ( var component in Components.GetAll<GadgetComponent>() )
		{
			component.Simulate( client );
		}
	}
}

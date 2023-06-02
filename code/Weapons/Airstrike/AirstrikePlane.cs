namespace Grubs;

[Prefab]
public partial class AirstrikeGadgetComponent : GadgetComponent
{
	/// <summary>
	/// What are we dropping?
	/// </summary>
	[Prefab]
	public Prefab Projectile { get; set; }

	[Prefab]
	public float DropRateSeconds { get; set; } = 0.2f;

	/// <summary>
	/// How many are we dropping?
	/// </summary>
	[Prefab]
	public int ProjectileCount { get; set; } = 1;

	[Prefab]
	public Vector3 DropOffset { get; set; }

	[Net]
	public bool RightToLeft { get; set; }

	public bool HasReachedTarget { get; private set; }
	public Vector3 TargetPosition { get; set; }

	private float _speed = 18;

	public override void Simulate( IClient client )
	{
		var dir = RightToLeft ? Vector3.Backward : Vector3.Forward;
		Gadget.Position += dir * _speed;

		if ( !Game.IsServer )
			return;

		const float xLookAhead = 200;
		var targetX = TargetPosition.x;

		bool withinTargetPosition = (RightToLeft && Gadget.Position.x <= targetX + xLookAhead) || (!RightToLeft && Gadget.Position.x >= targetX - xLookAhead);
		if ( withinTargetPosition && !HasReachedTarget )
		{
			HasReachedTarget = true;
			Gadget.ShouldCameraFollow = false;
			DropPayload();
			Gadget.DeleteAsync( 2 );
		}
	}

	private async void DropPayload()
	{
		for ( int i = 0; i < ProjectileCount; i++ )
		{
			if ( !PrefabLibrary.TrySpawn<Gadget>( Projectile.ResourcePath, out var bomb ) )
				continue;

			var drop = Gadget.GetAttachment( "droppoint", true );
			var dropPosition = drop.HasValue ? drop.Value.Position : Gadget.Position + Vector3.Down * 32;

			bomb.Owner = Gadget.Owner;
			bomb.Position = dropPosition;
			Player.Gadgets.Add( bomb );

			foreach ( var comp in bomb.Components.GetAll<GadgetComponent>() )
				comp.OnUse( null, 0 );

			await GameTask.DelaySeconds( DropRateSeconds );
		}
	}
}

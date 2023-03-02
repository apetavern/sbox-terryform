namespace Grubs;

[Prefab]
public partial class ProjectileComponent : ExplosiveComponent
{
	[Prefab]
	public bool ProjectileShouldUseTrace { get; set; } = false;

	[Prefab]
	public float ProjectileSpeed { get; set; } = 1000.0f;

	public List<ArcSegment> Segments { get; set; } = new();

	/// <summary>
	/// Debug console variable to see the projectiles path.
	/// </summary>
	[ConVar.Replicated( "projectile_debug" )]
	public static bool ProjectileDebug { get; set; }

	private void DrawSegments()
	{
		foreach ( var segment in Segments )
			DebugOverlay.Line( segment.StartPos, segment.EndPos, Game.IsServer ? Color.Red : Color.Green );
	}

	public override void Simulate( IClient client )
	{
		base.Simulate( client );

		if ( ProjectileShouldUseTrace && ProjectileDebug )
			DrawSegments();
	}

	protected override void PhysicsTick()
	{
		if ( !ProjectileShouldUseTrace )
		{
			base.PhysicsTick();
			return;
		}

		if ( (Segments[0].EndPos - Explosive.Position).IsNearlyZero( 2.5f ) )
		{
			if ( Segments.Count > 1 )
				Segments.RemoveAt( 0 );
			else
				Explode();

			return;
		}

		Explosive.Rotation = Rotation.LookAt( Segments[0].EndPos - Segments[0].StartPos );
		Explosive.Position = Vector3.Lerp( Segments[0].StartPos, Segments[0].EndPos, Time.Delta / (1 / ProjectileSpeed) );
	}
}

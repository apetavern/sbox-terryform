namespace Grubs;

public partial class LateJoinMechanic : ControllerMechanic
{
	private AnimatedEntity? _parachute { get; set; }
	public bool FinishedParachuting { get; private set; }

	protected override bool ShouldStart()
	{
		return true;
	}

	protected override void OnStart()
	{
		if ( _parachute.IsValid() )
			return;

		_parachute = new AnimatedEntity()
		{
			Model = Model.Load( "models/tools/parachute/parachute.vmdl" ),
		};

		_parachute?.SetParent( Entity, true );
		_parachute?.SetAnimParameter( "landed", false );
		_parachute?.SetAnimParameter( "deploy", true );
	}

	protected override void Simulate()
	{
		FinishedParachuting = Entity.Controller.IsGrounded;

		if ( !FinishedParachuting )
		{
			Entity.Velocity = new Vector3( GamemodeSystem.Instance.ActiveWindForce, Entity.Velocity.y, Entity.Velocity.ClampLength( 200f ).z );
			return;
		}

		_parachute?.SetAnimParameter( "deploy", false );
		_parachute?.SetAnimParameter( "landed", true );
		_parachute?.Delete();
	}
}
﻿using Sandbox;
using Grubs.Pawn;
using Grubs.Terrain;

namespace Grubs.Weapons
{
	public class Boomer : Weapon
	{
		public override string WeaponName => "Boomer";
		public override string ModelPath => "models/weapons/shotgun/shotgun.vmdl";
		public override HoldPose HoldPose => HoldPose.Shotgun;
		public override bool IsFiredTurnEnding => true;

		public override void Simulate( Client player )
		{
			base.Simulate( player );

			if ( IsServer )
			{
				using ( Prediction.Off() )
				{
					int input = Input.Pressed( InputButton.Attack1 ) ? 1 : (Input.Pressed( InputButton.Attack2 ) ? 0 : -1);

					var tempTrace = Trace.Ray( Owner.EyePos, Owner.EyePos + Owner.EyeRot.Forward.Normal * WeaponReach ).Ignore( this ).Run();
					DebugOverlay.Line( tempTrace.StartPos, tempTrace.EndPos );

					if ( input != -1 )
					{
						var position = tempTrace.EndPos;

						Color color = input == 1 ? Color.Red : Color.Green;
						DebugOverlay.Circle( position, Rotation.FromYaw( 90f ), 64f, color.WithAlpha( 0.15f ), true, 5f );

						SDF circle = new Circle( position, 128f, input == 0 ? SDF.MergeType.Add : SDF.MergeType.Subtract );
						Terrain.Terrain.Update( circle );
					}
				}
			}
		}

		public override void OnFireEffects()
		{
			Particles.Create( "particles/pistol_muzzleflash.vpcf", this, "muzzle" );
		}
	}
}

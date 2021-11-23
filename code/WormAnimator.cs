﻿using Sandbox;
using System;

namespace TerryForm
{
	public class WormAnimator : PawnAnimator
	{
		public override void Simulate()
		{
			DoRotation();

			// Aim angle
			{
				float aimAngle = -Pawn.EyeRot.Pitch().Clamp( -65f, 35f );
				SetParam( "aimangle", aimAngle );
			}

			{
				// TODO:
				// - How do we handle offsetting the player's model from their bbox?
				// - How do we actually calculate incline properly?
				var tr = Trace.Ray( Pawn.Position, Pawn.Position + Pawn.Rotation.Down*128 ).WorldOnly().Ignore( Pawn ).Run();
				float incline = (1.0f - tr.Normal.z) * 360f;
				DebugOverlay.ScreenText( incline.ToString() );

				SetParam( "incline", -incline );
			}

			// Velocity
			{
				float velocity = Pawn.Velocity.Cross( Vector3.Up ).Length.LerpInverse( 0f, 100f );
				SetParam( "velocity", velocity );
			}

			// TODO: Link this to current weapon (can we get away with setting this in the weapon itself?)
			{
				SetParam( "holdpose", (int)HoldPose.Bazooka );
			}
		}

		private void DoRotation()
		{
			//
			// Rotate the player when we try to look backwards
			//
			float playerFacing = Pawn.EyeRot.Forward.Dot( Pawn.Rotation.Forward );
			if ( playerFacing < 0 )
			{
				Rotation *= Rotation.From( 0, 180, 0 ); // Super janky
				Pawn.ResetInterpolation();
			}
		}
	}
}

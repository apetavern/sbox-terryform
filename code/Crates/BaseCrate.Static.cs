﻿using ModelDoc;
using Sandbox;
using TerryForm.Utils;

namespace TerryForm.Crates
{
	public partial class Crate
	{
		private static string GetRandomCrate()
		{
			float val = Rand.Float();
			float cumVal = 0;
			foreach ( var crateType in GameConfig.CrateTypes )
			{
				cumVal += crateType.Value;

				Log.Trace( $"{val} lt {cumVal}" );
				if ( val < cumVal )
				{
					return crateType.Key;
				}
			}

			return null;
		}

		public static Crate SpawnCrate()
		{
			Host.AssertServer();

			var crateType = GetRandomCrate();

			if ( string.IsNullOrEmpty( crateType ) )
			{
				Log.Trace( "Missed random chance" );
				return null;
			}

			var crate = Library.Create<Crate>( crateType );
			return crate;
		}

		[ServerCmd]
		public static void ForceSpawnCrate()
		{
			Rand.SetSeed( Time.Tick );
			var crate = SpawnCrate();

			if ( crate.IsValid() )
			{
				// TODO: sample from terrain to find a viable spot to plonk a crate down
				crate.Position = new Vector3( Rand.Float( -512, 512 ), 0, 512 );
				Log.Trace( $"Spawned crate" );
			}
		}
	}
}

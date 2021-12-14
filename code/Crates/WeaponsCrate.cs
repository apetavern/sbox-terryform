﻿using Sandbox;

namespace TerryForm.Crates
{
	[Library( "crate_weapons" )]
	public class WeaponsCrate : BaseCrate
	{
		public override void Spawn()
		{
			base.Spawn();
			SetModel( "models/crates/weapons_crate/weapons_crate.vmdl" );
		}
	}
}

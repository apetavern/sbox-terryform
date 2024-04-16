﻿using Grubs.Common;
using Grubs.Equipment;
using Grubs.Pawn.Controller;
using Grubs.Terrain;

namespace Grubs.Pawn;

[Title( "Grubs - Container" ), Category( "Grubs" )]
public sealed class Grub : Component
{
	public Player? Player { get; set; }

	[Property] public required HealthComponent Health { get; set; }
	[Property] public required GrubPlayerController PlayerController { get; set; }
	[Property] public required GrubCharacterController CharacterController { get; set; }
	[Property] public required GrubAnimator Animator { get; set; }
	[Property, ReadOnly] public EquipmentComponent? ActiveEquipment => Player?.Inventory.ActiveEquipment;

	[Sync] public string Name { get; set; } = "Grubby";

	protected override void OnStart()
	{
		base.OnStart();

		if ( !IsProxy )
			InitializeLocal();
	}

	private void InitializeLocal()
	{
		if ( GrubFollowCamera.Local is not null )
			GrubFollowCamera.Local.Target = GameObject;
	}

	public void OnHardFall()
	{
		Player?.Inventory.ToggleEquipment( false, Player.Inventory.ActiveSlot );
	}

	// public void Respawn()
	// {
	// 	var spawnPoints = Scene.GetAllComponents<SpawnPoint>().ToArray();
	// 	var spawn = Random.Shared.FromArray( spawnPoints )?.Transform.World ?? Transform.World;
	// 	Health.Heal( 150f );
	// 	Transform.Position = spawn.Position;
	// }

	[ConCmd( "gr_take_dmg" )]
	public static void TakeDmgCmd( float hp )
	{
		var grub = Game.ActiveScene.GetAllComponents<Grub>().FirstOrDefault();
		if ( grub is null )
			return;

		grub.Health.TakeDamage( hp );
	}
}
﻿using Grubs.Common;

namespace Grubs.Equipment.Weapons;

[Title( "Grubs - Ninja Rope Hook Tip" ), Category( "Equipment" )]
public sealed class NinjaRopeHook : Component, Component.ICollisionListener
{
	[Property]public GameObject MountObject { get; set; }

	[Property] public PhysicsProjectileComponent PhysicsProjectileComponent { get; set; }

	RopeBehaviorComponent Rope;

	protected override void OnUpdate()
	{
		if ( IsProxy ) return;

		if( !MountObject.IsValid() )
		{
			GameObject.Destroy();
		}else if ( MountObject.Enabled && Rope != null)
		{
			if ( PhysicsProjectileComponent.Grub.IsValid() )
			{
				PhysicsProjectileComponent.Grub.Animator.GrubRenderer.Set( "heightdiff", 18f );
				PhysicsProjectileComponent.Grub.Animator.GrubRenderer.Set( "onrope", true );
				PhysicsProjectileComponent.Grub.Transform.Rotation = Rotation.LookAt( Rope.HookDirection, PhysicsProjectileComponent.Grub.Transform.Rotation.Up );
				PhysicsProjectileComponent.Grub.CharacterController.IsOnGround = false;
			}
			else
			{
				MountObject.Components.Get<Mountable>().Dismount();
			}
		}
	}

	public void OnCollisionStart( Collision other )
	{
		Components.Get<Rigidbody>().Enabled = false;

		CreateRopeSystem();
		//Log.Info( "Collision Start" );
		//Log.Info( other.Other.GameObject.Tags );
	}

	public void CreateRopeSystem()
	{
		MountObject.Parent = Scene;
		MountObject.Transform.Position = PhysicsProjectileComponent.Grub.Transform.Position;
		MountObject.Enabled = true;
		Rope = MountObject.Components.Get<RopeBehaviorComponent>();
		MountObject.Components.Get<Mountable>().Mount( PhysicsProjectileComponent.Grub );
	}

	public void OnCollisionUpdate( Collision other )
	{
		
	}

	public void OnCollisionStop( CollisionStop other )
	{
		
	}
}

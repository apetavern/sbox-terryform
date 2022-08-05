﻿using Grubs.Utils;
using Grubs.Weapons;

namespace Grubs.Player;

[Category( "Grubs" )]
public partial class Worm : AnimatedEntity
{
	[Net, Predicted]
	public WormController Controller { get; set; }

	[Net, Predicted]
	public WormAnimator Animator { get; set; }

	[Net, Predicted]
	public GrubsWeapon ActiveChild { get; set; }

	[Net, Predicted]
	public GrubsWeapon LastActiveChild { get; set; }

	[Net]
	public int TeamNumber { get; set; }

	[Net]
	public bool IsTurn { get; set; } = false;

	private bool IsDressed { get; set; } = false;

	public Worm()
	{

	}

	public Worm( int teamNumber )
	{
		TeamNumber = teamNumber;
	}

	public void Spawn( Client cl )
	{
		base.Spawn();

		SetModel( "models/citizenworm.vmdl" );

		Name = Rand.FromArray( GameConfig.WormNames );
		Health = 100;

		Controller = new WormController();
		Animator = new WormAnimator();

		DressFromClient( cl );
		SetHatVisible( true );

	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		Controller?.Simulate( cl, this, Animator );

		if ( IsTurn )
			SimulateActiveChild( cl, ActiveChild );

	}

	public virtual void SimulateActiveChild( Client client, GrubsWeapon child )
	{
		if ( LastActiveChild != child )
		{
			OnActiveChildChanged( LastActiveChild, child );
			LastActiveChild = child;
		}

		if ( !LastActiveChild.IsValid() )
			return;

		LastActiveChild.Simulate( client );
	}

	public virtual void OnActiveChildChanged( GrubsWeapon previous, GrubsWeapon next )
	{
		previous?.ActiveEnd( this, previous.Owner != this );
		next?.ActiveStart( this );
	}

	public void EquipWeapon( GrubsWeapon weapon )
	{
		ActiveChild = weapon;
	}

	public char GetTeamName()
	{
		var index = TeamNumber - 1;

		return GameConfig.TeamNames[index];
	}

	public void DressFromClient( Client cl )
	{
		var clothes = new ClothingContainer();
		clothes.LoadFromClient( cl );
		IsDressed = true;


		// Skin tone
		var skinTone = clothes.Clothing.FirstOrDefault( model => model.Model == "models/citizenworm.vmdl" );
		SetMaterialGroup( skinTone?.MaterialGroup );

		// We only want the hair/hats so we won't use the logic built into Clothing
		var items = clothes.Clothing.Where( item =>
			item.Category == Clothing.ClothingCategory.Hair ||
			item.Category == Clothing.ClothingCategory.Hat
		);

		if ( !items.Any() )
			return;

		foreach ( var item in items )
		{
			var ent = new AnimatedEntity( item.Model, this );

			// Add a tag to the hat so we can reference it later.
			if ( item.Category == Clothing.ClothingCategory.Hat )
				ent.Tags.Add( "hat" );

			if ( !string.IsNullOrEmpty( item.MaterialGroup ) )
				ent.SetMaterialGroup( item.MaterialGroup );
		}

	}

	public void SetHatVisible( bool visible )
	{
		var hat = Children.OfType<AnimatedEntity>().FirstOrDefault( child => child.Tags.Has( "hat" ) );

		if ( hat is null )
			return;

		hat.EnableDrawing = visible;
	}
}

﻿using Grubs.Systems.Pawn;
using Grubs.Terrain;

namespace Grubs.Systems.GameMode;

[Title( "Free For All" ), Category( "Grubs/Game Mode" )]
public sealed class FreeForAll : BaseGameMode
{
	public override string Name => "Free For All";
	
	[Sync( SyncFlags.FromHost )]
	public FreeForAllState State { get; set; } = FreeForAllState.Lobby;

	[Sync( SyncFlags.FromHost )]
	public NetList<Player> PlayerQueue { get; private set; } = new();
	
	[Sync( SyncFlags.FromHost )]
	public Player ActivePlayer { get; private set; }

	protected override void OnModeInit()
	{
		Log.Info( $"{Name} mode initializing." );

		State = FreeForAllState.Lobby;

		GrubsTerrain.Instance.Init();
	}

	protected override void OnModeStarted()
	{
		if ( State is FreeForAllState.Playing )
		{
			Log.Warning( $"Tried to start {Name}, but State is already Playing." );
			return;
		}
		
		Log.Info( $"{Name} mode starting." );


		// For each player, spawn Grubs and initialize their inventory.
		foreach ( var player in Players )
		{
			InitializePlayer( player );
		}

		ActivePlayer = PlayerQueue.First();

		State = FreeForAllState.Playing;
	}

	protected override void OnPlayerJoined( Player player )
	{
		Log.Info( $"Adding {player.GameObject.Name} to Free For All game mode." );

		if ( State is not FreeForAllState.Playing )
			return;

		if ( GrubsConfig.SpawnLateJoiners )
		{
			InitializePlayer( player );
		}
		else
		{
			Log.Warning( "Player joined late. What do we do here?" );
		}
	}

	private void InitializePlayer( Player player )
	{
		const float grubSize = 8f;
		
		if ( !player.IsValid() )
			return;
		
		for ( var i = 0; i < GrubsConfig.GrubCount; i++ )
		{
			var spawnLocation = GrubsTerrain.Instance.FindSpawnLocation( size: grubSize );
			player.AddGrub( spawnLocation );
		}
			
		PlayerQueue.Add( player );
	}
}

public enum FreeForAllState
{
	Lobby,
	Playing,
	GameOver
}

﻿using Grubs.UI;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

namespace Grubs.States
{
	public partial class StateHandler : BaseNetworkable
	{
		[Net] public BaseState State { get; set; } = new WaitingState();
		public List<Pawn.Player> Players { get; set; } = new();
		public static StateHandler Instance { get; set; }
		public HudEntity<RootPanel> Hud { get; set; }

		public StateHandler()
		{
			Instance = this;

			if ( Host.IsServer )
			{
				Hud = new MenuHudEntity();
				State.Start();
			}
		}

		public void OnPlayerJoin( Pawn.Player player )
		{
			Players.Add( player );
			State?.OnPlayerJoin( player );
		}

		public void ChangeState( BaseState state )
		{
			Assert.NotNull( state );

			State?.Finish();
			State = state;
			State?.Start();

			if ( Host.IsServer )
				UpdateHud();
		}

		private void UpdateHud()
		{
			if ( Hud is not PlayingHudEntity && State is PlayingState )
			{
				Hud.Delete();
				Hud = new PlayingHudEntity();
			}
			else
			{
				Hud = new MenuHudEntity();
			}
		}

		public void RemovePlayer( Pawn.Player player )
		{
			Players.Remove( player );

			if ( Players.Count < 2 )
				ChangeState( new EndState() );

			// Temporarily announce winner. We'll handle this better through EndState later.
			var winner = Players[0];
			Log.Info( $"🎉 {winner.Client.Name} has won." );
		}

		[Event.Tick.Server]
		public static void Tick()
		{
			Instance?.State?.OnTick();
		}
	}
}

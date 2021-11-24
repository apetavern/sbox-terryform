﻿using Sandbox.UI;
using System;
using TerryForm.States;

namespace TerryForm.UI
{
	[UseTemplate]
	public class GameInfoPanel : Panel
	{
		public Label StateTime { get; set; }
		public Label TurnTime { get; set; }

		public GameInfoPanel()
		{
			StyleSheet.Load( "/Code/UI/GameInfoPanel.scss" );
		}

		public override void Tick()
		{
			base.Tick();

			if ( Game.StateHandler.State is PlayingState playingState )
			{
				TimeSpan stateTimeSpan = TimeSpan.FromSeconds( playingState.TimeLeft );
				StateTime.Text = string.Format( "{0:D2}:{1:D2}",
								stateTimeSpan.Minutes,
								stateTimeSpan.Seconds );

				if ( playingState.Turn == null )
				{
					TurnTime.Text = "0";
				}
				else
				{
					TimeSpan turnTimeSpan = TimeSpan.FromSeconds( playingState.Turn.TimeLeft );
					TurnTime.Text = turnTimeSpan.Seconds.ToString();
				}
			}
			else
			{
				StateTime.Text = "0:00";
				TurnTime.Text = "0";
			}
		}
	}
}

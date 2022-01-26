﻿using System.Collections.Generic;

namespace Grubs.Utils
{
	public static class GameConfig
	{
		public static bool IsDevMode { get; set; } = true;

		// Gameplay configuration.
		public static int WormCount { get; set; } = 2;
		public static int TurnDurationSeconds { get; set; } = 45;
		public static int TurnTimeRemainingAfterFired { get; set; } = 5;
		public static int MinimumPlayersToStart { get; set; } = 2;

		// Used to spawn a random crate using the library system
		// Library name, chance of spawning (0-1)
		public static Dictionary<string, float> CrateTypes => new()
		{
			{ "crate_tools", 0.2f },
			{ "crate_weapons", 0.1f },
			{ "crate_health", 0.3f },
		};
		public static int MaxActiveCrates { get; set; } = 5;

		// Worm configuration.
		public static float SecondsBetweenWormJumps { get; set; } = 2;

		public static string[] WormNames => new[]
		{
			"Froggy",
			"Balls",
			"Boggy",
			"Spicy",
			"Hot",
			"Pinky",
			"Perky",
			"Gumby",
			"Dick",
			"Panini"
		};

		// Classname, weapon quantity.
		public static Dictionary<string, int> LoadoutDefaults = new()
		{
			{ "BaseballBat", -1 },
			{ "Bazooka", -1 },
			{ "Grenade", -1 },
			{ "Railgun", -1 },
			{ "Shotgun", -1 },
			{ "Dynamite", -1 },
			{ "Uzi", -1 },
			{ "LandMine", -1 },
			{ "Minigun", -1 },
			{ "PetrolBomb", -1 },
			{ "Revolver", -1 },
			{ "AirStrike", -1 }
		};

		// Weapon Crate Drop Chances
		public static Dictionary<string, float> WeaponCrateDropChances = new()
		{
			{ "BaseballBat", 5f },
			{ "Railgun", 1f },
			{ "Shotgun", 5f },
			{ "Dynamite", 2f },
			{ "Uzi", 4f },
			{ "LandMine", 3f },
			{ "Minigun", 1f },
			{ "PetrolBomb", 2f },
			{ "Revolver", 3f }
		};

		// Tool Crate Drop Chances
		public static Dictionary<string, float> ToolCrateDropChances = new();
	}
}

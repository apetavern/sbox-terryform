﻿namespace Grubs.Terrain;

[Category( "Terrain" )]
public sealed class TerrainChunk
{
	public readonly TerrainMap Map;

	public bool[,] TerrainGrid { get; set; } = null!;
	public Vector3 Position { get; set; }

	public TerrainChunk xNeighbour = null!, yNeighbour = null!, xyNeighbour = null!;
	public bool IsDirty { get; set; } = false;

	public TerrainChunk( Vector3 position, TerrainMap map )
	{
		Map = map;
		Position = position;
	}
}

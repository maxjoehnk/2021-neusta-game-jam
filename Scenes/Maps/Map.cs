using Godot;

public class Map : TileMap
{
    private Position2D CatSpawnNode => GetNode<Position2D>("CatSpawn");
    private Position2D MouseSpawnNode => GetNode<Position2D>("MouseSpawn");

    public Vector2 CatSpawn => CatSpawnNode.Position;
    public Vector2 MouseSpawn => MouseSpawnNode.Position;
}
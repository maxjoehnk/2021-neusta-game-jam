using Godot;
using System;

public class Game : Node
{
    private Viewport CatViewport => GetNode<Viewport>("Viewports/CatView/Viewport");
    private PlayerCamera CatCamera => GetNode<PlayerCamera>("Viewports/CatView/Viewport/Camera");
    private Viewport MouseViewport => GetNode<Viewport>("Viewports/MouseView/Viewport");
    private PlayerCamera MouseCamera => GetNode<PlayerCamera>("Viewports/MouseView/Viewport/Camera");
    private Cat Cat => GetNode<Cat>("Viewports/CatView/Viewport/Cat");
    private PlayerOverlay CatOverlay => GetNode<PlayerOverlay>("CatOverlay");
    private Mouse Mouse => GetNode<Mouse>("Viewports/CatView/Viewport/Mouse");
    private PlayerOverlay MouseOverlay => GetNode<PlayerOverlay>("MouseOverlay");

    private TileMap Map => GetNode<TileMap>("Viewports/CatView/Viewport/Map");

    private CanvasModulate CanvasTint => GetNode<CanvasModulate>("CanvasModulate");
    private TextureRect Clock => GetNode<TextureRect>("MainOverlay/Clock");
    
    private float clockRotation = 180;
    
    public override void _Ready()
    {
        MouseViewport.World2d = CatViewport.World2d;
        CatCamera.Target = Cat;
        MouseCamera.Target = Mouse;
        SetupCameraLimits();
    }

    public override void _Process(float delta)
    {
        bool isNight = UpdateClock(delta);
        Cat.IsHunting = isNight;
        Mouse.IsHunting = !isNight;
        
        UpdatePlayerHUD(Cat, CatOverlay);
        UpdatePlayerHUD(Mouse, MouseOverlay);
    }

    private bool UpdateClock(float delta)
    {
        const int ClockRotationSpeed = 5;
        this.clockRotation += delta * ClockRotationSpeed;
        if (this.clockRotation > 360)
        {
            this.clockRotation -= 360;
        }

        bool isNight = this.clockRotation > 180;

        CanvasTint.Color = isNight ? Colors.MidnightBlue : Colors.Orange;
        CanvasTint.Color = CanvasTint.Color.Lightened(0.7f);
        Clock.RectRotation = this.clockRotation;

        return isNight;
    }

    private void UpdatePlayerHUD(Player player, PlayerOverlay hud)
    {
        hud.SetHunting(player.IsHunting);
        hud.SetLives(player.Lives);
    } 

    private void SetupCameraLimits()
    {
        Rect2 mapLimits = Map.GetUsedRect();
        Vector2 mapCellSize = Map.CellSize;

        ApplyLimits(CatCamera, mapLimits, mapCellSize);
        ApplyLimits(MouseCamera, mapLimits, mapCellSize);
    }

    private void ApplyLimits(Camera2D camera, Rect2 limits, Vector2 cellSize)
    {
        camera.LimitLeft = (int)Math.Floor(limits.Position.x * cellSize.x);
        camera.LimitRight = (int)Math.Floor(limits.End.x * cellSize.x);
        camera.LimitTop = (int)Math.Floor(limits.Position.y * cellSize.y);
        camera.LimitBottom = (int)Math.Floor(limits.End.y * cellSize.y);
    }
}

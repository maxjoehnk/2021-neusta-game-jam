using System;

using Godot;

using JetBrains.Annotations;

public class Game : Node
{
    private const float InitialClockRotation = 180;

    private Viewport CatViewport => GetNode<Viewport>("Viewports/CatView/Viewport");
    private PlayerCamera CatCamera => GetNode<PlayerCamera>("Viewports/CatView/Viewport/Camera");
    private Viewport MouseViewport => GetNode<Viewport>("Viewports/MouseView/Viewport");
    private PlayerCamera MouseCamera => GetNode<PlayerCamera>("Viewports/MouseView/Viewport/Camera");
    private Cat Cat => GetNode<Cat>("Viewports/CatView/Viewport/Cat");
    private PlayerOverlay CatOverlay => GetNode<PlayerOverlay>("Overlays/CatOverlay");
    private Mouse Mouse => GetNode<Mouse>("Viewports/CatView/Viewport/Mouse");
    private PlayerOverlay MouseOverlay => GetNode<PlayerOverlay>("Overlays/MouseOverlay");

    private Map Map => GetNode<Map>("Viewports/CatView/Viewport/Map");

    private CanvasModulate CanvasTint => GetNode<CanvasModulate>("CanvasModulate");
    private Sprite Clock => GetNode<Sprite>("ClockOverlay/Clock");
    private GameFinishedOverlay WinnerScreen => GetNode<GameFinishedOverlay>("Overlays/GameFinishedOverlay");

    private bool IsNight => this.clockRotation > 180;

    private float clockRotation = InitialClockRotation;

    private GameState state = GameState.Playing;
    private PlayerRole? winner;

    public override void _Ready()
    {
        this.MouseViewport.World2d = this.CatViewport.World2d;
        this.CatCamera.Target = this.Cat;
        this.MouseCamera.Target = this.Mouse;
        this.Mouse.Connect(nameof(Player.PlayerHit), this, nameof(this.OnPlayerGotHit));
        this.Cat.Connect(nameof(Player.PlayerHit), this, nameof(this.OnPlayerGotHit));
        this.WinnerScreen.Connect(nameof(GameFinishedOverlay.RestartGame), this, nameof(this.RestartGame));
        SetupCameraLimits();
        // HACK[max]: we run into hit detection once at the start of the game. This should be replaced by PlacePlayers
        RestartGame();
    }

    public override void _Process(float delta)
    {
        UpdateClock(delta);
        UpdatePlayers();

        UpdateGameState();
        ApplyGameState();
    }

    [UsedImplicitly]
    public void RestartGame()
    {
        this.RemoveProjectiles();
        this.Cat.Reset();
        this.Cat.Lives = 3;
        this.Mouse.Reset();
        this.Mouse.Lives = 3;
        this.clockRotation = InitialClockRotation;
        this.state = GameState.Playing;
        this.winner = null;
        this.PlacePlayers();
        this.UpdatePlayers();
        this.ApplyGameState();
    }

    private void RemoveProjectiles()
    {
        this.GetTree().CallGroup("projectiles", "queue_free");
    }

    private void ResetPlayers()
    {
        this.Cat.Reset();
        this.Mouse.Reset();
        this.PlacePlayers();
    }

    private void PlacePlayers()
    {
        Cat.Position = Map.CatSpawn;
        Mouse.Position = Map.MouseSpawn;
    }

    private void UpdatePlayers()
    {
        Cat.IsHunting = IsNight;
        Mouse.IsHunting = !IsNight;

        UpdatePlayerHUD(CatOverlay, Cat);
        UpdatePlayerHUD(MouseOverlay, Mouse);
    }

    private void UpdateGameState()
    {
        if (this.Cat.Lives == 0 || this.Mouse.Lives == 0)
        {
            this.state = GameState.WinnerScreen;
        }

        if (this.Cat.Lives == 0)
        {
            this.winner = PlayerRole.Mouse;
        }

        if (this.Mouse.Lives == 0)
        {
            this.winner = PlayerRole.Cat;
        }
    }

    private void ApplyGameState()
    {
        this.ApplyPlayingState();
        this.ApplyWinnerScreenState();
    }

    private void ApplyPlayingState()
    {
        if (this.state != GameState.Playing)
        {
            return;
        }

        GetTree().Paused = false;
        this.CatOverlay.Show();
        this.MouseOverlay.Show();
        this.Clock.Show();
        this.WinnerScreen.Hide();
    }

    private void ApplyWinnerScreenState()
    {
        if (this.state != GameState.WinnerScreen)
        {
            return;
        }

        GetTree().Paused = true;
        this.CatOverlay.Hide();
        this.MouseOverlay.Hide();
        this.Clock.Hide();
        this.WinnerScreen.Show();
        this.WinnerScreen.SetWinner((PlayerRole)this.winner);
    }

    private void UpdateClock(float delta)
    {
        const int ClockRotationSpeed = 5;
        this.clockRotation += delta * ClockRotationSpeed;
        if (this.clockRotation > 360)
        {
            this.clockRotation -= 360;
        }

        CanvasTint.Color = IsNight ? Colors.MidnightBlue : Colors.Orange;
        CanvasTint.Color = CanvasTint.Color.Lightened(0.7f);
        Clock.RotationDegrees = this.clockRotation;
    }

    private void UpdatePlayerHUD(PlayerOverlay hud, Player player)
    {
        hud.SetHunting(player.IsHunting);
        hud.SetLives(player.Lives);
        hud.SetSpecialCooldown(player.SpecialCooldown);
        hud.SetAttackCooldown(player.AttackCooldown);
        hud.StatusEffects = player.GetStatusEffects();
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

    public void OnPlayerGotHit()
    {
        this.ResetPlayers();
    }
}
using Godot;
using FarmGame.Tools;

namespace FarmGame.Player;

public partial class ToolWheel : Control
{
    [Signal]
    public delegate void ToolSelectedEventHandler(ToolData tool);

    [Export]
    public float Radius = 80f;

    [Export]
    public float Deadzone = 30f; // Bereich um die Mitte = "kein Werkzeug"

    private ToolData[] _tools;
    private int _hoveredIndex = -1; // -1 = kein Werkzeug (Mitte)

    public override void _Ready()
    {
        Visible = false;
        MouseFilter = MouseFilterEnum.Ignore;
    }

    public void Open(ToolData[] tools)
    {
        _tools = tools;
        _hoveredIndex = -1;
        GlobalPosition = GetGlobalMousePosition() - Size / 2f;
        Visible = true;
        QueueRedraw();
    }

    public void Close()
    {
        Visible = false;
        _tools = null;
    }

    public override void _Process(double delta)
    {
        if (!Visible || _tools == null || _tools.Length == 0)
            return;

        Vector2 center = GlobalPosition + Size / 2f;
        Vector2 toMouse = GetGlobalMousePosition() - center;

        if (toMouse.Length() < Deadzone)
        {
            _hoveredIndex = -1;
        }
        else
        {
            // Gleicher Versatz wie beim Zeichnen (Segmente starten oben, -90°)
            float angle = toMouse.Angle() + Mathf.Pi / 2f;
            angle = Mathf.PosMod(angle, Mathf.Tau);

            float segment = Mathf.Tau / _tools.Length;
            _hoveredIndex = (int)(angle / segment) % _tools.Length;
        }

        QueueRedraw();
    }

    public override void _Draw()
    {
        if (_tools == null || _tools.Length == 0)
            return;

        Vector2 center = Size / 2f;
        float segment = Mathf.Tau / _tools.Length;

        for (int i = 0; i < _tools.Length; i++)
        {
            float startAngle = i * segment - Mathf.Pi / 2f;
            Color color = (i == _hoveredIndex)
                ? new Color(1f, 1f, 1f, 0.9f)
                : new Color(1f, 1f, 1f, 0.4f);

            DrawArc(center, Radius, startAngle, startAngle + segment, 32, color, 40f, true);

            Vector2 labelPos = center + new Vector2(
                Mathf.Cos(startAngle + segment / 2f),
                Mathf.Sin(startAngle + segment / 2f)
            ) * Radius;

            DrawString(ThemeDB.FallbackFont, labelPos, _tools[i].ToolName,
                HorizontalAlignment.Center, -1, 16);
        }

        // Mitte = "kein Werkzeug"
        Color centerColor = (_hoveredIndex == -1)
            ? new Color(1f, 0.3f, 0.3f, 0.9f)
            : new Color(1f, 1f, 1f, 0.3f);
        DrawCircle(center, Deadzone, centerColor);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!Visible)
            return;

        if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Right && !mb.Pressed)
        {
            ToolData selected = (_tools != null && _hoveredIndex >= 0 && _hoveredIndex < _tools.Length)
                ? _tools[_hoveredIndex]
                : null;

            EmitSignal(SignalName.ToolSelected, selected as Resource);
            Close();
        }
    }
}
namespace GXPEngine;

public class MovableHitbox : Sprite
{
    private bool held;
    private Vec2 relative;

    private EasyDraw canvas;

    private Vec2 mousePos;


    protected MovableHitbox(string filePath) : base(filePath)
    {
        canvas = new EasyDraw(width, height);
        // AddChild(canvas);
    }

    protected void Update()
    {
        mousePos = new Vec2(Input.mouseX, Input.mouseY);
        
        if (Input.GetMouseButtonDown(0) && HitTestPoint(mousePos.x, mousePos.y))
        {
            held = true;
            relative = new Vec2(mousePos.x - x, mousePos.y - y);
        }
        else if (Input.GetMouseButtonUp(0)) held = false;

        if (held)
        {
            SetXY(mousePos.x - relative.x, mousePos.y - relative.y);
            canvas.ClearTransparent();
            canvas.Text($"{x}, {y}");
        }

        if (Input.GetMouseButtonDown(1) && HitTestPoint(mousePos.x, mousePos.y))
        {
            rotation = 45;
        }

    }
}
using System.Drawing;
using GXPEngine.BodyParts;
using GXPEngine.Core;
using GXPEngine.StageManagement;

namespace GXPEngine;

public class Hook : Sprite
{
    private readonly Vec2 velocity;
    private readonly MyGame myGame;
    public bool hasHit;

    private Collision collision;
    
    public Hook(float x, float y, Vec2 velocity_) : base("bodyParts/test/red/hook.png")
    {
        myGame = (MyGame) game;
        if (StageLoader.currentStage != null) parent = StageLoader.currentStage;
        SetXY(x,y);
        SetOrigin(width/2.0f,height/2.0f);
        collider.isTrigger = true;
        velocity = velocity_;

        hasHit = false;
    }

    private void Update()
    {
        if (hasHit) return;


        x += velocity.x * Time.deltaTime;
        y += velocity.y * Time.deltaTime;
        
        
        
        foreach (Sprite sprite in StageLoader.currentStage.grappleSurfaces.GetChildren())
        {
            if (HitTest(sprite))
            {
                hasHit = true;
                return;
            }
        }

        if (!hasHit)
        {
            foreach (Hitbox hitbox in StageLoader.currentStage.surfaces.GetChildren())
            {
                if (HitTest(hitbox))
                {
                    Kill();
                }
            }
        }
        
       
            
            
       

        if (x > StageLoader.currentStage.stageWidth + width || x < -width || y > StageLoader.currentStage.stageHeight + height || y < -height)
        {
            Kill();
        }
    }

    private void Kill()
    {
        GrapplingHook? grapplingHook = (GrapplingHook) myGame.player.upperBodyPart!;
        grapplingHook.hook = null;
        StageLoader.currentStage.background.Clear(Color.LightCyan);
        Destroy();
    }
}
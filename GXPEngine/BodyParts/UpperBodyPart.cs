using GXPEngine.GXPEngine.Core;

namespace GXPEngine.BodyParts;

public abstract class UpperBodyPart : BodyPart
{
    protected AnimationSprite? abilityModel;
    protected bool abilityUsable;
    protected int timeOfUsage;

    protected Vec2 abilityDirection;

    protected UpperBodyPart(string modelPath, int cols, int rows, int frames, Player player_) : base(modelPath, cols,
        rows, frames, player_)
    {
        abilityDirection = new Vec2(0, 0);
        abilityUsable = true;
    }

    protected override void Update()
    {
        abilityModel?.Animate(Time.deltaTime);

        if (abilityModel != null)
        {
          
            
            if (Input.GetMouseButtonDown(0))
            {
                UseAbility();
                abilityUsable = false;
                timeOfUsage = Time.now;
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelAbility();
            }
        }
    }

    protected virtual void RotateToMouse()
    {
        float newRotation = abilityDirection.GetAngleDegrees();
        abilityModel.rotation = newRotation;
    }

    public override void UpdatePosition()
    {
        SetXY(player.x,player.y);
    }

    protected void SetAbilityModel(string modelPath, int cols, int rows, int frames, bool addCollider_ = false, int rotation_ = 370)
    {
        abilityModel = new AnimationSprite(modelPath, cols, rows, frames, true,addCollider_);

        if (addCollider_) abilityModel.collider.isTrigger = true;
        
        abilityModel.SetXY(x + MyGame.playerBaseSize.x / 2, y + MyGame.partBaseSize.y/2);
        abilityModel.SetOrigin(MyGame.playerBaseSize.x/4, MyGame.partBaseSize.y/2);
        
        abilityDirection = MyGame.mousePos - new Vec2(x + MyGame.playerBaseSize.x / 2, y + MyGame.partBaseSize.y);

        abilityModel.rotation = _rotation > 360 ? Vec2.WrapAroundDegree(abilityDirection.GetAngleDegrees()) : _rotation;
        
        AddChild(abilityModel);


    }

    protected virtual void UseAbility(){}
    protected virtual void CancelAbility(){}

}
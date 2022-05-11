namespace GXPEngine;

public class Hitbox : Sprite
{
    public bool canGrappleOnto;
    public bool climbable;
    
    public Hitbox(bool canGrappleOnto_ = false, bool climbable_ = false) : base("hitboxes/nothing.png", true)
    {
        canGrappleOnto = canGrappleOnto_;
        climbable = climbable_;
    }
}
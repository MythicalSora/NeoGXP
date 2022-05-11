//In this file are all classes for the different body parts

using System;
using System.Drawing;
using GXPEngine.BreakableStuffs;
using GXPEngine.Core;
using GXPEngine.StageManagement;

namespace GXPEngine.BodyParts
{
    
    
    //Red
    public class JumpingLegs : LowerBodyPart {
        public JumpingLegs(Player player_) : base("bodyParts/test/red/lower.png", 1, 1, 1, player_)
        {
            jumpMultiplier = 1;
            speedMultiplier = 1;

            speed = speedMultiplier * MyGame.globalSpeed;
        }
    }

    public class GrapplingHook : UpperBodyPart
    {
        private readonly float grapplePower;
        public Hook? hook;
        private bool pulling;
        private readonly float pullStartingPower;
        private float pullPower;
        private readonly float pullPowerIncrement;
        private readonly float maxPullPower;
        private int shootTime;
        
        public GrapplingHook(Player player_) : base("bodyParts/test/red/upper.png", 1, 1, 1, player_)
        {
            SetAbilityModel("bodyParts/test/red/ability.png",1,1,1, addCollider_:true);

            grapplePower = 1.5f;
            pullPower = 0.05f;
            pullStartingPower = MyGame.globalSpeed * pullPower;
            
            pullPowerIncrement = 0.01f;
            maxPullPower = 1;
            


            hook = null;
            pulling = false;
        }

        protected override void UseAbility()
        {
            if (abilityModel == null) return;

            if (hook != null && !pulling && hook.hasHit)
            {
                pulling = true;
                if (player.lowerBodyPart != null) player.lowerBodyPart.disableKeyAndGravityMovement = true;
            }
            else if (hook == null)
            {
                pulling = false;

                shootTime = Time.now;

                Vec2 newPosition = new Vec2(player.x + abilityModel.x,player.y + abilityModel.y);
                
              
                hook = new Hook(newPosition.x,newPosition.y, abilityDirection.SetMagnitude(grapplePower * MyGame.globalSpeed))
                {
                    rotation = abilityModel.rotation,
                };
            }
        }

        protected override void CancelAbility()
        {
            pulling = false;
            hook?.LateDestroy();
            hook = null;
            player.lowerBodyPart?.CheckIfGrounded();

            if (!player.isGrounded) player.currentState = Player.State.Jump;
            
            player.lowerBodyPart!.disableKeyAndGravityMovement = false;
            
            StageLoader.currentStage?.background.Clear(Color.LightCyan);
        }

        protected override void Update()
        {
            abilityModel?.Animate(Time.deltaTime);

            if (abilityModel != null)
            {
                abilityDirection = MyGame.mousePos - new Vec2(x + MyGame.playerBaseSize.x / 2, y + MyGame.partBaseSize.y);
                RotateToMouse();

          
            
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
                else if (hook == null)
                {
                    RotateToMouse();
                }
            }
            
            if (hook != null)
            {
                Vec2 direction = new Vec2(hook.x,hook.y) - new Vec2(player.x + MyGame.partBaseSize.x / 2.0f,
                    player.y + MyGame.partBaseSize.y);

                // Vec2 direction = hook.velocity;
                
                
                
                
                StageLoader.currentStage?.background.Clear(Color.LightCyan);
                StageLoader.currentStage?.background.Stroke(Color.Brown);
                
                if (abilityModel != null)
                {

                    StageLoader.currentStage?.background.Line(x + abilityModel.x, y + abilityModel.y, hook.x, hook.y);
                    
                    if (!hook.HitTest(player) && !hook.HitTest(this))
                    {
                        abilityModel.rotation = direction.GetAngleDegrees();
                    }
                    
                }


                if (!hook.hasHit)
                {
                    if (direction.Magnitude() > 400) CancelAbility();
                }


                if (pulling)
                {
                    if (direction.Magnitude() < 20 || direction.y > 0)
                    {
                        CancelAbility();
                        return;
                    }

                    player.velocity = direction.SetMagnitude(pullPower);

                    if (pullPower < maxPullPower)
                    {
                        pullPower += pullPowerIncrement * Time.deltaTime;
                    }

                    if (player.horizontalCollision != null || player.verticalCollision != null)
                    {
                        CancelAbility();
                    }
                }
            }
        }


        protected override void RotateToMouse()
        {
            if (hook == null) base.RotateToMouse();
        }
    }

    //Blue
    public class ExtendyLegs : LowerBodyPart
    {
        private readonly int extendSpeed;
        private readonly float crouchIntensity;
        private readonly int maxExtendiness;
        
        public ExtendyLegs(Player player_) : base("bodyParts/test/blue/lower.png", 1, 1, 1, player_)
        {
            jumpMultiplier = 0;
            speedMultiplier = 1;
            speed = MyGame.globalSpeed * speedMultiplier;
            extendSpeed = 2;
            crouchIntensity = 0.2f;

            maxExtendiness = (int) MyGame.playerBaseSize.y * 6;
        }

        public override void HandleMovement()
        {
            base.HandleMovement();

            Collision? boundaryCollision;

            
            // Console.WriteLine($"State: {player.currentState}");
            // Console.WriteLine($"VerticalCol: {player.verticalCollision?.other}");
            // Console.WriteLine($"HorizontalCol: {player.horizontalCollision?.other.x}");


            if (Input.GetKey(Key.UP) && player.height < maxExtendiness)
            {
                boundaryCollision = player.MoveUntilCollision(0, -extendSpeed, StageLoader.currentStage?.surfaces.GetChildren()!);
                if (boundaryCollision == null)
                {
                    player.height += extendSpeed;
                    model.height += extendSpeed;
                }
            }
            else if (Input.GetKey(Key.DOWN))
            {
                boundaryCollision = player.MoveUntilCollision(0,extendSpeed, StageLoader.currentStage?.surfaces.GetChildren()!);
                if (boundaryCollision is {normal: {y: < -0.5f}} && player.height > MyGame.partBaseSize.y*(1+crouchIntensity))
                {
                    player.height -= extendSpeed;
                    model.height -= extendSpeed;
                }


            }
        }
    }

    public class StrongArm : UpperBodyPart
    {
        public StrongArm(Player player_) : base("bodyParts/test/blue/upper.png", 1, 1, 1, player_)
        {
            SetAbilityModel("bodyParts/test/blue/ability.png",1,1,1, true,player.mirrored?180:360);
        }
        protected override void UseAbility()
        {
            foreach (Breakable breakableBlock in StageLoader.currentStage.breakableBlocks.GetChildren())
            {
                if (abilityModel.HitTest(breakableBlock)) breakableBlock.Break(); 
            }
        }

        protected override void Update()
        {
            base.Update();
            

            abilityModel.rotation = player.mirrored ? 180 : 360;
        }
    }

    //Green
    public class SpiderLegs : LowerBodyPart
    {
        private bool inSpiderForm;
        private float climbSpeed;

        public SpiderLegs(Player player_) : base("bodyParts/test/green/lower.png", 1, 1, 1, player_)
        {
            jumpMultiplier = 0;
            inSpiderForm = false;
            speedMultiplier = 1;

            speed = MyGame.globalSpeed * speedMultiplier;

            const float climbMultiplier = 0.3f;
            climbSpeed = climbMultiplier * speed;

        }

        public override void HandleMovement()
        {

            inSpiderForm = false;

            foreach (Hitbox hitbox in StageLoader.currentStage.climbableSurfaces.GetChildren())
            {
                if (hitbox.climbable)
                {
                    if (player.HitTest(hitbox))
                    {
                        inSpiderForm = true;
                    }

                    // if (Input.GetKeyDown(Key.W) || Input.GetKeyDown(Key.S))
                    // {
                    //     climbKeyPressed = true;
                    // }
                }
            }
            
            // if (inSpiderForm == false && climbKeyPressed)
            // {
            //     climbKeyPressed = false;
            //     CheckIfGrounded();
            // }
            
            // if (!climbKeyPressed) base.HandleMovement();

            if (!inSpiderForm) base.HandleMovement();

            // if (inSpiderForm && climbKeyPressed)
            if (inSpiderForm)
            {
                if (Input.GetKey(Key.W))
                {
                    player.MoveUntilCollision(0, -climbSpeed * Time.deltaTime, StageLoader.currentStage.surfaces.GetChildren());
                }

                if (Input.GetKey(Key.A))
                {
                    player.MoveUntilCollision(-climbSpeed * Time.deltaTime, 0, StageLoader.currentStage.surfaces.GetChildren());
                    player.mirrored = true;
                }

                if (Input.GetKey(Key.D))
                {
                    player.MoveUntilCollision(climbSpeed * Time.deltaTime, 0, StageLoader.currentStage.surfaces.GetChildren());
                    player.mirrored = false;
                }

                if (Input.GetKey(Key.S))
                {
                    player.MoveUntilCollision(0, climbSpeed * Time.deltaTime, StageLoader.currentStage.surfaces.GetChildren());
                }
            }
            
        }
    }

    public class GreenUpperBodyPart : UpperBodyPart
    {
        public GreenUpperBodyPart(Player player_) : base("bodyParts/test/green/upper.png", 1, 1, 1, player_)
        {
            SetAbilityModel("bodyParts/test/green/ability.png",1,1,1);
            
            
            
            
        }
        
        protected override void UseAbility()
        {
        }
    }
    
    //PlaceHolder
    public class PlaceHolderLowerBodyPart : LowerBodyPart {public PlaceHolderLowerBodyPart(Player player_) : base("bodyParts/placeHolder/lower.png", 1, 1, 1, player_){}}

    public class PlaceHolderUpperBodyPart : UpperBodyPart
    {
        public PlaceHolderUpperBodyPart(Player player_) : base("bodyParts/placeHolder/upper.png", 1, 1, 1, player_){}
        protected override void UseAbility()
        {
        }
    }
}


namespace GXPEngine.BodyParts;

public abstract class BodyPart : GameObject
{
    public AnimationSprite model;
    protected Player player;
    
    protected BodyPart(string modelPath, int cols, int rows, int frames, Player player_)
    {
        player = player_;
        // parent = StageLoader.currentStage;
        model = new AnimationSprite(modelPath, cols, rows, frames, true, false);
        AddChild(model);
    }

    protected abstract void Update();
    public abstract void UpdatePosition();
    protected void SetModel(string modelPath, int cols, int rows, int frames)
    {
        model = new AnimationSprite(modelPath, cols, rows, frames, true, false);
    }
}
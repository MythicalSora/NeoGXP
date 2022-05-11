using GXPEngine.StageManagement;
namespace GXPEngine.BreakableStuffs;

public class WallNormal : Breakable
{
	public WallNormal(int pX, int pY) : base(pX, pY - 48, "placeholders/1x4-wall.png") {}

	protected override void MakeAnimation()
	{
		for (int i = 0; i < 2; i++)
		{
			Animation animation = new ("destruction.png", 5, 4, 18, true, 50);
			animation.SetXY(x + 8, y + i*24);
			AddChild(animation);
			StageLoader.currentStage?.animations.AddChild(animation);
		}
	}
}

public class WallSmall : Breakable
{
	public WallSmall(int pX, int pY) : base(pX, pY, "placeholders/1x1-wall.png") {}

	protected override void MakeAnimation()
	{
		Animation animation = new ("destruction.png", 5, 4, 18, true, 50);
		animation.SetXY(x + 8, y - 24);
		AddChild(animation);
		StageLoader.currentStage?.animations.AddChild(animation);
	}
}

public class Pencil : Breakable
{
	public Pencil(int pX, int pY) : base(pX, pY - 48, "placeholders/1x4-pencil.png") {}
	
	protected override void MakeAnimation()
	{
		for (int i = 0; i < 2; i++)
		{
			Animation animation = new ("destruction.png", 5, 4, 18, true, 50);
			animation.SetXY(x + 8, y + i*24);
			AddChild(animation);
			StageLoader.currentStage?.animations.AddChild(animation);
		}
	}
}

public class Painting : Breakable
{
	public Painting(int pX, int pY) : base(pX, pY - 114, "placeholders/3x8-painting.png") {}
	
	protected override void MakeAnimation()
	{
		Animation animation = new ("destruction.png", 5, 4, 18, true, 50);
		animation.SetXY(x + 32, y + 16);
		animation.SetScaleXY(2.0f);
		AddChild(animation);
		StageLoader.currentStage?.animations.AddChild(animation);
	}
}
using System;
using GXPEngine.StageManagement;
namespace GXPEngine.BreakableStuffs;

public class Breakable : Sprite
{
	private Hitbox hitbox;

	protected (string path, int cols, int rows, int frames, float animationDelay) breakAnimationInfo;

	
	public Breakable(int pX, int pY, string filePath = "placeholders/colors.png") : base(filePath)
	{
		SetXY(pX,pY);

		breakAnimationInfo = ("", -1, -1, -1, -1);
		
		// Console.WriteLine($"{x}, {y}");
		hitbox = new Hitbox();		
		hitbox.SetXY(x,y);
		hitbox.width = width;
		hitbox.height = height;
		StageLoader.currentStage?.surfaces.AddChild(hitbox);

		
		collider.isTrigger = true;
		
		
		
	}

	public void Break()
	{
		// (string path, int cols, int rows, int frames, float animationDelay) = breakAnimationInfo;
		//
		// if (path =="" || cols < 0 || rows < 0 || frames < 0 || animationDelay < 0)
		// {
		// 	breakAnimationInfo = ("placeholders/placeHolders.png", 4, 1, 4, 255);
		// 	( path,  cols,  rows,  frames,  animationDelay) = breakAnimationInfo;
		// }
		// 	
		// Animation animation = new (path, cols, rows, frames, true, (byte)animationDelay);
		// animation.SetXY(x,y);
		// StageLoader.currentStage?.animations.AddChild(animation);
		
		MakeAnimation();
		
		StageLoader.currentStage?.breakableBlocks.RemoveChild(hitbox);
		hitbox.Destroy();
		LateDestroy();
	}

	protected virtual void MakeAnimation()
	{
		Animation animation = new ("placeholders/placeHolders.png", 4, 1, 4, true, 255);
		animation.SetXY(x,y);
		StageLoader.currentStage?.animations.AddChild(animation);
	}
}
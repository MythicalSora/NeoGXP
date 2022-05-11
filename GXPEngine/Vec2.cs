using System;
using GXPEngine.GXPEngine.Utils;

namespace GXPEngine
{
	public struct Vec2 
	{
		public const float PI         = (float) Math.PI;
		public const float HALF_PI    = (float) (Math.PI / 2.0);
		public const float THIRD_PI   = (float) (Math.PI / 3.0);
		public const float QUARTER_PI = (float) (Math.PI / 4.0);
		public const float TWO_PI     = (float) (2.0 * Math.PI);

		public float x;
		public float y;

		private const float TOLERANCE = 0.001f;

		public Vec2 (float pX = 0, float pY = 0) 
		{
			x = pX;
			y = pY;
		}

		public Vec2(Core.Vector2 vector2)
		{
			x = vector2.x;
			y = vector2.y;
		}

		// ReSharper disable once InconsistentNaming
		public Vec2 SetXY(float newX, float newY)
		{
			x = newX;
			y = newY;
			return this;
		}

//Vector information

		public Vec2 UnitNormal()
		{
			return Normal().Normalized();
		}

		public Vec2 Normal()
		{
			return new Vec2(-y, x);
		}

		public Vec2 Reflect(Vec2 normal, float bounciness = 1)
		{
			this -= (1 + bounciness) * (Dot(normal)) * normal;

			return this;
		}


		public float Magnitude()
		{
			return x == 0 && y == 0 ? 0 : x == 0 ? Mathf.Abs(y) : y == 0 ? Mathf.Abs(x) : Mathf.Sqrt((x * x) + (y * y));
		}
	
		public Vec2 Normalize()
		{
			float length = Magnitude();

			if (length == 0) return this;
			x /= length;
			y /= length;

			return this;
		}


		public Vec2 Normalized()
		{
			return new Vec2(x, y).Normalize();
		}

		public Vec2 SetMagnitude(float magnitude)
		{
			Normalize();
			this *= magnitude;

			return this;
		}
		
		
		public Vec2 SetAngleDegrees(float degrees)
		{
			SetAngleRadians(Deg2Rad(degrees));

			return this;
		}

		public Vec2 SetAngleRadians(float radians)
		{
			float magnitude = Magnitude();
			x = Mathf.Cos(radians) * magnitude;
			y = Mathf.Sin(radians) * magnitude;

			return this;
		}

		public float GetAngleRadians()
		{
			return Mathf.Atan2(y, x);
		}

		public float GetAngleDegrees()
		{
			return Rad2Deg(GetAngleRadians());
		}

		public Vec2 RotateRadians(float radians)
		{
			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);

			float prevX = x;
			float prevY = y;
			
			x = prevX * cos - prevY * sin;
			y = prevX * sin + prevY * cos;

			return this;
		}

		public Vec2 RotateDegrees(float degrees)
		{
			RotateRadians(Deg2Rad(degrees));

			return this;
		}

		public Vec2 RotateAroundRadians(float pointX, float pointY, float radians)
		{
			SetXY(x - pointX, y - pointY);
			RotateRadians(radians);
			SetXY(x + pointX, y + pointY);

			return this;
		}

		public Vec2 RotateAroundDegrees(float pointX, float pointY, float degrees)
		{
			RotateAroundRadians(pointX,pointY,Deg2Rad(degrees));

			return this;
		}

		public float Dot(Vec2 other)
		{
			return (x * other.x + y * other.y);
		}
		
		public static float WrapAroundDegree(float angle)
		{
			return ((angle % 360) + 360) % 360; // returns a number between 0 and 360  
		}

		public static float WrapAroundRadians(float angle)
		{
			const float range = 2 * Mathf.PI;
			return ((angle % range) + range) % range;
		}
		
		
		//Static radial functions

		public static float Deg2Rad(float deg)
		{
			return deg * (Mathf.PI / 180);
		}

		public static float Rad2Deg(float rad)
		{
			return rad * (180 / Mathf.PI);
		}

		public static Vec2 GetUnitVectorDeg(float deg)
		{
			float rad = Deg2Rad(deg);

			return new Vec2(Mathf.Cos(rad), Mathf.Sin(rad));
		}

		public static Vec2 GetUnitVectorRad(float rad)
		{
			return new Vec2(Mathf.Cos(rad), Mathf.Sin(rad));
		}

		//Can't be unit tested
		public static Vec2 RandomUnitVector()
		{
			float deg = Utils.Random(0,360);
			return new Vec2(Mathf.Cos(deg), Mathf.Sin(deg));
		}
		
		public static float AngleDifference(float angle1, float angle2)
		{
			float diff = (angle2 - angle1 + 180) % 360 - 180; // output is always between -540 and 180
			return diff < -180 ? diff + 360 : diff; // not it's always between -180 and 180 (while still being equivalent to the original angle!)
		}






//Operators
		
		public static Vec2 operator+ (Vec2 left, Vec2 right) {
			return new Vec2(left.x+right.x, left.y+right.y);
		}

		public static Vec2 operator -(Vec2 left, Vec2 right)
		{
			return new Vec2(left.x - right.x, left.y - right.y);
		}

		public static Vec2 operator *(Vec2 left, Vec2 right)
		{
			return new Vec2(left.x * right.x, left.y * right.y);
		}

		public static Vec2 operator *(Vec2 vec2, float multiplier)
		{
			return new Vec2(vec2.x * multiplier, vec2.y * multiplier);
		}

		public static Vec2 operator *(float multiplier, Vec2 vec2)
		{
			return new Vec2(vec2.x * multiplier, vec2.y * multiplier);
		}

		public static Vec2 operator /(Vec2 left, Vec2 right)
		{
			return new Vec2(left.x / right.x, left.y / right.y);
		}

		public static Vec2 operator /(Vec2 vec2, float divisor)
		{
			return new Vec2(vec2.x / divisor, vec2.y / divisor);
		}

		public static bool operator ==(Vec2 left, Vec2 right)
		{
			return Math.Abs(left.x - right.x) < TOLERANCE && Math.Abs(left.y - right.y) < TOLERANCE;
		}

		public static bool operator !=(Vec2 left, Vec2 right)
		{
			return !(left == right);
		}
		
	
		
		
//Haven't a clue		

		public override string ToString() 
		{
			return $"({x},{y})";
		}
		
		private bool Equals(Vec2 other)
		{
			return x.Equals(other.x) && y.Equals(other.y);
		}

		public override bool Equals(object obj)
		{
			return obj is Vec2 other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (x.GetHashCode() * 397) ^ y.GetHashCode();
			}
		}
	}
}


using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LoG
{
	// Token: 0x020001DA RID: 474
	public static class HexUtils
	{
		// Token: 0x06000946 RID: 2374 RVA: 0x0002BFD5 File Offset: 0x0002A1D5
		public static CubeCoord ToCubeCoord(in HexCoord hex)
		{
			return HexUtils.ToCubeCoord(hex, HexUtils.UseEvenOffsets);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0002BFE4 File Offset: 0x0002A1E4
		public static CubeCoord ToCubeCoord(in HexCoord hex, bool evenOffset)
		{
			int column = hex.column;
			int num = evenOffset ? (hex.row - (hex.column + (hex.column & 1)) / 2) : (hex.row - (hex.column - (hex.column & 1)) / 2);
			int num2 = -column - num;
			return new CubeCoord((float)column, (float)num2, (float)num);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0002C03C File Offset: 0x0002A23C
		public static HexCoord ToHexCoord(in CubeCoord cube)
		{
			return HexUtils.ToHexCoord(cube, HexUtils.UseEvenOffsets);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002C04C File Offset: 0x0002A24C
		public static HexCoord ToHexCoord(in CubeCoord cube, bool evenOffset)
		{
			CubeCoord cubeCoord = cube;
			CubeCoord normalized = cubeCoord.Normalized;
			int num = (int)Math.Round((double)normalized.q);
			int num2 = (int)Math.Round((double)normalized.s);
			int column = num;
			return new HexCoord(evenOffset ? (num2 + (num + (num & 1)) / 2) : (num2 + (num - (num & 1)) / 2), column);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002C0A1 File Offset: 0x0002A2A1
		public static CubeCoord WorldspaceCoordToHex(float x, float z, float size)
		{
			return HexUtils.WorldspaceCoordToHex(x, z, size, size);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002C0AC File Offset: 0x0002A2AC
		public static CubeCoord WorldspaceCoordToHex(float x, float z, float width, float height)
		{
			return HexUtils.WorldspaceCoordToHex(x, z, 0f, 0f, width, height);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002C0C1 File Offset: 0x0002A2C1
		public static CubeCoord WorldspaceCoordToHex(float x, float z, float offsetX, float offsetZ, float width, float height)
		{
			return HexUtils.WorldspaceCoordToHex(HexUtils.Orientation.Default, x, z, offsetX, offsetZ, width, height);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002C0D8 File Offset: 0x0002A2D8
		public static CubeCoord WorldspaceCoordToHex(in HexUtils.Orientation orientation, float x, float z, float offsetX, float offsetZ, float width, float height)
		{
			float num = (x - offsetX) / width;
			float num2 = (z - offsetZ) / height;
			float q = orientation.b0 * num + orientation.b1 * num2;
			float r = orientation.b2 * num + orientation.b3 * num2;
			return new CubeCoord(q, r).Normalized;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002C125 File Offset: 0x0002A325
		[return: TupleElementNames(new string[]
		{
			"x",
			"z"
		})]
		public static ValueTuple<float, float> ToWorldSpaceCoord(in CubeCoord coord, float size)
		{
			return HexUtils.ToWorldSpaceCoord(coord, size, size);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0002C12F File Offset: 0x0002A32F
		[return: TupleElementNames(new string[]
		{
			"x",
			"z"
		})]
		public static ValueTuple<float, float> ToWorldSpaceCoord(in CubeCoord coord, float width, float height)
		{
			return HexUtils.ToWorldSpaceCoord(coord, HexUtils.Orientation.Default, 0f, 0f, width, height);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0002C148 File Offset: 0x0002A348
		[return: TupleElementNames(new string[]
		{
			"x",
			"z"
		})]
		public static ValueTuple<float, float> ToWorldSpaceCoord(in CubeCoord coord, in HexUtils.Orientation orientation, float offsetX, float offsetZ, float width, float height)
		{
			float item = offsetX + (orientation.f0 * coord.q + orientation.f1 * coord.r) * width;
			float item2 = offsetZ + (orientation.f2 * coord.q + orientation.f3 * coord.r) * height;
			return new ValueTuple<float, float>(item, item2);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002C19C File Offset: 0x0002A39C
		public static IEnumerable<HexCoord> EnumerateRange(HexCoord start, int width, int height)
		{
			int num;
			for (int column = start.column; column < start.column + width; column = num + 1)
			{
				for (int row = start.row; row < start.row + height; row = num + 1)
				{
					yield return new HexCoord(row, column);
					num = row;
				}
				num = column;
			}
			yield break;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002C1BC File Offset: 0x0002A3BC
		public static HexDirection DirectionTo(this HexCoord fromHexCoord, HexCoord toHexCoord, float equalityTolerance = 2.7182817f)
		{
			CubeCoord cubeCoord = (CubeCoord)toHexCoord;
			CubeCoord cubeCoord2 = (CubeCoord)fromHexCoord;
			CubeCoord cubeCoord3 = cubeCoord - cubeCoord2;
			if (Math.Abs(cubeCoord3.q) <= 2.7182817f)
			{
				if (cubeCoord3.r < 0f && cubeCoord3.s > 0f)
				{
					return HexDirection.Down;
				}
				if (cubeCoord3.r > 0f && cubeCoord3.s < 0f)
				{
					return HexDirection.Up;
				}
			}
			if (Math.Abs(cubeCoord3.r) <= 2.7182817f)
			{
				if (cubeCoord3.q < 0f && cubeCoord3.s > 0f)
				{
					return HexDirection.DownLeft;
				}
				if (cubeCoord3.q > 0f && cubeCoord3.s < 0f)
				{
					return HexDirection.UpRight;
				}
			}
			if (Math.Abs(cubeCoord3.r) <= 2.7182817f)
			{
				if (cubeCoord3.q < 0f && cubeCoord3.r > 0f)
				{
					return HexDirection.UpLeft;
				}
				if (cubeCoord3.q > 0f && cubeCoord3.r < 0f)
				{
					return HexDirection.DownRight;
				}
			}
			throw new Exception(string.Format("Cannot determine Direction from ({0}, {1}) to ({2}, {3})", new object[]
			{
				fromHexCoord.column,
				fromHexCoord.row,
				toHexCoord.column,
				toHexCoord.row
			}));
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002C310 File Offset: 0x0002A510
		public static HexDirection GetOppositeDirection(this HexDirection direction)
		{
			switch (direction)
			{
			case HexDirection.Up:
				direction = HexDirection.Down;
				break;
			case HexDirection.UpRight:
				direction = HexDirection.DownLeft;
				break;
			case HexDirection.DownRight:
				direction = HexDirection.UpLeft;
				break;
			case HexDirection.Down:
				direction = HexDirection.Up;
				break;
			case HexDirection.DownLeft:
				direction = HexDirection.UpRight;
				break;
			case HexDirection.UpLeft:
				direction = HexDirection.DownRight;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return direction;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002C362 File Offset: 0x0002A562
		public static CubeCoord RotateCW(this CubeCoord pos)
		{
			return new CubeCoord(-pos.r, -pos.s, -pos.q);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0002C37E File Offset: 0x0002A57E
		public static CubeCoord RotateCCW(this CubeCoord pos)
		{
			return new CubeCoord(-pos.s, -pos.q, -pos.r);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002C39C File Offset: 0x0002A59C
		public static CubeCoord RotateToOrientation(this CubeCoord pos, HexUtils.HexOrientationFlags orientation)
		{
			if (orientation <= HexUtils.HexOrientationFlags.Rotate180)
			{
				switch (orientation)
				{
				case HexUtils.HexOrientationFlags.Default:
					return pos;
				case HexUtils.HexOrientationFlags.Rotate60:
					return new CubeCoord(-pos.r, -pos.s, -pos.q);
				case HexUtils.HexOrientationFlags.Default | HexUtils.HexOrientationFlags.Rotate60:
					break;
				case HexUtils.HexOrientationFlags.Rotate120:
					return new CubeCoord(pos.s, pos.q, pos.r);
				default:
					if (orientation == HexUtils.HexOrientationFlags.Rotate180)
					{
						return new CubeCoord(-pos.q, -pos.r, -pos.s);
					}
					break;
				}
			}
			else
			{
				if (orientation == HexUtils.HexOrientationFlags.Rotate240)
				{
					return new CubeCoord(pos.r, pos.s, pos.q);
				}
				if (orientation == HexUtils.HexOrientationFlags.Rotate300)
				{
					return new CubeCoord(-pos.s, -pos.q, -pos.r);
				}
			}
			throw new ArgumentOutOfRangeException("orientation", orientation, null);
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0002C488 File Offset: 0x0002A688
		public static CubeCoord RotateToOrientation2(this CubeCoord pos, HexUtils.HexOrientationFlags orientation)
		{
			if (orientation <= HexUtils.HexOrientationFlags.Rotate180)
			{
				switch (orientation)
				{
				case HexUtils.HexOrientationFlags.Default:
					return pos;
				case HexUtils.HexOrientationFlags.Rotate60:
					return pos.RotateCW();
				case HexUtils.HexOrientationFlags.Default | HexUtils.HexOrientationFlags.Rotate60:
					break;
				case HexUtils.HexOrientationFlags.Rotate120:
					return pos.RotateCW().RotateCW();
				default:
					if (orientation == HexUtils.HexOrientationFlags.Rotate180)
					{
						return pos.RotateCW().RotateCW().RotateCW();
					}
					break;
				}
			}
			else
			{
				if (orientation == HexUtils.HexOrientationFlags.Rotate240)
				{
					return pos.RotateCW().RotateCW().RotateCW().RotateCW();
				}
				if (orientation == HexUtils.HexOrientationFlags.Rotate300)
				{
					return pos.RotateCW().RotateCW().RotateCW().RotateCW().RotateCW();
				}
			}
			throw new ArgumentOutOfRangeException("orientation", orientation, null);
		}

		// Token: 0x04000481 RID: 1153
		private const float DefaultEqualityTolerance = 2.7182817f;

		// Token: 0x04000482 RID: 1154
		public const int NumEdges = 6;

		// Token: 0x04000483 RID: 1155
		public static bool UseEvenOffsets;

		// Token: 0x020008A5 RID: 2213
		public struct Orientation
		{
			// Token: 0x060028F8 RID: 10488 RVA: 0x00087824 File Offset: 0x00085A24
			public Orientation(float f0, float f1, float f2, float f3, float b0, float b1, float b2, float b3, float startingAngle)
			{
				this.f0 = f0;
				this.f1 = f1;
				this.f2 = f2;
				this.f3 = f3;
				this.b0 = b0;
				this.b1 = b1;
				this.b2 = b2;
				this.b3 = b3;
				this.startingAngle = startingAngle;
			}

			// Token: 0x040012DE RID: 4830
			public static readonly HexUtils.Orientation Flat = new HexUtils.Orientation(1.5f, 0f, (float)Math.Sqrt(3.0) / 2f, (float)Math.Sqrt(3.0), 0.6666667f, 0f, -0.33333334f, (float)Math.Sqrt(3.0) / 3f, 0f);

			// Token: 0x040012DF RID: 4831
			public static readonly HexUtils.Orientation Pointy = new HexUtils.Orientation((float)Math.Sqrt(3.0), (float)Math.Sqrt(3.0) / 2f, 0f, 1.5f, (float)Math.Sqrt(3.0) / 3f, -0.33333334f, 0f, 0.6666667f, 0.5f);

			// Token: 0x040012E0 RID: 4832
			public static HexUtils.Orientation Default = HexUtils.Orientation.Flat;

			// Token: 0x040012E1 RID: 4833
			public float f0;

			// Token: 0x040012E2 RID: 4834
			public float f1;

			// Token: 0x040012E3 RID: 4835
			public float f2;

			// Token: 0x040012E4 RID: 4836
			public float f3;

			// Token: 0x040012E5 RID: 4837
			public float b0;

			// Token: 0x040012E6 RID: 4838
			public float b1;

			// Token: 0x040012E7 RID: 4839
			public float b2;

			// Token: 0x040012E8 RID: 4840
			public float b3;

			// Token: 0x040012E9 RID: 4841
			public float startingAngle;
		}

		// Token: 0x020008A6 RID: 2214
		[Flags]
		public enum HexOrientationFlags
		{
			// Token: 0x040012EB RID: 4843
			Default = 1,
			// Token: 0x040012EC RID: 4844
			Rotate60 = 2,
			// Token: 0x040012ED RID: 4845
			Rotate120 = 4,
			// Token: 0x040012EE RID: 4846
			Rotate180 = 8,
			// Token: 0x040012EF RID: 4847
			Rotate240 = 16,
			// Token: 0x040012F0 RID: 4848
			Rotate300 = 32
		}
	}
}

using System;
using System.ComponentModel;
using Game.Simulation.Utils;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001D9 RID: 473
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public struct HexCoord : IEquatable<HexCoord>, IComparable<HexCoord>, IDontSerializeIfDefault
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0002BCF3 File Offset: 0x00029EF3
		[JsonIgnore]
		public float MagnitudeSqr
		{
			get
			{
				return (float)(this.row * this.row + this.column * this.column);
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0002BD11 File Offset: 0x00029F11
		[JsonIgnore]
		public float Magnitude
		{
			get
			{
				return (float)Math.Sqrt((double)this.MagnitudeSqr);
			}
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0002BD20 File Offset: 0x00029F20
		public HexCoord(int row = 0, int column = 0)
		{
			this.row = row;
			this.column = column;
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0002BD30 File Offset: 0x00029F30
		[JsonIgnore]
		public bool IsValid
		{
			get
			{
				return this != HexCoord.Invalid;
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002BD42 File Offset: 0x00029F42
		public static explicit operator CubeCoord(in HexCoord cube)
		{
			return HexUtils.ToCubeCoord(cube);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0002BD4A File Offset: 0x00029F4A
		public static HexCoord operator -(in HexCoord lhs, in HexCoord rhs)
		{
			return new HexCoord(lhs.row - rhs.row, lhs.column - rhs.column);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0002BD6B File Offset: 0x00029F6B
		public static HexCoord operator +(in HexCoord lhs, in HexCoord rhs)
		{
			return new HexCoord(lhs.row + rhs.row, lhs.column + rhs.column);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0002BD8C File Offset: 0x00029F8C
		public static bool operator ==(in HexCoord lhs, in HexCoord rhs)
		{
			HexCoord hexCoord = lhs;
			return hexCoord.Equals(rhs);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0002BDAD File Offset: 0x00029FAD
		public static bool operator !=(HexCoord lhs, HexCoord rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0002BDBB File Offset: 0x00029FBB
		public bool Equals(HexCoord other)
		{
			return this.column == other.column && this.row == other.row;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0002BDDC File Offset: 0x00029FDC
		public override bool Equals(object obj)
		{
			if (obj is HexCoord)
			{
				HexCoord other = (HexCoord)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0002BE01 File Offset: 0x0002A001
		public override int GetHashCode()
		{
			return this.column * 397 ^ this.row;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002BE16 File Offset: 0x0002A016
		public ulong VisualHashCode()
		{
			return (ulong)(((long)this.row << 32) + (long)this.column);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002BE2A File Offset: 0x0002A02A
		public int CompareTo(HexCoord other)
		{
			if (this.column == other.column)
			{
				return this.row.CompareTo(other.row);
			}
			return this.column.CompareTo(other.column);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002BE5D File Offset: 0x0002A05D
		public void Normalize(int width, int height)
		{
			if (width > 0)
			{
				this.column = (this.column % width + width) % width;
			}
			if (height > 0)
			{
				this.row = (this.row % height + height) % height;
			}
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002BE8C File Offset: 0x0002A08C
		public readonly HexCoord Normalized(int width, int height)
		{
			HexCoord result = this;
			result.Normalize(width, height);
			return result;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002BEAC File Offset: 0x0002A0AC
		public float Distance(HexCoord other)
		{
			return (this - other).Magnitude;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002BECC File Offset: 0x0002A0CC
		public int HexDistance(HexCoord other)
		{
			int num = other.column - this.column;
			int value = other.row - this.row;
			int num2 = Math.Abs(num);
			int num3 = Math.Abs(value);
			if (num < 0 ^ (this.row & 1) == 1)
			{
				num2 = Math.Max(0, num2 - (num3 + 1) / 2);
			}
			else
			{
				num2 = Math.Max(0, num2 - num3 / 2);
			}
			return num2 + num3;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002BF32 File Offset: 0x0002A132
		public string VerboseString()
		{
			return string.Format("(row:{0:+0#;-0#}, column:{1:+0#;-0#})", this.row, this.column);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002BF54 File Offset: 0x0002A154
		public override string ToString()
		{
			return string.Format("({0},{1})", this.row, this.column);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002BF78 File Offset: 0x0002A178
		public bool IsDefault(DefaultValueAttribute defaultValueAttribute)
		{
			int num = 0;
			int defaultRow = 0;
			if (defaultValueAttribute != null)
			{
				num = defaultValueAttribute.IntValue(0);
				defaultRow = num;
			}
			return this.IsDefault(num, defaultRow);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002BF9E File Offset: 0x0002A19E
		public bool IsDefault(int defaultColumn, int defaultRow)
		{
			return this.row == defaultRow && this.column == defaultColumn;
		}

		// Token: 0x0400047C RID: 1148
		public const int InvalidValue = 2147483647;

		// Token: 0x0400047D RID: 1149
		public static readonly HexCoord Origin = default(HexCoord);

		// Token: 0x0400047E RID: 1150
		public static readonly HexCoord Invalid = new HexCoord(int.MaxValue, int.MaxValue);

		// Token: 0x0400047F RID: 1151
		[JsonProperty]
		public int column;

		// Token: 0x04000480 RID: 1152
		[JsonProperty]
		public int row;
	}
}

using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001D1 RID: 465
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public struct CubeCoord
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0002A88F File Offset: 0x00028A8F
		[JsonIgnore]
		public CubeCoord Normalized
		{
			get
			{
				return CubeCoord.Round(this);
			}
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0002A897 File Offset: 0x00028A97
		public CubeCoord(float q, float r)
		{
			this = new CubeCoord(q, r, -q - r);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0002A8A5 File Offset: 0x00028AA5
		public CubeCoord(float q, float r, float s)
		{
			this.q = q;
			this.r = r;
			this.s = s;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0002A8BC File Offset: 0x00028ABC
		public static CubeCoord operator -(in CubeCoord lhs, in CubeCoord rhs)
		{
			return new CubeCoord(lhs.q - rhs.q, lhs.r - rhs.r, lhs.s - rhs.s);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0002A8EA File Offset: 0x00028AEA
		public static CubeCoord operator +(in CubeCoord lhs, in CubeCoord rhs)
		{
			return new CubeCoord(lhs.q + rhs.q, lhs.r + rhs.r, lhs.s + rhs.s);
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x0002A918 File Offset: 0x00028B18
		public float Magnitude
		{
			get
			{
				return (Math.Abs(this.q) + Math.Abs(this.r) + Math.Abs(this.s)) / 2f;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0002A943 File Offset: 0x00028B43
		public int Length
		{
			get
			{
				return (int)this.Magnitude;
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0002A94C File Offset: 0x00028B4C
		public static explicit operator HexCoord(in CubeCoord cube)
		{
			return HexUtils.ToHexCoord(cube);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0002A954 File Offset: 0x00028B54
		public static CubeCoord Round(in CubeCoord h)
		{
			int num = (int)Math.Round((double)h.q);
			int num2 = (int)Math.Round((double)h.r);
			int num3 = (int)Math.Round((double)h.s);
			float num4 = Math.Abs((float)num - h.q);
			float num5 = Math.Abs((float)num2 - h.r);
			float num6 = Math.Abs((float)num3 - h.s);
			if (num4 > num5 && num4 > num6)
			{
				num = -num2 - num3;
			}
			else if (num5 > num6)
			{
				num2 = -num - num3;
			}
			else
			{
				num3 = -num - num2;
			}
			return new CubeCoord((float)num, (float)num2, (float)num3);
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0002A9E8 File Offset: 0x00028BE8
		public override string ToString()
		{
			return string.Format("({0},{1},{2})", this.q, this.r, this.s);
		}

		// Token: 0x04000453 RID: 1107
		public static readonly CubeCoord Origin = new CubeCoord(0f, 0f, 0f);

		// Token: 0x04000454 RID: 1108
		public static bool UseEvenOffsets = false;

		// Token: 0x04000455 RID: 1109
		[JsonProperty]
		public float q;

		// Token: 0x04000456 RID: 1110
		[JsonProperty]
		public float r;

		// Token: 0x04000457 RID: 1111
		[JsonProperty]
		public float s;
	}
}

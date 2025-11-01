using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001D2 RID: 466
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class Hex : IDeepClone<Hex>
	{
		// Token: 0x060008D2 RID: 2258 RVA: 0x0002AA36 File Offset: 0x00028C36
		public ulong VisualHash()
		{
			if (!string.IsNullOrEmpty(this.VisualOverrideGUID))
			{
				return (ulong)((long)this.VisualOverrideGUID.GetHashCode());
			}
			return (ulong)((long)this.Type * (long)this.HexCoord.VisualHashCode());
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0002AA65 File Offset: 0x00028C65
		[JsonConstructor]
		public Hex() : this(0, 0, TerrainType.Plain)
		{
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0002AA70 File Offset: 0x00028C70
		public Hex(int row, int column, TerrainType terrain = TerrainType.Plain)
		{
			this.HexCoord = new HexCoord(row, column);
			this.Type = terrain;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0002AA9E File Offset: 0x00028C9E
		public void SetTerrainType(TerrainType newType)
		{
			this.Type = newType;
			this.VisualOverrideGUID = string.Empty;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0002AAB2 File Offset: 0x00028CB2
		public int GetControllingPlayerID()
		{
			return this.ControllingPlayerID;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0002AABA File Offset: 0x00028CBA
		public void SetControllingPlayerID(int id)
		{
			this.ControllingPlayerID = id;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0002AAC3 File Offset: 0x00028CC3
		public bool IsUnclaimed()
		{
			return this.GetControllingPlayerID() == -1;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002AAD0 File Offset: 0x00028CD0
		public void DeepClone(out Hex clone)
		{
			clone = new Hex(this.HexCoord.row, this.HexCoord.column, this.Type)
			{
				VisualOverrideGUID = this.VisualOverrideGUID.DeepClone(),
				ControllingPlayerID = this.ControllingPlayerID
			};
		}

		// Token: 0x04000458 RID: 1112
		[JsonProperty]
		public TerrainType Type;

		// Token: 0x04000459 RID: 1113
		[JsonProperty]
		[DefaultValue("")]
		public string VisualOverrideGUID = "";

		// Token: 0x0400045A RID: 1114
		[JsonProperty]
		public HexCoord HexCoord;

		// Token: 0x0400045B RID: 1115
		[JsonProperty]
		[DefaultValue(-1)]
		public int ControllingPlayerID = -1;
	}
}

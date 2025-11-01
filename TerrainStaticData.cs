using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Core.StaticData.Attributes;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200047B RID: 1147
	[StaticDataValueType]
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TerrainStaticData : StaticDataEntity, IIndexableData
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06001545 RID: 5445 RVA: 0x00050508 File Offset: 0x0004E708
		public int Index
		{
			get
			{
				return (int)this.LegacyType;
			}
		}

		// Token: 0x04000AD1 RID: 2769
		[JsonProperty]
		public TerrainType LegacyType;

		// Token: 0x04000AD2 RID: 2770
		[JsonProperty]
		public bool Capturable;

		// Token: 0x04000AD3 RID: 2771
		[JsonProperty]
		public MoveCostType MoveCost;

		// Token: 0x04000AD4 RID: 2772
		[JsonProperty]
		[DefaultValue(1)]
		public int MovePoints = 1;

		// Token: 0x04000AD5 RID: 2773
		[JsonProperty]
		public bool AllowTeleport;

		// Token: 0x04000AD6 RID: 2774
		[JsonProperty]
		[DefaultValue(IslandMethod.Bunching)]
		public IslandMethod PatchExpansionMethod = IslandMethod.Bunching;

		// Token: 0x04000AD7 RID: 2775
		[JsonProperty]
		[DefaultValue(2)]
		public int PatchMinSize = 2;

		// Token: 0x04000AD8 RID: 2776
		[JsonProperty]
		[DefaultValue(7)]
		public int PatchMaxSize = 7;

		// Token: 0x04000AD9 RID: 2777
		[JsonProperty]
		public List<ConfigRef<TerrainStaticData>> TransmutableTypes = new List<ConfigRef<TerrainStaticData>>();

		// Token: 0x04000ADA RID: 2778
		[JsonProperty]
		public List<ConfigRef<ItemAbilityStaticData>> ProvidedAbilities = new List<ConfigRef<ItemAbilityStaticData>>();
	}
}

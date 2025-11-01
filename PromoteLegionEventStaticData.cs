using System;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200043C RID: 1084
	[Serializable]
	public class PromoteLegionEventStaticData : EventEffectStaticData, IGamePieceTargetAbility, IPlayerTargetAbility
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x0004F6D2 File Offset: 0x0004D8D2
		public GamePieceTargetSettings GamePieceTargetSettings
		{
			get
			{
				return this.TargetSettings;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x0004F6DA File Offset: 0x0004D8DA
		public PlayerTargetSettings PlayerTargetSettings
		{
			get
			{
				return this.PlayerSettings;
			}
		}

		// Token: 0x04000A32 RID: 2610
		[JsonProperty]
		public GamePieceTargetSettings TargetSettings;

		// Token: 0x04000A33 RID: 2611
		[JsonProperty]
		public PlayerTargetSettings PlayerSettings;

		// Token: 0x04000A34 RID: 2612
		[JsonProperty]
		public ConfigRef<LegionLevelTable> LevelTable;

		// Token: 0x04000A35 RID: 2613
		[JsonProperty]
		public int LevelUps;
	}
}

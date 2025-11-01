using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200028E RID: 654
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public sealed class GameGenerationParameters
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x00031FF6 File Offset: 0x000301F6
		[JsonIgnore]
		public int PlayerCount
		{
			get
			{
				return this.Players.Count;
			}
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00032004 File Offset: 0x00030204
		public GameGenerationParameters(int seed)
		{
			this.Seed = seed;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00032068 File Offset: 0x00030268
		public GameGenerationParameters() : this(Guid.NewGuid().GetHashCode())
		{
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00032090 File Offset: 0x00030290
		[JsonConstructor]
		private GameGenerationParameters(JsonToken token)
		{
		}

		// Token: 0x040005AC RID: 1452
		[JsonProperty]
		public int Seed;

		// Token: 0x040005AD RID: 1453
		[JsonProperty]
		public BoardGenerationParameters BoardGeneration = new BoardGenerationParameters();

		// Token: 0x040005AE RID: 1454
		[JsonProperty]
		public List<PlayerGenerationParameters> Players = new List<PlayerGenerationParameters>();

		// Token: 0x040005AF RID: 1455
		[JsonProperty]
		public Guid? GameId;

		// Token: 0x040005B0 RID: 1456
		[JsonProperty]
		[DefaultValue(-1)]
		public int StartingRegentId = -1;

		// Token: 0x040005B1 RID: 1457
		[JsonProperty]
		[DefaultValue(GameType.SingleplayerSkirmish)]
		public GameType GameType = GameType.SingleplayerSkirmish;

		// Token: 0x040005B2 RID: 1458
		[JsonProperty]
		[DefaultValue(20)]
		public int StartingPrestige = 20;

		// Token: 0x040005B3 RID: 1459
		[JsonProperty]
		[DefaultValue(50)]
		public int GameDuration = 50;

		// Token: 0x040005B4 RID: 1460
		[JsonProperty]
		[DefaultValue(true)]
		public bool FillEmptyRelicsWithDefaults = true;

		// Token: 0x040005B5 RID: 1461
		[JsonProperty]
		public TimeSpan TurnTimer = new TimeSpan(0, 3, 0);
	}
}

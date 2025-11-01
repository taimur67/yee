using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006F6 RID: 1782
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GameVictory : IDeepClone<GameVictory>
	{
		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x00077000 File Offset: 0x00075200
		[JsonIgnore]
		public int WinningPlayerID
		{
			get
			{
				if (this.PlayerIdsInWinningOrder.Count <= 0)
				{
					return -1;
				}
				return this.PlayerIdsInWinningOrder[0];
			}
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x0007701E File Offset: 0x0007521E
		[JsonConstructor]
		protected GameVictory()
		{
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x0007703C File Offset: 0x0007523C
		public GameVictory(VictoryType victoryType, List<int> playerIdsInWinningOrder, VictoryManipulationType manipulationType = VictoryManipulationType.None, int puppetPlayerID = -2147483648)
		{
			this.PrimaryVictoryType = victoryType;
			this.ManipulationType = manipulationType;
			this.PlayerIdsInWinningOrder = playerIdsInWinningOrder;
			this.PuppetPlayerID = puppetPlayerID;
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x00077077 File Offset: 0x00075277
		public void DeepClone(out GameVictory clone)
		{
			clone = new GameVictory
			{
				PrimaryVictoryType = this.PrimaryVictoryType,
				ManipulationType = this.ManipulationType,
				PlayerIdsInWinningOrder = this.PlayerIdsInWinningOrder.DeepClone(),
				PuppetPlayerID = this.PuppetPlayerID
			};
		}

		// Token: 0x04000F14 RID: 3860
		[JsonProperty]
		public VictoryType PrimaryVictoryType;

		// Token: 0x04000F15 RID: 3861
		[JsonProperty]
		public VictoryManipulationType ManipulationType;

		// Token: 0x04000F16 RID: 3862
		[JsonProperty]
		public List<int> PlayerIdsInWinningOrder = new List<int>();

		// Token: 0x04000F17 RID: 3863
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int PuppetPlayerID = int.MinValue;
	}
}

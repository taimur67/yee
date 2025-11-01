using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002F4 RID: 756
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerPowerKnowledgeModifier : PlayerTargetedKnowledgeModifier
	{
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x0003ABAD File Offset: 0x00038DAD
		public PlayerPowersLevels Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0003ABB5 File Offset: 0x00038DB5
		public PlayerPowerKnowledgeModifier(PlayerPowersLevels powersLevels)
		{
			this._data = powersLevels;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0003ABC4 File Offset: 0x00038DC4
		public override void Process(TurnState view, PlayerState playerState)
		{
			foreach (PowerType type in PlayerPowersLevels.PowerTypes)
			{
				PlayerPowerLevel playerPowerLevel = this._data[type];
				if (playerPowerLevel != -2147483648)
				{
					playerState.PowersLevels[type] = playerPowerLevel;
				}
			}
		}

		// Token: 0x040006BF RID: 1727
		[JsonProperty]
		private PlayerPowersLevels _data;
	}
}

using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003D8 RID: 984
	public class UpgradePraetorAbilityStaticData : ManuscriptAbilityStaticData, IGameItemTargetAbility, IPlayerTargetAbility
	{
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001325 RID: 4901 RVA: 0x00048CF8 File Offset: 0x00046EF8
		public GameItemTargetSettings GameItemTargetSettings
		{
			get
			{
				return new GameItemTargetSettings
				{
					ValidTargetTypes = BitMask.From(2)
				};
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001326 RID: 4902 RVA: 0x00048D0B File Offset: 0x00046F0B
		public PlayerTargetSettings PlayerTargetSettings
		{
			get
			{
				return this._playerTargetSettings;
			}
		}

		// Token: 0x040008DA RID: 2266
		[JsonProperty]
		public ConfigRef<PraetorCombatMoveStaticData> Move;

		// Token: 0x040008DB RID: 2267
		[JsonProperty]
		public PlayerTargetSettings _playerTargetSettings;
	}
}

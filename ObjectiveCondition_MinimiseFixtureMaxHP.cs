using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000555 RID: 1365
	[Serializable]
	public class ObjectiveCondition_MinimiseFixtureMaxHP : ObjectiveCondition_CapOutAttributesWithRitual
	{
		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001A5C RID: 6748 RVA: 0x0005BF6C File Offset: 0x0005A16C
		public override string LocalizationKey
		{
			get
			{
				return "MinimiseFixtureMaxHP";
			}
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x0005BF73 File Offset: 0x0005A173
		protected override bool IsAcceptableStat(GamePieceStat stat)
		{
			return stat == GamePieceStat.MaxHealth;
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0005BF79 File Offset: 0x0005A179
		protected override bool IsAcceptableGamePiece(GamePiece gamePiece)
		{
			return gamePiece.IsFixture();
		}
	}
}

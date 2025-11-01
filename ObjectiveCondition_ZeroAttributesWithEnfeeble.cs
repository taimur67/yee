using System;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000556 RID: 1366
	[Serializable]
	public class ObjectiveCondition_ZeroAttributesWithEnfeeble : ObjectiveCondition_CapOutAttributesWithRitual
	{
		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001A60 RID: 6752 RVA: 0x0005BF89 File Offset: 0x0005A189
		public override string LocalizationKey
		{
			get
			{
				return "ZeroAttributesWithEnfeeble";
			}
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x0005BF90 File Offset: 0x0005A190
		protected override bool IsAcceptableStat(GamePieceStat stat)
		{
			bool result;
			switch (stat)
			{
			case GamePieceStat.Ranged:
				result = true;
				break;
			case GamePieceStat.Melee:
				result = true;
				break;
			case GamePieceStat.Infernal:
				result = true;
				break;
			default:
				result = false;
				break;
			}
			return result;
		}
	}
}

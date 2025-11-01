using System;

namespace LoG
{
	// Token: 0x0200054A RID: 1354
	[Serializable]
	public class ObjectiveCondition_ArchfiendRank : ObjectiveCondition, IDynamicObjective
	{
		// Token: 0x06001A3F RID: 6719 RVA: 0x0005B7C9 File Offset: 0x000599C9
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			return (int)owner.Rank;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x0005B7D4 File Offset: 0x000599D4
		public ObjectiveDifficulty CalculateDifficulty(TurnContext context, PlayerState player)
		{
			int rank = (int)player.Rank;
			int num = this.Target - rank;
			ObjectiveDifficulty result;
			if (num <= 2)
			{
				if (num > 1)
				{
					result = ObjectiveDifficulty.Moderate;
				}
				else
				{
					result = ObjectiveDifficulty.Easy;
				}
			}
			else if (num > 3)
			{
				result = ObjectiveDifficulty.Hard;
			}
			else
			{
				result = ObjectiveDifficulty.Hard;
			}
			return result;
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x0005B810 File Offset: 0x00059A10
		public override int GetHashCode()
		{
			return base.GetHashCode() * 19 + this.Target;
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x0005B822 File Offset: 0x00059A22
		public override string Name
		{
			get
			{
				return string.Format("Raise rank to {0}", (Rank)this.Target);
			}
		}
	}
}

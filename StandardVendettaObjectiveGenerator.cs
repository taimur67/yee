using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200052B RID: 1323
	[Serializable]
	public class StandardVendettaObjectiveGenerator : VendettaObjectiveGenerator
	{
		// Token: 0x060019C3 RID: 6595 RVA: 0x0005A568 File Offset: 0x00058768
		public override VendettaObjective GenerateVendetta(TurnState turn, PlayerState player, PlayerState target)
		{
			if (player.Rank < this.MinimumRank)
			{
				return null;
			}
			VendettaObjective vendettaObjective = this.CreateObjective(turn, target);
			vendettaObjective.Id = base.Id + ":" + string.Join<int>(":", from t in vendettaObjective.Conditions
			select t.Target);
			vendettaObjective.SourceId = base.Id;
			vendettaObjective.ObjectiveDifficulty = this.ObjectiveDifficulty;
			return vendettaObjective;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0005A5F1 File Offset: 0x000587F1
		public virtual VendettaObjective CreateObjective(TurnState turn, PlayerState targetPlayer)
		{
			return new VendettaObjective(this.CreateConditions(turn, targetPlayer));
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0005A600 File Offset: 0x00058800
		public virtual IEnumerable<ObjectiveCondition> CreateConditions(TurnState turn, PlayerState target)
		{
			yield break;
		}

		// Token: 0x04000BBE RID: 3006
		[JsonProperty]
		public float ObjectiveDifficulty;

		// Token: 0x04000BBF RID: 3007
		[JsonProperty]
		public Rank MinimumRank;
	}
}

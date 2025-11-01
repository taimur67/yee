using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200057D RID: 1405
	public class ObjectiveCondition_EliminatePlayer : ObjectiveCondition
	{
		// Token: 0x06001ACB RID: 6859 RVA: 0x0005D8CC File Offset: 0x0005BACC
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			if (isInitialProgress)
			{
				return 0;
			}
			IEnumerable<PlayerEliminatedEvent> gameEvents = context.CurrentTurn.GetGameEvents<PlayerEliminatedEvent>();
			bool flag = false;
			bool flag2 = false;
			foreach (PlayerEliminatedEvent playerEliminatedEvent in gameEvents)
			{
				List<PlayerState> potentialCandidates = base.GetPotentialCandidates(context, owner, this.EliminationTarget, this.EliminationTargetArchfiend, this.EliminationTargetNegate);
				List<PlayerState> potentialCandidates2 = base.GetPotentialCandidates(context, owner, this.Eliminator, this.EliminatorArchfiend, this.EliminatorArchfiendNegate);
				using (List<PlayerState>.Enumerator enumerator2 = potentialCandidates.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Id == playerEliminatedEvent.AffectedPlayerID)
						{
							flag = true;
							break;
						}
					}
				}
				using (List<PlayerState>.Enumerator enumerator2 = potentialCandidates2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Id == playerEliminatedEvent.TriggeringPlayerID)
						{
							flag2 = true;
							break;
						}
					}
				}
			}
			if (!flag || !flag2)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x04000C2B RID: 3115
		[JsonProperty]
		[DefaultValue(ObjectivePlayerRole.Anyone)]
		public ObjectivePlayerRole EliminationTarget = ObjectivePlayerRole.Anyone;

		// Token: 0x04000C2C RID: 3116
		[JsonProperty]
		public ConfigRef<ArchFiendStaticData> EliminationTargetArchfiend;

		// Token: 0x04000C2D RID: 3117
		[JsonProperty]
		public bool EliminationTargetNegate;

		// Token: 0x04000C2E RID: 3118
		[JsonProperty]
		[DefaultValue(ObjectivePlayerRole.Player)]
		public ObjectivePlayerRole Eliminator;

		// Token: 0x04000C2F RID: 3119
		[JsonProperty]
		public ConfigRef<ArchFiendStaticData> EliminatorArchfiend;

		// Token: 0x04000C30 RID: 3120
		[JsonProperty]
		public bool EliminatorArchfiendNegate;
	}
}

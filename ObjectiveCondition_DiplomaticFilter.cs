using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200057A RID: 1402
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class ObjectiveCondition_DiplomaticFilter : ObjectiveCondition
	{
		// Token: 0x06001AC1 RID: 6849 RVA: 0x0005D788 File Offset: 0x0005B988
		protected static IEnumerable<PlayerState> GetTargets(TurnState turn, PlayerState owner, ObjectivePlayerRole objectivePlayerRole, ConfigRef<ArchFiendStaticData> associatedAF)
		{
			if (objectivePlayerRole == ObjectivePlayerRole.Player)
			{
				return IEnumerableExtensions.ToEnumerable<PlayerState>(owner);
			}
			if (objectivePlayerRole == ObjectivePlayerRole.Specified)
			{
				PlayerState playerState = turn.FindPlayerState(associatedAF.Id);
				if (playerState != null)
				{
					return IEnumerableExtensions.ToEnumerable<PlayerState>(playerState);
				}
			}
			else if (objectivePlayerRole == ObjectivePlayerRole.Anyone)
			{
				return turn.EnumeratePlayerStates(false, false);
			}
			return Enumerable.Empty<PlayerState>();
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x0005D7CC File Offset: 0x0005B9CC
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			if (isInitialProgress)
			{
				return 0;
			}
			return this.CalculateNumberActive(context.CurrentTurn, this.GetRelevantPairs(context.CurrentTurn, owner));
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x0005D7EC File Offset: 0x0005B9EC
		protected IEnumerable<DiplomaticPairStatus> GetRelevantPairs(TurnState turn, PlayerState owner)
		{
			foreach (PlayerState instigator in ObjectiveCondition_DiplomaticFilter.GetTargets(turn, owner, this.InstigatorRole, this.InstigatorArchfiend))
			{
				foreach (PlayerState secondPlayer in ObjectiveCondition_DiplomaticFilter.GetTargets(turn, owner, this.TargetRole, this.TargetArchfiend))
				{
					yield return turn.GetDiplomaticStatus(instigator, secondPlayer);
				}
				IEnumerator<PlayerState> enumerator2 = null;
				instigator = null;
			}
			IEnumerator<PlayerState> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x0005D80C File Offset: 0x0005BA0C
		public virtual int CalculateNumberActive(TurnState state, IEnumerable<DiplomaticPairStatus> diplomaticPairs)
		{
			return diplomaticPairs.Count((DiplomaticPairStatus t) => this.CheckCompleteStatus(state, t));
		}

		// Token: 0x06001AC5 RID: 6853
		public abstract bool CheckCompleteStatus(TurnState state, DiplomaticPairStatus status);

		// Token: 0x04000C25 RID: 3109
		[JsonProperty]
		[DefaultValue(ObjectivePlayerRole.Player)]
		public ObjectivePlayerRole InstigatorRole;

		// Token: 0x04000C26 RID: 3110
		[JsonProperty]
		public ConfigRef<ArchFiendStaticData> InstigatorArchfiend;

		// Token: 0x04000C27 RID: 3111
		[JsonProperty]
		[DefaultValue(ObjectivePlayerRole.Specified)]
		public ObjectivePlayerRole TargetRole = ObjectivePlayerRole.Specified;

		// Token: 0x04000C28 RID: 3112
		[JsonProperty]
		public ConfigRef<ArchFiendStaticData> TargetArchfiend;
	}
}

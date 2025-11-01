using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000466 RID: 1126
	[Serializable]
	public class CastRitualOnAllSchemeGenerator : BasicSchemeGenerator
	{
		// Token: 0x06001513 RID: 5395 RVA: 0x0004FD3C File Offset: 0x0004DF3C
		protected override IEnumerable<ObjectiveCondition> GenerateConditions(TurnContext context, PlayerState player)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != player.Id)
				{
					yield return new ObjectiveCondition_SuccessfullyCastRitual(playerState.Id)
					{
						Ritual = this.Ritual
					};
				}
			}
			IEnumerator<PlayerState> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000A9A RID: 2714
		public ConfigRef<RitualStaticData> Ritual;
	}
}

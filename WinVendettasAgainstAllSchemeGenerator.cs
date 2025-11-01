using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000471 RID: 1137
	[Serializable]
	public class WinVendettasAgainstAllSchemeGenerator : BasicSchemeGenerator
	{
		// Token: 0x06001533 RID: 5427 RVA: 0x00050178 File Offset: 0x0004E378
		protected override IEnumerable<ObjectiveCondition> GenerateConditions(TurnContext context, PlayerState player)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != player.Id)
				{
					yield return new ObjectiveCondition_WinVendettaAgainstPlayer(playerState.Id);
				}
			}
			IEnumerator<PlayerState> enumerator = null;
			yield break;
			yield break;
		}
	}
}

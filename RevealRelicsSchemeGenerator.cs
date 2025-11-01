using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x0200046D RID: 1133
	[Serializable]
	public class RevealRelicsSchemeGenerator : BasicSchemeGenerator
	{
		// Token: 0x06001523 RID: 5411 RVA: 0x0005003A File Offset: 0x0004E23A
		protected override IEnumerable<ObjectiveCondition> GenerateConditions(TurnContext context, PlayerState player)
		{
			foreach (PlayerState playerState in context.CurrentTurn.EnumeratePlayerStates(false, false))
			{
				if (playerState.Id != player.Id)
				{
					yield return new ObjectiveCondition_RevealRelicsForPlayer
					{
						TargetingPlayer = new int?(playerState.Id)
					};
				}
			}
			IEnumerator<PlayerState> enumerator = null;
			yield break;
			yield break;
		}
	}
}

using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005BC RID: 1468
	[Serializable]
	public class ObjectiveCondition_WinDuelsWithTechnique : ObjectiveCondition_WinDuels
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001B6F RID: 7023 RVA: 0x0005F53E File Offset: 0x0005D73E
		public override string LocalizationKey
		{
			get
			{
				return "WinDuelsWithTechnique";
			}
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0005F548 File Offset: 0x0005D748
		protected override bool Filter(TurnContext context, PraetorDuelOutcomeEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			ConfigRef<PraetorCombatMoveStaticData> configRef = @event.Winner.GetCombatMove(context.CurrentTurn) as ConfigRef<PraetorCombatMoveStaticData>;
			PraetorCombatMoveStaticData praetorCombatMoveStaticData;
			PraetorCombatMoveStaticData praetorCombatMoveStaticData2;
			return configRef != null && context.Database.TryFetch<PraetorCombatMoveStaticData>(configRef, out praetorCombatMoveStaticData) && context.Database.TryFetch<PraetorCombatMoveStaticData>(this.WinnerMustUseCombatMove, out praetorCombatMoveStaticData2) && !(praetorCombatMoveStaticData.Id != praetorCombatMoveStaticData2.Id);
		}

		// Token: 0x04000C60 RID: 3168
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public ConfigRef<PraetorCombatMoveStaticData> WinnerMustUseCombatMove;
	}
}

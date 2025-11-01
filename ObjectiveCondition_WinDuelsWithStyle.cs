using System;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005BB RID: 1467
	[Serializable]
	public class ObjectiveCondition_WinDuelsWithStyle : ObjectiveCondition_WinDuels
	{
		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001B6C RID: 7020 RVA: 0x0005F4C2 File Offset: 0x0005D6C2
		public override string LocalizationKey
		{
			get
			{
				return "WinDuelsWithStyle";
			}
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x0005F4CC File Offset: 0x0005D6CC
		protected override bool Filter(TurnContext context, PraetorDuelOutcomeEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			ConfigRef<PraetorCombatMoveStaticData> configRef = @event.Winner.GetCombatMove(context.CurrentTurn) as ConfigRef<PraetorCombatMoveStaticData>;
			PraetorCombatMoveStaticData praetorCombatMoveStaticData;
			return configRef != null && context.Database.TryFetch<PraetorCombatMoveStaticData>(configRef, out praetorCombatMoveStaticData) && !(praetorCombatMoveStaticData.TechniqueType.Id != this.WinnerMustUseCombatMoveStyle.Id);
		}

		// Token: 0x04000C5F RID: 3167
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public ConfigRef<PraetorCombatMoveStyle> WinnerMustUseCombatMoveStyle;
	}
}

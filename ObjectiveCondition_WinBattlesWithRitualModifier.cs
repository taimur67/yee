using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005B6 RID: 1462
	[Serializable]
	public class ObjectiveCondition_WinBattlesWithRitualModifier : ObjectiveCondition_WinBattles
	{
		// Token: 0x06001B59 RID: 7001 RVA: 0x0005EFBC File Offset: 0x0005D1BC
		protected override bool Filter(TurnContext context, BattleEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			GamePiece gamePiece;
			GamePiece gamePiece2;
			if (!@event.BattleResult.TryGetPiecesForPlayer(owner.Id, true, out gamePiece, out gamePiece2))
			{
				return false;
			}
			bool result = false;
			using (IEnumerator<ModifiableValue> enumerator = gamePiece.CombatStats.EnumerateStatValues().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ActiveModifiers.Any(delegate(StatModifierBase x)
					{
						RitualContext ritualContext = x.Provider as RitualContext;
						return ritualContext != null && ritualContext.RitualId == this.Ritual.Id;
					}))
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x04000C58 RID: 3160
		[JsonProperty]
		public ConfigRef<RitualStaticData> Ritual;
	}
}

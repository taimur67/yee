using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005B5 RID: 1461
	[Serializable]
	public class ObjectiveCondition_WinBattlesWithActiveRitual : ObjectiveCondition_WinBattles
	{
		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001B56 RID: 6998 RVA: 0x0005EEA3 File Offset: 0x0005D0A3
		public override string LocalizationKey
		{
			get
			{
				return "WinBattlesWithActiveRitual";
			}
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0005EEAC File Offset: 0x0005D0AC
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
			foreach (ActiveRitual activeRitual in context.CurrentTurn.GetActiveRituals(owner))
			{
				RitualStaticData ritualStaticData;
				if (context.Database.TryFetch<RitualStaticData>(activeRitual.StaticDataId, out ritualStaticData))
				{
					using (List<ConfigRef<RitualStaticData>>.Enumerator enumerator2 = this.AcceptedRituals.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.Id == ritualStaticData.ConfigRef.Id && activeRitual.TargetContext.ItemId == gamePiece.Id)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04000C57 RID: 3159
		[JsonProperty]
		public List<ConfigRef<RitualStaticData>> AcceptedRituals = new List<ConfigRef<RitualStaticData>>();
	}
}

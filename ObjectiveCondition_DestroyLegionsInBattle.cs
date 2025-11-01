using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000577 RID: 1399
	[Serializable]
	public class ObjectiveCondition_DestroyLegionsInBattle : ObjectiveCondition_WinBattles
	{
		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001AB7 RID: 6839 RVA: 0x0005D419 File Offset: 0x0005B619
		public override string LocalizationKey
		{
			get
			{
				if (!this.AllowNonStratagemItems && this.LevelDifference == LevelDifference.MustBeLower)
				{
					return "DefeatStrongerLegionStratagemsOnly";
				}
				return "WinBattlesWithStratagem";
			}
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x0005D438 File Offset: 0x0005B638
		protected override bool Filter(TurnContext context, BattleEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			GamePiece gamePiece;
			GamePiece b;
			if (!@event.BattleResult.TryGetPiecesForPlayer(owner.Id, true, out gamePiece, out b))
			{
				return false;
			}
			if (!this.CheckLevelDifference(gamePiece, b, this.LevelDifference))
			{
				return false;
			}
			List<GameItem> source = IEnumerableExtensions.ToList<GameItem>(gamePiece.Slots.Select(new Func<Identifier, GameItem>(context.CurrentTurn.FetchGameItem)));
			if (!this.AllowNonStratagemItems)
			{
				if (source.Any((GameItem x) => !(x is Stratagem)))
				{
					return false;
				}
			}
			if (!this.AllowStratagems)
			{
				if (source.Any((GameItem x) => x is Stratagem))
				{
					return false;
				}
			}
			GamePiece gamePiece2;
			if (!@event.BattleResult.TryGetLosingPiece_EndState(out gamePiece2))
			{
				return false;
			}
			if (this.AcceptedOpponentTypes.ValidGamePieceTypes.IsEmpty() && !gamePiece2.SubCategory.IsLegion())
			{
				return false;
			}
			if (!this.WithGamePiece.IsEmpty() && !gamePiece.StaticDataReference.Equals(this.WithGamePiece))
			{
				return false;
			}
			if (this.AllowActiveRituals)
			{
				return true;
			}
			foreach (ActiveRitual activeRitual in context.CurrentTurn.GetActiveRituals(owner))
			{
				RitualStaticData ritualStaticData;
				if (context.Database.TryFetch<RitualStaticData>(activeRitual.StaticDataId, out ritualStaticData) && activeRitual.TargetContext.ItemId == gamePiece.Id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x0005D5DC File Offset: 0x0005B7DC
		private bool CheckLevelDifference(GamePiece a, GamePiece b, LevelDifference levelDifference)
		{
			bool result;
			switch (levelDifference)
			{
			case LevelDifference.None:
				result = true;
				break;
			case LevelDifference.MustBeLower:
				result = (a.Level < b.Level);
				break;
			case LevelDifference.MustBeLowerOrEqual:
				result = (a.Level <= b.Level);
				break;
			case LevelDifference.MustBeEqual:
				result = (a.Level == b.Level);
				break;
			case LevelDifference.MustBeHigherOrEqual:
				result = (a.Level >= b.Level);
				break;
			case LevelDifference.MustBeHigher:
				result = (a.Level > b.Level);
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x0005D66C File Offset: 0x0005B86C
		public override int GetHashCode()
		{
			int num = (int)((base.GetHashCode() * 23 + this.LevelDifference) * (LevelDifference)29);
			if (this.AllowStratagems)
			{
				num += 17;
			}
			if (this.AllowNonStratagemItems)
			{
				num += 37;
			}
			return num;
		}

		// Token: 0x04000C1D RID: 3101
		[JsonProperty]
		public ConfigRef<GamePieceStaticData> WithGamePiece;

		// Token: 0x04000C1E RID: 3102
		[JsonProperty]
		[DefaultValue(LevelDifference.None)]
		public LevelDifference LevelDifference;

		// Token: 0x04000C1F RID: 3103
		[JsonProperty]
		[DefaultValue(true)]
		public bool AllowStratagems = true;

		// Token: 0x04000C20 RID: 3104
		[JsonProperty]
		[DefaultValue(true)]
		public bool AllowNonStratagemItems = true;

		// Token: 0x04000C21 RID: 3105
		[JsonProperty]
		[DefaultValue(true)]
		public bool AllowActiveRituals = true;
	}
}

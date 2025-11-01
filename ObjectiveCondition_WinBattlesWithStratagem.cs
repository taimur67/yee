using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005B7 RID: 1463
	[Serializable]
	public class ObjectiveCondition_WinBattlesWithStratagem : ObjectiveCondition_WinBattles
	{
		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001B5C RID: 7004 RVA: 0x0005F088 File Offset: 0x0005D288
		[JsonIgnore]
		public bool AnyTactics
		{
			get
			{
				return this.PermittedTactics.Count == 0;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001B5D RID: 7005 RVA: 0x0005F098 File Offset: 0x0005D298
		public override string LocalizationKey
		{
			get
			{
				if (this.PermittedTactics.Count > 0)
				{
					return "DefeatWithDecreaseOnlyStratagem";
				}
				return "WinBattlesWithStratagem";
			}
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0005F0B4 File Offset: 0x0005D2B4
		protected override bool Filter(TurnContext context, BattleEvent @event, PlayerState owner, PlayerState target)
		{
			if (!base.Filter(context, @event, owner, target))
			{
				return false;
			}
			GamePiece piece;
			GamePiece gamePiece;
			if (!@event.BattleResult.TryGetPiecesForPlayer(owner.Id, true, out piece, out gamePiece))
			{
				return false;
			}
			List<Stratagem> list = IEnumerableExtensions.ToList<Stratagem>(piece.GetAttachedItems(context.CurrentTurn));
			if (list.Count <= 0)
			{
				return false;
			}
			if (this.PermittedTactics.Count > 0)
			{
				if (list.SelectMany((Stratagem s) => from t in s.Tactics
				select t.StaticDataId).Any((string x) => !IEnumerableExtensions.Contains<string>(from y in this.PermittedTactics
				select y.Id, x)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0005F150 File Offset: 0x0005D350
		public override int GetHashCode()
		{
			int num = base.GetHashCode() * 23;
			foreach (ConfigRef<StratagemTacticStaticData> configRef in this.PermittedTactics)
			{
				num += configRef.Id.GetHashCode() * 17;
			}
			return num;
		}

		// Token: 0x04000C59 RID: 3161
		[JsonProperty]
		public List<ConfigRef<StratagemTacticStaticData>> PermittedTactics = new List<ConfigRef<StratagemTacticStaticData>>();
	}
}

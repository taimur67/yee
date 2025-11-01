using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000566 RID: 1382
	[Serializable]
	public class ObjectiveCondition_ControlGamePiece : BooleanStateObjectiveCondition, IDynamicObjective
	{
		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001A8E RID: 6798 RVA: 0x0005CB2C File Offset: 0x0005AD2C
		public override string LocalizationKey
		{
			get
			{
				return "ControlGamePiece";
			}
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x0005CB33 File Offset: 0x0005AD33
		[JsonConstructor]
		public ObjectiveCondition_ControlGamePiece()
		{
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x0005CB42 File Offset: 0x0005AD42
		public ObjectiveCondition_ControlGamePiece(Identifier item)
		{
			this.ItemId = item;
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x0005CB58 File Offset: 0x0005AD58
		private bool GetGamePiece(TurnContext context, out GamePiece gp)
		{
			return context.CurrentTurn.TryFetchGameItem<GamePiece>(this.ItemId, out gp) || context.CurrentTurn.TryFetchGameItem<GamePiece>(this.GamePiece, out gp);
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x0005CB88 File Offset: 0x0005AD88
		public override Result CanBeCompleted(TurnContext context, PlayerState owner)
		{
			GamePiece gamePiece;
			if (!this.GetGamePiece(context, out gamePiece) || gamePiece.Status != GameItemStatus.InPlay)
			{
				return Result.Failure;
			}
			return Result.Success;
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x0005CBB4 File Offset: 0x0005ADB4
		protected override bool CheckCompleteStatus(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			GamePiece item;
			return this.GetGamePiece(context, out item) && context.CurrentTurn.DoesPlayerControlItem(owner.Id, item);
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x0005CBE8 File Offset: 0x0005ADE8
		public ObjectiveDifficulty CalculateDifficulty(TurnContext context, PlayerState player)
		{
			TurnState currentTurn = context.CurrentTurn;
			GamePiece target;
			if (!this.GetGamePiece(context, out target))
			{
				return ObjectiveDifficulty.Hard;
			}
			List<GamePiece> list = IEnumerableExtensions.ToList<GamePiece>(currentTurn.GetAllActiveLegionsForPlayer(player.Id));
			bool flag = target.IsOwned();
			int level = target.Level;
			float num = flag ? this.Params.OwnedWeight : this.Params.UnOwnedWeight;
			int num2 = Math.Max(currentTurn.HexBoard.Columns, currentTurn.HexBoard.Rows);
			PathfinderHexboard pathfinder = new PathfinderHexboard();
			pathfinder.PopulateMap(context);
			if (list.Count <= 0)
			{
				return ObjectiveDifficulty.Hard;
			}
			float num3 = (float)IEnumerableExtensions.FirstOrDefault<int>(from x in list
			select pathfinder.FindPath(x.Location, target.Location, new PFAgentGamePiece(x)).Count into x
			orderby x
			select x);
			float num4 = this.Params.DistWeight + this.Params.PowerWeight + num;
			float num5 = this.Params.DistWeight / num4;
			float num6 = this.Params.PowerWeight / num4;
			float num7 = num / num4;
			double num8 = (double)(Math.Clamp(num3 / ((float)num2 / 2f), 0f, 1f) * num5 + Math.Clamp((float)level / this.Params.MaxLevel, 0f, 1f) * num6 + num7);
			float num9 = (float)EnumUtility.GetValues<ObjectiveDifficulty>().Max<ObjectiveDifficulty>();
			return (ObjectiveDifficulty)Math.Round(num8 * (double)num9);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x0005CD6D File Offset: 0x0005AF6D
		public override int GetHashCode()
		{
			return (int)(base.GetHashCode() * 19 + this.ItemId);
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001A96 RID: 6806 RVA: 0x0005CD7F File Offset: 0x0005AF7F
		public override string Name
		{
			get
			{
				return string.Format("Control GameItem: {0}", this.ItemId);
			}
		}

		// Token: 0x04000C0B RID: 3083
		[JsonProperty]
		public ObjectiveCondition_ControlGamePiece.DifficultyParams Params;

		// Token: 0x04000C0C RID: 3084
		[JsonProperty]
		public ConfigRef<GamePieceStaticData> GamePiece;

		// Token: 0x04000C0D RID: 3085
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		[DefaultValue(Identifier.Invalid)]
		public Identifier ItemId = Identifier.Invalid;

		// Token: 0x02000A19 RID: 2585
		[Serializable]
		public class DifficultyParams
		{
			// Token: 0x04001896 RID: 6294
			public float OwnedWeight = 1.1f;

			// Token: 0x04001897 RID: 6295
			public float UnOwnedWeight = 0.675f;

			// Token: 0x04001898 RID: 6296
			public float DistWeight = 1.2f;

			// Token: 0x04001899 RID: 6297
			public float PowerWeight = 3f;

			// Token: 0x0400189A RID: 6298
			public float MaxLevel = 6f;
		}
	}
}

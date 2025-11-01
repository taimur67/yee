using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000623 RID: 1571
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderAttachGameItemToGamePiece : ActionableOrder, AttachmentProcessor.IRequest
	{
		// Token: 0x06001D08 RID: 7432 RVA: 0x00064710 File Offset: 0x00062910
		public ConfigRef GetAbilityRef(TurnContext context)
		{
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (dataForRequest == null)
			{
				return null;
			}
			return dataForRequest.ConfigRef;
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x00064724 File Offset: 0x00062924
		[JsonIgnore]
		public Identifier ReassignedItemId
		{
			get
			{
				return this.GameItemId;
			}
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x0006472C File Offset: 0x0006292C
		public override Result TargetItemStolen(ConfigRef order, int originalOwner, GameItem item)
		{
			GamePiece gamePiece = item as GamePiece;
			if (gamePiece == null)
			{
				return new Result.ReassignStolenItemProblem(order, item);
			}
			return new Result.ReassignItemToStolenGamePieceProblem(order, this.GameItemId, gamePiece);
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x00064760 File Offset: 0x00062960
		public override Result TargetItemBanished(ConfigRef order, GameItem item)
		{
			GamePiece gamePiece = item as GamePiece;
			if (gamePiece == null)
			{
				return new Result.ReassignBanishedItemProblem(order, item);
			}
			return new Result.ReassignItemToBanishedGamePieceProblem(order, this.GameItemId, gamePiece);
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x00064794 File Offset: 0x00062994
		public override Result TargetItemInvalid(ConfigRef order, GameItem item)
		{
			GamePiece gamePiece = item as GamePiece;
			if (gamePiece == null)
			{
				return new Result.ReassignItemProblem(order, item);
			}
			return new Result.ReassignItemToInvalidGamePieceProblem(order, this.GameItemId, gamePiece);
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000647C5 File Offset: 0x000629C5
		public Result TargetItemUnattached(ConfigRef order, GameItem unattachedItem)
		{
			return new Result.ReassignItemProblem(order, unattachedItem);
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000647D3 File Offset: 0x000629D3
		public Result TargetItemLocked(ConfigRef order, GameItem lockedItem, GamePiece lockingPiece)
		{
			return new Result.ReassignLockedItemProblem(order, lockedItem, lockingPiece);
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001D0F RID: 7439 RVA: 0x000647E2 File Offset: 0x000629E2
		public override OrderTypes OrderType
		{
			get
			{
				if (this.ItemCategory != GameItemCategory.Praetor)
				{
					return OrderTypes.AttachArtifact;
				}
				return OrderTypes.AttachPraetor;
			}
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000647F2 File Offset: 0x000629F2
		[JsonConstructor]
		public OrderAttachGameItemToGamePiece()
		{
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x00064810 File Offset: 0x00062A10
		public OrderAttachGameItemToGamePiece(Identifier targetPieceId, GameItem gameItem = null, GameItemCategory gameItemCategory = GameItemCategory.None)
		{
			this.TargetPieceId = targetPieceId;
			if (gameItem == null)
			{
				this.ItemCategory = gameItemCategory;
				return;
			}
			this.ItemCategory = gameItem.Category;
			this.GameItemId = gameItem.Id;
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x00064862 File Offset: 0x00062A62
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new ValueConflict<int>(ConflictScopeFlags.Equip, (int)this.GameItemId);
			yield break;
		}

		// Token: 0x04000C82 RID: 3202
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier TargetPieceId = Identifier.Invalid;

		// Token: 0x04000C83 RID: 3203
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier GameItemId = Identifier.Invalid;

		// Token: 0x04000C84 RID: 3204
		[JsonProperty]
		[DefaultValue(GameItemCategory.None)]
		public GameItemCategory ItemCategory = GameItemCategory.None;
	}
}

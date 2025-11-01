using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000644 RID: 1604
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderReturnGameItemToVault : ActionableOrder, AttachmentProcessor.IRequest
	{
		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001D9A RID: 7578 RVA: 0x00066320 File Offset: 0x00064520
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.VaultItem;
			}
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x00066324 File Offset: 0x00064524
		public ConfigRef GetAbilityRef(TurnContext context)
		{
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (dataForRequest == null)
			{
				return null;
			}
			return dataForRequest.ConfigRef;
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x00066338 File Offset: 0x00064538
		public override Result TargetItemStolen(ConfigRef order, int originalOwner, GameItem item)
		{
			return new Result.ReassignStolenItemProblem(order, item);
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x00066346 File Offset: 0x00064546
		public override Result TargetItemBanished(ConfigRef order, GameItem item)
		{
			return new Result.ReassignBanishedItemProblem(order, item);
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x00066354 File Offset: 0x00064554
		public override Result TargetItemInvalid(ConfigRef order, GameItem item)
		{
			return new Result.ReassignItemProblem(order, item);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x00066362 File Offset: 0x00064562
		public Result TargetItemUnattached(ConfigRef order, GameItem unattachedItem)
		{
			return new Result.ReassignItemProblem(order, unattachedItem);
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x00066370 File Offset: 0x00064570
		public Result TargetItemLocked(ConfigRef order, GameItem lockedItem, GamePiece lockingPiece)
		{
			return new Result.ReassignLockedItemProblem(order, lockedItem, lockingPiece);
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x0006637F File Offset: 0x0006457F
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new ValueConflict<int>(ConflictScopeFlags.Equip, (int)this.GameItemId);
			yield break;
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x0006638F File Offset: 0x0006458F
		[JsonConstructor]
		public OrderReturnGameItemToVault()
		{
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x0006639E File Offset: 0x0006459E
		public OrderReturnGameItemToVault(Identifier gameItemId)
		{
			this.GameItemId = gameItemId;
		}

		// Token: 0x04000C96 RID: 3222
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier GameItemId = Identifier.Invalid;
	}
}

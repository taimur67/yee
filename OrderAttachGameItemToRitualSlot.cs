using System;
using System.ComponentModel;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000626 RID: 1574
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderAttachGameItemToRitualSlot : ActionableOrder, AttachmentProcessor.IRequest
	{
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x00064AC1 File Offset: 0x00062CC1
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.AttachArtifact;
			}
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x00064AC5 File Offset: 0x00062CC5
		public ConfigRef GetAbilityRef(TurnContext context)
		{
			AbilityStaticData dataForRequest = context.GetDataForRequest(this);
			if (dataForRequest == null)
			{
				return null;
			}
			return dataForRequest.ConfigRef;
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001D1C RID: 7452 RVA: 0x00064AD9 File Offset: 0x00062CD9
		[JsonIgnore]
		public Identifier ReassignedItemId
		{
			get
			{
				return this.GameItemId;
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00064AE1 File Offset: 0x00062CE1
		public override Result TargetItemStolen(ConfigRef order, int originalOwner, GameItem item)
		{
			return new Result.ReassignStolenItemProblem(order, item);
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x00064AEF File Offset: 0x00062CEF
		public override Result TargetItemBanished(ConfigRef order, GameItem item)
		{
			return new Result.ReassignBanishedItemProblem(order, item);
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x00064AFD File Offset: 0x00062CFD
		public override Result TargetItemInvalid(ConfigRef order, GameItem item)
		{
			return new Result.ReassignItemProblem(order, item);
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x00064B0B File Offset: 0x00062D0B
		public Result TargetItemUnattached(ConfigRef order, GameItem unattachedItem)
		{
			return new Result.ReassignItemProblem(order, unattachedItem);
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x00064B19 File Offset: 0x00062D19
		public Result TargetItemLocked(ConfigRef order, GameItem lockedItem, GamePiece lockingPiece)
		{
			return new Result.ReassignLockedItemProblem(order, lockedItem, lockingPiece);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x00064B28 File Offset: 0x00062D28
		[JsonConstructor]
		public OrderAttachGameItemToRitualSlot()
		{
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x00064B37 File Offset: 0x00062D37
		public OrderAttachGameItemToRitualSlot(Identifier targetItem)
		{
			this.GameItemId = targetItem;
		}

		// Token: 0x04000C85 RID: 3205
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier GameItemId = Identifier.Invalid;
	}
}

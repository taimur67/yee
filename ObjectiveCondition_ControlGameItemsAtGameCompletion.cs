using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000565 RID: 1381
	[Serializable]
	public class ObjectiveCondition_ControlGameItemsAtGameCompletion : BooleanStateObjectiveCondition
	{
		// Token: 0x06001A88 RID: 6792 RVA: 0x0005C910 File Offset: 0x0005AB10
		private bool GetGameItem(TurnContext context, ConfigRef gameItemRef, out GameItem gameItem)
		{
			return context.CurrentTurn.TryFetchGameItem<GameItem>(gameItemRef, out gameItem);
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x0005C920 File Offset: 0x0005AB20
		public override Result CanBeCompleted(TurnContext context, PlayerState owner)
		{
			using (HashSet<ConfigRef>.Enumerator enumerator = context.CurrentTurn.DeadItemReferences.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ConfigRef deadItemReference = enumerator.Current;
					if (this.GameItems.Find((ConfigRef<GameItemStaticData> x) => x.Id == deadItemReference.Id) != null)
					{
						return Result.Failure;
					}
				}
			}
			foreach (ConfigRef<GameItemStaticData> gameItemRef in this.GameItems)
			{
				GameItem gameItem;
				if (!this.GetGameItem(context, gameItemRef, out gameItem))
				{
					return Result.Success;
				}
				if (gameItem.Status == GameItemStatus.Banished)
				{
					return Result.Failure;
				}
			}
			return Result.Success;
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0005CA08 File Offset: 0x0005AC08
		protected override bool CheckCompleteStatus(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			if (!this.MarkAsCompleteEarly && context.CurrentTurn.Victory == null)
			{
				return false;
			}
			foreach (ConfigRef<GameItemStaticData> gameItemRef in this.GameItems)
			{
				GameItem item;
				if (!this.GetGameItem(context, gameItemRef, out item))
				{
					return false;
				}
				if (!context.CurrentTurn.DoesPlayerControlItem(owner.Id, item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x0005CA9C File Offset: 0x0005AC9C
		public override int GetHashCode()
		{
			int num = base.GetHashCode() * 19;
			List<ConfigRef<GameItemStaticData>> gameItems = this.GameItems;
			return (num + ((gameItems != null) ? new int?(gameItems.GetHashCode()) : null)).GetValueOrDefault();
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001A8C RID: 6796 RVA: 0x0005CAFD File Offset: 0x0005ACFD
		public override string Name
		{
			get
			{
				return "Control GameItems: " + ListExtensions.Join<ConfigRef<GameItemStaticData>>(this.GameItems, ", ");
			}
		}

		// Token: 0x04000C09 RID: 3081
		[JsonProperty]
		public bool MarkAsCompleteEarly;

		// Token: 0x04000C0A RID: 3082
		[JsonProperty]
		public List<ConfigRef<GameItemStaticData>> GameItems = new List<ConfigRef<GameItemStaticData>>();
	}
}

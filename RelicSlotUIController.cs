using System;
using Core.StaticData;
using Doozy.Runtime.UIManager.Containers;
using LoG.Core.UIManager;
using UnityEngine;

namespace LoG
{
	// Token: 0x020004AC RID: 1196
	public class RelicSlotUIController : UIController
	{
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06002379 RID: 9081 RVA: 0x00095458 File Offset: 0x00093658
		public ConfigRef SlottedRelicConfigRef
		{
			get
			{
				return this._slottedRelicConfigRef;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x0600237A RID: 9082 RVA: 0x00095460 File Offset: 0x00093660
		public DragRelicUIController SlottedRelic
		{
			get
			{
				return this._slottedRelic;
			}
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x00095468 File Offset: 0x00093668
		public void SetHovered(bool isHovered)
		{
			if (isHovered)
			{
				this._slotHighlight.Show();
				return;
			}
			this._slotHighlight.Hide();
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x00095484 File Offset: 0x00093684
		public void Slot(DragRelicUIController relic, ConfigRef config)
		{
			if (this._slottedRelic == relic)
			{
				return;
			}
			if (this._slottedRelic != null)
			{
				this.HandlePreviousSlotted(relic, config);
				return;
			}
			if (relic.IsSlotted())
			{
				relic.UnslotRelic();
			}
			this.SimpleSlot(relic, config);
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x000954C4 File Offset: 0x000936C4
		private void HandlePreviousSlotted(DragRelicUIController relic, ConfigRef config)
		{
			DragRelicUIController slottedRelic = this._slottedRelic;
			if (relic.IsSlotted())
			{
				RelicSlotUIController relicSlotUIController = relic.GetMaxSlot();
				RelicSlotUIController relicSlotUIController2 = slottedRelic.GetMinSlot();
				if (relicSlotUIController.SlotIndex < this.SlotIndex)
				{
					relicSlotUIController = relic.GetMinSlot();
					relicSlotUIController2 = slottedRelic.GetMaxSlot();
				}
				ConfigRef slottedRelicConfigRef = this._slottedRelicConfigRef;
				slottedRelic.SwapAssignedSlot(relicSlotUIController2, relicSlotUIController);
				relic.SwapAssignedSlot(relicSlotUIController, relicSlotUIController2);
				relicSlotUIController.SimpleSlot(slottedRelic, slottedRelicConfigRef);
				relicSlotUIController2.SimpleSlot(relic, config);
				return;
			}
			slottedRelic.UnslotRelic();
			this.SimpleSlot(relic, config);
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x00095540 File Offset: 0x00093740
		public void SimpleSlot(DragRelicUIController relic, ConfigRef config)
		{
			this._slottedRelic = relic;
			this._slottedRelicConfigRef = config;
			relic.AddAssignedSlot(this);
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x00095557 File Offset: 0x00093757
		public void Unslot()
		{
			this._slottedRelic = null;
			this._slottedRelicConfigRef = null;
		}

		// Token: 0x04001BE2 RID: 7138
		[SerializeField]
		private UIContainer _slotHighlight;

		// Token: 0x04001BE3 RID: 7139
		public int SlotIndex;

		// Token: 0x04001BE4 RID: 7140
		private ConfigRef _slottedRelicConfigRef;

		// Token: 0x04001BE5 RID: 7141
		private DragRelicUIController _slottedRelic;
	}
}

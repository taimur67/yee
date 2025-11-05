using System;
using System.Collections.Generic;
using System.Linq;
using AK.Wwise;
using Castle.Core.Internal;
using LoG.Simulation.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace LoG
{
	// Token: 0x0200043F RID: 1087
	public class DragRelicUIController : RelicUIController, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x0007DD43 File Offset: 0x0007BF43
		public List<RelicSlotUIController> CurrentSlots
		{
			get
			{
				return this._currentSlots;
			}
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x0007DD4C File Offset: 0x0007BF4C
		public bool IsSlotted()
		{
			int num = this.Type.SlotValue();
			return this._currentSlots.Count >= num;
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0007DD78 File Offset: 0x0007BF78
		public RelicSlotUIController GetMinSlot()
		{
			if (CollectionExtensions.IsNullOrEmpty(this._currentSlots))
			{
				return null;
			}
			this._currentSlots.SortOnValueAscending((RelicSlotUIController x) => x.SlotIndex);
			return IEnumerableExtensions.FirstOrDefault<RelicSlotUIController>(this._currentSlots);
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x0007DDCC File Offset: 0x0007BFCC
		public RelicSlotUIController GetMaxSlot()
		{
			if (CollectionExtensions.IsNullOrEmpty(this._currentSlots))
			{
				return null;
			}
			this._currentSlots.SortOnValueAscending((RelicSlotUIController x) => x.SlotIndex);
			return this._currentSlots.LastOrDefault<RelicSlotUIController>();
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0007DE1D File Offset: 0x0007C01D
		public void SwapAssignedSlot(RelicSlotUIController oldSlot, RelicSlotUIController newSlot)
		{
			this._currentSlots.Remove(oldSlot);
			this._currentSlots.Add(newSlot);
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0007DE38 File Offset: 0x0007C038
		public void AddAssignedSlot(RelicSlotUIController slot)
		{
			this._currentSlots.Add(slot);
			this._currentSlots = IEnumerableExtensions.ToList<RelicSlotUIController>(this._currentSlots.Distinct<RelicSlotUIController>());
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0007DE5C File Offset: 0x0007C05C
		public void ClearAssignedSlots()
		{
			this._currentSlots.Clear();
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0007DE6C File Offset: 0x0007C06C
		public void OnDrag(PointerEventData eventData)
		{
			this._dragRootTransform.position = eventData.position;
			List<RelicSlotUIController> list;
			if (this.TryGetOverlappingSlot(out list))
			{
				List<RelicSlotUIController> list2 = new List<RelicSlotUIController>();
				foreach (RelicSlotUIController relicSlotUIController in list)
				{
					List<RelicSlotUIController> list3 = list2;
					IEnumerable<RelicSlotUIController> collection;
					if (!(relicSlotUIController.SlottedRelic != null))
					{
						IEnumerable<RelicSlotUIController> enumerable = new RelicSlotUIController[]
						{
							relicSlotUIController
						};
						collection = enumerable;
					}
					else
					{
						IEnumerable<RelicSlotUIController> enumerable = relicSlotUIController.SlottedRelic.CurrentSlots;
						collection = enumerable;
					}
					list3.AddRange(collection);
				}
				using (List<RelicSlotUIController>.Enumerator enumerator = this._slots.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RelicSlotUIController relicSlotUIController2 = enumerator.Current;
						relicSlotUIController2.SetHovered(list2.Contains(relicSlotUIController2));
					}
					return;
				}
			}
			foreach (RelicSlotUIController relicSlotUIController3 in this._slots)
			{
				relicSlotUIController3.SetHovered(false);
			}
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0007DF9C File Offset: 0x0007C19C
		public void OnBeginDrag(PointerEventData eventData)
		{
			this._canvasGroup.blocksRaycasts = false;
			int sortingOrder = this._canvas.sortingOrder;
			this._canvas.overrideSorting = true;
			this._canvas.sortingOrder = sortingOrder + 1;
			Event grabAudioEvent = this._grabAudioEvent;
			if (grabAudioEvent != null)
			{
				grabAudioEvent.Post(base.gameObject);
			}
			this._relicLoadoutUIController.SetDragging(true);
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0007E000 File Offset: 0x0007C200
		public void OnEndDrag(PointerEventData eventData)
		{
			this._canvasGroup.blocksRaycasts = true;
			this._canvas.overrideSorting = false;
			List<RelicSlotUIController> list;
			if (this.TryGetOverlappingSlot(out list))
			{
				List<RelicSlotUIController> list2 = IEnumerableExtensions.ToList<RelicSlotUIController>(this._slots.Where(delegate(RelicSlotUIController x)
				{
					if (x.SlottedRelic == null)
					{
						return false;
					}
					int num = (x.SlottedRelic == this) ? 1 : 0;
					bool flag = base.RelicConfigRef != null && base.RelicConfigRef.Equals(x.SlottedRelicConfigRef);
					return num == 0 && flag;
				}));
				if (!CollectionExtensions.IsNullOrEmpty(list2))
				{
					IEnumerableExtensions.First<RelicSlotUIController>(list2).SlottedRelic.UnslotRelic();
				}
				using (List<RelicSlotUIController>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RelicSlotUIController relicSlotUIController = enumerator.Current;
						Event slottedAudioEvent = this._slottedAudioEvent;
						if (slottedAudioEvent != null)
						{
							slottedAudioEvent.Post(base.gameObject);
						}
						relicSlotUIController.Slot(this, base.RelicConfigRef);
					}
					goto IL_C5;
				}
			}
			Event droppedAudioEvent = this._droppedAudioEvent;
			if (droppedAudioEvent != null)
			{
				droppedAudioEvent.Post(base.gameObject);
			}
			this.UnslotRelic();
			IL_C5:
			this._relicLoadoutUIController.UpdateSelected();
			TransformExtensions.GetRectTransform(this._dragRootTransform).localPosition = Vector3.zero;
			foreach (RelicSlotUIController relicSlotUIController2 in this._slots)
			{
				relicSlotUIController2.SetHovered(false);
			}
			this._relicLoadoutUIController.SetDragging(false);
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0007E150 File Offset: 0x0007C350
		public void UnslotRelic()
		{
			if (!CollectionExtensions.IsNullOrEmpty(this.CurrentSlots))
			{
				foreach (RelicSlotUIController relicSlotUIController in this.CurrentSlots)
				{
					relicSlotUIController.Unslot();
				}
				this.ClearAssignedSlots();
			}
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0007E1B4 File Offset: 0x0007C3B4
		private bool TryGetOverlappingSlot(out List<RelicSlotUIController> validSlot)
		{
			validSlot = new List<RelicSlotUIController>();
			if (CollectionExtensions.IsNullOrEmpty(this._slots))
			{
				return false;
			}
			Rect dragWorldRect = TransformExtensions.GetRectTransform(this._dragRootTransform).GetWorldRect();
			foreach (RelicSlotUIController relicSlotUIController in this._slots)
			{
				if (ComponentExtensions.GetRectTransform<RelicSlotUIController>(relicSlotUIController).GetWorldRect().Overlaps(dragWorldRect))
				{
					validSlot.Add(relicSlotUIController);
				}
			}
			if (validSlot.Count < this.Type.SlotValue())
			{
				return false;
			}
			validSlot.SortOnValueAscending((RelicSlotUIController x) => DragRelicUIController.GetDistance(x, dragWorldRect));
			validSlot = validSlot.GetRange(0, this.Type.SlotValue());
			return !CollectionExtensions.IsNullOrEmpty(validSlot);
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0007E2A0 File Offset: 0x0007C4A0
		private static float GetDistance(RelicSlotUIController slot, Rect dragWorldRect)
		{
			return Vector2.Distance(ComponentExtensions.GetRectTransform<RelicSlotUIController>(slot).GetWorldRect().center, dragWorldRect.center);
		}

		// Token: 0x040016CF RID: 5839
		[SerializeField]
		protected Canvas _canvas;

		// Token: 0x040016D0 RID: 5840
		[SerializeField]
		protected CanvasGroup _canvasGroup;

		// Token: 0x040016D1 RID: 5841
		[SerializeField]
		protected Transform _dragRootTransform;

		// Token: 0x040016D2 RID: 5842
		[SerializeField]
		protected Event _grabAudioEvent;

		// Token: 0x040016D3 RID: 5843
		[SerializeField]
		protected Event _slottedAudioEvent;

		// Token: 0x040016D4 RID: 5844
		[SerializeField]
		protected Event _droppedAudioEvent;

		// Token: 0x040016D5 RID: 5845
		[Inject]
		private List<RelicSlotUIController> _slots;

		// Token: 0x040016D6 RID: 5846
		[Inject]
		private RelicLoadoutUIController _relicLoadoutUIController;

		// Token: 0x040016D7 RID: 5847
		private List<RelicSlotUIController> _currentSlots = new List<RelicSlotUIController>();
	}
}

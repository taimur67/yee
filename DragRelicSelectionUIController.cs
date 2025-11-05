using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using UnityEngine;
using Zenject;

namespace LoG
{
	// Token: 0x0200043E RID: 1086
	public class DragRelicSelectionUIController : RelicSelectionUIController
	{
		// Token: 0x06001EB6 RID: 7862 RVA: 0x0007D95C File Offset: 0x0007BB5C
		public void SetupRelicSlotsFromConfigRefs(List<ConfigRef> relicConfigRefs, List<RelicSlotUIController> slots, RelicLoadoutUIController loadoutUIController, Gender gender)
		{
			this._relicLayout.transform.DestroyChildrenOnly(false);
			foreach (RelicSlotUIController relicSlotUIController in slots)
			{
				relicSlotUIController.Unslot();
			}
			int num = 0;
			foreach (ConfigRef configRef in relicConfigRefs)
			{
				Pair<RelicStaticData, GameItemClientDataComponent> relicInfo = configRef.GetRelicInfo(this._dataAccessor, this._gameDatabase);
				RelicType type = relicInfo.First.Type;
				DragRelicUIController dragRelicUIController = UnityEngine.Object.Instantiate<DragRelicUIController>(this._relicTemplates.FirstOrDefault((RelicUIController x) => x.Type == type) as DragRelicUIController, this._relicLayout.transform);
				this.InjectRelicUI(slots, dragRelicUIController, loadoutUIController);
				this.SetupRelicUI(gender, relicInfo, dragRelicUIController, configRef);
				int num2 = relicInfo.First.Type.SlotValue();
				foreach (RelicSlotUIController relicSlotUIController2 in slots.GetRange(num, num2))
				{
					relicSlotUIController2.SimpleSlot(dragRelicUIController, configRef);
					dragRelicUIController.AddAssignedSlot(relicSlotUIController2);
				}
				num += num2;
			}
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0007DAD8 File Offset: 0x0007BCD8
		public void UpdateSelectedRelics(List<RelicSlotUIController> slots, RelicLoadoutUIController loadoutUIController, Gender gender)
		{
			this._relicLayout.transform.DestroyChildrenOnly(false);
			List<DragRelicUIController> list = new List<DragRelicUIController>();
			foreach (RelicSlotUIController relicSlotUIController in slots)
			{
				DragRelicUIController slottedRelic = relicSlotUIController.SlottedRelic;
				if (!(slottedRelic != null) || !list.Any((DragRelicUIController x) => x == slottedRelic))
				{
					RelicType type = RelicType.None;
					Pair<RelicStaticData, GameItemClientDataComponent> pair = null;
					ConfigRef slottedRelicConfigRef = null;
					if (slottedRelic != null)
					{
						slottedRelicConfigRef = slottedRelic.RelicConfigRef;
						pair = slottedRelicConfigRef.GetRelicInfo(this._dataAccessor, this._gameDatabase);
						type = pair.First.Type;
					}
					RelicUIController relicUIController = UnityEngine.Object.Instantiate<RelicUIController>(this._relicTemplates.FirstOrDefault((RelicUIController x) => x.Type == type), this._relicLayout.transform);
					DragRelicUIController dragRelicUIController = relicUIController as DragRelicUIController;
					if (dragRelicUIController != null)
					{
						this.InjectRelicUI(slots, dragRelicUIController, loadoutUIController);
						this.SetupRelicUI(gender, pair, relicUIController, slottedRelicConfigRef);
						foreach (RelicSlotUIController relicSlotUIController2 in IEnumerableExtensions.ToList<RelicSlotUIController>(from x in slots
						where slottedRelicConfigRef != null && slottedRelicConfigRef.Equals(x.SlottedRelicConfigRef)
						select x))
						{
							relicSlotUIController2.Slot(dragRelicUIController, slottedRelicConfigRef);
						}
						list.Add(dragRelicUIController);
					}
				}
			}
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x0007DC9C File Offset: 0x0007BE9C
		private void SetupRelicUI(Gender gender, Pair<RelicStaticData, GameItemClientDataComponent> relicInfo, RelicUIController relicUI, ConfigRef configRef)
		{
			RelicStaticData first = relicInfo.First;
			ModifierStaticData modifierStaticData = (first != null) ? IEnumerableExtensions.FirstOrDefault<ModifierStaticData>(first.GetModifiers()) : null;
			if (modifierStaticData == null)
			{
				GameDatabase gameDatabase = this._gameDatabase;
				RelicStaticData first2 = relicInfo.First;
				StaticDataEntity staticDataEntity = gameDatabase.Fetch<StaticDataEntity>((first2 != null) ? IEnumerableExtensions.FirstOrDefault<string>(IEnumerableExtensions.ToList<string>(first2.ProvidedAbilities)) : null);
				modifierStaticData = ((staticDataEntity != null) ? IEnumerableExtensions.FirstOrDefault<ModifierStaticData>(staticDataEntity.GetModifiers()) : null);
			}
			relicUI.Setup(relicInfo.Second, modifierStaticData, configRef, gender, null);
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x0007DD0E File Offset: 0x0007BF0E
		private void InjectRelicUI(List<RelicSlotUIController> slots, DragRelicUIController relicUI, RelicLoadoutUIController loadoutUIController)
		{
			DiContainer diContainer = new DiContainer(this._injector);
			diContainer.Rebind<List<RelicSlotUIController>>().FromInstance(slots);
			diContainer.Rebind<RelicLoadoutUIController>().FromInstance(loadoutUIController);
			diContainer.Inject(relicUI);
		}

		// Token: 0x040016CE RID: 5838
		[Inject]
		private DiContainer _injector;
	}
}

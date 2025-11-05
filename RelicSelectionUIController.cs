using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LoG
{
	// Token: 0x020004AB RID: 1195
	public class RelicSelectionUIController : MonoBehaviour
	{
		// Token: 0x06002377 RID: 9079 RVA: 0x00095278 File Offset: 0x00093478
		public void SetRelics(List<ConfigRef> relicConfigRefs, Gender gender, ArchfiendUIContext kingmakerTargetCtx = null)
		{
			this._relicLayout.transform.DestroyChildrenOnly(false);
			RelicsEconomyData relicsEconomyData = this._gameDatabase.FetchSingle<RelicsEconomyData>();
			int num = (relicsEconomyData != null) ? relicsEconomyData.MaxRelicsValue : 3;
			int num2 = 0;
			foreach (ConfigRef configRef in relicConfigRefs)
			{
				RelicType type = RelicType.None;
				Pair<RelicStaticData, GameItemClientDataComponent> pair = null;
				if (configRef != null)
				{
					pair = configRef.GetRelicInfo(this._dataAccessor, this._gameDatabase);
					type = pair.First.Type;
				}
				RelicUIController relicUIController = UnityEngine.Object.Instantiate<RelicUIController>(this._relicTemplates.FirstOrDefault((RelicUIController x) => x.Type == type), this._relicLayout.transform);
				num2 += type.SlotValue();
				if (pair != null)
				{
					this._container.Inject(relicUIController);
					RelicStaticData first = pair.First;
					ModifierStaticData modifierStaticData = (first != null) ? IEnumerableExtensions.FirstOrDefault<ModifierStaticData>(first.GetModifiers()) : null;
					if (modifierStaticData == null)
					{
						GameDatabase gameDatabase = this._gameDatabase;
						RelicStaticData first2 = pair.First;
						StaticDataEntity staticDataEntity = gameDatabase.Fetch<StaticDataEntity>((first2 != null) ? IEnumerableExtensions.FirstOrDefault<string>(IEnumerableExtensions.ToList<string>(first2.ProvidedAbilities)) : null);
						modifierStaticData = ((staticDataEntity != null) ? IEnumerableExtensions.FirstOrDefault<ModifierStaticData>(staticDataEntity.GetModifiers()) : null);
					}
					relicUIController.Setup(pair.Second, modifierStaticData, configRef, gender, kingmakerTargetCtx);
				}
			}
			RelicUIController relicUIController2;
			if (!this._showEmptySlotsAsUnknown)
			{
				relicUIController2 = this._relicTemplates.FirstOrDefault((RelicUIController x) => x.Type == RelicType.None);
			}
			else
			{
				relicUIController2 = this._unknownRelicTemplate;
			}
			RelicUIController original = relicUIController2;
			for (int i = num2; i < num; i++)
			{
				UnityEngine.Object.Instantiate<RelicUIController>(original, this._relicLayout.transform);
			}
		}

		// Token: 0x04001BDB RID: 7131
		[Inject]
		protected DiContainer _container;

		// Token: 0x04001BDC RID: 7132
		[Inject]
		protected ClientDataAccessor _dataAccessor;

		// Token: 0x04001BDD RID: 7133
		[Inject]
		protected GameDatabase _gameDatabase;

		// Token: 0x04001BDE RID: 7134
		[SerializeField]
		private bool _showEmptySlotsAsUnknown;

		// Token: 0x04001BDF RID: 7135
		[SerializeField]
		protected LayoutGroup _relicLayout;

		// Token: 0x04001BE0 RID: 7136
		[SerializeField]
		protected List<RelicUIController> _relicTemplates;

		// Token: 0x04001BE1 RID: 7137
		[SerializeField]
		protected RelicUIController _unknownRelicTemplate;
	}
}

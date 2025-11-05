using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Castle.Core.Internal;
using Core.StaticData;
using Doozy.Runtime.UIManager.Components;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Core.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Zenject;

namespace LoG
{
	// Token: 0x0200048C RID: 1164
	public class RelicLoadoutUIController : MenuUIController
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x0008F3DC File Offset: 0x0008D5DC
		private LoadoutSettings _loadoutSettings
		{
			get
			{
				return this._userSettingsService.UserSettings.Loadout;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x0600222F RID: 8751 RVA: 0x0008F3EE File Offset: 0x0008D5EE
		private bool _hasLoadoutChanges
		{
			get
			{
				return !this._equippedRelics.SequenceEqual(this._selectedRelics);
			}
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x0008F404 File Offset: 0x0008D604
		public void Start()
		{
			this._gender = this._dataAccessor.ArchfiendClientDataComponent(this._archfiendId).Gender;
			this._equippedRelics = this.RelicValidationAndSettingsCleanup(this._archfiendId);
			this._selectedRelics = this._equippedRelics;
			this.Setup();
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x0008F454 File Offset: 0x0008D654
		private List<ConfigRef> RelicValidationAndSettingsCleanup(string archfiendDataId)
		{
			RelicSetStaticData relicSetStaticData;
			if (!this._loadoutSettings.RelicSettings.TryGetValue(archfiendDataId, out relicSetStaticData))
			{
				ArchFiendStaticData archFiendStaticData = this._gameDatabase.Fetch<ArchFiendStaticData>(archfiendDataId);
				return new List<ConfigRef>
				{
					archFiendStaticData.DefaultRelics
				};
			}
			List<ConfigRef> list;
			if (relicSetStaticData.Relics.Count > 0 && !this._gameDatabase.IsValidRelicConfigRefs(relicSetStaticData, out list))
			{
				RelicSetStaticData value = new RelicSetStaticData
				{
					Relics = list
				};
				this._userSettingsService.UserSettings.Loadout.RelicSettings[archfiendDataId] = value;
				this._userSettingsService.SaveUserSettingsToFile();
				return list;
			}
			return relicSetStaticData.Relics;
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x0008F4F0 File Offset: 0x0008D6F0
		private void Setup()
		{
			this.UpdateConfirmButton();
			this.SetupRelicDictionary();
			DiContainer diContainer = new DiContainer(this._injector);
			diContainer.Bind<RelicLoadoutUIController>().FromInstance(this);
			diContainer.Inject(this._relicSelection);
			this._relicSelection.SetupRelicSlotsFromConfigRefs(this._selectedRelics, this._slots, this, this._gender);
			this.ValidateRelicsInSelection();
			this.SetRingsPage();
			this.UpdateArchfiendStats();
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x0008F55C File Offset: 0x0008D75C
		private void SetupRelicDictionary()
		{
			List<RelicStaticData> value = IEnumerableExtensions.ToList<RelicStaticData>(this._gameDatabase.GetAllRings());
			List<RelicStaticData> value2 = IEnumerableExtensions.ToList<RelicStaticData>(this._gameDatabase.GetAllAmulets());
			List<RelicStaticData> value3 = IEnumerableExtensions.ToList<RelicStaticData>(this._gameDatabase.GetAllCrowns());
			this._relicStaticData.Add(RelicType.Ring, value);
			this._relicStaticData.Add(RelicType.Amulet, value2);
			this._relicStaticData.Add(RelicType.Crown, value3);
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x0008F5C3 File Offset: 0x0008D7C3
		[UICallback]
		public void SetRingsPage()
		{
			this.SetupPage(RelicType.Ring, this._gender);
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x0008F5D2 File Offset: 0x0008D7D2
		[UICallback]
		public void SetAmuletsPage()
		{
			this.SetupPage(RelicType.Amulet, this._gender);
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x0008F5E1 File Offset: 0x0008D7E1
		[UICallback]
		public void SetCrownsPage()
		{
			this.SetupPage(RelicType.Crown, this._gender);
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x0008F5F0 File Offset: 0x0008D7F0
		[UICallback]
		public void Back()
		{
			if (this._hasLoadoutChanges)
			{
				if (this._uiManager != null)
				{
					ArchfiendClientDataComponent archfiendClientDataComponent = this._dataAccessor.ArchfiendClientDataComponent(this._archfiendId);
					LocalizedString localizedString = new LocalizedString();
					LocalizedReferenceExtensions.SetFromReference(localizedString, LocalizationKeys.Messages.RelicPendingChangesBody);
					LocalizedStringExtensions.SetLocalizedStringParam(localizedString, "archfiend", archfiendClientDataComponent.Name);
					this._uiManager.PushMessage().SetTitle(LocalizationKeys.Messages.RelicPendingChangesTitle).SetBody(localizedString).AddButton(LocalizationKeys.Button.ConfirmChanges, new Action(this.SaveAndClose), true, false).AddButton(LocalizationKeys.Button.DiscardChanges, new Action(base.PopView), true, false);
					return;
				}
			}
			else
			{
				base.PopView();
			}
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x0008F6B5 File Offset: 0x0008D8B5
		public override void Reset()
		{
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x0008F6B7 File Offset: 0x0008D8B7
		public void UpdateSelected()
		{
			this._selectedRelics = this.GetSelectedRelicsFromSlots();
			this.ValidateRelicsInSelection();
			this._relicSelection.UpdateSelectedRelics(this._slots, this, this._gender);
			this.UpdateConfirmButton();
			this.UpdateArchfiendStats();
			this.UpdateSelectedRelicOptions();
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x0008F6F8 File Offset: 0x0008D8F8
		private void ValidateRelicsInSelection()
		{
			int num = 0;
			int maxRelicsValue = this._gameDatabase.FetchSingle<RelicsEconomyData>().MaxRelicsValue;
			using (List<ConfigRef>.Enumerator enumerator = this._selectedRelics.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ConfigRef relic = enumerator.Current;
					IEnumerable<RelicSlotUIController> enumerable = from x in this._slots
					where x.SlottedRelicConfigRef == relic
					select x;
					int num2 = relic.GetRelicInfo(this._dataAccessor, this._gameDatabase).First.Type.SlotValue();
					num += num2;
					if (enumerable.Count<RelicSlotUIController>() < num2 || num > maxRelicsValue)
					{
						foreach (RelicSlotUIController relicSlotUIController in enumerable)
						{
							relicSlotUIController.Unslot();
						}
					}
				}
			}
			this._selectedRelics = this.GetSelectedRelicsFromSlots();
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x0008F804 File Offset: 0x0008DA04
		private void SaveAndClose()
		{
			this.ValidateRelicsInSelection();
			this.SaveLoadout();
			base.PopView();
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x0008F818 File Offset: 0x0008DA18
		private void UpdateArchfiendStats()
		{
			ArchFiendStaticData archfiendData = this._gameDatabase.Fetch<ArchFiendStaticData>(this._archfiendId);
			RelicSetStaticData relicSet = new RelicSetStaticData
			{
				Relics = this._selectedRelics
			};
			this._archfiendSelectionUIController.SetArchfiendPreviewGameState(archfiendData, relicSet);
			this._statsUI.Setup(this._gameStateService.CurrentPlayer.PowersLevels, true, null);
			Rank rank = this._gameStateService.CurrentPlayer.Rank;
			ArchfiendRankStaticData archfiendRank = this._gameDatabase.GetArchfiendRank(rank);
			ArchfiendRankClientDataComponent archfiendRankClientDataComponent = this._dataAccessor.ArchfiendRankClientDataComponent(archfiendRank.Id);
			this._rank.sprite = archfiendRankClientDataComponent.Icon;
			ArchfiendClientDataComponent archfiendClientDataComponent = this._dataAccessor.ArchfiendClientDataComponent(this._archfiendId);
			this._sigil.sprite = archfiendClientDataComponent.Sigil;
			this._sigilRing.color = archfiendClientDataComponent.GetArchfiendColor();
			GenderMetadata genderMetadata = new GenderMetadata(archfiendClientDataComponent.Gender);
			UIContext.SetupRankContext(this._injector, rank, genderMetadata).Inject(this._rankTooltip);
			List<LocalizedString> list = new List<LocalizedString>();
			foreach (ConfigRef relicConfig in this._selectedRelics)
			{
				Pair<RelicStaticData, GameItemClientDataComponent> relicInfo = relicConfig.GetRelicInfo(this._dataAccessor, this._gameDatabase);
				RelicStaticData first = relicInfo.First;
				ModifierStaticData modifierStaticData = (first != null) ? IEnumerableExtensions.FirstOrDefault<ModifierStaticData>(first.GetModifiers()) : null;
				if (modifierStaticData == null)
				{
					GameDatabase gameDatabase = this._gameDatabase;
					RelicStaticData first2 = relicInfo.First;
					StaticDataEntity staticDataEntity = gameDatabase.Fetch<StaticDataEntity>((first2 != null) ? IEnumerableExtensions.FirstOrDefault<string>(IEnumerableExtensions.ToList<string>(first2.ProvidedAbilities)) : null);
					modifierStaticData = ((staticDataEntity != null) ? IEnumerableExtensions.FirstOrDefault<ModifierStaticData>(staticDataEntity.GetModifiers()) : null);
				}
				list.Add(LocalizationKeys.Modifiers.GetLocalizedStringWithModifiers(relicInfo.Second.Description, modifierStaticData, this._gender, this._gameStateService.Rules ?? new GameRules()));
			}
			if (CollectionExtensions.IsNullOrEmpty(list))
			{
				this._listRelicDescriptions.gameObject.SetActive(false);
				return;
			}
			LocalizedString localizedString = new LocalizedString();
			LocalizedReferenceExtensions.SetFromKey(localizedString, LocalizationKeys.Generic.List);
			localizedString.Arguments = new object[]
			{
				list
			};
			this._listRelicDescriptions.gameObject.SetActive(true);
			this._listRelicDescriptions.StringReference = localizedString;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x0008FA60 File Offset: 0x0008DC60
		private void UpdateConfirmButton()
		{
			this._confirmAttention.SetAttentionStateVisual(this._hasLoadoutChanges);
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x0008FA74 File Offset: 0x0008DC74
		private List<ConfigRef> GetSelectedRelicsFromSlots()
		{
			return IEnumerableExtensions.ToList<ConfigRef>((from x in this._slots
			where x.SlottedRelicConfigRef != null
			select x.SlottedRelicConfigRef).Distinct<ConfigRef>());
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x0008FADC File Offset: 0x0008DCDC
		[UICallback]
		public void SaveLoadout()
		{
			this._equippedRelics = this._selectedRelics;
			this._loadoutSettings.RelicSettings[this._archfiendId] = new RelicSetStaticData
			{
				Relics = this._equippedRelics
			};
			this._userSettingsService.SaveUserSettingsToFile();
			this._archfiendSelectionUIController.Reset();
			this.UpdateConfirmButton();
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x0008FB38 File Offset: 0x0008DD38
		private void SetupPage(RelicType relicType, Gender gender)
		{
			this._relicPageLayout.transform.DestroyChildrenOnly(false);
			RelicUIController relicUIController = this._relicTemplates.FirstOrDefault((RelicUIController x) => x.Type == relicType);
			LayoutElement component = relicUIController.GetComponent<LayoutElement>();
			this._relicPageLayout.cellSize = new Vector2(component.preferredWidth, component.preferredHeight);
			GridLayoutGroup relicPageLayout = this._relicPageLayout;
			int constraintCount;
			switch (relicType)
			{
			case RelicType.Ring:
				constraintCount = this._ringColumnCount;
				break;
			case RelicType.Amulet:
				constraintCount = this._amuletColumnCount;
				break;
			case RelicType.Crown:
				constraintCount = this._crownColumnCount;
				break;
			default:
				constraintCount = this._relicPageLayout.constraintCount;
				break;
			}
			relicPageLayout.constraintCount = constraintCount;
			foreach (RelicStaticData relicStaticData in this._relicStaticData[relicType])
			{
				RelicUIController relicUIController2 = UnityEngine.Object.Instantiate<RelicUIController>(relicUIController, this._relicPageLayout.transform);
				Pair<RelicStaticData, GameItemClientDataComponent> relicInfo = relicStaticData.ConfigRef.GetRelicInfo(this._dataAccessor, this._gameDatabase);
				DiContainer diContainer = new DiContainer(this._injector);
				diContainer.Bind<List<RelicSlotUIController>>().FromInstance(this._slots);
				diContainer.Bind<RelicLoadoutUIController>().FromInstance(this);
				diContainer.Inject(relicUIController2);
				RelicStaticData first = relicInfo.First;
				ModifierStaticData modifierStaticData = (first != null) ? IEnumerableExtensions.FirstOrDefault<ModifierStaticData>(first.GetModifiers()) : null;
				if (modifierStaticData == null)
				{
					GameDatabase gameDatabase = this._gameDatabase;
					RelicStaticData first2 = relicInfo.First;
					StaticDataEntity staticDataEntity = gameDatabase.Fetch<StaticDataEntity>((first2 != null) ? IEnumerableExtensions.FirstOrDefault<string>(IEnumerableExtensions.ToList<string>(first2.ProvidedAbilities)) : null);
					modifierStaticData = ((staticDataEntity != null) ? IEnumerableExtensions.FirstOrDefault<ModifierStaticData>(staticDataEntity.GetModifiers()) : null);
				}
				relicUIController2.Setup(relicInfo.Second, modifierStaticData, relicStaticData.ConfigRef, gender, null);
			}
			this.UpdateSelectedRelicOptions();
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x0008FD20 File Offset: 0x0008DF20
		private void UpdateSelectedRelicOptions()
		{
			RelicUIController[] componentsInChildren = this._relicPageLayout.GetComponentsInChildren<RelicUIController>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				RelicUIController relic = componentsInChildren[i];
				bool selected = this._selectedRelics.Any((ConfigRef x) => x.Equals(relic.RelicConfigRef));
				relic.SetSelected(selected);
			}
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x0008FD7C File Offset: 0x0008DF7C
		public void SetDragging(bool isRelicDragging)
		{
			List<RelicUIController> list = new List<RelicUIController>();
			list.AddRange(this._relicPageLayout.GetComponentsInChildren<RelicUIController>());
			list.AddRange(this._relicSelection.GetComponentsInChildren<RelicUIController>());
			foreach (RelicUIController relicUIController in list)
			{
				if (relicUIController.Tooltip != null)
				{
					if (isRelicDragging)
					{
						relicUIController.Tooltip.SetTooltipOff();
					}
					else
					{
						relicUIController.Tooltip.SetTooltipOn();
					}
				}
			}
		}

		// Token: 0x04001A65 RID: 6757
		[Inject]
		private DiContainer _injector;

		// Token: 0x04001A66 RID: 6758
		[Inject]
		private ClientDataAccessor _dataAccessor;

		// Token: 0x04001A67 RID: 6759
		[Inject]
		private GameDatabase _gameDatabase;

		// Token: 0x04001A68 RID: 6760
		[Inject]
		private UserSettingsService _userSettingsService;

		// Token: 0x04001A69 RID: 6761
		[Inject]
		private ArchfiendSelectionUIController _archfiendSelectionUIController;

		// Token: 0x04001A6A RID: 6762
		[Inject]
		private GameStateService _gameStateService;

		// Token: 0x04001A6B RID: 6763
		[Inject]
		private string _archfiendId;

		// Token: 0x04001A6C RID: 6764
		[SerializeField]
		private DragRelicSelectionUIController _relicSelection;

		// Token: 0x04001A6D RID: 6765
		[SerializeField]
		private List<RelicSlotUIController> _slots;

		// Token: 0x04001A6E RID: 6766
		[SerializeField]
		private UIButton _confirmSelection;

		// Token: 0x04001A6F RID: 6767
		[SerializeField]
		private AttentionUIElement _confirmAttention;

		// Token: 0x04001A70 RID: 6768
		[SerializeField]
		private List<RelicUIController> _relicTemplates;

		// Token: 0x04001A71 RID: 6769
		[SerializeField]
		private GridLayoutGroup _relicPageLayout;

		// Token: 0x04001A72 RID: 6770
		[SerializeField]
		private int _ringColumnCount = 6;

		// Token: 0x04001A73 RID: 6771
		[SerializeField]
		private int _amuletColumnCount = 4;

		// Token: 0x04001A74 RID: 6772
		[SerializeField]
		private int _crownColumnCount = 3;

		// Token: 0x04001A75 RID: 6773
		[SerializeField]
		private ArchfiendStatsUI _statsUI;

		// Token: 0x04001A76 RID: 6774
		[SerializeField]
		private Image _sigil;

		// Token: 0x04001A77 RID: 6775
		[SerializeField]
		private Image _sigilRing;

		// Token: 0x04001A78 RID: 6776
		[SerializeField]
		private LocalizeStringEvent _listRelicDescriptions;

		// Token: 0x04001A79 RID: 6777
		[SerializeField]
		private Image _rank;

		// Token: 0x04001A7A RID: 6778
		[SerializeField]
		private DynamicContextTooltipTargetUI _rankTooltip;

		// Token: 0x04001A7B RID: 6779
		private List<ConfigRef> _equippedRelics;

		// Token: 0x04001A7C RID: 6780
		private List<ConfigRef> _selectedRelics;

		// Token: 0x04001A7D RID: 6781
		private Dictionary<RelicType, List<RelicStaticData>> _relicStaticData = new Dictionary<RelicType, List<RelicStaticData>>();

		// Token: 0x04001A7E RID: 6782
		private Gender _gender;
	}
}

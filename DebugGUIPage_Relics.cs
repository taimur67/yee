using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Core.Injection;
using LoG.SI2;
using UnityEngine;

namespace LoG
{
	// Token: 0x02000183 RID: 387
	public class DebugGUIPage_Relics : DebugGUIPage
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x0004112B File Offset: 0x0003F32B
		public override string DisplayName
		{
			get
			{
				return "Relic Loadouts";
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00041132 File Offset: 0x0003F332
		private string _archfiendId
		{
			get
			{
				return this._archfiendIds[this._archfiendIndex];
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x00041141 File Offset: 0x0003F341
		private ArchFiendStaticData _archfiendStaticData
		{
			get
			{
				return this._database.Fetch<ArchFiendStaticData>(this._archfiendId);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00041154 File Offset: 0x0003F354
		private int _currentRelicsValue
		{
			get
			{
				UserSettingsService settingsService = this._settingsService;
				LoadoutSettings loadoutSettings;
				if (settingsService == null)
				{
					loadoutSettings = null;
				}
				else
				{
					UserSettings userSettings = settingsService.UserSettings;
					loadoutSettings = ((userSettings != null) ? userSettings.Loadout : null);
				}
				LoadoutSettings loadoutSettings2 = loadoutSettings;
				if (loadoutSettings2 == null)
				{
					return 0;
				}
				RelicSetStaticData relicSetStaticData;
				if (!loadoutSettings2.RelicSettings.TryGetValue(this._archfiendId, out relicSetStaticData))
				{
					return 0;
				}
				return relicSetStaticData.Relics.Sum((ConfigRef x) => this._database.Fetch<RelicStaticData>(x.Id).RelicValue);
			}
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x000411B4 File Offset: 0x0003F3B4
		public override void OnPageOpened()
		{
			this._maxRelicsValue = this._database.FetchSingle<RelicsEconomyData>().MaxRelicsValue;
			this._archfiendIds = IEnumerableExtensions.ToArray<string>(from x in this._database.Enumerate<ArchFiendStaticData>()
			select x.Id);
			this._archfiendNames = IEnumerableExtensions.ToArray<string>(this._archfiendIds.Select(delegate(string x)
			{
				ArchfiendClientDataComponent archfiendClientDataComponent = this._clientData.ArchfiendClientDataComponent(x);
				return archfiendClientDataComponent.Name.GetLocalizedString().ToColoredString(archfiendClientDataComponent.GetArchfiendColor());
			}));
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00041233 File Offset: 0x0003F433
		public override void DrawContents()
		{
			this._archfiendIndex = this.DebugGUI.DrawMultilineToolbar(this._archfiendIndex, this._archfiendNames, 4);
			this.DrawArchfiendRelics();
			this.DrawRelics();
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00041260 File Offset: 0x0003F460
		private void DrawRelics()
		{
			UserSettingsService settingsService = this._settingsService;
			bool flag;
			if (settingsService == null)
			{
				flag = (null != null);
			}
			else
			{
				UserSettings userSettings = settingsService.UserSettings;
				flag = (((userSettings != null) ? userSettings.Loadout : null) != null);
			}
			if (!flag)
			{
				return;
			}
			if (this.DebugGUI.DrawFoldout("AllRelicsList", "Relics", null, false, false))
			{
				IOrderedEnumerable<RelicStaticData> items = from t in this._database.Enumerate<RelicStaticData>()
				orderby t.RelicValue, t.Id
				select t;
				this.DebugGUI.DrawFilterable<RelicStaticData>("Relics", items, ref this._relicSearch, new Action<RelicStaticData>(this.<DrawRelics>g__DrawRelicEntry|18_0));
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00041320 File Offset: 0x0003F520
		private void EquipRelic(string relicId)
		{
			UserSettingsService settingsService = this._settingsService;
			LoadoutSettings loadoutSettings;
			if (settingsService == null)
			{
				loadoutSettings = null;
			}
			else
			{
				UserSettings userSettings = settingsService.UserSettings;
				loadoutSettings = ((userSettings != null) ? userSettings.Loadout : null);
			}
			LoadoutSettings loadoutSettings2 = loadoutSettings;
			if (loadoutSettings2 == null)
			{
				return;
			}
			if (!loadoutSettings2.RelicSettings.ContainsKey(this._archfiendId))
			{
				loadoutSettings2.RelicSettings[this._archfiendId] = this._database.Fetch(this._archfiendStaticData.DefaultRelics);
			}
			RelicStaticData relicStaticData = this._database.Fetch<RelicStaticData>(relicId);
			int relicValue = relicStaticData.RelicValue;
			if (this._currentRelicsValue + relicValue > this._maxRelicsValue)
			{
				return;
			}
			loadoutSettings2.RelicSettings[this._archfiendId].Relics.Add(relicStaticData.ConfigRef);
			this._settingsService.SaveUserSettingsToFile();
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x000413DC File Offset: 0x0003F5DC
		private void UnEquipRelic(ConfigRef relicConfig)
		{
			UserSettingsService settingsService = this._settingsService;
			LoadoutSettings loadoutSettings;
			if (settingsService == null)
			{
				loadoutSettings = null;
			}
			else
			{
				UserSettings userSettings = settingsService.UserSettings;
				loadoutSettings = ((userSettings != null) ? userSettings.Loadout : null);
			}
			LoadoutSettings loadoutSettings2 = loadoutSettings;
			if (loadoutSettings2 == null)
			{
				return;
			}
			if (!loadoutSettings2.RelicSettings.ContainsKey(this._archfiendId))
			{
				loadoutSettings2.RelicSettings[this._archfiendId] = this._database.Fetch(this._archfiendStaticData.DefaultRelics);
			}
			loadoutSettings2.RelicSettings[this._archfiendId].Relics.Remove(relicConfig);
			this._settingsService.SaveUserSettingsToFile();
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00041470 File Offset: 0x0003F670
		private void DrawArchfiendRelics()
		{
			UserSettingsService settingsService = this._settingsService;
			LoadoutSettings loadoutSettings;
			if (settingsService == null)
			{
				loadoutSettings = null;
			}
			else
			{
				UserSettings userSettings = settingsService.UserSettings;
				loadoutSettings = ((userSettings != null) ? userSettings.Loadout : null);
			}
			LoadoutSettings loadoutSettings2 = loadoutSettings;
			if (loadoutSettings2 == null)
			{
				return;
			}
			if (this.DebugGUI.DrawFoldout("RelicsLoadout_" + this._archfiendId, string.Format("Loadout {0}/{1}", this._currentRelicsValue, this._maxRelicsValue), null, false, false))
			{
				RelicSetStaticData relicSetStaticData;
				if (!loadoutSettings2.RelicSettings.TryGetValue(this._archfiendId, out relicSetStaticData))
				{
					relicSetStaticData = (loadoutSettings2.RelicSettings[this._archfiendId] = this._database.Fetch(this._archfiendStaticData.DefaultRelics));
				}
				foreach (ConfigRef configRef in IEnumerableExtensions.ToList<ConfigRef>(relicSetStaticData.Relics))
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label(configRef.Id, Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Un-equip Relic", new GUILayoutOption[]
					{
						GUILayout.Width(200f)
					}))
					{
						this.UnEquipRelic(configRef);
					}
					GUILayout.EndHorizontal();
				}
			}
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0004160C File Offset: 0x0003F80C
		[CompilerGenerated]
		private void <DrawRelics>g__DrawRelicEntry|18_0(RelicStaticData relicStaticData)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label(string.Format("<b>[{0}]</b> {1}", relicStaticData.RelicValue, relicStaticData.Id), Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Equip to " + this._archfiendNames[this._archfiendIndex], new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			}))
			{
				this.EquipRelic(relicStaticData.Id);
			}
			GUILayout.EndHorizontal();
		}

		// Token: 0x04000A30 RID: 2608
		[InjectField]
		private UserSettingsService _settingsService;

		// Token: 0x04000A31 RID: 2609
		[InjectField]
		private GameDatabase _database;

		// Token: 0x04000A32 RID: 2610
		[InjectField]
		private ClientDataAccessor _clientData;

		// Token: 0x04000A33 RID: 2611
		private string[] _archfiendNames;

		// Token: 0x04000A34 RID: 2612
		private string[] _archfiendIds;

		// Token: 0x04000A35 RID: 2613
		private int _archfiendIndex;

		// Token: 0x04000A36 RID: 2614
		private int _maxRelicsValue;

		// Token: 0x04000A37 RID: 2615
		private string _relicSearch = string.Empty;
	}
}

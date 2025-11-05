using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Core.Injection;
using UnityEngine;
using Zenject;

namespace LoG
{
	// Token: 0x02000180 RID: 384
	public class DebugGUIPage_GameGenerator : DebugGUIPage
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x0003FCF8 File Offset: 0x0003DEF8
		// (set) Token: 0x06000D4D RID: 3405 RVA: 0x0003FD00 File Offset: 0x0003DF00
		private int PlayerCount
		{
			get
			{
				return this._playerCount;
			}
			set
			{
				this._playerCount = value;
				while (this._players.Count < this._playerCount)
				{
					this._players.Add(new PlayerGenerationParameters
					{
						Role = ((this._players.Count == 0) ? PlayerRole.Human : PlayerRole.GOAP)
					});
				}
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0003FD50 File Offset: 0x0003DF50
		public DebugGUIPage_GameGenerator()
		{
			this._parameters = new GameGenerationParameters
			{
				FillEmptyRelicsWithDefaults = false
			};
			this.PlayerCount = 2;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0003FD88 File Offset: 0x0003DF88
		public override void DrawContents()
		{
			this.Draw(this._parameters);
			if (GUILayout.Button("Play", Array.Empty<GUILayoutOption>()))
			{
				GameState gameState = this.Generate();
				SI2GameplayStatePayload si2GameplayStatePayload = this._gameplayState.CreateTypedPayload();
				si2GameplayStatePayload.FullSaveGame = new FullSaveGame
				{
					GameState = gameState
				};
				this._appManager.LoadGameState(si2GameplayStatePayload);
			}
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0003FDE4 File Offset: 0x0003DFE4
		public GameState Generate()
		{
			this._parameters.Players = IEnumerableExtensions.ToList<PlayerGenerationParameters>(this._players.Take(this.PlayerCount));
			SimulationRandom random = new SimulationRandom(Guid.NewGuid().GetHashCode());
			HashSet<string> usedPlayers = (from t in this._parameters.Players
			select t.ArchfiendId into id
			where !string.IsNullOrEmpty(id)
			select id).ToHashSet<string>();
			List<ArchFiendStaticData> list = IEnumerableExtensions.ToList<ArchFiendStaticData>(from t in this._database.Enumerate<ArchFiendStaticData>()
			where !usedPlayers.Contains(t.Id)
			select t);
			for (int i = 0; i < this._parameters.Players.Count; i++)
			{
				PlayerGenerationParameters playerGenerationParameters = this._parameters.Players[i];
				ArchFiendStaticData archFiendStaticData;
				if (string.IsNullOrEmpty(playerGenerationParameters.ArchfiendId) && list.TryPopRandom(random, out archFiendStaticData))
				{
					playerGenerationParameters.ArchfiendId = archFiendStaticData.Id;
				}
				this._parameters.Players[i] = playerGenerationParameters;
			}
			ValueTuple<GameState, TurnProcessContext, GameGenerationContext> valueTuple = DiContainerExtensions.CreateAndInjectInstance<GameGenerator>(this._container).CreateGameState(this._parameters, this._database);
			return DiContainerExtensions.CreateAndInjectInstance<TurnProcessor>(this._container).ProcessTurnZero(valueTuple.Item1);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0003FF54 File Offset: 0x0003E154
		public void Draw(GameGenerationParameters parameters)
		{
			this.DebugGUI.StringEdit<int>("Seed", ref parameters.Seed);
			this.DebugGUI.Toggle("FillEmptyRelicsWithDefaults", ref parameters.FillEmptyRelicsWithDefaults, Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Set Preset:", Array.Empty<GUILayoutOption>());
			BoardGenerationParametersPreset boardGenerationParametersPreset = null;
			if (this.DebugGUI.DrawPopup<BoardGenerationParametersPreset>("Parameter Presets", IEnumerableExtensions.ToList<BoardGenerationParametersPreset>(this._database.Enumerate<BoardGenerationParametersPreset>()), ref boardGenerationParametersPreset, null))
			{
				parameters.BoardGeneration = boardGenerationParametersPreset.Parameters;
			}
			GUILayout.EndHorizontal();
			this.Draw(parameters.BoardGeneration);
			if (this.DebugGUI.DrawFoldout("Game Rules", null, false))
			{
				this.DebugGUI.StringEdit<int>("StartingRegentId", ref parameters.StartingRegentId);
				this.DebugGUI.StringEdit<int>("StartingPrestige", ref parameters.StartingPrestige);
				this.DebugGUI.StringEdit<int>("GameDuration", ref parameters.GameDuration);
			}
			if (this.DebugGUI.DrawFoldout("Players", null, false))
			{
				this.Draw(this._players);
			}
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0004006C File Offset: 0x0003E26C
		public void Draw(BoardGenerationParameters parameters)
		{
			if (this.DebugGUI.DrawFoldout("Board", null, false))
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				this.DebugGUI.StringEdit<int>("BoardRows", ref parameters.BoardRows);
				this.DebugGUI.StringEdit<int>("BoardColumns", ref parameters.BoardColumns);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				this.DebugGUI.StringEdit<int>("MinPlacesOfPower", ref parameters.MinPlacesOfPower);
				this.DebugGUI.StringEdit<int>("MaxPlacesOfPower", ref parameters.MaxPlacesOfPower);
				GUILayout.EndHorizontal();
			}
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00040108 File Offset: 0x0003E308
		public void Draw(List<PlayerGenerationParameters> parameters)
		{
			this.PlayerCount = this.DebugGUI.DrawHorizontalSlider("Num Players", this.PlayerCount, 1, 8);
			for (int i = 0; i < this.PlayerCount; i++)
			{
				this.DebugGUI.PushKey(i);
				PlayerGenerationParameters value = parameters[i];
				this.Draw(ref value);
				parameters[i] = value;
				this.DebugGUI.PopKey(i);
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00040180 File Offset: 0x0003E380
		public void Draw(ref PlayerGenerationParameters parameter)
		{
			List<string> options = IEnumerableExtensions.ToList<string>(from t in this._database.Enumerate<ArchFiendStaticData>()
			select t.Id);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Archfiend", Array.Empty<GUILayoutOption>());
			this.DebugGUI.DrawPopup<string>("Archfiend", options, ref parameter.ArchfiendId, null);
			GUILayout.EndHorizontal();
			this.DebugGUI.SelectEnum<PlayerRole>("Role", ref parameter.Role);
			this.DebugGUI.SelectEnum<AIDifficulty>("Difficulty", ref parameter.Difficulty);
			this.DebugGUI.StringEdit<int>("BaseOrderSlots", ref parameter.BaseOrderSlots);
			if (this.DebugGUI.DrawFoldout("Relics", null, false))
			{
				this.DrawRelics(ref parameter);
			}
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00040260 File Offset: 0x0003E460
		public void DrawRelics(ref PlayerGenerationParameters parameter)
		{
			PlayerGenerationParameters playerGenerationParameters = parameter;
			if (playerGenerationParameters.Relics == null)
			{
				playerGenerationParameters.Relics = new List<string>();
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Clear", Array.Empty<GUILayoutOption>()))
			{
				parameter.Relics.Clear();
			}
			ArchFiendStaticData archFiendStaticData;
			ConfigRef<RelicSetStaticData> cref;
			if (this._database.TryFetch<ArchFiendStaticData>(parameter.ArchfiendId, out archFiendStaticData) && GUILayout.Button("Use a Recommended Set", Array.Empty<GUILayoutOption>()) && archFiendStaticData.RecommendedRelics.TrySelectRandom(this.DebugGUI.Random, out cref))
			{
				RelicSetStaticData relicSetStaticData = this._database.Fetch(cref);
				parameter.Relics = IEnumerableExtensions.ToList<string>(from t in relicSetStaticData.Relics
				select t.Id);
			}
			RelicSetStaticData relicSetStaticData2 = null;
			if (this.DebugGUI.SelectStaticData("Use Relic Set", this._database, ref relicSetStaticData2))
			{
				parameter.Relics = IEnumerableExtensions.ToList<string>(from t in relicSetStaticData2.Relics
				select t.Id);
			}
			GUILayout.EndHorizontal();
			int num = 0;
			foreach (string text in IEnumerableExtensions.ToList<string>(parameter.Relics))
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label(text, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(true)
				});
				if (GUILayout.Button("X", new GUILayoutOption[]
				{
					GUILayout.Width(50f)
				}))
				{
					parameter.Relics.Remove(text);
				}
				GUILayout.EndHorizontal();
				RelicStaticData relicStaticData;
				if (this.Database.TryFetch<RelicStaticData>(text, out relicStaticData))
				{
					num += relicStaticData.RelicValue;
				}
			}
			GUILayout.Label(string.Format("Total Cost: {0}", num), Array.Empty<GUILayoutOption>());
			RelicStaticData relicStaticData2 = null;
			if (this.DebugGUI.SelectStaticData("Add Relic", this._database, ref relicStaticData2))
			{
				this.DebugGUI.StartCoroutine(DebugGUIPage_GameGenerator.<DrawRelics>g__AddRelicCoroutine|18_1(parameter.Relics, relicStaticData2.Id));
			}
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00040498 File Offset: 0x0003E698
		[CompilerGenerated]
		internal static IEnumerator <DrawRelics>g__AddRelicCoroutine|18_1(List<string> relics, string id)
		{
			yield return 0;
			relics.Add(id);
			yield break;
		}

		// Token: 0x04000A1C RID: 2588
		private const string key = "DebugGUIPage_GameGenerator";

		// Token: 0x04000A1D RID: 2589
		[Inject]
		private GameDatabase _database;

		// Token: 0x04000A1E RID: 2590
		[Inject]
		private DiContainer _container;

		// Token: 0x04000A1F RID: 2591
		[Inject]
		private SI2GameplayState _gameplayState;

		// Token: 0x04000A20 RID: 2592
		[InjectField]
		private SI2AppManager _appManager;

		// Token: 0x04000A21 RID: 2593
		private GameGenerationParameters _parameters = new GameGenerationParameters();

		// Token: 0x04000A22 RID: 2594
		private List<PlayerGenerationParameters> _players = new List<PlayerGenerationParameters>();

		// Token: 0x04000A23 RID: 2595
		private int _playerCount;
	}
}

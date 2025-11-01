using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020006D5 RID: 1749
	public class TurnProcessContext : TurnContext
	{
		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x0006DEDC File Offset: 0x0006C0DC
		// (set) Token: 0x06001FFA RID: 8186 RVA: 0x0006DEE4 File Offset: 0x0006C0E4
		public TurnProcessContext.ProcessType Process { get; protected set; }

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x0006DEED File Offset: 0x0006C0ED
		// (set) Token: 0x06001FFC RID: 8188 RVA: 0x0006DEF5 File Offset: 0x0006C0F5
		public TurnProcessStage CurrentStage { get; set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001FFD RID: 8189 RVA: 0x0006DEFE File Offset: 0x0006C0FE
		public IReadOnlyList<TurnModuleProcessor> TurnModuleProcessors
		{
			get
			{
				return this.ModuleProcessors;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001FFE RID: 8190 RVA: 0x0006DF08 File Offset: 0x0006C108
		public bool IsPreview
		{
			get
			{
				TurnProcessContext.ProcessType process = this.Process;
				return process == TurnProcessContext.ProcessType.Preview || process == TurnProcessContext.ProcessType.AIPreview;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x0006DF2D File Offset: 0x0006C12D
		public bool IsAIPreview
		{
			get
			{
				return this.Process == TurnProcessContext.ProcessType.AIPreview;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x0006DF38 File Offset: 0x0006C138
		public ProcessContexts ProcessContexts
		{
			get
			{
				return this._processContexts;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x0006DF40 File Offset: 0x0006C140
		public BidContext BiddingContext
		{
			get
			{
				return this._processContexts.BiddingContext;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x0006DF4D File Offset: 0x0006C14D
		public TributeContext TributeContext
		{
			get
			{
				return this._processContexts.TributeContext;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x0006DF5A File Offset: 0x0006C15A
		public DiplomaticContext DiplomaticContext
		{
			get
			{
				return this._processContexts.DiplomaticContext;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x0006DF67 File Offset: 0x0006C167
		public GrandEventsContext EventsContext
		{
			get
			{
				return this._processContexts.EventsContext;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x0006DF74 File Offset: 0x0006C174
		public EventDrawContext EventDrawContext
		{
			get
			{
				return this._processContexts.EventDrawContext;
			}
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x0006DF81 File Offset: 0x0006C181
		public TurnProcessContext()
		{
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x0006DFB5 File Offset: 0x0006C1B5
		public TurnProcessContext(GameState state, GameDatabase database) : this(state.Rules, state.CurrentTurn, database)
		{
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x0006DFCA File Offset: 0x0006C1CA
		public TurnProcessContext(GameRules rules, TurnState turn, GameDatabase database) : base(rules, turn, database)
		{
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x0006E001 File Offset: 0x0006C201
		public void ClearContexts()
		{
			this.CurrentStage = TurnProcessStage.TurnModule_TurnStart;
			this._processContexts = new ProcessContexts();
			this.ModuleProcessors = new List<TurnModuleProcessor>();
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x0006E020 File Offset: 0x0006C220
		public void BeginProcess(TurnProcessContext.ProcessType processType)
		{
			this.ClearContexts();
			this.Process = processType;
			if (!this.IsPreview)
			{
				this.RePopulateModuleProcessors();
				this._currentTurn.AddGameEvent<TurnStartEvent>(new TurnStartEvent());
			}
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x0006E04E File Offset: 0x0006C24E
		public void EndProcess()
		{
			this.ModuleProcessors.Clear();
			if (!this.IsPreview)
			{
				this._currentTurn.AddGameEvent<TurnEndEvent>(new TurnEndEvent());
			}
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x0006E074 File Offset: 0x0006C274
		public void AddModuleProcessor(TurnModuleProcessor processor)
		{
			this.ModuleProcessors.Add(processor);
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x0006E082 File Offset: 0x0006C282
		public void RemoveModuleProcessor(TurnModuleProcessor processor)
		{
			this.ModuleProcessors.Remove(processor);
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x0006E091 File Offset: 0x0006C291
		public void RemoveModuleProcessorOfType<T>() where T : TurnModuleProcessor
		{
			this.ModuleProcessors.RemoveAll((TurnModuleProcessor x) => x is T);
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x0006E0C0 File Offset: 0x0006C2C0
		public void RePopulateModuleProcessors()
		{
			this.ModuleProcessors.Clear();
			foreach (TurnModuleInstance instance in base.CurrentTurn.ActiveModules)
			{
				TurnModuleProcessor turnModuleProcessor = TurnModuleProcessorFactory.CreateProcessor(instance, this);
				if (turnModuleProcessor != null)
				{
					this.ModuleProcessors.Add(turnModuleProcessor);
				}
			}
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x0006E134 File Offset: 0x0006C334
		public void AddActionResult(Guid guid, Result result)
		{
			this.ActionResults[guid] = result;
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x0006E143 File Offset: 0x0006C343
		public bool TryGetActionResult(Guid guid, out Result result)
		{
			return this.ActionResults.TryGetValue(guid, out result);
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x0006E152 File Offset: 0x0006C352
		public void AddDecisionResult(DecisionId id, Result result)
		{
			this.DecisionResults[id] = result;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x0006E161 File Offset: 0x0006C361
		public bool TryGetDecisionResult(DecisionId id, out Result result)
		{
			return this.DecisionResults.TryGetValue(id, out result);
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x0006E170 File Offset: 0x0006C370
		public void SetExceptionHandler(Func<Exception, Result> handler)
		{
			this._exceptionHandler = handler;
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0006E179 File Offset: 0x0006C379
		public Result OnException(Exception e)
		{
			if (this._exceptionHandler == null)
			{
				throw e;
			}
			return this._exceptionHandler(e);
		}

		// Token: 0x04000D35 RID: 3381
		private List<TurnModuleProcessor> ModuleProcessors = new List<TurnModuleProcessor>();

		// Token: 0x04000D36 RID: 3382
		private ProcessContexts _processContexts = new ProcessContexts();

		// Token: 0x04000D37 RID: 3383
		public bool ForcePreservePlayerTurns;

		// Token: 0x04000D38 RID: 3384
		public bool PaperworkRestructurePlayed;

		// Token: 0x04000D39 RID: 3385
		private Dictionary<Guid, Result> ActionResults = new Dictionary<Guid, Result>();

		// Token: 0x04000D3A RID: 3386
		private Dictionary<DecisionId, Result> DecisionResults = new Dictionary<DecisionId, Result>();

		// Token: 0x04000D3B RID: 3387
		private Func<Exception, Result> _exceptionHandler;

		// Token: 0x02000AC3 RID: 2755
		public enum ProcessType
		{
			// Token: 0x04001AE3 RID: 6883
			Process,
			// Token: 0x04001AE4 RID: 6884
			Preview,
			// Token: 0x04001AE5 RID: 6885
			AIPreview
		}
	}
}

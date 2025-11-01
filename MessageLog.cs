using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000309 RID: 777
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MessageLog : IDeepClone<MessageLog>
	{
		// Token: 0x06000F36 RID: 3894 RVA: 0x0003C674 File Offset: 0x0003A874
		public void AddIncomingMessage(Message message)
		{
			List<Message> historyWithPlayer = this.GetHistoryWithPlayer(message.FromPlayerId);
			historyWithPlayer.Add(message);
			historyWithPlayer.SortOnValueAscending((Message x) => x.TurnId);
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0003C6B0 File Offset: 0x0003A8B0
		public List<Message> GetAllReceivedMessages()
		{
			List<Message> list = new List<Message>();
			foreach (KeyValuePair<int, List<Message>> keyValuePair in this._messageHistories)
			{
				int num;
				List<Message> list2;
				keyValuePair.Deconstruct(out num, out list2);
				List<Message> collection = list2;
				list.AddRange(collection);
			}
			return list;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0003C718 File Offset: 0x0003A918
		public List<Message> GetAllReceivedMessagesFromPlayer(int fromPlayerId)
		{
			return IEnumerableExtensions.ToList<Message>(from x in this.GetHistoryWithPlayer(fromPlayerId)
			where x.FromPlayerId == fromPlayerId
			select x);
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0003C754 File Offset: 0x0003A954
		public List<Message> GetIncomingMessagesForTurn(int turnId)
		{
			MessageLog.<>c__DisplayClass4_0 CS$<>8__locals1 = new MessageLog.<>c__DisplayClass4_0();
			CS$<>8__locals1.turnId = turnId;
			List<Message> list = new List<Message>();
			foreach (KeyValuePair<int, List<Message>> keyValuePair in this._messageHistories)
			{
				int num;
				List<Message> list2;
				keyValuePair.Deconstruct(out num, out list2);
				List<Message> source = list2;
				list.AddRange(source.Where(new Func<Message, bool>(CS$<>8__locals1.<GetIncomingMessagesForTurn>g__Predicate|0)));
			}
			return list;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0003C7DC File Offset: 0x0003A9DC
		public List<Message> GetIncomingMessagesForTurn(int turnId, int fromPlayerId)
		{
			MessageLog.<>c__DisplayClass5_0 CS$<>8__locals1 = new MessageLog.<>c__DisplayClass5_0();
			CS$<>8__locals1.turnId = turnId;
			List<Message> list = new List<Message>();
			List<Message> historyWithPlayer = this.GetHistoryWithPlayer(fromPlayerId);
			list.AddRange(historyWithPlayer.Where(new Func<Message, bool>(CS$<>8__locals1.<GetIncomingMessagesForTurn>g__Predicate|0)));
			return list;
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0003C81B File Offset: 0x0003AA1B
		public void AddOutgoingMessage(Message message)
		{
			List<Message> historyWithPlayer = this.GetHistoryWithPlayer(message.ToPlayerId);
			historyWithPlayer.Add(message);
			historyWithPlayer.SortOnValueAscending((Message x) => x.TurnId);
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0003C854 File Offset: 0x0003AA54
		public List<Message> GetAllSentMessages(int playerId)
		{
			MessageLog.<>c__DisplayClass7_0 CS$<>8__locals1 = new MessageLog.<>c__DisplayClass7_0();
			CS$<>8__locals1.playerId = playerId;
			List<Message> list = new List<Message>();
			foreach (KeyValuePair<int, List<Message>> keyValuePair in this._messageHistories)
			{
				int num;
				List<Message> list2;
				keyValuePair.Deconstruct(out num, out list2);
				List<Message> source = list2;
				list.AddRange(source.Where(new Func<Message, bool>(CS$<>8__locals1.<GetAllSentMessages>g__Predicate|0)));
			}
			return list;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0003C8DC File Offset: 0x0003AADC
		public List<Message> GetHistoryWithPlayer(int otherPlayerId)
		{
			if (!this._messageHistories.ContainsKey(otherPlayerId))
			{
				this._messageHistories.Add(otherPlayerId, new List<Message>());
			}
			List<Message> result;
			this._messageHistories.TryGetValue(otherPlayerId, out result);
			return result;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0003C918 File Offset: 0x0003AB18
		public void SetHistoryWithPlayer(int otherPlayerId, List<Message> messages)
		{
			this._messageHistories[otherPlayerId] = messages;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0003C927 File Offset: 0x0003AB27
		public void DeepClone(out MessageLog clone)
		{
			clone = new MessageLog
			{
				_messageHistories = this._messageHistories.DeepClone<Message>()
			};
		}

		// Token: 0x040006FB RID: 1787
		[JsonProperty]
		private Dictionary<int, List<Message>> _messageHistories = new Dictionary<int, List<Message>>();
	}
}

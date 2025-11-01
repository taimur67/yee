using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000305 RID: 773
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class Message : IIdentifiable, IEquatable<IIdentifiable>, IDeepClone<Message>
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0003C01E File Offset: 0x0003A21E
		// (set) Token: 0x06000F14 RID: 3860 RVA: 0x0003C026 File Offset: 0x0003A226
		[JsonIgnore]
		public Identifier Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0003C02F File Offset: 0x0003A22F
		public bool Equals(IIdentifiable other)
		{
			return other != null && (this == other || this.Id == other.Id);
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0003C04C File Offset: 0x0003A24C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			IIdentifiable identifiable = obj as IIdentifiable;
			return identifiable != null && this.Equals(identifiable);
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0003C077 File Offset: 0x0003A277
		public override int GetHashCode()
		{
			return (int)this._id;
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0003C07F File Offset: 0x0003A27F
		public void MarkAsRead()
		{
			this.IsMarkedRead = true;
		}

		// Token: 0x06000F19 RID: 3865
		public abstract string GetContent();

		// Token: 0x06000F1A RID: 3866
		public abstract string SetContent(string content);

		// Token: 0x06000F1B RID: 3867 RVA: 0x0003C088 File Offset: 0x0003A288
		protected void DeepCloneMessageParts(Message message)
		{
			message.Id = this.Id;
			message.TurnId = this.TurnId;
			message.FromPlayerId = this.FromPlayerId;
			message.FromPlayerPlayFabId = this.FromPlayerPlayFabId;
			message.ToPlayerId = this.ToPlayerId;
			message.ToPlayerPlayFabId = this.ToPlayerPlayFabId;
			message.IsMarkedRead = this.IsMarkedRead;
			message.IsMuted = this.IsMuted;
		}

		// Token: 0x06000F1C RID: 3868
		public abstract void DeepClone(out Message message);

		// Token: 0x06000F1D RID: 3869 RVA: 0x0003C0F5 File Offset: 0x0003A2F5
		public Message()
		{
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0003C100 File Offset: 0x0003A300
		public Message(TurnContext context, PlayerState sender, PlayerState receiver)
		{
			this.FromPlayerId = sender.Id;
			this.FromPlayerPlayFabId = sender.PlayFabId;
			this.ToPlayerId = receiver.Id;
			this.ToPlayerPlayFabId = receiver.PlayFabId;
			this.TurnId = context.CurrentTurn.TurnValue;
			this.Id = context.CurrentTurn.GenerateIdentifier();
		}

		// Token: 0x040006E2 RID: 1762
		[JsonProperty]
		private Identifier _id;

		// Token: 0x040006E3 RID: 1763
		[JsonProperty]
		public int TurnId;

		// Token: 0x040006E4 RID: 1764
		[JsonProperty]
		public int FromPlayerId;

		// Token: 0x040006E5 RID: 1765
		[JsonProperty]
		public string FromPlayerPlayFabId;

		// Token: 0x040006E6 RID: 1766
		[JsonProperty]
		public int ToPlayerId;

		// Token: 0x040006E7 RID: 1767
		[JsonProperty]
		public string ToPlayerPlayFabId;

		// Token: 0x040006E8 RID: 1768
		[JsonProperty]
		public bool IsMarkedRead;

		// Token: 0x040006E9 RID: 1769
		[JsonProperty]
		public bool IsMuted;
	}
}

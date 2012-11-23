using GameCore.Acts;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using RusLanguage;

namespace GameCore.Essences.Tools
{
	public interface ITool
	{
		EActResults UseTool(Intelligent _intelligent);
	}

	internal class Torch : Item, ITool
	{
		private LightSource m_lightSource;

		public Torch(Material _material) : base(_material) { }

        public override int TileIndex
        {
            get
            {
                return 2;
            }
        }

		public override string Name { get { return "факел"; } }

		public override EItemCategory Category { get { return EItemCategory.TOOLS; } }

		public override FColor LerpColor { get { return IsOn ? new FColor(1f, m_lightSource.Color) : base.LerpColor; } }

		public bool IsOn { get; private set; }

		public override ILightSource Light { get { return IsOn ? m_lightSource : null; } }

		#region ITool Members

		public EActResults UseTool(Intelligent _intelligent)
		{
			if (IsOn)
			{
				if (_intelligent.IsAvatar)
				{
					MessageManager.SendMessage(_intelligent, new SimpleTextMessage(EMessageType.INFO, this[EPadej.IMEN] + " потушен"));
				}
				else
				{
					MessageManager.SendMessage(_intelligent, new SimpleTextMessage(EMessageType.INFO, _intelligent[EPadej.IMEN] + " потушил " + this[EPadej.VIN]));
				}
			}
			else
			{
				if (_intelligent.IsAvatar)
				{
					MessageManager.SendMessage(_intelligent, new SimpleTextMessage(EMessageType.INFO, this[EPadej.IMEN] + " зажжен"));
				}
				else
				{
					MessageManager.SendMessage(_intelligent, new SimpleTextMessage(EMessageType.INFO, _intelligent[EPadej.IMEN] + " зажег " + this[EPadej.VIN]));
				}
			}
			IsOn = !IsOn;
			return EActResults.DONE;
		}

		#endregion

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			return ItemBattleInfo.Empty;
		}

		public void LightCells(LiveMap _liveMap, Point _point) { m_lightSource.LightCells(_liveMap, _point); }
        public override void Resolve(Creature _creature) { m_lightSource = new LightSource(10, new FColor(2f, 1f, 0.9f, 0.5f)); }
    }
}
using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using RusLanguage;

namespace GameCore.Objects.Tools
{
	public interface ITool
	{
		EActResults UseTool(Intelligent _intelligent);
	}

	class Torch : Item, ITool
	{
		private LightSource m_lightSource;

		public Torch(Material _material) : base(_material)
		{
		}

		public void LightCells(LiveMap _liveMap, Point _point)
		{
			m_lightSource.LightCells(_liveMap, _point);
		}

		public override ETiles Tile
		{
			get { return ETiles.TORCH; }
		}

		public override string Name
		{
			get { return "факел"; }
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.TOOLS; }
		}

		public override void Resolve(Creature _creature)
		{
			m_lightSource = new LightSource(10, new FColor(4f, 1f, 0.9f, 0.5f));
		}

		public override FColor LerpColor
		{
			get
			{
				return IsOn ? new FColor(1f, m_lightSource.Color) : base.LerpColor;
			}
		}

		public bool IsOn { get; private set; }

		public override ILightSource Light
		{
			get
			{
				return IsOn?m_lightSource : null;
			}
		}

		public EActResults UseTool(Intelligent _intelligent)
		{
			if(IsOn)
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
	}
}

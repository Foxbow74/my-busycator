using GameCore.Acts;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;

namespace GameCore.Objects.Tools
{
	public interface ITool
	{
		EActResults UseTool(Intelligent _intelligent);
	}

	class Torch : Item, ILightSource, ITool
	{
		private LightSource m_lightSource;

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
			get { return "афакел"; }
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.TOOLS; }
		}

		public override void Resolve(Creature _creature)
		{
			m_lightSource = new LightSource(4, new FColor(1f, 1f, 0.9f, 0.5f));
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
					MessageManager.SendMessage(_intelligent, new SimpleTextMessage(EMessageType.INFO, "факел потушен"));
				}
				else
				{
					MessageManager.SendMessage(_intelligent, new SimpleTextMessage(EMessageType.INFO, _intelligent.Name + " потушил факел"));
				}
			}
			else
			{
				if (_intelligent.IsAvatar)
				{
					MessageManager.SendMessage(_intelligent, new SimpleTextMessage(EMessageType.INFO, "факел зажжен"));
				}
				else
				{
					MessageManager.SendMessage(_intelligent, new SimpleTextMessage(EMessageType.INFO, _intelligent.Name + " зажег факел"));
				}
			}
			IsOn = !IsOn;
			return EActResults.DONE;
		}
	}
}

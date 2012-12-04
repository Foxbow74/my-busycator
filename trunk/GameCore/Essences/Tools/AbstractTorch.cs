using GameCore.AbstractLanguage;
using GameCore.Acts;
using GameCore.Battle;
using GameCore.Creatures;
using GameCore.Mapping;
using GameCore.Messages;
using GameCore.Misc;
using GameCore.XLanguage;

namespace GameCore.Essences.Tools
{
	public abstract class AbstractTorch : Item, ITool
	{
		private readonly int m_radius;
		private readonly FColor m_color;
		private LightSource m_lightSource;

		protected AbstractTorch(Noun _name, Material _material, int _radius, FColor _color)
			: base(_name, _material)
		{
			m_radius = _radius;
			m_color = _color;
		}

		public override int TileIndex
        {
            get
            {
                return 2;
            }
        }

		public override EItemCategory Category { get { return EItemCategory.TOOLS; } }

		public override FColor LerpColor { get { return IsOn ? new FColor(1f, m_lightSource.Color) : base.LerpColor; } }

		public bool IsOn { get; private set; }

		public override ILightSource Light { get { return IsOn ? m_lightSource : null; } }

		#region ITool Members

		public EActResults UseTool(Intelligent _intelligent)
		{
			if (IsOn)
			{
				MessageManager.SendXMessage(_intelligent, new XMessage(EXMType.CREATURE_LIGHT_OFF_IT, _intelligent, this));
			}
			else
			{
				MessageManager.SendXMessage(_intelligent, new XMessage(EXMType.CREATURE_LIGHT_ON_IT, _intelligent, this));
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

		internal override Essence Clone(Creature _resolver)
		{
			var clone = (AbstractTorch)base.Clone(_resolver);
			clone.m_lightSource = new LightSource(m_radius, m_color);
			return clone;
		}
    }
}
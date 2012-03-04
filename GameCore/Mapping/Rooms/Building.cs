using GameCore.Mapping.Layers;
using GameCore.Misc;

namespace GameCore.Mapping.Rooms
{
	class Building:Room
	{
		public Building(Rct _roomRect, Rct _areaRect, Point _blockId, WorldLayer _layer)
			: base(_roomRect, _areaRect, _blockId, _layer)
		{
		}
	}
}

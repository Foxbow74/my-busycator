using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics;

namespace GameCore.Objects
{
	public class Door:Object
	{
		public override Tile Tile
		{
			get { return Tiles.DoorTile; }
		}

		public override string Name
		{
			get { return "дверь"; }
		}
	}
}

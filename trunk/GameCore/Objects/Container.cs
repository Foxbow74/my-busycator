using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics;

namespace GameCore.Objects
{
	public abstract class Container:Object
	{
	}

	public class Chest:Container
	{
		public override Tile Tile
		{
			get { return Tiles.ChestTile; }
		}

		public override string Name
		{
			get { return "сундук"; }
		}
	}
}

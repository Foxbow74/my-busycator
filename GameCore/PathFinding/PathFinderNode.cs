using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameCore.Misc;

namespace GameCore.PathFinding
{
	public struct PathFinderNode
	{
		#region Variables Declaration

		public int F;
		public int G;
		public int H; // f = gone + heuristic
		public int PX; // Parent
		public int PY;
		public int X;
		public int Y;

		#endregion
	}

	#region Structs

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct PathFinderNodeFast
	{
		#region Variables Declaration

		public int F; // f = gone + heuristic
		public int G;
		public ushort PX; // Parent
		public ushort PY;
		public byte Status;

		#endregion
	}

	#endregion

	/// <summary>
	/// http://www.codeguru.com/csharp/csharp/cs_misc/designtechniques/article.php/c12527__1/A-Star-A-Implementation-in-C-Path-Finding-PathFinder.htm
	/// </summary>
	public class Aa
	{
		#region HeuristicFormula enum

		public enum HeuristicFormula
		{
			MANHATTAN = 1,
			MAX_DXDY = 2,
			DIAGONAL_SHORT_CUT = 3,
			EUCLIDEAN = 4,
			EUCLIDEAN_NO_SQR = 5,
			CUSTOM1 = 6
		}

		#endregion

		#region Variables Declaration

		// Heap variables are initializated to default, but I like to do it anyway
		private const int MhEstimate = 2;
		private const bool MTieBreaker = false;
		private const int MSearchLimit = 2000;
		private const ushort MGridX = 0;
		private const ushort MGridY = 0;
		private const ushort MGridXMinus1 = 0;
		private const ushort MGridYLog2 = 0;
		private readonly PathFinderNodeFast[] m_calcGrid;
		private readonly List<PathFinderNode> m_close = new List<PathFinderNode>();

		private readonly sbyte[,] m_direction = new sbyte[8,2]
		                                        	{{0, -1}, {1, 0}, {0, 1}, {-1, 0}, {1, -1}, {1, 1}, {-1, 1}, {-1, -1}};

		private readonly byte[,] m_grid;
		private readonly PriorityQueueB<int> m_open;
		private int m_closeNodeCounter;

		private byte m_closeNodeValue = 2;
		private int m_endLocation;
		private HeuristicFormula m_formula = HeuristicFormula.MANHATTAN;
		private bool m_found;

		//Promoted local variables to member variables to avoid recreation between calls
		private int m_h;
		private int m_horiz;
		private int m_location;
		private ushort m_locationX;
		private ushort m_locationY;
		private int m_newG;
		private int m_newLocation;
		private ushort m_newLocationX;
		private ushort m_newLocationY;
		private byte m_openNodeValue = 1;
		private bool m_stop;

		#endregion

		public List<PathFinderNode> FindPath(Point _start, Point _end, HeuristicFormula _formula)
		{
			lock (this)
			{
				// Is faster if we don't clear the matrix, just assign different values for open and close and ignore the rest
				// I could have user Array.Clear() but using unsafe code is faster, no much but it is.
				//fixed (PathFinderNodeFast* pGrid = tmpGrid) 
				//    ZeroMemory((byte*) pGrid, sizeof(PathFinderNodeFast) * 1000000);

				m_found = false;
				m_stop = false;
				m_closeNodeCounter = 0;
				m_openNodeValue += 2;
				m_closeNodeValue += 2;
				m_open.Clear();
				m_close.Clear();

#if DEBUGON
                if (mDebugProgress && PathFinderDebug != null)
                    PathFinderDebug(0, 0, start.X, start.Y, PathFinderNodeType.Start, -1, -1);
                if (mDebugProgress && PathFinderDebug != null)
                    PathFinderDebug(0, 0, end.X, end.Y, PathFinderNodeType.End, -1, -1);
#endif

				m_location = (_start.Y << MGridYLog2) + _start.X;
				m_endLocation = (_end.Y << MGridYLog2) + _end.X;
				m_calcGrid[m_location].G = 0;
				m_calcGrid[m_location].F = MhEstimate;
				m_calcGrid[m_location].PX = (ushort) _start.X;
				m_calcGrid[m_location].PY = (ushort) _start.Y;
				m_calcGrid[m_location].Status = m_openNodeValue;

				m_open.Push(m_location);
				while (m_open.Count > 0 && !m_stop)
				{
					m_location = m_open.Pop();

					//Is it in closed list? means this node was already processed
					if (m_calcGrid[m_location].Status == m_closeNodeValue)
						continue;

					m_locationX = (ushort) (m_location & MGridXMinus1);
					m_locationY = (ushort) (m_location >> MGridYLog2);

					if (m_location == m_endLocation)
					{
						m_calcGrid[m_location].Status = m_closeNodeValue;
						m_found = true;
						break;
					}

					if (m_closeNodeCounter > MSearchLimit)
					{
						return null;
					}

					m_horiz = (m_locationX - m_calcGrid[m_location].PX);

					//Lets calculate each successors
					for (int i = 0; i < 8; i++)
					{
						m_newLocationX = (ushort) (m_locationX + m_direction[i, 0]);
						m_newLocationY = (ushort) (m_locationY + m_direction[i, 1]);
						m_newLocation = (m_newLocationY << MGridYLog2) + m_newLocationX;

						if (m_newLocationX >= MGridX || m_newLocationY >= MGridY)
							continue;

						// Unbreakeable?
						if (m_grid[m_newLocationX, m_newLocationY] == 0)
							continue;

						if (i > 3)
							m_newG = m_calcGrid[m_location].G + (int) (m_grid[m_newLocationX, m_newLocationY]*2.41);
						else
							m_newG = m_calcGrid[m_location].G + m_grid[m_newLocationX, m_newLocationY];

						if ((m_newLocationX - m_locationX) != 0)
						{
							if (m_horiz == 0)
								m_newG += Math.Abs(m_newLocationX - _end.X) + Math.Abs(m_newLocationY - _end.Y);
						}
						if ((m_newLocationY - m_locationY) != 0)
						{
							if (m_horiz != 0)
								m_newG += Math.Abs(m_newLocationX - _end.X) + Math.Abs(m_newLocationY - _end.Y);
						}

						//Is it open or closed?
						if (m_calcGrid[m_newLocation].Status == m_openNodeValue || m_calcGrid[m_newLocation].Status == m_closeNodeValue)
						{
							// The current node has less code than the previous? then skip this node
							if (m_calcGrid[m_newLocation].G <= m_newG)
								continue;
						}

						m_calcGrid[m_newLocation].PX = m_locationX;
						m_calcGrid[m_newLocation].PY = m_locationY;
						m_calcGrid[m_newLocation].G = m_newG;

						switch (_formula)
						{
							case HeuristicFormula.MANHATTAN:
								m_h = MhEstimate*(Math.Abs(m_newLocationX - _end.X) + Math.Abs(m_newLocationY - _end.Y));
								break;
							case HeuristicFormula.MAX_DXDY:
								m_h = MhEstimate*(Math.Max(Math.Abs(m_newLocationX - _end.X), Math.Abs(m_newLocationY - _end.Y)));
								break;
							case HeuristicFormula.DIAGONAL_SHORT_CUT:
								var hDiagonal = Math.Min(Math.Abs(m_newLocationX - _end.X), Math.Abs(m_newLocationY - _end.Y));
								var hStraight = (Math.Abs(m_newLocationX - _end.X) + Math.Abs(m_newLocationY - _end.Y));
								m_h = (MhEstimate*2)*hDiagonal + MhEstimate*(hStraight - 2*hDiagonal);
								break;
							case HeuristicFormula.EUCLIDEAN:
								m_h = (int) (MhEstimate*Math.Sqrt(Math.Pow((m_newLocationY - _end.X), 2) + Math.Pow((m_newLocationY - _end.Y), 2)));
								break;
							case HeuristicFormula.EUCLIDEAN_NO_SQR:
								m_h = (int) (MhEstimate*(Math.Pow((m_newLocationX - _end.X), 2) + Math.Pow((m_newLocationY - _end.Y), 2)));
								break;
							case HeuristicFormula.CUSTOM1:
								var dxy = new Point(Math.Abs(_end.X - m_newLocationX), Math.Abs(_end.Y - m_newLocationY));
								int Orthogonal = Math.Abs(dxy.X - dxy.Y);
								int Diagonal = Math.Abs(((dxy.X + dxy.Y) - Orthogonal)/2);
								m_h = MhEstimate*(Diagonal + Orthogonal + dxy.X + dxy.Y);
								break;
						}
						if (MTieBreaker)
						{
							int dx1 = m_locationX - _end.X;
							int dy1 = m_locationY - _end.Y;
							int dx2 = _start.X - _end.X;
							int dy2 = _start.Y - _end.Y;
							int cross = Math.Abs(dx1*dy2 - dx2*dy1);
							m_h = (int) (m_h + cross*0.001);
						}
						m_calcGrid[m_newLocation].F = m_newG + m_h;
						m_open.Push(m_newLocation);
						m_calcGrid[m_newLocation].Status = m_openNodeValue;
					}

					m_closeNodeCounter++;
					m_calcGrid[m_location].Status = m_closeNodeValue;
				}

				if (m_found)
				{
					m_close.Clear();

					var fNodeTmp = m_calcGrid[(_end.Y << MGridYLog2) + _end.X];
					PathFinderNode fNode;
					fNode.F = fNodeTmp.F;
					fNode.G = fNodeTmp.G;
					fNode.H = 0;
					fNode.PX = fNodeTmp.PX;
					fNode.PY = fNodeTmp.PY;
					fNode.X = _end.X;
					fNode.Y = _end.Y;

					while (fNode.X != fNode.PX || fNode.Y != fNode.PY)
					{
						m_close.Add(fNode);
						var posX = fNode.PX;
						var posY = fNode.PY;
						fNodeTmp = m_calcGrid[(posY << MGridYLog2) + posX];
						fNode.F = fNodeTmp.F;
						fNode.G = fNodeTmp.G;
						fNode.H = 0;
						fNode.PX = fNodeTmp.PX;
						fNode.PY = fNodeTmp.PY;
						fNode.X = posX;
						fNode.Y = posY;
					}

					m_close.Add(fNode);

					return m_close;
				}
				return null;
			}
		}

		#region Inner Classes

		internal class ComparePfNode : IComparer<PathFinderNode>
		{
			#region IComparer<PathFinderNode> Members

			public int Compare(PathFinderNode _x, PathFinderNode _y)
			{
				if (_x.F > _y.F)
					return 1;
				if (_x.F < _y.F)
					return -1;
				return 0;
			}

			#endregion
		}

		#endregion
	}
}
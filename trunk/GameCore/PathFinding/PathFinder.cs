using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameCore.Creatures;
using GameCore.Misc;
using UnsafeUtils;

namespace GameCore.PathFinding
{
	/// <summary>
	/// http://www.codeguru.com/csharp/csharp/cs_misc/designtechniques/article.php/c12527__1/A-Star-A-Implementation-in-C-Path-Finding-PathFinder.htm
	/// </summary>
	public class PathFinder
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
		private const int MH_ESTIMATE = 2;
		private const int M_SEARCH_LIMIT = 2000;
		private readonly ushort m_gridX;
		private readonly ushort m_gridY;
		private readonly ushort m_gridXMinus1;
		private readonly ushort m_gridYLog2;
		private PathFinderNodeFast[] m_calcGrid;

		private readonly sbyte[,] m_direction = new sbyte[8,2]
		                                        	{{0, -1}, {1, 0}, {0, 1}, {-1, 0}, {1, -1}, {1, 1}, {-1, 1}, {-1, -1}};

		private readonly PriorityQueueB<int> m_open;
		private int m_closeNodeCounter;

		private byte m_closeNodeValue = 2;
		private int m_endLocation;
		private HeuristicFormula m_formula = HeuristicFormula.EUCLIDEAN_NO_SQR;
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

		private ComparePfNodeMatrix m_comparePfNodeMatrix;
		#endregion

		private int m_sizeInCells;

		public PathFinder(int _sizeInCells)
		{
			m_gridY = 0;
			m_sizeInCells = _sizeInCells;
			m_calcGrid = new PathFinderNodeFast[_sizeInCells * _sizeInCells];
			m_gridX = 256;// (ushort)_sizeInCells;
			m_gridY = 256;// (ushort)_sizeInCells;

			m_gridXMinus1 = (ushort)(m_gridX - 1);
			m_gridYLog2 = (ushort)Math.Log(m_gridY, 2);

			m_comparePfNodeMatrix = new ComparePfNodeMatrix(m_calcGrid);
			m_open = new PriorityQueueB<int>(m_comparePfNodeMatrix);
		}

		public void Clear()
		{
			UnsafeUtils.UnsafeUtils.ClearPathFinderNodeFast(m_calcGrid);
			m_comparePfNodeMatrix.Clear();
		}

		public List<Point> FindPath(Creature _creature, Point _end)
		{
			//return null;
			using (new Profiler())
			{
				lock (this)
				{
					var start = _creature[0, 0].PathMapCoords;

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
					
					m_location = (start.Y << m_gridYLog2) + start.X;
					m_endLocation = (_end.Y << m_gridYLog2) + _end.X;

					m_calcGrid[m_location].G = 0;
					m_calcGrid[m_location].F = MH_ESTIMATE;
					m_calcGrid[m_location].PX = (ushort) start.X;
					m_calcGrid[m_location].PY = (ushort) start.Y;
					m_calcGrid[m_location].Status = m_openNodeValue;

					m_open.Push(m_location);
					while (m_open.Count > 0 && !m_stop)
					{
						m_location = m_open.Pop();

						//Is it in closed list? means this node was already processed
						if (m_calcGrid[m_location].Status == m_closeNodeValue)
							continue;

						m_locationX = (ushort) (m_location & m_gridXMinus1);
						m_locationY = (ushort) (m_location >> m_gridYLog2);

						if (m_location == m_endLocation)
						{
							m_calcGrid[m_location].Status = m_closeNodeValue;
							m_found = true;
							break;
						}

						if (m_closeNodeCounter > M_SEARCH_LIMIT)
						{
							return null;
						}

						m_horiz = (m_locationX - m_calcGrid[m_location].PX);

						//Lets calculate each successors
						for (var i = 0; i < 8; i++)
						{
							m_newLocationX = (ushort) (m_locationX + m_direction[i, 0]);
							m_newLocationY = (ushort) (m_locationY + m_direction[i, 1]);
							m_newLocation = (m_newLocationY << m_gridYLog2) + m_newLocationX;
							//Debug.WriteLine(m_newLocation);
							if (m_newLocationX >= m_gridX || m_newLocationY >= m_gridY)
								continue;

							var b = World.TheWorld.LiveMap.GetPfIsPassable(new Point(m_newLocationX, m_newLocationY), _creature);

							// Unbreakeable?
							if (b == 0)
								continue;

							if (i > 3)
								m_newG = m_calcGrid[m_location].G + (int) (b*2.41);
							else
								m_newG = m_calcGrid[m_location].G + b;

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

							if (m_newLocation >= m_calcGrid.Length) return null;

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

							switch (m_formula)
							{
								case HeuristicFormula.MANHATTAN:
									m_h = MH_ESTIMATE*(Math.Abs(m_newLocationX - _end.X) + Math.Abs(m_newLocationY - _end.Y));
									break;
								case HeuristicFormula.MAX_DXDY:
									m_h = MH_ESTIMATE*(Math.Max(Math.Abs(m_newLocationX - _end.X), Math.Abs(m_newLocationY - _end.Y)));
									break;
								case HeuristicFormula.DIAGONAL_SHORT_CUT:
									var hDiagonal = Math.Min(Math.Abs(m_newLocationX - _end.X), Math.Abs(m_newLocationY - _end.Y));
									var hStraight = (Math.Abs(m_newLocationX - _end.X) + Math.Abs(m_newLocationY - _end.Y));
									m_h = (MH_ESTIMATE*2)*hDiagonal + MH_ESTIMATE*(hStraight - 2*hDiagonal);
									break;
								case HeuristicFormula.EUCLIDEAN:
									m_h = (int) (MH_ESTIMATE*Math.Sqrt(Math.Pow((m_newLocationY - _end.X), 2) + Math.Pow((m_newLocationY - _end.Y), 2)));
									break;
								case HeuristicFormula.EUCLIDEAN_NO_SQR:
									m_h = (int) (MH_ESTIMATE*(Math.Pow((m_newLocationX - _end.X), 2) + Math.Pow((m_newLocationY - _end.Y), 2)));
									break;
								case HeuristicFormula.CUSTOM1:
									var dxy = new Point(Math.Abs(_end.X - m_newLocationX), Math.Abs(_end.Y - m_newLocationY));
									var orthogonal = Math.Abs(dxy.X - dxy.Y);
									var diagonal = Math.Abs(((dxy.X + dxy.Y) - orthogonal)/2);
									m_h = MH_ESTIMATE*(diagonal + orthogonal + dxy.X + dxy.Y);
									break;
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
						var result = new List<Point>();
						var fNodeTmp = m_calcGrid[(_end.Y << m_gridYLog2) + _end.X];
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
							result.Add(new Point(fNode.X, fNode.Y));
							//m_close.Add(fNode);
							var posX = fNode.PX;
							var posY = fNode.PY;
							fNodeTmp = m_calcGrid[(posY << m_gridYLog2) + posX];
							fNode.F = fNodeTmp.F;
							fNode.G = fNodeTmp.G;
							fNode.H = 0;
							fNode.PX = fNodeTmp.PX;
							fNode.PY = fNodeTmp.PY;
							fNode.X = posX;
							fNode.Y = posY;
						}

						result.Add(fNode.Point);
						result.Reverse();

						return result;
					}
					return null;
				}
			}
		}

		#region Inner Classes

		internal class ComparePfNodeMatrix : IComparer<int>
		{
			readonly PathFinderNodeFast[] m_matrix;

			#region Constructors
			public ComparePfNodeMatrix(PathFinderNodeFast[] _matrix)
			{
				m_matrix = _matrix;
			}
			#endregion

			#region IComparer Members
			public int Compare(int _a, int _b)
			{
				if (m_matrix[_a].F > m_matrix[_b].F)
					return 1;
				if (m_matrix[_a].F < m_matrix[_b].F)
					return -1;
				return 0;
			}
			#endregion

			public void Clear()
			{
				foreach (var pathFinderNodeFast in m_matrix)
				{
					if(pathFinderNodeFast.PX!=0)
					{
						
					}
				}
			}
		}

		#endregion
	}
}
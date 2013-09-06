using System.Drawing;
using System.Linq;
using GameCore;
using GameCore.Mapping;
using GameCore.Misc;
using Point = GameCore.Misc.Point;

namespace Shader
{
    struct ShadowCaster
    {
        public float Opacity;
        public LiveMapCell LiveMapCell;
    }

    public class LosManagerEx
    {
        private const int MAP_SIZE = Constants.MAP_BLOCK_SIZE * 3;
        private static readonly Point[] AllBlockPoints;
        private static readonly Edge[] m_allEdges = new Edge[MAP_SIZE * MAP_SIZE * 4];

        static LosManagerEx()
        {
            AllBlockPoints = Point.Zero.GetAllBlockPoints().ToArray();
        }

        private readonly ShadowCaster[,] m_shadowCasters = new ShadowCaster[MAP_SIZE, MAP_SIZE];
        private int m_edgesCount;
        

        public void Recalc(LiveMap _liveMap)
        {
            using (new Profiler("LosManagerEx.Recalc"))
            {
                #region подготовка карты

                var liveBlocks = _liveMap.GetLightedLiveBlocks();
                var point = new Point(1, 1);

                for (var i = 0; i < 9; i++)
                {
                    var dlt = (liveBlocks[i, 0] + point)*Constants.MAP_BLOCK_SIZE;
                    var livBlockXY = liveBlocks[i, 1];
                    for (var index = 0; index < AllBlockPoints.Length; index++)
                    {
                        var blockPoint = AllBlockPoints[index];
                        var liveCellXY = livBlockXY + blockPoint;
                        var liveMapCell = _liveMap.Cells[liveCellXY.X, liveCellXY.Y];

                        var opacity = liveMapCell.CalcOpacity();

                        if (opacity < float.Epsilon)
                        {
                            m_shadowCasters[dlt.X + blockPoint.X, dlt.Y + blockPoint.Y].Opacity = 0;
                        }
                        else
                        {
                            m_shadowCasters[dlt.X + blockPoint.X, dlt.Y + blockPoint.Y].Opacity = opacity;
                            m_shadowCasters[dlt.X + blockPoint.X, dlt.Y + blockPoint.Y].LiveMapCell = liveMapCell;
                        }

//                            liveMapCell.l
                    }

                    foreach (var lightSourcesInfo in _liveMap.Blocks[livBlockXY.X, livBlockXY.Y].MapBlock.LightSources)
                    {
                            
                    }

                    foreach (var lightSourcesInfo in World.TheWorld.CreatureManager.LightSources())
                    {
                        
                    }

                    if (World.TheWorld.Avatar.Light != null)
                    {
                        //World.TheWorld.Avatar.Light.LightCells(this, centerLiveCell);
                    }
                }

                #endregion

                m_edgesCount = 0;

                for (var i = 0; i < MAP_SIZE; ++i)
                {
                    for (var j = 0; j < MAP_SIZE; ++j)
                    {
                        if (m_shadowCasters[i, j].LiveMapCell != null)
                        {
                            var opacity = m_shadowCasters[i, j].Opacity;
                            var rect = new RectangleF(i, j, 1, 1);
                            if (j > 0 && m_shadowCasters[i, j - 1].LiveMapCell == null)
                            {
                                m_allEdges[m_edgesCount++] = new Edge(new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Top)) { Opacity = opacity };
                            }
                            if (i < (MAP_SIZE - 1) && m_shadowCasters[i + 1, j].LiveMapCell == null)
                            {
                                m_allEdges[m_edgesCount++] = new Edge(new PointF(rect.Right, rect.Top), new PointF(rect.Right, rect.Bottom)) { Opacity = opacity };
                            }
                            if (j < (MAP_SIZE - 1) && m_shadowCasters[i, j + 1].LiveMapCell == null)
                            {
                                m_allEdges[m_edgesCount++] = new Edge(new PointF(rect.Right, rect.Bottom), new PointF(rect.Left, rect.Bottom)) {Opacity = opacity};
                            }
                            if (i > 0 && m_shadowCasters[i - 1, j].LiveMapCell == null)
                            {
                                m_allEdges[m_edgesCount++] = new Edge(new PointF(rect.Left, rect.Bottom), new PointF(rect.Left, rect.Top)) { Opacity = opacity };
                            }
                        }
                    }
                }

                var dPoint = _liveMap.GetDPoint();
                var viewportSize = _liveMap.VieportSize;

            }
        }
    }
}

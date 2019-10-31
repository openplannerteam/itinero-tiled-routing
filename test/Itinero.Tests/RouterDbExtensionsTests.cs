using System.Linq;
using Itinero.LocalGeo;
using Xunit;

namespace Itinero.Tests
{
    public class RouterDbExtensionsTests
    {
        [Fact]
        public void RouterDbExtensions_Snap_OnVertex_ShouldSnapToVertex()
        {
            var routerDb = new RouterDb();
            var vertex1 = routerDb.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = routerDb.AddVertex(4.797506332397461, 51.26674845584085);

            var edgeId = routerDb.AddEdge(vertex1, vertex2);

            var snapResult = routerDb.Snap(4.792613983154297, 51.26535213392538);
            Assert.NotNull(snapResult);
            var snap = (SnapPoint) snapResult;
            Assert.Equal(0, snap.Offset);
            Assert.Equal(edgeId, snap.EdgeId);
        }

        [Fact]
        public void RouterDbExtensions_RouterDbEdgeEnumerator_GetShapeBetween_FullRange_NoShape_ShouldReturnVertices()
        {
            var routerDb = new RouterDb();
            var vertex1 = routerDb.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = routerDb.AddVertex(4.797506332397461, 51.26674845584085);

            var edgeId = routerDb.AddEdge(vertex1, vertex2);

            var edgeEnumerator = routerDb.GetEdgeEnumerator();
            edgeEnumerator.MoveToEdge(edgeId);

            var shape = edgeEnumerator.GetShapeBetween().ToList();
            Assert.True(Coordinate.DistanceEstimateInMeter(routerDb.GetVertex(vertex1), shape[0]) < 2);
            Assert.True(Coordinate.DistanceEstimateInMeter(routerDb.GetVertex(vertex2), shape[1]) < 2);
        }

        [Fact]
        public void RouterDbExtensions_RouterDbEdgeEnumerator_GetShapeBetween_FirstHalfRange_NoShape_ShouldReturnFirstHalf()
        {
            var routerDb = new RouterDb();
            var vertex1 = routerDb.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = routerDb.AddVertex(4.797506332397461, 51.26674845584085);

            var edgeId = routerDb.AddEdge(vertex1, vertex2);

            var edgeEnumerator = routerDb.GetEdgeEnumerator();
            edgeEnumerator.MoveToEdge(edgeId);

            var shape = edgeEnumerator.GetShapeBetween(0, ushort.MaxValue / 2).ToList();
            Assert.True(Coordinate.DistanceEstimateInMeter(routerDb.GetVertex(vertex1), shape[0]) < 2);
            Assert.True(Coordinate.DistanceEstimateInMeter(new Line(routerDb.GetVertex(vertex1), routerDb.GetVertex(vertex2)).Middle, shape[1]) < 2);
        }

        [Fact]
        public void RouterDbExtensions_RouterDbEdgeEnumerator_GetShapeBetween_SecondHalfRange_NoShape_ShouldReturnSecondHalf()
        {
            var routerDb = new RouterDb();
            var vertex1 = routerDb.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = routerDb.AddVertex(4.797506332397461, 51.26674845584085);

            var edgeId = routerDb.AddEdge(vertex1, vertex2);

            var edgeEnumerator = routerDb.GetEdgeEnumerator();
            edgeEnumerator.MoveToEdge(edgeId);

            var shape = edgeEnumerator.GetShapeBetween(ushort.MaxValue / 2, ushort.MaxValue).ToList();
            Assert.True(Coordinate.DistanceEstimateInMeter(new Line(routerDb.GetVertex(vertex1), routerDb.GetVertex(vertex2)).Middle, shape[0]) < 2);
            Assert.True(Coordinate.DistanceEstimateInMeter(routerDb.GetVertex(vertex2), shape[1]) < 2);
        }
    }
}
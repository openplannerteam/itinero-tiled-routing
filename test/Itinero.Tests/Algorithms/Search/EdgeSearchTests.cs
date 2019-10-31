using Itinero.Algorithms.Search;
using Itinero.Data;
using Itinero.Data.Graphs;
using Xunit;

namespace Itinero.Tests.Algorithms.Search
{
    public class EdgeSearchTests
    {
        [Fact]
        public void EdgeSearch_SearchEdgesInBox_ShouldReturnNothingWhenNoEdges()
        {
            var network = new RouterDb();
            network.AddVertex(4.792613983154297, 51.26535213392538);
            network.AddVertex(4.797506332397461, 51.26674845584085);

            var edges = network.SearchEdgesInBox((4.796, 51.265, 4.798, 51.267));
            Assert.NotNull(edges);
            Assert.False(edges.MoveNext());
        }
        
        [Fact]
        public void EdgeSearch_SearchEdgesInBox_ShouldReturnEdgeWhenOneVertexInBox()
        {
            var network = new RouterDb();
            var vertex1 = network.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = network.AddVertex(4.797506332397461, 51.26674845584085);
            var edge = network.AddEdge(vertex1, vertex2);
            
            var edges = network.SearchEdgesInBox((4.796, 51.265, 4.798, 51.267));
            Assert.NotNull(edges);
            Assert.True(edges.MoveNext());
            Assert.Equal(edge, edges.Id);
            Assert.False(edges.MoveNext());
        }

        [Fact]
        public void EdgeSearch_SnapInBox_ShouldSnapToVertex1WhenVertex1Closest()
        {
            var network = new RouterDb();
            var vertex1 = network.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = network.AddVertex(4.797506332397461, 51.26674845584085);
            var edge = network.AddEdge(vertex1, vertex2);
            
            var snapPoint = network.SnapInBox((4.792613983154297 - 0.001, 51.26535213392538 - 0.001, 
                4.792613983154297 + 0.001, 51.26535213392538 + 0.001));
            Assert.Equal(edge, snapPoint.EdgeId);
            Assert.Equal(0, snapPoint.Offset);
        }

        [Fact]
        public void EdgeSearch_SnapInBox_ShouldSnapToVertex2WhenVertex2Closest()
        {
            var network = new RouterDb();
            var vertex1 = network.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = network.AddVertex(4.797506332397461, 51.26674845584085);
            var edge = network.AddEdge(vertex1, vertex2);
            
            var snapPoint = network.SnapInBox((4.797506332397461 - 0.001, 51.26674845584085 - 0.001, 
                4.797506332397461 + 0.001, 51.26674845584085 + 0.001));
            Assert.Equal(edge, snapPoint.EdgeId);
            Assert.Equal(ushort.MaxValue, snapPoint.Offset);
        }

        [Fact]
        public void EdgeSearch_SnapInBox_ShouldSnapToSegmentWhenMiddleIsClosest()
        {
            var network = new RouterDb();
            var vertex1 = network.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = network.AddVertex(4.797506332397461, 51.26674845584085);
            var edge = network.AddEdge(vertex1, vertex2);

            (double lon, double lat) middle = ((4.79261398315429 + 4.797506332397461) / 2,(51.26535213392538 + 51.26674845584085) / 2);
            var snapPoint = network.SnapInBox((middle.lon - 0.01, middle.lat - 0.01, 
                middle.lon + 0.01, middle.lat + 0.01));
            Assert.Equal(edge, snapPoint.EdgeId);
            Assert.Equal(0.5, ((float)snapPoint.Offset / ushort.MaxValue), 1);
        }

        [Fact]
        public void EdgeSearch_SnapInBox_MultipleEdges_ShouldSnapToSegmentWhenMiddleIsClosest()
        {
            var network = new RouterDb();
            var vertex1 = network.AddVertex(
                4.283750653266907,
                51.486985061332);
            var vertex2 = network.AddVertex(
                4.2828816175460815,
                51.48716544161935);
            var vertex3 = network.AddVertex(
                4.282275438308716,
                51.4854718430416);
            var vertex4 = network.AddVertex(
                4.283267855644226,
                51.485478524027414);
            var edge1 = network.AddEdge(vertex1, vertex2);
            var edge2 = network.AddEdge(vertex2, vertex3);
            var edge3 = network.AddEdge(vertex3, vertex4);
            var edge4 = network.AddEdge(vertex4, vertex1);

            (double lon, double lat) middle = ((4.283750653266907 + 4.2828816175460815) / 2,(51.486985061332 + 51.48716544161935) / 2);
            var snapPoint = network.SnapInBox((middle.lon - 0.01, middle.lat - 0.01, 
                middle.lon + 0.01, middle.lat + 0.01));
            Assert.Equal(edge1, snapPoint.EdgeId);
            Assert.Equal(0.5, ((float)snapPoint.Offset / ushort.MaxValue), 1);
        }
    }
}
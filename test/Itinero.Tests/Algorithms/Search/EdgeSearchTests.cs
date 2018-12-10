using Itinero.Algorithms.Search;
using Itinero.Data;
using NUnit.Framework;

namespace Itinero.Tests.Algorithms.Search
{
    public class EdgeSearchTests
    {
        [Test]
        public void EdgeSearch_SearchEdgesInBox_ShouldReturnNothingWhenNoEdges()
        {
            var network = new Network();
            network.AddVertex(4.792613983154297, 51.26535213392538);
            network.AddVertex(4.797506332397461, 51.26674845584085);

            var edges = network.Graph.SearchEdgesInBox((4.796, 51.265, 4.798, 51.267));
            Assert.IsNotNull(edges);
            Assert.IsFalse(edges.MoveNext());
        }
        
        [Test]
        public void EdgeSearch_SearchEdgesInBox_ShouldReturnEdgeWhenOneVertexInBox()
        {
            var network = new Network();
            var vertex1 = network.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = network.AddVertex(4.797506332397461, 51.26674845584085);
            var edge = network.AddEdge(vertex1, vertex2);
            
            var edges = network.Graph.SearchEdgesInBox((4.796, 51.265, 4.798, 51.267));
            Assert.IsNotNull(edges);
            Assert.IsTrue(edges.MoveNext());
            Assert.AreEqual(edge, edges.GraphEnumerator.Id);
            Assert.IsFalse(edges.MoveNext());
        }

        [Test]
        public void EdgeSearch_SnapInBox_ShouldSnapToVertex1WhenVertex1Closest()
        {
            var network = new Network();
            var vertex1 = network.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = network.AddVertex(4.797506332397461, 51.26674845584085);
            var edge = network.AddEdge(vertex1, vertex2);
            
            var snapPoint = network.SnapInBox((4.792613983154297 - 0.001, 51.26535213392538 - 0.001, 
                4.792613983154297 + 0.001, 51.26535213392538 + 0.001));
            Assert.AreEqual(edge, snapPoint.EdgeId);
            Assert.AreEqual(0, snapPoint.Offset);
        }

        [Test]
        public void EdgeSearch_SnapInBox_ShouldSnapToVertex2WhenVertex2Closest()
        {
            var network = new Network();
            var vertex1 = network.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = network.AddVertex(4.797506332397461, 51.26674845584085);
            var edge = network.AddEdge(vertex1, vertex2);
            
            var snapPoint = network.SnapInBox((4.797506332397461 - 0.001, 51.26674845584085 - 0.001, 
                4.797506332397461 + 0.001, 51.26674845584085 + 0.001));
            Assert.AreEqual(edge, snapPoint.EdgeId);
            // TODO: come up with a way to snap to vertices/shapepoints when they are just too close.
            //Assert.AreEqual(ushort.MaxValue, snapPoint.Offset, 10);
        }

        [Test]
        public void EdgeSearch_SnapInBox_ShouldSnapToSegmentWhenMiddleIsClosest()
        {
            var network = new Network();
            var vertex1 = network.AddVertex(4.792613983154297, 51.26535213392538);
            var vertex2 = network.AddVertex(4.797506332397461, 51.26674845584085);
            var edge = network.AddEdge(vertex1, vertex2);

            (double lon, double lat) middle = ((4.79261398315429 + 4.797506332397461) / 2,(51.26535213392538 + 51.26674845584085) / 2);
            var snapPoint = network.SnapInBox((middle.lon - 0.01, middle.lat - 0.01, 
                middle.lon + 0.01, middle.lat + 0.01));
            Assert.AreEqual(edge, snapPoint.EdgeId);
            Assert.AreEqual(ushort.MaxValue / 2, snapPoint.Offset, 10);
        }
    }
}
using Itinero.Algorithms.DataStructures;
using Itinero.Algorithms.Routes;
using Itinero.LocalGeo;
using Itinero.Profiles;
using Xunit;

namespace Itinero.Tests.Algorithms.Routes
{
    public class RouteBuilderTests
    {
        [Fact]
        public void RouteBuilder_Build_1EdgePath_ShouldReturn1EdgeRoute()
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
            
            // build path.
            var path = new Path(network.Network);
            path.Prepend(edge1, vertex2);

            // build route.
            var routeBuilder = new RouteBuilder();
            var routeResult = routeBuilder.Build(network, new DefaultProfile(), path);
            
            Assert.NotNull(routeResult);
            Route route = routeResult;
            Assert.NotNull(route);
            Assert.NotNull(route.Shape);
            Assert.Equal(2, route.Shape.Count);
            Assert.True(Coordinate.DistanceEstimateInMeter(network.GetVertex(vertex1), route.Shape[0]) < 2);
            Assert.True(Coordinate.DistanceEstimateInMeter(network.GetVertex(vertex2), route.Shape[1]) < 2);
        }
        
        [Fact]
        public void RouteBuilder_Build_1EdgePathReverse_ShouldReturn1EdgeRouteReverse()
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
            
            // build path.
            var path = new Path(network.Network);
            path.Prepend(edge1, vertex1);

            // build route.
            var routeBuilder = new RouteBuilder();
            var routeResult = routeBuilder.Build(network, new DefaultProfile(), path);
            
            Assert.NotNull(routeResult);
            Route route = routeResult;
            Assert.NotNull(route);
            Assert.NotNull(route.Shape);
            Assert.Equal(2, route.Shape.Count);
            Assert.True(Coordinate.DistanceEstimateInMeter(network.GetVertex(vertex2), route.Shape[0]) < 2);
            Assert.True(Coordinate.DistanceEstimateInMeter(network.GetVertex(vertex1), route.Shape[1]) < 2);
        }
    }
}
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
    }
}
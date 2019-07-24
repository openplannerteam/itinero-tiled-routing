using System.Threading.Tasks;
using Itinero.Algorithms;
using Itinero.Profiles;

namespace Itinero.Tests.Functional
{
    /// <summary>
    /// A simple point-to-point routing test.
    /// </summary>
    public class PointToPointRoutingTest : FunctionalTest<Route, (RouterDb routerDb, SnapPoint sp1, SnapPoint sp2, Profile profile)>
    {
        protected override async Task<Route> Execute((RouterDb routerDb, SnapPoint sp1, SnapPoint sp2, Profile profile) input)
        {
            var (routerDb, sp1, sp2, profile) = input;
            var route = await routerDb.Calculate( new RoutingSettings() { Profile = profile, MaxDistance = double.MaxValue }, sp1, sp2);
            return route.Value;
        }
        
        /// <summary>
        /// The default point-to-point routing test.
        /// </summary>
        public static readonly PointToPointRoutingTest Default = new PointToPointRoutingTest();
    }
}
using Itinero.Data.Attributes;
using Xunit;

namespace Itinero.Tests.Profiles.Lua.Osm
{
    public class BicycleTests
    {
        [Fact]
        public void Bicycle_Residential_ShouldReturnNonZeroFactor()
        {
            var profile = Itinero.Profiles.Lua.Osm.OsmProfiles.Bicycle;
            var factor = profile.Factor(new AttributeCollection(
                new Attribute("highway", "residential")));
            
            Assert.Equal(1, factor.ForwardPreferenceFactor, 0);
            Assert.Equal(1, factor.BackwardPreferenceFactor, 0);
            Assert.True(factor.BackwardFactor != 0);
            Assert.True(factor.ForwardFactor != 0);
        }
        
        [Fact]
        public void Bicycle_Pedestrian_ShouldReturnZeroFactor()
        {
            var profile = Itinero.Profiles.Lua.Osm.OsmProfiles.Bicycle;
            var factor = profile.Factor(new AttributeCollection(
                new Attribute("highway", "pedestrian")));
            
            Assert.True(factor.BackwardFactor == 0);
            Assert.True(factor.ForwardFactor == 0);
        }
        
        [Fact]
        public void Bicycle_OneWay_ShouldReturnZeroBackwardFactor()
        {
            var profile = Itinero.Profiles.Lua.Osm.OsmProfiles.Bicycle;
            var factor = profile.Factor(new AttributeCollection(
                new Attribute("highway", "residential"),
                new Attribute("oneway", "yes")));
            
            Assert.True(factor.BackwardFactor == 0);
            Assert.True(factor.ForwardFactor != 0);
        }
        
        [Fact]
        public void Bicycle_Cycleway_ShouldBeFavoured()
        {
            var profile = Itinero.Profiles.Lua.Osm.OsmProfiles.Bicycle;
            var factor = profile.Factor(new AttributeCollection(
                new Attribute("highway", "cycleway")));
            
            Assert.Equal(2, factor.ForwardPreferenceFactor, 0);
            Assert.Equal(2, factor.BackwardPreferenceFactor, 0);
            Assert.True(factor.BackwardFactor != 0);
            Assert.True(factor.ForwardFactor != 0);
        }
        
        [Fact]
        public void Bicycle_Primary_ShouldBeAvoided()
        {
            var profile = Itinero.Profiles.Lua.Osm.OsmProfiles.Bicycle;
            var factor = profile.Factor(new AttributeCollection(
                new Attribute("highway", "primary")));
            
            Assert.Equal(0.5, factor.ForwardPreferenceFactor, 1);
            Assert.Equal(0.5, factor.BackwardPreferenceFactor, 1);
            Assert.True(factor.BackwardFactor != 0);
            Assert.True(factor.ForwardFactor != 0);
        }
    }
}
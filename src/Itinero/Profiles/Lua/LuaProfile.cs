using System;
using Itinero.Data.Attributes;

namespace Itinero.Profiles.Lua
{
    /// <summary>
    /// Represents a dynamic routing profile that is based on a lua function.
    /// </summary>
    public class LuaProfile : Profile
    {
        private readonly Script _script;
        private readonly Table _attributesTable;
        private readonly Table _resultsTable;
        
        /// <summary>
        /// Creates a new dynamic profile.
        /// </summary>
        internal LuaProfile(Script script)
        {
            _script = script;
            
            _attributesTable = new Table(_script);
            _resultsTable = new Table(_script);
            
            var dynName = _script.Globals.Get("name");
            this.Name = dynName.String ?? throw new Exception("Dynamic profile doesn't define a name.");
        }

        /// <summary>
        /// Load profile from a raw lua script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns>The profile.</returns>
        public static LuaProfile Load(string script)
        {
            var s = new Script();
            s.DoString(script);
            return new LuaProfile(s);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name { get; }

        /// <summary>
        /// Get a function to calculate properties for a set given edge attributes.
        /// </summary>
        /// <returns></returns>
        public sealed override EdgeFactor Factor(IAttributeCollection attributes)
        {
            lock (_script)
            {
                // build lua table.
                _attributesTable.Clear();
                if (attributes == null || attributes.Count == 0)
                {
                    return EdgeFactor.NoFactor;
                }
                foreach (var attribute in attributes)
                {
                    _attributesTable.Set(attribute.Key, DynValue.NewString(attribute.Value));
                }

                // call factor_and_speed function.
                _resultsTable.Clear();
                _script.Call(_script.Globals["factor"], _attributesTable, _resultsTable);

                // get the results.
                if (!_resultsTable.TryGetDouble("forward", out var forwardFactor))
                {
                    forwardFactor = 0;
                }
                if (!_resultsTable.TryGetDouble("backward", out var backwardFactor))
                {
                    backwardFactor = 0;
                }
                if (!_resultsTable.TryGetBool("canstop", out var canstop))
                {
                    canstop = backwardFactor != 0 || forwardFactor != 0;
                }
                
                // the speeds are supposed to be in m/s.
                if (!_resultsTable.TryGetDouble("forward_speed", out var speedForward))
                { // when forward_speed isn't explicitly filled, the assumption is that factors are in 1/(m/s)
                    speedForward = 0;
                    if (forwardFactor > 0)
                    { // convert to m/s.
                        speedForward =  1.0 / forwardFactor;
                    }
                }
                
                if (!_resultsTable.TryGetDouble("backward_speed", out var speedBackward))
                { // when backward_speed isn't explicitly filled, the assumption is that factors are in 1/(m/s)
                    speedBackward = 0;
                    if (backwardFactor > 0)
                    { // convert to m/s.
                        speedBackward = 1.0 / backwardFactor;
                    }
                }

                return new EdgeFactor((uint)(forwardFactor * EdgeFactor.OffsetFactor), (uint)(backwardFactor * EdgeFactor.OffsetFactor), 
                    (ushort)(speedForward * EdgeFactor.OffsetFactor), (ushort)(speedBackward * EdgeFactor.OffsetFactor), canstop);
            }
        }
    }
}
namespace Itinero.Profiles.Handlers
{
    /// <summary>
    /// The default profile handler, no caching, no optimizations.
    /// </summary>
    internal class ProfileHandlerDefault : ProfileHandler
    {
        private readonly Profile _profile;

        public ProfileHandlerDefault(Profile profile)
        {
            _profile = profile;
        }

        private EdgeFactor? _edgeFactor = null;
        private uint _length;

        public override void MoveTo(RouterDbEdgeEnumerator enumerator)
        {
            _edgeFactor = enumerator.FactorInEdgeDirection(_profile);

            _length = enumerator.EdgeLengthCentimeter();
        }

        public override uint ForwardWeight => _edgeFactor.Value.ForwardFactor * _length;
        public override uint BackwardWeight => _edgeFactor.Value.ForwardFactor * _length;
        public override uint ForwardSpeed  => _edgeFactor.Value.ForwardSpeed;
        public override uint BackwardSpeed => _edgeFactor.Value.BackwardSpeed;
        public override bool CanStop  => _edgeFactor.Value.CanStop;
    }
}
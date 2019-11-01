namespace Itinero.Profiles
{
    /// <summary>
    /// A factor returned by a vehicle profile to influence routing augmented with estimated speed. 
    /// </summary>
    public struct EdgeFactor
    {
        /// <summary>
        /// Creates a new edge factor.
        /// </summary>
        /// <param name="forwardFactor">The forward factor. By default this is 1/km/h * 100.</param>
        /// <param name="backwardFactor">The backward factor. By default this is 1/km/h * 100.</param>
        /// <param name="forwardSpeed">The forward speed in ms/s * 100.</param>
        /// <param name="backwardSpeed">The backward speed in ms/s * 100.</param>
        /// <param name="canStop">The can stop.</param>
        public EdgeFactor(uint forwardFactor, uint backwardFactor,
            ushort forwardSpeed, ushort backwardSpeed, bool canStop = true)
        {
            this.ForwardFactor = forwardFactor;
            this.BackwardFactor = backwardFactor;
            this.ForwardSpeed = forwardSpeed;
            this.BackwardSpeed = backwardSpeed;
            this.CanStop = canStop;
        }
        
        internal const double OffsetFactor = 100; 
        
        /// <summary>
        /// Gets the forward factor, multiplied by an edge distance this is the weight.
        /// </summary>
        public uint ForwardFactor { get; }
        
        /// <summary>
        /// Gets the backward factor, multiplied by an edge distance this is the weight.
        /// </summary>
        public uint BackwardFactor { get; }
        
        /// <summary>
        /// Gets the backward speed in m/s multiplied by 100.
        /// </summary>
        public ushort BackwardSpeed { get; }

        /// <summary>
        /// Gets the backward speed in m/s.
        /// </summary>
        public double BackwardSpeedMeterPerSecond => this.BackwardSpeed / OffsetFactor;

        /// <summary>
        /// The factor that indicates preference of the default speed.
        /// </summary>
        public double BackwardPreferenceFactor =>(1 / (this.BackwardFactor / OffsetFactor)) / this.BackwardSpeedMeterPerSecond;

        /// <summary>
        /// Gets the forward speed in ms/s multiplied by 100.
        /// </summary>
        public ushort ForwardSpeed { get; }

        /// <summary>
        /// Gets the backward speed in m/s.
        /// </summary>
        public double ForwardSpeedMeterPerSecond => this.ForwardSpeed / OffsetFactor;

        /// <summary>
        /// The factor that indicates preference of the default speed.
        /// </summary>
        public double ForwardPreferenceFactor => (1 / (this.ForwardFactor / OffsetFactor)) / this.ForwardSpeedMeterPerSecond;

        /// <summary>
        /// Gets the can stop flag.
        /// </summary>
        public bool CanStop { get; }

        /// <summary>
        /// Gets a static no-factor.
        /// </summary>
        public static EdgeFactor NoFactor => new EdgeFactor(0, 0, 0, 0);
        
        /// <summary>
        /// Gets the exact reverse, switches backward and forward.
        /// </summary>
        public EdgeFactor Reverse => new EdgeFactor(this.BackwardFactor, this.ForwardFactor, this.BackwardSpeed, this.ForwardSpeed, this.CanStop);

        /// <inheritdoc/>
        public override string ToString()
        {
            var forwardSpeed = this.ForwardSpeed / OffsetFactor * 3.6;
            if (this.ForwardFactor == this.BackwardFactor &&
                this.ForwardSpeed == this.BackwardSpeed)
            {
                return $"{forwardSpeed:F1}km/h x {this.ForwardPreferenceFactor:F2} ({this.ForwardFactor:F0})";
            }
            var backwardSpeed = this.BackwardSpeed / OffsetFactor * 3.6;
            return $"F:{forwardSpeed:F1}km/h x {this.ForwardPreferenceFactor:F2} ({this.ForwardFactor:F0}) " +
                   $"B:{backwardSpeed:F1}km/h x {this.BackwardPreferenceFactor:F2} ({this.BackwardFactor:F0})";
        }
    }
}
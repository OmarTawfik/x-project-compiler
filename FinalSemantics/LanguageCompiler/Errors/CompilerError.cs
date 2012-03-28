namespace LanguageCompiler.Errors
{
    using Irony.Parsing;

    /// <summary>
    /// Holds all data related to a compiler error.
    /// </summary>
    public struct CompilerError
    {
        /// <summary>
        /// Starting location of this error.
        /// </summary>
        private SourceLocation startingLocation;

        /// <summary>
        /// Ending location of this error.
        /// </summary>
        private SourceLocation endingLocation;

        /// <summary>
        /// Message describing this error.
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes a new instance of the CompilerError struct.
        /// </summary>
        /// <param name="startingLocation">Starting location of this error.</param>
        /// <param name="endingLocation">Ennding location of this error.</param>
        /// <param name="message">Message describing this error.</param>
        public CompilerError(SourceLocation startingLocation, SourceLocation endingLocation, string message)
        {
            this.startingLocation = startingLocation;
            this.endingLocation = endingLocation;
            this.message = message;
        }

        /// <summary>
        /// Gets the starting location of this error.
        /// </summary>
        public SourceLocation StartingLocation
        {
            get { return this.startingLocation; }
        }

        /// <summary>
        /// Gets the ending location of this error.
        /// </summary>
        public SourceLocation EndingLocation
        {
            get { return this.endingLocation; }
        }

        /// <summary>
        /// Gets the message describing this error.
        /// </summary>
        public string Message
        {
            get { return this.message; }
        }
    }
}
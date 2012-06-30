namespace WPF_UVA.Controls
{
    /// <summary>
    /// Holds the name and length of a project sound.
    /// </summary>
    public class SoundData
    {
        /// <summary>
        /// Name of the sound file.
        /// </summary>
        private string name;

        /// <summary>
        /// Length of the sound file in seconds.
        /// </summary>
        private int seconds;

        /// <summary>
        /// Initializes a new instance of the SoundData class.
        /// </summary>
        /// <param name="name">Name of the sound file.</param>
        /// <param name="seconds">Length of the sound file in seconds.</param>
        public SoundData(string name, int seconds)
        {
            this.name = name;
            this.seconds = seconds;
        }

        /// <summary>
        /// Gets the name of the sound file.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the length of the sound file in seconds.
        /// </summary>
        public int Seconds
        {
            get { return this.seconds; }
        }
    }
}

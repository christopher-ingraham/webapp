namespace DA.WI.NSGHSM.Logic.Messaging
{
    public class MessageByte
    {
        #region Private Fields
        private int byteNumber;
        private int byteValue;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the byte number.
        /// </summary>
        /// <value>
        /// The byte number.
        /// </value>
        public int ByteNumber
        {
            get { return byteNumber; }
            set { byteNumber = value; }
        }

        /// <summary>
        /// Gets or sets the byte value.
        /// </summary>
        /// <value>
        /// The byte value.
        /// </value>
        public int ByteValue
        {
            get { return byteValue; }
            set { byteValue = value; }
        }
        #endregion
    }
}

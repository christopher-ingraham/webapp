using System;
using System.Collections.ObjectModel;
using System.Text;

namespace DA.WI.NSGHSM.Logic.Messaging
{
    public class MessageHeader
    {
        #region Private Fields
        private string domain;
        private string group;
        private string plcId;
        private string code;
        private int areaId;
        private int stationCode;
        private int replyId;
        private int replyResult;
        private byte plcOp;

        private string queueName;
        private int? channelNumber;
        private ulong deliveryTag;
        private string routingKey;
        #endregion

        #region Public Properties
        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        public string Group
        {
            get { return group; }
            set { group = value; }
        }

        public string PlcId
        {
            get { return plcId; }
            set { plcId = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public int AreaId
        {
            get { return areaId; }
            set { areaId = value; }
        }

        public int StationCode
        {
            get { return stationCode; }
            set { stationCode = value; }
        }

        public int ReplyId
        {
            get { return replyId; }
            set { replyId = value; }
        }

        public int ReplyResult
        {
            get { return replyResult; }
            set { replyResult = value; }
        }

        public byte PlcOp
        {
            get { return plcOp; }
            set { plcOp = value; }
        }

        public string QueueName
        {
            get { return queueName; }
            set { queueName = value; }
        }

        public int? ChannelNumber
        {
            get { return channelNumber; }
            set { channelNumber = value; }
        }

        public ulong DeliveryTag
        {
            get { return deliveryTag; }
            set { deliveryTag = value; }
        }

        public string RoutingKey
        {
            get { return routingKey; }
            set { routingKey = value; }
        }
        #endregion
    }
    public class Message
    {
        public MessageHeader header { get; set; }
        public ObservableCollection<MessageByte> messageByteCollection { get; set; }

        public byte[] byteArray { get; set; }

        public Message(MessageHeader header, ObservableCollection<MessageByte> messageByteCollection, byte[] byteArray)
        {
            this.header = header;
            this.messageByteCollection = messageByteCollection;
            this.byteArray = byteArray;
        }
        public Int32 decodeInt32(int offset)
        {
            byte[] bytes = {
                byteArray[2 + offset],
                byteArray[3 + offset],
                byteArray[0 + offset],
                byteArray[1 + offset],
             };

            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.
            //if (BitConverter.IsLittleEndian)
            // Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }


        public Int16 decodeInt16(int offset)
        {
            byte[] bytes = {
                byteArray[0 + offset],
                byteArray[1 + offset]
             };

            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.
            //if (BitConverter.IsLittleEndian)
            //Array.Reverse(bytes);

            return BitConverter.ToInt16(bytes, 0);
        }

        public string decodeString(int offset, int length)
        {
            byte[] bytes = new byte[length];
            for (int i=0; i<length; i+=2) {
                bytes[i] = byteArray[offset + i+1];
                bytes[i+1] = byteArray[offset + i];
            }

            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.
            //if (BitConverter.IsLittleEndian)
            // Array.Reverse(bytes);

            return Encoding.ASCII.GetString(bytes); 
        }

        public Single decodeSingle(int offset)
        {
            byte[] bytes = {
                byteArray[2 + offset],
                byteArray[3 + offset],
                byteArray[0 + offset],
                byteArray[1 + offset],
             };

            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.
            //if (BitConverter.IsLittleEndian)
            // Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes); ;
        }
    }
}

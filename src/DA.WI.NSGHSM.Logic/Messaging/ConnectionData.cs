namespace DA.WI.NSGHSM.Logic.Messaging
{
    public class ConnectionData
    {
        #region Private Fields
        public string hostName;
        public int portNumber;
        public string domainName;
        public int? areaId;
        public int? stationCode;
        public string subscribedGroup;
        public string status;
        #endregion

        #region Public Properties
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        public int PortNumber
        {
            get { return portNumber; }
            set {  portNumber = value;  }
        }

        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        public int? AreaId
        {
            get { return areaId; }
            set { areaId = value; }
        }

        public int? StationCode
        {
            get { return stationCode; }
            set { stationCode = value; }
        }

        public string SubscribedGroup
        {
            get { return subscribedGroup; }
            set { subscribedGroup = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        #endregion
    }
}

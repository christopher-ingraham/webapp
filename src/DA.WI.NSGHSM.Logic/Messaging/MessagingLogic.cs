using DA.MW.Broker.Common.Entities;
using DA.MW.Broker.Common.Protocol;
using DA.MW.Broker.Common;
using DA.MW.Broker;
using DA.WI.NSGHSM.Dto.PlantOverview;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System;
using System.Collections;

namespace DA.WI.NSGHSM.Logic.Messaging
{
    public class MessageBrokerConfiguration
    {
        private IConfiguration configuration;

        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Domain { get; private set; }
        public int AreaId { get; private set; }
        public int StationCode { get; private set; }
        public string RabbitUserName { get; private set; }
        public string RabbitUserPass { get; private set; }

        public MessageBrokerConfiguration(IConfiguration iConfiguration)
        {
            this.configuration = iConfiguration;
            var MessageBroker = iConfiguration.GetSection("MessageBroker");
            Host = MessageBroker.GetValue<string>("Host");
            Port = MessageBroker.GetValue<int>("Port");
            Domain = MessageBroker.GetValue<string>("Domain");
            AreaId = MessageBroker.GetValue<int>("AreaId");
            StationCode = MessageBroker.GetValue<int>("StationCode");
            RabbitUserName = MessageBroker.GetValue<string>("RabbitUserName");
            RabbitUserPass = MessageBroker.GetValue<string>("RabbitUserPass");
        }
    }

    public class MessagingLogic
    {
        internal IBrokerClient brokerClient;
        internal BrokerQueue brokerQueue;
        private bool IsConnected;
        private bool isFetch;
        private bool isSend;
        private bool isSafeSend;
        private int msgLength;
        private DateTime? msgDateTime;
        private DateTime? receivedDateTime;
        private string messageBodyAsText;
        MessageBrokerConfiguration configuration;
        ILogger<MessagingLogic> logger;

        public RoughingMillDataDto roughingMillDataActual { get; set; }
        public FinishingMillDataDto finishingMillDataActual { get; set; }
        public CoolingDowncoilersDataDto coolingDowncoilersDataDto { get; set; }
        public Subject<Message> messageObservable { get; set; }
        public MessagingLogic(IConfiguration iConfiguration, ILogger<MessagingLogic> _logger)
        {
            this.configuration = new MessageBrokerConfiguration(iConfiguration);
            this.logger = _logger;
            logger.LogInformation("messaging logic start");
            this.messageObservable = new Subject<Message>();

            int numStandRM = 2;
            roughingMillDataActual = new RoughingMillDataDto();
            roughingMillDataActual.DescalerData = new RoughingMillDescalerDataDto();
            roughingMillDataActual.DescalerData.Status = new StepStatusDto();
            roughingMillDataActual.IntensiveData = new RoughingMillIntensiveDataDto[numStandRM];
            for (int i = 0; i < numStandRM; i++)
            {
                roughingMillDataActual.IntensiveData[i] = new RoughingMillIntensiveDataDto();
                roughingMillDataActual.IntensiveData[i].BottomStatus = new StepStatusDto();
                roughingMillDataActual.IntensiveData[i].TopStatus = new StepStatusDto();
            }
            roughingMillDataActual.AfterData = new RoughingMillAfterDataDto();
            roughingMillDataActual.AfterData.ActualData = new RoughingMillAfterActualValueDto();
            roughingMillDataActual.EdgerData = new RoughingMillEdgerDataDto();
            roughingMillDataActual.EdgerData.Status = new StepStatusDto();
            roughingMillDataActual.EdgerData.ActualData = new RoughingMillEdgerActualValueDto();
            roughingMillDataActual.StandData = new RoughingMillStandDataDto[numStandRM];
            for (int i = 0; i < numStandRM; i++)
            {
                roughingMillDataActual.StandData[i] = new RoughingMillStandDataDto();
                roughingMillDataActual.StandData[i].ActualData = new RoughingMillValueDto();
                roughingMillDataActual.StandData[i].Status = new StepStatusDto();
            }

            int numStandFM = 6;
            finishingMillDataActual = new FinishingMillDataDto();
            finishingMillDataActual.StandData = new FinishingMillStandDataDto[numStandFM];
            for (int i = 0; i < numStandFM; i++)
            {
                finishingMillDataActual.StandData[i] = new FinishingMillStandDataDto();
                finishingMillDataActual.StandData[i].ActualData = new FinishingMillStandValueDto();
                finishingMillDataActual.StandData[i].Status = new StepStatusDto();
            }
            finishingMillDataActual.EntryData = new FinishingMillEntryData();
            finishingMillDataActual.ExitData = new FinishingMillExitData();
            finishingMillDataActual.EntryData.ActualData = new FinishingMillEntryActualValueDto();
            finishingMillDataActual.ExitData.ActualData = new FinishingMillExitActualValueDto();
            finishingMillDataActual.DescalerData = new FinishingMillDescalerDataDto();
            finishingMillDataActual.DescalerData.Status = new StepStatusDto();

            int numDC = 2;
            int numIntensive = 3;
            int numNormal = 6;
            int numTrimming = 2;
            coolingDowncoilersDataDto = new CoolingDowncoilersDataDto();
            coolingDowncoilersDataDto.DowncoilerData = new DowncoilersDataDto[numDC];
            for (int i = 0; i < numDC; i++)
            {
                coolingDowncoilersDataDto.DowncoilerData[i] = new DowncoilersDataDto();
                coolingDowncoilersDataDto.DowncoilerData[i].ActualData = new DowncoilersDataActualValueDto();
                coolingDowncoilersDataDto.DowncoilerData[i].Status = new StepStatusDto();
            }
            coolingDowncoilersDataDto.IntensiveData = new IntensiveDataDto[numIntensive];
            for (int i = 0; i < numIntensive; i++)
            {
                coolingDowncoilersDataDto.IntensiveData[i] = new IntensiveDataDto();
                coolingDowncoilersDataDto.IntensiveData[i].TopStatus = new StepStatusDto();
                coolingDowncoilersDataDto.IntensiveData[i].BottomStatus = new StepStatusDto();
            }
            coolingDowncoilersDataDto.NormalData = new NormalDataDto[numNormal];
            for (int i = 0; i < numNormal; i++)
            {
                coolingDowncoilersDataDto.NormalData[i] = new NormalDataDto();
                coolingDowncoilersDataDto.NormalData[i].ActualData = new NormalDataValueDto();
                coolingDowncoilersDataDto.NormalData[i].TopStatus = new StepStatusDto();
                coolingDowncoilersDataDto.NormalData[i].BottomStatus = new StepStatusDto();
            }
            coolingDowncoilersDataDto.TrimmingData = new TrimmingDataDto[numTrimming];
            for (int i = 0; i < numTrimming; i++)
            {
                coolingDowncoilersDataDto.TrimmingData[i] = new TrimmingDataDto();
                coolingDowncoilersDataDto.TrimmingData[i].ActualData = new TrimmingDataValueDto();
                coolingDowncoilersDataDto.TrimmingData[i].TopStatus = new StepStatusDto();
                coolingDowncoilersDataDto.TrimmingData[i].BottomStatus = new StepStatusDto();
            }
            coolingDowncoilersDataDto.RollssmallData = new RollssmallData();
            coolingDowncoilersDataDto.RollssmallData.Status = new StepStatusDto();
            coolingDowncoilersDataDto.EntryPyrometerData = new EntryPyrometerDataDto();
            coolingDowncoilersDataDto.ExitPyrometerData = new ExitPyrometerDataDto();
            coolingDowncoilersDataDto.IntPyrometerData = new IntPyrometerDataDto();
            coolingDowncoilersDataDto.EntryPyrometerData.ActualData = new EntryPyrometerActualValueDto();
            coolingDowncoilersDataDto.ExitPyrometerData.ActualData = new ExitPyrometerActualValueDto();
            coolingDowncoilersDataDto.IntPyrometerData.ActualData = new IntPyrometerActualValueDto();

            connect(null);
        }


        public void connect(ConfigurationParams config)
        {
            config = new ConfigurationParams
            {
                BrokerType = BrokerType.RabbitMQ,
                Host = configuration.Host,
                Port = configuration.Port,
                Domain = configuration.Domain,
                AreaId = configuration.AreaId,
                StationCode = configuration.StationCode,
                RabbitUserName = configuration.RabbitUserName,
                RabbitUserPass = configuration.RabbitUserPass
            };
            BrokerClientFactoryParams bcfp = new BrokerClientFactoryParams(config, BrokerUtils.GetBasicTaskScheduler())
            {
                DllLookupPath = @"../Assemblies/DAMWLib/",
                EventConnectionEstablished = onEventConnectionEstablished,
                EventConnectionLost = onEventConnectionLost,
                EventConnectionReattempt = onEventConnectionReattempt
            };
            //creating and opening the connection^M
            brokerClient = new BrokerClientFactory().CreateBrokerClient(bcfp);

            //creation of the queue using Guid and connect the method that manage the received messages^M
            brokerQueue = brokerClient?.CreateQueue(new BrokerQueueParams($"DAMWBrokerReceiver.{Guid.NewGuid()}"), ManageReceivedMessage);

            subscribe();

        }

        protected void subscribe()
        {
            brokerClient?.Subscribe(brokerQueue, new List<string> { "*.*.*.*.*", "*" });
        }

        protected void onEventConnectionEstablished()
        {
            logger.LogInformation("Connection established");
            IsConnected = true;
        }

        protected void onEventConnectionLost()
        {
            logger.LogInformation("Connection lost");
            IsConnected = false;

        }

        protected void onEventConnectionReattempt()
        {
            logger.LogInformation("Connection reattempt");
        }

        private void DisconnectCommandExecute(object obj)
        {
            //closing the connection
            brokerClient?.Dispose();
            brokerClient = null;
            IsConnected = false;
            /*
                        NewSubscription = "";
                        SubscriptionList.Clear();
                        MessageBodyAsText = "";
                        MessageBodyAsBytes.Clear();
                        Header = new MessageHeader();
            */
        }

        private IBrokerMessageAck ManageReceivedMessage(IBrokerMessage receivedMessage)
        {
            if (receivedMessage != null)
            {
                MessageHeader Header = new MessageHeader();
                ObservableCollection<MessageByte> messageBodyAsBytes;

                Header.Domain = receivedMessage.Header.Domain;
                Header.Group = receivedMessage.Header.Group;
                Header.PlcId = receivedMessage.Header.PlcId;
                Header.Code = receivedMessage.Header.Code;
                Header.AreaId = receivedMessage.Header.AreaId.Value;
                Header.StationCode = receivedMessage.Header.StationCode.Value;
                Header.ReplyId = receivedMessage.Header.ReplyId;
                Header.ReplyResult = Convert.ToByte(receivedMessage.Header.ReplyResult);
                Header.PlcOp = receivedMessage.Header.PlcOp;

                msgLength = receivedMessage.Body.Count();
                msgDateTime = DateTime.FromOADate(receivedMessage.Header.TimeStamp);
                receivedDateTime = DateTime.Now;

                //convertion of the PLC Operation in the booleans that are show in the view
                int poType = receivedMessage.Header.PlcOp;
                switch (poType)
                {
                    case 2:
                        {
                            isFetch = true;
                            break;
                        }
                    case 3:
                        {
                            isSend = true;
                            break;
                        }
                    case 4:
                        {
                            isSafeSend = true;
                            break;
                        }
                }

                //if you are connected to RabbitMQ, the message that you will receive it's a IBrokerMessageExt
                //it contains more info compared to the IBrokerMessage (the Dispatcher one)
                IBrokerMessageExt receivedMessageExt = (IBrokerMessageExt)receivedMessage;
                Header.QueueName = receivedMessageExt.QueueName;
                Header.ChannelNumber = receivedMessageExt.ChannelNumber;
                Header.DeliveryTag = receivedMessageExt.DeliveryTag;
                Header.RoutingKey = receivedMessageExt.RoutingKey;

                messageBodyAsBytes = new ObservableCollection<MessageByte>();
                for (int i = 1; i <= receivedMessage.Body.Count(); i++)
                {
                    MessageByte tempB = new MessageByte();
                    tempB.ByteNumber = i;
                    tempB.ByteValue = Convert.ToInt32(receivedMessage.Body[i - 1]);
                    messageBodyAsBytes.Add(tempB);
                }

                messageBodyAsBytes.OrderBy(bn => bn.ByteNumber);
                //MessageBodyAsText = BrokerUtils.Deserialize<string>(receivedMessage.Body);
                messageBodyAsText = Encoding.UTF8.GetString(receivedMessage.Body);
                Message message = new Message(Header, messageBodyAsBytes, receivedMessage.Body);
                messageObservable.OnNext(new Message(Header, messageBodyAsBytes, receivedMessage.Body));
                logger.LogInformation("Message Received code:" + Header.Code);
                switch (Header.Code)
                {
                    case "1106":
                        this.processMessageRM(message);
                        break;
                    case "1107":
                        this.processMessageFM(message);
                        break;
                    case "1108":
                        this.processMessageCD(message);
                        break;
                }
            }

            return BrokerUtils.CreateMessageAck(receivedMessage, AcknowledgementType.Ack);
        }

        protected void processMessageRM(Message message)
        {
            if (message.header.Code != "1106")
            {
                return;
            }
            logger.LogInformation("RoughingMill received message: " + message.header.Code);
            // rispetto al file excel (LANDEF) sono sfasati di -2 (ad esempio LANDEF 154 => messaggio 152)
            logger.LogInformation("piece number: " + message.decodeInt32(12));
            if (message.decodeInt32(12) > 0)
            {
                roughingMillDataActual.PieceNo = message.decodeInt32(12);
            };
            logger.LogInformation("piece id: " + message.decodeString(16, 10));

            logger.LogInformation("Stand 1 - - - - -");
            logger.LogInformation("Exit Thickness: " + message.decodeSingle(232));
            logger.LogInformation("Rolling force: " + message.decodeSingle(296));
            logger.LogInformation("Stand Speed: " + message.decodeSingle(336));
            logger.LogInformation("Reduction: " + message.decodeSingle(384));
            logger.LogInformation("Gap: " + message.decodeSingle(368));
            logger.LogInformation("WR Bending: " + message.decodeSingle(400));

            logger.LogInformation("Stand 2 - - - - -");
            logger.LogInformation("Exit Thickness: " + message.decodeSingle(236));
            logger.LogInformation("Rolling force: " + message.decodeSingle(300));
            logger.LogInformation("Stand Speed: " + message.decodeSingle(340));
            logger.LogInformation("Reduction: " + message.decodeSingle(388));
            logger.LogInformation("Gap: " + message.decodeSingle(372));
            logger.LogInformation("WR Bending: " + message.decodeSingle(404));

            logger.LogInformation("Before first stand -----");
            logger.LogInformation("Entry Width: " + message.decodeSingle(212));
            logger.LogInformation("Entry Speed: " + message.decodeSingle(204));
            logger.LogInformation("Entry Temperature 1: " + message.decodeSingle(184));
            logger.LogInformation("Entry Temperature 2: " + message.decodeSingle(188));
            logger.LogInformation("Entry Temperature 3: " + message.decodeSingle(192));
            logger.LogInformation("Edger Width: " + message.decodeSingle(130));
            logger.LogInformation("Edger Force: " + message.decodeSingle(134));

            logger.LogInformation("After last stand -----");
            logger.LogInformation("Exit Width: " + message.decodeSingle(272));
            logger.LogInformation("Exit Speed: " + message.decodeSingle(264));
            logger.LogInformation("Exit Temperature 1: " + message.decodeSingle(244));
            logger.LogInformation("Exit Temperature 2: " + message.decodeSingle(248));
            logger.LogInformation("Exit Temperature 3: " + message.decodeSingle(252));

            
            // --------- Entry Width
            if((message.decodeInt16(76) & 0x04) == 0){ roughingMillDataActual.EdgerData.ActualData.EntryWidthStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x04) > 0) && ((message.decodeInt16(78) & 0x04) > 0)){roughingMillDataActual.EdgerData.ActualData.EntryWidthStatus = 1;} 
            else {roughingMillDataActual.EdgerData.ActualData.EntryWidthStatus = 2;};

            // --------- Entry Speed
            if((message.decodeInt16(76) & 0x08) == 0){ roughingMillDataActual.EdgerData.ActualData.EntrySpeedStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x08) > 0) && ((message.decodeInt16(78) & 0x08) > 0)){roughingMillDataActual.EdgerData.ActualData.EntrySpeedStatus = 1;} 
            else {roughingMillDataActual.EdgerData.ActualData.EntrySpeedStatus = 2;};

            // --------- Entry Temperature 1,2,3
            if((message.decodeInt16(76) & 0x01) == 0){ roughingMillDataActual.EdgerData.ActualData.EntryTempStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x01) > 0) && ((message.decodeInt16(78) & 0x01) > 0)){roughingMillDataActual.EdgerData.ActualData.EntryTempStatus = 1;} 
            else {roughingMillDataActual.EdgerData.ActualData.EntryTempStatus = 2;};

            // --------- Exit Width
            if((message.decodeInt16(76) & 0x100) == 0){ roughingMillDataActual.AfterData.ActualData.ExitWidthStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x100) > 0) && ((message.decodeInt16(78) & 0x100) > 0)){roughingMillDataActual.AfterData.ActualData.ExitWidthStatus = 1;} 
            else {roughingMillDataActual.AfterData.ActualData.ExitWidthStatus = 2;};

            // --------- Exit Speed
            if((message.decodeInt16(76) & 0x200) == 0){ roughingMillDataActual.AfterData.ActualData.ExitSpeedStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x200) > 0) && ((message.decodeInt16(78) & 0x200) > 0)){roughingMillDataActual.AfterData.ActualData.ExitSpeedStatus = 1;} 
            else {roughingMillDataActual.AfterData.ActualData.ExitSpeedStatus = 2;};

            // --------- Exit Temperature 1,2,3
            if((message.decodeInt16(76) & 0x40) == 0){ roughingMillDataActual.AfterData.ActualData.ExitTempStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x40) > 0) && ((message.decodeInt16(78) & 0x40) > 0)){roughingMillDataActual.AfterData.ActualData.ExitTempStatus = 1;} 
            else {roughingMillDataActual.AfterData.ActualData.ExitTempStatus = 2;};
           

            roughingMillDataActual.AfterData.ActualData.ExitWidth = message.decodeSingle(272);
            roughingMillDataActual.AfterData.ActualData.ExitSpeed = message.decodeSingle(264);
            roughingMillDataActual.AfterData.ActualData.ExitTemp1 = message.decodeSingle(244);
            roughingMillDataActual.AfterData.ActualData.ExitTemp2 = message.decodeSingle(248);
            roughingMillDataActual.AfterData.ActualData.ExitTemp3 = message.decodeSingle(252);

            roughingMillDataActual.EdgerData.ActualData.EntryWidth = message.decodeSingle(212);
            roughingMillDataActual.EdgerData.ActualData.EntrySpeed = message.decodeSingle(204);
            roughingMillDataActual.EdgerData.ActualData.EntryTemp1 = message.decodeSingle(184);
            roughingMillDataActual.EdgerData.ActualData.EntryTemp2 = message.decodeSingle(188);
            roughingMillDataActual.EdgerData.ActualData.EntryTemp3 = message.decodeSingle(192);
            roughingMillDataActual.EdgerData.ActualData.EdgerWidth = message.decodeSingle(130);
            roughingMillDataActual.EdgerData.ActualData.EdgerForce = message.decodeSingle(134);

            roughingMillDataActual.StandData[0].ActualData.ExitThickness = message.decodeSingle(232);
            roughingMillDataActual.StandData[0].ActualData.RollingForce = message.decodeSingle(296);
            roughingMillDataActual.StandData[0].ActualData.StandSpeed = message.decodeSingle(336);
            roughingMillDataActual.StandData[0].ActualData.Reduction = message.decodeSingle(384);
            roughingMillDataActual.StandData[0].ActualData.Gap = message.decodeSingle(368);
            roughingMillDataActual.StandData[0].ActualData.WRBending = message.decodeSingle(400);

            roughingMillDataActual.StandData[1].ActualData.ExitThickness = message.decodeSingle(236);
            roughingMillDataActual.StandData[1].ActualData.RollingForce = message.decodeSingle(300);
            roughingMillDataActual.StandData[1].ActualData.StandSpeed = message.decodeSingle(340);
            roughingMillDataActual.StandData[1].ActualData.Reduction = message.decodeSingle(388);
            roughingMillDataActual.StandData[1].ActualData.Gap = message.decodeSingle(372);
            roughingMillDataActual.StandData[1].ActualData.WRBending = message.decodeSingle(404);

            // DESCALER STATUS ---------------------------------------------------------------------------------------------------------
            if (message.decodeInt16(154) > 0) { roughingMillDataActual.DescalerData.Status.Run = true; } else { roughingMillDataActual.DescalerData.Status.Run = false; }
            if ((message.decodeInt16(78) & 0x01) > 0 || (message.decodeInt16(78) & 0x04) > 0) { roughingMillDataActual.DescalerData.Status.Mis = true; } else { roughingMillDataActual.DescalerData.Status.Mis = false; }

            // EDGER STATUS ---------------------------------------------------------------------------------------------------------
            if (message.decodeSingle(138) > 1) { roughingMillDataActual.EdgerData.Status.Run = true; } else { roughingMillDataActual.EdgerData.Status.Run = false; }
            if (message.decodeInt16(128) > 0) { roughingMillDataActual.EdgerData.Status.Mis = true; } else { roughingMillDataActual.EdgerData.Status.Mis = false; }
            if (message.decodeInt16(126) > 0) { roughingMillDataActual.EdgerData.Status.Dis = true; } else { roughingMillDataActual.EdgerData.Status.Dis = false; }

            // STANDS STATUS ---------------------------------------------------------------------------------------------------------
            if (message.decodeSingle(336) > 0.1) { roughingMillDataActual.StandData[0].Status.Run = true; } else { roughingMillDataActual.StandData[0].Status.Run = false; }
            if (message.decodeSingle(336) > 0.1) { roughingMillDataActual.StandData[1].Status.Run = true; } else { roughingMillDataActual.StandData[1].Status.Run = false; }
            if ((message.decodeInt16(68) & 0x01) > 0) { roughingMillDataActual.StandData[0].Status.Mis = true; } else { roughingMillDataActual.StandData[0].Status.Mis = false; }
            if ((message.decodeInt16(68) & 0x02) > 0) { roughingMillDataActual.StandData[1].Status.Mis = true; } else { roughingMillDataActual.StandData[1].Status.Mis = false; }
            if ((message.decodeInt16(70) & 0x01) > 0) { roughingMillDataActual.StandData[0].Status.Dis = true; } else { roughingMillDataActual.StandData[0].Status.Dis = false; }
            if ((message.decodeInt16(70) & 0x02) > 0) { roughingMillDataActual.StandData[1].Status.Dis = true; } else { roughingMillDataActual.StandData[1].Status.Dis = false; }

            // INTENSIVE STATUS ---------------------------------------------------------------------------------------------------------

            for (int i = 0; i < 2; i++)
            {
                if ((message.decodeInt32(522) > 0) && (message.decodeInt32(530) != 0)) { roughingMillDataActual.IntensiveData[i].TopStatus.Run = true; } else { roughingMillDataActual.IntensiveData[i].TopStatus.Run = false; }
                if ((message.decodeInt32(534) > 0) && (message.decodeInt32(542) != 0)) { roughingMillDataActual.IntensiveData[i].BottomStatus.Run = true; } else { roughingMillDataActual.IntensiveData[i].BottomStatus.Run = false; }
                if ((message.decodeInt16(78) & 0x20) > 0 || (message.decodeInt16(78) & 0x100) > 0) { roughingMillDataActual.IntensiveData[i].TopStatus.Mis = true; } else { roughingMillDataActual.IntensiveData[i].TopStatus.Mis = false; }
                if ((message.decodeInt16(78) & 0x20) > 0 || (message.decodeInt16(78) & 0x100) > 0) { roughingMillDataActual.IntensiveData[i].BottomStatus.Mis = true; } else { roughingMillDataActual.IntensiveData[i].BottomStatus.Mis = false; }
            }

        }

        protected void processMessageFM(Message message)
        {
            if (message.header.Code != "1107")
            {
                return;
            }
            // rispetto al file excel (LANDEF) sono sfasati di -2 (ad esempio LANDEF 154 => messaggio 152) 
            logger.LogInformation("FinishingMill received message: " + message.header.Code);
            logger.LogInformation("piece number: " + message.decodeInt32(12));

            if (message.decodeInt32(12) > 0)
            {
                finishingMillDataActual.PieceNo = message.decodeInt32(12);
            }
            logger.LogInformation("piece id: " + message.decodeString(16, 10));

            logger.LogInformation("Stand 1 - - - - -");
            logger.LogInformation("Exit Thickness: " + message.decodeSingle(260));
            logger.LogInformation("Rolling force: " + message.decodeSingle(388));
            logger.LogInformation("Stand Speed: " + message.decodeSingle(508));
            logger.LogInformation("Reduction: " + message.decodeSingle(652));
            logger.LogInformation("Gap: " + message.decodeSingle(604));
            logger.LogInformation("WR Bending: " + message.decodeSingle(700));
            logger.LogInformation("WR Shifting: " + message.decodeSingle(676));
            logger.LogInformation("Looper Angle: " + message.decodeSingle(884));
            logger.LogInformation("Looper Specific Tension: " + message.decodeSingle(844));
            logger.LogInformation("Interstand Cooling Flow: " + message.decodeSingle(928));

            logger.LogInformation("Stand 2 - - - - -");
            logger.LogInformation("Exit Thickness: " + message.decodeSingle(264));
            logger.LogInformation("Rolling force: " + message.decodeSingle(392));
            logger.LogInformation("Stand Speed: " + message.decodeSingle(512));
            logger.LogInformation("Reduction: " + message.decodeSingle(656));
            logger.LogInformation("Gap: " + message.decodeSingle(608));
            logger.LogInformation("WR Bending: " + message.decodeSingle(704));
            logger.LogInformation("WR Shifting: " + message.decodeSingle(680));
            logger.LogInformation("Looper Angle: " + message.decodeSingle(888));
            logger.LogInformation("Looper Specific Tension: " + message.decodeSingle(848));
            logger.LogInformation("Interstand Cooling Flow: " + message.decodeSingle(932));

            logger.LogInformation("Stand 3 - - - - -");
            logger.LogInformation("Exit Thickness: " + message.decodeSingle(268));
            logger.LogInformation("Rolling force: " + message.decodeSingle(396));
            logger.LogInformation("Stand Speed: " + message.decodeSingle(516));
            logger.LogInformation("Reduction: " + message.decodeSingle(660));
            logger.LogInformation("Gap: " + message.decodeSingle(612));
            logger.LogInformation("WR Bending: " + message.decodeSingle(708));
            logger.LogInformation("WR Shifting: " + message.decodeSingle(684));
            logger.LogInformation("Looper Angle: " + message.decodeSingle(892));
            logger.LogInformation("Looper Specific Tension: " + message.decodeSingle(852));
            logger.LogInformation("Interstand Cooling Flow: " + message.decodeSingle(936));

            logger.LogInformation("Stand 4 - - - - -");
            logger.LogInformation("Exit Thickness: " + message.decodeSingle(272));
            logger.LogInformation("Rolling force: " + message.decodeSingle(400));
            logger.LogInformation("Stand Speed: " + message.decodeSingle(520));
            logger.LogInformation("Reduction: " + message.decodeSingle(664));
            logger.LogInformation("Gap: " + message.decodeSingle(616));
            logger.LogInformation("WR Bending: " + message.decodeSingle(712));
            logger.LogInformation("WR Shifting: " + message.decodeSingle(688));
            logger.LogInformation("Looper Angle: " + message.decodeSingle(896));
            logger.LogInformation("Looper Specific Tension: " + message.decodeSingle(856));
            logger.LogInformation("Interstand Cooling Flow: " + message.decodeSingle(940));

            logger.LogInformation("Stand 5 - - - - -");
            logger.LogInformation("Exit Thickness: " + message.decodeSingle(276));
            logger.LogInformation("Rolling force: " + message.decodeSingle(404));
            logger.LogInformation("Stand Speed: " + message.decodeSingle(524));
            logger.LogInformation("Reduction: " + message.decodeSingle(668));
            logger.LogInformation("Gap: " + message.decodeSingle(620));
            logger.LogInformation("WR Bending: " + message.decodeSingle(716));
            logger.LogInformation("WR Shifting: " + message.decodeSingle(692));
            logger.LogInformation("Looper Angle: " + message.decodeSingle(900));
            logger.LogInformation("Looper Specific Tension: " + message.decodeSingle(860));
            logger.LogInformation("Interstand Cooling Flow: " + message.decodeSingle(944));

            logger.LogInformation("Stand 6 - - - - -");
            logger.LogInformation("Exit Thickness: " + message.decodeSingle(280));
            logger.LogInformation("Rolling force: " + message.decodeSingle(408));
            logger.LogInformation("Stand Speed: " + message.decodeSingle(528));
            logger.LogInformation("Reduction: " + message.decodeSingle(672));
            logger.LogInformation("Gap: " + message.decodeSingle(624));
            logger.LogInformation("WR Bending: " + message.decodeSingle(720));
            logger.LogInformation("WR Shifting: " + message.decodeSingle(696));

            logger.LogInformation("Before first stand -----");
            logger.LogInformation("Entry Width: " + message.decodeSingle(228));
            logger.LogInformation("Entry Speed: " + message.decodeSingle(224));
            logger.LogInformation("Entry Temperature 1: " + message.decodeSingle(204));
            logger.LogInformation("Entry Temperature 2: " + message.decodeSingle(208));
            logger.LogInformation("Entry Temperature 3: " + message.decodeSingle(212));

            logger.LogInformation("After last stand -----");
            logger.LogInformation("Exit Thickness: " + message.decodeSingle(344) + " OR " + message.decodeSingle(364));
            logger.LogInformation("Exit Width: " + message.decodeSingle(328) + " OR " + message.decodeSingle(368));
            logger.LogInformation("Exit Speed: " + message.decodeSingle(324));
            logger.LogInformation("Flatness: " + message.decodeSingle(332) + " OR " + message.decodeSingle(372));
            logger.LogInformation("Crown (C25): " + message.decodeSingle(1056));
            logger.LogInformation("Crown (C40): " + message.decodeSingle(1052));
            logger.LogInformation("Wedge (W25): " + message.decodeSingle(1064));
            logger.LogInformation("Wedge (W40): " + message.decodeSingle(1060));
            logger.LogInformation("Exit Temperature 1: " + message.decodeSingle(304));
            logger.LogInformation("Exit Temperature 2: " + message.decodeSingle(308));
            logger.LogInformation("Exit Temperature 3: " + message.decodeSingle(312));


            finishingMillDataActual.EntryData.ActualData.EntryTemperature1 = message.decodeSingle(204);
            finishingMillDataActual.EntryData.ActualData.EntryTemperature2 = message.decodeSingle(208);
            finishingMillDataActual.EntryData.ActualData.EntryTemperature3 = message.decodeSingle(212);
            finishingMillDataActual.EntryData.ActualData.EntrySpeed = message.decodeSingle(224);
            finishingMillDataActual.EntryData.ActualData.EntryWidth = message.decodeSingle(228);

            finishingMillDataActual.StandData[0].ActualData.StepNo = 1;
            finishingMillDataActual.StandData[0].ActualData.ExitThickness = message.decodeSingle(260);
            finishingMillDataActual.StandData[0].ActualData.RollingForce = message.decodeSingle(388);
            finishingMillDataActual.StandData[0].ActualData.StandSpeed = message.decodeSingle(508);
            finishingMillDataActual.StandData[0].ActualData.Reduction = message.decodeSingle(652);
            finishingMillDataActual.StandData[0].ActualData.Gap = message.decodeSingle(604);
            finishingMillDataActual.StandData[0].ActualData.WRBending = message.decodeSingle(700);
            finishingMillDataActual.StandData[0].ActualData.WRShifting = message.decodeSingle(676);
            finishingMillDataActual.StandData[0].ActualData.LooperAngle = message.decodeSingle(884);
            finishingMillDataActual.StandData[0].ActualData.LooperSpecificTension = message.decodeSingle(844);
            finishingMillDataActual.StandData[0].ActualData.InterstandCoolingFlow = message.decodeSingle(928);

            finishingMillDataActual.StandData[1].ActualData.StepNo = 2;
            finishingMillDataActual.StandData[1].ActualData.ExitThickness = message.decodeSingle(264);
            finishingMillDataActual.StandData[1].ActualData.RollingForce = message.decodeSingle(392);
            finishingMillDataActual.StandData[1].ActualData.StandSpeed = message.decodeSingle(512);
            finishingMillDataActual.StandData[1].ActualData.Reduction = message.decodeSingle(656);
            finishingMillDataActual.StandData[1].ActualData.Gap = message.decodeSingle(608);
            finishingMillDataActual.StandData[1].ActualData.WRBending = message.decodeSingle(704);
            finishingMillDataActual.StandData[1].ActualData.WRShifting = message.decodeSingle(680);
            finishingMillDataActual.StandData[1].ActualData.LooperAngle = message.decodeSingle(888);
            finishingMillDataActual.StandData[1].ActualData.LooperSpecificTension = message.decodeSingle(848);
            finishingMillDataActual.StandData[1].ActualData.InterstandCoolingFlow = message.decodeSingle(932);

            finishingMillDataActual.StandData[2].ActualData.StepNo = 3;
            finishingMillDataActual.StandData[2].ActualData.ExitThickness = message.decodeSingle(268);
            finishingMillDataActual.StandData[2].ActualData.RollingForce = message.decodeSingle(396);
            finishingMillDataActual.StandData[2].ActualData.StandSpeed = message.decodeSingle(516);
            finishingMillDataActual.StandData[2].ActualData.Reduction = message.decodeSingle(660);
            finishingMillDataActual.StandData[2].ActualData.Gap = message.decodeSingle(612);
            finishingMillDataActual.StandData[2].ActualData.WRBending = message.decodeSingle(708);
            finishingMillDataActual.StandData[2].ActualData.WRShifting = message.decodeSingle(684);
            finishingMillDataActual.StandData[2].ActualData.LooperAngle = message.decodeSingle(892);
            finishingMillDataActual.StandData[2].ActualData.LooperSpecificTension = message.decodeSingle(852);
            finishingMillDataActual.StandData[2].ActualData.InterstandCoolingFlow = message.decodeSingle(936);

            finishingMillDataActual.StandData[3].ActualData.StepNo = 4;
            finishingMillDataActual.StandData[3].ActualData.ExitThickness = message.decodeSingle(272);
            finishingMillDataActual.StandData[3].ActualData.RollingForce = message.decodeSingle(400);
            finishingMillDataActual.StandData[3].ActualData.StandSpeed = message.decodeSingle(520);
            finishingMillDataActual.StandData[3].ActualData.Reduction = message.decodeSingle(664);
            finishingMillDataActual.StandData[3].ActualData.Gap = message.decodeSingle(616);
            finishingMillDataActual.StandData[3].ActualData.WRBending = message.decodeSingle(712);
            finishingMillDataActual.StandData[3].ActualData.WRShifting = message.decodeSingle(688);
            finishingMillDataActual.StandData[3].ActualData.LooperAngle = message.decodeSingle(896);
            finishingMillDataActual.StandData[3].ActualData.LooperSpecificTension = message.decodeSingle(856);
            finishingMillDataActual.StandData[3].ActualData.InterstandCoolingFlow = message.decodeSingle(940);

            finishingMillDataActual.StandData[4].ActualData.StepNo = 5;
            finishingMillDataActual.StandData[4].ActualData.ExitThickness = message.decodeSingle(276);
            finishingMillDataActual.StandData[4].ActualData.RollingForce = message.decodeSingle(404);
            finishingMillDataActual.StandData[4].ActualData.StandSpeed = message.decodeSingle(524);
            finishingMillDataActual.StandData[4].ActualData.Reduction = message.decodeSingle(668);
            finishingMillDataActual.StandData[4].ActualData.Gap = message.decodeSingle(620);
            finishingMillDataActual.StandData[4].ActualData.WRBending = message.decodeSingle(716);
            finishingMillDataActual.StandData[4].ActualData.WRShifting = message.decodeSingle(692);
            finishingMillDataActual.StandData[4].ActualData.LooperAngle = message.decodeSingle(900);
            finishingMillDataActual.StandData[4].ActualData.LooperSpecificTension = message.decodeSingle(860);
            finishingMillDataActual.StandData[4].ActualData.InterstandCoolingFlow = message.decodeSingle(944);

            finishingMillDataActual.StandData[5].ActualData.StepNo = 6;
            finishingMillDataActual.StandData[5].ActualData.ExitThickness = message.decodeSingle(280);
            finishingMillDataActual.StandData[5].ActualData.RollingForce = message.decodeSingle(408);
            finishingMillDataActual.StandData[5].ActualData.StandSpeed = message.decodeSingle(528);
            finishingMillDataActual.StandData[5].ActualData.Reduction = message.decodeSingle(672);
            finishingMillDataActual.StandData[5].ActualData.Gap = message.decodeSingle(624);
            finishingMillDataActual.StandData[5].ActualData.WRBending = message.decodeSingle(720);
            finishingMillDataActual.StandData[5].ActualData.WRShifting = message.decodeSingle(696);

            if (((message.decodeInt16(1162) & 0x01) == 0) && ((message.decodeInt16(1162) & 0x02) > 0))
            {
                finishingMillDataActual.ExitData.ActualData.ExitThickness = message.decodeSingle(344);

                if((message.decodeInt16(76) & 0x2000) == 0){ finishingMillDataActual.ExitData.ActualData.ExitThicknessStatus = 0; } 
                else if (((message.decodeInt16(76) & 0x2000) > 0) && ((message.decodeInt16(78) & 0x2000) > 0)){finishingMillDataActual.ExitData.ActualData.ExitThicknessStatus = 1;} 
                else {finishingMillDataActual.ExitData.ActualData.ExitThicknessStatus = 2;};
            }
            else
            {
                finishingMillDataActual.ExitData.ActualData.ExitThickness = message.decodeSingle(364);

                if((message.decodeInt16(76) & 0x800) == 0){ finishingMillDataActual.ExitData.ActualData.ExitThicknessStatus = 0; } 
                else if (((message.decodeInt16(76) & 0x800) > 0) && ((message.decodeInt16(78) & 0x800) > 0)){finishingMillDataActual.ExitData.ActualData.ExitThicknessStatus = 1;} 
                else {finishingMillDataActual.ExitData.ActualData.ExitThicknessStatus = 2;};
            }

            if (((message.decodeInt16(1162) & 0x04) == 0) && ((message.decodeInt16(1162) & 0x08) > 0))
            {
                finishingMillDataActual.ExitData.ActualData.ExitWidth = message.decodeSingle(328);

                if((message.decodeInt16(76) & 0x1000) == 0){ finishingMillDataActual.ExitData.ActualData.ExitWidthStatus = 0; } 
                else if (((message.decodeInt16(76) & 0x1000) > 0) && ((message.decodeInt16(78) & 0x1000) > 0)){finishingMillDataActual.ExitData.ActualData.ExitWidthStatus = 1;} 
                else {finishingMillDataActual.ExitData.ActualData.ExitWidthStatus = 2;};
            }
            else
            {
                finishingMillDataActual.ExitData.ActualData.ExitWidth = message.decodeSingle(368);

                if((message.decodeInt16(76) & 0x800) == 0){ finishingMillDataActual.ExitData.ActualData.ExitWidthStatus = 0; } 
                else if (((message.decodeInt16(76) & 0x800) > 0) && ((message.decodeInt16(78) & 0x800) > 0)){finishingMillDataActual.ExitData.ActualData.ExitWidthStatus = 1;} 
                else {finishingMillDataActual.ExitData.ActualData.ExitWidthStatus = 2;};
            }

            if (((message.decodeInt16(1162) & 0x10) == 0) && ((message.decodeInt16(1162) & 0x20) > 0))
            {
                finishingMillDataActual.ExitData.ActualData.Flatness = message.decodeSingle(332);

                if((message.decodeInt16(76) & 0x1000) == 0){ finishingMillDataActual.ExitData.ActualData.FlatnessStatus = 0; } 
                else if (((message.decodeInt16(76) & 0x1000) > 0) && ((message.decodeInt16(78) & 0x1000) > 0)){finishingMillDataActual.ExitData.ActualData.FlatnessStatus = 1;} 
                else {finishingMillDataActual.ExitData.ActualData.FlatnessStatus = 2;};
            }
            else
            {
                finishingMillDataActual.ExitData.ActualData.Flatness = message.decodeSingle(372);
                
                if((message.decodeInt16(76) & 0x800) == 0){ finishingMillDataActual.ExitData.ActualData.FlatnessStatus = 0; } 
                else if (((message.decodeInt16(76) & 0x800) > 0) && ((message.decodeInt16(78) & 0x800) > 0)){finishingMillDataActual.ExitData.ActualData.FlatnessStatus = 1;} 
                else {finishingMillDataActual.ExitData.ActualData.FlatnessStatus = 2;};
            }

            finishingMillDataActual.ExitData.ActualData.ExitSpeed = message.decodeSingle(324);
            finishingMillDataActual.ExitData.ActualData.StripCrown25 = message.decodeSingle(1056);
            finishingMillDataActual.ExitData.ActualData.StripCrown40 = message.decodeSingle(1052);
            finishingMillDataActual.ExitData.ActualData.StripWedge25 = message.decodeSingle(1064);
            finishingMillDataActual.ExitData.ActualData.StripWedge40 = message.decodeSingle(1060);
            finishingMillDataActual.ExitData.ActualData.ExitTemp1 = message.decodeSingle(304);
            finishingMillDataActual.ExitData.ActualData.ExitTemp2 = message.decodeSingle(308);
            finishingMillDataActual.ExitData.ActualData.ExitTemp3 = message.decodeSingle(312);


            // DESCALER STATUS ---------------------------------------------------------------------------------------------------------
            if (message.decodeInt16(150) > 0) { finishingMillDataActual.DescalerData.Status.Run = true; } else { finishingMillDataActual.DescalerData.Status.Run = false; }
            if ((message.decodeInt16(78) & 0x01) > 0 || (message.decodeInt16(78) & 0x08) > 0) { finishingMillDataActual.DescalerData.Status.Mis = true; } else { finishingMillDataActual.DescalerData.Status.Mis = false; }

            // STAND STATUS --------------------------------------------------------------------------------------------------------------

            if (message.decodeSingle(508) > 0.1) { finishingMillDataActual.StandData[0].Status.Run = true; } else { finishingMillDataActual.StandData[0].Status.Run = false; }
            if (message.decodeSingle(512) > 0.1) { finishingMillDataActual.StandData[1].Status.Run = true; } else { finishingMillDataActual.StandData[1].Status.Run = false; }
            if (message.decodeSingle(516) > 0.1) { finishingMillDataActual.StandData[2].Status.Run = true; } else { finishingMillDataActual.StandData[2].Status.Run = false; }
            if (message.decodeSingle(520) > 0.1) { finishingMillDataActual.StandData[3].Status.Run = true; } else { finishingMillDataActual.StandData[3].Status.Run = false; }
            if (message.decodeSingle(524) > 0.1) { finishingMillDataActual.StandData[4].Status.Run = true; } else { finishingMillDataActual.StandData[4].Status.Run = false; }
            if (message.decodeSingle(528) > 0.1) { finishingMillDataActual.StandData[5].Status.Run = true; } else { finishingMillDataActual.StandData[5].Status.Run = false; }


            if ((message.decodeInt16(68) & 0x01) > 0) { finishingMillDataActual.StandData[0].Status.Mis = true; } else { finishingMillDataActual.StandData[0].Status.Mis = false; }
            if ((message.decodeInt16(68) & 0x02) > 0) { finishingMillDataActual.StandData[1].Status.Mis = true; } else { finishingMillDataActual.StandData[1].Status.Mis = false; }
            if ((message.decodeInt16(68) & 0x04) > 0) { finishingMillDataActual.StandData[2].Status.Mis = true; } else { finishingMillDataActual.StandData[2].Status.Mis = false; }
            if ((message.decodeInt16(68) & 0x08) > 0) { finishingMillDataActual.StandData[3].Status.Mis = true; } else { finishingMillDataActual.StandData[3].Status.Mis = false; }
            if ((message.decodeInt16(68) & 0x10) > 0) { finishingMillDataActual.StandData[4].Status.Mis = true; } else { finishingMillDataActual.StandData[4].Status.Mis = false; }
            if ((message.decodeInt16(68) & 0x20) > 0) { finishingMillDataActual.StandData[5].Status.Mis = true; } else { finishingMillDataActual.StandData[5].Status.Mis = false; }

            if ((message.decodeInt16(70) & 0x01) > 0) { finishingMillDataActual.StandData[0].Status.Dis = true; } else { finishingMillDataActual.StandData[0].Status.Dis = false; }
            if ((message.decodeInt16(70) & 0x02) > 0) { finishingMillDataActual.StandData[1].Status.Dis = true; } else { finishingMillDataActual.StandData[1].Status.Dis = false; }
            if ((message.decodeInt16(70) & 0x04) > 0) { finishingMillDataActual.StandData[2].Status.Dis = true; } else { finishingMillDataActual.StandData[2].Status.Dis = false; }
            if ((message.decodeInt16(70) & 0x08) > 0) { finishingMillDataActual.StandData[3].Status.Dis = true; } else { finishingMillDataActual.StandData[3].Status.Dis = false; }
            if ((message.decodeInt16(70) & 0x10) > 0) { finishingMillDataActual.StandData[4].Status.Dis = true; } else { finishingMillDataActual.StandData[4].Status.Dis = false; }
            if ((message.decodeInt16(70) & 0x20) > 0) { finishingMillDataActual.StandData[5].Status.Dis = true; } else { finishingMillDataActual.StandData[5].Status.Dis = false; }


            // --------- Entry Speed
            if((message.decodeInt16(76) & 0x08) == 0){ finishingMillDataActual.EntryData.ActualData.EntrySpeedStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x08) > 0) && ((message.decodeInt16(78) & 0x08) > 0)){finishingMillDataActual.EntryData.ActualData.EntrySpeedStatus = 1;} 
            else {finishingMillDataActual.EntryData.ActualData.EntrySpeedStatus = 2;};

            // --------- Entry Temp
            if((message.decodeInt16(76) & 0x01) == 0){ finishingMillDataActual.EntryData.ActualData.EntryTempStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x01) > 0) && ((message.decodeInt16(78) & 0x01) > 0)){finishingMillDataActual.EntryData.ActualData.EntryTempStatus = 1;} 
            else {finishingMillDataActual.EntryData.ActualData.EntryTempStatus = 2;};

            // --------- Crown
            if((message.decodeInt16(76) & 0x800) == 0){ finishingMillDataActual.ExitData.ActualData.CrownStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x800) > 0) && ((message.decodeInt16(78) & 0x800) > 0)){finishingMillDataActual.ExitData.ActualData.CrownStatus = 1;} 
            else {finishingMillDataActual.ExitData.ActualData.CrownStatus = 2;};

            // --------- Wedge
            if((message.decodeInt16(76) & 0x800) == 0){ finishingMillDataActual.ExitData.ActualData.WedgeStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x800) > 0) && ((message.decodeInt16(78) & 0x800) > 0)){finishingMillDataActual.ExitData.ActualData.WedgeStatus = 1;} 
            else {finishingMillDataActual.ExitData.ActualData.WedgeStatus = 2;};

            // --------- Exit Temp
            if((message.decodeInt16(76) & 0x40) == 0){ finishingMillDataActual.ExitData.ActualData.ExitTempStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x40) > 0) && ((message.decodeInt16(78) & 0x40) > 0)){finishingMillDataActual.ExitData.ActualData.ExitTempStatus = 1;} 
            else {finishingMillDataActual.ExitData.ActualData.ExitTempStatus = 2;};

            // --------- Exit Speed
            if((message.decodeInt16(76) & 0x2000) == 0){ finishingMillDataActual.ExitData.ActualData.ExitSpeedStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x2000) > 0) && ((message.decodeInt16(78) & 0x2000) > 0)){finishingMillDataActual.ExitData.ActualData.ExitSpeedStatus = 1;} 
            else {finishingMillDataActual.ExitData.ActualData.ExitSpeedStatus = 2;};

        }


        protected void processMessageCD(Message message)
        {
            if (message.header.Code != "1108")
            {
                return;
            }
            logger.LogInformation("CoolingDowncoilers received message: " + message.header.Code);
            // rispetto al file excel (LANDEF) sono sfasati di -2 (ad esempio LANDEF 154 => messaggio 152)
            logger.LogInformation("PieceNo: " + message.decodeInt32(12));
            logger.LogInformation("DOWNCOILER DATA --- DC 1 ------------------------");
            logger.LogInformation("Piece id: " + message.decodeString(582, 20).Trim());
            logger.LogInformation("Internal Diameter: " + message.decodeSingle(602));
            logger.LogInformation("External Diameter: " + message.decodeSingle(606));
            logger.LogInformation("SpecTension: " + message.decodeSingle(616));
            logger.LogInformation("Strip Speed: " + message.decodeSingle(620));

            logger.LogInformation("DOWNCOILER DATA --- DC 2 ------------------------");
            logger.LogInformation("Piece id: " + message.decodeString(644, 20).Trim());
            logger.LogInformation("Internal Diameter: " + message.decodeSingle(664));
            logger.LogInformation("External Diameter: " + message.decodeSingle(668));
            logger.LogInformation("SpecTension: " + message.decodeSingle(678));
            logger.LogInformation("Strip Speed: " + message.decodeSingle(682));

            logger.LogInformation("ENTRY PYROMETER ------------------------");
            logger.LogInformation("Entry Temperature 1: " + message.decodeSingle(198));
            logger.LogInformation("Entry Temperature 2: " + message.decodeSingle(210));
            logger.LogInformation("Entry Temperature 3: " + message.decodeSingle(222));

            logger.LogInformation("INTERMEDIATE PYROMETER ------------------------");
            logger.LogInformation("Int. Temperature 1: " + message.decodeSingle(238));
            logger.LogInformation("Int. Temperature 2: " + message.decodeSingle(250));
            logger.LogInformation("Int. Temperature 3: " + message.decodeSingle(262));

            logger.LogInformation("EXIT PYROMETER ------------------------");
            logger.LogInformation("Exit Temperature LR 1: " + message.decodeSingle(278));
            logger.LogInformation("Exit Temperature LR 2: " + message.decodeSingle(290));
            logger.LogInformation("Exit Temperature LR 3: " + message.decodeSingle(302));
            logger.LogInformation("Exit Temperature HR 1: " + message.decodeSingle(318));
            logger.LogInformation("Exit Temperature HR 2: " + message.decodeSingle(330));
            logger.LogInformation("Exit Temperature HR 3: " + message.decodeSingle(342));


            logger.LogInformation("STATUS --------------------------");

            logger.LogInformation("TOP_HEAD_FDBK_INTENSIVE_1: " + message.decodeInt16(392));
            logger.LogInformation("TOP_HEAD_FDBK_INTENSIVE_2: " + message.decodeInt16(394));
            if (((message.decodeInt16(392) & 0x0f) > 0) || ((message.decodeInt16(394) & 0x0f) > 0)) { coolingDowncoilersDataDto.IntensiveData[0].TopStatus.Run = true; } else { coolingDowncoilersDataDto.IntensiveData[0].TopStatus.Run = false; }
            if (((message.decodeInt16(392) >> 4 & 0x0f) > 0) || ((message.decodeInt16(394) >> 4 & 0x0f) > 0)) { coolingDowncoilersDataDto.IntensiveData[1].TopStatus.Run = true; } else { coolingDowncoilersDataDto.IntensiveData[1].TopStatus.Run = false; }
            if (((message.decodeInt16(392) >> 8 & 0x0f) > 0) || ((message.decodeInt16(394) >> 8 & 0x0f) > 0)) { coolingDowncoilersDataDto.IntensiveData[2].TopStatus.Run = true; } else { coolingDowncoilersDataDto.IntensiveData[2].TopStatus.Run = false; }

            logger.LogInformation("BOT_HEAD_FDBK_INTENSIVE_1: " + message.decodeInt16(396));
            logger.LogInformation("BOT_HEAD_FDBK_INTENSIVE_2: " + message.decodeInt16(398));
            logger.LogInformation("BOT_HEAD_FDBK_INTENSIVE_3: " + message.decodeInt16(400));
            logger.LogInformation("BOT_HEAD_FDBK_INTENSIVE_4: " + message.decodeInt16(402));
            if (((message.decodeInt16(396) & 0x0f) > 0) ||
                ((message.decodeInt16(398) & 0x0f) > 0) ||
                ((message.decodeInt16(400) & 0x0f) > 0) ||
                ((message.decodeInt16(402) & 0x0f) > 0))
            {
                coolingDowncoilersDataDto.IntensiveData[0].BottomStatus.Run = true;
            }
            else { coolingDowncoilersDataDto.IntensiveData[0].BottomStatus.Run = false; }

            if (((message.decodeInt16(396) >> 4 & 0x0f) > 0) ||
                ((message.decodeInt16(398) >> 4 & 0x0f) > 0) ||
                ((message.decodeInt16(400) >> 4 & 0x0f) > 0) ||
                ((message.decodeInt16(402) >> 4 & 0x0f) > 0))
            {
                coolingDowncoilersDataDto.IntensiveData[1].BottomStatus.Run = true;
            }
            else { coolingDowncoilersDataDto.IntensiveData[1].BottomStatus.Run = false; }

            if (((message.decodeInt16(396) >> 8 & 0x0f) > 0) ||
                ((message.decodeInt16(398) >> 8 & 0x0f) > 0) ||
                ((message.decodeInt16(400) >> 8 & 0x0f) > 0) ||
                ((message.decodeInt16(402) >> 8 & 0x0f) > 0))
            {
                coolingDowncoilersDataDto.IntensiveData[2].BottomStatus.Run = true;
            }
            else { coolingDowncoilersDataDto.IntensiveData[2].BottomStatus.Run = false; }

            if ((message.decodeInt16(194) & 0x01) > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    coolingDowncoilersDataDto.IntensiveData[i].TopStatus.Mis = true;
                    coolingDowncoilersDataDto.IntensiveData[i].BottomStatus.Mis = true;
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    coolingDowncoilersDataDto.IntensiveData[i].TopStatus.Mis = false;
                    coolingDowncoilersDataDto.IntensiveData[i].BottomStatus.Mis = false;
                }
            }



            logger.LogInformation("TOP_HEAD_FDBK_NORMAL: " + message.decodeInt32(404));
            if (((message.decodeInt32(404) & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[0].TopStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[0].TopStatus.Run = false; }
            if (((message.decodeInt32(404) >> 4 & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[1].TopStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[1].TopStatus.Run = false; }
            if (((message.decodeInt32(404) >> 8 & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[2].TopStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[2].TopStatus.Run = false; }
            if (((message.decodeInt32(404) >> 12 & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[3].TopStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[3].TopStatus.Run = false; }
            if (((message.decodeInt32(404) >> 16 & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[4].TopStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[4].TopStatus.Run = false; }
            if (((message.decodeInt32(404) >> 20 & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[5].TopStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[5].TopStatus.Run = false; }

            logger.LogInformation("BOT_HEAD_FDBK_NORMAL: " + message.decodeInt32(408));
            if (((message.decodeInt32(408) & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[0].BottomStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[0].BottomStatus.Run = false; }
            if (((message.decodeInt32(408) >> 4 & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[1].BottomStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[1].BottomStatus.Run = false; }
            if (((message.decodeInt32(408) >> 8 & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[2].BottomStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[2].BottomStatus.Run = false; }
            if (((message.decodeInt32(408) >> 12 & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[3].BottomStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[3].BottomStatus.Run = false; }
            if (((message.decodeInt32(408) >> 16 & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[4].BottomStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[4].BottomStatus.Run = false; }
            if (((message.decodeInt32(408) >> 20 & 0x0f) > 0)) { coolingDowncoilersDataDto.NormalData[5].BottomStatus.Run = true; } else { coolingDowncoilersDataDto.NormalData[5].BottomStatus.Run = false; }

            if (((message.decodeInt16(194) & 0x02) > 0))
            {
                for (int i = 0; i < 6; i++)
                {
                    coolingDowncoilersDataDto.NormalData[i].TopStatus.Mis = true;
                    coolingDowncoilersDataDto.NormalData[i].BottomStatus.Mis = true;
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    coolingDowncoilersDataDto.NormalData[i].TopStatus.Mis = false;
                    coolingDowncoilersDataDto.NormalData[i].BottomStatus.Mis = false;
                }
            }



            logger.LogInformation("TOP_HEAD_FDBK_TRIMMING: " + message.decodeInt32(412));
            if (((message.decodeInt32(410) & 0xffff) > 0)) { coolingDowncoilersDataDto.TrimmingData[0].TopStatus.Run = true; } else { coolingDowncoilersDataDto.TrimmingData[0].TopStatus.Run = false; }
            if (((message.decodeInt32(410) >> 16 & 0xffff) > 0)) { coolingDowncoilersDataDto.TrimmingData[1].TopStatus.Run = true; } else { coolingDowncoilersDataDto.TrimmingData[1].TopStatus.Run = false; }

            logger.LogInformation("BOT_HEAD_FDBK_TRIMMING: " + message.decodeInt32(416));
            if (((message.decodeInt32(414) & 0xffff) > 0)) { coolingDowncoilersDataDto.TrimmingData[0].BottomStatus.Run = true; } else { coolingDowncoilersDataDto.TrimmingData[0].BottomStatus.Run = false; }
            if (((message.decodeInt32(414) >> 16 & 0xffff) > 0)) { coolingDowncoilersDataDto.TrimmingData[1].BottomStatus.Run = true; } else { coolingDowncoilersDataDto.TrimmingData[1].BottomStatus.Run = false; }

            if (((message.decodeInt16(194) & 0x02) > 0) || ((message.decodeInt16(194) & 0x04) > 0))
            {
                for (int i = 0; i < 2; i++)
                {
                    coolingDowncoilersDataDto.TrimmingData[i].TopStatus.Mis = true;
                    coolingDowncoilersDataDto.TrimmingData[i].BottomStatus.Mis = true;
                }
                coolingDowncoilersDataDto.RollssmallData.Status.Mis = true;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    coolingDowncoilersDataDto.TrimmingData[i].TopStatus.Mis = false;
                    coolingDowncoilersDataDto.TrimmingData[i].BottomStatus.Mis = false;
                }
                coolingDowncoilersDataDto.RollssmallData.Status.Mis = false;
            }


            logger.LogInformation("MOTOR_SPEED_1: " + message.decodeSingle(624));
            logger.LogInformation("MOTOR_SPEED_2: " + message.decodeSingle(686));
            logger.LogInformation("SP_TENSION_1: " + message.decodeSingle(616));
            logger.LogInformation("SP_TENSION_2: " + message.decodeSingle(678));
            logger.LogInformation("ACTIVE_DC: " + message.decodeInt16(576));
            if ((message.decodeSingle(624)) > 1) { coolingDowncoilersDataDto.DowncoilerData[0].Status.Run = true; } else { coolingDowncoilersDataDto.DowncoilerData[0].Status.Run = false; }
            if ((message.decodeSingle(686)) > 1) { coolingDowncoilersDataDto.DowncoilerData[1].Status.Run = true; } else { coolingDowncoilersDataDto.DowncoilerData[1].Status.Run = false; }
            if ((message.decodeSingle(616)) > 1) { coolingDowncoilersDataDto.DowncoilerData[0].Status.Mis = true; } else { coolingDowncoilersDataDto.DowncoilerData[0].Status.Mis = false; }
            if ((message.decodeSingle(678)) > 1) { coolingDowncoilersDataDto.DowncoilerData[1].Status.Mis = true; } else { coolingDowncoilersDataDto.DowncoilerData[1].Status.Mis = false; }
            if ((message.decodeInt16(576)) == 2)
            {
                coolingDowncoilersDataDto.DowncoilerData[0].ActiveDC = true;
                coolingDowncoilersDataDto.DowncoilerData[1].ActiveDC = true;
            }
            else
            {
                coolingDowncoilersDataDto.DowncoilerData[0].ActiveDC = false;
                coolingDowncoilersDataDto.DowncoilerData[1].ActiveDC = false;
            }

            if (message.decodeInt32(12) > 0)
            {
                coolingDowncoilersDataDto.PieceNo = message.decodeInt32(12);
            }
            coolingDowncoilersDataDto.DowncoilerData[0].ActualData.PieceId = message.decodeString(582, 20).Trim();
            coolingDowncoilersDataDto.DowncoilerData[0].ActualData.InternalDiameter = message.decodeSingle(602);
            coolingDowncoilersDataDto.DowncoilerData[0].ActualData.ExternalDiameter = message.decodeSingle(606);
            coolingDowncoilersDataDto.DowncoilerData[0].ActualData.SpecificTensions = message.decodeSingle(616);
            coolingDowncoilersDataDto.DowncoilerData[0].ActualData.StripSpeed = message.decodeSingle(620);

            coolingDowncoilersDataDto.DowncoilerData[1].ActualData.PieceId = message.decodeString(644, 20).Trim();
            coolingDowncoilersDataDto.DowncoilerData[1].ActualData.InternalDiameter = message.decodeSingle(664);
            coolingDowncoilersDataDto.DowncoilerData[1].ActualData.ExternalDiameter = message.decodeSingle(668);
            coolingDowncoilersDataDto.DowncoilerData[1].ActualData.SpecificTensions = message.decodeSingle(678);
            coolingDowncoilersDataDto.DowncoilerData[1].ActualData.StripSpeed = message.decodeSingle(682);

            coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp1 = message.decodeSingle(198);
            coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp2 = message.decodeSingle(210);
            coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp3 = message.decodeSingle(222);

            coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp1 = message.decodeSingle(238);
            coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp2 = message.decodeSingle(250);
            coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp3 = message.decodeSingle(262);

            coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp1LR = message.decodeSingle(278);
            coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp2LR = message.decodeSingle(290);
            coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp3LR = message.decodeSingle(302);
            coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp1HR = message.decodeSingle(318);
            coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp2HR = message.decodeSingle(330);
            coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp3HR = message.decodeSingle(342);

            coolingDowncoilersDataDto.NormalData[0].ActualData.StepNo = 4;
            coolingDowncoilersDataDto.NormalData[1].ActualData.StepNo = 5;
            coolingDowncoilersDataDto.NormalData[2].ActualData.StepNo = 6;
            coolingDowncoilersDataDto.NormalData[3].ActualData.StepNo = 7;
            coolingDowncoilersDataDto.NormalData[4].ActualData.StepNo = 8;
            coolingDowncoilersDataDto.NormalData[5].ActualData.StepNo = 9;

            coolingDowncoilersDataDto.TrimmingData[0].ActualData.StepNo = 10;
            coolingDowncoilersDataDto.TrimmingData[1].ActualData.StepNo = 11;

            // --------- Entry Temp 1
            if((message.decodeInt16(76) & 0x01) == 0){ coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp1Status = 0; } 
            else if (((message.decodeInt16(76) & 0x01) > 0) && ((message.decodeInt16(78) & 0x01) > 0)){coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp1Status = 1;} 
            else {coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp1Status = 2;};

            // --------- Entry Temp 2
            if((message.decodeInt16(76) & 0x02) == 0){ coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp2Status = 0; } 
            else if (((message.decodeInt16(76) & 0x02) > 0) && ((message.decodeInt16(78) & 0x01) > 0)){coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp2Status = 1;} 
            else {coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp2Status = 2;};

            // --------- Entry Temp 3
            if((message.decodeInt16(76) & 0x04) == 0){ coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp3Status = 0; } 
            else if (((message.decodeInt16(76) & 0x04) > 0) && ((message.decodeInt16(78) & 0x01) > 0)){coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp3Status = 1;} 
            else {coolingDowncoilersDataDto.EntryPyrometerData.ActualData.EntryTemp3Status = 2;};

            // --------- Int Temp 1
            if((message.decodeInt16(76) & 0x08) == 0){ coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp1Status = 0; } 
            else if (((message.decodeInt16(76) & 0x08) > 0) && ((message.decodeInt16(78) & 0x02) > 0)){coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp1Status = 1;} 
            else {coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp1Status = 2;};

            // --------- Int Temp 2
            if((message.decodeInt16(76) & 0x10) == 0){ coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp2Status = 0; } 
            else if (((message.decodeInt16(76) & 0x10) > 0) && ((message.decodeInt16(78) & 0x02) > 0)){coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp2Status = 1;} 
            else {coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp2Status = 2;};

            // --------- Int Temp 3
            if((message.decodeInt16(76) & 0x20) == 0){ coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp3Status = 0; } 
            else if (((message.decodeInt16(76) & 0x20) > 0) && ((message.decodeInt16(78) & 0x02) > 0)){coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp3Status = 1;} 
            else {coolingDowncoilersDataDto.IntPyrometerData.ActualData.IntTemp3Status = 2;};

            // --------- Exit Temp 1 LR
            if((message.decodeInt16(76) & 0x40) == 0){ coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp1LRStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x40) > 0) && ((message.decodeInt16(78) & 0x04) > 0)){coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp1LRStatus = 1;} 
            else {coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp1LRStatus = 2;};

            // --------- Exit Temp 2 LR
            if((message.decodeInt16(76) & 0x80) == 0){ coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp2LRStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x80) > 0) && ((message.decodeInt16(78) & 0x04) > 0)){coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp2LRStatus = 1;} 
            else {coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp2LRStatus = 2;};

            // --------- Exit Temp 3 LR
            if((message.decodeInt16(76) & 0x100) == 0){ coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp3LRStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x100) > 0) && ((message.decodeInt16(78) & 0x04) > 0)){coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp3LRStatus = 1;} 
            else {coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp3LRStatus = 2;};

            // --------- Exit Temp 1 HR
            if((message.decodeInt16(76) & 0x200) == 0){ coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp1HRStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x200) > 0) && ((message.decodeInt16(78) & 0x08) > 0)){coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp1HRStatus = 1;} 
            else {coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp1HRStatus = 2;};

            // --------- Exit Temp 2 HR
            if((message.decodeInt16(76) & 0x400) == 0){ coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp2HRStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x400) > 0) && ((message.decodeInt16(78) & 0x08) > 0)){coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp2HRStatus = 1;} 
            else {coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp2HRStatus = 2;};

            // --------- Exit Temp 3 HR
            if((message.decodeInt16(76) & 0x800) == 0){ coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp3HRStatus = 0; } 
            else if (((message.decodeInt16(76) & 0x800) > 0) && ((message.decodeInt16(78) & 0x08) > 0)){coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp3HRStatus = 1;} 
            else {coolingDowncoilersDataDto.ExitPyrometerData.ActualData.ExitTemp3HRStatus = 2;};


        }

    }
}
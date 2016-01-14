// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Runtime.Serialization;

namespace mqttconsole.Data
{
    [DataContract]
    public class TemperatureData
    {
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public double Temperature { get; set; }
        [DataMember]
        public double Humidity { get; set; }
        [DataMember]
        public double? ExternalTemperature { get; set; }
    }

    [DataContract]
    class DeviceProperties
    {
        [DataMember]
        public string DeviceID { get; set; }
        [DataMember]
        public bool HubEnabledState { get; set; }
        [DataMember]
        public DateTime CreatedTime { get; set; }
        [DataMember]
        public string DeviceState { get; set; }
        [DataMember]
        public object UpdateTime { get; set; }
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public string ModelNumber { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }
        [DataMember]
        public string FirmwareVersion { get; set; }
        [DataMember]
        public string Platform { get; set; }
        [DataMember]
        public string Process { get; set; }
        [DataMember]
        public string InstalledRAM { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
    }

    [DataContract]
    class DeviceCommand
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Object Parameters { get; set; }
    }

    [DataContract]
    class DeviceCommandHistoryEntry { }

    [DataContract]
    class TemperatureInitData
    {
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public DeviceProperties DeviceProperties { get; set; }
        [DataMember]
        public DeviceCommand[] Commands { get; set; }
        [DataMember]
        public DeviceCommandHistoryEntry[] CommandHistory { get; set; }
        [DataMember]
        public bool IsSimulatedDevice { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string ObjectType { get; set; }
    }
}

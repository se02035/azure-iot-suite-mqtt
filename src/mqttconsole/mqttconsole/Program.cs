// Licensed under the MIT license. See LICENSE file in the project root for full license information. 


using Microsoft.Azure.Devices.Common.Security;
using mqttconsole.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace mqttconsole
{
    class Program
    {
        private static string TopicDevice2Service;
        private static MqttClient client;

        static void Main(string[] args)
        {
            // Azure IoT Hub
            const string IoTHubName = "[Azure IoT Suite IoT Hub URI]"; // azure iot suite

            // Azure IoT gateway (MQTT broker)
            const string GatewayHost = "[IP Address Azure Iot Protocol Gateway]";
            const int GatewayMqttPort = 8883;

            // device/client information
            const string DeviceId = "[DeviceId in Azure IoT Suite]";
            const string DeviceKey = "[DeviceKey in Azure IoT Suite]"; // azure iot suite

            string Username = string.Format("{0}/{1}", IoTHubName, DeviceId);
            string Password = CreateSharedAccessSignature(DeviceId, DeviceKey, IoTHubName);

            // define topics
            TopicDevice2Service = string.Format("devices/{0}/messages/events", DeviceId);

            try
            {
                // ignore any TLS errors
                client = new MqttClient(GatewayHost, GatewayMqttPort, true, MqttSslProtocols.TLSv1_0, (sender, certificate, chain, errors) => true, null);
                client.Connect(DeviceId, Username, Password);

                // initialize the new physical device on Azure IoT Suite
                InitializeDevice(DeviceId);

                // send fake telemtry data
                var rand = new Random();
                Console.Write("Press ENTER to start sending telemetry data"); Console.ReadKey();
                while (true)
                {
                    SendTelemetryData(DeviceId, rand.Next(10, 60), rand.Next(10, 60));
                    System.Threading.Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        static string CreateSharedAccessSignature(string deviceId, string deviceKey, string iotHubName)
        {
            return new SharedAccessSignatureBuilder
            {
                Key = deviceKey,
                Target = string.Format("{0}/devices/{1}", iotHubName, deviceId),
                KeyName = null,
                TimeToLive = TimeSpan.FromMinutes(20)
            }.ToSignature();
        }

        private static void SendTelemetryData(string deviceId, double temp, double humidity)
        {
            Console.WriteLine("{0}: Sending data. Temp '{1}' / Hum '{2}'", DateTime.Now.ToLongTimeString(), temp, humidity);
            var data = new TemperatureData()
            {
                DeviceId = deviceId,
                Temperature = temp,
                Humidity = humidity
            };

            client.Publish(TopicDevice2Service, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
        }

        private static void InitializeDevice(string deviceId)
        {
            Console.Write("Press ENTER to start device registration"); Console.ReadKey();
            Console.WriteLine("{0}: Sending device '{1}' registration data", DateTime.Now.ToLongTimeString(), deviceId);

            var data = new TemperatureInitData()
            {
                DeviceId = deviceId,
                DeviceProperties = new DeviceProperties()
                {
                    CreatedTime = DateTime.Now.ToUniversalTime(),
                    DeviceID = deviceId,
                    DeviceState = "normal",
                    FirmwareVersion = "1.8",
                    HubEnabledState = true,
                    InstalledRAM = "1024 MB",
                    Latitude = 48.27071325,
                    Longitude = 14.58153975,
                    Manufacturer = "DEMO MQTT",
                    ModelNumber = "v2",
                    Platform = "MyPlattform",
                    Process = "x86",
                    SerialNumber = "xxxxx",
                },
                CommandHistory = new List<DeviceCommandHistoryEntry>().ToArray(),
                Commands = new List<DeviceCommand>().ToArray(),
                IsSimulatedDevice = false,
                ObjectType = "DeviceInfo",
                Version = "1.0"
            };

            client.Publish(TopicDevice2Service, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
        }
    }
}

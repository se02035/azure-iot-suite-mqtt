# Connect to Azure IoT Suite over MQTT using the Microsoft Azure Protocol Gateway #

This C# demo project shows how to integrate with the Azure IoT Suite (Remote Monitoring) using MQTT. Since MQTT isn't natively supported by Azure IoT Hub we have to use a MQTT broker here. 

However, Microsoft Azure IoT Protocol Gateway can act as the required broker. The good part is that Microsoft already provides a sample implementation (for MQTT) ob Github. This MQTT broker implementation will be used in this sample and hosted in an Azure Cloud Service (allowing us to easily build a highly available and easily scalable solution).

## Prerequisites ##
- Microsoft Azure Subscription ([https://azure.microsoft.com/en-us/?b=16.01](https://azure.microsoft.com/en-us/?b=16.01 "Microsoft Azure"))
- Microsoft .NET 4.5.2
- Microsoft Visual Studio 2015 ([https://www.visualstudio.com/en-us/visual-studio-homepage-vs.aspx](https://www.visualstudio.com/en-us/visual-studio-homepage-vs.aspx "Visual Studio"))

## Setup ##

### Azure IoT Suite - Remote Monitoring ###
The Azure IoT Suite provides preconfigured soltuions which consists of commonly used Azure services (e.g. Azure IoT Hub, Stream Analytics jobs, Azure SQL, etc). In order to use it a Microsoft Azure subscription is required (this is the target where all the Azure resources will be deployed to).

Start creating a new Azure IoT Suite Remote Monitoring solution from here: [https://www.azureiotsuite.com/](https://www.azureiotsuite.com/ "Microsoft Azure IoT Suite")

> It is important to create the Azure IoT Suite solution before deploying the Azure IoT Protocol Gateway as the protocol gateway needs to know some internals about the Azure IoT Hub it should talk to. 

### Azure IoT Protocol Gateway ###
The Azure IoT protocol gateway is a framework that can be used to enable communication with Azure IoT and connected IoT devices over other protocols like HTTP(S) and AMQP (which are currently integrated into the Azure IoT). However, MQTT is quite popular and, therefore, the current Microsoft Azure IoT Protocol Gateway implementation ([https://github.com/Azure/azure-iot-protocol-gateway](https://github.com/Azure/azure-iot-protocol-gateway "Microsoft Azure IoT Protocol Gateway")) provides an implementation that supports communication over the MQTT protocol.

The Azure IoT protocol gateway comes with a Powershell script (`deploy.ps1`) that deploys the gateway to an Azure Cloud service (by default, if not specified as a parameter, it will create 2 instances. Here are the important parameters:

    .\deploy.ps1 "[CLOUD SERVICE NAME]" "[STORAGE ACCOUNT NAME]" "[AZURE LOCATION e.g. East US]" "[TLS CERTIFICATE PATH e.g. mycert.pfx]" "[TLS CERT PASSWORD]" "[AZRUE IOT CONNECTION STRING]" -SubscriptionName "[NAME OF TARGET AZURE SUBSCRIPTION]"

> The SubscriptionName parameter is optional.

Additional information on how to run, test, package and deploy the Azure IoT Protocol Gateway can be found here: [https://github.com/Azure/azure-iot-protocol-gateway](https://github.com/Azure/azure-iot-protocol-gateway "Microsoft Azure IoT Protocol Gateway")

## Implementation ##

Please have a look at the demo .NET implementation in this repository.

### Using a MQTT Client (M2Mqtt) ###
There are many different MQTT client implementations out there. Auch Microsoft arbeitet an IoT client SDKs (unterschiedliche Technologien) welche MQTT unterstützen ([https://github.com/Azure/azure-iot-sdks](https://github.com/Azure/azure-iot-sdks "Github Azure IoT SDKs")). 

For this demo I used the M2Mqtt client implementation (available as a NuGet package [https://www.nuget.org/packages/M2Mqtt/](https://www.nuget.org/packages/M2Mqtt/ "M2Mqtt NuGet Package")).

### Connect devices to the Azure IoT Suite ###
There is a pretty good blog article available on how to connect devices to the IoT Suite Remote Monitoring solution ([https://azure.microsoft.com/en-gb/documentation/articles/iot-suite-connecting-devices/](https://azure.microsoft.com/en-gb/documentation/articles/iot-suite-connecting-devices/ "Connect your device to the IoT Suite remote monitoring preconfigured solution")). You'll notice that the instructions in the blog target C and Node.js. So, there is an implementation available that can be used on your devices to send telemetry data to the remote monitoring solution

Although a .NET implementation isn't mentioned on this blog article you'll find a .NET implementation in the implementation of the IoT Suite Remote Monitoring solution itself. The solution is also on GitHub ([https://github.com/Azure/azure-iot-remote-monitoring/tree/634637230e134331ed2b4a2c0551311c7159ccc5](https://github.com/Azure/azure-iot-remote-monitoring/tree/634637230e134331ed2b4a2c0551311c7159ccc5 "Azure IoT Remote Monitoring")) - have a look at the simluator implementation!

Unfortunately, the data contract of valid telemetry data (when sending events to the Azure IoT Hub) isn't explicitely published. But it can be retrieved from the device implementations mentioned above. There are two types of events:

1. Device Information (initialization)
2. Telemtry Data (temperature, humidity)

#### Device Information ####

```c#
    public class DeviceInformationData
    {
        public string DeviceId { get; set; }
        public DevicePropertiesData DeviceProperties { get; set; }
        public DeviceCommandData[] Commands { get; set; }
        public DeviceCommandHistoryEntryData[] CommandHistory { get; set; }
        public bool IsSimulatedDevice { get; set; }
        public string Version { get; set; }
        public string ObjectType { get; set; }
    }

    public class DevicePropertiesData
    {
        public string DeviceID { get; set; }
        public bool HubEnabledState { get; set; }
        public DateTime CreatedTime { get; set; }
        public string DeviceState { get; set; }
        public object UpdateTime { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public string FirmwareVersion { get; set; }
        public string Platform { get; set; }
        public string Process { get; set; }
        public string InstalledRAM { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class DeviceCommandData
    {
        public string Name { get; set; }
        public object Parameters { get; set; }
    }

    public class DeviceCommandHistoryEntryData { }
````

#### Telemetry Data ####

```C#
	public class TelemetryData
	{
	    public string DeviceId { get; set; }
	    public double Temperature { get; set; }
	    public double Humidity { get; set; }
	    public double? ExternalTemperature { get; set; }
	}
````

## Debugging ##
A very handy tool when working with the Azure IoT Hub is the Microsoft Azure IoT Hub Device Explorer (https://github.com/Azure/azure-iot-sdks/blob/master/tools/DeviceExplorer/doc/how_to_use_device_explorer.md). Besides providing functionality to work with the device registration, it can also be used to monitor device2service communication (very helpful if you need to know what if and what data is received by the Azure IoT Hub).


## Additional resources ##
- Microsoft Azure IoT Suite ([https://azure.microsoft.com/en-us/solutions/iot-suite/](https://azure.microsoft.com/en-us/solutions/iot-suite/ "Microsoft Azure IoT Suite"))
- Microsoft Azure IoT Protocol Gateway ([https://github.com/Azure/azure-iot-protocol-gateway]( https://github.com/Azure/azure-iot-protocol-gateway "Microsoft Azure IoT Protocol Gateway"))
- M2Mqtt NuGet Package ([https://www.nuget.org/packages/M2Mqtt/](https://www.nuget.org/packages/M2Mqtt/ "M2Mqtt NuGet Package"))
- Microsoft Azure IoT Hub Device Explorer [https://github.com/Azure/azure-iot-sdks/blob/master/tools/DeviceExplorer/doc/how_to_use_device_explorer.md](https://github.com/Azure/azure-iot-sdks/blob/master/tools/DeviceExplorer/doc/how_to_use_device_explorer.md "Azure Device Explorer")
- Microsoft Azure ([https://azure.microsoft.com/en-us/?b=16.01](https://azure.microsoft.com/en-us/?b=16.01 "Microsoft Azure"))
- Microsoft Visual Studio ([https://www.visualstudio.com/en-us/visual-studio-homepage-vs.aspx](https://www.visualstudio.com/en-us/visual-studio-homepage-vs.aspx "Microsoft Visual Studio"))
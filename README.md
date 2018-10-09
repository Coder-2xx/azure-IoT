# Azure IoT

This repository demonstarte web application to manage simulated devices connected to Azure IoT Hub. Simulated devices devices send device-to-cloud (D2C) telemetry and upload files. Web application can send messages to simulated using cloud-to-device messaging endpoint.

SignalR is used to display live feeds from simulated devices. For more details refer architecture diagram.

Solution depends on:
 * Azure IoT Hub Dot Net SDKs
 * Azure IoT Hub used to register devices, to send telemetry and receive method calls
 * Azure File Storage to store files received from simulated devices
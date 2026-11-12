# SimulateRoomHardware

**A C# console application that simulates electricity usage and publishes room monitoring data through MQTT.**

## Overview

SimulateRoomHardware is a student IoT simulation project developed using C# and .NET. It generates simulated electricity consumption data for multiple rental rooms and publishes the measurements to a public MQTT broker.

The application acts as a **virtual IoT device**, allowing MQTT communication and data processing to be explored without requiring physical sensors or ESP32 hardware.

## Features

- Simulates electricity usage for three rooms: `101`, `102`, and `103`.
- Generates power consumption values based on the current system time.
- Simulates air conditioner status for each room.
- Serializes room data into JSON messages.
- Publishes data to room-specific MQTT topics.
- Repeats the simulation approximately every second.

## Technologies

| Technology | Purpose |
|---|---|
| C# / .NET | Console application and simulation logic |
| MQTTnet | MQTT client communication |
| EMQX Public Broker | MQTT message delivery |
| JSON | Structured monitoring data |

## System Architecture

```text
SimulateRoomHardware (C#)
          |
          | JSON messages over MQTT
          v
  broker.emqx.io:1883
          |
          v
 MQTT Subscribers
 (monitoring applications)
```

This repository implements the **data simulation and publishing component**. MQTT receivers and mobile monitoring applications are separate components and are not included in this repository.

## Simulated Data

The application generates different electricity usage patterns based on the current local system time.

| Period | Time | Power Consumption | AC On Probability |
|---|---|---|---|
| Daytime | 06:00–17:59 | 50–250 W | 15% |
| Nighttime | 18:00–05:59 | 800–2000 W | 80% |

These values are generated for demonstration purposes and do not represent measurements from physical electricity sensors.

## MQTT Configuration

**Broker:** `broker.emqx.io`

**Port:** `1883` (unencrypted TCP)

**Topics:**

```text
pms/rooms/101
pms/rooms/102
pms/rooms/103
```

Each MQTT message contains the following JSON fields:

```json
{
  "RoomNumber": "101",
  "TotalPower": 1250.5,
  "IsAcOn": true,
  "Timestamp": "2026-11-12 20:30:00"
}
```

The example above is illustrative. Actual values are generated dynamically.

## Getting Started

### Prerequisites

- .NET SDK compatible with the project
- Internet connection to access the MQTT broker

### 1. Clone the repository

```bash
git clone https://github.com/Libraa27/SimulateRoomHardware.git
cd SimulateRoomHardware
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Run the application

```bash
dotnet run
```

The application connects to the public MQTT broker and continuously publishes simulated data for the three rooms.

Stop the application with `Ctrl+C`.

## How It Works

1. The application connects to the EMQX public MQTT broker.
2. It reads the current local system time.
3. It generates simulated electricity consumption and air conditioner status for each room.
4. It converts the generated data into JSON.
5. It publishes the JSON payload to the corresponding MQTT topic.
6. It waits approximately one second before repeating the process.

## Limitations

- The application uses simulated data rather than physical sensors.
- MQTT communication uses an unencrypted public broker connection.
- The simulation does not store historical electricity consumption.
- The current implementation does not include a graphical interface.
- The program runs continuously until manually stopped.

## Project Context

This repository was developed as part of a student project exploring IoT communication, electricity monitoring, and MQTT-based data exchange.

It focuses on the software simulation component of a larger monitoring system.

## Author

**Truong Hoang Anh**

GitHub: [@Libraa27](https://github.com/Libraa27)

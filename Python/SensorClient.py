from email import message
import os.path
import time
from ServerRaumHTTPHandler import ServerRaumHTTPHandler
import paho.mqtt.client as mqtt

class SensorClient:

    hostName = "localhost"
    serverPort = 1111

    def __init__(self):
        try:
            f = open("config.txt")
            room = f.readline().rstrip()  # raumnummer lesen  , strip to avoid newline
            self.room = room                        # raumnummer merken
            self.brokerIP = f.readline().rstrip()       # MQTT broker ip, strip to avoid newline
            self.interval = int(f.readline().rstrip())  # Zeitintervall für Datenabfrage, strip to avoid newline
            f.close()

        except:
            print("Config file missing, client will shut down.")
            return;

        # Falls eine limits.txt existiert -> Werte aus Datei ziehen
        self.templimit = 0
        self.humiditylimit = 0
        try:
            f = open("limits.txt")
            self.templimit = int(f.readline().rstrip())         # Limit für Temperatur
            self.humiditylimit = int(f.readline().rstrip())     # limit für Feuchtigkeit
            f.close()

        except:
            # Limits - Default Werte
            self.templimit = 40.0
            self.humiditylimit = 85.0

        self.clientID = "SensorClientRoom" + str(self.room)
        self.client = mqtt.Client(self.clientID)  # create new instance
        self.client.connect(self.brokerIP)  # connect to broker

        self.temp = 20
        self.humidity = 80

        print("Sensor Client is running:")
        print("Room: " + self.room)
        print("Time interval for data push: " + str(self.interval))
        print("Broker IP: " + self.brokerIP);
        print("Temp - limit: " + str(self.templimit))
        print("Humid - limit: " + str(self.humiditylimit))

        ServerRaumHTTPHandler.run(SensorClient.hostName, SensorClient.serverPort)
        print("ServerRaumHTTPHandler is running")

        self.run()

    def readDataFromSensor(self):
        self.temp += 1
        self.humidity += 1
        pass

    def statusInfo(self):
        #dummy
        return str(self.room) + ":" + str(self.temp) + ":" + str(self.humidity) + ":" + \
               str(self.templimit) + ":" + str(self.humiditylimit)

    def run(self):
        while(True):
            time.sleep(int(self.interval))
            message = self.statusInfo()
            self.temp = self.temp + 0.5;
            self.humidity = self.humidity + 0.5;
            self.client.publish("sensorclient/" + self.room + "/data", message)
            self.readDataFromSensor()
            if self.temp > self.templimit or self.humidity > self.humiditylimit:
                self.client.publish("sensorclient/" + self.room + "/alarm", "Alarm: " + self.room)
                self.alarm = True

    def messageReceived(self):
        pass

    def saveLimits(self):
        file = open("limits.txt", "w")
        file.write(str(self.templimit) + "\n")
        file.write(str(self.humiditylimit)+ "\n")
        file.close()
        print("New limits saved")


c = SensorClient()

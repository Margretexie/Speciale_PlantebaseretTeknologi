#include "Adafruit_seesaw.h"
#include <Adafruit_NeoPixel.h>

// LED connected to digital pin 6
int dataPin = 6;           
#define NUMPIXELS          45
#define MAXIMUM_BRIGHTNESS 130
#define MEDIUM_BRIGHTNESS  130
#define LOW_BRIGHTNESS     100

int maxBrightness = 130;
int minBrightness = 0;
boolean serialBool = false;

Adafruit_NeoPixel pixel = Adafruit_NeoPixel(NUMPIXELS, dataPin);

Adafruit_seesaw ss;

int EMGPin = A1;    // select the input pin for the sensor
int EMGValue = 0;  // variable to store the value coming from the sensor
char incomingByte; //message from unity

void setup() {
  Serial.begin(4800);
  
  if (!ss.begin(0x36)) {
    Serial.println("ERROR! seesaw not found");
    while(1) delay(1);
  } else {
    Serial.println("seesaw started!");
  }
  
  pixel.begin(); // This initializes the NeoPixel library.
  turnOffLights();
}

void loop() {
  //pulseLED();
  EMGValue = analogRead(EMGPin);  // read the value from the EMG sensor:
  int tempC = ss.getTemp();
  int capread = ss.touchRead(0);
  serialBool = true;
  
  //EMGValue = map(EMGValue, 1, 1024, 0, 200); //map to speed
  //tempC = map(tempC, 0, 30, 0, 10); //map to cell size
  capread = map(capread, 1, 2000, -1, 5); //map to saturation

  Serial.print(EMGValue); Serial.print(",");
  Serial.print(tempC); Serial.print(","); //Temperature in celcius
  Serial.println(capread); //Capacitive value

  while (Serial.available() > 0 && serialBool == true) {
    // read the incoming byte:
    incomingByte = Serial.read();
    if (incomingByte =='h'){
      pulseLED();
    }
  }


  
}

void pulseLED(){
  // fade in
  for (int x = minBrightness; x < MAXIMUM_BRIGHTNESS; x+=10) {
    setLightsToBrightness(x);
      EMGValue = analogRead(EMGPin);  // read the value from the EMG sensor:
  int tempC = ss.getTemp();
  int capread = ss.touchRead(0);
  
  //EMGValue = map(EMGValue, 1, 1024, 0, 200); //map to speed
  //tempC = map(tempC, 0, 30, 0, 10); //map to cell size
  capread = map(capread, 1, 2000, -1, 5); //map to saturation

  Serial.print(EMGValue); Serial.print(",");
  Serial.print(tempC); Serial.print(","); //Temperature in celcius
  Serial.println(capread); //Capacitive value
  }
  // fade out
  for (int x = MAXIMUM_BRIGHTNESS; x >= minBrightness; x-=10) {
    setLightsToBrightness(x);
      EMGValue = analogRead(EMGPin);  // read the value from the EMG sensor:
  int tempC = ss.getTemp();
  int capread = ss.touchRead(0);
  
  //EMGValue = map(EMGValue, 1, 1024, 0, 200); //map to speed
  //tempC = map(tempC, 0, 30, 0, 10); //map to cell size
  capread = map(capread, 1, 2000, -1, 5); //map to saturation

  Serial.print(EMGValue); Serial.print(",");
  Serial.print(tempC); Serial.print(","); //Temperature in celcius
  Serial.println(capread); //Capacitive value
  }
  serialBool = false;
  
}

void setLightsToColor(int red, int green, int blue) {
  for (uint8_t i = 0; i < NUMPIXELS; i++) {
    pixel.setPixelColor(i, pixel.Color(red, green, blue));
  }
}

void setLightsToBrightness(int brightness) {
  for (uint8_t i = 0; i < NUMPIXELS; i++) {
  pixel.setBrightness(brightness);
  setLightsToColor(75, 193, 0);
  pixel.show();
  }
}

void turnOffLights() {
  setLightsToBrightness(0);
}

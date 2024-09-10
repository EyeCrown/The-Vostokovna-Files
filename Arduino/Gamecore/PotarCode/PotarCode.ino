#include <ArduinoJson.h>
/*
* Variables
*/

const int pinHours    = A0;
const int pinMinutes  = A4;
const int pinMeridian = A2;
const int pinCamera   = A5;

JsonDocument doc;

int hour;
int minute;

int meridian;
boolean meridianPeriod;

int cameraId;

const int MIN_ANALOG = 0;
const int MAX_ANALOG = 1024;

const int MIN_HOUR = 0;
const int MAX_HOUR = 12;

const int MIN_MINUTE   = 0;
const int MAX_MINUTE   = 60;

const int maxSizePotar = MAX_ANALOG / 3; // ~90°
int minAnalogCamera;
int maxAnalogCamera;

int maxCamera = 6;



/*
* Initialisation
*/
void setup() {
  pinMode(pinHours, INPUT);
  pinMode(pinMinutes, INPUT);
  pinMode(pinMeridian, INPUT);
  pinMode(pinCamera, INPUT);

  Serial.begin (19200);

  SetupAnalogCameraBoundaries(analogRead(pinCamera));
}


/*
* 
*/
void loop() {
  //doc.clear();

  hour      = analogRead(pinHours);
  minute    = analogRead(pinMinutes);
  meridian  = analogRead(pinMeridian);
  cameraId  = analogRead(pinCamera);
  
  meridianPeriod = GetMeridianPeriod(meridian); // Do this before ConvertHour()/Minute()

  hour    = ConvertToHour(hour);
  minute  = ConvertToMinute(minute);
  cameraId = GetCameraId(cameraId);

  doc["hour"]   = hour;
  doc["minute"] = minute;
  doc["camera"] = cameraId;

  serializeJson(doc, Serial);
  Serial.println();
  PrintTime();
}

void PrintTime(){

  String period = (meridian) ? "PM" : "AM";
  
  Serial.println("Il est " + String(hour) + "h" + String(minute) + " > " + String(meridianPeriod));

}

int ConvertToHour(int val){
  int result = map(val, MIN_ANALOG, MAX_ANALOG, MIN_HOUR, MAX_HOUR);

  return meridianPeriod ? result + 12 : result;
}

int ConvertToMinute(int val){
  return map(val, MIN_ANALOG, MAX_ANALOG, MIN_MINUTE, MAX_MINUTE);
}


/*
* AM: False | PM: True
*/
boolean GetMeridianPeriod(int value){
  return value > MAX_ANALOG / 2;
}

int GetCameraId(int val){
  val = constrain(val, minAnalogCamera, maxAnalogCamera);

  return map(val, minAnalogCamera, maxAnalogCamera, 0, maxCamera-1) + 1; // index from 1 to n
}


void SetupAnalogCameraBoundaries(int initialValue){
  minAnalogCamera = initialValue;
  maxAnalogCamera = initialValue + maxSizePotar;
  
  // There is no security if initialValue + maxSizePotar > MAX_ANALOG, it"s only a warning
  if (maxAnalogCamera > MAX_ANALOG)
    Serial.println("WARNING: maxAnalogCamera ("+ String(maxAnalogCamera) +") > MAX_ANALOG ("+ String(MAX_ANALOG) +")");

}



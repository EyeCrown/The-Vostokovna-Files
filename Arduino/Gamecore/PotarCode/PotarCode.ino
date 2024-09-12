/*
* Constants
*/
const int MIN_ANALOG = 0;
const int MAX_ANALOG = 1024;

const int MIN_HOUR = 0;
const int MAX_HOUR = 12;

const int MIN_MINUTE   = 0;
const int MAX_MINUTE   = 60;

const int MAX_SIZE_POTAR = MAX_ANALOG / 3; // ~90°

/*
* Variables
*/

// === Pins ===

const int pinHours    = A0;
const int pinMinutes  = A1;
const int pinCamera   = A2;
const int pinMeridian = A3;

const int pinHelp = 7;


// === Variables ===

String data;

int hour;
int minute;

// AM : PM
int meridian;
bool meridianPeriod;

int cameraId;

int helpState;

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
  pinMode(pinHelp, INPUT);


  Serial.begin(19200);

  SetupAnalogCameraBoundaries(analogRead(pinCamera));
}


/*
* Main loop
*/
void loop() {
  data = "";

  hour      = analogRead(pinHours);
  minute    = analogRead(pinMinutes);
  meridian  = analogRead(pinMeridian);
  cameraId  = analogRead(pinCamera);

  helpState = digitalRead(pinHelp);
  
  meridianPeriod = GetMeridianPeriod(meridian); // Do this before ConvertHour()/Minute()

  hour    = ConvertToHour(hour);
  minute  = ConvertToMinute(minute);
  cameraId = GetCameraId(cameraId);

  doc["hour"]   = hour;
  doc["minute"] = minute;
  doc["camera"] = cameraId;

  data = String(hour) + "," + String(minute) + "," + String(cameraId);

  if (helpState == HIGH)
    data += ",help"; 

  Serial.println(data);
}


/*
* Methods
*/
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
bool GetMeridianPeriod(int value){
  return value > MAX_ANALOG / 2;
}

int GetCameraId(int val){
  val = constrain(val, minAnalogCamera, maxAnalogCamera);

  return map(val, minAnalogCamera, maxAnalogCamera, 0, maxCamera-1) + 1; // index from 1 to n
}


/*
* Reset boundaries of camera potentiometer
*/
void SetupAnalogCameraBoundaries(int initialValue){
  minAnalogCamera = initialValue;
  maxAnalogCamera = initialValue + MAX_SIZE_POTAR;
  
  // There is no security if initialValue + MAX_SIZE_POTAR > MAX_ANALOG, it"s only a warning
  if (maxAnalogCamera > MAX_ANALOG)
    Serial.println("WARNING: maxAnalogCamera ("+ String(maxAnalogCamera) +") > MAX_ANALOG ("+ String(MAX_ANALOG) +")");

}



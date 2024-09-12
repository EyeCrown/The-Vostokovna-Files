#include <ArduinoJson.h>

/*
*   VARIABLES
*/
int pinA = 3; // Connected to CLK on KY-040
int pinB = 4; // Connected to DT on KY-040
int encoderPosCount = 0;
int numberRotation;

int reset;

int pinALast;
int aVal;
int bVal;
boolean bCW;

String lastClk;

JsonDocument doc;

/*
*   METHODS
*/
void setup() {
  pinMode (pinA,INPUT);
  pinMode (pinB,INPUT);
  pinMode (7, INPUT);
  /* Read Pin A
  Whatever state it's in will reflect the last position
  */
  pinALast = digitalRead(pinA);
  Serial.begin (19200);
}

void loop() {
  doc.clear();
  
  aVal = digitalRead(pinA);
  bVal = digitalRead(pinB);
  
  reset = digitalRead(7);

  if (!reset) {
    Serial.println("RESET");
    encoderPosCount = 0;
  }


  String message = "";

  if (RotationDetected()){ // Means the knob is rotating
    doc["rotation"] = true;
    
    message += "IIIIIIIIIIIIIII ";

    numberRotation++;
    
    String direction;
    // if the knob is rotating, we need to determine clockwise
    // We do that by reading pin B.
    if (bVal != aVal) { // Means pin A Changed first - We're Rotating Clockwise
      encoderPosCount ++;
      bCW = true;

      direction = "clockwise";
    } else {// Otherwise B changed first and we're moving CCW
      bCW = false;
      encoderPosCount--;

      direction = "counterclockwise";
    }

    lastClk = direction;
    doc["direction"] = direction;
  }
  else { // No rotation detected
    message += "O ";


    doc["rotation"] = false;
    doc["direction"] = lastClk;


  }
  

  doc["PosCount"] = encoderPosCount;

  
  //serializeJson(doc, Serial);

  //encoderPosCount %= 20; 


  Serial.println();

  String final = "Input: " + String(digitalRead(7)) + "    ";
  final += "Position: " + String(encoderPosCount) + "    ";
  final += "Number of rotations: " + String(numberRotation / 2) + "    \n";

  Serial.print(final);
  
  pinALast = aVal;

  delay(1);
}



boolean RotationDetected() {
  return aVal != pinALast;
}


/*
* Variables
*/
int value;


/*
* Initialisation
*/
void setup() {
  Serial.begin (19200);
}


/*
* 
*/
void loop() {
  value = analogRead(A0);

  Serial.println("Value: " + String(value));
}
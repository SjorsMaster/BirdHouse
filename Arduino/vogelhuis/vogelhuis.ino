#include "signalenzo.h" //Thanks to Aeralius for helping create this library!

float lastValue;

void setup() {
   //Opening connection
   Serial.begin(9600);

  //Regestering all the buttons  
  pinMode(7, INPUT_PULLUP);
  registerSignalCommand(7);
  pinMode(6, INPUT_PULLUP);
  registerSignalCommand(6);  
  pinMode(5, INPUT_PULLUP);
  registerSignalCommand(5);  
  pinMode(4, INPUT_PULLUP);
  registerSignalCommand(4);  
}

void loop() {
  //Don't torture the Arduino :)
  delay(50);
  checkSignals();

  float Input = analogRead(0)/1023.0;
  if(Input > lastValue+.01f || Input < lastValue-.01f){
    lastValue = Input;
    Serial.print("A0,"); Serial.println(lastValue);
  }
}

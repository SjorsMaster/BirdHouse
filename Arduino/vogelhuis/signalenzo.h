//With help from Aeralius!

bool signalPins[14] = {};   
bool signalPinsUps[14] = {};
bool signalPinsDowns[14] = {};

void registerSignalCommand( int Pin )
{
  signalPins[Pin]=true; //Add the pin to the pin list thing :)
   
}


void checkSignals()
{
  for(int i=0;i<14;i++)
  {
    if(signalPins[i]==true)
    {
      //DOWN
      if(!digitalRead(i) and !signalPinsUps[i])
      {
        signalPinsUps[i] = true;
        signalPinsDowns[i] = false;
        Serial.print("D");Serial.print(i); Serial.println(",1");  
      }
      //UP
      if(digitalRead(i) and !signalPinsDowns[i])
      {
        signalPinsUps[i] = false;
        signalPinsDowns[i] = true;
        Serial.print("D");Serial.print(i); Serial.println(",0"); 
      }
    }
  }
}

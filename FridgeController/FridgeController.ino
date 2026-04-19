#include <Servo.h>
#include <string.h>

//circuit board component variablles
const int potPin = A0;
const int pot2Pin = A1;
const int sliderPot = A2;
const int slider2Pot = A3;

int potVal;
int pot2Val;
int sliderVal;
int slider2Val;
int previousPotVal;
int previousPot2Val;
int previousSliderVal;
int previousSlider2Val;



const int ledPin = 13;
const int led2Pin = 12;
const int led3Pin = 11;
const int led4Pin = 10;

//fan turner componenets
Servo fanServo;
const int fanServoPin = 5;

int fanAngle;

const int leftSwitchPin = 6;
const int rightSwitchPin = 7;

//these overview the state of the switches
int leftSwitchState = 0;
int previousLeftSwitchState = 0;
int rightSwitchState = 0;
int previousRightSwitchState = 0;

//thermometer component variabels
Servo thermoServo;
const int thermoServoPin = 8;
int thermometerVal;
int thermometerAngle;


//fan component variables
const int enablePin = 4;
const int controlPin1 = 2; // controll pins handle which direction the motor is moving
const int controlPin2 = 3;

int motorEnabled = 0;
int motorSpeed = 0;
int motorDirection = 1;




void setup() {
  //REMINDER: you don't need to set up pot pins in the analog area

  //set up fan turner components
  fanServo.attach(fanServoPin);
  //controls fot fan
  pinMode(leftSwitchPin, INPUT);
  pinMode(rightSwitchPin, INPUT);

  //set up thermoComponents
  thermoServo.attach(thermoServoPin);

  //set up the fan motor
  pinMode(controlPin1, OUTPUT);
  pinMode(controlPin2, OUTPUT);
  pinMode(enablePin, OUTPUT);

  //set the motor to Off
  digitalWrite(enablePin, LOW);
  //set the direction of the motor
  digitalWrite(controlPin1, HIGH);
  digitalWrite(controlPin2, LOW);

  //comunicate to unity project
  Serial.begin(115200);
  

}

void controlMotorSpeed(int motorSpeed) {
  //turn motor on/off
  if (motorEnabled == 1) digitalWrite(enablePin, HIGH);
  else digitalWrite(enablePin, LOW);

  //set speed of motor
  if (motorEnabled == true) {
    analogWrite(enablePin, motorSpeed);
  }
  else {
    analogWrite(enablePin, 0);
  }
}

//DONE
void sendDataPacket() {
  //-----------------------------------------------------------------------------FAN BUTTONS
  //if the state of the button changes then send an info update
  if(previousLeftSwitchState != leftSwitchState)
    if(leftSwitchState == LOW)
        Serial.println("{\"command\":\"LEFT\", \"state\":\"DOWN\"}");
      else 
        Serial.println("{\"command\":\"LEFT\", \"state\":\"UP\"}"); 
  if(previousRightSwitchState != rightSwitchState)
      if(rightSwitchState == LOW)
        Serial.println("{\"command\":\"LEFT\", \"state\":\"DOWN\"}");
      else 
        Serial.println("{\"command\":\"LEFT\", \"state\":\"UP\"}"); 
  //-----------------------------------------------------------------------------CIRCUIT DIALS
  if(previousPotVal != potVal)
    Serial.print("{\"command\":\"POT1\", \"state\":\"");
    Serial.print(potVal);
    Serial.println("\"}"); 
  if(previousPot2Val != pot2Val)
    Serial.print("{\"command\":\"POT2\", \"state\":\"");
    Serial.print(pot2Val);
    Serial.println("\"}"); 
  if(previousSliderVal != sliderVal)
    Serial.print("{\"command\":\"SLIDER1\", \"state\":\"");
    Serial.print(sliderVal);
    Serial.println("\"}"); 
  if(previousSlider2Val != slider2Val)
    Serial.print("{\"command\":\"POT1\", \"state\":\"");
    Serial.print(slider2Val);
    Serial.println("\"}"); 
}

//DONE
//reads from unity program
void readDataPacket() {
  //things that need to be read from the data packet:
  // - thermometer value/angle
  // - power of fan
  // - angle of fan
  //thermometerVal = ;
  //read the line
  String line = Serial.readString();
  if(line.equals("")) return; //if there is nothing there then don't update the variables

  //seperate the first number from the rest of the string
  thermometerVal = atoi(line.substring(0, line.indexOf(',')).c_str());
  line = line.substring(line.indexOf(',')+1);

  //get the second number from the list
  fanAngle = atoi(line.substring(0, line.indexOf(',')).c_str());
  line = line.substring(line.indexOf(',')+1);


  motorSpeed = atoi(line.c_str());

  Serial.println(thermometerVal);
  Serial.println(fanAngle);
  Serial.println(motorSpeed);
}

//DONE
void readCircuitInput(){
  potVal = analogRead(potPin);
  pot2Val = analogRead(pot2Pin);
  sliderVal = analogRead(sliderPot);
  slider2Val = analogRead(slider2Pot);
}

//DONE
void updatePreviousVariables(){
  //fan controls
  previousLeftSwitchState = leftSwitchState;
  previousRightSwitchState = rightSwitchState;

  //circuit controls
  previousPotVal = potVal;
  previousPot2Val = pot2Val;
  previousSliderVal = sliderVal;
  previousSlider2Val = slider2Val;
}

//TODO: add led to items otherwise DONE
void loop() {
  //read the state of the left/right control switch states
  leftSwitchState = digitalRead(leftSwitchPin);
  rightSwitchState = digitalRead(rightSwitchPin);

  //read curcuit board inputs
  readCircuitInput();

  //collect the variables from the board and send it to unity
  sendDataPacket();

  //recieve the data packet that was sent from the unity
  readDataPacket();
  

  //------------------------------------------------------------------------------------OUTPUT
  //update thermometer
  thermometerAngle = map(thermometerVal, 0, 2, 0, 179);
  thermoServo.write(thermometerAngle);
  //update motor controls
  controlMotorSpeed(motorSpeed);

  //update Fan turner controls
  fanServo.write(fanAngle);  
  //------------------------------------------------------------------------------------
  
}  //end loop



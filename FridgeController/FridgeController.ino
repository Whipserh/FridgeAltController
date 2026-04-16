#include <Servo.h>


//circuit board component variablles
const int potPin = A0;
const int pot2Pin = A1;
const int sliderPot = A2;
const int slider2Pot = A3;
int potVal;
int pot2Val;
int sliderVal;
int slider2Val;


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

private
void controlMotorSpeed(bool enableMotor, int motorSpeed) {
  //turn motor on/off
  if (enableMotor) digitalWrite(enablePin, HIGH);
  else digitalWrite(enablePin, LOW);

  //set speed of motor
  if (motorEnabled == 1) {
    analogWrite(enablePin, motorSpeed);
  }
  else {
    analogWrite(enablePin, 0);
  }
}

//write to data packet
//value of each input
private void sendDataPacket() {
  string data = "{";
}

//reads from unity program
private void readDataPacket() {
  //things that need to be read from the data packet:
  // - thermometer value/angle
  // - power of fan
  // - angle of fan
  //thermometerVal = ;
}

private void readCircuitInput(){
  potVal = analogRead(potPin);
  pot2Val = analogRead(pot2Pin);
  sliderVal = analogRead(sliderPot);
  slider2Val = analogRead(slider2Pot);
}

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
  
  //update thermometer
  thermometerAngle = map(thermometerVal, 0, 2, 0, 179);
  thermoServo.write(thermometerAngle);
  //update motor controls
  controlMotorSpeed(enableMotor, motorSpeed);

  //update Fan turner controls
  fanServo.write(fanAngle);  

  previousLeftSwitchState = leftSwitchState;
  previousRightSwitchState = rightSwitchState;
}  //end loop

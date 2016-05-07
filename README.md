# WindowsIOTLEDMatrix
This Library allows you to use LED Matrix with your Raspberry Pi 3 on Windows IOT Core. It is working with LED matrices 32x16 that can be chained. Currently tested to a chain of 4 matrices of 32x32 => 128x32

The port mapping to make it work is:<br/>
GPIO 5 (SDA)       -->  OE (Output Enabled)<br/>
GPIO 6 (SCL)       -->  CLK (Serial Clock)<br/>
GPIO 4 (GPCLK0)    -->  LAT (Data Latch)<br/>
GPIO 12 (CE1)       -->  A  --|<br/>
GPIO 13 (CE0)       -->  B    |   Row<br/>
GPIO 16 (MISO)      -->  C    | Address<br/>
GPIO 19 (MOSI)     -->  D  --|<br/>
GPIO 17            -->  R1 (LED 1: Red)<br/>
GPIO 18 (PCM_CLK)  -->  B1 (LED 1: Blue)<br/>
GPIO 22            -->  G1 (LED 1: Green)<br/>
GPIO 23            -->  R2 (LED 2: Red)<br/>
GPIO 24            -->  B2 (LED 2: Blue)<br/>
GPIO 25            -->  G2 (LED 2: Green)<br/>

The library is still not very fast and does some flickering. Any help will be apreciated.
This library was based on the work of https://github.com/mattdh666/rpi-led-matrix-panel

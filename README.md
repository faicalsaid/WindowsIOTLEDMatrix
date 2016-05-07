# WindowsIOTLEDMatrix
This Library allows you to use LED Matrix with your Raspberry Pi 3 on Windows IOT Core. It is working with LED matrices 32x16 that can be chained. Currently tested to a chain of 4 matrices of 32x32 => 128x32

The port mapping to make it work is:
GPIO 5 (SDA)       -->  OE (Output Enabled)
GPIO 6 (SCL)       -->  CLK (Serial Clock)
GPIO 4 (GPCLK0)    -->  LAT (Data Latch)
GPIO 12 (CE1)       -->  A  --|
GPIO 13 (CE0)       -->  B    |   Row
GPIO 16 (MISO)      -->  C    | Address
GPIO 19 (MOSI)     -->  D  --|
GPIO 17            -->  R1 (LED 1: Red)
GPIO 18 (PCM_CLK)  -->  B1 (LED 1: Blue)
GPIO 22            -->  G1 (LED 1: Green)
GPIO 23            -->  R2 (LED 2: Red)
GPIO 24            -->  B2 (LED 2: Blue)
GPIO 25            -->  G2 (LED 2: Green)



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace LedMatrixEngineSharp
{

    public class GpioProxy
    {
        public GpioController gpio { get; set; }
        #region "Pin Numbers"
        const int OE_PIN = 5;
        const int CLOCK_PIN = 6;
        const int LATCH_PIN = 4;
        const int ROWADDRA_PIN = 12;
        const int ROWADDRB_PIN = 13;
        const int ROWADDRC_PIN = 16;
        const int ROWADDRD_PIN = 19;
        const int R1_PIN = 17;
        const int G1_PIN = 22;
        const int B1_PIN = 18;
        const int R2_PIN = 23;
        const int G2_PIN = 25;
        const int B2_PIN = 24;
        #endregion

        public GpioPin outputEnabled;
        public GpioPin clock;
        public GpioPin latch;
        public GpioPin rowAddressA;
        public GpioPin rowAddressB;
        public GpioPin rowAddressC;
        public GpioPin rowAddressD;
        public GpioPin r1;
        public GpioPin g1;
        public GpioPin b1;
        public GpioPin r2;
        public GpioPin g2;
        public GpioPin b2;

        public GpioProxy()
        {
            gpio = GpioController.GetDefault();
        }

        public void setupOutputBits()
        {
            outputEnabled = gpio.OpenPin(OE_PIN);
            outputEnabled.SetDriveMode(GpioPinDriveMode.Output);
            clock = gpio.OpenPin(CLOCK_PIN);
            clock.SetDriveMode(GpioPinDriveMode.Output);
            latch = gpio.OpenPin(LATCH_PIN);
            latch.SetDriveMode(GpioPinDriveMode.Output);
            rowAddressA = gpio.OpenPin(ROWADDRA_PIN);
            rowAddressA.SetDriveMode(GpioPinDriveMode.Output);
            rowAddressB = gpio.OpenPin(ROWADDRB_PIN);
            rowAddressB.SetDriveMode(GpioPinDriveMode.Output);
            rowAddressC = gpio.OpenPin(ROWADDRC_PIN);
            rowAddressC.SetDriveMode(GpioPinDriveMode.Output);
            rowAddressD = gpio.OpenPin(ROWADDRD_PIN);
            rowAddressD.SetDriveMode(GpioPinDriveMode.Output);
            r1 = gpio.OpenPin(R1_PIN);
            r1.Write(GpioPinValue.Low);
            r1.SetDriveMode(GpioPinDriveMode.Output);
            b1 = gpio.OpenPin(B1_PIN);
            b1.Write(GpioPinValue.Low);
            b1.SetDriveMode(GpioPinDriveMode.Output);
            g1 = gpio.OpenPin(G1_PIN);
            g1.Write(GpioPinValue.Low);
            g1.SetDriveMode(GpioPinDriveMode.Output);
            r2 = gpio.OpenPin(R2_PIN);
            r2.Write(GpioPinValue.Low);
            r2.SetDriveMode(GpioPinDriveMode.Output);
            b2 = gpio.OpenPin(B2_PIN);
            b2.Write(GpioPinValue.Low);
            b2.SetDriveMode(GpioPinDriveMode.Output);
            g2 = gpio.OpenPin(G2_PIN);
            g2.Write(GpioPinValue.Low);
            g2.SetDriveMode(GpioPinDriveMode.Output);

        }

        GpioPinValue lastr1;
        GpioPinValue lastg1;
        GpioPinValue lastb1;
        GpioPinValue lastr2;
        GpioPinValue lastg2;
        GpioPinValue lastb2;
        public void setRGB(GpioPinValue _r1, GpioPinValue _g1, GpioPinValue _b1, GpioPinValue _r2, GpioPinValue _g2, GpioPinValue _b2)
        {
            if (lastr1 != _r1)
            {
                r1.Write(_r1);
                lastr1 = _r1;
            }
            if (lastg1 != _g1)
            {
                g1.Write(_g1);
                lastg1 = _g1;
            }
            if (lastb1 != _b1)
            {
                b1.Write(_b1);
                lastb1 = _b1;
            }
            if (lastr2 != _r2)
            {
                r2.Write(_r2);
                lastr2 = _r2;
            }
            if (lastg2 != _g2)
            {
                g2.Write(_g2);
                lastg2 = _g2;
            }
            if (lastb2 != _b2)
            {
                b2.Write(_b2);
                lastb2 = _b2;
            }
        }

        public void setRowAddress(int row)
        {
            rowAddressA.Write((row & 1) == 1 ? GpioPinValue.High : GpioPinValue.Low);
            rowAddressB.Write((row & 2) == 2 ? GpioPinValue.High : GpioPinValue.Low);
            rowAddressC.Write((row & 4) == 4 ? GpioPinValue.High : GpioPinValue.Low);
            rowAddressD.Write((row & 8) == 8 ? GpioPinValue.High : GpioPinValue.Low);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using System.Threading;
using System.Diagnostics;
using Microsoft.Graphics.Canvas;
//using MicroLibrary;

namespace LedMatrixEngineSharp
{

    public class RgbMatrix
    {
        public GpioProxy proxy;

        public Color _fontColor { get; set; }
        public int _fontHeight { get; set; }
        public int _fontSize { get; set; }
        public int _fontWidth { get; set; }
        public int _textCursorX { get; set; }
        public int _textCursorY { get; set; }
        public bool _wordWrap { get; set; }
        public int RowsPerSubPanel { get; set; }
        private LedDisplay display;
        private int pwmbits = 1;
        public int RowClockTime { get; set; }
        private CanvasDevice device;
        private CanvasRenderTarget target;
        public CanvasDrawingSession Session { get; set; }

        long[] RowSleepNanos;
        byte[] Font5x7;
        byte[] Font4x6;
        byte[] Font3x5;

        int Width = 128;
        int Height = 32;
        public RgbMatrix()
        {
            #region "Fonts"
            Font3x5 = new byte[]{
        0x00, 0x00, 0x00,  // (space)
		0x17, 0x00, 0x00,  // !
		0x03, 0x00, 0x03,  // "
		0x0A, 0x1F, 0x0A,  // #
		0x16, 0x13, 0x1A,  // 0x
		0x09, 0x04, 0x0A,  // %
		0x0A, 0x15, 0x1A,  // &
		0x03, 0x00, 0x00,  // '
		0x00, 0x0E, 0x11,  // (
		0x11, 0x0E, 0x00,  // )
		0x06, 0x06, 0x00,  // *
		0x04, 0x0E, 0x04,  // +
		0x0C, 0x1C, 0x00,  // ,
		0x04, 0x04, 0x04,  // -
		0x10, 0x00, 0x00,  // .
		0x18, 0x04, 0x03,  // /
		0x1F, 0x11, 0x1F,  // 0
		0x02, 0x1F, 0x00,  // 1
		0x1D, 0x15, 0x17,  // 2
		0x15, 0x15, 0x1F,  // 3
		0x0F, 0x08, 0x1E,  // 4
		0x17, 0x15, 0x1D,  // 5
		0x1F, 0x15, 0x1D,  // 6
		0x01, 0x01, 0x1F,  // 7
		0x1F, 0x15, 0x1F,  // 8
		0x17, 0x15, 0x1F,  // 9
		0x00, 0x0A, 0x00,  // :
		0x00, 0x1A, 0x00,  // ;
		0x04, 0x0A, 0x11,  // <
		0x0A, 0x0A, 0x0A,  // =
		0x11, 0x0A, 0x04,  // >
		0x00, 0x15, 0x07,  // ?
		0x1F, 0x15, 0x17,  // @
		0x1F, 0x05, 0x1F,  // A
		0x1F, 0x15, 0x1B,  // B
		0x1F, 0x11, 0x11,  // C
		0x1F, 0x11, 0x0E,  // D
		0x1F, 0x15, 0x15,  // E
		0x1F, 0x05, 0x01,  // F
		0x1F, 0x11, 0x1D,  // G
		0x1F, 0x04, 0x1F,  // H
		0x11, 0x1F, 0x11,  // I
		0x08, 0x10, 0x0F,  // J
		0x1F, 0x04, 0x1B,  // K
		0x1F, 0x10, 0x10,  // L
		0x1F, 0x06, 0x1F,  // M
		0x1C, 0x04, 0x1C,  // N
		0x1F, 0x11, 0x1F,  // O
		0x1F, 0x05, 0x07,  // P
		0x0E, 0x19, 0x1E,  // Q
		0x1F, 0x05, 0x1B,  // R
		0x17, 0x15, 0x1D,  // S
		0x01, 0x1F, 0x01,  // T
		0x1F, 0x10, 0x1F,  // U
		0x0F, 0x10, 0x0F,  // V
		0x1F, 0x0C, 0x1F,  // W
		0x1B, 0x04, 0x1B,  // X
		0x17, 0x14, 0x1F,  // Y
		0x19, 0x15, 0x13,  // Z
		0x00, 0x1F, 0x11,  // [
		0x03, 0x04, 0x18,  // BackSlash
		0x11, 0x1F, 0x00,  // ]
		0x06, 0x01, 0x06,  // ^
		0x10, 0x10, 0x10,  // _
		0x01, 0x01, 0x02,  // `
		0x18, 0x14, 0x1C,  // a
		0x1F, 0x14, 0x1C,  // b
		0x1C, 0x14, 0x14,  // c
		0x1C, 0x14, 0x1F,  // d
		0x0C, 0x1A, 0x14,  // e
		0x04, 0x1E, 0x05,  // f
		0x17, 0x15, 0x1E,  // g
		0x1F, 0x04, 0x1C,  // h
		0x00, 0x1D, 0x00,  // i
		0x08, 0x10, 0x0D,  // j
		0x1F, 0x0C, 0x1A,  // k
		0x00, 0x1F, 0x00,  // l
		0x18, 0x0C, 0x18,  // m
		0x18, 0x04, 0x18,  // n
		0x1E, 0x12, 0x1E,  // o
		0x1F, 0x05, 0x07,  // p
		0x07, 0x05, 0x1F,  // q
		0x1E, 0x04, 0x04,  // r
		0x12, 0x15, 0x09,  // s
		0x02, 0x1F, 0x02,  // t
		0x1C, 0x10, 0x1C,  // u
		0x0C, 0x10, 0x0C,  // v
		0x0C, 0x18, 0x0C,  // w
		0x14, 0x08, 0x14,  // x
		0x16, 0x18, 0x06,  // y
		0x04, 0x1C, 0x10,  // z
		0x04, 0x0E, 0x11,  // {
		0x00, 0x1F, 0x00,  // |
		0x11, 0x0E, 0x04,  // }
		0x02, 0x04, 0x02,  // ~
		0x1F, 0x1F, 0x1F   // 
	};
            Font4x6 = new byte[]{
        0x00, 0x00, 0x00, 0x00, // (space)
		0x00, 0x2F, 0x00, 0x00, // !
		0x03, 0x00, 0x03, 0x00, // "
		0x3F, 0x0A, 0x3F, 0x0A, // #
		0x03, 0x02, 0x07, 0x00, // $
		0x33, 0x0B, 0x34, 0x33, // %
		0x1A, 0x25, 0x2A, 0x10, // &
		0x00, 0x03, 0x00, 0x00, // '
		0x00, 0x1E, 0x21, 0x00, // (
		0x00, 0x21, 0x1E, 0x00, // )
		0x0A, 0x04, 0x0A, 0x00, // *
		0x04, 0x0E, 0x04, 0x00, // +
		0x20, 0x10, 0x00, 0x00, // ,
		0x04, 0x04, 0x04, 0x00, // -
		0x00, 0x20, 0x00, 0x00, // .
		0x30, 0x08, 0x04, 0x03, // /
		0x1E, 0x29, 0x25, 0x1E, // 0
		0x22, 0x3F, 0x20, 0x00, // 1
		0x32, 0x29, 0x25, 0x22, // 2
		0x12, 0x21, 0x25, 0x1A, // 3
		0x0C, 0x0A, 0x3F, 0x08, // 4
		0x27, 0x25, 0x25, 0x19, // 5
		0x1E, 0x25, 0x25, 0x19, // 6
		0x01, 0x39, 0x05, 0x03, // 7
		0x1A, 0x25, 0x25, 0x1A, // 8
		0x06, 0x29, 0x29, 0x1E, // 9
		0x00, 0x14, 0x00, 0x00, // :
		0x20, 0x14, 0x00, 0x00, // ;
		0x08, 0x14, 0x22, 0x22, // <
		0x0A, 0x0A, 0x0A, 0x0A, // =
		0x22, 0x22, 0x14, 0x08, // >
		0x02, 0x01, 0x2D, 0x02, // ?
		0x1E, 0x21, 0x2D, 0x2E, // @
		0x3E, 0x09, 0x09, 0x3E, // A
		0x3F, 0x25, 0x25, 0x1A, // B
		0x1E, 0x21, 0x21, 0x12, // C
		0x3F, 0x21, 0x21, 0x1E, // D
		0x3F, 0x25, 0x25, 0x21, // E
		0x3F, 0x05, 0x05, 0x01, // F
		0x1E, 0x21, 0x29, 0x1A, // G
		0x3F, 0x04, 0x04, 0x3F, // H
		0x21, 0x3F, 0x21, 0x00, // I
		0x10, 0x21, 0x21, 0x1F, // J      
		0x3F, 0x0C, 0x12, 0x21, // K      
		0x3F, 0x20, 0x20, 0x20, // L      
		0x3F, 0x02, 0x06, 0x3F, // M      
		0x3F, 0x04, 0x08, 0x3F, // N      
		0x1E, 0x21, 0x21, 0x1E, // O
		0x3F, 0x09, 0x09, 0x06, // P
		0x1E, 0x21, 0x11, 0x2E, // Q
		0x3F, 0x09, 0x09, 0x36, // R
		0x22, 0x25, 0x25, 0x19, // S
		0x01, 0x3F, 0x01, 0x01, // T
		0x1F, 0x20, 0x20, 0x1F, // U
		0x0F, 0x30, 0x10, 0x0F, // V
		0x3F, 0x10, 0x18, 0x3F, // W
		0x33, 0x0C, 0x0C, 0x33, // X
		0x07, 0x38, 0x04, 0x03, // Y
		0x31, 0x29, 0x25, 0x23, // Z
		0x00, 0x3F, 0x21, 0x00, // [
		0x03, 0x04, 0x08, 0x30, // "\"
		0x00, 0x21, 0x3F, 0x00, // ]
		0x02, 0x01, 0x02, 0x00, // ^
		0x20, 0x20, 0x20, 0x20, // _
		0x00, 0x01, 0x02, 0x00, // `
		0x10, 0x2A, 0x2A, 0x3C, // a
		0x3F, 0x24, 0x24, 0x18, // b
		0x1C, 0x22, 0x22, 0x14, // c
		0x18, 0x24, 0x24, 0x3F, // d
		0x1C, 0x2A, 0x2A, 0x0C, // e
		0x3E, 0x09, 0x01, 0x02, // f
		0x24, 0x2A, 0x2A, 0x1E, // g
		0x3F, 0x08, 0x04, 0x38, // h
		0x24, 0x3D, 0x20, 0x00, // i
		0x10, 0x20, 0x20, 0x1D, // j
		0x3F, 0x08, 0x14, 0x22, // k
		0x21, 0x3F, 0x20, 0x00, // l
		0x3E, 0x02, 0x1C, 0x3E, // m
		0x3E, 0x02, 0x02, 0x3C, // n
		0x1C, 0x22, 0x22, 0x1C, // o
		0x3E, 0x0A, 0x0A, 0x04, // p
		0x04, 0x0A, 0x0A, 0x3E, // q
		0x3E, 0x02, 0x02, 0x04, // r
		0x24, 0x2A, 0x2A, 0x12, // s
		0x04, 0x1E, 0x24, 0x20, // t
		0x1E, 0x20, 0x20, 0x3E, // u
		0x1E, 0x20, 0x10, 0x0E, // v
		0x1E, 0x38, 0x20, 0x1E, // w
		0x36, 0x08, 0x08, 0x36, // x
		0x26, 0x28, 0x28, 0x1E, // y
		0x32, 0x2A, 0x2A, 0x26, // z
		0x04, 0x1B, 0x21, 0x00, // {
		0x00, 0x3F, 0x00, 0x00, // |
		0x00, 0x21, 0x1B, 0x04, // }
		0x04, 0x02, 0x04, 0x02, // ~
		0x3F, 0x35, 0x35, 0x3F  // 
	};

            Font5x7 = new byte[]{
        0x00, 0x00, 0x00, 0x00, 0x00,// (space)
		0x00, 0x00, 0x5F, 0x00, 0x00,// !
		0x00, 0x07, 0x00, 0x07, 0x00,// "
		0x14, 0x7F, 0x14, 0x7F, 0x14,// #
		0x24, 0x2A, 0x7F, 0x2A, 0x12,// $
		0x23, 0x13, 0x08, 0x64, 0x62,// %
		0x36, 0x49, 0x55, 0x22, 0x50,// &
		0x00, 0x05, 0x03, 0x00, 0x00,// '
		0x00, 0x1C, 0x22, 0x41, 0x00,// (
		0x00, 0x41, 0x22, 0x1C, 0x00,// )
		0x08, 0x2A, 0x1C, 0x2A, 0x08,// *
		0x08, 0x08, 0x3E, 0x08, 0x08,// +
		0x00, 0x50, 0x30, 0x00, 0x00,// ,
		0x08, 0x08, 0x08, 0x08, 0x08,// -
		0x00, 0x60, 0x60, 0x00, 0x00,// .
		0x20, 0x10, 0x08, 0x04, 0x02,// /
		0x3E, 0x51, 0x49, 0x45, 0x3E,// 0
		0x00, 0x42, 0x7F, 0x40, 0x00,// 1
		0x42, 0x61, 0x51, 0x49, 0x46,// 2
		0x21, 0x41, 0x45, 0x4B, 0x31,// 3
		0x18, 0x14, 0x12, 0x7F, 0x10,// 4
		0x27, 0x45, 0x45, 0x45, 0x39,// 5
		0x3C, 0x4A, 0x49, 0x49, 0x30,// 6
		0x01, 0x71, 0x09, 0x05, 0x03,// 7
		0x36, 0x49, 0x49, 0x49, 0x36,// 8
		0x06, 0x49, 0x49, 0x29, 0x1E,// 9
		0x00, 0x36, 0x36, 0x00, 0x00,// :
		0x00, 0x56, 0x36, 0x00, 0x00,// ;
		0x00, 0x08, 0x14, 0x22, 0x41,// <
		0x14, 0x14, 0x14, 0x14, 0x14,// =
		0x41, 0x22, 0x14, 0x08, 0x00,// >
		0x02, 0x01, 0x51, 0x09, 0x06,// ?
		0x32, 0x49, 0x79, 0x41, 0x3E,// @
		0x7E, 0x11, 0x11, 0x11, 0x7E,// A
		0x7F, 0x49, 0x49, 0x49, 0x36,// B
		0x3E, 0x41, 0x41, 0x41, 0x22,// C
		0x7F, 0x41, 0x41, 0x22, 0x1C,// D
		0x7F, 0x49, 0x49, 0x49, 0x41,// E
		0x7F, 0x09, 0x09, 0x01, 0x01,// F
		0x3E, 0x41, 0x41, 0x51, 0x32,// G
		0x7F, 0x08, 0x08, 0x08, 0x7F,// H
		0x00, 0x41, 0x7F, 0x41, 0x00,// I
		0x20, 0x40, 0x41, 0x3F, 0x01,// J
		0x7F, 0x08, 0x14, 0x22, 0x41,// K
		0x7F, 0x40, 0x40, 0x40, 0x40,// L
		0x7F, 0x02, 0x04, 0x02, 0x7F,// M
		0x7F, 0x04, 0x08, 0x10, 0x7F,// N
		0x3E, 0x41, 0x41, 0x41, 0x3E,// O
		0x7F, 0x09, 0x09, 0x09, 0x06,// P
		0x3E, 0x41, 0x51, 0x21, 0x5E,// Q
		0x7F, 0x09, 0x19, 0x29, 0x46,// R
		0x46, 0x49, 0x49, 0x49, 0x31,// S
		0x01, 0x01, 0x7F, 0x01, 0x01,// T
		0x3F, 0x40, 0x40, 0x40, 0x3F,// U
		0x1F, 0x20, 0x40, 0x20, 0x1F,// V
		0x7F, 0x20, 0x18, 0x20, 0x7F,// W
		0x63, 0x14, 0x08, 0x14, 0x63,// X
		0x03, 0x04, 0x78, 0x04, 0x03,// Y
		0x61, 0x51, 0x49, 0x45, 0x43,// Z
		0x00, 0x00, 0x7F, 0x41, 0x41,// [
		0x02, 0x04, 0x08, 0x10, 0x20,// "\"
		0x41, 0x41, 0x7F, 0x00, 0x00,// ]
		0x04, 0x02, 0x01, 0x02, 0x04,// ^
		0x40, 0x40, 0x40, 0x40, 0x40,// _
		0x00, 0x01, 0x02, 0x04, 0x00,// `
		0x20, 0x54, 0x54, 0x54, 0x78,// a
		0x7F, 0x48, 0x44, 0x44, 0x38,// b
		0x38, 0x44, 0x44, 0x44, 0x20,// c
		0x38, 0x44, 0x44, 0x48, 0x7F,// d
		0x38, 0x54, 0x54, 0x54, 0x18,// e
		0x08, 0x7E, 0x09, 0x01, 0x02,// f
		0x08, 0x14, 0x54, 0x54, 0x3C,// g
		0x7F, 0x08, 0x04, 0x04, 0x78,// h
		0x00, 0x44, 0x7D, 0x40, 0x00,// i
		0x20, 0x40, 0x44, 0x3D, 0x00,// j
		0x00, 0x7F, 0x10, 0x28, 0x44,// k
		0x00, 0x41, 0x7F, 0x40, 0x00,// l
		0x7C, 0x04, 0x18, 0x04, 0x78,// m
		0x7C, 0x08, 0x04, 0x04, 0x78,// n
		0x38, 0x44, 0x44, 0x44, 0x38,// o
		0x7C, 0x14, 0x14, 0x14, 0x08,// p
		0x08, 0x14, 0x14, 0x18, 0x7C,// q
		0x7C, 0x08, 0x04, 0x04, 0x08,// r
		0x48, 0x54, 0x54, 0x54, 0x20,// s
		0x04, 0x3F, 0x44, 0x40, 0x20,// t
		0x3C, 0x40, 0x40, 0x20, 0x7C,// u
		0x1C, 0x20, 0x40, 0x20, 0x1C,// v
		0x3C, 0x40, 0x30, 0x40, 0x3C,// w
		0x44, 0x28, 0x10, 0x28, 0x44,// x
		0x0C, 0x50, 0x50, 0x50, 0x3C,// y
		0x44, 0x64, 0x54, 0x4C, 0x44,// z
		0x00, 0x08, 0x36, 0x41, 0x00,// {
		0x00, 0x00, 0x7F, 0x00, 0x00,// |
		0x00, 0x41, 0x36, 0x08, 0x00,// }
		0x08, 0x08, 0x2A, 0x1C, 0x08,// ->
		0x08, 0x1C, 0x2A, 0x08, 0x08 // <-
	};
            #endregion
            _textCursorX = 0;
            _textCursorY = 0;
            Windows.UI.Color white = Windows.UI.Color.FromArgb(255, 255, 255, 255);
            proxy = new GpioProxy();
            proxy.setupOutputBits();

             device = new CanvasDevice();
             target = new CanvasRenderTarget(device, Width, Height, 96);
             Session = target.CreateDrawingSession();

            _fontColor = white;
            _fontSize = 1;
            _fontWidth = 3;
            _fontHeight = 5;
            _wordWrap = true;
            RowsPerSubPanel = 16;
            RowClockTime = 100;
            RowSleepNanos = new long[]  {   // Only using the first PwmBits elements.
                (1 * RowClockTime) - RowClockTime,
                        (2 * RowClockTime) - RowClockTime,
                        (4 * RowClockTime) - RowClockTime,
                        (8 * RowClockTime) - RowClockTime,
                        (16 * RowClockTime) - RowClockTime,
                        (32 * RowClockTime) - RowClockTime,
                        (64 * RowClockTime) - RowClockTime,
	                    // Too much flicker with 8 bits. We should have a separate screen pass
	                    // with this bit plane. Or interlace. Or trick with -OE switch on in the
	                    // middle of row-clocking, thus have RowClockTime / 2
	                    (128 * RowClockTime) - RowClockTime, // too much flicker.
                    };
            display = new LedDisplay(pwmbits, Width);
            clearDisplay();
        }

        public void Flush(int x, int y, int wx, int wy)
        {
            Session.Flush();
            Color[] lc = target.GetPixelColors();
            
            for (int i = 0; i < lc.Length; i++)
            {
                int posx = i % Width;
                int posy = i / Width;
                if(posx>=x && posx<=x+wx)
                {
                    if(posy>=y && posy<=y+wy)
                    {
                        drawPixel(posx, posy, lc[i]);
                    }
                }            
                
            }
        }

        public void clearDisplay()
        {
            //foreach (DisplayRow row in display.colormatrix)
            //{
            //    for (int i = 0; i < row.color1.Length; i++)
            //    {
            //        row.color1[i].A = 0;
            //        row.color1[i].R = 0;
            //        row.color1[i].G = 0;
            //        row.color1[i].B = 0;

            //        row.color2[i].A = 0;
            //        row.color2[i].R = 0;
            //        row.color2[i].G = 0;
            //        row.color2[i].B = 0;
            //    }
            //}
        }

        public void drawPixel(int x, int y, Color c)
        {
            if (y > 31)
            {
                x = 127 - x;
                y = 63 - y;
            }

            int red = c.R;
            int green = c.G;
            int blue = c.B;

            red >>= 8 - pwmbits;
            green >>= 8 - pwmbits;
            blue >>= 8 - pwmbits;

            for (int b = 0; b < pwmbits; b++)
            {
                int mask = 1 << b;
                if (y < 16)
                {
                    // Upper sub-panel
                    display.planes[b].colormatrix[y].color1[x].A = c.A;
                    display.planes[b].colormatrix[y].color1[x].R = (byte)((red & mask) == mask ? 255 : 0);
                    display.planes[b].colormatrix[y].color1[x].G = (byte)((green & mask) == mask ? 255 : 0);
                    display.planes[b].colormatrix[y].color1[x].B = (byte)((blue & mask) == mask ? 255 : 0);

                }
                else
                {
                    // Lower sub-panel
                    display.planes[b].colormatrix[y - 16].color2[x].A = c.A;
                    display.planes[b].colormatrix[y - 16].color2[x].R = (byte)((red & mask) == mask ? 255 : 0);
                    display.planes[b].colormatrix[y - 16].color2[x].G = (byte)((green & mask) == mask ? 255 : 0);
                    display.planes[b].colormatrix[y - 16].color2[x].B = (byte)((blue & mask) == mask ? 255 : 0);
                }
            }

        }
        int pwmi = 0;
        int row = 0;


        // Write pixels to the LED panel.
        public void updateDisplay(Windows.Foundation.IAsyncAction action)
        {
            while (true)
            {
                //MicroStopwatch sw;
                //sw = new MicroStopwatch();
                GpioPinValue lastr1 = GpioPinValue.Low;
                GpioPinValue lastg1 = GpioPinValue.Low;
                GpioPinValue lastb1 = GpioPinValue.Low;
                GpioPinValue lastr2 = GpioPinValue.Low;
                GpioPinValue lastg2 = GpioPinValue.Low;
                GpioPinValue lastb2 = GpioPinValue.Low;
                for (int a = 0; a < 2; a++)
                {
                    for (int row = a; row < RowsPerSubPanel; row += 2)
                    {
                        for (int pwmi = 0; pwmi < pwmbits; pwmi++)
                        {
                            // Rows can't be switched very quickly without ghosting, so we do the
                            // full PWM of one row before switching rows.
                            //printf("Row %d",row);
                            //sw.Reset();
                            //sw.Start();
                            DisplayRow myrow = display.planes[pwmi].colormatrix[row];

                            //Task.Delay(TimeSpan.FromTicks(2000));
                            // Clock in the row. The time this takes is the smallest time we can
                            // leave the LEDs on, thus the smallest time-constant we can use for
                            // PWM (doubling the sleep time with each bit).
                            // So this is the critical path; I'd love to know if we can employ some
                            // DMA techniques to speed this up.
                            // (With this code, one row roughly takes 3.0 - 3.4usec to clock in).
                            //
                            // However, in particular for longer chaining, it seems we need some more
                            // wait time to settle.
                            //long StabilizeWaitNanos = 625; //TODO: mateo was 256
                            for (int col = 0; col < Width; col++) //TODO: Columncount
                            {
                                //proxy.setRGB(GpioPinValue.Low, GpioPinValue.Low, GpioPinValue.Low, GpioPinValue.Low, GpioPinValue.Low, GpioPinValue.Low);



                                //sleepNanos(StabilizeWaitNanos);
                                //_gpio->setBits(out,0); //serialmask
                                //proxy.setRGB(myrow.color1[col].R == 0 ? GpioPinValue.Low : GpioPinValue.High,
                                //    myrow.color1[col].G == 0 ? GpioPinValue.Low : GpioPinValue.High,
                                //    myrow.color1[col].B == 0 ? GpioPinValue.Low : GpioPinValue.High,
                                //    myrow.color2[col].R == 0 ? GpioPinValue.Low : GpioPinValue.High,
                                //    myrow.color2[col].G == 0 ? GpioPinValue.Low : GpioPinValue.High,
                                //    myrow.color2[col].B == 0 ? GpioPinValue.Low : GpioPinValue.High);
                                GpioPinValue _r1 = myrow.color1[col].R == 0 ? GpioPinValue.Low : GpioPinValue.High;
                                GpioPinValue _g1 = myrow.color1[col].G == 0 ? GpioPinValue.Low : GpioPinValue.High;
                                GpioPinValue _b1 = myrow.color1[col].B == 0 ? GpioPinValue.Low : GpioPinValue.High;
                                GpioPinValue _r2 = myrow.color2[col].R == 0 ? GpioPinValue.Low : GpioPinValue.High;
                                GpioPinValue _g2 = myrow.color2[col].G == 0 ? GpioPinValue.Low : GpioPinValue.High;
                                GpioPinValue _b2 = myrow.color2[col].B == 0 ? GpioPinValue.Low : GpioPinValue.High;

                                if (lastr1 != _r1)
                                {
                                    proxy.r1.Write(_r1);
                                    lastr1 = _r1;
                                }
                                if (lastg1 != _g1)
                                {
                                    proxy.g1.Write(_g1);
                                    lastg1 = _g1;
                                }
                                //if (lastb1 != _b1)
                                //{
                                //    b1.Write(_b1);
                                //    lastb1 = _b1;
                                //}
                                if (lastr2 != _r2)
                                {
                                    proxy.r2.Write(_r2);
                                    lastr2 = _r2;
                                }
                                if (lastg2 != _g2)
                                {
                                    proxy.g2.Write(_g2);
                                    lastg2 = _g2;
                                }
                                //if (lastb2 != _b2)
                                //{
                                //    b2.Write(_b2);
                                //    lastb2 = _b2;
                                //}


                                //_gpio->setBits(clock, 2); //nomask
                                proxy.clock.Write(GpioPinValue.High);
                                proxy.clock.Write(GpioPinValue.Low);
                                //makeSleep(100);
                                //Task.Delay(TimeSpan.FromTicks(100));
                            }



                            proxy.setRowAddress(row);

                            proxy.latch.Write(GpioPinValue.High);
                            proxy.latch.Write(GpioPinValue.Low);
                            //sw.Stop();
                            //long elapsed = sw.ElapsedTicks;
                            //if(elapsed<50000)
                            //{
                            //    for(int i = 0; i< 20;i++)
                            //    {

                            //    }
                            //}
                            proxy.outputEnabled.Write(GpioPinValue.High);
                            proxy.outputEnabled.Write(GpioPinValue.Low);
                           
                            //sw.Start();

                            //while (sw.ElapsedMicroseconds <= RowClockTime)
                            //{

                            //}

                            //sw.Stop();
                            //sw.Reset();

                            //makeSleep(RowClockTime);
                            //await Task.Delay(TimeSpan.FromMilliseconds(1));
                        }
                    }
                }
            }
        }



        public void setTextCursor(int x, int y)
        {
            _textCursorX = x;
            _textCursorY = y;
        }

        public void setFontColor(Color color)
        {
            _fontColor = color;
        }

        public void setFontSize(int size)
        {
            _fontSize = (size >= 3) ? 3 : size; //only 3 sizes for now

            if (_fontSize == 1)
            {
                _fontWidth = 3;
                _fontHeight = 5;
            }
            else if (_fontSize == 2) //medium (4x6)
            {
                _fontWidth = 4;
                _fontHeight = 6;
            }
            else if (_fontSize == 3) //large (5x7)
            {
                _fontWidth = 5;
                _fontHeight = 7;
            }
        }

        public void setWordWrap(bool wrap)
        {
            _wordWrap = wrap;
        }

        // Write a character using the Text cursor and stored Font settings.
        public void writeChar(char c)
        {
            if (c == '\n')
            {
                _textCursorX = 0;
                _textCursorY += _fontHeight;
            }
            else if (c == '\r')
            {
                ; //ignore
            }
            else
            {
                putChar(_textCursorX, _textCursorY, c, _fontSize, _fontColor);

                _textCursorX += _fontWidth + 1;

                if (_wordWrap && (_textCursorX > (Width - _fontWidth)))
                {
                    _textCursorX = 0;
                    _textCursorY += _fontHeight + 1;
                }
            }
        }

        // Put a character on the display using glcd fonts.
        public void putChar(int x, int y, char c, int size,
            Color color)
        {
            byte[] font = Font5x7;
            int fontWidth = 5;
            int fontHeight = 7;

            if (size == 1) //small (3x5)
            {
                font = Font3x5;
                fontWidth = 3;
                fontHeight = 5;
            }
            else if (size == 2) //medium (4x6)
            {
                font = Font4x6;
                fontWidth = 4;
                fontHeight = 6;
            }
            else if (size == 3) //large (5x7)
            {
                ; //already initialized as default
            }

            for (int i = 0; i < fontWidth + 1; i++)
            {
                int line;

                if (i == fontWidth)
                {
                    line = 0x0;
                }
                else
                {
                    line = font[((int)c - 0x20) * fontWidth + i];
                    //line = pgm_read_byte(font + ((c - 0x20) * fontWidth) + i);
                }

                for (int j = 0; j < fontHeight + 1; j++)
                {
                    if ((line & 0x1) == 1)
                    {
                        drawPixel(x + i, y + j, color);
                    }

                    line >>= 1;
                }
            }
        }

        // Clear the inside of the given Rectangle. 
        public void clearRect(int fx, int fy, int fw, int fh)
        {
            int maxX, maxY;
            maxX = (fx + fw) > Width ? Width : (fx + fw);
            maxY = (fy + fh) > Height ? Height : (fy + fh);

            for (int b = pwmbits - 1; b >= 0; b--)
            {
                for (int x = fx; x < maxX; x++)
                {
                    for (int y = fy; y < maxY; y++)
                    {


                        if (y < 16)
                        {
                            // Upper sub-panel
                            display.planes[b].colormatrix[y].color1[x].R = 0;
                            display.planes[b].colormatrix[y].color1[x].G = 0;
                            display.planes[b].colormatrix[y].color1[x].B = 0;

                        }
                        else
                        {
                            // Lower sub-panel
                            display.planes[b].colormatrix[y - 16].color2[x].R = 0;
                            display.planes[b].colormatrix[y - 16].color2[x].G = 0;
                            display.planes[b].colormatrix[y - 16].color2[x].B = 0;
                        }
                    }
                }
            }
        }
    }
}

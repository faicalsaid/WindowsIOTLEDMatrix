using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace LedMatrixEngineSharp
{
    class LedDisplay
    {
        private int Width;
        public Plane[] planes;
        public LedDisplay(int pwmbits,int width)
        {
            Width = width;
            planes = new Plane[pwmbits];
            for (int j = 0; j < pwmbits; j++)
            {
                planes[j] = new Plane();
                for (int i = 0; i < 16; i++)
                {
                    planes[j].colormatrix[i] = new DisplayRow(i, width);
                }
            }
        }
    }
    public class DisplayRow
    {
        RowAddress raddress;
        public Color[] color1 { get; set; }
        public Color[] color2 { get; set; }

        public DisplayRow(int address, int width)
        {
            raddress = new RowAddress(address);
            color1 = new Color[width];
            color2 = new Color[width];
            
        }
    }
    public class Plane
    {
        public DisplayRow[] colormatrix { get; set; }
        public Plane()
        {
            colormatrix = new DisplayRow[16];
        }
    }
    public class RowAddress
    {
        public bool A { get; set; }
        public bool B { get; set; }
        public bool C { get; set; }
        public bool D { get; set; }

        public RowAddress(int address)
        {
            A = (address & 1) == 1;
            B = (address & 2) == 2;
            C = (address & 4) == 4;
            D = (address & 8) == 8;

        }
    }
}

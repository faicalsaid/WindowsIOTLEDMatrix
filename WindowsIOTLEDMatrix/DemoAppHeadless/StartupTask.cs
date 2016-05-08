using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using LedMatrixEngineSharp;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace DemoAppHeadless
{
    public sealed class StartupTask : IBackgroundTask
    {
        //This is a headless app for Win IOT

        //Led Matrix
        RgbMatrix matrix;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //initialize the led matrix
            matrix = new RgbMatrix();
            //Don't need await
            Windows.System.Threading.ThreadPool.RunAsync(matrix.updateDisplay, Windows.System.Threading.WorkItemPriority.High);
            //draw on the led matrix
            drawSomething();
        }

        private void drawSomething()
        {
            //clear the matrix
            matrix.Session.Clear(Color.FromArgb(255, 0, 0, 0));
            CanvasTextFormat ff = new CanvasTextFormat();
            ff.FontSize = 16;
            ff.FontFamily = "Courier New";
            ff.HorizontalAlignment = CanvasHorizontalAlignment.Center;
            //write hello world
            matrix.Session.DrawText("Hello World!", 0, 0, 128, 16, Color.FromArgb(255,255,0,0), ff);
            //draw a circle
            matrix.Session.DrawCircle(54, 16, 10, Color.FromArgb(255, 0, 255, 0), 1);

            //flush the win2d to the led matrix for the specified rectangle 
            matrix.Flush(0, 0, 128, 32);
        }
    }
}

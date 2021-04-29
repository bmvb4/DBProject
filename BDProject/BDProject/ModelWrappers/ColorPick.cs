using SkiaSharp;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text;

namespace BDProject.ModelWrappers
{
    public class ColorPick
    {
        public ColorPick(string hex)
        {
            Color = Color.FromHex(hex);
        }

        public Color Color { get; set; }
    }
}

using OurPresence.Modeller.Domain;
using System.Collections.Generic;
using Terminal.Gui;

namespace OurPresence.Modeller.Cli
{
    public static class Windows
    {
        public static Toplevel ModuleWindow()
        {
            var moduleWindow = new Toplevel
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            return moduleWindow;
        }

        public static Window ModelWindow()
        {
            var win = new Window("Model")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };

            List<Field> _fields = new();

            return win;
        }
    }
}

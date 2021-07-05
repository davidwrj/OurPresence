// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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

            List<Field> fields = new();

            return win;
        }
    }
}

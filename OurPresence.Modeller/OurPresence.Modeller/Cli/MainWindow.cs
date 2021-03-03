using OurPresence.Modeller.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terminal.Gui;

namespace OurPresence.Modeller.Cli
{
    public static class Windows
    {
        public static Toplevel ModuleWindow()
        {
            var moduleWindow = new Toplevel()
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

            List<Field> _fields = new List<Field>();

            return win;
        }

        //public class ModelWindow : PopupWindow
        //{

        //    public ModelWindow(Window parentWindow)
        //        : base("Model", 8, 10, 80, 20, parentWindow)
        //    {
        //        BackgroundColour = ConsoleColor.Gray;

        //        var labelModel = new Label("Model", 10, 12, "lebelModel", this);
        //        var textModel = new TextBox(10, 32, "textModel", this, 50);

        //        var addField = new Button(10, 12, "Add Field", "addField", this)
        //        {
        //            Action = delegate ()
        //            {
        //                var w = new FieldWindow(this);
        //                if (w.Field != null)
        //                    _fields.Add(w.Field);
        //            }
        //        };
        //        var exitModel = new Button(10, 82, "Exit", "exitModel", this) { Action = delegate () { ExitWindow(); } };

        //        Inputs.Add(labelModel);
        //        Inputs.Add(textModel);

        //        Inputs.Add(addField);
        //        Inputs.Add(exitModel);

        //        SelectFirstItem();

        //        Draw();
        //        MainLoop();
        //    }
        //}

        //public class FieldWindow : PopupWindow
        //{
        //    public FieldWindow(Window parentWindow)
        //        : base("Field", 11, 12, 80, 20, parentWindow)
        //    {
        //        BackgroundColour = ConsoleColor.White;

        //        var labelField = new Label("Field", 13, 14, "labelField", this);
        //        var textField = new TextBox(13, 25, "textField", this, 50);
        //        var labelNullable = new Label("Nullable", 14, 14, "labelNullable", this);
        //        var checkNullable = new CheckBox(14, 25, "checkNullable", this);
        //        var labelDataType = new Label("Data Type", 15, 14, "labelDataType", this);
        //        var comboDataType = new Dropdown(15, 25, Enum.GetNames(typeof(DataTypes)), "comboDataType", this) { };

        //        var exitField = new Button(28, 84, "Exit", "exitField", this)
        //        {
        //            Action = delegate ()
        //            {
        //                ExitWindow();
        //            }
        //        };

        //        Inputs.Add(labelField);
        //        Inputs.Add(textField);
        //        Inputs.Add(labelNullable);
        //        Inputs.Add(checkNullable);
        //        Inputs.Add(labelDataType);
        //        Inputs.Add(comboDataType);

        //        Inputs.Add(exitField);

        //        CurrentlySelected = textField;

        //        Draw();
        //        MainLoop();

        //        if (!Enum.TryParse<DataTypes>(comboDataType.Text, out var dataType))
        //            return;

        //        Field = new Field(textField.GetText(), dataType: dataType, checkNullable.Checked);
        //    }

        //public Domain.Field Field { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FormDesign
{
    internal class VariablesP
    {
        internal static Point pointA;
        internal static Point pointB;
        internal static Size sizePercent;
        internal static Size size;
        internal static Point zeroPoint = new Point(0, 0);
    }

    public class Metodos
    {
        static int DistDosPuntos(int a, int b)
        {
            if (a >= b)
            {
                return a - b;
            }
            else
            {
                return b - a;
            }
        }

        static Point GetPointPercent(int percentX, int percentY, Size parentSize)
        {
            int x = parentSize.Width * percentX / 100;
            int y = parentSize.Height * percentY / 100;
            Point point = new Point(x, y);
            return point;
        }

        public  static void SetSizePixels(Size porcentaje, Size parentSize)
        {
            int width = (int)(parentSize.Width * porcentaje.Width / 100);
            int height = (int)(parentSize.Height * porcentaje.Height / 100);
            VariablesP.size = new Size(width, height);
        }

        public static void SetPercentPointVar(Point a, Point b, Size parentSize)
        {
            VariablesP.pointA = GetPointPercent(a.X,a.Y, parentSize);
            VariablesP.pointB = GetPointPercent(b.X, b.Y, parentSize);
        }

        public static void SetSizeVar(Point a, Point b, Size parentSize)
        {
            int heightPercent = DistDosPuntos(a.Y, b.Y);
            int widthPercent = DistDosPuntos(a.X, b.X);
            VariablesP.sizePercent = new Size(widthPercent, heightPercent);
            SetSizePixels(VariablesP.sizePercent, parentSize);
        }

        public static void SetChangePosPercVar(Point a, Point b, Size parentSize)
        {
            SetPercentPointVar(a, b, parentSize);
            SetSizeVar(a, b, parentSize);
        }

        public static Font TxtAdjustByHeight(Size objSize, Font font, float x)
        {
            float i = objSize.Height / x;
            Font r = new Font(font.FontFamily, i);
            return r;
        }

        public static Point GetLocation(string position, Size objSize, Size parentSize)
        {
            Point pos = new Point();

            switch (position)
            {
                case "topLeft": //Arriba Izquierda
                    pos.X = 0;
                    pos.Y = 0;
                    break;
                case "topRight": //Arriba Derecha
                    pos.X = parentSize.Width - objSize.Width;
                    pos.Y = 0;
                    break;
                case "bottomLeft": //Abajo Izquierda
                    pos.X = 0;
                    pos.Y = parentSize.Height - objSize.Height;
                    break;
                case "bottomRight": //Abajo Derecha
                    pos.X = parentSize.Width - objSize.Width;
                    pos.Y = parentSize.Height - objSize.Height;
                    break;
                case "top": //Arriba
                    pos.X = (parentSize.Width - objSize.Width) / 2;
                    pos.Y = 0; 
                    break;
                case "left": //Izquierda
                    pos.X = 0;
                    pos.Y = (parentSize.Height - objSize.Height) / 2;
                    break;
                case "right": //Derecha
                    pos.X = parentSize.Width - objSize.Width;
                    pos.Y = (parentSize.Height - objSize.Height) / 2;
                    break;
                case "bottom": //Abajo
                    pos.X = (parentSize.Width - objSize.Width) / 2;
                    pos.Y = parentSize.Height - objSize.Height;
                    break;
                default: //Centro
                    pos.X = (parentSize.Width - objSize.Width) / 2;
                    pos.Y = (parentSize.Height - objSize.Height) / 2;
                    break;
            }
            return pos;
        }

        public static Point GetLocationScreen(int screenNumb)
        {
            Screen screen = Screen.AllScreens[screenNumb];
            Point newPoint = screen.Bounds.Location;
            return newPoint;
        }

        public static Point GetMovedPointScreen(Point point, int screenNumb)
        {
            Point screen = GetLocationScreen(screenNumb);
            Point result = new Point(screen.X + point.X, screen.Y + point.Y);
            return result;
        }

    }

    public class Percent
    {
        public static void SizePosition(Point a, Point b, Control obj)
        {
            SizeAdju.Percent(a, b, obj);
            LocationAdju.Percent(a, b, obj);
        }

        public static void SizePositionText(Point a, Point b, Control obj, float txtSize = 2)
        {
            SizeAdju.PercentText(a, b, obj, txtSize);
            LocationAdju.Percent(a, b, obj);
        }

        public static void Flex(Panel[] panels, int[] numb, [Optional] Control parent, [Optional] string direction)
        {
            try
            {
                int sumNumb = numb.Sum();
                if (panels.Length < numb.Length || numb.Length < panels.Length)
                {
                    MessageBox.Show("Flex -- Diference in array lenght \n panels.lenght = " + panels.Length.ToString() + " \n int.lenght = " + numb.Length.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
                bool parentBool;
                if (parent == null)
                {
                    parentBool = true;
                    parent = panels[0].Parent;
                }
                else
                {
                    parentBool = false;
                }

                int i = 0;
                int lastPoint = 0;
                Point a, b;
                foreach (Panel x in panels)
                {
                    if (parentBool)
                    {
                        x.Parent = parent;
                    }

                    int percentPanels = (numb[i] * 100) / sumNumb;
                    int z = percentPanels + lastPoint;
                    if(i+1 == panels.Length)
                    {
                        z = 100;
                    }
                    if (direction == "row")
                    {
                        a = new Point(lastPoint, 0);
                        b = new Point(z, 100);
                        SizePosition(a, b, x);
                    }
                    else
                    {
                        a = new Point(0, lastPoint);
                        b = new Point(100, z);
                        SizePosition(a, b, x);
                    }
                    //MessageBox.Show("percent: " + percentPanels + " \n Point a: " + a + " \n Point b: " + b);
                    lastPoint += percentPanels;
                    i++;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in FormDesignMain", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        //-------------------------------------
        public static void SizePosition(int a1, int a2, int b1, int b2, Control obj)
        {
            SizeAdju.Percent(new Point(a1, a2), new Point(b1, b2), obj);
            LocationAdju.Percent(new Point(a1, a2), new Point(b1, b2), obj);
        }

        public static void SizePositionText(int a1, int a2, int b1, int b2, Control obj, float txtSize = 2)
        {
            SizeAdju.PercentText(new Point(a1, a2), new Point(b1, b2), obj, txtSize);
            LocationAdju.Percent(new Point(a1, a2), new Point(b1, b2), obj);
        }
        //-------------------------------------
    }

    public class SizeAdju
    {
        public static void Percent(Size size, Form obj, Size parentSize)
        {
            Metodos.SetSizePixels(size, parentSize);
            obj.Size = VariablesP.size;
        }

        public static void Percent(Point a, Point b, Control obj)
        {
            Metodos.SetSizeVar(a, b, obj.Parent.Size);
            obj.Size = VariablesP.size;
        }

        public static void PercentText(Point a, Point b, Control obj, float txtSize = 2)
        {
            Metodos.SetSizeVar(a, b, obj.Parent.Size);
            obj.Size = VariablesP.size;
            obj.Font = Metodos.TxtAdjustByHeight(obj.Size, obj.Font, txtSize);
        }

        //-------------------------------------

    }

    public class LocationAdju
    {
        public static void Percent(Point a, Point b, Control obj)
        {
            Metodos.SetPercentPointVar(a, b, obj.Parent.Size);
            obj.Location = VariablesP.pointA;
        }

        public static void Percent(Point a, Point b, Form obj, Size parentSize)
        {
            Metodos.SetPercentPointVar(a, b, obj.Parent.Size);
            obj.Location = VariablesP.pointA;
        }

        public static void Set(string position, Size parentSize, Form obj)
        {
            Point point = Metodos.GetLocation(position, obj.Size, parentSize);
            obj.StartPosition = FormStartPosition.Manual;
            obj.Location = point;
        }
        public static void Set(string position, Size parentSize, Control obj)
        {
            Point point = Metodos.GetLocation(position, obj.Size, parentSize);
            obj.Location = point;
        }

        public static void SetScreen(string position, Size parentSize, Form obj, int screen)
        {
            Point point = Metodos.GetLocation(position, obj.Size, parentSize);
            obj.StartPosition = FormStartPosition.Manual;
            obj.Location = point;
            Metodos.GetMovedPointScreen(obj.Location, screen);

        }
    }
}

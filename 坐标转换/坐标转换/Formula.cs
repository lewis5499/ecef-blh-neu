using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CoordinateConversion_and_CoordinateSystemConversion
{
    class Formula
    {
        //WGS84椭球常量
        const double a = 6378137.0;
        const double f = 1.0 / 298.257223563;
        const double b = a - f * a;
        const double epsilon = 0.000000000000001;
        const double pi = 3.14159265358979323846;
        const double d2r = pi / 180;
        const double r2d = 180 / pi;
        static readonly double e = Math.Sqrt(a * a - b * b) / a;

        //数据读取与显示
        public void Readfile(out List<XYZPoint> Xpoint)
        {
            Xpoint = new List<XYZPoint>();

            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = Application.StartupPath;
            file.RestoreDirectory = true;
            file.Filter = "All Files(*.*)|*.*|Dat Files(*.dat)|*.dat|Text Files(*.txt)|*.txt";
            file.FilterIndex = 1;

            if (file.ShowDialog() == DialogResult.OK)
            {
                XYZPoint p;

                var reader = new StreamReader(file.FileName);

                string str = reader.ReadLine();
                //str = reader.ReadLine();
                var arr = str.Split(' ');

                while (str != null)
                {
                    arr = str.Split(',');
                    p = new XYZPoint();
                    p.name = arr[0];
                    p.X = double.Parse(arr[1]);
                    p.Y = double.Parse(arr[2]);
                    p.Z = double.Parse(arr[3]);
                    Xpoint.Add(p);

                    str = reader.ReadLine();
                }
                reader.Close();
            }
        }

        //XYZ转换为BLH
        public void xyz2blh(List<XYZPoint> Xpoint, out List<BLHPoint> Bpoint)
        {
            BLHPoint pp;
            Bpoint = new List<BLHPoint>();

            foreach (XYZPoint p in Xpoint)
            {
                double L1 = Math.Atan2(p.Y , p.X);

                double tmpX = p.X;
                double temY = p.Y;
                double temZ = p.Z;

                double curB = 0;
                double N = 0;
                double calB = Math.Atan2(temZ, Math.Sqrt(tmpX * tmpX + temY * temY));

                int counter = 0;
                while (Math.Abs(curB - calB) * r2d > epsilon && counter < 25)
                {
                    curB = calB;
                    N = a / Math.Sqrt(1 - e * e * Math.Sin(curB) * Math.Sin(curB));
                    calB = Math.Atan2(temZ + N * e * e * Math.Sin(curB), Math.Sqrt(tmpX * tmpX + temY * temY));
                    counter++;
                }

                pp = new BLHPoint();
                pp.name = p.name;              
                pp.L = Math.Atan2(temY, tmpX) * r2d;
                pp.B = curB * r2d;
                pp.H = temZ / Math.Sin(curB) - N * (1 - e * e);

                Bpoint.Add(pp);
            }
        }

        //XYZ经过第[num]点BLH转换为NEU
        public void blh2neu(List<XYZPoint> Xpoint, List<BLHPoint> Bpoint, int num, out List<NEUPoint> Npoint)
        {
            Npoint = new List<NEUPoint>();
            NEUPoint pp = new NEUPoint();

            double B1 = Bpoint[num-1].B/180*pi;//转为弧度
            double L1 = Bpoint[num-1].L/180*pi;
            double X1 = Xpoint[num-1].X;
            double Y1 = Xpoint[num-1].Y;
            double Z1 = Xpoint[num-1].Z;

            foreach (XYZPoint p in Xpoint)
            {
                pp = new NEUPoint();

                //构造矩阵
                double[,] deta = new double[3, 1];

                deta[0, 0] = p.X - X1;
                deta[1, 0] = p.Y - Y1;
                deta[2, 0] = p.Z - Z1;

                //构造T矩阵
                double[,] T = new double[3, 3];
                T[0, 0] = -Math.Sin(B1) * Math.Cos(L1);
                T[0, 1] = -Math.Sin(B1) * Math.Sin(L1);
                T[0, 2] = Math.Cos(B1);
                T[1, 0] = -Math.Sin(L1);
                T[1, 1] = Math.Cos(L1);
                T[1, 2] = 0;
                T[2, 0] = Math.Cos(B1) * Math.Cos(L1);
                T[2, 1] = Math.Cos(B1) * Math.Sin(L1);
                T[2, 2] = Math.Sin(B1);
                //矩阵相乘
                double[,] tmp =Elevation_adjust.matrix.matMul(T, deta);

                pp.name = p.name;
                pp.N = tmp[0, 0];
                pp.E = tmp[1, 0];
                pp.U = tmp[2, 0];

                Npoint.Add(pp);
            }
        }

        //撰写报告
        public void Report(List<XYZPoint> Xpoint, List<BLHPoint> Bpoint, int num, List<NEUPoint> Npoint, out string report1,out string report2,out string report3)
        {
            report1 = null;
            report2 = null;
            report3 = null;

            report1 += "**Space Rectangular Coordinate System**\nName            X                    Y                   Z\n";
            foreach (XYZPoint p in Xpoint)
            {
                report1 += String.Format("{0,6},{1,15},{2,15},{3,15}", p.name, p.X.ToString("f5"), p.Y.ToString("f5"), p.Z.ToString("f5")) + "\n";
            }
            report2 += "*******Geodetic Coordinate System*******\nName           B                     L                   H\n";
            foreach (BLHPoint p in Bpoint)
            {
                report2 += String.Format("{0,6},{1,15},{2,15},{3,15}", p.name, p.B.ToString("f5"), p.L.ToString("f5"), p.H.ToString("f5")) + "\n";
            }
            report3 += "******Topocentric Coordinate System******\n";
            report3 += "以第" + num.ToString() + "点为站心！\nName           N                     E                   U\n";
            foreach (NEUPoint p in Npoint)
            {
                report3 += String.Format("{0,6},{1,15},{2,15},{3,15}", p.name, p.N.ToString("f5"), p.E.ToString("f5"), p.U.ToString("f5")) + "\n";
            }
        }
    }
}
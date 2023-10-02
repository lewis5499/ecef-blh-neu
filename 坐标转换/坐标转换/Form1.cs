using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoordinateConversion_and_CoordinateSystemConversion
{
    public partial class Form1 : Form
    {
        //七参数
        List<string> list_Name07 = new List<string>();
        List<string> list_Name7 = new List<string>();
        List<double> list_X07 = new List<double>();
        List<double> list_Y07 = new List<double>();
        List<double> list_Z07 = new List<double>();
        List<double> list_X7 = new List<double>();
        List<double> list_Y7 = new List<double>();
        List<double> list_Z7 = new List<double>();
        string Data7 = "";

        //四参数
        List<string> list_Name04 = new List<string>();
        List<string> list_Name4 = new List<string>();
        List<double> list_X04 = new List<double>();
        List<double> list_Y04 = new List<double>();
        List<double> list_Z04 = new List<double>();
        List<double> list_X4 = new List<double>();
        List<double> list_Y4 = new List<double>();
        List<double> list_Z4 = new List<double>();
        string Data4 = "";

        bool j = false;
        bool j0 = false;

        //XYZ2BLHNEU
        Formula formula = new Formula();
        bool jj = false;
        bool j1 = false;
        List<XYZPoint> Xpoint;
        List<BLHPoint> Bpoint;
        List<NEUPoint> Npoint;

        string report1;
        string report2;
        string report3;

        public Form1()
        {
            InitializeComponent();
            richTextBox5.Text = "References:\n";
            richTextBox5.Text += "四参数坐标转换（小角度）\n编程说明：\n XYZ_origin_1.xyz中为源坐标系坐标，XYZ_target_1.xyz为目标坐标系坐标，共8个点，要求以前5个点作为转换公共点，后3个点作为检核点。程序应输出：求取的四参数（单位无需转换），条件数n，冗余度r，公共点内符合指标（验后单位权中误差、每个公共点坐标残差），检核点外符合指标（检核点坐标残差）。\n";
            richTextBox5.Text += "另：将最后一个点（小数点后三位替换为学号后三位）转换后的坐标结果输出。\n\n";
            richTextBox5.Text += "七参数/六参数坐标转换（小角度）\n编程说明：\n XYZ_origin_2.xyz中为源坐标系坐标，XYZ_target_2.xyz为目标坐标系坐标，共6个点，要求以前4个点作为转换公共点，后2个点作为检核点。分别输出程序应输出求取的七参数（单位无需转换），条件数n，冗余度r，公共点内符合指标（验后单位权中误差、每个公共点坐标残差），检核点外符合指标（检核点坐标残差）。\n";
            richTextBox5.Text += "另：将最后一个点（小数点后三位替换为学号后三位）转换后的坐标结果输出。\n\n";
            richTextBox5.Text += "地心地固系与大地坐标系转换\n使用到的文件：XYZ2BLHNEU.xyz，使用WGS84椭球。\n编程说明：\n文件中共14435个点，要求全部转为大地坐标系，对转换后的14435个大地经纬度点，分别绘制平面（经度-纬度）、高程散点图，给出该图是何应用场景的猜想。截取转换后的前10个点大地经纬度结果放入报告。\n另外生成的14435个大地坐标系结果存成文件上交电子文档。\n\n";
            richTextBox5.Text += "地心地固系与站心坐标系转换\n使用到的文件：XYZ2BLHNEU.xyz，使用WGS84椭球。\n编程说明：\n文件中共14435个点，要求全部转为以第一个点为站心的站心坐标系，对转换后的14435点，分别绘制平面（N - E）、高程散点图，给出该图是何应用场景的猜想。截取转换后的前10个点站心坐标系结果放入报告。\n另外生成的14435个站心坐标系结果存成文件上交电子文档。\n\n";
            richTextBox5.Text += "©2022 Hengzhen Liu. All Rights Reserved.\n\n";
        }//PreLoad

        private void button4_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            list_Name04.Clear();
            list_Name07.Clear();
            list_Name4.Clear();
            list_Name7.Clear();
            list_X04.Clear();
            list_X07.Clear();
            list_X4.Clear();
            list_X7.Clear();
            list_Y04.Clear();
            list_Y07.Clear();
            list_Y4.Clear();
            list_Y7.Clear();
            list_Z04.Clear();
            list_Z07.Clear();
            list_Z4.Clear();
            list_Z7.Clear();
            j = false;
            j0 = false;
        }//Clear

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)//七参数
            {
                OpenFileDialog file = new OpenFileDialog();
                file.InitialDirectory = Application.StartupPath;
                file.RestoreDirectory = true;
                file.Filter = "All Files(*.*)|*.*|Dat Files(*.dat)|*.dat|Text Files(*.txt)|*.txt";
                file.FilterIndex = 1;
                if (file.ShowDialog() == DialogResult.OK)//文件选择对话
                {
                    string str1 = "";

                    var list_Name = new List<string>();
                    var list_X0 = new List<double>();
                    var list_Y0 = new List<double>();
                    var list_Z0 = new List<double>();

                    var Buffer = new StreamReader(file.FileName);
                    try
                    {
                        string Container = string.Empty;
                        while (!Buffer.EndOfStream)
                        {
                            Container = Buffer.ReadLine();
                            str1 += Container + "\n";
                            var arr = Container.Split(',');

                            list_Name.Add(arr[0]);
                            list_X0.Add(Convert.ToDouble(arr[1]));
                            list_Y0.Add(Convert.ToDouble(arr[2]));
                            list_Z0.Add(Convert.ToDouble(arr[3]));
                        }
                        Buffer.Close();//关闭流释放内存
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    richTextBox1.Text = str1;
                    for (int i = 0; i < list_Name.Count; i++)//Copy
                    {
                        list_Name07.Add(list_Name[i]);
                        list_X07.Add(list_X0[i]);
                        list_Y07.Add(list_Y0[i]);
                        list_Z07.Add(list_Z0[i]);
                    }
                }
            }
            else if (radioButton1.Checked == true)//四参数
            {
                OpenFileDialog file = new OpenFileDialog();
                file.InitialDirectory = Application.StartupPath;
                file.RestoreDirectory = true;
                file.Filter = "All Files(*.*)|*.*|Dat Files(*.dat)|*.dat|Text Files(*.txt)|*.txt";
                file.FilterIndex = 1;
                if (file.ShowDialog() == DialogResult.OK)//文件选择对话
                {
                    string str1 = "";

                    var list_Name = new List<string>();
                    var list_X0 = new List<double>();
                    var list_Y0 = new List<double>();
                    var list_Z0 = new List<double>();

                    var Buffer = new StreamReader(file.FileName);
                    try
                    {
                        string Container = string.Empty;
                        while (!Buffer.EndOfStream)
                        {
                            Container = Buffer.ReadLine();
                            str1 += Container + "\n";
                            var arr = Container.Split(',');

                            list_Name.Add(arr[0]);
                            list_X0.Add(Convert.ToDouble(arr[1]));
                            list_Y0.Add(Convert.ToDouble(arr[2]));
                            list_Z0.Add(Convert.ToDouble(arr[3]));
                        }
                        Buffer.Close();//关闭流释放内存
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    richTextBox1.Text = str1;
                    for (int i = 0; i < list_Name.Count; i++)//Copy
                    {
                        list_Name04.Add(list_Name[i]);
                        list_X04.Add(list_X0[i]);
                        list_Y04.Add(list_Y0[i]);
                        list_Z04.Add(list_Z0[i]);
                    }
                }
            }
        }//打开原始文件xyz_Origin

        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)//七参数
            {
                OpenFileDialog file = new OpenFileDialog();
                file.InitialDirectory = Application.StartupPath;
                file.RestoreDirectory = true;
                file.Filter = "All Files(*.*)|*.*|Dat Files(*.dat)|*.dat|Text Files(*.txt)|*.txt";
                file.FilterIndex = 1;
                if (file.ShowDialog() == DialogResult.OK)//文件选择对话
                {
                    string str2 = "";

                    var list_Name = new List<string>();
                    var list_X = new List<double>();
                    var list_Y = new List<double>();
                    var list_Z = new List<double>();

                    var Buffer = new StreamReader(file.FileName);
                    try
                    {
                        string Container = string.Empty;
                        while (!Buffer.EndOfStream)
                        {
                            Container = Buffer.ReadLine();
                            str2 += Container + "\n";
                            var arr = Container.Split(',');

                            list_Name.Add(arr[0]);
                            list_X.Add(Convert.ToDouble(arr[1]));
                            list_Y.Add(Convert.ToDouble(arr[2]));
                            list_Z.Add(Convert.ToDouble(arr[3]));
                        }
                        Buffer.Close();//关闭流释放内存
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    richTextBox2.Text = str2;
                    for (int i = 0; i < list_Name.Count; i++)//Copy
                    {
                        list_Name7.Add(list_Name[i]);
                        list_X7.Add(list_X[i]);
                        list_Y7.Add(list_Y[i]);
                        list_Z7.Add(list_Z[i]);
                    }
                }
            }
            else if (radioButton1.Checked == true)//四参数
            {
                OpenFileDialog file = new OpenFileDialog();
                file.InitialDirectory = Application.StartupPath;
                file.RestoreDirectory = true;
                file.Filter = "All Files(*.*)|*.*|Dat Files(*.dat)|*.dat|Text Files(*.txt)|*.txt";
                file.FilterIndex = 1;
                if (file.ShowDialog() == DialogResult.OK)//文件选择对话
                {
                    string str2 = "";

                    var list_Name = new List<string>();
                    var list_X = new List<double>();
                    var list_Y = new List<double>();
                    var list_Z = new List<double>();

                    var Buffer = new StreamReader(file.FileName);
                    try
                    {
                        string Container = string.Empty;
                        while (!Buffer.EndOfStream)
                        {
                            Container = Buffer.ReadLine();
                            str2 += Container + "\n";
                            var arr = Container.Split(',');

                            list_Name.Add(arr[0]);
                            list_X.Add(Convert.ToDouble(arr[1]));
                            list_Y.Add(Convert.ToDouble(arr[2]));
                            list_Z.Add(Convert.ToDouble(arr[3]));
                        }
                        Buffer.Close();//关闭流释放内存
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    richTextBox2.Text = str2;
                    for (int i = 0; i < list_Name.Count; i++)//Copy
                    {
                        list_Name4.Add(list_Name[i]);
                        list_X4.Add(list_X[i]);
                        list_Y4.Add(list_Y[i]);
                        list_Z4.Add(list_Z[i]);
                    }
                }
            }
        }//打开目标文件xyz_Target

        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true&&j==false)
            {
                if (j0==false)
                {
                    //double[,] Matrix_l = new double[list_Name7.Count*3, 1];//use 4 points
                    double[,] Matrix_l = new double[12, 1];
                    //double[,] Matrix_B = new double[list_Name7.Count * 3, 7];//use 4 points
                    double[,] Matrix_B = new double[12, 7];
                    for (int i = 0; i < 4; i++)//not'list_Name7.Count'
                    {
                        Matrix_l[3 * i, 0] = list_X7[i] - list_X07[i];
                        Matrix_l[3 * i + 1, 0] = list_Y7[i] - list_Y07[i];
                        Matrix_l[3 * i + 2, 0] = list_Z7[i] - list_Z07[i];
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        Matrix_B[3 * i, 0] = 1;
                        Matrix_B[3 * i, 1] = 0;
                        Matrix_B[3 * i, 2] = 0;
                        Matrix_B[3 * i, 3] = 0;
                        Matrix_B[3 * i, 4] = -list_Z07[i];
                        Matrix_B[3 * i, 5] = list_Y07[i];
                        Matrix_B[3 * i, 6] = list_X07[i];

                        Matrix_B[3 * i + 1, 0] = 0;
                        Matrix_B[3 * i + 1, 1] = 1;
                        Matrix_B[3 * i + 1, 2] = 0;
                        Matrix_B[3 * i + 1, 3] = list_Z07[i];
                        Matrix_B[3 * i + 1, 4] = 0;
                        Matrix_B[3 * i + 1, 5] = -list_X07[i];
                        Matrix_B[3 * i + 1, 6] = list_Y07[i];

                        Matrix_B[3 * i + 2, 0] = 0;
                        Matrix_B[3 * i + 2, 1] = 0;
                        Matrix_B[3 * i + 2, 2] = 1;
                        Matrix_B[3 * i + 2, 3] = -list_Y07[i];
                        Matrix_B[3 * i + 2, 4] = list_X07[i];
                        Matrix_B[3 * i + 2, 5] = 0;
                        Matrix_B[3 * i + 2, 6] = list_Z07[i];
                    }
                    var Matrix_BT = Elevation_adjust.matrix.matTran(Matrix_B);
                    var Matrix_P = new double[12, 12];
                    for (int i = 0; i < 12; i++)
                    {
                        Matrix_P[i, i] = 1;//同精度，故P为单位阵
                    }
                    var Matrix_BTP = Elevation_adjust.matrix.matMul(Matrix_BT, Matrix_P);
                    var Matrix_BTPB = Elevation_adjust.matrix.matMul(Matrix_BTP, Matrix_B);
                    var Matrix_BTPB_1 = Elevation_adjust.matrix.matInv(Matrix_BTPB);
                    var Matrix_BTPB_1BT = Elevation_adjust.matrix.matMul(Matrix_BTPB_1, Matrix_BT);
                    var Matrix_BTPB_1BTP = Elevation_adjust.matrix.matMul(Matrix_BTPB_1BT, Matrix_P);
                    var Matrix_BTPB_1BTPl = Elevation_adjust.matrix.matMul(Matrix_BTPB_1BTP, Matrix_l);//MATRIX x^

                    //MessageBox.Show(Elevation_adjust.matrix.matToStr(Matrix_l));
                    //MessageBox.Show(list_X7[3].ToString());
                    string str = "条件数=";
                    str += (3 * 4).ToString() + "\n"; ;//选择空间四个点算
                    str += "冗余度=";
                    str += (12 - 7).ToString() + "\n";//7参数

                    var Matrix_Bxx = Elevation_adjust.matrix.matMul(Matrix_B, Matrix_BTPB_1BTPl);
                    var Matrix_V = Elevation_adjust.matrix.matSub(Matrix_Bxx, Matrix_l);
                    var Matrix_VT = Elevation_adjust.matrix.matTran(Matrix_V);
                    var Matrix_VTP = Elevation_adjust.matrix.matMul(Matrix_VT, Matrix_P);
                    var Matrix_VTPV = Elevation_adjust.matrix.matMul(Matrix_VTP, Matrix_V);
                    var Sigma = Math.Sqrt(Matrix_VTPV[0, 0] / 5);

                    str += "验后单位权中误差=" + Sigma.ToString() + "\n\n";
                    str += "每个公共点坐标残差[每三行为一组，分别为第i组的x,y,z坐标残差(i<=4)]:\n";
                    str += Elevation_adjust.matrix.matToStr(Matrix_V) + "\n";
                    str += "验核点坐标残差[每三行为一组，分别为第i组的x,y,z坐标残差(i=5,6)]:\n";

                    double[,] Matrix_B_ = new double[6, 7];
                    for (int i = 4; i < 6; i++)
                    {
                        Matrix_B_[3 * (i - 4), 0] = 1;
                        Matrix_B_[3 * (i - 4), 1] = 0;
                        Matrix_B_[3 * (i - 4), 2] = 0;
                        Matrix_B_[3 * (i - 4), 3] = 0;
                        Matrix_B_[3 * (i - 4), 4] = -list_Z07[i];
                        Matrix_B_[3 * (i - 4), 5] = list_Y07[i];
                        Matrix_B_[3 * (i - 4), 6] = list_X07[i];

                        Matrix_B_[3 * (i - 4) + 1, 0] = 0;
                        Matrix_B_[3 * (i - 4) + 1, 1] = 1;
                        Matrix_B_[3 * (i - 4) + 1, 2] = 0;
                        Matrix_B_[3 * (i - 4) + 1, 3] = list_Z07[i];
                        Matrix_B_[3 * (i - 4) + 1, 4] = 0;
                        Matrix_B_[3 * (i - 4) + 1, 5] = -list_X07[i];
                        Matrix_B_[3 * (i - 4) + 1, 6] = list_Y07[i];

                        Matrix_B_[3 * (i - 4) + 2, 0] = 0;
                        Matrix_B_[3 * (i - 4) + 2, 1] = 0;
                        Matrix_B_[3 * (i - 4) + 2, 2] = 1;
                        Matrix_B_[3 * (i - 4) + 2, 3] = -list_Y07[i];
                        Matrix_B_[3 * (i - 4) + 2, 4] = list_X07[i];
                        Matrix_B_[3 * (i - 4) + 2, 5] = 0;
                        Matrix_B_[3 * (i - 4) + 2, 6] = list_Z07[i];
                    }
                    var Matrix_bx = Elevation_adjust.matrix.matMul(Matrix_B_, Matrix_BTPB_1BTPl);
                    for (int i = 4; i < 6; i++)
                    {
                        Matrix_bx[3 * (i - 4), 0] += list_X07[i];
                        Matrix_bx[3 * (i - 4) + 1, 0] += list_Y07[i];
                        Matrix_bx[3 * (i - 4) + 2, 0] += list_Z07[i];
                    }
                    double[,] Matrix_LL_ = new double[6, 1];
                    for (int i = 4; i < 6; i++)
                    {
                        Matrix_LL_[3 * (i - 4), 0] = list_X7[i];
                        Matrix_LL_[3 * (i - 4) + 1, 0] = list_Y7[i];
                        Matrix_LL_[3 * (i - 4) + 2, 0] = list_Z7[i];
                    }
                    var Matrix_Delta = Elevation_adjust.matrix.matSub(Matrix_bx, Matrix_LL_);

                    str += Elevation_adjust.matrix.matToStr(Matrix_Delta) + "\n";

                    double[,] Matrix_Test = new double[3, 1];
                    Matrix_Test[0, 0] = Convert.ToDouble(list_X07[5].ToString().Substring(0, list_X07[5].ToString().Length - 4) + "134");//数据特点，小数点后有四位
                    Matrix_Test[1, 0] = Convert.ToDouble(list_Y07[5].ToString().Substring(0, list_Y07[5].ToString().Length - 4) + "134");//数据特点，小数点后有四位
                    Matrix_Test[2, 0] = Convert.ToDouble(list_Z07[5].ToString().Substring(0, list_Z07[5].ToString().Length - 3) + "134");//数据特点，小数点后有三位

                    string strr = "特定坐标点(x,y,z):\n原始坐标:\n" + "x=" + Matrix_Test[0, 0].ToString() + "\n";
                    strr += "y=" + Matrix_Test[1, 0].ToString() + "\n";
                    strr += "z=" + Matrix_Test[2, 0] + "\n";

                    double[,] Matrix_bb = new double[3, 7];
                    Matrix_bb[0, 0] = 1;
                    Matrix_bb[0, 1] = 0;
                    Matrix_bb[0, 2] = 0;
                    Matrix_bb[0, 3] = 0;
                    Matrix_bb[0, 4] = -Matrix_Test[2, 0];
                    Matrix_bb[0, 5] = Matrix_Test[1, 0];
                    Matrix_bb[0, 6] = Matrix_Test[0, 0];

                    Matrix_bb[1, 0] = 0;
                    Matrix_bb[1, 1] = 1;
                    Matrix_bb[1, 2] = 0;
                    Matrix_bb[1, 3] = Matrix_Test[2, 0];
                    Matrix_bb[1, 4] = 0;
                    Matrix_bb[1, 5] = -Matrix_Test[0, 0];
                    Matrix_bb[1, 6] = Matrix_Test[1, 0];

                    Matrix_bb[2, 0] = 0;
                    Matrix_bb[2, 1] = 0;
                    Matrix_bb[2, 2] = 1;
                    Matrix_bb[2, 3] = -Matrix_Test[1, 0];
                    Matrix_bb[2, 4] = Matrix_Test[0, 0];
                    Matrix_bb[2, 5] = 0;
                    Matrix_bb[2, 6] = Matrix_Test[2, 0];

                    var Matrix_bbxx = Elevation_adjust.matrix.matMul(Matrix_bb, Matrix_BTPB_1BTPl);
                    var Matrix_TargetPoint = Elevation_adjust.matrix.matAdd(Matrix_Test, Matrix_bbxx);
                    strr += "转换后坐标:\n" + "x=" + Matrix_TargetPoint[0, 0].ToString() + "\n";
                    strr += "y=" + Matrix_TargetPoint[1, 0].ToString() + "\n";
                    strr += "z=" + Matrix_TargetPoint[2, 0] + "\n";

                    textBox1.Text = Matrix_BTPB_1BTPl[0, 0].ToString();
                    textBox2.Text = Matrix_BTPB_1BTPl[1, 0].ToString();
                    textBox3.Text = Matrix_BTPB_1BTPl[2, 0].ToString();
                    textBox4.Text = Matrix_BTPB_1BTPl[3, 0].ToString();
                    textBox5.Text = Matrix_BTPB_1BTPl[4, 0].ToString();
                    textBox6.Text = Matrix_BTPB_1BTPl[5, 0].ToString();
                    textBox7.Text = Matrix_BTPB_1BTPl[6, 0].ToString();

                    richTextBox3.Text = str;
                    richTextBox4.Text = strr;
                    Data7 += str + "\n" + strr + "\n";
                    Data7 += "参数输出:\n";
                    Data7 += "平移参数X:/m=" + Matrix_BTPB_1BTPl[0, 0].ToString() + "\n";
                    Data7 += "平移参数Y:/m=" + Matrix_BTPB_1BTPl[1, 0].ToString() + "\n";
                    Data7 += "平移参数Z:/m=" + Matrix_BTPB_1BTPl[2, 0].ToString() + "\n";
                    Data7 += "旋转参数 X:=" + Matrix_BTPB_1BTPl[3, 0].ToString() + "\n";
                    Data7 += "旋转参数 Y:=" + Matrix_BTPB_1BTPl[4, 0].ToString() + "\n";
                    Data7 += "旋转参数 Z:=" + Matrix_BTPB_1BTPl[5, 0].ToString() + "\n";
                    Data7 += "尺度比 PPM=" + Matrix_BTPB_1BTPl[6, 0].ToString() + "\n";
                }
                j0 = true;
                j = true;
            }
            else if (radioButton1.Checked == true&&j==false)//四参数
            {
                if (j0==false)
                {
                    //double[,] Matrix_l = new double[list_Name7.Count*3, 1];//use 5 points
                    double[,] Matrix_l = new double[10, 1];
                    //double[,] Matrix_B = new double[list_Name7.Count * 3, 7];//use 5 points
                    double[,] Matrix_B = new double[10, 4];
                    for (int i = 0; i < 5; i++)//not'list_Name7.Count'
                    {
                        Matrix_l[2 * i, 0] = list_X4[i] - list_X04[i];
                        Matrix_l[2 * i + 1, 0] = list_Y4[i] - list_Y04[i];
                        //Matrix_l[3 * i + 2, 0] = list_Z4[i] - list_Z04[i];
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        Matrix_B[2 * i, 0] = 1;
                        Matrix_B[2 * i, 1] = 0;
                        //Matrix_B[2 * i, 2] = 0;
                        //Matrix_B[2 * i, 3] = 0;
                        //Matrix_B[2 * i, 4] = -list_Z04[i];
                        Matrix_B[2 * i, 2] = list_Y04[i];
                        Matrix_B[2 * i, 3] = list_X04[i];

                        Matrix_B[2 * i + 1, 0] = 0;
                        Matrix_B[2 * i + 1, 1] = 1;
                        //Matrix_B[2 * i + 1, 2] = 0;
                        //Matrix_B[2 * i + 1, 3] = list_Z04[i];
                        //Matrix_B[2 * i + 1, 4] = 0;
                        Matrix_B[2 * i + 1, 2] = -list_X04[i];
                        Matrix_B[2 * i + 1, 3] = list_Y04[i];

                        //Matrix_B[2 * i + 2, 0] = 0;
                        //Matrix_B[2 * i + 2, 1] = 0;
                        //Matrix_B[2 * i + 2, 2] = 1;
                        //Matrix_B[2 * i + 2, 3] = -list_Y04[i];
                        //Matrix_B[2 * i + 2, 4] = list_X04[i];
                        //Matrix_B[2 * i + 2, 5] = 0;
                        //Matrix_B[2 * i + 2, 6] = list_Z04[i];
                    }
                    var Matrix_BT = Elevation_adjust.matrix.matTran(Matrix_B);
                    var Matrix_P = new double[10, 10];
                    for (int i = 0; i < 10; i++)
                    {
                        Matrix_P[i, i] = 1;//同精度，故P为单位阵
                    }
                    var Matrix_BTP = Elevation_adjust.matrix.matMul(Matrix_BT, Matrix_P);
                    var Matrix_BTPB = Elevation_adjust.matrix.matMul(Matrix_BTP, Matrix_B);
                    var Matrix_BTPB_1 = Elevation_adjust.matrix.matInv(Matrix_BTPB);
                    var Matrix_BTPB_1BT = Elevation_adjust.matrix.matMul(Matrix_BTPB_1, Matrix_BT);
                    var Matrix_BTPB_1BTP = Elevation_adjust.matrix.matMul(Matrix_BTPB_1BT, Matrix_P);
                    var Matrix_BTPB_1BTPl = Elevation_adjust.matrix.matMul(Matrix_BTPB_1BTP, Matrix_l);//MATRIX x^
                    
                    //MessageBox.Show(Elevation_adjust.matrix.matToStr(Matrix_l));
                    //MessageBox.Show(list_X7[3].ToString());
                    string str = "条件数=";
                    str += (3 * 5 - 5).ToString() + "\n"; ;//选择空间5个点算-5个z（平面）
                    str += "冗余度=";
                    str += (15 - 4 - 5).ToString() + "\n";//4参数

                    var Matrix_Bxx = Elevation_adjust.matrix.matMul(Matrix_B, Matrix_BTPB_1BTPl);
                    var Matrix_V = Elevation_adjust.matrix.matSub(Matrix_Bxx, Matrix_l);
                    var Matrix_VT = Elevation_adjust.matrix.matTran(Matrix_V);
                    var Matrix_VTP = Elevation_adjust.matrix.matMul(Matrix_VT, Matrix_P);
                    var Matrix_VTPV = Elevation_adjust.matrix.matMul(Matrix_VTP, Matrix_V);
                    var Sigma = Math.Sqrt(Matrix_VTPV[0, 0] / 6);

                    str += "验后单位权中误差=" + Sigma.ToString() + "\n\n";
                    str += "每个公共点坐标残差[每两行为一组，分别为第i组的x,y坐标残差(i<=5)]:\n";
                    str += Elevation_adjust.matrix.matToStr(Matrix_V) + "\n";
                    str += "验核点坐标残差[每两行为一组，分别为第i组的x,y坐标残差(i=6,7,8)]:\n";

                    double[,] Matrix_B_ = new double[6, 4];
                    for (int i = 5; i < 8; i++)
                    {
                        Matrix_B_[2 * (i - 5), 0] = 1;
                        Matrix_B_[2 * (i - 5), 1] = 0;
                        //Matrix_B_[3 * (i - 5), 2] = 0;
                        //Matrix_B_[3 * (i - 5), 3] = 0;
                        //Matrix_B_[3 * (i - 5), 4] = -list_Z04[i];
                        Matrix_B_[2 * (i - 5), 2] = list_Y04[i];
                        Matrix_B_[2 * (i - 5), 3] = list_X04[i];

                        Matrix_B_[2 * (i - 5) + 1, 0] = 0;
                        Matrix_B_[2 * (i - 5) + 1, 1] = 1;
                        //Matrix_B_[3 * (i - 5) + 1, 2] = 0;
                        //Matrix_B_[3 * (i - 5) + 1, 3] = list_Z04[i];
                        //Matrix_B_[3 * (i - 5) + 1, 4] = 0;
                        Matrix_B_[2 * (i - 5) + 1, 2] = -list_X04[i];
                        Matrix_B_[2 * (i - 5) + 1, 3] = list_Y04[i];

                        //Matrix_B_[3 * (i - 5) + 2, 0] = 0;
                        //Matrix_B_[3 * (i - 5) + 2, 1] = 0;
                        //Matrix_B_[3 * (i - 5) + 2, 2] = 1;
                        //Matrix_B_[3 * (i - 5) + 2, 3] = -list_Y04[i];
                        //Matrix_B_[3 * (i - 5) + 2, 4] = list_X04[i];
                        //Matrix_B_[3 * (i - 5) + 2, 5] = 0;
                        //Matrix_B_[3 * (i - 5) + 2, 6] = list_Z04[i];
                    }
                    var Matrix_bx = Elevation_adjust.matrix.matMul(Matrix_B_, Matrix_BTPB_1BTPl);
                    for (int i = 5; i < 8; i++)
                    {
                        Matrix_bx[2 * (i - 5), 0] += list_X04[i];
                        Matrix_bx[2 * (i - 5) + 1, 0] += list_Y04[i];
                        //Matrix_bx[3 * (i - 5) + 2, 0] += list_Z04[i];
                    }
                    double[,] Matrix_LL_ = new double[6, 1];
                    for (int i = 5; i < 8; i++)
                    {
                        Matrix_LL_[2 * (i - 5), 0] = list_X4[i];
                        Matrix_LL_[2 * (i - 5) + 1, 0] = list_Y4[i];
                        //Matrix_LL_[3 * (i - 5) + 2, 0] = list_Z4[i];
                    }
                    var Matrix_Delta = Elevation_adjust.matrix.matSub(Matrix_bx, Matrix_LL_);

                    str += Elevation_adjust.matrix.matToStr(Matrix_Delta) + "\n";

                    double[,] Matrix_Test = new double[2, 1];
                    Matrix_Test[0, 0] = Convert.ToDouble(list_X04[7].ToString().Substring(0, list_X04[7].ToString().Length - 3) + "134");//数据特点，小数点后有3位
                    Matrix_Test[1, 0] = Convert.ToDouble(list_Y04[7].ToString().Substring(0, list_Y04[7].ToString().Length - 3) + "134");//数据特点，小数点后有3位
                    //Matrix_Test[2, 0] = Convert.ToDouble(list_Z04[7].ToString().Substring(0, list_Z04[7].ToString().Length - 3) + "134");//数据特点，小数点后有3位
                    //Matrix_Test[2, 0] = 0.000;

                    //Matrix_Test[0, 0] = 115500.237;
                    //Matrix_Test[1, 0] = 118500.826;

                    string strr = "特定坐标点(x,y):\n原始坐标:\n" + "x=" + Matrix_Test[0, 0].ToString() + "\n";
                    strr += "y=" + Matrix_Test[1, 0].ToString() + "\n";
                    //strr += "z=" + Matrix_Test[2, 0] + "\n";

                    double[,] Matrix_bb = new double[2, 4];
                    Matrix_bb[0, 0] = 1;
                    Matrix_bb[0, 1] = 0;
                    //Matrix_bb[0, 2] = 0;
                    //Matrix_bb[0, 3] = 0;
                    //Matrix_bb[0, 4] = -Matrix_Test[2, 0];
                    Matrix_bb[0, 2] = Matrix_Test[1, 0];
                    Matrix_bb[0, 3] = Matrix_Test[0, 0];

                    Matrix_bb[1, 0] = 0;
                    Matrix_bb[1, 1] = 1;
                    //Matrix_bb[1, 2] = 0;
                    //Matrix_bb[1, 3] = Matrix_Test[2, 0];
                    //Matrix_bb[1, 4] = 0;
                    Matrix_bb[1, 2] = -Matrix_Test[0, 0];
                    Matrix_bb[1, 3] = Matrix_Test[1, 0];

                    //Matrix_bb[2, 0] = 0;
                    //Matrix_bb[2, 1] = 0;
                    //Matrix_bb[2, 2] = 1;
                    //Matrix_bb[2, 3] = -Matrix_Test[1, 0];
                    //Matrix_bb[2, 4] = Matrix_Test[0, 0];
                    //Matrix_bb[2, 5] = 0;
                    //Matrix_bb[2, 6] = Matrix_Test[2, 0];

                    var Matrix_bbxx = Elevation_adjust.matrix.matMul(Matrix_bb, Matrix_BTPB_1BTPl);
                    var Matrix_TargetPoint = Elevation_adjust.matrix.matAdd(Matrix_Test, Matrix_bbxx);
                    strr += "转换后坐标:\n" + "x=" + Matrix_TargetPoint[0, 0].ToString() + "\n";
                    strr += "y=" + Matrix_TargetPoint[1, 0].ToString() + "\n";
                    //strr += "z=" + Matrix_TargetPoint[2, 0] + "\n";

                    //label9.Text= "旋转参数 R:";

                    textBox1.Text = Matrix_BTPB_1BTPl[0, 0].ToString();
                    textBox2.Text = Matrix_BTPB_1BTPl[1, 0].ToString();
                    textBox3.Text = "- - - - - - - - - - -";
                    textBox4.Text = "- - - - - - - - - - -";
                    textBox5.Text = "- - - - - - - - - - -";
                    textBox6.Text = Matrix_BTPB_1BTPl[2, 0].ToString();
                    textBox7.Text = Matrix_BTPB_1BTPl[3, 0].ToString();

                    richTextBox3.Text = str;
                    richTextBox4.Text = strr;
                    Data4 += str + "\n" + strr + "\n";
                    Data4 += "参数输出:\n";
                    Data4 += "平移参数X:/m=" + Matrix_BTPB_1BTPl[0, 0].ToString() + "\n";
                    Data4 += "平移参数Y:/m=" + Matrix_BTPB_1BTPl[1, 0].ToString() + "\n";
                    //Data4 += "平移参数Z:/m=" + Matrix_BTPB_1BTPl[2, 0].ToString() + "\n";
                    //Data4 += "旋转参数 X:=" + Matrix_BTPB_1BTPl[3, 0].ToString() + "\n";
                    //Data4 += "旋转参数 Y:=" + Matrix_BTPB_1BTPl[4, 0].ToString() + "\n";
                    Data4 += "旋转参数 R:=" + Matrix_BTPB_1BTPl[2, 0].ToString() + "\n";
                    Data4 += "尺度比 PPM=" + Matrix_BTPB_1BTPl[3, 0].ToString() + "\n";
                }
                j0 = true;
                j = true;
            }
        }//Compute

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true&&j==true)
            {
                SaveFileDialog file = new SaveFileDialog();
                file.InitialDirectory = Application.StartupPath;
                file.RestoreDirectory = true;
                file.Filter = "All Files(*.*)|*.*|Dat Files(*.dat)|*.dat|Text Files(*.txt)|*.txt";
                file.FilterIndex = 3;
                if (file.ShowDialog() == DialogResult.OK)
                {
                    var stream = new FileStream(file.FileName, FileMode.Create);
                    var writer = new StreamWriter(stream);

                    writer.Write(Data7);
                    writer.Close();
                    stream.Close();
                }
            }
            else if (radioButton1.Checked == true && j == true)
            {
                SaveFileDialog file = new SaveFileDialog();
                file.InitialDirectory = Application.StartupPath;
                file.RestoreDirectory = true;
                file.Filter = "All Files(*.*)|*.*|Dat Files(*.dat)|*.dat|Text Files(*.txt)|*.txt";
                file.FilterIndex = 3;
                if (file.ShowDialog() == DialogResult.OK)
                {
                    var stream = new FileStream(file.FileName, FileMode.Create);
                    var writer = new StreamWriter(stream);

                    writer.Write(Data4);
                    writer.Close();
                    stream.Close();
                }
            }
        }//Save

        private void button6_Click(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true||radioButton3.Checked==true)
            {
                Xpoint = new List<XYZPoint>();
                jj = true;
                formula.Readfile(out Xpoint);               
            }
        }//打开文件XYZ2BLHNEU

        private void button7_Click(object sender, EventArgs e)
        {
            if (jj == true&&j1==true)
            {
                Xpoint.Clear();
                Bpoint.Clear();
                if (radioButton4.Checked)
                {
                    Npoint.Clear();
                }
                
            }
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            richTextBox6.Clear();
            richTextBox7.Clear();
            jj = false;
            j1 = false;      
        }//Clear XYZ2BLHNEU

        private void button9_Click(object sender, EventArgs e)
        {
            if ((radioButton4.Checked==true||radioButton3.Checked==true)&&jj==true)
            {
                SaveFileDialog file = new SaveFileDialog();
                file.InitialDirectory = Application.StartupPath;
                file.RestoreDirectory = true;
                file.Filter = "All Files(*.*)|*.*|Dat Files(*.dat)|*.dat|Text Files(*.txt)|*.txt";
                file.FilterIndex = 3;
                if (file.ShowDialog() == DialogResult.OK)
                {
                    var stream = new FileStream(file.FileName, FileMode.Create);
                    var writer = new StreamWriter(stream);
                   
                    writer.Write(richTextBox7.Text);
                    writer.Close();
                    stream.Close();
                }
            }
        }//Save XYZ2BLHNEU

        private void button8_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true&&jj==true)
            {
                if (j1 == false)
                {
                    Bpoint = new List<BLHPoint>();
                    formula.xyz2blh(Xpoint, out Bpoint);
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
                    richTextBox6.Text = report1;
                    richTextBox7.Text = report2;
                }
                j1 = true;           
            }
            if (radioButton4.Checked==true&&jj==true)
            {
                int num = 1;
                if (j1==false)
                {
                    Bpoint = new List<BLHPoint>();
                    formula.xyz2blh(Xpoint, out Bpoint);
                    formula.blh2neu(Xpoint, Bpoint, num, out Npoint);
                    report3 = null;
                    report2 = null;
                    report1 = null;
                    formula.Report(Xpoint, Bpoint, num, Npoint, out report1, out report2, out report3);
                    richTextBox6.Text = report1;
                    richTextBox7.Text = report3;
                }
                j1 = true;
            }
        }//Compute XYZ2BLHNEU     
    }
}

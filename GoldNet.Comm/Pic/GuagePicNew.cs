using System.Web;
using System.Web.UI.WebControls;
using Dundas.Gauges.WebControl;

namespace GoldNet.Comm.Pic
{
    public class GuagePicNew
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GuagePicNew()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 根据数据绘制仪表图
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public GaugeContainer BuideGuagePic(string numbers,string title,string val1,string val2,string picnum)
        {
            //图表容器对象
            GaugeContainer gauge = new GaugeContainer();
            gauge.BackColor = System.Drawing.Color.White;
            gauge.Height = Unit.Parse("180");
            //gauge.Width = Unit.Parse("150");
            gauge.ImageUrl = "TempImages/GaugePic" + picnum;

            //文本
            GaugeLabel label1 = new GaugeLabel();
            // label1.Font = new System.Drawing.Font("Arial", 8.25);
            label1.Text = "%";
            //相对容器尺寸的百分比
            label1.Size.Height = 10;
            label1.Size.Width = 10;
            label1.Location.Y = 65;
            label1.Location.X = 55;
            gauge.Labels.Add(label1);

            //图例文字设置
            GaugeLabel label2 = new GaugeLabel();
            // label2.Font =new System.Drawing.Font( "Arial", 7);
            label2.Text = title;
            label2.Size.Height = 20;
            label2.Size.Width = 40;
            label2.Location.X = 33;
            label2.Location.Y = 86;
            gauge.Labels.Add(label2);

            //图例文字设置
            GaugeLabel label3 = new GaugeLabel();
            // label3.Font =new System.Drawing.Font( "Arial", 7);
            label3.Text = val1;
            label3.Size.Height = 15;
            label3.Size.Width = 26;
            label3.Location.Y = 10;
            label3.Location.X = 75;
            gauge.Labels.Add(label3);

            //图例文字设置
            GaugeLabel label4 = new GaugeLabel();
            //label4.Font =new System.Drawing.Font( "Arial", 7);
            label4.Text = val2;
            label4.Size.Height = 15;
            label4.Size.Width = 26;
            label4.Location.Y =25;
            label4.Location.X = 75;
            gauge.Labels.Add(label4);

            //定义圆形仪表图表对象
            CircularGauge circul = new CircularGauge();
            circul.Name = "circul";
            //circul.Size.Height = float.Parse("80");
            //circul.Size.Width = float.Parse("80");
            //设置中心点坐标
            circul.PivotPoint.X = float.Parse("50");
            circul.PivotPoint.Y = float.Parse("50");
            circul.Location.X = float.Parse("0");
            circul.Location.Y = float.Parse("0");

            //绘制仪表“不足”区域带
            CircularRange one = new CircularRange();
            one.StartValue = double.Parse("2");
            one.EndValue = double.Parse("78");
            //区域带的的宽度
            one.StartWidth = float.Parse("35");
            one.EndWidth = float.Parse("35");
            one.FillColor = System.Drawing.Color.Red;
            //区域带与仪表刻度的距离
            one.DistanceFromScale = float.Parse("22");
            //区域带的渐变类型及渐变色
            one.FillGradientType = RangeGradientType.StartToEnd;
            one.FillGradientEndColor = System.Drawing.Color.Yellow;
            circul.Ranges.Add(one);

            //绘制仪表“正常”区域
            CircularRange two = new CircularRange();
            two.StartValue = double.Parse("82");
            two.EndValue = double.Parse("98");
            two.StartWidth = float.Parse("35");
            two.EndWidth = float.Parse("35");
            two.FillColor = System.Drawing.Color.Yellow;
            two.DistanceFromScale = float.Parse("22");
            two.FillGradientType = RangeGradientType.None;
            circul.Ranges.Add(two);

            //绘制仪表“超额”区域
            CircularRange three = new CircularRange();
            three.StartValue = double.Parse("102");
            three.EndValue = double.Parse("198");
            three.StartWidth = float.Parse("35");
            three.EndWidth = float.Parse("35");
            three.FillColor = System.Drawing.Color.Yellow;
            three.DistanceFromScale = float.Parse("22");
            three.FillGradientType = RangeGradientType.StartToEnd;
            three.FillGradientEndColor = System.Drawing.Color.LimeGreen;
            circul.Ranges.Add(three);

            //绘制仪表刻度
            CircularScale scal1 = new CircularScale();
            //刻度边框
            scal1.BorderColor = System.Drawing.Color.GreenYellow;
            scal1.BorderWidth = 1;
            //刻度颜色
            scal1.FillColor = System.Drawing.Color.GreenYellow;
            //刻度最大值
            scal1.Maximum = double.Parse("200");
            //刻度半径
            scal1.Radius = float.Parse("42");
            //刻度度数
            scal1.StartAngle = float.Parse("80");
            scal1.SweepAngle = float.Parse("200");
            //刻度数字颜色
            scal1.LabelStyle.TextColor = System.Drawing.Color.Black;
            //主刻度标识
            scal1.MajorTickMark.FillColor = System.Drawing.Color.White;
            scal1.MajorTickMark.Width = float.Parse("0.5");
            scal1.MajorTickMark.Shape = MarkerStyle.Rectangle;
            //次刻度标识
            scal1.MinorTickMark.FillColor = System.Drawing.Color.Black;
            scal1.MinorTickMark.Width = float.Parse("0.3");
            circul.Scales.Add(scal1);

            //绘制仪表指针
            CircularPointer point = new CircularPointer();
            point.Name = "point";
            //指针帽宽度
            point.CapWidth = float.Parse("30");
            //指针渐变类型
            point.FillGradientType = GradientType.None;
            //指针渐变颜色
            //point.FillGradientEndColor = System.Drawing.Color.FromArgb(200, 59, 105);
            //指针类型
            point.Type = CircularPointerType.Needle;
            point.NeedleStyle = NeedleStyle.NeedleStyle1;
            point.Width = float.Parse("20");
            circul.Pointers.Add(point);

            //绘制仪表边框
            circul.BackFrame.BackColor = System.Drawing.Color.FromArgb(0, 0, 0);
            circul.BackFrame.BackGradientEndColor = System.Drawing.Color.FromArgb(0, 0, 0);
            circul.BackFrame.FrameWidth = float.Parse("2");
            //边框形状
            circul.BackFrame.Shape = BackFrameShape.Circular;
            //边框样式
            circul.BackFrame.Style = BackFrameStyle.None;
            gauge.CircularGauges.Add(circul);

            //仪表数字盘
            NumericIndicator numers1 = new NumericIndicator();
            //背景色
            numers1.BackColor = System.Drawing.Color.Transparent;
            //样式
            numers1.Style = NumericIndicatorStyle.Digital14Segment;
            numers1.BorderWidth = 0;
            //是否显示小数
            numers1.ShowDecimalPoint = true;
            //边框样式
            numers1.BorderColor = System.Drawing.Color.Transparent;
            //渐变样式
            numers1.BackGradientType = GradientType.None;
            //小数位数
            numers1.Decimals = 1;
            //设置当前显示值
            numers1.Value = double.Parse(numbers);
            //小数颜色
            numers1.DecimalColor = System.Drawing.Color.Black;
            //父对象
            numers1.Parent = "";
            //算数符
            numers1.ShowSign = ShowSign.None;
            //numers1.LedDimColor = System.Drawing.Color.FromArgb(20,0,0,0);
            //显示数据位数，包括小数
            numers1.Digits = 4;
            //数字颜色
            numers1.DigitColor = System.Drawing.Color.Black;
            //零是否显示
            numers1.ShowLeadingZeros = false;
            numers1.Location.Y = float.Parse("65");
            numers1.Location.X = float.Parse("40");
            numers1.Size.Height = float.Parse("10");
            numers1.Size.Width = float.Parse("14");
            gauge.NumericIndicators.Add(numers1);

            //仪表容器属性设置
            gauge.BackFrame.BackColor = System.Drawing.Color.FromArgb(115, 100, 146);
            gauge.BackFrame.BackGradientEndColor = System.Drawing.Color.FromArgb(26, 20, 105);
            gauge.BackFrame.FrameGradientType = GradientType.None;
            gauge.BackFrame.FrameWidth = float.Parse("5");
            gauge.BackFrame.Shape = BackFrameShape.AutoShape;
            gauge.BackFrame.Style = BackFrameStyle.None;

            //
            State state = new State();
            state.EndValue = double.Parse("120");
            state.BorderWidth = 1;
            state.StartValue = double.Parse("80");
            state.Text = "";

            ////图例
            //StateIndicator sta1 = new StateIndicator();
            ////从左到下的阴影大小
            //sta1.ShadowOffset = float.Parse("0");
            ////样式
            //sta1.Style = StateIndicatorStyle.RectangularLed;
            //// sta1.Style = "RectangularLed";
            //sta1.FillColor = System.Drawing.Color.Red;
            ////父对象
            //sta1.Parent = "CircularGauges.Default";
            ////渐变色
            //sta1.FillGradientEndColor = System.Drawing.Color.Red;
            ////文字内容
            //sta1.Text = "预警";
            ////指定数字来源
            //sta1.ValueSource = "Default";
            //sta1.Size.Height = float.Parse("6");
            //sta1.Size.Width = float.Parse("8");
            ////图例集合
            //sta1.States.Add(state);
            //sta1.Location.Y = float.Parse("9");
            //sta1.Location.X = float.Parse("80");
            //gauge.StateIndicators.Add(sta1);


            ////图例设置
            //StateIndicator sta2 = new StateIndicator();
            //sta2.ShadowOffset = float.Parse("0");
            //sta2.Style = StateIndicatorStyle.RectangularLed;
            ////sta2.Style = "RectangularLed";
            //sta2.FillColor = System.Drawing.Color.Yellow;
            //sta2.Parent = "CircularGauges.Default";
            //sta2.FillGradientEndColor = System.Drawing.Color.Yellow;
            //sta2.Text = "正常";
            //sta2.ValueSource = "Default";
            //sta2.Size.Height = float.Parse("6");
            //sta2.Size.Width = float.Parse("8");
            //sta2.States.Add(state);
            //sta2.Location.Y = float.Parse("18");
            //sta2.Location.X = float.Parse("80");
            //gauge.StateIndicators.Add(sta2);

            ////图例设置
            //StateIndicator sta3 = new StateIndicator();
            //sta3.ShadowOffset = float.Parse("0");
            //sta3.Style = StateIndicatorStyle.RectangularLed;
            ////sta3.Style = "RectangularLed";
            //sta3.FillColor = System.Drawing.Color.LimeGreen;
            //sta3.Parent = "CircularGauges.Default";
            //sta3.FillGradientEndColor = System.Drawing.Color.LimeGreen;
            //sta3.Text = "超额";
            //sta3.ValueSource = "Default";
            //sta3.Size.Height = float.Parse("6");
            //sta3.Size.Width = float.Parse("8");
            //sta3.States.Add(state);
            //sta3.Location.Y = float.Parse("27");
            //sta3.Location.X = float.Parse("80");
            //gauge.StateIndicators.Add(sta3);

            //仪表设置值
            gauge.CircularGauges["circul"].Pointers["point"].Value = double.Parse(numbers);
            gauge.SaveAsImage(HttpContext.Current.Server.MapPath("./TempImages/") + "GaugePic" + picnum + ".png");

            return gauge;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public GaugeContainer BuideGuagePic1(string numbers)
        {
            GaugeContainer gauge = new GaugeContainer();
            gauge.BackColor = System.Drawing.Color.Transparent;

            gauge.ImageUrl = "TempImages/GaugePic1";

            CircularGauge circul = new CircularGauge();
            circul.Size.Height = float.Parse("100");
            circul.Size.Width = float.Parse("100");
            circul.PivotPoint.X = float.Parse("50");
            circul.PivotPoint.Y = float.Parse("50");
            circul.Location.X = float.Parse("0");
            circul.Location.Y = float.Parse("0");

            CircularRange one = new CircularRange();
            circul.Ranges.Add(one);

            CircularScale scal1 = new CircularScale();
            circul.Scales.Add(scal1);

            circul.BackFrame.Shape = BackFrameShape.Circular;
            circul.BackFrame.Style = BackFrameStyle.Edged;

            gauge.CircularGauges.Add(circul);

            InputValue value1 = new InputValue();
            gauge.Values.Add(value1);

            gauge.BackFrame.Shape = BackFrameShape.Rectangular;
            gauge.BackFrame.Style = BackFrameStyle.None;
            //gauge.RenderControl();
            gauge.SaveAsImage(HttpContext.Current.Server.MapPath("./TempImages/") + "GaugePic1.png");

            return gauge;
        }
    }
}

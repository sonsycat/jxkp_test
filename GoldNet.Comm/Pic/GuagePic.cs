using System.Web;
using System.Web.UI.WebControls;
using Dundas.Gauges.WebControl;

namespace GoldNet.Comm.Pic
{
    /// <summary>
    /// GuagePic 的摘要说明
    /// </summary>
    public class GuagePic
    {
        public GuagePic()
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
        public GaugeContainer BuideGuagePic(string numbers)
        {
            //图表容器对象
            GaugeContainer gauge = new GaugeContainer();
            gauge.BackColor = System.Drawing.Color.White;
            gauge.Height = Unit.Parse("180");
            gauge.ImageUrl = "TempImages/GaugePic";

            //
            GaugeLabel label1 = new GaugeLabel();
            // label1.Font = new System.Drawing.Font("Arial", 8.25);
            label1.Text = "%";
            label1.Size.Height = 10;
            label1.Size.Width = 10;
            label1.Location.Y = 70;
            label1.Location.X = 55;
            gauge.Labels.Add(label1);

            //图例设置
            GaugeLabel label2 = new GaugeLabel();
            // label2.Font =new System.Drawing.Font( "Arial", 7);
            label2.Text = "不足";
            label2.Size.Height = 10;
            label2.Size.Width = 10;
            label2.Location.Y = 9;
            label2.Location.X = 90;
            gauge.Labels.Add(label2);

            GaugeLabel label3 = new GaugeLabel();
            // label3.Font =new System.Drawing.Font( "Arial", 7);
            label3.Text = "正常";
            label3.Size.Height = 10;
            label3.Size.Width = 10;
            label3.Location.Y = 18;
            label3.Location.X = 90;
            gauge.Labels.Add(label3);

            GaugeLabel label4 = new GaugeLabel();
            //label4.Font =new System.Drawing.Font( "Arial", 7);
            label4.Text = "超额";
            label4.Size.Height = 10;
            label4.Size.Width = 10;
            label4.Location.Y = 27;
            label4.Location.X = 90;
            gauge.Labels.Add(label4);

            //定义圆形仪表图表对象
            CircularGauge circul = new CircularGauge();
            circul.Name = "circul";
            circul.Size.Height = float.Parse("100");
            circul.Size.Width = float.Parse("100");
            circul.PivotPoint.X = float.Parse("50");
            circul.PivotPoint.Y = float.Parse("50");
            circul.Location.X = float.Parse("0");
            circul.Location.Y = float.Parse("0");

            //绘制仪表不足区域
            CircularRange one = new CircularRange();
            one.EndValue = double.Parse("78");
            one.StartValue = double.Parse("2");
            one.StartWidth = float.Parse("35");
            one.FillColor = System.Drawing.Color.Red;
            one.DistanceFromScale = float.Parse("22");
            one.EndWidth = float.Parse("35");
            one.FillGradientType = RangeGradientType.None;
            circul.Ranges.Add(one);

            //绘制仪表正常区域
            CircularRange two = new CircularRange();
            two.EndValue = double.Parse("98");
            two.StartValue = double.Parse("82");
            two.StartWidth = float.Parse("35");
            two.FillColor = System.Drawing.Color.Yellow;
            two.DistanceFromScale = float.Parse("22");
            two.EndWidth = float.Parse("35");
            two.FillGradientType = RangeGradientType.None;
            circul.Ranges.Add(two);

            //绘制仪表超额区域
            CircularRange three = new CircularRange();
            three.EndValue = double.Parse("198");
            three.StartValue = double.Parse("102");
            three.StartWidth = float.Parse("35");
            three.FillColor = System.Drawing.Color.LimeGreen;
            three.DistanceFromScale = float.Parse("22");
            three.EndWidth = float.Parse("35");
            three.FillGradientType = RangeGradientType.None;
            circul.Ranges.Add(three);

            //绘制图表刻度
            CircularScale scal1 = new CircularScale();
            scal1.BorderColor = System.Drawing.Color.White;
            scal1.BorderWidth = 0;
            scal1.FillColor = System.Drawing.Color.White;
            scal1.Maximum = double.Parse("200");
            scal1.Radius = float.Parse("42");
            scal1.StartAngle = float.Parse("60");
            scal1.SweepAngle = float.Parse("240");
            scal1.LabelStyle.TextColor = System.Drawing.Color.White;
            scal1.MajorTickMark.FillColor = System.Drawing.Color.White;
            scal1.MajorTickMark.Shape = MarkerStyle.Rectangle;
            scal1.MajorTickMark.Width = float.Parse("0.5");
            scal1.MinorTickMark.FillColor = System.Drawing.Color.White;
            scal1.MinorTickMark.Width = float.Parse("0");
            circul.Scales.Add(scal1);

            //绘制仪表指针
            CircularPointer point = new CircularPointer();
            point.Name = "point";
            point.CapWidth = float.Parse("30");
            point.FillGradientEndColor = System.Drawing.Color.FromArgb(200, 59, 105);
            point.FillGradientType = GradientType.None;
            point.NeedleStyle = NeedleStyle.NeedleStyle1;
            point.Width = float.Parse("20");
            circul.Pointers.Add(point);

            //绘制仪表边框
            circul.BackFrame.BackColor = System.Drawing.Color.FromArgb(169, 201, 213);
            circul.BackFrame.BackGradientEndColor = System.Drawing.Color.FromArgb(169, 201, 213);
            circul.BackFrame.FrameWidth = float.Parse("2");
            circul.BackFrame.Shape = BackFrameShape.Circular;
            circul.BackFrame.Style = BackFrameStyle.Edged;
            gauge.CircularGauges.Add(circul);

            NumericIndicator numers1 = new NumericIndicator();
            numers1.BackColor = System.Drawing.Color.Transparent;
            numers1.Style = NumericIndicatorStyle.Digital14Segment;
            numers1.BorderWidth = 0;
            numers1.ShowDecimalPoint = true;
            numers1.BorderColor = System.Drawing.Color.Transparent;
            numers1.BackGradientType = GradientType.None;
            numers1.Decimals = 0;
            numers1.Value = double.Parse(numbers);
            numers1.DecimalColor = System.Drawing.Color.Red;
            numers1.Parent = "";
            numers1.ShowSign = ShowSign.None;
            //numers1.LedDimColor = System.Drawing.Color.FromArgb(20,0,0,0);
            numers1.Digits = 3;
            numers1.DigitColor = System.Drawing.Color.Black;
            numers1.ShowLeadingZeros = false;
            numers1.Location.Y = float.Parse("70");
            numers1.Location.X = float.Parse("40");
            numers1.Size.Height = float.Parse("10");
            numers1.Size.Width = float.Parse("14");
            gauge.NumericIndicators.Add(numers1);

            gauge.BackFrame.BackColor = System.Drawing.Color.FromArgb(115, 100, 146);
            gauge.BackFrame.BackGradientEndColor = System.Drawing.Color.FromArgb(26, 20, 105);
            gauge.BackFrame.FrameWidth = float.Parse("5");
            gauge.BackFrame.Shape = BackFrameShape.Rectangular;
            gauge.BackFrame.Style = BackFrameStyle.None;

            StateIndicator sta1 = new StateIndicator();
            sta1.ShadowOffset = float.Parse("2");
            sta1.Style = StateIndicatorStyle.RectangularLed;
            // sta1.Style = "RectangularLed";
            sta1.FillColor = System.Drawing.Color.Red;
            sta1.Parent = "CircularGauges.Default";
            sta1.FillGradientEndColor = System.Drawing.Color.Red;
            sta1.Text = "";
            sta1.ValueSource = "Default";
            sta1.Size.Height = float.Parse("6");
            sta1.Size.Width = float.Parse("8");
            State state = new State();
            state.EndValue = double.Parse("120");
            state.BorderWidth = 1;
            state.StartValue = double.Parse("80");
            state.Text = "";
            sta1.States.Add(state);
            sta1.Location.Y = float.Parse("9");
            sta1.Location.X = float.Parse("80");
            gauge.StateIndicators.Add(sta1);

            StateIndicator sta2 = new StateIndicator();
            sta2.ShadowOffset = float.Parse("2");
            sta2.Style = StateIndicatorStyle.RectangularLed;
            //sta2.Style = "RectangularLed";
            sta2.FillColor = System.Drawing.Color.Yellow;
            sta2.Parent = "CircularGauges.Default";
            sta2.FillGradientEndColor = System.Drawing.Color.Yellow;
            sta2.Text = "";
            sta2.ValueSource = "Default";
            sta2.Size.Height = float.Parse("6");
            sta2.Size.Width = float.Parse("8");
            sta2.States.Add(state);
            sta2.Location.Y = float.Parse("18");
            sta2.Location.X = float.Parse("80");
            gauge.StateIndicators.Add(sta2);

            StateIndicator sta3 = new StateIndicator();
            sta3.ShadowOffset = float.Parse("2");
            sta3.Style = StateIndicatorStyle.RectangularLed;
            //sta3.Style = "RectangularLed";
            sta3.FillColor = System.Drawing.Color.LimeGreen;
            sta3.Parent = "CircularGauges.Default";
            sta3.FillGradientEndColor = System.Drawing.Color.LimeGreen;
            sta3.Text = "";
            sta3.ValueSource = "Default";
            sta3.Size.Height = float.Parse("6");
            sta3.Size.Width = float.Parse("8");
            sta3.States.Add(state);
            sta3.Location.Y = float.Parse("27");
            sta3.Location.X = float.Parse("80");
            gauge.StateIndicators.Add(sta3);
            gauge.CircularGauges["circul"].Pointers["point"].Value = double.Parse(numbers);
            gauge.SaveAsImage(HttpContext.Current.Server.MapPath("./TempImages/") + "GaugePic.png");

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

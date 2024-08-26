using System;
using System.Text;
using System.Collections;
using System.Web.UI.WebControls;


namespace GoldNet.Comm.Pic
{
    public class Charts
    {

        public static string EncodeDataURL(string dataURL, bool noCacheStr)
        {
            string result = dataURL;
            if (noCacheStr)
            {
                result += (dataURL.IndexOf("?") != -1) ? "&" : "?";
                result += "FCCurrTime=" + DateTime.Now.ToString().Replace(":", "_");
            }

            return System.Web.HttpUtility.UrlEncode(result);
        }


        private static string RenderChartALL(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS, bool transparent)
        {

            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("<!-- START Script Block for Chart {0} -->" + Environment.NewLine, chartId);
            builder.AppendFormat("<div id='{0}Div' align='center'>" + Environment.NewLine, chartId);
            builder.Append("Chart." + Environment.NewLine);
            builder.Append("</div>" + Environment.NewLine);
            builder.Append("<script type=\"text/javascript\">" + Environment.NewLine);
            builder.AppendFormat("var chart_{0} = new FusionCharts(\"{1}\", \"{0}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\");" + Environment.NewLine, chartId, chartSWF, chartWidth, chartHeight, boolToNum(debugMode), boolToNum(registerWithJS));
            if (strXML.Length == 0)
            {
                builder.AppendFormat("chart_{0}.setDataURL(\"{1}\");" + Environment.NewLine, chartId, strURL);
            }
            else
            {
                builder.AppendFormat("chart_{0}.setDataXML(\"{1}\");" + Environment.NewLine, chartId, strXML);
            }

            if (transparent == true)
            {
                builder.AppendFormat("chart_{0}.setTransparent({1});" + Environment.NewLine, chartId, "true");
            }

            builder.AppendFormat("chart_{0}.render(\"{1}Div\");" + Environment.NewLine, chartId, chartId);
            builder.Append("</script>" + Environment.NewLine);
            builder.AppendFormat("<!-- END Script Block for Chart {0} -->" + Environment.NewLine, chartId);
            return builder.ToString();
        }


        public static string RenderChart(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS, bool transparent)
        {
            return RenderChartALL(chartSWF, strURL, strXML, chartId, chartWidth, chartHeight, debugMode, registerWithJS, transparent);
        }


        public static string RenderChart(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS)
        {
            return RenderChart(chartSWF, strURL, strXML, chartId, chartWidth, chartHeight, debugMode, registerWithJS, false);
        }



        public static string RenderChartHTML(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode)
        {
            return RenderChartHTMLALL(chartSWF, strURL, strXML, chartId, chartWidth, chartHeight, debugMode, false, false);
        }



        public static string RenderChartHTML(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS)
        {
            return RenderChartHTMLALL(chartSWF, strURL, strXML, chartId, chartWidth, chartHeight, debugMode, registerWithJS, false);
        }


        public static string RenderChartHTML(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS, bool transparent)
        {
            return RenderChartHTMLALL(chartSWF, strURL, strXML, chartId, chartWidth, chartHeight, debugMode, registerWithJS, transparent);
        }

        private static string RenderChartHTMLALL(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS, bool transparent)
        {

            StringBuilder strFlashVars = new StringBuilder();
            string flashVariables = String.Empty;
            if (strXML.Length == 0)
            {
                //DataURL Mode
                flashVariables = String.Format("&chartWidth={0}&chartHeight={1}&debugMode={2}&registerWithJS={3}&DOMId={4}&dataURL={5}", chartWidth, chartHeight, boolToNum(debugMode), (registerWithJS), chartId, strURL);
            }
            else
            //DataXML Mode
            {
                flashVariables = String.Format("&chartWidth={0}&chartHeight={1}&debugMode={2}&registerWithJS={3}&DOMId={4}&dataXML={5}", chartWidth, chartHeight, boolToNum(debugMode), boolToNum(registerWithJS), chartId, strXML);
            }

            strFlashVars.AppendFormat("<!-- START Code Block for Chart {0} -->" + Environment.NewLine, chartId);
            strFlashVars.AppendFormat("<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0\" width=\"{0}\" height=\"{1}\" name=\"{2}\" id==\"{2}\" >" + Environment.NewLine, chartWidth, chartHeight, chartId);
            strFlashVars.Append("<param name=\"allowScriptAccess\" value=\"always\" />" + Environment.NewLine);
            strFlashVars.AppendFormat("<param name=\"movie\" value=\"{0}\"/>" + Environment.NewLine, chartSWF);
            strFlashVars.AppendFormat("<param name=\"FlashVars\" value=\"{0}\" />" + Environment.NewLine, flashVariables);
            strFlashVars.Append("<param name=\"quality\" value=\"high\" />" + Environment.NewLine);

            string strwmode = "";
            if (transparent == true)
            {
                strFlashVars.Append("<param name=\"wmode\" value=\"transparent\" />" + Environment.NewLine);
                strwmode = "wmode=\"transparent\"";
            }

            strFlashVars.AppendFormat("<embed src=\"{0}\" FlashVars=\"{1}\" quality=\"high\" width=\"{2}\" height=\"{3}\" name=\"{4}\" id=\"{4}\" allowScriptAccess=\"always\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" {5} />" + Environment.NewLine, chartSWF, flashVariables, chartWidth, chartHeight, chartId, strwmode);
            strFlashVars.Append("</object>" + Environment.NewLine);
            strFlashVars.AppendFormat("<!-- END Code Block for Chart {0} -->" + Environment.NewLine, chartId);
            string FlashXML = "<div id='" + chartId + "Div'>"; FlashXML += strFlashVars.ToString() + "</div>";
            return FlashXML;


        }


        private static int boolToNum(bool value)
        {
            return value ? 1 : 0;
        }

    }


}

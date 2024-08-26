using System;
using System.Web;
using System.Web.Services;

namespace GoldNet.JXKP
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ajax : IHttpHandler
    {
        private HttpContext context;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            this.context = context;

            try
            {
                Run(context);
            }
            catch (Exception err)
            {

            }
        }

        void Run(HttpContext context)
        {
            var method = AjaxRequestHelper.GetMethod(TypeName, MethodName);
            if (method == null)
            {
                System.Threading.Thread.Sleep(1000);
                context.Response.Write(AjaxResult.Error(string.Format("找不到类{0} 方法{1}", TypeName, MethodName)));
                context.ApplicationInstance.CompleteRequest();
                return;
            }


            object obj = null;
            if (IncludeHttpContext)
            {
                var objtype = AjaxRequestHelper.GetType(TypeName);
                obj = Activator.CreateInstance(objtype, new object[1] { context });
            }
            else if (IsInstance)
            {
                var objtype = AjaxRequestHelper.GetType(TypeName);
                obj = Activator.CreateInstance(objtype);
            }
            var result = method.Invoke(obj, AjaxRequestHelper.GetMethodParms(method, context));
            context.Response.Write(result);
            context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// 类名
        /// </summary>
        protected string TypeName
        {
            get { return this.context.Request["type"]; }
        }
        /// <summary>
        /// 方法名
        /// </summary>
        protected string MethodName
        {
            get { return this.context.Request["method"]; }
        }
        /// <summary>
        /// 是否以httpcontext为第一个参数，创建一个实例
        /// </summary>
        protected bool IncludeHttpContext
        {
            get { return !string.IsNullOrEmpty(this.context.Request["HttpContext"]); }
        }

        /// <summary>
        /// 是否 实例化对象
        /// </summary>
        public bool IsInstance
        {
            get { return !string.IsNullOrEmpty(this.context.Request["Instance"]); }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

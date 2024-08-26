using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using GoldNet.JXKP.Templet.BLL.Fields;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
namespace GoldNet.JXKP.Templet.BLL.Fields
{
    class FildeGuide
    {
        #region   私有变量
        private GuideInfo _guideInfo;

        #endregion

        #region   内部实体
        public class GuideInfo
        {
            public int ID;                    //指标名称ID
            public string GuideName;          //指标名称
            public int GuideTypeID;           //指标类别ID
            public string GuideType;          //指标类别
            public string TypeSign;           //指标类别标志  0为公共指标，1为专业指标
            public double GuideNum;           //指标分值
            public int TempletID;             //模板列ID
            public int DateCol;               //时间列ID
            public int TargetCol;             //科室列ID
            public int GuideNameCol;          //考评内容列ID
            public int GuideNameColValue;     //考评值列ID

            public GuideInfo(int id, string guideName, int guideTypeID, string guideType, string typeSign, double guideNum, int templetID, int dateCol, int targetCol, int guideNameCol, int guideNameColValue)
            {
                ID = id;
                GuideName = guideName;
                GuideTypeID = guideTypeID;
                GuideType = guideType;
                TypeSign = typeSign;
                GuideNum = guideNum;
                TempletID = templetID;
                DateCol = dateCol;
                TargetCol = targetCol;
                GuideNameCol = guideNameCol;
                GuideNameColValue = guideNameColValue;
            }

            public GuideInfo() { }

        }
        #endregion

        #region  构造函数
        public FildeGuide(string guideName)
        {
            _guideInfo = getGuideByName(guideName);
        }

        public FildeGuide(int id, string guideName, int guideTypeID, string guideType, string typeSign, double guideNum, int templetID, int dateCol, int targetCol, int guideNameCol, int guideNameColValue)
		{
            _guideInfo = new GuideInfo();
			_guideInfo.ID = id;
			_guideInfo.GuideName = guideName;
			_guideInfo.GuideTypeID = guideTypeID;
			_guideInfo.GuideType = guideType;
			_guideInfo.TypeSign = typeSign;
			_guideInfo.GuideNum = guideNum;
			_guideInfo.TempletID = templetID;
			_guideInfo.DateCol = dateCol;
			_guideInfo.TargetCol = targetCol;
			_guideInfo.GuideNameCol = guideNameCol;
			_guideInfo.GuideNameColValue = guideNameColValue;
		}
		#endregion

        #region  非公有方法
        /// <summary>
        /// 获得内部的VO
        /// </summary>
        /// <param name="guideName">指标名称</param>
        /// <returns>返回指定名称的指标的ID，指标类型的ID......</returns>
        private GuideInfo getGuideByName(string guideName)
        {
            Goldnet.Dal.ZLGL_Guide_Dict dal = new Goldnet.Dal.ZLGL_Guide_Dict();
            DataSet ds = dal.getGuideByName(guideName);
            if (ds.Tables[0].Rows.Count != 0)
            {
                int intID = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                string strGuideName = ds.Tables[0].Rows[0][1].ToString();
                int intGuideTypeID = Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString());
                string strGuideType = ds.Tables[0].Rows[0][3].ToString();
                string strTypeSign = ds.Tables[0].Rows[0][4].ToString();
                double dGuideNum = Convert.ToDouble(ds.Tables[0].Rows[0][5].ToString());
                int intTempletID = Convert.ToInt32(ds.Tables[0].Rows[0][6].ToString());
                int intDateCol = Convert.ToInt32(ds.Tables[0].Rows[0][7].ToString());
                int intTargetCol = Convert.ToInt32(ds.Tables[0].Rows[0][8].ToString());
                int intGuideNameCol = Convert.ToInt32(ds.Tables[0].Rows[0][9].ToString());
                int intGuideNameColValue = Convert.ToInt32(ds.Tables[0].Rows[0][10].ToString());

                GuideInfo guideInfo = new GuideInfo(intID, strGuideName, intGuideTypeID, strGuideType, strTypeSign, dGuideNum, intTempletID, intDateCol, intTargetCol, intGuideNameCol, intGuideNameColValue);
                return guideInfo;
            }
            else
            {
                throw new GuideNameNotExistedException(guideName);
            }
        }
      
        
        #endregion

        #region  属性
        /// <summary>
        /// 指标名称ID
        /// </summary>
        public int ID
        {
            get
            {
                return _guideInfo.ID;
            }
        }

        /// <summary>
        /// 指标名称
        /// </summary>
        public string GuideName
        {
            get
            {
                return _guideInfo.GuideName;
            }
        }

        /// <summary>
        /// 指标类别ID
        /// </summary>
        public int GuideTypeID
        {
            get
            {
                return _guideInfo.GuideTypeID;
            }
        }

        /// <summary>
        /// 指标类别
        /// </summary>
        public string GuideType
        {
            get
            {
                return _guideInfo.GuideType;
            }
        }

        /// <summary>
        /// 指标类别标志
        /// </summary>
        public string TypeSign
        {
            get
            {
                return _guideInfo.TypeSign;
            }
        }

        /// <summary>
        /// 指标分值
        /// </summary>
        public double GuideNum
        {
            get
            {
                return _guideInfo.GuideNum;
            }
        }

        /// <summary>
        /// 模板列ID
        /// </summary>
        public int TempletID
        {
            get
            {
                return _guideInfo.TempletID;
            }
        }

        /// <summary>
        /// 时间列ID
        /// </summary>
        public int DateCol
        {
            get
            {
                return _guideInfo.DateCol;
            }
        }

        /// <summary>
        /// 科室列ID
        /// </summary>
        public int TargetCol
        {
            get
            {
                return _guideInfo.TargetCol;
            }
        }

        /// <summary>
        /// 考评内容列ID
        /// </summary>
        public int GuideNameCol
        {
            get
            {
                return _guideInfo.GuideNameCol;
            }
        }

        /// <summary>
        /// 考评值列ID
        /// </summary>
        public int GuideNameColValue
        {
            get
            {
                return _guideInfo.GuideNameColValue;
            }
        }

        #endregion
    }
}

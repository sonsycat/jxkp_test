using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

namespace GoldNet.Comm
{
    public class Utility
    {

        public static object[] ConvertDataTableToObjectArr(DataTable dt) 
        {
            
            object[] objdata = new object[dt.Rows.Count];
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object[] rowdata = new object[dt.Columns.Count];

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    rowdata[j] = dt.Rows[i][j];
                }
                objdata[i] = rowdata;

            }
    
            return objdata;
        
        }

    }
}

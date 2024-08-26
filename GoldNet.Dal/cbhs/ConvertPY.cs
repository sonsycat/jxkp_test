using System;
using System.Collections.Generic;
using System.Text;

namespace Goldnet.Dal
{
    public class ConvertPY
    {
        public string GetPYString(string str)
        {
            string tempStr = "";
            foreach(char c in str)
            {
               
                tempStr += GetPYChar(c.ToString());               
            }
            return tempStr;
        }
        public string GetPYChar(string c)
        {
            int i =0;
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            if (array.Length < 2)
            {
                return c.ToString();
            }
            else
            {
                 i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            }
            if(i<0xB0A1) 
            {
                return "";
            }
            else if(i<0xB0C5)
            {
                return "A";
            }
            else if (i < 0xB2C1)
            {
                return "B";
            }
            else if (i < 0xB4EE)
            {
                return "C";
            }
            else if (i < 0xB6EA)
            {
                return "D";
            }
            else if (i < 0xB7A2)
            {
                return "E";
            }
            else if (i < 0xB8C1)
            {
                return "F";
            }
            else if (i < 0xB9FE)
            {
                return "G";
            }
            else if (i < 0xBBF7)
            {
                return "H";
            }
            else if (i < 0xBFA6)
            {
                return "J";
            }
            else if (i < 0xC0AC)
            {
                return "K";
            }
            else if (i < 0xC2E8)
            {
                return "L";
            }
            else if (i < 0xC4C3)
            {
                return "M";
            }
            else if (i < 0xC5B6)
            {
                return "N";
            }
            else if (i < 0xC5BE)
            {
                return "O";
            }
            else if (i < 0xC6DA)
            {
                return "P";
            }
            else if (i < 0xC8BB)
            {
                return "Q";
            }
            else if (i < 0xC8F6)
            {
                return "R";
            }
            else if (i < 0xCBFA)
            {
                return "S";
            }
            else if (i < 0xCDDA)
            {
                return "T";
            }
            //else if (i < 0xCEF4)
            //{
            //    return "u";
            //}
            //else if (i < 0xC6DA)
            //{
            //    return "v";
            //}
            else if (i < 0xCEF4)
            {
                return "W";
            }
            else if (i < 0xD1B9)
            {
                return "X";
            }
            else if (i < 0xD4D1)
            {
                return "Y";
            }
            else if (i < 0xD7FA)
            {
                return "Z";
            }
            else
            {
                return "";
            }
        }
    }
}

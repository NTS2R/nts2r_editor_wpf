using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nts2r_editor_wpf
{
    public static class Utils
    {
        private static byte[] _nesData;
        private static string _nesFileUrl;
        private static string _nesExcelUrl;

        public static bool OpenFile(string fileUrl)
        {
            _nesFileUrl = fileUrl;
            Debug.WriteLine(_nesFileUrl);
            _nesData = Utils.FileToByte(_nesFileUrl);
            _nesExcelUrl = _nesFileUrl.Replace(".nes", ".xlsx");
            return true;
        }

        public static string GetExcelUrl()
        {
            return _nesExcelUrl;
        }

//        public static byte[] GetNesData()
//        {
//            return _nesData;
//        }

        public static byte GetNesByte(int offset)
        {
            return _nesData[offset];
        }

        public static bool SetNesByte(int offset, byte value)
        {
            _nesData[offset] = value;
            return true;
        }

        public static byte GetMapper()
        {
            byte low = (byte) (GetNesByte(0x06) >> 4);
            byte high = GetNesByte(0x07);
            return (byte) (high | low);
        }

        public static byte GetCompositeLimitLevel(int index)
        {
            return GetNesByte(Config.GetCompositeLevelLimitAddress() + index);
        }

        public static byte GetAttackCount(int index)
        {
            return GetNesByte(Config.GetAttackCountAddress() + index);
        }

        public static byte GetStratagemCount(int index)
        {
            return GetNesByte(Config.GetStratagemCountAddress() + index);
        }

        public static short GetMilitaryLimit(int index)
        {
            return Config.GetMilitaryLimit(index);
        }

        public static bool SetMapper(byte mapperValue)
        {
            byte offset6 = (byte)((GetNesByte(0x06) & 0x0F) | ((mapperValue & 0x0F) << 4));
            SetNesByte(0x06, offset6);
            byte offset7 = (byte)((GetNesByte(0x07) & 0x0F) | (mapperValue & 0xF0));
            SetNesByte(0x07, offset7);
            return true;
        }

        public static byte[] FileToByte(string fileUrl)
        {
            try
            {
                using (FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read))
                {
                    byte[] byteArray = new byte[fs.Length];
                    fs.Read(byteArray, 0, byteArray.Length);
                    return byteArray;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}

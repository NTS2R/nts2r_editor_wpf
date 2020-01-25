using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

        public static bool ParseConfig()
        {
            return Config.ParseConfig();
        }

        public static string GetChsName(byte[] nameBytes)
        {
            var chsName = string.Empty;
            var getNameBytes = new byte[2];
            foreach (var currentByte in nameBytes)
            {
                if (currentByte >= 0xB0)
                {
                    getNameBytes[0] = currentByte;
                }
                else
                {
                    getNameBytes[1] = currentByte;
                    var word = Config.GetChsNameWord(getNameBytes);
                    chsName += word;
                }
            }

            return chsName;
        }

        public static string GetChtName(byte[] nameBytes, byte nameControl)
        {
            if (nameBytes.Length != 3) return "";
            var chtName = string.Empty;
            for (int i = 0; i < 3; i++)
            {
                byte areaIndex = (byte) (((1 << (2 - i)) & nameControl) > 0 ? 1 : 0);
                chtName += Config.GetChtNameWord(nameBytes[i], areaIndex);
            }

            return chtName;
        }

        public static string GetDegradeName(byte degrade)
        {
            return Config.GetDegradeName(degrade);
        }

        public static string GetTerrainName(byte terrian)
        {
            return Config.GetTerrainName(terrian);
        }

        public static Dictionary<byte, Tuple<string, byte>> GetAllGeneral()
        {
            var indexWithGenernal = new Dictionary<byte, Tuple<string, byte>>();
            (var generalAddress, var genernalDictionary) = Config.GetGeneralAddressWithDictionary();
            var flagToGeneralSkill = new Dictionary<byte, string>();
            foreach (var item in genernalDictionary)
            {
                Debug.WriteLine($"key : {item.Value.Flag:x2}, value: {item.Key}");
                flagToGeneralSkill.Add(item.Value.Flag, item.Key);
            }

            for (int index = 0x00; index <= 0xFF; index++)
            {
                var flag = GetNesByte(generalAddress + index);
                if (flagToGeneralSkill.ContainsKey(flag))
                {
                    var generalSkillName = flagToGeneralSkill[flag];
                    var generalSKillAddress = genernalDictionary[generalSkillName].Address;
                    var data = GetNesByte(generalSKillAddress + index);
                    indexWithGenernal.Add(
                        Convert.ToByte(index),
                        new Tuple<string, byte>(generalSkillName, data)
                    );
                }
                else
                {
                    indexWithGenernal.Add(
                        Convert.ToByte(index),
                        new Tuple<string, byte>(string.Empty, 0)
                    );
                }
            }

            return indexWithGenernal;
        }

        public static byte[] GetNotCompositeAsObject()
        {
            var list = new List<byte>();
            var address = Config.GetMilitaryNotCompositeAsObjectAddress();
            for (int index = 0x00; index <= 0x7F; index++)
            {
                var militaryIndex = GetNesByte(address + index);
                list.Add(militaryIndex);
            }

            return list.Distinct().ToArray();
        }

        public static byte[] GetNotCompositeToObject()
        {
            var list = new List<byte>();
            var address = Config.GetMilitaryNotCompositeToObjectAddress();
            for (int index = 0x00; index <= 0x7F; index++)
            {
                var militaryIndex = GetNesByte(address + index);
                list.Add(militaryIndex);
            }
            return list.Distinct().ToArray();
        }
    }
}

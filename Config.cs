using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nts2r_editor_wpf
{
    public static class Config
    {
        // ToDo Config 不应该依赖Utils

        private const int militaryBaseAddress = 0x64010;
        private const int militaryLowIndexAddress = 0x6DE10;
        private const int militaryHighIndexAddress = 0x6DF10;
        private const int militaryLeastLength = 25;
        private const int compositeLevelLimitAddress = 0xF9910;
        private const int militaryAttackCountAddress = 0xF8010;
        private const int militaryStratagemCountAddress = 0xFB210;
        private const int militrayLimitLowAddress = 0xF8210;
        private const int militrayLimitHighAddress = 0xF8310;

        // ToDo 错误使用Utils
        public static int GetMilitaryBaseAddress(int index)
        {
            var low = Utils.GetNesByte(militaryLowIndexAddress + index);
            var high = Utils.GetNesByte(militaryHighIndexAddress + index);
            var address = militaryBaseAddress + high * 0x100 + low;
            return address;
        }

        // ToDo 错误划分职责
        public static short GetMilitaryLimit(int index)
        {
            var low = Utils.GetNesByte(militrayLimitLowAddress + index);
            var high = Utils.GetNesByte(militrayLimitHighAddress + index);
            return (short) (high * 0x100 + low);
        }

        public static int GetMilitaryLeastLength()
        {
            return militaryLeastLength;
        }

        public static int GetCompositeLevelLimitAddress()
        {
            return compositeLevelLimitAddress;
        }

        public static int GetAttackCountAddress()
        {
            return militaryAttackCountAddress;
        }

        public static int GetStratagemCountAddress()
        {
            return militaryStratagemCountAddress;
        }
    }
}

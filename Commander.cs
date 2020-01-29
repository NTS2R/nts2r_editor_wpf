using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nts2r_editor_wpf
{
    public class Commander
    {
        private List<Military> _militaries = new List<Military>();
        public Commander()
        {
            var leastLength = Config.GetMilitaryLeastLength();
            for (int index = 0x00; index <= 0xFF; index++)
            {
                var startAddress = Utils.GetMilitaryStartAddress(index);
                List<byte> data = new List<byte>();

                for (int i = 0; i < leastLength; i++)
                {
                    data.Add(Utils.GetNesByte(startAddress + i));
                }

                int offset = 0;

                while (true)
                {
                    var theByte = Utils.GetNesByte(startAddress + leastLength + offset);
                    data.Add(theByte);
                    if (theByte == 0xFF)
                        break;
                    offset++;
                }

                var military = new Military(data.ToArray(), startAddress, index);
                _militaries.Add(military);
            }
        }

        public Military GetMilitary(int index)
        {
            return _militaries[index];
        }
    }
}

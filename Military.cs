using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nts2r_editor_wpf
{
    public class Military
    {
        public struct Skill
        {
            public byte qi { get; set; } // 5  0x80 （奇）奇门遁甲
            public byte ren { get; set; } // 7  0x80 （仁）仁德
            public byte hui { get; set; } // 7  0x40 （慧）聪慧
            public byte dang { get; set; }// 7  0x20 （挡）格挡
            public byte bi { get; set; }  // 12 0x80 （避）闪避
            public byte gong { get; set; }// 12 0x40 （攻）强袭
            public byte wu { get; set; }  // 12 0x20 （武）勇武
            public byte zhi { get; set; } // 12 0x10 （智）智囊
            public byte shu { get; set; } // 12 0x08 （术）策略
            public byte fan { get; set; } // 13 0x80 （返）反甲
            public byte hun { get; set; } // 13 0x40 （魂）军魂
            public byte jue { get; set; } // 13 0x20 （觉）觉醒
            public byte fang { get; set; }// 20 0x80 （防）坚守
            public byte mou { get; set; } // 20 0x40 （谋）神算
            public byte liao { get; set; }// 20 0x20 （疗）妙手
            public byte lin { get; set; } // 20 0x10 （临）攻城
            public byte shi { get; set; } // 20 0x08 （识）勘察
            public byte fen { get; set; } // 20 0x04 （奋）奋勇
            public byte tong { get; set; }// 20 0x02 （统）统御
            public byte ming { get; set; }// 20 0x01 （命）命中
        }

        struct Admiral
        {
            public byte admiralCategory;
            public byte gong;
            public byte fang;
            public byte ming;
            public byte bi;
        }

        private List<byte> _militaryData; //人物数据
        public int Address { get; set; }
        public Skill _skill;
        private Admiral _admiral;
        private byte _color; //0模型颜色
        public byte Chapter { get; set; } //0章节
        private byte _model; //1模型
        public byte Force { get; set; } //2武力
        public byte Wit { get; set; } //3智力
        public byte Speed { get; set; } //4速度
        private byte _enemyTroopsCategory; // 5 敌方流派 0x7F
        private byte _troopsCategory; // 6 我方流派
        private byte _enemyStratagemCategory; // 7 敌方谋略流派 0x1F
        private byte _enemyStratagemValue; //8 敌方谋略值
        private byte _enemyAttackValue; //9 敌方攻击力
        private byte _enemyDefenseValue; //10 敌方防御力
        public byte Terrain { get; set; } // 11 地形 0xF0
        private byte _stratagemCategory; // 11 计策流派 0x0F
        public byte DegradeCategory { get; set; } // 12 武器
        private byte _treasureCategory; //13 宝物 0x1F
        private byte[] _faceBytes = new byte[6]; // 14-19 脸谱
        private byte _faceControl; // 21 脸谱控制 0xF0
        public byte ChtNameControl { get; set; } // 21 繁体名字控制 0x0F;
        public byte[] ChtNameBytes = new byte[3]; // 22-24 繁体名字
        public List<byte> ChsNameBytes = new List<byte>();
        public byte CompositeLimitLevel { get; set; } //合成等级控制
        public byte AttackCount { get; set; } // 攻击次数
        public byte StratagemCount { get; set; } // 策略次数
        public short MilitaryLimit { get; set; } // 攻击倍率
        public Military(byte[] militaryData, int address, int index)
        {
            Address = address;
            _militaryData = militaryData.ToList();
            var offset0 = _militaryData[0];
            _color = (byte) (offset0 & 0x0F);
            Chapter = (byte) (offset0 >> 4);
            _model = _militaryData[1];
            Force = _militaryData[2];
            Wit = _militaryData[3];
            Speed = _militaryData[4];
            var offset5 = _militaryData[5];
            _enemyTroopsCategory = (byte) (offset5 & 0x7F);
            _skill.qi = (byte)((offset5 & 0x80) >> 7);
            _troopsCategory = _militaryData[6];
            _enemyStratagemCategory = (byte) (_militaryData[7] & 0x1F);
            _skill.ren = (byte)((_militaryData[7] & 0x80) >> 7);
            _skill.hui = (byte)((_militaryData[7] & 0x40) >> 6);
            _skill.dang = (byte)((_militaryData[7] & 0x20) >> 5);
            _enemyStratagemValue = _militaryData[8];
            _enemyAttackValue = _militaryData[9];
            _enemyDefenseValue = _militaryData[10];
            Terrain = (byte) ((_militaryData[11] & 0xF0) >> 4);
            _stratagemCategory = (byte)(_militaryData[11] & 0x0F);
            DegradeCategory = (byte)(_militaryData[12] & 0x07);
            _skill.bi = (byte)((_militaryData[12] & 0x80) >> 7);
            _skill.gong = (byte)((_militaryData[12] & 0x40) >> 6);
            _skill.wu = (byte)((_militaryData[12] & 0x20) >> 5);
            _skill.zhi = (byte)((_militaryData[12] & 0x10) >> 4);
            _skill.shu = (byte)((_militaryData[12] & 0x08) >> 3);
            _skill.fan = (byte)((_militaryData[13] & 0x80) >> 7);
            _skill.hun = (byte)((_militaryData[13] & 0x40) >> 6);
            _skill.jue = (byte)((_militaryData[13] & 0x20) >> 5);
            _treasureCategory = (byte)(_militaryData[13] & 0x1F);
            for (int i = 0; i < 6; i++)
            {
                _faceBytes[i] = _militaryData[14 + i];
            }

            _skill.fang = (byte)((_militaryData[20] & 0x80) >> 7);
            _skill.mou = (byte)((_militaryData[20] & 0x40) >> 6);
            _skill.liao = (byte)((_militaryData[20] & 0x20) >> 5);
            _skill.lin = (byte)((_militaryData[20] & 0x10) >> 4);
            _skill.shi = (byte)((_militaryData[20] & 0x08) >> 3);
            _skill.fen = (byte)((_militaryData[20] & 0x04) >> 2);
            _skill.tong = (byte)((_militaryData[20] & 0x02) >> 1);
            _skill.ming = (byte)(_militaryData[20] & 0x01);

            _faceControl = (byte)((_militaryData[21] & 0xF0) >> 4);
            ChtNameControl = (byte)(_militaryData[21] & 0x0F);
            for (int i = 0; i < 3; i++)
            {
                ChtNameBytes[i] = _militaryData[22 + i];
            }

            for (int i = 25; i < _militaryData.Count; i++)
            {
                ChsNameBytes.Add(_militaryData[i]);
            }

            CompositeLimitLevel = Utils.GetCompositeLimitLevel(index);
            AttackCount = Utils.GetAttackCount(index);
            StratagemCount = Utils.GetStratagemCount(index);
            MilitaryLimit = Utils.GetMilitaryLimit(index);
        }
    }
}

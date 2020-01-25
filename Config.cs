using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace nts2r_editor_wpf
{
    public static class Config
    {
        // ToDo Config 不应该依赖Utils
        public struct GeneralPair
        {
            public byte Flag;
            public int Address;

            public GeneralPair(byte flag, int address)
            {
                Flag = flag;
                Address = address;
            }
        };

        private const int militaryBaseAddress = 0x64010;
        private const int militaryLowIndexAddress = 0x6DE10;
        private const int militaryHighIndexAddress = 0x6DF10;
        private const int militaryLeastLength = 25;
        private const int compositeLevelLimitAddress = 0xF9910;
        private const int militaryAttackCountAddress = 0xF8010;
        private const int militaryStratagemCountAddress = 0xFB210;
        private const int militrayLimitLowAddress = 0xF8210;
        private const int militrayLimitHighAddress = 0xF8310;
        private static int militaryGeneralAddress;
        private static int militaryNotCompositeAsObjectAddress = 0xF9810;
        private static int militaryNotCompositeToObjectAddress = 0xF9890;
        private static Dictionary<Tuple<byte, byte>, string> chsNameLibrary = new Dictionary<Tuple<byte, byte>, string>();
        private static Dictionary<Tuple<byte, byte>, string> chtNameLibrary = new Dictionary<Tuple<byte, byte>, string>();
        private static Dictionary<byte, string> degradeDictionary = new Dictionary<byte, string>();
        private static Dictionary<byte, string> terrainDictionary = new Dictionary<byte, string>();
        private static Dictionary<string, GeneralPair> generalDataDictionary = new Dictionary<string, GeneralPair>();

        public static int GetMilitaryNotCompositeAsObjectAddress()
        {
            return militaryNotCompositeAsObjectAddress;
        }

        public static int GetMilitaryNotCompositeToObjectAddress()
        {
            return militaryNotCompositeToObjectAddress;
        }

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

        public static bool ParseConfig()
        {
            var currentDirectoryUri = System.Environment.CurrentDirectory;
            Debug.WriteLine($"Current Directory: {currentDirectoryUri}");
            var configDirectoryUri = currentDirectoryUri + "/config";
            Debug.WriteLine($"Config Directory: {configDirectoryUri}");
            if (!Directory.Exists(configDirectoryUri))
            {
                MessageBox.Show("config目录不存在，请检查", "请检查config目录");
                return false;
            }
            ParseConfigChsName(configDirectoryUri);
            ParseConfigChtName(configDirectoryUri);
            ParseConfigDegrade(configDirectoryUri);
            ParseConfigTerrain(configDirectoryUri);
            ParseConfigGeneral(configDirectoryUri);
            return true;
        }

        private static void ParseConfigGeneral(string fileNameDirectoryUri)
        {
            generalDataDictionary.Clear();
            const string fileName = "general_name.json";
            var fullUrl = fileNameDirectoryUri + "/" + fileName;
            Debug.WriteLine($"general_name file fullUri: {fullUrl}");
            try
            {
                using (var sr = new StreamReader(fullUrl))
                {
                    var text = sr.ReadToEnd();
                    var generalNameJson =
                        JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(text);
                    militaryGeneralAddress = Convert.ToInt32(generalNameJson["地址"]["判断地址"], 16);
                    Debug.WriteLine($"militaryGeneralAddress: {militaryGeneralAddress:X}");
                    foreach (var item in generalNameJson["名字"])
                    {
                        var pair = new GeneralPair(
                            Convert.ToByte(item.Value, 16),
                            Convert.ToInt32(generalNameJson["地址"][item.Key], 16));
                        Debug.WriteLine($"{item.Key} - {pair.Flag:x2}, {pair.Address:x6}");
                        generalDataDictionary.Add(item.Key, pair);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static Tuple<int, Dictionary<string, GeneralPair>> GetGeneralAddressWithDictionary()
        {
            return new Tuple<int, Dictionary<string, GeneralPair>>(militaryGeneralAddress, generalDataDictionary);
        }

        private static void ParseConfigChsName(string fileNameDirectoryUri)
        {
            chsNameLibrary.Clear();
            //name file
            for (var i = 0xB0; i <= 0xBF; i++)
            {
                var fileName = $"{i.ToString("X").ToLower()}.dat";
                var fullUrl = fileNameDirectoryUri + "/" + fileName;
                Debug.WriteLine($"Name FullUri: {fullUrl}");
                try
                {
                    using (var sr = new StreamReader(fullUrl))
                    {
                        var text = sr.ReadToEnd();
                        text = text.Replace('\n', ' ');
                        var textSet = text.Split(' ');
                        foreach (var word in textSet)
                        {
                            if (word.Length < 3) continue;
                            var index = word.Substring(0, 2);
                            var chineseWord = word.Substring(2, 1);
                            var chsNameLibraryIndex = new Tuple<byte, byte>((byte)i, Convert.ToByte(index, 16));
                            Debug.WriteLine($"index: ({i:X}, {index}), word: {chineseWord}");
                            chsNameLibrary.Add(chsNameLibraryIndex, chineseWord);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public static string GetChsNameWord(byte[] chsNameBytes)
        {
            var chsNameTuple = new Tuple<byte, byte>(chsNameBytes[0], chsNameBytes[1]);
            return chsNameLibrary[chsNameTuple];
        }

        private static void ParseConfigChtName(string fileNameDirectoryUri)
        {
            chtNameLibrary.Clear();
            for (var i = 0x0; i <= 0x1; i++)
            {
                var fileName = $"name{i+1}.dat";
                var fullUrl = fileNameDirectoryUri + "/" + fileName;
                Debug.WriteLine($"Name FullUri: {fullUrl}");
                try
                {
                    using (var sr = new StreamReader(fullUrl))
                    {
                        var text = sr.ReadToEnd();
                        var textSet = text.Replace('\n', ' ').Replace(" ", "");
                        byte count = 0;
                        foreach (var word in textSet.Where(word => word != ' '))
                        {
                            Debug.WriteLine($"index: ({i}, {count:X}), word: {word}");
                            var chtNameLibraryIndex = new Tuple<byte, byte>((byte)i, count);
                            chtNameLibrary.Add(chtNameLibraryIndex, word.ToString());
                            count++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public static string GetChtNameWord(byte chtName, byte chtNameControl)
        {
            var chtNameTuple = new Tuple<byte, byte>(chtNameControl, chtName);
            return chtNameLibrary.ContainsKey(chtNameTuple) ? chtNameLibrary[chtNameTuple] : string.Empty;
        }

        private static void ParseConfigDegrade(string fileNameDirectoryUri)
        {
            degradeDictionary.Clear();
            const string fileName = "degrade.dat";
            var fullUrl = fileNameDirectoryUri + "/" + fileName;
            Debug.WriteLine($"Degrade file fullUri: {fullUrl}");
            try
            {
                using (var sr = new StreamReader(fullUrl))
                {
                    var text = sr.ReadToEnd();
                    var degradeJson = JsonSerializer.Deserialize<Dictionary<string, string>>(text);
                    foreach (var item in degradeJson)
                    {
                        Debug.WriteLine($"{item.Key}: {item.Value}");
                        degradeDictionary.Add(Convert.ToByte(item.Key, 16), item.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static string GetDegradeName(byte degrade)
        {
            return degradeDictionary.ContainsKey(degrade) ? degradeDictionary[degrade] : string.Empty;
        }

        private static void ParseConfigTerrain(string fileNameDirectoryUri)
        {
            
            terrainDictionary.Clear();
            const string fileName = "hero_terrian_name.dat"; // 兼容迷恋C编辑器配置文件 terrian 错误不变更
            var fullUrl = fileNameDirectoryUri + "/" + fileName;
            Debug.WriteLine($"hero_terrian_name file fullUri: {fullUrl}");
            try
            {
                using (var sr = new StreamReader(fullUrl))
                {
                    var text = sr.ReadToEnd();
                    var terrainJson = JsonSerializer.Deserialize<Dictionary<string, string>>(text);
                    foreach (var item in terrainJson)
                    {
                        Debug.WriteLine($"{item.Key}: {item.Value}");
                        terrainDictionary.Add(Convert.ToByte(item.Key, 16), item.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static string GetTerrainName(byte terrain)
        {
            return terrainDictionary.ContainsKey(terrain) ? terrainDictionary[terrain] : string.Empty;
        }
    }
}

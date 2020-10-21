using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static LinStats.Character;

namespace LinStats
{
    public abstract class Character
    {
        public string name;
        public string role;

        public int level = 0;
        public int hp = 0;
        public int mp = 0;
        public int elixirsUsed = 0;
        public int highestLevel = 0;
        public int baseHpPerLevel;

        public bool initialStatsAllocated = false;

        public int[,] mpDisArray  = {
                { 0, 0, 0 ,0 ,0 ,0 ,0, 0, 0, 0 },
                { 0 , -1, -1, -1 ,-1 , -1, -1, -1, -1, -1},
                { 0 , -1, -2, -2 ,-2 , -2, -2, -2, -2, -2},
                { 0 , -1, -2, -3 ,-3 , -3, -3, -3, -3, -3},
                { 0 , -1, -2, -3 ,-4 , -4, -4, -4, -4, -4},
                { 0 , -1, -2, -3 ,-4 , -5, -5, -5, -5, -5},
                { 0 , -1, -2, -3 ,-4 , -5, -6, -6, -6, -6},
                { 0 , -1, -2, -3 ,-4 , -5, -6, -7, -7, -7},
                { 0 , -1, -2, -3 ,-4 , -5, -6, -7, -7, -7},
                { 0 , -1, -2, -3 ,-4 , -5, -6, -7, -7, -7},
                { 0 , -1, -2, -3 ,-4 , -5, -6, -7, -7, -7},
                { 0 , -1, -2, -3 ,-4 , -5, -6, -7, -8, -8},
                { 0 , -1, -2, -3 ,-4 , -5, -6, -7, -8, -8},
                { 0 , -1, -2, -3 ,-4 , -5, -6, -7, -8, -8},
                { 0 , -1, -2, -3 ,-4 , -5, -6, -7, -8, -8},
                { 0 , -1, -2, -3 ,-4 , -5, -6, -7, -8, -9},
            };

        public Dictionary<string, int> baseStat = new Dictionary<string, int>()
        {
            {"str", 0 }, {"dex", 0 }, {"con", 0 }, {"int", 0 }, {"wis", 0 }, {"cha", 0 }, {"bon", 0}
        };

        public Dictionary<string, int> maxBase = new Dictionary<string, int>()
        {
            {"str", 0 }, {"dex", 0 }, {"con", 0 }, {"int", 0 }, {"wis", 0 }, {"cha", 0 }
        };

        public Dictionary<string, int> minBase = new Dictionary<string, int>()
        {
            {"str", 0 }, {"dex", 0 }, {"con", 0 }, {"int", 0 }, {"wis", 0 }, {"cha", 0 }
        };

        public Dictionary<string, int> statBonuses = new Dictionary<string, int>()
        {
            {"hpPerLevel", 0 }, { "hpRegen", 0}, { "meleeDamage", 0}, { "meleeHit", 0}, { "er", 0}, { "sp", 0}, { "magicHit", 0}, { "mpDiscount", 0}, { "mpPerLevel", 0},
            { "mpRegen", 0}, { "rangedDamage", 0}, { "rangedHit", 0}, {"magicBonus", 0 }, { "mr", 0}, { "magicCrit", 0}, { "weightCap", 0}, {"ac", 0}, {"magicLevel", 0}
        };

        public Dictionary<string, int> baseStatBonuses = new Dictionary<string, int>()
        {
            {"hpPerLevel", 0 }, { "hpRegen", 0}, { "meleeDamage", 0}, { "meleeHit", 0}, { "er", 0}, { "sp", 0}, { "magicHit", 0}, { "mpDiscount", 0}, { "mpPerLevel", 0},
            { "mpRegen", 0}, { "rangedDamage", 0}, { "rangedHit", 0}, {"magicBonus", 0 },  { "mr", 0}, { "magicCrit", 0}, { "weightCap", 0}, {"ac", 0}, {"magicLevel", 0}
        };

        public int GetMeleeHit()
        {
            return statBonuses["meleeHit"] + baseStatBonuses["meleeHit"];
        }

        public int GetMeleeDamage()
        {
           return statBonuses["meleeDamage"] + baseStatBonuses["meleeDamage"];
        }

        public int GetAc()
        {
            return statBonuses["ac"] + baseStatBonuses["ac"];
        }

        public int GetEr()
        {
            return statBonuses["er"] + baseStatBonuses["er"];
        }

        public int GetMpDiscount()
        {
            int effectiveInt = baseStat["int"] - 12;
            int effectiveMagicLevel = GetMagicLevel() - 1;

            if (effectiveInt >= 15)
            {
                effectiveInt = 15;
            }
            if (effectiveInt <= 0)
            {
                effectiveInt = 0;
            }

            if(effectiveMagicLevel < 0)
            {
                effectiveMagicLevel = 0;
            }

            int discount = mpDisArray[effectiveInt, effectiveMagicLevel];

            return statBonuses["mpDiscount"] + baseStatBonuses["mpDiscount"] + discount;
        }

        public int GetMagicHit()
        {
            return statBonuses["magicHit"] + baseStatBonuses["magicHit"];
        }

        public int GetMr()
        {
            return statBonuses["mr"] + baseStatBonuses["mr"];
        }

        public int GetMpRegen()
        {
            return statBonuses["mpRegen"] + baseStatBonuses["mpRegen"];
        }

        public int GetWeightCap()
        {
            int weightTotal = ((baseStat["str"] + baseStat["con"] + statBonuses["weightCap"] + baseStatBonuses["weightCap"] + 1) / 2) * 150;

            if(weightTotal > 3600)
            {
                weightTotal = 3600;
            }

            return weightTotal;
        }

        public int GetHpPerLevel()
        {
            return baseHpPerLevel + statBonuses["hpPerLevel"] + baseStatBonuses["hpPerLevel"];
        }

        public int GetHpRegen()
        {
            return statBonuses["hpRegen"] + baseStatBonuses["hpRegen"];
        }

        public int GetMagicCrit()
        {
            return statBonuses["magicCrit"] + baseStatBonuses["magicCrit"];
        }

        public int GetRangedHit()
        {
            return statBonuses["rangedHit"] + baseStatBonuses["rangedHit"];
        }

        public int GetRangedDamage()
        {
            return statBonuses["rangedDamage"] + baseStatBonuses["rangedDamage"];
        }

        public int GetMagicLevel()
        {
            return statBonuses["magicLevel"];
        }

        public int GetMagicBonus()
        {
            return baseStatBonuses["magicBonus"] + statBonuses["magicBonus"];
        }
        public int GetSp()
        {
            return baseStatBonuses["sp"] + statBonuses["sp"] + GetMagicBonus() + GetMagicLevel();
        }

        public void UseElixir()
        {
            if (elixirsUsed < 5)
            {
                baseStat["bon"]++;
                elixirsUsed++;
            }
        }

        public void CalcMagicLevel()
        {
            if (role == "Knight")
            {
                statBonuses["magicLevel"] = level / 50;
                if (statBonuses["magicLevel"] > 1)
                {
                    statBonuses["magicLevel"] = 1;
                }
            } else if (role == "Wizard")
            {
                statBonuses["magicLevel"] = level / 4;
                if (statBonuses["magicLevel"] > 10)
                {
                    statBonuses["magicLevel"] = 10;
                }
            } else if (role == "Elf")
            {
                statBonuses["magicLevel"] = level / 8;
                if (statBonuses["magicLevel"] > 6)
                {
                    statBonuses["magicLevel"] = 6;
                }

            } else if (role == "Royal")
            {
                statBonuses["magicLevel"] = level / 10;
                if (statBonuses["magicLevel"] > 2)
                {
                    statBonuses["magicLevel"] = 2;
                }
            } else if (role == "Dark Elf")
            {
                statBonuses["magicLevel"] = level / 12;
                if (statBonuses["magicLevel"] > 2) {
                    statBonuses["magicLevel"] = 2;
                }
            } else if (role == "Dragon Knight")
            {
                statBonuses["magicLevel"] = level / 15;
                if (statBonuses["magicLevel"] > 3)
                {
                    statBonuses["magicLevel"] = 3;
                }
            } else if (role == "Illusionist")
            {
                statBonuses["magicLevel"] = level / 10;
                if (statBonuses["magicLevel"] > 4)
                {
                    statBonuses["magicLevel"] = 4;
                }
            }
        }
        public void CalcStatBonus() //calculates general stat bonuses
        {
            switch (baseStat["str"])
            {
                case var exp when (baseStat["str"] >= 48):
                    statBonuses["meleeDamage"] = 15;
                    statBonuses["meleeHit"] = 14;
                    break;
                case var exp when (baseStat["str"] >= 47):
                    statBonuses["meleeDamage"] = 15;
                    statBonuses["meleeHit"] = 13;
                    break;
                case var exp when (baseStat["str"] >= 45):
                    statBonuses["meleeDamage"] = 14;
                    statBonuses["meleeHit"] = 13;
                    break;
                case var exp when (baseStat["str"] >= 43):
                    statBonuses["meleeDamage"] = 14;
                    statBonuses["meleeHit"] = 12;
                    break;
                case var exp when (baseStat["str"] >= 42):
                    statBonuses["meleeDamage"] = 13;
                    statBonuses["meleeHit"] = 12;
                    break;
                case var exp when (baseStat["str"] >= 39):
                    statBonuses["meleeDamage"] = 13;
                    statBonuses["meleeHit"] = 11;
                    break;
                case var exp when (baseStat["str"] >= 36):
                    statBonuses["meleeDamage"] = 12;
                    statBonuses["meleeHit"] = 10;
                    break;
                case var exp when (baseStat["str"] >= 35):
                    statBonuses["meleeDamage"] = 12;
                    statBonuses["meleeHit"] = 9;
                    break;
                case var exp when (baseStat["str"] >= 34):
                    statBonuses["meleeDamage"] = 11;
                    statBonuses["meleeHit"] = 9;
                    break;
                case var exp when (baseStat["str"] >= 33):
                    statBonuses["meleeDamage"] = 10;
                    statBonuses["meleeHit"] = 9;
                    break;
                case var exp when (baseStat["str"] >= 31):
                    statBonuses["meleeDamage"] = 9;
                    statBonuses["meleeHit"] = 8;
                    break;
                case var exp when (baseStat["str"] >= 30):
                    statBonuses["meleeDamage"] = 8;
                    statBonuses["meleeHit"] = 8;
                    break;
                case var exp when (baseStat["str"] >= 29):
                    statBonuses["meleeDamage"] = 8;
                    statBonuses["meleeHit"] = 7;
                    break;
                case var exp when (baseStat["str"] >= 27):
                    statBonuses["meleeDamage"] = 7;
                    statBonuses["meleeHit"] = 7;
                    break;
                case var exp when (baseStat["str"] >= 26):
                    statBonuses["meleeDamage"] = 7;
                    statBonuses["meleeHit"] = 6;
                    break;
                case var exp when (baseStat["str"] >= 24):
                    statBonuses["meleeDamage"] = 6;
                    statBonuses["meleeHit"] = 6;
                    break;
                case var exp when (baseStat["str"] >= 23):
                    statBonuses["meleeDamage"] = 6;
                    statBonuses["meleeHit"] = 5;
                    break;
                case var exp when (baseStat["str"] >= 21):
                    statBonuses["meleeDamage"] = 5;
                    statBonuses["meleeHit"] = 5;
                    break;
                case var exp when (baseStat["str"] >= 19):
                    statBonuses["meleeDamage"] = 4;
                    statBonuses["meleeHit"] = 4;
                    break;
                case var exp when (baseStat["str"] >= 18):
                    statBonuses["meleeDamage"] = 3;
                    statBonuses["meleeHit"] = 4;
                    break;
                case var exp when (baseStat["str"] >= 17):
                    statBonuses["meleeDamage"] = 3;
                    statBonuses["meleeHit"] = 3;
                    break;
                case var exp when (baseStat["str"] >= 16):
                    statBonuses["meleeDamage"] = 2;
                    statBonuses["meleeHit"] = 3;
                    break;
                case var exp when (baseStat["str"] >= 15):
                    statBonuses["meleeDamage"] = 2;
                    statBonuses["meleeHit"] = 2;
                    break;
                case var exp when (baseStat["str"] >= 14):
                    statBonuses["meleeDamage"] = 2;
                    statBonuses["meleeHit"] = 1;
                    break;
                case var exp when (baseStat["str"] >= 13):
                    statBonuses["meleeDamage"] = 1;
                    statBonuses["meleeHit"] = 1;
                    break;
                case var exp when (baseStat["str"] >= 12):
                    statBonuses["meleeDamage"] = 0;
                    statBonuses["meleeHit"] = 1;
                    break;
                case var exp when (baseStat["str"] >= 11):
                    statBonuses["meleeDamage"] = 0;
                    statBonuses["meleeHit"] = 0;
                    break;
                case var exp when (baseStat["str"] >= 10):
                    statBonuses["meleeDamage"] = -1;
                    statBonuses["meleeHit"] = 0;
                    break;
                case var exp when (baseStat["str"] >= 9):
                    statBonuses["meleeDamage"] = -1;
                    statBonuses["meleeHit"] = -1;
                    break;
                case var exp when (baseStat["str"] >= 8):
                    statBonuses["meleeDamage"] = -2;
                    statBonuses["meleeHit"] = -1;
                    break;
                case var exp when (baseStat["str"] >= 7):
                    statBonuses["meleeDamage"] = -2;
                    statBonuses["meleeHit"] = -2;
                    break;
                case var exp when (baseStat["str"] >= 6):
                    statBonuses["meleeDamage"] = -3;
                    statBonuses["meleeHit"] = -2;
                    break;
                case var exp when (baseStat["str"] >= 5):
                    statBonuses["meleeDamage"] = -3;
                    statBonuses["meleeHit"] = -3;
                    break;
                case var exp when (baseStat["str"] >= 4):
                    statBonuses["meleeDamage"] = -4;
                    statBonuses["meleeHit"] = -3;
                    break;
                case var exp when (baseStat["str"] >= 3):
                    statBonuses["meleeDamage"] = -4;
                    statBonuses["meleeHit"] = -4;
                    break;
                case var exp when (baseStat["str"] >= 2):
                    statBonuses["meleeDamage"] = -5;
                    statBonuses["meleeHit"] = -4;
                    break;
                case var exp when (baseStat["str"] >= 1):
                    statBonuses["meleeDamage"] = -5;
                    statBonuses["meleeHit"] = -5;
                    break;
            }
            switch (baseStat["dex"])
            {
                case var exp when (baseStat["dex"] >= 50):
                    statBonuses["rangedDamage"] = 13;
                    statBonuses["rangedHit"] = 25;
                    statBonuses["er"] = 21;
                    break;
                case var exp when (baseStat["dex"] >= 48):
                    statBonuses["rangedDamage"] = 13;
                    statBonuses["rangedHit"] = 25;
                    statBonuses["er"] = 20;
                    break;
                case var exp when (baseStat["dex"] >= 46):
                    statBonuses["rangedDamage"] = 12;
                    statBonuses["rangedHit"] = 25;
                    statBonuses["er"] = 19;
                    break;
                case var exp when (baseStat["dex"] >= 44):
                    statBonuses["rangedDamage"] = 12;
                    statBonuses["rangedHit"] = 25;
                    statBonuses["er"] = 18;
                    break;
                case var exp when (baseStat["dex"] >= 42):
                    statBonuses["rangedDamage"] = 11;
                    statBonuses["rangedHit"] = 25;
                    statBonuses["er"] = 17;
                    break;
                case var exp when (baseStat["dex"] >= 40):
                    statBonuses["rangedDamage"] = 11;
                    statBonuses["rangedHit"] = 25;
                    statBonuses["er"] = 16;
                    break;
                case var exp when (baseStat["dex"] >= 39):
                    statBonuses["rangedDamage"] = 10;
                    statBonuses["rangedHit"] = 25;
                    statBonuses["er"] = 15;
                    break;
                case var exp when (baseStat["dex"] >= 38):
                    statBonuses["rangedDamage"] = 10;
                    statBonuses["rangedHit"] = 24;
                    statBonuses["er"] = 15;
                    break;
                case var exp when (baseStat["dex"] >= 37):
                    statBonuses["rangedDamage"] = 10;
                    statBonuses["rangedHit"] = 23;
                    statBonuses["er"] = 15;
                    break;
                case var exp when (baseStat["dex"] >= 36):
                    statBonuses["rangedDamage"] = 10;
                    statBonuses["rangedHit"] = 22;
                    statBonuses["er"] = 14;
                    break;
                case var exp when (baseStat["dex"] >= 35):
                    statBonuses["rangedDamage"] = 9;
                    statBonuses["rangedHit"] = 21;
                    statBonuses["er"] = 13;
                    break;
                case var exp when (baseStat["dex"] >= 34):
                    statBonuses["rangedDamage"] = 9;
                    statBonuses["rangedHit"] = 20;
                    statBonuses["er"] = 13;
                    break;
                case var exp when (baseStat["dex"] >= 33):
                    statBonuses["rangedDamage"] = 8;
                    statBonuses["rangedHit"] = 18;
                    statBonuses["er"] = 12;
                    break;
                case var exp when (baseStat["dex"] >= 32):
                    statBonuses["rangedDamage"] = 8;
                    statBonuses["rangedHit"] = 18;
                    statBonuses["er"] = 12;
                    break;
                case var exp when (baseStat["dex"] >= 31):
                    statBonuses["rangedDamage"] = 8;
                    statBonuses["rangedHit"] = 17;
                    statBonuses["er"] = 11;
                    break;
                case var exp when (baseStat["dex"] >= 30):
                    statBonuses["rangedDamage"] = 8;
                    statBonuses["rangedHit"] = 16;
                    statBonuses["er"] = 11;
                    break;
                case var exp when (baseStat["dex"] >= 29):
                    statBonuses["rangedDamage"] = 7;
                    statBonuses["rangedHit"] = 15;
                    statBonuses["er"] = 10;
                    break;
                case var exp when (baseStat["dex"] >= 28):
                    statBonuses["rangedDamage"] = 7;
                    statBonuses["rangedHit"] = 14;
                    statBonuses["er"] = 10;
                    break;
                case var exp when (baseStat["dex"] >= 27):
                    statBonuses["rangedDamage"] = 7;
                    statBonuses["rangedHit"] = 13;
                    statBonuses["er"] = 9;
                    break;
                case var exp when (baseStat["dex"] >= 26):
                    statBonuses["rangedDamage"] = 6;
                    statBonuses["rangedHit"] = 12;
                    statBonuses["er"] = 9;
                    break;
                case var exp when (baseStat["dex"] >= 25):
                    statBonuses["rangedDamage"] = 6;
                    statBonuses["rangedHit"] = 11;
                    statBonuses["er"] = 8;
                    break;
                case var exp when (baseStat["dex"] >= 24):
                    statBonuses["rangedDamage"] = 6;
                    statBonuses["rangedHit"] = 10;
                    statBonuses["er"] = 8;
                    break;
                case var exp when (baseStat["dex"] >= 23):
                    statBonuses["rangedDamage"] = 5;
                    statBonuses["rangedHit"] = 9;
                    statBonuses["er"] = 7;
                    break;
                case var exp when (baseStat["dex"] >= 22):
                    statBonuses["rangedDamage"] = 5;
                    statBonuses["rangedHit"] = 8;
                    statBonuses["er"] = 7;
                    break;
                case var exp when (baseStat["dex"] >= 21):
                    statBonuses["rangedDamage"] = 5;
                    statBonuses["rangedHit"] = 7;
                    statBonuses["er"] = 6;
                    break;
                case var exp when (baseStat["dex"] >= 20):
                    statBonuses["rangedDamage"] = 4;
                    statBonuses["rangedHit"] = 6;
                    statBonuses["er"] = 6;
                    break;
                case var exp when (baseStat["dex"] >= 19):
                    statBonuses["rangedDamage"] = 4;
                    statBonuses["rangedHit"] = 5;
                    statBonuses["er"] = 5;
                    break;
                case var exp when (baseStat["dex"] >= 18):
                    statBonuses["rangedDamage"] = 4;
                    statBonuses["rangedHit"] = 4;
                    statBonuses["er"] = 5;
                    break;
                case var exp when (baseStat["dex"] >= 17):
                    statBonuses["rangedDamage"] = 3;
                    statBonuses["rangedHit"] = 4;
                    statBonuses["er"] = 4;
                    break;
                case var exp when (baseStat["dex"] >= 16):
                    statBonuses["rangedDamage"] = 2;
                    statBonuses["rangedHit"] = 3;
                    statBonuses["er"] = 4;
                    break;
                case var exp when (baseStat["dex"] >= 15):
                    statBonuses["rangedDamage"] = 1;
                    statBonuses["rangedHit"] = 3;
                    statBonuses["er"] = 3;
                    break;
                case var exp when (baseStat["dex"] >= 14):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = 2;
                    statBonuses["er"] = 3;
                    break;
                case var exp when (baseStat["dex"] >= 13):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = 2;
                    statBonuses["er"] = 2;
                    break;
                case var exp when (baseStat["dex"] >= 12):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = 1;
                    statBonuses["er"] = 2;
                    break;
                case var exp when (baseStat["dex"] >= 11):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = 1;
                    statBonuses["er"] = 1;
                    break;
                case var exp when (baseStat["dex"] >= 10):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = 0;
                    statBonuses["er"] = 1;
                    break;
                case var exp when (baseStat["dex"] >= 9):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = 0;
                    statBonuses["er"] = 0;
                    break;
                case var exp when (baseStat["dex"] >= 8):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = -1;
                    statBonuses["er"] = 0;
                    break;
                case var exp when (baseStat["dex"] >= 7):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = -1;
                    statBonuses["er"] = -1;
                    break;
                case var exp when (baseStat["dex"] >= 6):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = -2;
                    statBonuses["er"] = -1;
                    break;
                case var exp when (baseStat["dex"] >= 5):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = -2;
                    statBonuses["er"] = -2;
                    break;
                case var exp when (baseStat["dex"] >= 4):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = -3;
                    statBonuses["er"] = -2;
                    break;
                case var exp when (baseStat["dex"] >= 3):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = -3;
                    statBonuses["er"] = -3;
                    break;
                case var exp when (baseStat["dex"] >= 2):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = -4;
                    statBonuses["er"] = -3;
                    break;
                case var exp when (baseStat["dex"] >= 0):
                    statBonuses["rangedDamage"] = 0;
                    statBonuses["rangedHit"] = -4;
                    statBonuses["er"] = -4;
                    break;
            }
            switch (baseStat["con"]) // CALCULATE hpPerLevel
            {
                case var exp when (baseStat["con"] >= 40):
                    statBonuses["hpPerLevel"] = 25;
                    break;
                case var exp when (baseStat["con"] >= 16):
                    statBonuses["hpPerLevel"] = (baseStat["con"] - 16) + 1;
                    break;
                default:
                    statBonuses["hpPerLevel"] = 0;
                    break;
            }
            switch (baseStat["con"]) // CALCULATE hpr
            {
                case var exp when (baseStat["con"] >= 37):
                    statBonuses["hpRegen"] = 25;
                    break;
                case var exp when (baseStat["con"] >= 13):
                    statBonuses["hpRegen"] = (baseStat["con"] - 13) + 1;
                    break;
                case var exp when (baseStat["con"] >= 8):
                    statBonuses["hpRegen"] = 1;
                    break;
                default:
                    statBonuses["hpRegen"] = 0;
                    break;
            }
            switch (baseStat["wis"]) // CALCULATE mr
            {
                case var exp when (baseStat["wis"] >= 50):
                    statBonuses["mr"] = 65;
                    break;
                case var exp when (baseStat["wis"] >= 47):
                    statBonuses["mr"] = 64;
                    break;
                case var exp when (baseStat["wis"] >= 44):
                    statBonuses["mr"] = 62;
                    break;
                case var exp when (baseStat["wis"] >= 40):
                    statBonuses["mr"] = 59;
                    break;
                case var exp when (baseStat["wis"] >= 35):
                    statBonuses["mr"] = 55;
                    break;
                case var exp when (baseStat["wis"] >= 30):
                    statBonuses["mr"] = 52;
                    break;
                case var exp when (baseStat["wis"] >= 24):
                    statBonuses["mr"] = 50;
                    break;
                case var exp when (baseStat["wis"] >= 23):
                    statBonuses["mr"] = 47;
                    break;
                case var exp when (baseStat["wis"] >= 22):
                    statBonuses["mr"] = 37;
                    break;
                case var exp when (baseStat["wis"] >= 21):
                    statBonuses["mr"] = 28;
                    break;
                case var exp when (baseStat["wis"] >= 20):
                    statBonuses["mr"] = 21;
                    break;
                case var exp when (baseStat["wis"] >= 19):
                    statBonuses["mr"] = 15;
                    break;
                case var exp when (baseStat["wis"] >= 18):
                    statBonuses["mr"] = 10;
                    break;
                case var exp when (baseStat["wis"] >= 17):
                    statBonuses["mr"] = 6;
                    break;
                case var exp when (baseStat["wis"] >= 15):
                    statBonuses["mr"] = 3;
                    break;
                default:
                    statBonuses["mr"] = 0;
                    break;
            }
            switch (baseStat["wis"]) // CALCULATE mpr
            {
                case var exp when (baseStat["wis"] >= 38):
                    statBonuses["mpPerLevel"] = 25;
                    break;
                case var exp when (baseStat["wis"] >= 26):
                    statBonuses["mpPerLevel"] = baseStat["wis"] - 13;
                    break;
                case var exp when (baseStat["wis"] >= 16):
                    statBonuses["mpPerLevel"] = baseStat["wis"] - 14;
                    break;
                case var exp when (baseStat["wis"] >= 14):
                    statBonuses["mpPerLevel"] = 2;
                    break;
                case var exp when (baseStat["wis"] >= 12):
                    statBonuses["mpPerLevel"] = 1;
                    break;
                case var exp when (baseStat["wis"] >= 11):
                    statBonuses["mpPerLevel"] = 0;
                    break;
                case var exp when (baseStat["wis"] >= 9):
                    statBonuses["mpPerLevel"] = -1;
                    break;
                default:
                    statBonuses["mpPerLevel"] = -2;
                    break;
            }
            switch (baseStat["int"]) // CALCULATE magic bonus
            {
                case var exp when (baseStat["int"] >= 36):
                    statBonuses["magicBonus"] = 11;
                    break;
                case var exp when (baseStat["int"] >= 25):
                    statBonuses["magicBonus"] = 10;
                    break;
                case var exp when (baseStat["int"] >= 17):
                    statBonuses["magicBonus"] = baseStat["int"] - 15;
                    break;
                case var exp when (baseStat["int"] >= 15):
                    statBonuses["magicBonus"] = 2;
                    break;
                case var exp when (baseStat["int"] >= 12):
                    statBonuses["magicBonus"] = 1;
                    break;
                case var exp when (baseStat["int"] >= 9):
                    statBonuses["magicBonus"] = 0;
                    break;
                case var exp when (baseStat["int"] >= 6):
                    statBonuses["magicBonus"] = -1;
                    break;
                case var exp when (baseStat["int"] >= 3):
                    statBonuses["magicBonus"] = -2;
                    break;
                default:
                    statBonuses["magicBonus"] = -3;
                    break;
            }

        }

        public void RaiseStat(string stat, string dir)
        {
            if (initialStatsAllocated == false)
            {
                if (dir == "plus")
                {
                    if (baseStat["bon"] > 0 && baseStat[stat] < maxBase[stat])
                    {
                        baseStat["bon"]--;
                        baseStat[stat]++;
                    }
                }
                else if (dir == "minus")
                {
                    if (baseStat[stat] > minBase[stat])
                    {
                        baseStat["bon"]++;
                        baseStat[stat]--;
                    }
                }
                StatBonusCalc(stat);
            }
            else if (dir == "plus")
            {
                if (baseStat["bon"] >= 1 && baseStat[stat] < 25)
                {
                    baseStat["bon"]--;
                    baseStat[stat]++;
                }
            }
        }

        public void LevelUp()
        {
            hp += GetHpPerLevel();
            CalcMagicLevel();

            if (highestLevel == level && level >= 50)
            {
                baseStat["bon"] += 1;
            }

            level++;

            if (level > highestLevel)
            {
                highestLevel = level;
            }
        }

        public void LevelDown()
        {
            if (level - 1 >= 10)
            {
                level--;
                hp -= GetHpPerLevel();
                
                if(hp < 1)
                {
                    hp = 1;
                }
            }
        }

        public void StatBonusCalc(string stat)
        {
            if(initialStatsAllocated == false)
            {
                if (role == "Knight")
                {
                    if (stat == "str")
                    {
                        baseStatBonuses["meleeDamage"] = 0;
                        baseStatBonuses["meleeHit"] = 0;

                        if(baseStat[stat] >= 17)
                        {
                            baseStatBonuses["meleeDamage"] += 2;
                        }
                        if(baseStat[stat] >= 18)
                        {
                            baseStatBonuses["meleeHit"] += 2;
                        }
                        if(baseStat[stat] >= 19)
                        {
                            baseStatBonuses["meleeDamage"] += 2;
                        }
                        if(baseStat[stat] == 20)
                        {
                            baseStatBonuses["meleeHit"] += 2;
                        }
                    }
                    else if (stat == "dex")
                    {
                        baseStatBonuses["ac"] = 0;
                        baseStatBonuses["er"] = 0;

                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["ac"] -= 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["er"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["ac"] -= 2;
                        }
                        if (baseStat[stat] == 16)
                        {
                            baseStatBonuses["er"] += 2;
                        }
                    }
                    else if (stat == "int")
                    {
                        baseStatBonuses["mpDiscount"] = 0;
                        baseStatBonuses["magicHit"] = 0;

                        if (baseStat[stat] >= 9)
                        {
                            baseStatBonuses["mpDiscount"] -= 1;
                        }
                        if (baseStat[stat] >= 10)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                        if (baseStat[stat] >= 11)
                        {
                            baseStatBonuses["mpDiscount"] -= 1;
                        }
                        if (baseStat[stat] == 12)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                    }
                    else if (stat == "wis")
                    {
                        baseStatBonuses["mr"] =  0;
                        baseStatBonuses["mpRegen"] = 0;

                        if (baseStat[stat] >= 10)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                        if (baseStat[stat] >= 11)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                        if (baseStat[stat] == 13)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                    }
                    else if (stat == "con")
                    {
                        baseStatBonuses["weightCap"] = 0;
                        baseStatBonuses["hpPerLevel"] = 0;
                        baseStatBonuses["hpRegen"] = 0;

                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["weightCap"] += 1;
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["hpRegen"] += 2;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["hpPerLevel"] += 2;
                        }
                        if (baseStat[stat] == 18)
                        {
                            baseStatBonuses["hpRegen"] += 2;
                        }

                    }
                    else if (stat == "cha")
                    {

                    }
                }
                else if (role == "Wizard")
                {
                    if (stat == "str")
                    {
                        baseStatBonuses["weightCap"] = 0;
                        baseStatBonuses["meleeDamage"] = 0;
                        baseStatBonuses["meleeHit"] = 0;

                        if (baseStat["con"] >= 13)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat["con"] >= 15)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }

                        if (baseStat[stat] >= 9)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat[stat] >= 10)
                        {
                            baseStatBonuses["meleeDamage"] += 1;
                        }
                        if (baseStat[stat] >= 11)
                        {
                            baseStatBonuses["meleeHit"] += 1;
                        }
                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["meleeDamage"] += 1;
                        }
                        if (baseStat[stat] == 13)
                        {
                            baseStatBonuses["meleeHit"] += 1;
                        }

                    }
                    else if (stat == "dex")
                    {
                        baseStatBonuses["ac"] = 0;
                        baseStatBonuses["er"] = 0;

                        if (baseStat[stat] >= 8)
                        {
                            baseStatBonuses["ac"] -= 1;
                        }
                        if (baseStat[stat] >= 9)
                        {
                            baseStatBonuses["er"] += 1;
                        }
                        if (baseStat[stat] >= 10)
                        {
                            baseStatBonuses["ac"] -= 2;
                        }
                        if (baseStat[stat] == 11)
                        {
                            baseStatBonuses["er"] += 2;
                        }
                    }
                    else if (stat == "int")
                    {
                        baseStatBonuses["magicBonus"] = 0;
                        baseStatBonuses["magicHit"] = 0;
                        baseStatBonuses["magicCrit"] = 0;

                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["magicBonus"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["magicCrit"] += 2;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["magicCrit"] += 2;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["magicCrit"] += 2;
                        }
                        if (baseStat[stat] >= 18)
                        {
                            baseStatBonuses["magicCrit"] += 2;
                        }
                    }
                    else if (stat == "wis")
                    {
                        baseStatBonuses["mpPerLevel"] = 0;
                        baseStatBonuses["mpRegen"] = 0;
                        baseStatBonuses["mr"] = 0;

                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["mpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["mpRegen"] += 1; ;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["mpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 18)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                    }
                    else if (stat == "con")
                    {
                        baseStatBonuses["hpPerLevel"] = 0;
                        baseStatBonuses["hpRegen"] = 0;
                        baseStatBonuses["weightCap"] = 0;

                        if (baseStat["str"] >= 9)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }

                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["hpPerLevel"] += 1; ;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] == 18)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                    }
                    else if (stat == "cha")
                    {

                    }
                }
                else if (role == "Elf")
                {
                    if (stat == "str")
                    {
                        baseStatBonuses["meleeDamage"] = 0;
                        baseStatBonuses["meleeHit"] = 0;
                        baseStatBonuses["weightCap"] = 0;

                        if (baseStat["con"] >= 15)
                        {
                            baseStatBonuses["weightCap"] += 2;
                        }

                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["meleeDamage"] += 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["meleeHit"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["meleeDamage"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["meleeHit"] += 1;
                        }
                        if(baseStat[stat] == 16)
                        {
                            baseStatBonuses["weightCap"] += 2;
                        }
                    }
                    else if (stat == "dex")
                    {
                        baseStatBonuses["ac"] = 0;
                        baseStatBonuses["rangedHit"] = 0;
                        baseStatBonuses["rangedDamage"] = 0;

                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["rangedHit"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["rangedDamage"] += 2;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["ac"] -= 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["rangedHit"] += 1;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["rangedDamage"] += 1;
                        }
                        if (baseStat[stat] == 18)
                        {
                            baseStatBonuses["ac"] -= 2;
                        }
                    }
                    else if (stat == "int")
                    {
                        baseStatBonuses["magicHit"] = 0;
                        baseStatBonuses["magicCrit"] = 0;

                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["magicCrit"] += 2;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                        if (baseStat[stat] == 16)
                        {
                            baseStatBonuses["magicCrit"] += 2;
                        }
                    }
                    else if (stat == "wis")
                    {
                        baseStatBonuses["mr"] = 0;
                        baseStatBonuses["mpRegen"] = 0;
                        baseStatBonuses["mpPerLevel"] = 0;

                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["mpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["mpPerLevel"] += 1;
                        }
                        if (baseStat[stat] == 18)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                    }
                    else if (stat == "con")
                    {
                        baseStatBonuses["weightCap"] = 0;
                        baseStatBonuses["hpPerLevel"] = 0;
                        baseStatBonuses["hpRegen"] = 0;

                        if(baseStat["str"] >= 16)
                        {
                            baseStatBonuses["weightCap"] = 2;
                        }

                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["weightCap"] += 2;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] == 18)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                    }
                }
                else if (role == "Royal")
                {
                    if (stat == "str")
                    {
                        baseStatBonuses["weightCap"] = 0;
                        baseStatBonuses["meleeDamage"] = 0;
                        baseStatBonuses["meleeHit"] = 0;

                        if (baseStat["con"] >= 11)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }

                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["meleeDamage"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["meleeHit"] += 1;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat[stat] >= 18)
                        {
                            baseStatBonuses["meleeDamage"] += 1;
                        }
                        if (baseStat[stat] >= 19)
                        {
                            baseStatBonuses["meleeHit"] += 1;
                        }
                        if (baseStat[stat] == 19)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }

                    }
                    else if (stat == "dex")
                    {
                        baseStatBonuses["ac"] = 0;
                        baseStatBonuses["er"] = 0;
                        baseStatBonuses["rangedDamage"] = 0;

                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["ac"] -= 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["rangedDamage"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["er"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["ac"] -= 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["er"] += 1;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["ac"] -= 1;
                        }
                        if (baseStat[stat] == 18)
                        {
                            baseStatBonuses["er"] += 1;
                        }
                    }
                    else if (stat == "int")
                    {
                        baseStatBonuses["magicHit"] = 0;
                        baseStatBonuses["mpDiscount"] = 0;

                        if (baseStat[stat] >= 11)
                        {
                            baseStatBonuses["mpDiscount"] -= 1;
                        }
                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["mpDiscount"] -= 1;
                        }
                        if (baseStat[stat] == 14)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                    }
                    else if (stat == "wis")
                    {
                        baseStatBonuses["mr"] = 0;
                        baseStatBonuses["mpRegen"] = 0;
                        baseStatBonuses["mpPerLevel"] = 0;

                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                        if (baseStat[stat] == 16)
                        {
                            baseStatBonuses["mpPerLevel"] += 1;
                        }
                    }
                    else if (stat == "con")
                    {
                        baseStatBonuses["weightCap"] = 0;
                        baseStatBonuses["hpPerLevel"] = 0;
                        baseStatBonuses["hpRegen"] = 0;

                        if (baseStat["str"] >= 14)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat["str"] >= 17)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat["str"] == 19)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }

                        if (baseStat[stat] >= 11)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] == 18)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                    }
                }
                else if (role == "Dark Elf")
                {
                    if (stat == "str")
                    {
                        baseStatBonuses["weightCap"] = 0;
                        baseStatBonuses["meleeDamage"] = 0;
                        baseStatBonuses["meleeHit"] = 0;
                        if(stat == "str")
                        {
                            if (baseStat["con"] >= 8)
                            {
                                baseStatBonuses["weightCap"] += 1;
                            }

                            if (baseStat[stat] >= 12)
                            {
                                baseStatBonuses["weightCap"] += 2;
                            }
                            if (baseStat[stat] >= 13)
                            {
                                baseStatBonuses["meleeDamage"] += 1;
                            }
                            if (baseStat[stat] >= 14)
                            {
                                baseStatBonuses["meleeHit"] += 1;
                            }
                            if (baseStat[stat] >= 15)
                            {
                                baseStatBonuses["weightCap"] += 1;
                            }
                            if (baseStat[stat] >= 16)
                            {
                                baseStatBonuses["meleeHit"] += 1;
                            }
                            if (baseStat[stat] == 17)
                            {
                                baseStatBonuses["meleeDamage"] += 1;
                            }
                        }
                    }
                    else if (stat == "dex")
                    {
                        baseStatBonuses["er"] = 0;
                        baseStatBonuses["rangedHit"] = 0;
                        baseStatBonuses["ac"] = 0;
                        baseStatBonuses["rangedDamage"] = 0;

                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["er"] += 2;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["rangedHit"] += 1;
                            baseStatBonuses["ac"] -= 1;
                        }
                        if (baseStat[stat] == 17)
                        {
                            baseStatBonuses["rangedHit"] += 1;
                            baseStatBonuses["rangedDamage"] += 2;
                        }
                    }
                    else if (stat == "int")
                    {
                        baseStatBonuses["magicHit"] = 0;
                        baseStatBonuses["mpDiscount"] = 0;

                        if (baseStat[stat] >= 11)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["mpDiscount"] -= 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                        if (baseStat[stat] == 14)
                        {
                            baseStatBonuses["mpDiscount"] -= 1;
                        }
                    }
                    else if (stat == "wis")
                    {
                        baseStatBonuses["mr"] = 0;
                        baseStatBonuses["mpPerLevel"] = 0;
                        baseStatBonuses["mpRegen"] = 0;

                        if (baseStat[stat] >= 10)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                        if (baseStat[stat] >= 11)
                        {
                            baseStatBonuses["mpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                        if (baseStat[stat] == 15)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                    }
                    else if (stat == "con")
                    {
                        baseStatBonuses["weightCap"] = 0;
                        baseStatBonuses["hpPerLevel"] = 0;
                        baseStatBonuses["hpRegen"] = 0;

                        if (baseStat["str"] >= 12)
                        {
                            baseStatBonuses["weightCap"] += 2;
                        }
                        if (baseStat["str"] >= 15)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }

                        if (baseStat[stat] >= 8)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat[stat] >= 9)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 10)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 11)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] == 12)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                    }
                }
                else if (role == "Illusionist")
                {
                    if (stat == "str")
                    {
                        baseStatBonuses["weightCap"] = 0;
                        baseStatBonuses["meleeDamage"] = 0;
                        baseStatBonuses["meleeHit"] = 0;
                        if (stat == "str")
                        {
                            if (baseStat["con"] >= 16)
                            {
                                baseStatBonuses["weightCap"] += 1;
                            }
                            if (baseStat["con"] == 17)
                            {
                                baseStatBonuses["weightCap"] += 1;
                            }

                            if (baseStat[stat] >= 11)
                            {
                                baseStatBonuses["meleeHit"] += 1;
                            }
                            if (baseStat[stat] >= 12)
                            {
                                baseStatBonuses["meleeDamage"] += 1;
                            }
                            if (baseStat[stat] >= 13)
                            {
                                baseStatBonuses["meleeHit"] += 1;
                            }
                            if (baseStat[stat] >= 14)
                            {
                                baseStatBonuses["meleeDamage"] += 1;
                            }
                            if (baseStat[stat] >= 15)
                            {
                                baseStatBonuses["meleeHit"] += 1;
                            }
                            if (baseStat[stat] >= 16)
                            {
                                baseStatBonuses["meleeHit"] += 1;
                            }
                            if (baseStat[stat] == 17)
                            {
                                baseStatBonuses["weightCap"] += 1;
                            }
                        }
                    }
                    else if (stat == "dex")
                    {
                        baseStatBonuses["ac"] = 0;
                        baseStatBonuses["er"] = 0;

                        if (baseStat[stat] >= 10)
                        {
                            baseStatBonuses["ac"] -= 1;
                        }
                        if (baseStat[stat] >= 11)
                        {
                            baseStatBonuses["er"] += 1;
                        }
                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["ac"] -= 1;
                        }
                        if (baseStat[stat] == 13)
                        {
                            baseStatBonuses["er"] += 1;
                        }
                    }
                    else if (stat == "int")
                    {
                        baseStatBonuses["mpDiscount"] = 0;
                        baseStatBonuses["magicHit"] = 0;
                        baseStatBonuses["magicBonus"] = 0;

                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["mpDiscount"] -= 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["mpDiscount"] -= 1;
                        }
                        if (baseStat[stat] == 15)
                        {
                            baseStatBonuses["magicBonus"] += 1;
                        }
                        if (baseStat[stat] == 16)
                        {
                            baseStatBonuses["magicBonus"] += 1;
                        }
                    }
                    else if (stat == "wis")
                    {
                        baseStatBonuses["mr"] = 0;
                        baseStatBonuses["mpRegen"] = 0;
                        baseStatBonuses["mr"] = 0;

                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["mpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["mr"] += 2;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["mpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                        if (baseStat[stat] == 17)
                        {
                            baseStatBonuses["mr"] += 1;
                        }
                    }
                    else if (stat == "con")
                    {
                        baseStatBonuses["weightCap"] = 0;
                        baseStatBonuses["hpPerLevel"] = 0;
                        baseStatBonuses["hpRegen"] = 0;

                        if (baseStat["str"] == 17)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }

                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                        if (baseStat[stat] == 17)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }
                    }
                }
                else if (role == "Dragon Knight")
                {
                    if (stat == "str")
                    {
                        baseStatBonuses["meleeHit"] = 0;
                        baseStatBonuses["meleeDmg"] = 0;
                        baseStatBonuses["weightCap"] = 0;

                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["meleeHit"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["meleeDmg"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["meleeDmg"] += 1;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["meleeHit"] += 2;
                        }
                        if (baseStat[stat] >= 18)
                        {
                            baseStatBonuses["meleeDmg"] += 2;
                            baseStatBonuses["weightCap"] += 1;
                        }              
                    }
                    else if (stat == "dex")
                    {
                        baseStatBonuses["er"] = 0;
                        baseStatBonuses["ac"] = 0;

                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["ac"] += 1;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["er"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["ac"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["er"] += 1;
                        }
                    }
                    else if (stat == "int")
                    {
                        baseStatBonuses["sp"] = 0;
                        baseStatBonuses["magicHit"] = 0;

                        if (baseStat[stat] >= 12)
                        {
                            baseStatBonuses["magicHit"] += 2;
                        }
                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["sp"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["sp"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["magicHit"] += 1;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["sp"] += 1;
                        }
                    }
                    else if (stat == "wis")
                    {
                        baseStatBonuses["mpPerLevel"] = 0;
                        baseStatBonuses["mr"] = 0;
                        baseStatBonuses["mpRegen"] = 0;

                        if (baseStat[stat] >= 13)
                        {
                            baseStatBonuses["mpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 14)
                        {
                            baseStatBonuses["mr"] += 2;
                        }
                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["mpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["mpRegen"] += 1;
                        }
                    }
                    else if (stat == "con")
                    {
                        baseStatBonuses["hpPerLevel"] = 0;
                        baseStatBonuses["hpr"] = 0;

                        if (baseStat["str"] >= 18)
                        {
                            baseStatBonuses["weightCap"] += 1;
                        }

                        if (baseStat[stat] >= 15)
                        {
                            baseStatBonuses["hpPerLevel"] += 1;
                        }
                        if (baseStat[stat] >= 16)
                        {
                            baseStatBonuses["hpRegen"] += 1;
                        }
                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["hpPerLevel"] += 2;
                        }
                        if (baseStat[stat] >= 18)
                        {
                            baseStatBonuses["hpRegen"] += 2;
                        }
                    }
                }
            }
            

            return;
        }

        public string OutputStats() // This function puts together and returns the string that is displayed on the right panel
        {
            string outputText = "Name: " + name +
                                "\nClass: " + role +
                                "\nLevel: " + level +
                                "\n---STATS---" +
                                "\nHP: " + hp +
                                "\nMP: " + mp +
                                "\n" +
                                "\nAC: " + GetAc() +
                                "\nMR: " + GetMr() +
                                "\nER: " + GetEr() +
                                "\nSTR: " + baseStat["str"] +
                                "\nCON: " + baseStat["con"] +
                                "\nINT: " + baseStat["int"] +
                                "\nDEX: " + baseStat["dex"] +
                                "\nWIS: " + baseStat["wis"] +
                                "\nCHA: " + baseStat["cha"] +
                                "\n" +
                                "\n---STAT BONUSES---" +
                                "\nMelee Damage: " + GetMeleeDamage() + " (" + baseStatBonuses["meleeDamage"] + ")" +
                                "\nMelee Hit: " + GetMeleeHit() + " (" + baseStatBonuses["meleeHit"] + ")" +
                                "\n\nRanged Damage: " + GetRangedDamage() + " (" + baseStatBonuses["rangedDamage"] + ")" +
                                "\nRanged Hit: " + GetRangedHit() + " (" + baseStatBonuses["rangedHit"] + ")" +
                                "\n\nMagic Crit Chance: " + GetMagicCrit() + " (" + baseStatBonuses["magicCrit"] + ")" +
                                "\nMP Discount: " + GetMpDiscount() + " (" + baseStatBonuses["mpDiscount"] + ")" +
                                "\nMagic Hit: " + GetMagicHit() + " (" + baseStatBonuses["magicHit"] + ")" +
                                "\nMP Regen: " + GetMpRegen() + " (" + baseStatBonuses["mpRegen"] + ")" +
                                "\nMagic Bonus: " + GetMagicBonus() + " (" + baseStatBonuses["magicBonus"] + ")" +
                                "\nSpell Power: " + GetSp() + " (" + baseStatBonuses["sp"] + ")" +
                                "\nMagic Level: " + GetMagicLevel() + 
                                "\n\nWeight Cap: " + GetWeightCap() + " (" + baseStatBonuses["weightCap"] + ")" +
                                "\nHP Per Level: " + GetHpPerLevel() + " (" + baseStatBonuses["hpPerLevel"] + ")" +
                                "\nHP Regen: " + GetHpRegen() + " (" + baseStatBonuses["hpRegen"] + ")";


            return outputText;
        }

        public class Knight : Character // constructor for Knight class
        {            
            public Knight(String inputName)
            {
                name = inputName;
                role = "Knight";

                baseStat["str"] = 16;
                baseStat["int"] = 8;
                baseStat["dex"] = 12;
                baseStat["con"] = 14;
                baseStat["wis"] = 9;
                baseStat["cha"] = 12;
                baseStat["bon"] = 4;

                minBase["str"] = 16;
                minBase["int"] = 8;
                minBase["dex"] = 12;
                minBase["con"] = 14;
                minBase["wis"] = 9;
                minBase["cha"] = 12;

                maxBase["str"] = 20;
                maxBase["int"] = 12;
                maxBase["dex"] = 16;
                maxBase["con"] = 18;
                maxBase["wis"] = 13;
                maxBase["cha"] = 16;

                baseHpPerLevel = 16;
            }
        }

        public class Wizard : Character //Constructor for Wizard class
        {
            
            public Wizard(String inputName)
            {
                name = inputName;
                role = "Wizard";

                baseStat["str"] = 8;
                baseStat["int"] = 12;
                baseStat["dex"] = 7;
                baseStat["con"] = 12;
                baseStat["wis"] = 12;
                baseStat["cha"] = 8;
                baseStat["bon"] = 16;

                minBase["str"] = 8;
                minBase["int"] = 12;
                minBase["dex"] = 7;
                minBase["con"] = 12;
                minBase["wis"] = 12;
                minBase["cha"] = 8;

                maxBase["str"] = 20;
                maxBase["int"] = 18;
                maxBase["dex"] = 18;
                maxBase["con"] = 18;
                maxBase["wis"] = 18;
                maxBase["cha"] = 18;

                baseHpPerLevel = 6;
            }
        }

        public class Elf : Character //Constructor for Elf class
        {
            
            public Elf(String inputName)
            {
                name = inputName;
                role = "Elf";

                baseStat["str"] = 11;
                baseStat["int"] = 12;
                baseStat["dex"] = 12;
                baseStat["con"] = 12;
                baseStat["wis"] = 12;
                baseStat["cha"] = 9;
                baseStat["bon"] = 7;

                minBase["str"] = 11;
                minBase["int"] = 12;
                minBase["dex"] = 12;
                minBase["con"] = 12;
                minBase["wis"] = 12;
                minBase["cha"] = 9;

                maxBase["str"] = 18;
                maxBase["int"] = 18;
                maxBase["dex"] = 18;
                maxBase["con"] = 18;
                maxBase["wis"] = 18;
                maxBase["cha"] = 16;

                baseHpPerLevel = 9;
            }
        }

        public class Royal : Character //Constructor for Royal class
        {
            public Royal(String inputName)
            {
                name = inputName;
                role = "Royal";

                baseStat["str"] = 13;
                baseStat["int"] = 10;
                baseStat["dex"] = 10;
                baseStat["con"] = 10;
                baseStat["wis"] = 11;
                baseStat["cha"] = 13;
                baseStat["bon"] = 8;

                minBase["str"] = 13;
                minBase["int"] = 10;
                minBase["dex"] = 10;
                minBase["con"] = 10;
                minBase["wis"] = 11;
                minBase["cha"] = 13;

                maxBase["str"] = 20;
                maxBase["int"] = 18;
                maxBase["dex"] = 18;
                maxBase["con"] = 18;
                maxBase["wis"] = 18;
                maxBase["cha"] = 18;

                baseHpPerLevel = 10;
            }
        }

        public class DarkElf : Character //Constructor for DarkElf class
        {
            public DarkElf(String inputName)
            {
                name = inputName;
                role = "Dark Elf";


                baseStat["str"] = 12;
                baseStat["int"] = 11;
                baseStat["dex"] = 15;
                baseStat["con"] = 8;
                baseStat["wis"] = 10;
                baseStat["cha"] = 10;
                baseStat["bon"] = 10;

                minBase["str"] = 12;
                minBase["int"] = 11;
                minBase["dex"] = 15;
                minBase["con"] = 8;
                minBase["wis"] = 10;
                minBase["cha"] = 10;

                maxBase["str"] = 18;
                maxBase["int"] = 18;
                maxBase["dex"] = 18;
                maxBase["con"] = 18;
                maxBase["wis"] = 18;
                maxBase["cha"] = 18;

                baseHpPerLevel = 9;
            }
        }

        public class Illusionist : Character //Constructor for Illusionist class
        {
            public Illusionist(String inputName)
            {
                name = inputName;
                role = "Illusionist";

                baseStat["str"] = 11;
                baseStat["int"] = 12;
                baseStat["dex"] = 10;
                baseStat["con"] = 12;
                baseStat["wis"] = 12;
                baseStat["cha"] = 8;
                baseStat["bon"] = 10;

                minBase["str"] = 11;
                minBase["int"] = 12;
                minBase["dex"] = 10;
                minBase["con"] = 12;
                minBase["wis"] = 12;
                minBase["cha"] = 8;

                maxBase["str"] = 18;
                maxBase["int"] = 18;
                maxBase["dex"] = 18;
                maxBase["con"] = 18;
                maxBase["wis"] = 18;
                maxBase["cha"] = 18;

                baseHpPerLevel = 5;
            }
        }

        public class DragonKnight : Character //Constructor for DragonKnight class
        {
            public DragonKnight(String inputName)
            {
                name = inputName;
                role = "Dragon Knight";

                baseStat["str"] = 13;
                baseStat["int"] = 11;
                baseStat["dex"] = 11;
                baseStat["con"] = 14;
                baseStat["wis"] = 12;
                baseStat["cha"] = 8;
                baseStat["bon"] = 6;

                minBase["str"] = 13;
                minBase["int"] = 11;
                minBase["dex"] = 11;
                minBase["con"] = 14;
                minBase["wis"] = 12;
                minBase["cha"] = 8;

                maxBase["str"] = 18;
                maxBase["int"] = 17;
                maxBase["dex"] = 17;
                maxBase["con"] = 18;
                maxBase["wis"] = 18;
                maxBase["cha"] = 14;

                baseHpPerLevel = 12;
            }
        }
    }

}

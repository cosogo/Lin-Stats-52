using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

/**
* role variable: 
* 1 = royal, 
* 2 = elf, 
* 3 = wizard, 
* 4 = dark elf, 
* 5 = dragon knight, 
* 6 = illu, 
* 7 = knight
*/

namespace LinStats
{
    public class BonusChart
    {
        private const int MAXSTAT = 61;

        private readonly int[] erChart = new int[MAXSTAT];
        private readonly int[,] strDexChart = new int[MAXSTAT, 5];
        private readonly int[,] mpDiscountChart = new int[16, 10];
        private readonly int[] mrChart = new int[MAXSTAT];
        private readonly int[,] mpChart = new int[36, 2];

        public int GetEr(int dex, int role, int level)
        {
            int erPerLevel;

            if (dex < 0)
            {
                dex = 0;
            }
            else if (dex > 60)
            {
                dex = 60;
            }

            switch (role)
            {
                case 1:
                    erPerLevel = 8;
                    break;
                case 2:
                    erPerLevel = 8;
                    break;
                case 3:
                    erPerLevel = 10;
                    break;
                case 4:
                    erPerLevel = 6;
                    break;
                case 5:
                    erPerLevel = 7;
                    break;
                case 6:
                    erPerLevel = 9;
                    break;
                case 7:
                    erPerLevel = 4;
                    break;
                default:
                    erPerLevel = 0;
                    break;
            }

            return erChart[dex] + erPerLevel;
        }

        public int GetMpDiscount(int magicLevel, int intelligence)
        {
            int effectiveInt = intelligence - 12;
            int effectiveMagicLevel = magicLevel - 1;

            if (effectiveInt >= 15)
            {
                effectiveInt = 15;
            }
            if (effectiveInt <= 0)
            {
                effectiveInt = 0;
            }

            if (effectiveMagicLevel < 0)
            {
                effectiveMagicLevel = 0;
            }

            return mpDiscountChart[effectiveInt, effectiveMagicLevel];
        }

        public int GetDmgFromStr(int str)
        {
            if (str < 0)
            {
                str = 0;
            }
            else if (str > 60)
            {
                str = 60;
            }

            return strDexChart[str, 3];
        }

        public int GetDmgFromDex(int dex)
        {
            if (dex < 0)
            {
                dex = 0;
            }
            else if (dex > 60)
            {
                dex = 60;
            }

            return strDexChart[dex, 4];
        }

        public int GetHitFromStr(int str)
        {
            if (str < 0)
            {
                str = 0;
            }
            else if (str > 60)
            {
                str = 60;
            }

            return strDexChart[str, 1];
        }

        public int GetHitFromDex(int dex)
        {
            if (dex < 0)
            {
                dex = 0;
            }
            else if (dex > 60)
            {
                dex = 60;
            }

            return strDexChart[dex, 2];
        }

        public int GetMr(int wis, int role, int level)
        {
            int mrFromRole = 0;
            int mrFromLevel = 0;

            if (wis < 0)
            {
                wis = 0;
            }
            else if (wis > 60)
            {
                wis = 60;
            }

            if (role == 1)
            {
                mrFromRole = 10;
            }
            else if (role == 2)
            {
                mrFromRole = 25;
            }
            else if (role == 3)
            {
                mrFromRole = 15;
            }
            else if (role == 4)
            {
                mrFromRole = 10;
            }
            else if (role == 5)
            {
                mrFromRole = 18;
            }
            else if (role == 6)
            {
                mrFromRole = 20;
            }
            else if (role == 7)
            {
                mrFromRole = 0;
            }

            mrFromLevel = level / 2;

            return mrChart[wis] + mrFromRole + mrFromLevel;
        }

        public int GetMpPerLevel(int wis, int role)
        {
            float mpGain;
            int finalMpGain = 0;

            if (wis > 35)
            {
                wis = 35;
            }
            else if (wis < 0)
            {
                wis = 0;
            }

            Random rnd = new Random();

            mpGain = rnd.Next(0, mpChart[wis, 0]) + 1 + mpChart[wis, 1];

            if (role == 1)
            {
                mpGain += wis >= 16 ? 1 : 0;
                finalMpGain = (int)(mpGain);
            }
            else if (role == 2)
            {
                mpGain += wis >= 17 ? 2 : wis >= 14 ? 1 : 0;
                finalMpGain = (int)(mpGain * 1.5);
            }
            else if (role == 3)
            {
                mpGain += wis >= 17 ? 2 : wis >= 13 ? 1 : 0;
                finalMpGain = (int)(mpGain * 2);
            }
            else if (role == 4)
            {
                mpGain += wis >= 12 ? 1 : 0;
                finalMpGain = (int)(mpGain * 1.5);
            }
            else if (role == 5)
            {
                mpGain += wis >= 16 ? 2 : wis >= 13 ? 1 : 0;
                finalMpGain = (int)(mpGain * 2 / 3);
            }
            else if (role == 6)
            {
                mpGain += wis >= 16 ? 2 : wis >= 13 ? 1 : 0;
                finalMpGain = (int)(mpGain * 5 / 3);
            }
            else if (role == 7)
            {
                finalMpGain = (int)(mpGain * 2 / 3);
            }

            return finalMpGain;
        }

        public int getMpRegen(int wis)
        {
            if (wis < 14)
            {
                return 0;
            }
            else if (wis == 14)
            {
                return 1;
            }
            else if (wis == 15 || wis == 16)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        public int GetHitFromDexStr(int dex, int str)
        {
            return GetHitFromDex(dex) + GetHitFromStr(str);
        }

        public int GetAcFromDex(int dex, int level)
        {
            /*
             * AC from dex is determined by dex
             * min bonus is <=9, max bonus is >=18
             */
            int ac = 10;

            if (dex <= 9)
            {
                ac -= level / 8;
            }
            else if (dex <= 12)
            {
                ac -= level / 7;
            }
            else if (dex <= 15)
            {
                ac -= level / 6;
            }
            else if (dex <= 17)
            {
                ac -= level / 5;
            }
            else
            {
                ac -= level / 4;
            }

            return ac;
        }

        public int GetHitPerLevel(int level, int role)
        {
            /*
             * hit per level is decided by the class
             * wizard does not get hit per level
             */

            int hpl = 0;

            switch (role)
            {
                case 1:
                    return level / 5;
                case 2:
                    return level / 5;
                case 3:
                    return 0;
                case 4:
                    return level / 3;
                case 5:
                    return level / 3;
                case 6:
                    return level / 5;
                case 7:
                    return level / 3;
                default:
                    break;
            }

            if (role == 3)
            {
                return 0;
            }

            if (role == 7)
            {
                hpl = level / 3;
            }

            return hpl;


        }

        public int GetMeleeDmgPerLevel(int str, int level, int role)
        {
            /*
             * some classes do not get a melee dmg per level bonus
             */

            if (role == 0 || role == 1 || role == 2 || role == 3 || role == 6)
            {
                return 0;
            }
            else
            {
                return level / 10;
            }
        }

        public int GetRangedDmgPerLevel(int level, int role)
        {
            /*
             * only elf gets ranged dmg per level bonuses
             */

            if (role == 2)
            {
                return level / 10;
            }
            else
            {
                return 0;
            }
        }

        public int GetMagicLevel(int role, int level)
        {
            /*
             * magicLevel max and rate are decided by class
             */

            int magicLevel = 0;

            if (role == 7)
            {
                magicLevel = level / 50;
                if (magicLevel > 1)
                {
                    magicLevel = 1;
                }
            }
            else if (role == 3)
            {
                magicLevel = level / 4;
                if (magicLevel > 10)
                {
                    magicLevel = 10;
                }
            }
            else if (role == 2)
            {
                magicLevel = level / 8;
                if (magicLevel > 6)
                {
                    magicLevel = 6;
                }

            }
            else if (role == 1)
            {
                magicLevel = level / 10;
                if (magicLevel > 2)
                {
                    magicLevel = 2;
                }
            }
            else if (role == 4)
            {
                magicLevel = level / 12;
                if (magicLevel > 2)
                {
                    magicLevel = 2;
                }
            }
            else if (role == 5)
            {
                magicLevel = level / 15;
                if (magicLevel > 3)
                {
                    magicLevel = 3;
                }
            }
            else if (role == 6)
            {
                magicLevel = level / 10;
                if (magicLevel > 4)
                {
                    magicLevel = 4;
                }
            }

            return magicLevel;
        }

        public int GetMagicBonus(int intelligence)
        {
            // copied this if switch from l1j src code because I am lazy
            int i = intelligence;

            if (i <= 5)
            {
                return -2;
            }
            else if (i <= 8)
            {
                return -1;
            }
            else if (i <= 11)
            {
                return 0;
            }
            else if (i <= 14)
            {
                return 1;
            }
            else if (i <= 17)
            {
                return 2;
            }
            else if (i <= 24)
            {
                return i - 15;
            }
            else if (i <= 35)
            {
                return 10;
            }
            else if (i <= 42)
            {
                return 11;
            }
            else if (i <= 49)
            {
                return 12;
            }
            else if (i <= 50)
            {
                return 13;
            }
            else
            {
                return i - 25;
            }
        }

        public int GetDr(int ac, int role, bool softAcOn)
        {
            /*
             * If soft ac is on, then all classes get dr = ac / 2
             * If it is not, each class gets a specified DR divisor
             */

            if (softAcOn == false)
            {
                return ac / 2;
            }

            if (role == 7)
            {
                return ac / 2;
            }
            else if (role == 4)
            {
                return ac / 4;
            }
            else if (role == 1 || role == 2 || role == 5)
            {
                return ac / 3;
            }
            else
            {
                return ac / 5;
            }
        }

        public int GetWeightCap(int str, int con)
        {
            int weightCap;

            weightCap = ((str + con + 1) / 2) * 150;

            if (weightCap > 3600)
            {
                weightCap = 3600;
            }

            return weightCap;
        }

        public int GetHpPerLevel(int con, int role)
        {
            int hpUp = 0;

            if (con >= 40) // hp bonus from con
            {
                hpUp += 25;
            }
            else if (con > 15)
            {
                hpUp += (con - 15);
            }

            Random rand = new Random();

            if (role == 1) // hp bonus from class
            {
                hpUp += rand.Next(0, 3) + 11;
            }
            else if (role == 2)
            {
                hpUp += rand.Next(0, 3) + 10;
            }
            else if (role == 3)
            {
                hpUp += rand.Next(0, 3) + 7;
            }
            else if (role == 4)
            {
                hpUp += rand.Next(0, 3) + 10;
            }
            else if (role == 5)
            {
                hpUp += rand.Next(0, 3) + 12;
            }
            else if (role == 6)
            {
                hpUp += rand.Next(0, 3) + 9;
            }
            else if (role == 7)
            {
                hpUp += rand.Next(0, 3) + 17;
            }

            return hpUp;
        }

        public BonusChart()
        {
            // ER FROM DEX
            erChart[0] = -1;
            erChart[1] = -1;
            erChart[2] = -1;
            erChart[3] = -1;
            erChart[4] = -1;
            erChart[5] = -1;
            erChart[6] = -1;
            erChart[7] = 0;
            erChart[8] = 0;
            erChart[9] = 1;
            erChart[10] = 1;
            erChart[11] = 2;
            erChart[12] = 2;
            erChart[13] = 3;
            erChart[14] = 3;
            erChart[15] = 4;
            erChart[16] = 4;
            erChart[17] = 5;
            erChart[18] = 5;
            erChart[19] = 6;
            erChart[20] = 6;
            erChart[21] = 7;
            erChart[22] = 7;
            erChart[23] = 8;
            erChart[24] = 8;
            erChart[25] = 9;
            erChart[26] = 9;
            erChart[27] = 10;
            erChart[28] = 10;
            erChart[29] = 11;
            erChart[30] = 11;
            erChart[31] = 12;
            erChart[32] = 12;
            erChart[33] = 13;
            erChart[34] = 13;
            erChart[35] = 14;
            erChart[36] = 14;
            erChart[37] = 15;
            erChart[38] = 15;
            erChart[39] = 16;
            erChart[40] = 16;
            erChart[41] = 17;
            erChart[42] = 17;
            erChart[43] = 18;
            erChart[44] = 18;
            erChart[45] = 19;
            erChart[46] = 19;
            erChart[47] = 20;
            erChart[48] = 20;
            erChart[49] = 21;
            erChart[50] = 21;
            erChart[51] = 22;
            erChart[52] = 22;
            erChart[53] = 23;
            erChart[54] = 23;
            erChart[55] = 24;
            erChart[56] = 24;
            erChart[57] = 25;
            erChart[58] = 25;
            erChart[59] = 26;
            erChart[60] = 25;

            //MP DISCOUNT (row = int, col = magic level
            mpDiscountChart = new int[16, 10] {
                    { 0, 0, 0 ,0 ,0 ,0 ,0, 0, 0, 0 },
                    { 0, -1, -1, -1 ,-1 , -1, -1, -1, -1, -1},
                    { 0, -1, -2, -2 ,-2 , -2, -2, -2, -2, -2},
                    { 0, -1, -2, -3 ,-3 , -3, -3, -3, -3, -3},
                    { 0, -1, -2, -3 ,-4 , -4, -4, -4, -4, -4},
                    { 0, -1, -2, -3 ,-4 , -5, -5, -5, -5, -5},
                    { 0, -1, -2, -3 ,-4 , -5, -6, -6, -6, -6},
                    { 0, -1, -2, -3 ,-4 , -5, -6, -7, -7, -7},
                    { 0, -1, -2, -3 ,-4 , -5, -6, -7, -7, -7},
                    { 0, -1, -2, -3 ,-4 , -5, -6, -7, -7, -7},
                    { 0, -1, -2, -3 ,-4 , -5, -6, -7, -7, -7},
                    { 0, -1, -2, -3 ,-4 , -5, -6, -7, -8, -8},
                    { 0, -1, -2, -3 ,-4 , -5, -6, -7, -8, -8},
                    { 0, -1, -2, -3 ,-4 , -5, -6, -7, -8, -8},
                    { 0, -1, -2, -3 ,-4 , -5, -6, -7, -8, -8},
                    { 0, -1, -2, -3 ,-4 , -5, -6, -7, -8, -9},
                };

            //HIT &DMG from STR&DEX (0 = statValue, 1=strHit, 2=dexHit, 3=strDmg, 4=dexDmg
            strDexChart = new int[MAXSTAT, 5]
            {
                    { 0, -2, -1, -2, 0},
                    { 1, -2, -1, -2, 0 },
                    { 2, -2, -1, -2, 0 },
                    { 3, -2, -1,-2, 0 },
                    { 4, -2, -1, -2, 0 },
                    { 5, -2, -1, -2, 0 },
                    { 6, -2, -1, -2, 0 },
                    { 7, -2, -1, -2, 0 },
                    { 8, -2, -1, -2, 0 },
                    { 9, -1, 0, -1, 0 },
                    { 10, -1, 0, -1, 0 },
                    { 11, 0, 1, 0, 0 },
                    { 12, 0, 1, 0, 0 },
                    { 13, 1, 2, 1, 0 },
                    { 14, 1, 2, 1, 0 },
                    { 15, 2, 3, 2, 1 },
                    { 16, 2, 3, 2, 2 },
                    { 17, 3, 4, 3, 3 },
                    { 18, 3, 4, 3, 4 },
                    { 19, 4, 5, 4, 4 },
                    { 20, 4, 6, 4, 4 },
                    { 21, 5, 7, 5, 5 },
                    { 22, 5, 8, 5, 5 },
                    { 23, 5, 9, 6, 5 },
                    { 24, 6, 10, 6, 6 },
                    { 25, 6, 11, 6, 6 },
                    { 26, 6, 12, 7, 6 },
                    { 27, 7, 13, 7, 7 },
                    { 28, 7, 14, 7, 7 },
                    { 29, 7, 15, 8, 7 },
                    { 30, 8, 16, 8, 8 },
                    { 31, 8, 17, 9, 8 },
                    { 32, 8, 18, 9, 8 },
                    { 33, 9, 19, 10, 9 },
                    { 34, 9, 19, 11, 9 },
                    { 35, 9, 19, 12, 9 },
                    { 36, 10, 20, 12, 10 },
                    { 37, 10, 20, 12, 10 },
                    { 38, 10, 20, 12, 10 },
                    { 39, 11, 21, 13, 10 },
                    { 40, 11, 21, 13, 11 },
                    { 41, 11, 21, 13, 11 },
                    { 42, 12, 22, 13, 11 },
                    { 43, 12, 22, 14, 11 },
                    { 44, 12, 22, 14, 12 },
                    { 45, 13, 23, 14, 12 },
                    { 46, 13, 23, 14, 12 },
                    { 47, 13, 23, 15, 12 },
                    { 48, 14, 24, 15, 13 },
                    { 49, 14, 24, 15, 13 },
                    { 50, 14, 24, 15, 13 },
                    { 51, 15, 25, 16, 13 },
                    { 52, 15, 25, 16, 14 },
                    { 53, 15, 25, 16, 14 },
                    { 54, 16, 26, 16, 14 },
                    { 55, 16, 26, 17, 14 },
                    { 56, 16, 26, 17, 15 },
                    { 57, 17, 27, 17, 15 },
                    { 58, 17, 27, 17, 15 },
                    { 59, 17, 27, 18, 15 },
                    { 60, 17, 28, 18, 16 }
            };

            //mr based off wis
            mrChart[0] = 0;
            mrChart[1] = 0;
            mrChart[2] = 0;
            mrChart[3] = 0;
            mrChart[4] = 0;
            mrChart[5] = 0;
            mrChart[6] = 0;
            mrChart[7] = 0;
            mrChart[8] = 0;
            mrChart[9] = 0;
            mrChart[10] = 0;
            mrChart[11] = 0;
            mrChart[12] = 0;
            mrChart[13] = 0;
            mrChart[14] = 0;
            mrChart[15] = 3;
            mrChart[16] = 3;
            mrChart[17] = 6;
            mrChart[18] = 10;
            mrChart[19] = 15;
            mrChart[20] = 21;
            mrChart[21] = 28;
            mrChart[22] = 37;
            mrChart[23] = 47;
            mrChart[24] = 50;
            mrChart[25] = 50;
            mrChart[26] = 50;
            mrChart[27] = 50;
            mrChart[28] = 50;
            mrChart[29] = 50;
            mrChart[30] = 50;
            mrChart[31] = 50;
            mrChart[32] = 50;
            mrChart[33] = 50;
            mrChart[34] = 50;
            mrChart[35] = 50;
            mrChart[36] = 50;
            mrChart[37] = 50;
            mrChart[38] = 50;
            mrChart[39] = 50;
            mrChart[40] = 50;
            mrChart[41] = 50;
            mrChart[42] = 50;
            mrChart[43] = 50;
            mrChart[44] = 50;
            mrChart[45] = 50;
            mrChart[46] = 50;
            mrChart[47] = 50;
            mrChart[48] = 50;
            mrChart[49] = 50;
            mrChart[50] = 50;
            mrChart[51] = 50;
            mrChart[52] = 50;
            mrChart[53] = 50;
            mrChart[54] = 50;
            mrChart[55] = 50;
            mrChart[56] = 50;
            mrChart[57] = 50;
            mrChart[58] = 50;
            mrChart[59] = 50;
            mrChart[60] = 50;

            //mp per level based on wis. mp per level = range(0, col0) + col1
            mpChart = new int[36, 2] {
                { 2, 0 }, //0
                { 2, 0 }, //1
                { 2, 0 }, //2
                { 2, 0 }, //3
                { 2, 0 }, //4
                { 2, 0 }, //5
                { 2, 0 }, //6
                { 2, 0 }, //7
                { 2, 0 }, //8
                { 3, 0 }, //9
                { 2, 1 }, //10
                { 2, 1 }, //11
                { 3, 1 }, //12
                { 3, 1 }, //13
                { 3, 1 }, //14
                { 3, 2 }, //15
                { 3, 2 }, //16
                { 3, 2 }, //17
                { 4, 2 }, //18
                { 4, 2 }, //19
                { 4, 2 }, //20
                { 4, 3 }, //21
                { 4, 3 }, //22
                { 4, 3 }, //23
                { 5, 3 }, //24
                { 4, 4 }, //25
                { 4, 4 }, //26
                { 5, 4 }, //27
                { 5, 4 }, //28
                { 4, 5 }, //29
                { 4, 5 }, //30
                { 5, 5 }, //31
                { 5, 5 }, //32
                { 4, 6 }, //33
                { 4, 6 }, //34
                { 5, 6 }, //35
            };

        }
    }
}

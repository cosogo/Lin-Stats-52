using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinStats
{
    public class BonusChart
    {
        public const int MAXSTAT = 61;

        private readonly int[] erChart = new int[MAXSTAT];
        private readonly int[,] strDexChart = new int[MAXSTAT, 5];
        private readonly int[,] mpDiscountChart = new int[16, 10];
        private readonly int[] mrChart = new int[MAXSTAT];
        private readonly int[,] mpChart = new int[36,2];

        public int GetErFromDex(int dex)
        {
            if (dex < 0){
                dex = 0;
            } else if (dex > 60){
                dex = 60;
            }

            return erChart[dex];
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

        public int GetMrFromWis(int wis)
        {
            if (wis < 0)
            {
                wis = 0;
            } else if (wis > 60)
            {
                wis = 60;
            }

            return mrChart[wis];
        }

        public int GetMpFromWis(int wis, float modifier)
        {
            float mpGain;

            if(wis > 35)
            {
                wis = 35;
            } else if (wis < 0)
            {
                wis = 0;
            }

            Random rnd = new Random();

            mpGain = rnd.Next(0, mpChart[wis, 0]) + mpChart[wis, 1];

            return (int)(mpGain * modifier);
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
                mpDiscountChart = new int[16,10] {
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
                { 2, 0 },
                { 2, 0 },
                { 2, 0 },
                { 2, 0 },
                { 2, 0 },
                { 2, 0 },
                { 2, 0 },
                { 2, 0 },
                { 2, 0 },
                { 2, 0 },
                { 1, 1 },
                { 1, 1 },
                { 2, 1 },
                { 2, 1 },
                { 2, 1 },
                { 2, 2 },
                { 2, 2 },
                { 2, 2 },
                { 3, 2 },
                { 3, 2 },
                { 3, 2 },
                { 3, 3 },
                { 3, 3 },
                { 3, 3 },
                { 4, 3 },
                { 3, 4 },
                { 3, 4 },
                { 4, 4 },
                { 4, 4 },
                { 3, 5 },
                { 3, 5 },
                { 4, 5 },
                { 4, 5 },
                { 3, 6 },
                { 3, 6 },
                { 4, 6 },
            };
            
        }
    }
}

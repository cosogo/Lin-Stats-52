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
using static LinStats.BonusChart;

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
        public int baseMr;
        public int erFromLevel;
        public int hitFromLevel;
        public int maxHp;
        public int maxMp;
        public float mpModifier;


        public bool initialStatsAllocated = false;

        public BonusChart statBonusChart = new BonusChart();

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

        public Dictionary<string, int> baseStatBonuses = new Dictionary<string, int>()
        {
            { "hpRegen", 0}, { "sp", 0}, { "magicHit", 0}, { "mpDiscount", 0}, { "mpPerLevel", 0}, {"er", 0}, {"meleeDamage", 0}, {"meleeHit", 0}, {"rangedHit", 0},
            {"rangedDamage", 0}, { "mpRegen", 0},  { "mr", 0}, { "magicCrit", 0}, { "weightCap", 0}, {"ac", 0}, {"magicLevel", 0}, {"magicBonus", 0}, {"hpPerLevel", 0}
        };

        public int GetMeleeHit()
        {
            int levelHit = 0;

            if(role != "Wizard") // mage does not get a hit bonus from level
            {
                levelHit = level / hitFromLevel;
            }

            return statBonusChart.GetHitFromDex(baseStat["dex"]) + statBonusChart.GetHitFromStr(baseStat["str"]) + baseStatBonuses["meleeHit"] + levelHit;
        }

        public int GetMeleeDamage()
        {
            if(role == "Elf") // elf only get's a damage from level to ranged damage
            {
                return statBonusChart.GetDmgFromStr(baseStat["str"]) + baseStatBonuses["meleeDamage"];
            } else
            {
                return statBonusChart.GetDmgFromStr(baseStat["str"]) + baseStatBonuses["meleeDamage"] + CalcDamageFromLevel(); ;
            }
        }

        public int GetAc()
        {
            int ac = 10;

            if (baseStat["dex"] <= 9)
            {
                ac -= level / 8;
            }
            else if (baseStat["dex"] <= 12)
            {
                ac -= level / 7;
            }
            else if (baseStat["dex"] <= 15)
            {
                ac -= level / 6;
            }
            else if (baseStat["dex"] <= 17)
            {
                ac -= level / 5;
            }
            else
            {
                ac -= level / 4;
            }

            return ac + baseStatBonuses["ac"];
        }

        public int GetDr()
        {
            if(role == "Knight")
            {
                return GetAc() / 2;
            } else if (role == "Dark Elf")
            {
                return GetAc() / 4;
            } else if (role == "Dragon Knight" || role == "Elf" || role == "Royal")
            {
                return GetAc() / 3;
            } else
            {
                return GetAc() / 5;
            }
        }

        public int GetEr()
        {
            return statBonusChart.GetErFromDex(baseStat["dex"]) + baseStatBonuses["er"] + (level/erFromLevel);
        }

        public int GetMpDiscount()
        {
            return statBonusChart.GetMpDiscount(GetMagicLevel(), baseStat["int"]) + baseStatBonuses["mpDiscount"];
        }

        public int GetMagicHit()
        {
            return baseStatBonuses["magicHit"];
        }

        public int GetMr()
        {
            return baseStatBonuses["mr"] + statBonusChart.GetMrFromWis(baseStat["wis"]) + baseMr + (level/2);
        }

        public int GetMpRegen()
        {
            return CalcMpRegen() + baseStatBonuses["mpRegen"];
        }

        public int GetWeightCap()
        {
            int weightTotal = ((baseStat["str"] + baseStat["con"] + baseStatBonuses["weightCap"] + 1) / 2) * 150;

            if(weightTotal > 3600)
            {
                weightTotal = 3600;
            }

            return weightTotal;
        }

        public int GetHpPerLevel()
        {
            return CalcHpPerLevel() + baseStatBonuses["hpPerLevel"];
        }

        public int GetMpPerLevel()
        {
            return baseStatBonuses["mpPerLevel"] + CalcMpPerLevel();
        }

        public int GetHpRegen()
        {
            return CalcHpRegen() + baseStatBonuses["hpRegen"];
        }

        public int GetMagicCrit()
        {
            return (10 + baseStatBonuses["magicCrit"]);
        }

        public int GetRangedHit()
        {
            int levelHit = 0;

            if (role != "Wizard") // mage does not get a hit bonus from level
            {
                levelHit = level / hitFromLevel;
            }

            return statBonusChart.GetHitFromDex(baseStat["dex"]) + statBonusChart.GetHitFromStr(baseStat["str"]) + baseStatBonuses["rangedHit"] + levelHit;
        }

        public int GetRangedDamage()
        {
            if (role == "Elf") // elf's damage bonus from level only applies to ranged dmg
            {
                return statBonusChart.GetDmgFromDex(baseStat["dex"]) + baseStatBonuses["rangedDamage"] + CalcDamageFromLevel();
            }
            else
            {
                return statBonusChart.GetDmgFromDex(baseStat["dex"]) + baseStatBonuses["rangedDamage"];
            }
            
        }

        public int GetMagicLevel()
        {
            int magicLevel = 0;

            if (role == "Knight")
            {
                magicLevel = level / 50;
                if (magicLevel > 1)
                {
                    magicLevel = 1;
                }
            }
            else if (role == "Wizard")
            {
                magicLevel = level / 4;
                if (magicLevel > 10)
                {
                    magicLevel = 10;
                }
            }
            else if (role == "Elf")
            {
                magicLevel = level / 8;
                if (magicLevel > 6)
                {
                    magicLevel = 6;
                }

            }
            else if (role == "Royal")
            {
                magicLevel = level / 10;
                if (magicLevel > 2)
                {
                    magicLevel = 2;
                }
            }
            else if (role == "Dark Elf")
            {
                magicLevel = level / 12;
                if (magicLevel > 2)
                {
                    magicLevel = 2;
                }
            }
            else if (role == "Dragon Knight")
            {
                magicLevel = level / 15;
                if (magicLevel > 3)
                {
                    magicLevel = 3;
                }
            }
            else if (role == "Illusionist")
            {
                magicLevel = level / 10;
                if (magicLevel > 4)
                {
                    magicLevel = 4;
                }
            }

            return magicLevel;
        }

        public int GetMagicBonus()
        {
            return baseStatBonuses["magicBonus"] + CalcMagicBonus();
        }
        public int GetSp()
        {
            return baseStatBonuses["sp"] + GetMagicBonus() + GetMagicLevel();
        }

        public void UseElixir()
        {
            if (elixirsUsed < 5)
            {
                baseStat["bon"]++;
                elixirsUsed++;
            }
        }

        public int CalcMpRegen()
        {
            if(baseStat["wis"] < 14)
            {
                return 0;
            } else if(baseStat["wis"] == 14)
            {
                return 1;
            } else if (baseStat["wis"] == 15 || baseStat["wis"] == 16)
            {
                return 2;
            } else
            {
                return 3;
            }
        }

        public int CalcDamageFromLevel()
        {
            if (role == "Wizard" || role == "Illusionist" || role == "Royal")
            {
                return 0;
            } else
            {
                return level/10;
            }
        }

        public int CalcHpPerLevel()
        {
            int hpUp = 0;
            Random rand = new Random();

            if(baseStat["con"] >= 40){
                hpUp = 25;
            } else if (baseStat["con"] > 15){
                hpUp = (baseStat["con"] - 15); 
            }

            hpUp += rand.Next(0, 3) + baseHpPerLevel;

            return hpUp;
        }

        public int CalcHpRegen() { 
            if(baseStat["con"] >= 37)
            {
                return 25;
            } else if (baseStat["con"] >= 13)
            {
                return (baseStat["con"] - 13) + 1;
            } else  if (baseStat["con"] >= 8)
            {
                return 1;
            } else
            {
                return 0;
            }
        }

        public int CalcMagicBonus()
        {

            // copied this if switch from l1j src code because I am lazy
            int i = baseStat["int"];

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

        public int CalcMpPerLevel() //calculates general stat bonuses
        {
            return statBonusChart.GetMpFromWis(baseStat["wis"], role);
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
            if(level == 1)
            {
                if(role == "Knight")
                {
                    hp = 16;

                    if (baseStat["Wis"] >= 13)
                    {
                        mp = 2;
                    }
                    else
                    {
                        mp = 1;
                    }
                } 
                else if (role == "Royal")
                {
                    hp = 14;

                    if(baseStat["wis"] >= 15)
                    {
                        mp = 3;
                    } else if (baseStat["wis"] == 18)
                    {
                        mp = 4;
                    } else
                    {
                        mp = 2;
                    }
                } 
                else if (role == "Elf")
                {
                    hp = 15;

                    if (baseStat["wis"] == 18)
                    {
                        mp = 6;
                    } else
                    {
                        mp = 4;
                    }
                } 
                else if (role == "Wizard")
                {
                    hp = 12;

                    if (baseStat["wis"] == 18)
                    {
                        mp = 8;
                    } else
                    {
                        mp = 6;
                    }
                } 
                else if (role == "Dark Elf")
                {
                    hp = 12;

                    if (baseStat["wis"] >= 15)
                    {
                        mp = 4;
                    }
                    else if (baseStat["wis"]  == 18)
                    {
                        mp = 6;
                    } else
                    {
                        mp = 3;
                    }
                } 
                else if (role == "Dragon Knight")
                {
                    hp = 15;

                    if (baseStat["wis"] == 18)
                    {
                        mp = 6;
                    } else
                    {
                        mp = 4;
                    }
                } 
                else if (role == "Illusionist")
                {
                    hp = 15;

                    if (baseStat["wis"] == 18)
                    {
                        mp = 6;
                    } else
                    {
                        mp = 4;
                    }
                }
            } 
            else
            {
                hp += GetHpPerLevel();
                mp += GetMpPerLevel();
            }

            if(hp > maxHp)
            {
                hp = maxHp;
            }

            if(mp > maxMp)
            {
                mp = maxMp;
            }

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
                mp -= GetMpPerLevel();
                
                if(hp < 1)
                {
                    hp = 1;
                }

                if(mp < 1)
                {
                    mp = 1;
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
                                "\nSTR: " + baseStat["str"] +
                                "\nCON: " + baseStat["con"] +
                                "\nINT: " + baseStat["int"] +
                                "\nDEX: " + baseStat["dex"] +
                                "\nWIS: " + baseStat["wis"] +
                                "\nCHA: " + baseStat["cha"] +
                                "\n" +
                                "\nAC: " + GetAc() + " (" + baseStatBonuses["ac"] + ")" +
                                "\nDR: " + GetDr() +
                                "\nMR: " + GetMr() +
                                "\nER: " + GetEr() +
                                "\n" +
                                "\nHP Per Level: " + GetHpPerLevel() + " (" + baseStatBonuses["hpPerLevel"] + ")" +
                                "\nMP Per Level: " + GetMpPerLevel() + " (" + baseStatBonuses["mpPerLevel"] + ")" +
                                "\nHP Regen: " + GetHpRegen() + " (" + baseStatBonuses["hpRegen"] + ")" +
                                "\nMP Regen: " + GetMpRegen() + " (" + baseStatBonuses["mpRegen"] + ")" +
                                "\n" +
                                "\n---STAT BONUSES---" +
                                "\nMelee Damage: " + GetMeleeDamage() + " (" + baseStatBonuses["meleeDamage"] + ")" +
                                "\nMelee Hit: " + GetMeleeHit() + " (" + baseStatBonuses["meleeHit"] + ")" +
                                "\n\nRanged Damage: " + GetRangedDamage() + " (" + baseStatBonuses["rangedDamage"] + ")" +
                                "\nRanged Hit: " + GetRangedHit() + " (" + baseStatBonuses["rangedHit"] + ")" +
                                "\n\nMagic Crit Chance: " + GetMagicCrit() + "% (" + baseStatBonuses["magicCrit"] + ")" +
                                "\nMP Discount: " + GetMpDiscount() + " (" + baseStatBonuses["mpDiscount"] + ")" +
                                "\nMagic Hit: " + GetMagicHit() + " (" + baseStatBonuses["magicHit"] + ")" +
                                "\nMagic Bonus: " + GetMagicBonus() + " (" + baseStatBonuses["magicBonus"] + ")" +
                                "\nSpell Power: " + GetSp() + " (" + baseStatBonuses["sp"] + ")" +
                                "\nMagic Level: " + GetMagicLevel() +
                                "\n\nWeight Cap: " + GetWeightCap() + " (" + baseStatBonuses["weightCap"] + ")";


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

                baseHpPerLevel = 17;
                baseMr = 0;
                erFromLevel = 4;
                hitFromLevel = 3;
                maxHp = 2000;
                maxMp = 600;
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

                baseHpPerLevel = 7;
                baseMr = 15;
                erFromLevel = 10;
                maxHp = 1000;
                maxMp = 1200;
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

                baseHpPerLevel = 10;
                baseMr = 25;
                erFromLevel = 8;
                hitFromLevel = 5;
                maxHp = 1400;
                maxMp = 900;
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

                baseHpPerLevel = 11;
                baseMr = 10;
                erFromLevel = 8;
                hitFromLevel = 5;
                maxHp = 1400;
                maxMp = 800;
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

                baseHpPerLevel = 10;
                baseMr = 10;
                erFromLevel = 6;
                hitFromLevel = 3;
                maxHp = 1400;
                maxMp = 900;
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

                baseHpPerLevel = 9;
                baseMr = 20;
                erFromLevel = 9;
                hitFromLevel = 5;
                maxHp = 1200;
                maxMp = 1100;
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
                baseMr = 18;
                erFromLevel = 7;
                hitFromLevel = 3;
                maxHp = 1800;
            }
        }
    }

}

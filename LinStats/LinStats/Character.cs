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

/**
 * This class defines the character for the stat calculator. This class essentially handles storing and displaying the character data.
 * Stat calculations are generally handles in a separate class (BonusChart) unless the values are dependent on something specific to the character.
 */

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
        public int maxHp;
        public int maxMp;
        public float mpModifier;


        public bool initialStatsAllocated = false;

        public BonusChart statBonusChart = new BonusChart();

        public Dictionary<string, int> baseStat = new Dictionary<string, int>() // dict to hold the base stat values
        {
            {"str", 0 }, {"dex", 0 }, {"con", 0 }, {"int", 0 }, {"wis", 0 }, {"cha", 0 }, {"bon", 0}
        };

        public Dictionary<string, int> maxBase = new Dictionary<string, int>() // these get defined on character creation. they are the max values for stats assigned at creation
        {
            {"str", 0 }, {"dex", 0 }, {"con", 0 }, {"int", 0 }, {"wis", 0 }, {"cha", 0 }
        };

        public Dictionary<string, int> minBase = new Dictionary<string, int>() // defines minimum stat values, 0 across the board
        {
            {"str", 0 }, {"dex", 0 }, {"con", 0 }, {"int", 0 }, {"wis", 0 }, {"cha", 0 }
        };

        public Dictionary<string, int> baseStatBonuses = new Dictionary<string, int>() // these are the bonuses from your base stats
        {
            { "hpRegen", 0}, { "sp", 0}, { "magicHit", 0}, { "mpDiscount", 0}, { "mpPerLevel", 0}, {"er", 0}, {"meleeDamage", 0}, {"meleeHit", 0}, {"rangedHit", 0},
            {"rangedDamage", 0}, { "mpRegen", 0},  { "mr", 0}, { "magicCrit", 0}, { "weightCap", 0}, {"ac", 0}, {"magicLevel", 0}, {"magicBonus", 0}, {"hpPerLevel", 0}
        };

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

                baseMr = 0;
                erFromLevel = 4;
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

                baseMr = 25;
                erFromLevel = 8;
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

                baseMr = 10;
                erFromLevel = 8;
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

                baseMr = 10;
                erFromLevel = 6;
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

                baseMr = 20;
                erFromLevel = 9;
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

                baseMr = 18;
                erFromLevel = 7;
                maxHp = 1800;
            }
        }

        public int GetRoleNumber() // turns role into a number value to be used with the bonusChart class
        {
            if (role == "Royal")
            {
                return 1;
            }
            if (role == "Elf")
            {
                return 2;
            }
            if (role == "Wizard")
            {
                return 3;
            }
            if (role == "Dark Elf")
            {
                return 4;
            }
            if (role == "Dragon Knight")
            {
                return 5;
            }
            if (role == "Illusionist")
            {
                return 6;
            }
            if (role == "Knight")
            {
                return 7;
            }

            else return 0;
        }

        public int GetMeleeHit()
        {
            return
                statBonusChart.GetHitFromDexStr(baseStat["dex"], baseStat["str"]) +
                baseStatBonuses["meleeHit"] +
                statBonusChart.GetHitPerLevel(level, GetRoleNumber());
        }

        public int GetMeleeDamage()
        {
            return
                statBonusChart.GetDmgFromStr(baseStat["str"]) +
                baseStatBonuses["meleeDamage"] +
                statBonusChart.GetMeleeDmgPerLevel(baseStat["str"], level, GetRoleNumber());
        }

        public int GetAc()
        {
            return
                statBonusChart.GetAcFromDex(baseStat["dex"], level) +
                baseStatBonuses["ac"];
        }

        public int GetDr()
        {
            return
                statBonusChart.GetDr(GetAc(), GetRoleNumber(), true);
        }

        public int GetEr()
        {
            return
                statBonusChart.GetEr(baseStat["dex"], GetRoleNumber(), level) +
                baseStatBonuses["er"];
        }

        public int GetMpDiscount()
        {
            return
                statBonusChart.GetMpDiscount(GetMagicLevel(), baseStat["int"]) +
                baseStatBonuses["mpDiscount"];
        }

        public int GetMagicHit()
        {
            return baseStatBonuses["magicHit"];
        }

        public int GetMr()
        {
            return
                baseStatBonuses["mr"] +
                statBonusChart.GetMr(baseStat["wis"], GetRoleNumber(), level);
        }

        public int GetMpRegen()
        {
            return
                statBonusChart.getMpRegen(baseStat["wis"]) +
                baseStatBonuses["mpRegen"];
        }

        public int GetWeightCap()
        {
            return statBonusChart.GetWeightCap(baseStat["str"], baseStat["con"], baseStatBonuses["weightCap"]);
        }

        public int GetHpPerLevel()
        {
            return
                statBonusChart.GetHpPerLevel(baseStat["con"], GetRoleNumber()) +
                baseStatBonuses["hpPerLevel"];
        }

        public int GetMpPerLevel()
        {
            return
                baseStatBonuses["mpPerLevel"] +
                statBonusChart.GetMpPerLevel(baseStat["wis"], GetRoleNumber());
        }

        public int GetHpRegen()
        {
            return
                statBonusChart.GetHpRegen(baseStat["con"]) + 
                baseStatBonuses["hpRegen"];
        }

        public int GetMagicCrit()
        {
            return
                (10 + baseStatBonuses["magicCrit"]);
        }

        public int GetRangedHit()
        {
            return
                statBonusChart.GetHitFromDex(baseStat["dex"]) +
                statBonusChart.GetHitFromStr(baseStat["str"]) +
                baseStatBonuses["rangedHit"] +
                statBonusChart.GetHitPerLevel(level, GetRoleNumber());
        }

        public int GetRangedDamage()
        {
            return
                statBonusChart.GetDmgFromDex(baseStat["dex"]) +
                baseStatBonuses["rangedDamage"] +
                statBonusChart.GetRangedDmgPerLevel(level, GetRoleNumber());
        }

        public int GetMagicLevel()
        {
            return
                statBonusChart.GetMagicLevel(GetRoleNumber(), level);
        }

        public int GetMagicBonus()
        {
            return
                baseStatBonuses["magicBonus"] +
                statBonusChart.GetMagicBonus(baseStat["int"]);
        }

        public int GetSp()
        {
            return
                baseStatBonuses["sp"] + GetMagicBonus() + GetMagicLevel();
        }

        public void UseElixir() // elixir use. only 5 elixirs are allowed
        {
            if (elixirsUsed < 5)
            {
                baseStat["bon"]++;
                elixirsUsed++;
            }
        }

        public void RaiseStat(string stat, string dir)
        {
            if (initialStatsAllocated == false) // if you are assigning base stats the max stat values are checked before assigning
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
            else if (dir == "plus") // max base stat balues are not applied. only the global max stat value is (25)
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
            if (level == 1) // initial stat generation. these values are not set based on stats but based on the class you are
            {
                if (role == "Knight")
                {
                    hp = 16;

                    if (baseStat["wis"] >= 13)
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

                    if (baseStat["wis"] >= 15)
                    {
                        mp = 3;
                    }
                    else if (baseStat["wis"] == 18)
                    {
                        mp = 4;
                    }
                    else
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
                    }
                    else
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
                    }
                    else
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
                    else if (baseStat["wis"] == 18)
                    {
                        mp = 6;
                    }
                    else
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
                    }
                    else
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
                    }
                    else
                    {
                        mp = 4;
                    }
                }
            }
            else // once the character is created, it starts calculating hp and mp based on stats
            {
                hp += GetHpPerLevel();
                mp += GetMpPerLevel();
            }

            if (hp > maxHp)
            {
                hp = maxHp;
            }

            if (mp > maxMp)
            {
                mp = maxMp;
            }

            if (highestLevel == level && level >= 50) // if you delevel and relevel, this stops you from getting bonus stats on the relevels
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

                if (hp < 1)
                {
                    hp = 1;
                }

                if (mp < 1)
                {
                    mp = 1;
                }
            }
        }

        public void StatBonusCalc(string stat) // numbers seem a bit arbitrary here. they are based on break points reaached during initial stat allocation for certain bonuses.
        {
            if (initialStatsAllocated == false)
            {
                if (role == "Knight")
                {
                    if (stat == "str")
                    {
                        baseStatBonuses["meleeDamage"] = 0;
                        baseStatBonuses["meleeHit"] = 0;

                        if (baseStat[stat] >= 17)
                        {
                            baseStatBonuses["meleeDamage"] += 2;
                        }
                        if (baseStat[stat] >= 18)
                        {
                            baseStatBonuses["meleeHit"] += 2;
                        }
                        if (baseStat[stat] >= 19)
                        {
                            baseStatBonuses["meleeDamage"] += 2;
                        }
                        if (baseStat[stat] == 20)
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
                        baseStatBonuses["mr"] = 0;
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
                        if (baseStat[stat] == 16)
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

                        if (baseStat["str"] >= 16)
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
                        if (stat == "str")
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
    }

}

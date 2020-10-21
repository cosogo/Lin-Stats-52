using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool charLocked = false;
        public Character player = null;
        public bool autoLevel = false;

        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = this;

            if(player == null)
            {
                this.DisplayField = "null";
            }

        }

        //Binds the display field so it automatically updates on changes to this.DisplayField
        public String DisplayField
        {
            get { return (string)GetValue(DisplayProperty); }
            set { SetValue(DisplayProperty, value); }
        }

        public static readonly DependencyProperty DisplayProperty = DependencyProperty.Register("DisplayField", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public void RedrawWindow()
        {
            player.CalcStatBonus();
            this.DisplayField = player.OutputStats();
        }

        private void InitialSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(CharType.Text))
            {
                MessageBox.Show("Select a class.");
            }
            else
            {
                CharType.IsEnabled = false;
                CharName.IsEnabled = false;

                if (CharType.Text == "Knight")
                {
                    player = new Knight(CharName.Text);
                }
                else if (CharType.Text == "Wizard")
                {
                    player = new Wizard(CharName.Text);
                }
                else if (CharType.Text == "Royal")
                {
                    player = new Royal(CharName.Text);
                }
                else if (CharType.Text == "Elf")
                {
                    player = new Elf(CharName.Text);
                }
                else if (CharType.Text == "Dark Elf")
                {
                    player = new DarkElf(CharName.Text);
                }
                else if (CharType.Text == "Illusionist")
                {
                    player = new Illusionist(CharName.Text);
                }
                else
                {
                    player = new DragonKnight(CharName.Text);
                }

                RedrawWindow();

                BaseStatsGrid.IsEnabled = true;
                SubmitStats.IsEnabled = true;
                BaseStrBox.Text = player.baseStat["str"].ToString();
                BaseConBox.Text = player.baseStat["con"].ToString();
                BaseIntBox.Text = player.baseStat["int"].ToString();
                BaseDexBox.Text = player.baseStat["dex"].ToString();
                BaseWisBox.Text = player.baseStat["wis"].ToString();
                BaseChaBox.Text = player.baseStat["cha"].ToString();

                BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            }

        }

        private void StrPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("str", "plus", autoLevel);
            BaseStrBox.Text = player.baseStat["str"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void StrMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("str", "minus", autoLevel);
            BaseStrBox.Text = player.baseStat["str"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void ConPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("con", "plus", autoLevel);
            BaseConBox.Text = player.baseStat["con"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void ConMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("con", "minus", autoLevel);
            BaseConBox.Text = player.baseStat["con"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void IntPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("int", "plus", autoLevel);
            BaseIntBox.Text = player.baseStat["int"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void IntMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("int", "minus", autoLevel);
            BaseIntBox.Text = player.baseStat["int"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void DexPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("dex", "plus", autoLevel);
            BaseDexBox.Text = player.baseStat["dex"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void DexMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("dex", "minus", autoLevel);
            BaseDexBox.Text = player.baseStat["dex"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void WisPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("wis", "plus", autoLevel);
            BaseWisBox.Text = player.baseStat["wis"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void WisMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("wis", "minus", autoLevel);
            BaseWisBox.Text = player.baseStat["wis"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void ChaPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("cha", "plus", autoLevel);
            BaseChaBox.Text = player.baseStat["cha"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void ChaMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("cha", "minus", autoLevel);
            BaseChaBox.Text = player.baseStat["cha"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void SubmitStats_Click(object sender, RoutedEventArgs e)
        {
            if(player.initialStatsAllocated == false)
            {
                if (player.baseStat["bon"] != 0)
                {
                    MessageBox.Show("Allocate all bonus stats");
                }
                else
                {
                    LevelUpGrid.IsEnabled = true;
                    player.initialStatsAllocated = true;

                    for (int i = 0; i < 5; i++)
                    {
                        player.LevelUp();
                    }
                }
                RedrawWindow();
            }
        }

        private void ElixirUp_Click(object sender, RoutedEventArgs e)
        {
            if(player.elixirsUsed < 5)
            {
                player.baseStat["bon"]++;
                player.elixirsUsed++;
                BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            }

            RedrawWindow();
        }

        private void LevelUpOne_Click(object sender, RoutedEventArgs e)
        {
            player.LevelUp();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void LevelUpFive_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < 5; i++)
            {
                if (player.level + 5 < player.highestLevel || player.level + 5 < 51) {
                    player.LevelUp();
                    BonusStatsBlock.Text = player.baseStat["bon"].ToString();
                    RedrawWindow();
                }
            }
        }

        private void LevelDown_Click(object sender, RoutedEventArgs e) {
            player.levelDown();
            RedrawWindow();
        }

        private void LevelOnStatUp_Checked(object sender, RoutedEventArgs e)
        {
            autoLevel = true;
        }

        private void LevelOnStatUp_Unchecked(object sender, RoutedEventArgs e)
        {
            autoLevel = false;
        }
    }

}

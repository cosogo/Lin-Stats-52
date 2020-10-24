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

        public void RedrawWindow() // redraws the right window to output player stats
        {
            if(player != null) {
                this.DisplayField = player.OutputStats();
            } else
            {
                this.DisplayField = null;
            }
        }

        private void InitialSubmit_Click(object sender, RoutedEventArgs e) // initial class and name submit
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

        // the below functions handle the +/- functionality on the stats

        private void StrPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("str", "plus");
            if (player.baseStat["bon"] == 0 && autoLevel == true)
            {
                player.LevelUp();
            }
            BaseStrBox.Text = player.baseStat["str"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void StrMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("str", "minus");
            BaseStrBox.Text = player.baseStat["str"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void ConPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("con", "plus");
            if (player.baseStat["bon"] == 0 && autoLevel == true)
            {
                player.LevelUp();
            }
            BaseConBox.Text = player.baseStat["con"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void ConMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("con", "minus");
            BaseConBox.Text = player.baseStat["con"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void IntPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("int", "plus");
            if (player.baseStat["bon"] == 0 && autoLevel == true)
            {
                player.LevelUp();
            }
            BaseIntBox.Text = player.baseStat["int"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void IntMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("int", "minus");
            BaseIntBox.Text = player.baseStat["int"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void DexPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("dex", "plus");
            if (player.baseStat["bon"] == 0 && autoLevel == true)
            {
                player.LevelUp();
            }
            BaseDexBox.Text = player.baseStat["dex"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void DexMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("dex", "minus");
            BaseDexBox.Text = player.baseStat["dex"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void WisPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("wis", "plus");
            if (player.baseStat["bon"] == 0 && autoLevel == true)
            {
                player.LevelUp();
            }
            BaseWisBox.Text = player.baseStat["wis"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void WisMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("wis", "minus");
            BaseWisBox.Text = player.baseStat["wis"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void ChaPlusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("cha", "plus");
            if (player.baseStat["bon"] == 0 && autoLevel == true)
            {
                player.LevelUp();
            }
            BaseChaBox.Text = player.baseStat["cha"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void ChaMinusBut_Click(object sender, RoutedEventArgs e)
        {
            player.RaiseStat("cha", "minus");
            BaseChaBox.Text = player.baseStat["cha"].ToString();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void SubmitStats_Click(object sender, RoutedEventArgs e) // submits the base stats
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

        private void ElixirUp_Click(object sender, RoutedEventArgs e) // Assigns an bonus stat pt. but only up to 5
        {
            player.UseElixir();

            BonusStatsBlock.Text = player.baseStat["bon"].ToString();

            RedrawWindow();
        }

        private void LevelUpOne_Click(object sender, RoutedEventArgs e) // Levels the player 1 time
        {
            player.LevelUp();
            BonusStatsBlock.Text = player.baseStat["bon"].ToString();
            RedrawWindow();
        }

        private void LevelUpFive_Click(object sender, RoutedEventArgs e) // Levels the player 5 times but will not level past the point where you need to allocate stats
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

        private void LevelDown_Click(object sender, RoutedEventArgs e) { // Handles level down functionality
            player.LevelDown();
            RedrawWindow();
        }

        private void LevelOnStatUp_Checked(object sender, RoutedEventArgs e) // This function handles the checking of auto-level
        {
            autoLevel = true;
        }

        private void LevelOnStatUp_Unchecked(object sender, RoutedEventArgs e) // This function handles the unchecking of auto-level
        {
            autoLevel = false;
        }

        private void Exit_Click(object sender, RoutedEventArgs e) // This function exits
        {
            System.Environment.Exit(1);
        }

        private void New_Click(object sender, RoutedEventArgs e) // This function will reset all of the necessary variables for a new session
        {
            charLocked = false;
            player = null;
            autoLevel = false;

            CharType.IsEnabled = true;
            CharName.IsEnabled = true;
            BaseStatsGrid.IsEnabled = false;
            SubmitStats.IsEnabled = false;

            RedrawWindow();
        }

        private void Contact_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("For questions/bug reports/requests:" +
                "\nDiscord: Depardieu#7391" +
                "\nEmail: alfredolor89@gmail.com" +
                "\nGithub: TBH");
        }
    }

}

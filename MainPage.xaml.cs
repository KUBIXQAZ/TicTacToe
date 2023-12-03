using Google.Android.Material.BottomAppBar;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Serialization;

namespace TicTacToe;

[Serializable]
public class Stats
{
    public int Wins { get; set; }
    public int Losses { get; set; }
}

public partial class MainPage : ContentPage
{
    List<Button> buttons = new List<Button>();
    List<Button> activeButtons = new List<Button>();

    public string userSymbol = "x";
    public string botSymbol = "o";

    Stats stats;

    public string xmlName = "userdata.xml";
    public static string userdataFilePath;
    public static string myAppFolder;

    public MainPage()
	{
		InitializeComponent();

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        myAppFolder = Path.Combine(appDataPath, "TicTacToe");
        userdataFilePath = Path.Combine(myAppFolder, xmlName);

        stats = new Stats();

        buttons = grid.Children.OfType<Button>().Where(button => button != NewGameB).ToList();

        LoadStats();
        NewGame();
    }

    private void LoadStats()
    {
        if (File.Exists(userdataFilePath))
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Stats));
                using (Stream stream = new FileStream(userdataFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    stats = (Stats)serializer.Deserialize(stream);
                }

                userWins.Text = $"Wins {stats.Wins}";
                botWins.Text = $"{stats.Losses} Losses";
            } catch { }
        }
    }

    private void SaveStats()
    {
        try
        {
            Directory.CreateDirectory(myAppFolder);

            XmlSerializer serializer = new XmlSerializer(typeof(Stats));
            using (Stream stream = new FileStream(userdataFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serializer.Serialize(stream, stats);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving stats: {ex.Message}");
        }
    }


    private void playerPick(object sender, EventArgs e)
    {
        Button btn = (Button)sender;

        btn.Text = userSymbol;
        btn.IsEnabled = false;
        btn.BackgroundColor = Colors.Red;
        activeButtons.Remove(btn);

        int status = UpdateState();
        if (status == 0) botPick();
        else
        {
            CheckStatus(status);
        }
    }

    private void botPick()
    {
        Random random = new Random();
        Button btn;

        //defense//
        //horizontal//
        if (btn1.Text == userSymbol && btn2.Text == userSymbol && btn3.Text == "") btn = buttons[2];
        else if (btn1.Text == userSymbol && btn2.Text == "" && btn3.Text == userSymbol) btn = buttons[1];
        else if (btn1.Text == "" && btn2.Text == userSymbol && btn3.Text == userSymbol) btn = buttons[0];

        else if (btn4.Text == userSymbol && btn5.Text == userSymbol && btn6.Text == "") btn = buttons[5];
        else if (btn4.Text == userSymbol && btn5.Text == "" && btn6.Text == userSymbol) btn = buttons[4];
        else if (btn4.Text == "" && btn5.Text == userSymbol && btn6.Text == userSymbol) btn = buttons[3];

        else if (btn7.Text == userSymbol && btn8.Text == userSymbol && btn9.Text == "") btn = buttons[8];
        else if (btn7.Text == userSymbol && btn8.Text == "" && btn9.Text == userSymbol) btn = buttons[7];
        else if (btn7.Text == "" && btn8.Text == userSymbol && btn9.Text == userSymbol) btn = buttons[6];
        //vertical//
        else if (btn1.Text == userSymbol && btn4.Text == userSymbol && btn7.Text == "") btn = buttons[6];
        else if (btn1.Text == userSymbol && btn4.Text == "" && btn7.Text == userSymbol) btn = buttons[3];
        else if (btn1.Text == "" && btn4.Text == userSymbol && btn7.Text == userSymbol) btn = buttons[0];

        else if (btn2.Text == userSymbol && btn5.Text == userSymbol && btn8.Text == "") btn = buttons[7];
        else if (btn2.Text == userSymbol && btn5.Text == "" && btn8.Text == userSymbol) btn = buttons[4];
        else if (btn2.Text == "" && btn5.Text == userSymbol && btn8.Text == userSymbol) btn = buttons[1];

        else if (btn3.Text == userSymbol && btn6.Text == userSymbol && btn9.Text == "") btn = buttons[8];
        else if (btn3.Text == userSymbol && btn6.Text == "" && btn9.Text == userSymbol) btn = buttons[5];
        else if (btn3.Text == "" && btn6.Text == userSymbol && btn9.Text == userSymbol) btn = buttons[2];
        //diagonally from left-top to right-bottom//
        else if (btn1.Text == userSymbol && btn5.Text == userSymbol && btn9.Text == "") btn = buttons[8];
        else if (btn1.Text == userSymbol && btn5.Text == "" && btn9.Text == userSymbol) btn = buttons[4];
        else if (btn1.Text == "" && btn5.Text == userSymbol && btn9.Text == userSymbol) btn = buttons[0];
        //diagonally from left-bottom to right-top//
        else if (btn7.Text == userSymbol && btn5.Text == userSymbol && btn3.Text == "") btn = buttons[2];
        else if (btn7.Text == userSymbol && btn5.Text == "" && btn3.Text == userSymbol) btn = buttons[4];
        else if (btn7.Text == "" && btn5.Text == userSymbol && btn3.Text == userSymbol) btn = buttons[6];
        //attack step 2//
        //horizontal//
        else if (btn1.Text == botSymbol && btn2.Text == "" && btn3.Text == "") btn = buttons[random.Next(1, 3)];
        else if (btn1.Text == "" && btn2.Text == botSymbol && btn3.Text == "") btn = buttons[random.Next(2) * 2];
        else if (btn1.Text == "" && btn2.Text == "" && btn3.Text == botSymbol) btn = buttons[random.Next(2)];

        else if (btn4.Text == botSymbol && btn5.Text == "" && btn6.Text == "") btn = buttons[random.Next(4, 6)];
        else if (btn4.Text == "" && btn5.Text == botSymbol && btn6.Text == "") btn = buttons[random.Next(2) * 2 + 3];
        else if (btn4.Text == "" && btn5.Text == "" && btn6.Text == botSymbol) btn = buttons[random.Next(3, 5)];

        else if (btn7.Text == botSymbol && btn8.Text == "" && btn9.Text == "") btn = buttons[random.Next(7, 9)];
        else if (btn7.Text == "" && btn8.Text == botSymbol && btn9.Text == "") btn = buttons[random.Next(2) * 2 + 6];
        else if (btn7.Text == "" && btn8.Text == "" && btn9.Text == botSymbol) btn = buttons[random.Next(6, 8)];
        //vertical//
        else if (btn1.Text == botSymbol && btn4.Text == "" && btn7.Text == "") btn = buttons[random.Next(3, 7)];
        else if (btn1.Text == "" && btn4.Text == botSymbol && btn7.Text == "") btn = buttons[random.Next(2) * 6];
        else if (btn1.Text == "" && btn4.Text == "" && btn7.Text == botSymbol) btn = buttons[random.Next(2) * 3];

        else if (btn2.Text == botSymbol && btn5.Text == "" && btn8.Text == "") btn = buttons[random.Next(4, 8)];
        else if (btn2.Text == "" && btn5.Text == botSymbol && btn8.Text == "") btn = buttons[random.Next(2) * 7 + 1];
        else if (btn2.Text == "" && btn5.Text == "" && btn8.Text == botSymbol) btn = buttons[random.Next(1, 5)];

        else if (btn3.Text == botSymbol && btn6.Text == "" && btn9.Text == "") btn = buttons[random.Next(5, 9)];
        else if (btn3.Text == "" && btn6.Text == botSymbol && btn9.Text == "") btn = buttons[random.Next(2) * 6 + 2];
        else if (btn3.Text == "" && btn6.Text == "" && btn9.Text == botSymbol) btn = buttons[random.Next(2, 6)];
        //diagonally from left-top to right-bottom//
        else if (btn1.Text == botSymbol && btn5.Text == "" && btn9.Text == "") btn = buttons[random.Next(4, 9)];
        else if (btn1.Text == "" && btn5.Text == botSymbol && btn9.Text == "") btn = buttons[random.Next(2) * 8];
        else if (btn1.Text == "" && btn5.Text == "" && btn9.Text == botSymbol) btn = buttons[random.Next(2) * 4];
        //diagonally from left-bottom to right-top//
        else if (btn7.Text == botSymbol && btn5.Text == "" && btn3.Text == "") btn = buttons[random.Next(2) * 2 + 2];
        else if (btn7.Text == "" && btn5.Text == botSymbol && btn3.Text == "") btn = buttons[random.Next(2) * 4 + 2];
        else if (btn7.Text == "" && btn5.Text == "" && btn3.Text == botSymbol) btn = buttons[random.Next(2) * 2 + 4];
        //attact step 3//
        //horizontal//
        else if (btn1.Text == "" && btn2.Text == botSymbol && btn3.Text == botSymbol) btn = buttons[0];
        else if (btn1.Text == botSymbol && btn2.Text == "" && btn3.Text == botSymbol) btn = buttons[1];
        else if (btn1.Text == botSymbol && btn2.Text == botSymbol && btn3.Text == "") btn = buttons[2];

        else if (btn4.Text == "" && btn5.Text == botSymbol && btn6.Text == botSymbol) btn = buttons[3];
        else if (btn4.Text == botSymbol && btn5.Text == "" && btn6.Text == botSymbol) btn = buttons[4];
        else if (btn4.Text == botSymbol && btn5.Text == botSymbol && btn6.Text == "") btn = buttons[5];

        else if (btn7.Text == "" && btn8.Text == botSymbol && btn9.Text == botSymbol) btn = buttons[6];
        else if (btn7.Text == botSymbol && btn8.Text == "" && btn9.Text == botSymbol) btn = buttons[7];
        else if (btn7.Text == botSymbol && btn8.Text == botSymbol && btn9.Text == "") btn = buttons[8];
        //vertical//
        else if (btn1.Text == "" && btn4.Text == botSymbol && btn7.Text == botSymbol) btn = buttons[0];
        else if (btn1.Text == botSymbol && btn4.Text == "" && btn7.Text == botSymbol) btn = buttons[3];
        else if (btn1.Text == botSymbol && btn4.Text == botSymbol && btn7.Text == "") btn = buttons[6];

        else if (btn2.Text == "" && btn5.Text == botSymbol && btn8.Text == botSymbol) btn = buttons[1];
        else if (btn2.Text == botSymbol && btn5.Text == "" && btn8.Text == botSymbol) btn = buttons[4];
        else if (btn2.Text == botSymbol && btn5.Text == botSymbol && btn8.Text == "") btn = buttons[7];

        else if (btn3.Text == "" && btn6.Text == botSymbol && btn9.Text == botSymbol) btn = buttons[2];
        else if (btn3.Text == botSymbol && btn6.Text == "" && btn9.Text == botSymbol) btn = buttons[5];
        else if (btn3.Text == botSymbol && btn6.Text == botSymbol && btn9.Text == "") btn = buttons[8];
        //diagonally from left-top to right-bottom//
        else if (btn1.Text == "" && btn5.Text == botSymbol && btn9.Text == botSymbol) btn = buttons[0];
        else if (btn1.Text == botSymbol && btn5.Text == "" && btn9.Text == botSymbol) btn = buttons[4];
        else if (btn1.Text == botSymbol && btn5.Text == botSymbol && btn9.Text == "") btn = buttons[8];
        //diagonally from left-bottom to right-top//
        else if (btn7.Text == "" && btn5.Text == botSymbol && btn3.Text == botSymbol) btn = buttons[6];
        else if (btn7.Text == botSymbol && btn5.Text == "" && btn3.Text == botSymbol) btn = buttons[4];
        else if (btn7.Text == botSymbol && btn5.Text == botSymbol && btn3.Text == "") btn = buttons[2];

        //random pick//
        else btn = activeButtons[random.Next(activeButtons.Count)];

        btn.Text = botSymbol;
        btn.IsEnabled = false;
        btn.BackgroundColor = Colors.Blue;
        activeButtons.Remove(btn);

        CheckStatus(UpdateState());
    }

    private int UpdateState()
    {
        if (btn1.Text == userSymbol && btn2.Text == userSymbol && btn3.Text == userSymbol ||
            btn4.Text == userSymbol && btn5.Text == userSymbol && btn6.Text == userSymbol ||
            btn7.Text == userSymbol && btn8.Text == userSymbol && btn9.Text == userSymbol ||
            btn1.Text == userSymbol && btn4.Text == userSymbol && btn7.Text == userSymbol ||
            btn2.Text == userSymbol && btn5.Text == userSymbol && btn8.Text == userSymbol ||
            btn3.Text == userSymbol && btn6.Text == userSymbol && btn9.Text == userSymbol ||
            btn1.Text == userSymbol && btn5.Text == userSymbol && btn9.Text == userSymbol ||
            btn3.Text == userSymbol && btn5.Text == userSymbol && btn7.Text == userSymbol) return 1;

        else if (btn1.Text == botSymbol && btn2.Text == botSymbol && btn3.Text == botSymbol ||
            btn4.Text == botSymbol && btn5.Text == botSymbol && btn6.Text == botSymbol ||
            btn7.Text == botSymbol && btn8.Text == botSymbol && btn9.Text == botSymbol ||
            btn1.Text == botSymbol && btn4.Text == botSymbol && btn7.Text == botSymbol ||
            btn2.Text == botSymbol && btn5.Text == botSymbol && btn8.Text == botSymbol ||
            btn3.Text == botSymbol && btn6.Text == botSymbol && btn9.Text == botSymbol ||
            btn1.Text == botSymbol && btn5.Text == botSymbol && btn9.Text == botSymbol ||
            btn3.Text == botSymbol && btn5.Text == botSymbol && btn7.Text == botSymbol) return 2;

        else if (activeButtons.Count == 0) return 3;

        else return 0;
    }
    
    private bool CheckStatus(int state)
    {
        if (state == 1 || state == 2 || state == 3)
        {
            foreach (var btn in activeButtons)
            {
                btn.IsEnabled = false;
            }
            if (state == 1)
            {
                BackgroundColor = Color.FromRgb(0, 43, 0);
                stats.Wins++;
            } else if (state == 2)
            {
                BackgroundColor = Color.FromRgb(48, 0, 0);
                stats.Losses++;
            } else if (state == 3)
            {
                BackgroundColor = Color.FromRgb(0, 0, 87);
            }
            SaveStats();
            LoadStats();
            return false; 
        }
        else return true;
    }

    private void NewGame()
    {
        activeButtons = grid.Children.OfType<Button>().Where(button => button != NewGameB).ToList();
        foreach (var btn in activeButtons)
        {
            btn.IsEnabled = true;
            btn.BackgroundColor = Color.FromRgb(56, 56, 56);
            btn.Text = string.Empty;
            BackgroundColor = Color.FromRgb(18, 18, 18);
        }
    }

    private void NewGameButton(object sender, EventArgs e)
    {
        NewGame();
    }
}
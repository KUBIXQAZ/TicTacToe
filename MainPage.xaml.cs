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
        buttons.Remove(btn);

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
        Button btn = buttons[random.Next(buttons.Count)];

        btn.Text = botSymbol;
        btn.IsEnabled = false;
        btn.BackgroundColor = Colors.Blue;
        buttons.Remove(btn);

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

        else return 0;
    }
    
    private bool CheckStatus(int state)
    {
        if (state == 1 || state == 2)
        {
            foreach (var btn in buttons)
            {
                btn.IsEnabled = false;
            }
            if (state == 1)
            {
                BackgroundColor = Color.FromRgb(0, 43, 0);
                stats.Wins++;
            }
            else if (state == 2)
            {
                BackgroundColor = Color.FromRgb(48, 0, 0);
                stats.Losses++;
            }
            SaveStats();
            LoadStats();
            return false; 
        }
        else return true;
    }

    private void NewGame()
    {
        buttons = grid.Children.OfType<Button>().Where(button => button != NewGameB).ToList();
        foreach (var btn in buttons)
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


using System.Windows;
using System.Windows.Input;

namespace GalConeServerMapGeneratorUi.ViewModel;
using TesterLib;
using GalConeServerMapGeneratorUi.inf;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MainViewModel : NotifyPropertyChanged
{
    private ServerClient server;
    private EventWorker worker;
    private PlanetWorker planetWorker;
    private PlayerHandler playerHandler;
    private string _infomationString;
    private ICommand _readySignal;
    private ICommand _generateMapBitmap;
    private ICommand _endGame;

    public ICommand EndGame
    {
        get
        {
            if (_endGame==null)
            {
                _endGame = new RelayCommand(param =>
                {
                    Task.Run(() =>
                    {
                        worker.sendEnd(playerHandler);
                        InfomationString = " End of server initiated";
                        System.Environment.Exit(0);
                    });
                });
            }

            return _endGame;
        }
    }
    public ICommand GenerateMapBitmap
    {
        get
        {
            if (_generateMapBitmap==null)
            {
                _generateMapBitmap = new RelayCommand(param =>
                {
                   
                   Task.Run(()=>  InfomationString = planetWorker.run(server.lastrecivedmessage));
                });
            }

            return _generateMapBitmap;
        }
    }
    public ICommand readySignal
    {
        get
        {
            if (_readySignal == null)
            {
                _readySignal = new RelayCommand(param => { worker.sendReady(); });
            }
            return _readySignal;
        }
    }

    public string InfomationString
    {
        get => _infomationString;

        set
        {
            _infomationString = value;
            NotifyOfPropertyChanged();
        }
    }


    public MainViewModel()
    {
        server = new ServerClient("127.0.0.1", 10800); 
        worker = new EventWorker(server);
        planetWorker = new PlanetWorker();
        server.Start();

        Task.Run(() => server.ReceiveEvents());
        while (server.lastrecivedmessage == null)
        {

        }
        var playerData = JObject.Parse(server.lastrecivedmessage);

        var playerId = playerData["id"].ToObject<int>();
        var players = playerData["players"].ToObject<List<PlayerHandler>>();

        // Create a new PlayerHandler instance with the deserialized values
         playerHandler = new PlayerHandler(playerId, "null", false,
            null, players, false);

        _infomationString = $"Id of player --{playerHandler.id}";



    }
}
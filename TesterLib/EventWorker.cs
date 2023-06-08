using Newtonsoft.Json;

namespace TesterLib;

public class EventWorker
{
    private ServerClient _serverClient;
    public EventWorker(ServerClient serverClient)
    {
        _serverClient = serverClient;
    }
    public void sendReady()
    {
        var message = new { name = "ready", ready = true };
        _serverClient.Send(JsonConvert.SerializeObject(message));
    }

    
    public void sendEnd(PlayerHandler _player)
    {
        var message = new {name ="end",player= _player};
        _serverClient.Send(JsonConvert.SerializeObject(message));
    }
}
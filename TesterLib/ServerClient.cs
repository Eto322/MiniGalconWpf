using System.Net.Sockets;
using System.Text;

namespace TesterLib;

public class ServerClient
{
    static TcpClient _client;
    static NetworkStream _stream;
    static BinaryWriter _writer;
    static BinaryReader _reader;


    public string serverAddress { get; set; }
    public string lastrecivedmessage { get; set; } = null;
    public int serverPort { get; set; }
    public bool isWorking=false;
    public ServerClient(string serverAddress,int serverPort)
    {
        this.serverAddress = serverAddress;
        this.serverPort = serverPort;
    }

    public  void Start()
    {
        _client = new TcpClient(serverAddress, serverPort);
        _stream = _client.GetStream();
        _writer = new BinaryWriter(_stream);
        _reader = new BinaryReader(_stream);
        isWorking=true;
    }

   public void Send(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        _writer.Write(bytes.Length);
        _writer.Write(bytes);
        _writer.Flush();
    }

    public void ReceiveEvents()
    {
        Console.WriteLine("Started receiving data from server...");

        while (_client.Connected)
        {
            if (!_stream.DataAvailable)
            {
                continue;
            }

            try
            {
                var size = _reader.ReadInt32();
                var message = _reader.ReadBytes(size);
                var received = Encoding.UTF8.GetString(message);
                lastrecivedmessage= received;
                Console.WriteLine($"Received: {received}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to process server message due to exception: " + ex.Message);
            }
        }

        Console.WriteLine("Connection closed. Stopped receiving.");
    }
}
using System.Reflection;

namespace TesterLib;

public class PlayerHandler
{
    public int id { get; }
    public string nickname { get; }

    public bool ready { get; }

    public string addr { get; }

    public List<PlayerHandler> handlers { get; }

    public bool isIntialazied { get; }

    public PlayerHandler(int id, string nickname, bool ready, string addr, List<PlayerHandler> handlers, bool isIntialazied)
    {
        this.id = id;
        this.nickname = nickname;
        this.ready = ready;
        this.addr = addr;
        this.handlers = handlers;
        this.isIntialazied = isIntialazied;
    }

    public void DisplayFields()
    {
        Type type = typeof(PlayerHandler);
        PropertyInfo[] properties = type.GetProperties();

        Console.WriteLine("PlayerHandler fields:");

        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(this);
            Console.WriteLine($"{property.Name}: {value}");
        }
    }
}
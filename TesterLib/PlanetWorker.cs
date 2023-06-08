using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesterLib
{

    public class PlanetInfo
    {
        public int Size { get; set; }
        public int? Owner { get; set; }
        public int UnitsCount { get; set; }
        public Coords Coords { get; set; }
        public int Id { get; set; }

        public PlanetInfo(int size, int? owner, int unitsCount, Coords coords, int id)
        {
            Size = size;
            Owner = owner;
            UnitsCount = unitsCount;
            Coords = coords;
            Id = id;
        }
    }

    public class Coords
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class PlanetWorker
    {
        public string run(string json)
        {
            var data = JObject.Parse(json);
            var name = data["name"].ToString();

            Console.WriteLine($"Received event: {name}");

            if (name == "map_init")
            {
                var planets = data["map"];

                Console.WriteLine("Planets:");

                // Collect planet information
                var planetInfoList = new List<PlanetInfo>();
                foreach (var planet in planets)
                {
                    var size = planet["size"].ToObject<int>();
                    var owner = planet["owner"]?.ToObject<int?>();
                    var unitsCount = planet["units_count"].ToObject<int>();
                    var coords = planet["coords"].ToObject<Coords>();
                    var id = planet["id"].ToObject<int>();

                    var planetInfo = new PlanetInfo(size, owner, unitsCount, coords, id);
                    planetInfoList.Add(planetInfo);

                    Console.WriteLine($"Planet ID: {id}");
                    Console.WriteLine($"Size: {size}");
                    Console.WriteLine($"Owner: {(owner != null ? owner.ToString() : "None")}");
                    Console.WriteLine($"Units Count: {unitsCount}");
                    Console.WriteLine($"Coordinates: X={coords.X}, Y={coords.Y}");
                    Console.WriteLine();
                }

                // Draw the image
                using (var bitmap = new Bitmap(1000, 1000))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.Black);

                    foreach (var planetInfo in planetInfoList)
                    {
                        var x = planetInfo.Coords.X;
                        var y = planetInfo.Coords.Y;
                        var radius = planetInfo.Size * 10;

                        // Calculate the position on the image based on the coordinates
                        var positionX = 400 + (int)x;
                        var positionY = 300 + (int)y;

                        // Determine the color based on the owner
                        Color planetColor = Color.Blue;

                        switch (planetInfo.Owner)
                        {
                            case 1:
                                planetColor = Color.Red;
                                break;
                            case 2:
                                planetColor = Color.Green; 
                                break;
                            case 3:
                                planetColor = Color.BlueViolet;
                                break;
                            case 4:
                                planetColor=Color.CornflowerBlue;
                                break;
                            case 5:
                                planetColor=Color.OrangeRed;
                                break;
                            case 6:
                                planetColor= Color.Orchid;
                                break;
                            case 7: 
                                planetColor=Color.DarkSalmon;
                                break;
                            case 8:
                                planetColor = Color.Yellow;
                                break;
                        }


                        // Draw the planet
                        graphics.FillEllipse(new SolidBrush(planetColor), positionX - radius, positionY - radius, radius * 2, radius * 2);

                        // Draw the planet ID
                        graphics.DrawString(planetInfo.Id.ToString(), new Font("Arial", 10), Brushes.Black, positionX - 10, positionY - 5);
                    }

                   
                    if (!Directory.Exists("results"))
                    {
                        Directory.CreateDirectory("results");
                    }
                    var filePath = Path.Combine("results", "planet_map.png");
                    bitmap.Save(filePath, ImageFormat.Png);

                    filePath = Path.Combine("results", "data.json");
                    string jsonData = data.ToString();
                    using (StreamWriter writer = File.CreateText(filePath))
                    {
                        writer.Write(jsonData);
                    }

                }

                return ("Image saved as planet_map.png.");
            }
            else
            {
               return ("Not all player connected ");
            }
        }
    }
    
}

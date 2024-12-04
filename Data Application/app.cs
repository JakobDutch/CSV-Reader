using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "C:\Users\boris\Documents\BML\Dataset XR Applicatie(Sheet1).CSV"; // Replace with the actual file path

        try
        {
            var graphData = ParseCsvToGraphData(filePath);

            foreach (var entry in graphData)
            {
                Console.WriteLine($"Title: {entry.Key}");
                Console.WriteLine($"X: {entry.Value.X}, Y: {entry.Value.Y}, Z: {entry.Value.Z}");
                Console.WriteLine($"Extra Info: Country: {entry.Value.ExtraInfo["country"]}, City: {entry.Value.ExtraInfo["city"]}, Start Year: {entry.Value.ExtraInfo["start_year"]}");
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static Dictionary<string, GraphPoint> ParseCsvToGraphData(string filePath)
    {
        var graphData = new Dictionary<string, GraphPoint>();

        using (var reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine(); // Read and skip the header row

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var values = line.Split(',');

                string title = values[0];

                // Parse X value
                float xValue = float.Parse(values[1], CultureInfo.InvariantCulture);

                // Parse Y value (clean currency formatting if needed)
                string yValueStr = values[2].Replace("\u20ac", "").Replace(",", "").Trim();
                float yValue = float.Parse(yValueStr, CultureInfo.InvariantCulture);

                // Parse Z value (handle empty values as nullable floats)
                float? zValue = string.IsNullOrWhiteSpace(values[3]) ? (float?)null : float.Parse(values[3], CultureInfo.InvariantCulture);

                // Collect extra information
                var extraInfo = new Dictionary<string, string>
                {
                    { "country", values[4] },
                    { "city", values[5] },
                    { "start_year", values[6] }
                };

                graphData[title] = new GraphPoint
                {
                    X = xValue,
                    Y = yValue,
                    Z = zValue,
                    ExtraInfo = extraInfo
                };
            }
        }
        return graphData;
    }
}

class GraphPoint
{
    public float X { get; set; }
    public float Y { get; set; }
    public float? Z { get; set; } // Nullable for cases where Z is not available
    public Dictionary<string, string> ExtraInfo { get; set; }
}

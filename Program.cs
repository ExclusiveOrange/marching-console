using System;
using System.Text;

class Program
{
  private static int seed = 1337;
  
  private static int width = 40;
  private static int height = 40;
  
  private static float xScale = 5f;
  private static float xOffset = 0f;
  
  private static float yScale = 5f;
  private static float yOffset = 0f;
  
  public static void
    Main(string[] args)
  {
    var fastNoise = new FastNoiseLite();
    fastNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
    fastNoise.SetSeed(seed);
    
    var stringBuilder = new StringBuilder();

    for (int y = 0; y < height; ++y)
    {
      stringBuilder.Clear();

      for (int x = 0; x < width; ++x)
      {
        float noiseValue = fastNoise.GetNoise((x + xOffset) * xScale, (y + yOffset) * yScale);
        stringBuilder.Append(ValueToText(noiseValue));
      }
      
      Console.WriteLine(stringBuilder.ToString());
    }
  }

  private static string
    ValueToText(float negOneToPosOne)
  {
    return negOneToPosOne < 0 ? "  " : "##";
  }
}
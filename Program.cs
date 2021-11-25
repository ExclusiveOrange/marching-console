using System;
using System.Text;

//
// a   b
//   x
// c   d
//
// where x is the text character to decide
// where a,b,c,d are floating point values in [-1, +1]
//
// brute algorithm:
//   ...
//   keep two row buffers:
//     [..., a, b, ...]
//     [..., c, d, ...]
//   ???

class Program
{
  private static int seed = 1337;

  private static int width = 160;
  private static int height = 40;

  private static float xScale = 6f;
  private static float xOffset = 0f;

  private static float yScale = 10f;
  private static float yOffset = -10f;

  private static char[] charTable =
  {
    // from unicode 2500 to 25FF
    /*  0 */ ' ',
    /*  1 */ '▘',
    /*  2 */ '▝',
    /*  3 */ '▀',
    /*  4 */ '▖',
    /*  5 */ '▌',
    /*  6 */ '▞',
    /*  7 */ '▛',
    /*  8 */ '▗',
    /*  9 */ '▚',
    /* 10 */ '▐',
    /* 11 */ '▜',
    /* 12 */ '▄',
    /* 13 */ '▙',
    /* 14 */ '▟',
    /* 15 */ '░'
  };

  public static void
    Main(string[] args)
  {
    Console.OutputEncoding = Encoding.UTF8;

    var fastNoise = new FastNoiseLite();
    fastNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
    fastNoise.SetSeed(seed);

    void fillRow(float[] row, int y)
    {
      for (int x = 0; x <= width; ++x)
        row[x] = fastNoise.GetNoise((x + xOffset - 0.5f) * xScale, (y + yOffset - 0.5f) * yScale);
    }

    float[] rowA = new float[width + 1];
    float[] rowB = new float[width + 1];

    var stringBuilder = new StringBuilder();

    fillRow(rowA, 0);

    for (int y = 0;;)
    {
      stringBuilder.Clear();
      fillRow(rowB, y);

      for (int x = 0; x < width; ++x)
      {
        var (tl, tr, bl, br) = (rowA[x], rowA[x + 1], rowB[x], rowB[x + 1]);

        var charIndex = (tl > 0 ? 1 : 0) + (tr > 0 ? 2 : 0) + (bl > 0 ? 4 : 0) + (br > 0 ? 8 : 0);

        stringBuilder.Append(charTable[charIndex]);
      }

      Console.WriteLine(stringBuilder.ToString());

      if (++y >= height)
        break;

      (rowB, rowA) = (rowA, rowB);
    }
  }

  private static string
    ValueToText(float negOneToPosOne)
  {
    return negOneToPosOne < 0 ? "  " : "##";
  }
}
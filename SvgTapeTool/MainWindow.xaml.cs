using ASCIIToPaperTapeSVG;
using Microsoft.Win32;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using Svg;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SvgTapeTool;

/// // <summary>
/// This tool generates SVG representations of paper tape segments.
/// based on colemanjw2's work at https://github.com/colemanjw2/ASCII2PaperTape/
/// </summary>
public partial class MainWindow : Window
{
    private int maxColumns = 0; // Default max columns per line for input where 0 is unlimited

    public MainWindow()
    {
        InitializeComponent();
        tbInput.Text = "10 PRINT \"HELLO WORLD!\"\r\n20 GOTO 10"; // Default input
        this.DataContext = new ConfigurationViewModel();
    }

    private void btRun_Click(object sender, RoutedEventArgs e)
    {
        string input = tbInput.Text.Trim();

        // Handle line endings
        string lineEnding = cbLineEndings.SelectedIndex == 0 ? "\r\n" : "\n";
        input = input.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", lineEnding);

        string processed = ParseControlCodes(input);

        string outputFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "paper_tape.svg");
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            InitialDirectory = Directory.GetCurrentDirectory(),
            FileName = "paper_tape.svg",
            DefaultExt = ".svg",
            Filter = "SVG files (*.svg)|*.svg"
        };
        if (saveFileDialog.ShowDialog() == true)
        {
            outputFile = saveFileDialog.FileName;
        }

        GenerateSVG(processed, outputFile);

        var settings = new WpfDrawingSettings
        {
            IncludeRuntime = true,
            TextAsGeometry = false
        };

        var reader = new FileSvgReader(settings);
        var drawing = reader.Read(outputFile);

        SvgViewer.Source = new Uri(outputFile);
    }

    public static string ParseControlCodes(string input)
    {
        return Regex.Replace(input, @"<0x([0-9A-Fa-f]{2})>", match =>
        {
            int value = Convert.ToInt32(match.Groups[1].Value, 16);
            return ((char)value).ToString();
        });
    }
    public static List<string> SplitTextIntoSegments(string fullText)
    {
        float usableWidth = TapeConstants.MAX_TAPE_LENGTH_IN - 2 * TapeConstants.HORIZONTAL_MARGIN;
        int maxChars = (int)(usableWidth / TapeConstants.COLUMN_SPACING);

        List<string> segments = new();
        for (int i = 0; i < fullText.Length; i += maxChars)
        {
            int length = Math.Min(maxChars, fullText.Length - i);
            segments.Add(fullText.Substring(i, length));
        }

        return segments;
    }

    public static float ComputeSegmentWidthIn(string segment)
    {
        return 2 * TapeConstants.HORIZONTAL_MARGIN + segment.Length * TapeConstants.COLUMN_SPACING;
    }

    public static void DrawSegment(SvgGroup group, string textSegment, float yOffset)
    {
        for (int i = 0; i < textSegment.Length; i++)
        {
            int code = textSegment[i];
            float x = TapeConstants.HORIZONTAL_MARGIN + i * TapeConstants.COLUMN_SPACING;

            // Sprocket hole
            AddCircle(group, x, yOffset + TapeConstants.VerticalPositions[TapeConstants.SPROCKET_IDX], TapeConstants.SPROCKET_HOLE_DIAM / 2, System.Drawing.Color.Red);

            for (int bit = 0; bit < 8; bit++)
            {
                if (((code >> bit) & 1) == 1)
                {
                    float y = yOffset + TapeConstants.VerticalPositions[TapeConstants.BitIndexMap[bit]];
                    AddCircle(group, x, y, TapeConstants.DATA_HOLE_DIAM / 2, System.Drawing.Color.Red);
                }
            }
        }
    }

    public static void AddCircle(SvgGroup group, float xIn, float yIn, float radiusIn, System.Drawing.Color strokeColor)
    {
        var circle = new SvgCircle
        {
            CenterX = new SvgUnit(SvgUnitType.Inch, xIn),
            CenterY = new SvgUnit(SvgUnitType.Inch, yIn),
            Radius = new SvgUnit(SvgUnitType.Inch, radiusIn),
            Fill = SvgPaintServer.None,
            Stroke = new SvgColourServer(strokeColor),
            StrokeWidth = new SvgUnit(SvgUnitType.Millimeter, 0.1f)
        };
        group.Children.Add(circle);
    }
    public static void GenerateSVG(string fullText, string filename)
    {
        var segments = SplitTextIntoSegments(fullText);
        float totalHeightIn = segments.Count * TapeConstants.TAPE_HEIGHT;
        float totalWidthIn = 0;
        foreach (var seg in segments)
        {
            totalWidthIn = Math.Max(totalWidthIn, ComputeSegmentWidthIn(seg));
        }

        var svgDoc = new SvgDocument
        {
            Width = new SvgUnit(SvgUnitType.Inch, totalWidthIn),
            Height = new SvgUnit(SvgUnitType.Inch, totalHeightIn)
        };

        for (int idx = 0; idx < segments.Count; idx++)
        {
            string segment = segments[idx];
            float yOffset = idx * TapeConstants.TAPE_HEIGHT;

            var group = new SvgGroup();

            // Rectangle around segment
            var rect = new SvgRectangle
            {
                X = new SvgUnit(SvgUnitType.Inch, 0),
                Y = new SvgUnit(SvgUnitType.Inch, yOffset),
                Width = new SvgUnit(SvgUnitType.Inch, ComputeSegmentWidthIn(segment)),
                Height = new SvgUnit(SvgUnitType.Inch, TapeConstants.TAPE_HEIGHT),
                Fill = SvgPaintServer.None,
                Stroke = new SvgColourServer(System.Drawing.Color.Black),
                StrokeWidth = new SvgUnit(SvgUnitType.Millimeter, 0.1f)
            };
            group.Children.Add(rect);

            DrawSegment(group, segment, yOffset);

            // Label text
            var label = new SvgText($"Tape #{idx + 1} (chars={segment.Length})")
            {
                X = new SvgUnitCollection { new SvgUnit(SvgUnitType.Inch, 0.05f) },
                Y = new SvgUnitCollection { new SvgUnit(SvgUnitType.Inch, yOffset + 0.05f) },
                FontSize = new SvgUnit(SvgUnitType.Inch, 0.05f),
                Stroke = new SvgColourServer(System.Drawing.Color.Blue),
                StrokeWidth = new SvgUnit(SvgUnitType.Millimeter, 0.02f),
                Fill = SvgPaintServer.None
            };
            group.Children.Add(label);

            svgDoc.Children.Add(group);
        }

        svgDoc.Write(filename);
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        e.Handled = true;
    }

    private void cbColumnMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cbColumnMode.SelectedItem.ToString().Contains("40"))
        {
            maxColumns = 40;
        }
        else if (cbColumnMode.SelectedItem.ToString().Contains("80"))
        {
            maxColumns = 80;
        }
        else
        {
            maxColumns = 0; // No limit
        }
    }

    private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        tbInput.TextChanged -= tbInput_TextChanged; // Avoid recursion
        FormatTextBoxLines();
        tbInput.TextChanged += tbInput_TextChanged;
    }

    private void FormatTextBoxLines()
    {
        if (maxColumns == 0)
            return; // Do nothing if unlimited

        string[] lines = tbInput.Text.Replace("\r\n", "\n").Split('\n');
        List<string> formattedLines = new List<string>();

        foreach (string line in lines)
        {
            int index = 0;
            while (index < line.Length)
            {
                int length = Math.Min(maxColumns, line.Length - index);
                formattedLines.Add(line.Substring(index, length));
                index += maxColumns;
            }

            // Handle empty lines
            if (line.Length == 0)
                formattedLines.Add("");
        }

        int cursor = tbInput.SelectionStart;
        tbInput.Text = string.Join(Environment.NewLine, formattedLines);
        tbInput.SelectionStart = Math.Min(cursor, tbInput.Text.Length);
    }
}
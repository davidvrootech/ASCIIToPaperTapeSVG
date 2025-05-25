using ExCSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgTapeTool
{
    public static class TapeConstants
    {
        static TapeConstants()
        {
            // Load persisted settings on first access
            TAPE_HEIGHT = ASCIIToPaperTapeSVG.Properties.Settings.Default.TapeHeight;
            HORIZONTAL_MARGIN = ASCIIToPaperTapeSVG.Properties.Settings.Default.HorizontalMargin;
            COLUMN_SPACING = ASCIIToPaperTapeSVG.Properties.Settings.Default.ColumnSpacing;
            DATA_HOLE_DIAM = ASCIIToPaperTapeSVG.Properties.Settings.Default.DataHoleDiam;
            SPROCKET_HOLE_DIAM = ASCIIToPaperTapeSVG.Properties.Settings.Default.SprocketHoleDiam;
            MAX_TAPE_LENGTH_IN = ASCIIToPaperTapeSVG.Properties.Settings.Default.MaxTapeLengthIn;
        }

        public static float TAPE_HEIGHT { get; set; }
        public static float HORIZONTAL_MARGIN { get; set; }
        public static float COLUMN_SPACING { get; set; }
        public static float DATA_HOLE_DIAM { get; set; }
        public static float SPROCKET_HOLE_DIAM { get; set; }
        public static float MAX_TAPE_LENGTH_IN { get; set; }

        public static readonly float[] VerticalPositions = {
        0.1f, // bit7
        0.2f, // bit6
        0.3f, // bit5
        0.4f, // bit4
        0.5f, // bit3
        0.6f, // sprocket
        0.7f, // bit2
        0.8f, // bit1
        0.9f  // bit0
    };

        public static readonly Dictionary<int, int> BitIndexMap = new Dictionary<int, int> {
        { 7, 0 }, { 6, 1 }, { 5, 2 }, { 4, 3 },
        { 3, 4 }, { 2, 6 }, { 1, 7 }, { 0, 8 }
    };

        public const int SPROCKET_IDX = 5;
    }
}

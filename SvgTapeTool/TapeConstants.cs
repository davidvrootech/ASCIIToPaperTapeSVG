using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgTapeTool
{
    public static class TapeConstants
    {
        public const float TAPE_HEIGHT = 1.0f;
        public const float HORIZONTAL_MARGIN = 0.25f;
        public const float COLUMN_SPACING = 0.1f;
        public const float DATA_HOLE_DIAM = 0.072f;
        public const float SPROCKET_HOLE_DIAM = 0.046f;
        public const float MAX_TAPE_LENGTH_IN = 16.0f;

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

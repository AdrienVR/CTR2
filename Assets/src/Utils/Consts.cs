
namespace Consts
{
    public class Layer
    {
        public const int Default = 0;
        public const int TransparentFX = 1;
        public const int IgnoreRaycast = 2;
        public const int Water = 4;
        public const int UI = 5;
        public const int layer_j1 = 8;
        public const int layer_j2 = 9;
        public const int layer_j3 = 10;
        public const int layer_j4 = 11;
        public const int layer2d_j1 = 12;
        public const int layer2d_j2 = 13;
        public const int layer2d_j3 = 14;
        public const int layer2d_j4 = 15;
        public const int layer2d_common = 16;
        public const int Ground = 17;
        public const int Wall = 18;
    }

    public class LayerMaskInt
    {
        public const int Ground = 1 << Layer.Ground;
        public const int Wall = 1 << Layer.Wall;
    }
}

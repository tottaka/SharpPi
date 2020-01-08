using OpenTK;

namespace SharpPi.Graphics
{
    public struct Vertex
    {
        /// <summary>
        /// Size of the <see cref="Vertex"/> struct in bytes.
        /// </summary>
        public const int Size = (3 + 2 + 3) * 4;

        public readonly Vector3 Position;
        public readonly Vector2 UV;
        public readonly Vector3 Normal;

        public Vertex(Vector3 position, Vector2 uv, Vector3 normal)
        {
            Position = position;
            UV = uv;
            Normal = normal;
        }
    }
}

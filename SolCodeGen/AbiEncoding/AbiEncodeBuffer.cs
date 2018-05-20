using System;

namespace SolCodeGen.AbiEncoding
{
    public ref struct AbiEncodeBuffer
    {
        /// <summary>
        /// Entire buffer (head and tail).
        /// </summary>
        public Span<byte> Buffer;

        /// <summary>
        /// Byte count that the head buffer has been incremented to.
        /// </summary>
        public int HeadCursorPosition;

        /// <summary>
        /// Length of the header section of the buffer
        /// </summary>
        public int HeadLength;

        public Span<byte> Head;
        public Span<byte> HeadCursor;

        public int DataAreaCursorPosition;
        public Span<byte> DataArea;
        public Span<byte> DataAreaCursor;

        /*
        public static implicit operator AbiDataBuffer(byte[] data) => (ReadOnlySpan<byte>)data;
        public static implicit operator AbiDataBuffer(ReadOnlySpan<byte> data) => new AbiDataBuffer {
            Buffer = data,
            Head = data,
            HeadCursor = data
        };
        */

        public AbiEncodeBuffer(string hexString, params AbiTypeInfo[] typeInfo)
            : this(hexString.HexToSpan(), typeInfo) { }

        public AbiEncodeBuffer(Memory<byte> mem, params AbiTypeInfo[] typeInfo)
            : this(mem.Span, typeInfo) { }

        public AbiEncodeBuffer(Span<byte> span, params AbiTypeInfo[] typeInfo)
        {
            Buffer = span;
            HeadLength = 0;

            foreach (var t in typeInfo)
            {
                switch (t.Category)
                {
                    case SolidityTypeCategory.FixedArray:
                        HeadLength += UInt256.SIZE * t.ArrayLength;
                        break;
                    default:
                        HeadLength += UInt256.SIZE;
                        break;
                }
            }

            HeadCursorPosition = 0;
            Head = Buffer.Slice(0, HeadLength);
            HeadCursor = Head;

            DataAreaCursorPosition = 0;
            DataArea = span.Slice(HeadLength);
            DataAreaCursor = DataArea;
        }

        public void IncrementHeadCursor(int len)
        {
            HeadCursorPosition += len;
            HeadCursor = HeadCursor.Slice(len);
        }

        public void IncrementDataCursor(int len)
        {
            DataAreaCursorPosition += len;
            DataAreaCursor = DataAreaCursor.Slice(len);
        }
    }

}

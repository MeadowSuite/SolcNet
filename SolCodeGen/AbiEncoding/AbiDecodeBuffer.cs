﻿using HoshoEthUtil;
using SolCodeGen.Utils;
using System;

namespace SolCodeGen.AbiEncoding
{
    /// <summary>
    /// Stackalloc struct used for ABI encoding & decoding.
    /// Static types (primitive types of fixed size) are encoded entirely in the header.
    /// Dynamic types (variable size, eg: string, bytes, T[]) have their data area
    /// offset encoded in the head, and their actual contents in the data area buffer.
    /// <see href="http://solidity.readthedocs.io/en/latest/abi-spec.html#use-of-dynamic-types"/>
    /// </summary>
    public ref struct AbiDecodeBuffer
    {
        /// <summary>
        /// Entire buffer (head and tail).
        /// </summary>
        public ReadOnlySpan<byte> Buffer;

        public ReadOnlySpan<byte> HeadCursor;

        public AbiDecodeBuffer(string hexString, params AbiTypeInfo[] typeInfo)
            : this(hexString.HexToReadOnlySpan(), typeInfo) { }

        public AbiDecodeBuffer(ReadOnlyMemory<byte> mem, params AbiTypeInfo[] typeInfo)
            : this(mem.Span, typeInfo) { }

        public AbiDecodeBuffer(ReadOnlySpan<byte> span, params AbiTypeInfo[] typeInfo)
        {
            Buffer = span;
            int headLength = 0;

            foreach(var t in typeInfo)
            {
                switch(t.Category)
                {
                    case SolidityTypeCategory.FixedArray:
                        headLength += UInt256.SIZE * t.ArrayLength;
                        break;
                    default:
                        headLength += UInt256.SIZE;
                        break;
                }
            }

            HeadCursor = Buffer.Slice(0, headLength);
        }

        public void IncrementHeadCursor(int len)
        {
            HeadCursor = HeadCursor.Slice(len);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;

namespace SolCodeGen.AbiEncoding.Encoders
{
    public class DynamicArrayEncoder<TItem> : AbiTypeEncoder<IEnumerable<TItem>>
    {
        IAbiTypeEncoder<TItem> _itemEncoder;

        public DynamicArrayEncoder(IAbiTypeEncoder<TItem> itemEncoder)
        {
            _itemEncoder = itemEncoder;
        }

        public override int GetEncodedSize()
        {
            if (_info.Category != SolidityTypeCategory.DynamicArray)
            {
                throw UnsupportedTypeException();
            }
            int len = _itemEncoder.GetEncodedSize() * _val.Count();
            return 32 + len;
        }

        public override Span<byte> Encode(Span<byte> buffer)
        {
            if (_info.Category != SolidityTypeCategory.DynamicArray)
            {
                throw UnsupportedTypeException();
            }

            // write length prefix
            buffer = UInt256Encoder.Encode(buffer, _val.Count());

            foreach (var item in _val)
            {
                _itemEncoder.SetValue(item);
                buffer = _itemEncoder.Encode(buffer);
            }

            return buffer;
        }

        public override ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> buffer, out IEnumerable<TItem> val)
        {
            buffer = UInt256Encoder.Decode(buffer, out var len);
            if (len > int.MaxValue)
            {
                throw new ArgumentException($"Array input data is invalid: the byte length prefix is {len} which is unlikely to be intended");
            }
            var items = new TItem[(int)len];
            for (var i = 0; i < len; i++)
            {
                buffer = _itemEncoder.Decode(buffer, out var item);
                items[i] = item;
            }
            val = items;
            return buffer;
        }
    }

}

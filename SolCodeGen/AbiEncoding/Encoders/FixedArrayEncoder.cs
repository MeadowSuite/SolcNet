using System;
using System.Collections.Generic;
using System.Linq;

namespace SolCodeGen.AbiEncoding.Encoders
{
    public class FixedArrayEncoder<TItem> : AbiTypeEncoder<IEnumerable<TItem>>
    {
        IAbiTypeEncoder<TItem> _itemEncoder;

        public FixedArrayEncoder(IAbiTypeEncoder<TItem> itemEncoder)
        {
            _itemEncoder = itemEncoder;
        }

        public override int GetEncodedSize()
        {
            if (_info.Category != SolidityTypeCategory.FixedArray)
            {
                throw UnsupportedTypeException();
            }
            int len = _itemEncoder.GetEncodedSize() * _info.ArrayLength;
            return len;
        }

        public override Span<byte> Encode(Span<byte> buffer)
        {
            if (_info.Category != SolidityTypeCategory.FixedArray)
            {
                throw UnsupportedTypeException();
            }

            var itemCount = _val.Count();
            if (itemCount != _info.ArrayLength)
            {
                throw new ArgumentOutOfRangeException($"Fixed size array type '{_info.SolidityName}' needs exactly {_info.ArrayLength} items, was given {itemCount}");
            }

            foreach (var item in _val)
            {
                _itemEncoder.SetValue(item);
                buffer = _itemEncoder.Encode(buffer);
            }

            return buffer;
        }

        public override ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> buffer, out IEnumerable<TItem> val)
        {
            var items = new TItem[_info.ArrayLength];
            for(var i = 0; i < items.Length; i++)
            {
                buffer = _itemEncoder.Decode(buffer, out var item);
                items[i] = item;
            }
            val = items;
            return buffer;
        }

    }

}

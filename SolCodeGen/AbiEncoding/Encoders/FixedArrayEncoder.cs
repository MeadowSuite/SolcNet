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

        public override void Encode(ref AbiEncodeBuffer buff)
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
                _itemEncoder.Encode(ref buff);
            }
            
        }

        public override void Decode(ref AbiDecodeBuffer buff, out IEnumerable<TItem> val)
        {
            var items = new TItem[_info.ArrayLength];
            for(var i = 0; i < items.Length; i++)
            {
                _itemEncoder.Decode(ref buff, out var item);
                items[i] = item;
            }
            val = items;
        }

    }

}

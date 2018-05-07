using System;
using System.Collections.Generic;

namespace SolCodeGen.SolidityTypeEncoding
{
    public class ArrayEncoder<TItem> : ISolidityTypeEncoder<IEnumerable<TItem>>
    {
        IEnumerable<TItem> _val;
        SolidityTypeInfo _info;

        ISolidityTypeEncoder<TItem> _itemEncoder;

        public ArrayEncoder(ISolidityTypeEncoder<TItem> itemEncoder)
        {
            _itemEncoder = itemEncoder;
        }

        public void SetValue(in IEnumerable<TItem> val)
        {
            _val = val;
            throw new NotImplementedException();
        }

        public void SetTypeInfo(SolidityTypeInfo info)
        {
            _info = info;
        }

        public int GetEncodedSize()
        {
            throw new NotImplementedException();
        }

        public Span<byte> Encode(Span<byte> buffer)
        {
            if (_info.Category == SolidityTypeCategory.Bytes)
            {
                throw new NotImplementedException();
            }
            else if (_info.Category == SolidityTypeCategory.String)
            {
                throw new NotImplementedException();
            }
            else if (_info.Category == SolidityTypeCategory.BytesM)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (_info.Category == SolidityTypeCategory.DynamicArray)
                {
                    UInt256 len = _info.ArrayLength;
                    var lenEncoder = new UInt256Encoder();
                    lenEncoder.SetTypeInfo(_info.ArrayItemInfo);
                    lenEncoder.SetValue(len);
                    buffer = lenEncoder.Encode(buffer);
                }
                else if (_info.Category != SolidityTypeCategory.FixedArray)
                {
                    throw new ArgumentException($"Encoder does not support solidity type category '{_info.Category}', type name: {_info.SolidityName}");
                }
                foreach (var item in _val)
                {
                    _itemEncoder.SetValue(item);
                    buffer = _itemEncoder.Encode(buffer);
                }
            }

            return buffer;

        }

    }

}

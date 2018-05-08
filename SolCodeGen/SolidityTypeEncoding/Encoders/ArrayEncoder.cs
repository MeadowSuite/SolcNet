using System;
using System.Collections.Generic;
using System.Linq;

namespace SolCodeGen.SolidityTypeEncoding.Encoders
{
    public class ArrayEncoder<TItem> : SolidityTypeEncoder<IEnumerable<TItem>>
    {

        ISolidityTypeEncoder<TItem> _itemEncoder;

        public ArrayEncoder(ISolidityTypeEncoder<TItem> itemEncoder)
        {
            _itemEncoder = itemEncoder;
        }

        public override int GetEncodedSize()
        {
            switch (_info.Category)
            {
                case SolidityTypeCategory.DynamicArray:
                    {
                        int len = _itemEncoder.GetEncodedSize() * _val.Count();
                        return 32 + len;
                    }
                case SolidityTypeCategory.FixedArray:
                    {
                        int len = _itemEncoder.GetEncodedSize() * _info.ArrayLength;
                        return len;
                    }
                default:
                    throw UnsupportedTypeException();
            }
        }

        public override Span<byte> Encode(Span<byte> buffer)
        {
            if (_info.Category == SolidityTypeCategory.DynamicArray)
            {
                // write length prefix
                buffer = UInt256Encoder.EncodeUnchecked(buffer, _val.Count());
            }
            else if (_info.Category != SolidityTypeCategory.FixedArray)
            {
                throw UnsupportedTypeException();
            }

            foreach (var item in _val)
            {
                _itemEncoder.SetValue(item);
                buffer = _itemEncoder.Encode(buffer);
            }

            return buffer;

        }

    }

}

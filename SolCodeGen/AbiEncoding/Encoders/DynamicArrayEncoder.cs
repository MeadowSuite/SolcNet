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
            return (UInt256.SIZE * 2) + len;
        }

        public override void Encode(ref AbiEncodeBuffer buff)
        {
            if (_info.Category != SolidityTypeCategory.DynamicArray)
            {
                throw UnsupportedTypeException();
            }


            // write data offset position into header
            int offset = buff.HeadLength + buff.DataAreaCursorPosition;
            UInt256Encoder.Instance.Encode(buff.HeadCursor, offset);
            buff.IncrementHeadCursor(UInt256.SIZE);

            // write array item count into data buffer
            int len = _val.Count();
            UInt256Encoder.Instance.Encode(buff.DataAreaCursor, len);
            buff.IncrementDataCursor(UInt256.SIZE);

            // write payload into data buffer
            var payloadBuffer = new AbiEncodeBuffer(buff.DataAreaCursor, Enumerable.Repeat(_info.ArrayItemInfo, len).ToArray());
            foreach (var item in _val)
            {
                _itemEncoder.SetValue(item);
                _itemEncoder.Encode(ref payloadBuffer);
            }
        }

        public override void Decode(ref AbiDecodeBuffer buff, out IEnumerable<TItem> val)
        {
            // Read the next header int which is the offset to the start of the data
            // in the data payload area.
            UInt256Encoder.Instance.Decode(buff.HeadCursor, out int startingPosition);

            // The first int in our offset of data area is the length of the rest of the payload.
            var encodedLength = buff.Buffer.Slice(startingPosition, UInt256.SIZE);
            UInt256Encoder.Instance.Decode(encodedLength, out int itemCount);

            var payloadOffset = startingPosition + UInt256.SIZE;
            var payload = buff.Buffer.Slice(payloadOffset, buff.Buffer.Length - payloadOffset);
            var payloadBuffer = new AbiDecodeBuffer(payload, Enumerable.Repeat(_info.ArrayItemInfo, itemCount).ToArray());
            var items = new TItem[itemCount];
            for (var i = 0; i < itemCount; i++)
            {
                _itemEncoder.Decode(ref payloadBuffer, out var item);
                items[i] = item;
            }
            val = items;

            buff.IncrementHeadCursor(UInt256.SIZE);
        }
    }

}

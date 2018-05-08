using SolcNet;
using System;
using System.Runtime.InteropServices;

namespace SolCodeGen
{

    [StructLayout(LayoutKind.Sequential)]
    public struct Address : IEquatable<Address>
    {
        public const int SIZE = 20;

        public static readonly Address Zero = new Address();

        // parts are big-endian
        readonly uint P1;
        readonly uint P2;
        readonly uint P3;
        readonly uint P4;
        readonly uint P5;

        public byte[] GetBytes() => MemoryMarshal.AsBytes(new Span<uint>(new[] { P1, P2, P3, P4, P5 })).ToArray();
        public string GetHexString() => HexConverter.GetHex<Address>(this, hexPrefix: true);

        public Address(string hexString)
        {
            if (hexString.Length == 42)
            {
                if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    hexString = hexString.Substring(2);
                }
            }
            if (hexString.Length != 40)
            {
                throw new ArgumentException("Address hex string should be 40 chars long, or 42 with a 0x prefix, was given " + hexString.Length, nameof(hexString));
            }
            
            Span<byte> bytes = EncodingUtils.HexToBytes(hexString);
            var uintView = MemoryMarshal.Cast<byte, uint>(bytes);
            P1 = uintView[0];
            P2 = uintView[1];
            P3 = uintView[2];
            P4 = uintView[3];
            P5 = uintView[4];

            // TODO: checksum validation
            //HasChecksum = HexString != hexString && hexString.ToUpperInvariant() != hexString;
        }

        public Address(byte[] bytes)
        {
            if (bytes.Length != SIZE)
            {
                throw new ArgumentException("Byte arrays for addresses should be 20 bytes long, was given " + bytes.Length, nameof(bytes));
            }
            var uintView = MemoryMarshal.Cast<byte, uint>(bytes);
            P1 = uintView[0];
            P2 = uintView[1];
            P3 = uintView[2];
            P4 = uintView[3];
            P5 = uintView[4];
        }

        public override string ToString() => GetHexString();

        public override int GetHashCode() => (P1, P2, P3, P4, P5).GetHashCode();

        public override bool Equals(object obj) => obj is Address addr ? Equals(addr) : false;

        public bool Equals(Address other)
        {
            return other.P1 == P1 && other.P2 == P2 && other.P3 == P3 && other.P4 == P4 && other.P5 == P5;
        }

        public static bool operator ==(Address a, Address b) => a.Equals(b);
        public static bool operator !=(Address a, Address b) => !a.Equals(b);

        public static implicit operator Address(string value) => HexConverter.HexToValue<Address>(value, checkEndian: false);
        public static implicit operator string(Address value) => value.GetHexString();

        /// <summary>
        /// https://github.com/ethereum/EIPs/blob/master/EIPS/eip-55.md
        /// </summary>
        static bool PassesChecksumEncoding(string addressHexStr)
        {
            // TODO: implement
            throw new NotImplementedException();
        }

    }
    


}

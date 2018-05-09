using SolcNet;
using System;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

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
            if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                hexString = hexString.Substring(2);
            }

            // special handling for default/empty address 0x0
            if (hexString == "0")
            {
                P1 = 0; P2 = 0; P3 = 0; P4 = 0; P5 = 0;
                return;
            }

            if (hexString.Length != 40)
            {
                throw new ArgumentException("Address hex string should be 40 chars long, or 42 with a 0x prefix, was given " + hexString.Length, nameof(hexString));
            }
            
            Span<byte> bytes = EncodingUtils.HexToBytes(hexString);
            if (!ValidChecksum(hexString))
            {
                throw new ArgumentException("Address does not pass mixed-case checksum validation, https://github.com/ethereum/EIPs/blob/master/EIPS/eip-55.md");
            }

            var uintView = MemoryMarshal.Cast<byte, uint>(bytes);
            P1 = uintView[0];
            P2 = uintView[1];
            P3 = uintView[2];
            P4 = uintView[3];
            P5 = uintView[4];
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

        public static implicit operator Address(string value) => new Address(value);
        public static implicit operator string(Address value) => value.GetHexString();

        /// <summary>
        /// https://github.com/ethereum/EIPs/blob/master/EIPS/eip-55.md
        /// </summary>
        public static bool ValidChecksum(string addressHexStr)
        {
            bool foundUpper = false, foundLower = false;

            foreach (var c in addressHexStr)
            {
                foundUpper |= c > 64 && c < 71;
                foundLower |= c > 96 && c < 103;
                if (foundUpper && foundLower)
                {
                    break;
                }
            }

            if (foundUpper && foundLower)
            {
                // get lowercase utf16 buffer
                Span<byte> addr = stackalloc byte[80];
                addressHexStr.AsSpan().ToLowerInvariant(MemoryMarshal.Cast<byte, char>(addr));

                // inline buffer conversion from utf16 to ascii
                for (var i = 0; i < 40; i++)
                {
                    addr[i] = addr[i * 2];
                }
                addr = addr.Slice(0, 40);

                // get hash of ascii hex
                var hashBytes = Keccak.ComputeHash(addr);

                for (var i = 0; i < 40; i++)
                {
                    char inspectChar = addressHexStr[i];

                    // skip check if character is a number
                    if (inspectChar > 64)
                    {
                        // get character casing flag
                        var c = i % 2 == 0 ? hashBytes[i / 2] >> 4 : hashBytes[i / 2] & 0x0F;
                        bool upperFlag = c >= 8;

                        // verify character is uppercase, otherwise bad checksum
                        if (upperFlag && inspectChar > 96)
                        {
                            return false;
                        }
                        // verify character is lowercase
                        else if (!upperFlag && inspectChar < 97)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

    }
    


}

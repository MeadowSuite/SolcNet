using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Ported from https://github.com/monero-project/monero/blob/master/src/crypto/keccak.c
// Optimizations taken from: https://bitbucket.org/jdluzen/sha3/src

namespace SolCodeGen
{

    public static class Keccak
    {
        const int STATE_SIZE = 200;
        const int HASH_DATA_AREA = 136;
        const int ROUNDS = 24;
        const int LANE_BITS = 8 * 8;
        const int TEMP_BUFF_SIZE = 144;

        static readonly ulong[] RoundConstants = {
            0x0000000000000001UL, 0x0000000000008082UL, 0x800000000000808aUL,
            0x8000000080008000UL, 0x000000000000808bUL, 0x0000000080000001UL,
            0x8000000080008081UL, 0x8000000000008009UL, 0x000000000000008aUL,
            0x0000000000000088UL, 0x0000000080008009UL, 0x000000008000000aUL,
            0x000000008000808bUL, 0x800000000000008bUL, 0x8000000000008089UL,
            0x8000000000008003UL, 0x8000000000008002UL, 0x8000000000000080UL,
            0x000000000000800aUL, 0x800000008000000aUL, 0x8000000080008081UL,
            0x8000000000008080UL, 0x0000000080000001UL, 0x8000000080008008UL
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong ROL(ulong a, int offset)
        {
            return (a << (offset % LANE_BITS)) ^ (a >> (LANE_BITS - (offset % LANE_BITS)));
        }

        // update the state with given number of rounds
        public static void KeccakF(Span<ulong> st, int rounds)
        {
            Debug.Assert(st.Length == 25);

            ulong Aba, Abe, Abi, Abo, Abu;
            ulong Aga, Age, Agi, Ago, Agu;
            ulong Aka, Ake, Aki, Ako, Aku;
            ulong Ama, Ame, Ami, Amo, Amu;
            ulong Asa, Ase, Asi, Aso, Asu;
            ulong BCa, BCe, BCi, BCo, BCu;
            ulong Da, De, Di, Do, Du;
            ulong Eba, Ebe, Ebi, Ebo, Ebu;
            ulong Ega, Ege, Egi, Ego, Egu;
            ulong Eka, Eke, Eki, Eko, Eku;
            ulong Ema, Eme, Emi, Emo, Emu;
            ulong Esa, Ese, Esi, Eso, Esu;

            //copyFromState(A, state)
            Aba = st[0];
            Abe = st[1];
            Abi = st[2];
            Abo = st[3];
            Abu = st[4];
            Aga = st[5];
            Age = st[6];
            Agi = st[7];
            Ago = st[8];
            Agu = st[9];
            Aka = st[10];
            Ake = st[11];
            Aki = st[12];
            Ako = st[13];
            Aku = st[14];
            Ama = st[15];
            Ame = st[16];
            Ami = st[17];
            Amo = st[18];
            Amu = st[19];
            Asa = st[20];
            Ase = st[21];
            Asi = st[22];
            Aso = st[23];
            Asu = st[24];

            for (var round = 0; round < ROUNDS; round += 2)
            {
                //    prepareTheta
                BCa = Aba ^ Aga ^ Aka ^ Ama ^ Asa;
                BCe = Abe ^ Age ^ Ake ^ Ame ^ Ase;
                BCi = Abi ^ Agi ^ Aki ^ Ami ^ Asi;
                BCo = Abo ^ Ago ^ Ako ^ Amo ^ Aso;
                BCu = Abu ^ Agu ^ Aku ^ Amu ^ Asu;

                //thetaRhoPiChiIotaPrepareTheta(round  , A, E)
                Da = BCu ^ ROL(BCe, 1);
                De = BCa ^ ROL(BCi, 1);
                Di = BCe ^ ROL(BCo, 1);
                Do = BCi ^ ROL(BCu, 1);
                Du = BCo ^ ROL(BCa, 1);

                Aba ^= Da;
                BCa = Aba;
                Age ^= De;
                BCe = ROL(Age, 44);
                Aki ^= Di;
                BCi = ROL(Aki, 43);
                Amo ^= Do;
                BCo = ROL(Amo, 21);
                Asu ^= Du;
                BCu = ROL(Asu, 14);
                Eba = BCa ^ ((~BCe) & BCi);
                Eba ^= RoundConstants[round];
                Ebe = BCe ^ ((~BCi) & BCo);
                Ebi = BCi ^ ((~BCo) & BCu);
                Ebo = BCo ^ ((~BCu) & BCa);
                Ebu = BCu ^ ((~BCa) & BCe);

                Abo ^= Do;
                BCa = ROL(Abo, 28);
                Agu ^= Du;
                BCe = ROL(Agu, 20);
                Aka ^= Da;
                BCi = ROL(Aka, 3);
                Ame ^= De;
                BCo = ROL(Ame, 45);
                Asi ^= Di;
                BCu = ROL(Asi, 61);
                Ega = BCa ^ ((~BCe) & BCi);
                Ege = BCe ^ ((~BCi) & BCo);
                Egi = BCi ^ ((~BCo) & BCu);
                Ego = BCo ^ ((~BCu) & BCa);
                Egu = BCu ^ ((~BCa) & BCe);

                Abe ^= De;
                BCa = ROL(Abe, 1);
                Agi ^= Di;
                BCe = ROL(Agi, 6);
                Ako ^= Do;
                BCi = ROL(Ako, 25);
                Amu ^= Du;
                BCo = ROL(Amu, 8);
                Asa ^= Da;
                BCu = ROL(Asa, 18);
                Eka = BCa ^ ((~BCe) & BCi);
                Eke = BCe ^ ((~BCi) & BCo);
                Eki = BCi ^ ((~BCo) & BCu);
                Eko = BCo ^ ((~BCu) & BCa);
                Eku = BCu ^ ((~BCa) & BCe);

                Abu ^= Du;
                BCa = ROL(Abu, 27);
                Aga ^= Da;
                BCe = ROL(Aga, 36);
                Ake ^= De;
                BCi = ROL(Ake, 10);
                Ami ^= Di;
                BCo = ROL(Ami, 15);
                Aso ^= Do;
                BCu = ROL(Aso, 56);
                Ema = BCa ^ ((~BCe) & BCi);
                Eme = BCe ^ ((~BCi) & BCo);
                Emi = BCi ^ ((~BCo) & BCu);
                Emo = BCo ^ ((~BCu) & BCa);
                Emu = BCu ^ ((~BCa) & BCe);

                Abi ^= Di;
                BCa = ROL(Abi, 62);
                Ago ^= Do;
                BCe = ROL(Ago, 55);
                Aku ^= Du;
                BCi = ROL(Aku, 39);
                Ama ^= Da;
                BCo = ROL(Ama, 41);
                Ase ^= De;
                BCu = ROL(Ase, 2);
                Esa = BCa ^ ((~BCe) & BCi);
                Ese = BCe ^ ((~BCi) & BCo);
                Esi = BCi ^ ((~BCo) & BCu);
                Eso = BCo ^ ((~BCu) & BCa);
                Esu = BCu ^ ((~BCa) & BCe);

                //    prepareTheta
                BCa = Eba ^ Ega ^ Eka ^ Ema ^ Esa;
                BCe = Ebe ^ Ege ^ Eke ^ Eme ^ Ese;
                BCi = Ebi ^ Egi ^ Eki ^ Emi ^ Esi;
                BCo = Ebo ^ Ego ^ Eko ^ Emo ^ Eso;
                BCu = Ebu ^ Egu ^ Eku ^ Emu ^ Esu;

                //thetaRhoPiChiIotaPrepareTheta(round+1, E, A)
                Da = BCu ^ ROL(BCe, 1);
                De = BCa ^ ROL(BCi, 1);
                Di = BCe ^ ROL(BCo, 1);
                Do = BCi ^ ROL(BCu, 1);
                Du = BCo ^ ROL(BCa, 1);

                Eba ^= Da;
                BCa = Eba;
                Ege ^= De;
                BCe = ROL(Ege, 44);
                Eki ^= Di;
                BCi = ROL(Eki, 43);
                Emo ^= Do;
                BCo = ROL(Emo, 21);
                Esu ^= Du;
                BCu = ROL(Esu, 14);
                Aba = BCa ^ ((~BCe) & BCi);
                Aba ^= RoundConstants[round + 1];
                Abe = BCe ^ ((~BCi) & BCo);
                Abi = BCi ^ ((~BCo) & BCu);
                Abo = BCo ^ ((~BCu) & BCa);
                Abu = BCu ^ ((~BCa) & BCe);

                Ebo ^= Do;
                BCa = ROL(Ebo, 28);
                Egu ^= Du;
                BCe = ROL(Egu, 20);
                Eka ^= Da;
                BCi = ROL(Eka, 3);
                Eme ^= De;
                BCo = ROL(Eme, 45);
                Esi ^= Di;
                BCu = ROL(Esi, 61);
                Aga = BCa ^ ((~BCe) & BCi);
                Age = BCe ^ ((~BCi) & BCo);
                Agi = BCi ^ ((~BCo) & BCu);
                Ago = BCo ^ ((~BCu) & BCa);
                Agu = BCu ^ ((~BCa) & BCe);

                Ebe ^= De;
                BCa = ROL(Ebe, 1);
                Egi ^= Di;
                BCe = ROL(Egi, 6);
                Eko ^= Do;
                BCi = ROL(Eko, 25);
                Emu ^= Du;
                BCo = ROL(Emu, 8);
                Esa ^= Da;
                BCu = ROL(Esa, 18);
                Aka = BCa ^ ((~BCe) & BCi);
                Ake = BCe ^ ((~BCi) & BCo);
                Aki = BCi ^ ((~BCo) & BCu);
                Ako = BCo ^ ((~BCu) & BCa);
                Aku = BCu ^ ((~BCa) & BCe);

                Ebu ^= Du;
                BCa = ROL(Ebu, 27);
                Ega ^= Da;
                BCe = ROL(Ega, 36);
                Eke ^= De;
                BCi = ROL(Eke, 10);
                Emi ^= Di;
                BCo = ROL(Emi, 15);
                Eso ^= Do;
                BCu = ROL(Eso, 56);
                Ama = BCa ^ ((~BCe) & BCi);
                Ame = BCe ^ ((~BCi) & BCo);
                Ami = BCi ^ ((~BCo) & BCu);
                Amo = BCo ^ ((~BCu) & BCa);
                Amu = BCu ^ ((~BCa) & BCe);

                Ebi ^= Di;
                BCa = ROL(Ebi, 62);
                Ego ^= Do;
                BCe = ROL(Ego, 55);
                Eku ^= Du;
                BCi = ROL(Eku, 39);
                Ema ^= Da;
                BCo = ROL(Ema, 41);
                Ese ^= De;
                BCu = ROL(Ese, 2);
                Asa = BCa ^ ((~BCe) & BCi);
                Ase = BCe ^ ((~BCi) & BCo);
                Asi = BCi ^ ((~BCo) & BCu);
                Aso = BCo ^ ((~BCu) & BCa);
                Asu = BCu ^ ((~BCa) & BCe);
            }

            //copyToState(state, A)
            st[0] = Aba;
            st[1] = Abe;
            st[2] = Abi;
            st[3] = Abo;
            st[4] = Abu;
            st[5] = Aga;
            st[6] = Age;
            st[7] = Agi;
            st[8] = Ago;
            st[9] = Agu;
            st[10] = Aka;
            st[11] = Ake;
            st[12] = Aki;
            st[13] = Ako;
            st[14] = Aku;
            st[15] = Ama;
            st[16] = Ame;
            st[17] = Ami;
            st[18] = Amo;
            st[19] = Amu;
            st[20] = Asa;
            st[21] = Ase;
            st[22] = Asi;
            st[23] = Aso;
            st[24] = Asu;
        }

        public static Span<byte> ComputeHash(Span<byte> input)
        {
            Span<byte> output = new byte[32];
            ComputeHash(input, output, output.Length);
            return output;
        }

        public static void ComputeHash(Span<byte> input, Span<byte> md)
        {
            ComputeHash(input, md, md.Length);
        }

        // compute a keccak hash (md) of given byte length from "in"
        public static void ComputeHash(Span<byte> input, Span<byte> md, int mdlen)
        {
            var inlen = input.Length;
            Span<ulong> st = new ulong[25];
            Span<byte> temp = new byte[TEMP_BUFF_SIZE];

            if (mdlen <= 0 || mdlen > 200)
            {
                throw new ArgumentException("Bad keccak use");
            }
            if (mdlen < md.Length)
            {
                throw new ArgumentException("mdlen is smaller than md");
            }

            int rsiz = STATE_SIZE == mdlen ? HASH_DATA_AREA : 200 - 2 * mdlen;
            int rsizw = rsiz / 8;

            MemoryMarshal.AsBytes(st).Slice(0, STATE_SIZE).Clear();

            int i;
            for (; inlen >= rsiz; inlen -= rsiz, input = input.Slice(rsiz))
            {
                var input64 = MemoryMarshal.Cast<byte, ulong>(input);

                for (i = 0; i < rsizw; i++)
                {
                    st[i] ^= input64[i];
                }
                KeccakF(st, ROUNDS);
            }

            // last block and padding
            if (inlen >= TEMP_BUFF_SIZE || inlen > rsiz || rsiz - inlen + inlen + 1 >= TEMP_BUFF_SIZE || rsiz == 0 || rsiz - 1 >= TEMP_BUFF_SIZE || rsizw * 8 > TEMP_BUFF_SIZE)
            {
                throw new ArgumentException("Bad keccak use");
            }

            input.Slice(0, inlen).CopyTo(temp);
            temp[inlen++] = 1;
            temp[rsiz - 1] |= 0x80;

            var temp64 = MemoryMarshal.Cast<byte, ulong>(temp);

            for (i = 0; i < rsizw; i++)
            {
                st[i] ^= temp64[i];
            }

            KeccakF(st, ROUNDS);

            MemoryMarshal.AsBytes(st).Slice(0, mdlen).CopyTo(md);
        }

        public static void Keccak1600(Span<byte> input, Span<byte> md)
        {
            ComputeHash(input, md, STATE_SIZE);
        }

    }
}
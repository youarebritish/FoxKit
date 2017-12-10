#region License
// Copyright (c) 2011 Google, Inc.
// Copyright (c) 2014 Atvaark
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// CityHash, by Geoff Pike and Jyrki Alakuijala
// CityHash C# Port, by Atvaark
//
// This file provides CityHash64() and related functions.
//
// It's probably possible to create even faster hash functions by
// writing a program that systematically explores some of the space of
// possible hash functions, by using SIMD instructions, or by
// compromising on hash quality.
#endregion
using System;
using System.Text;
using uint8 = System.Byte;
using uint32 = System.UInt32;
using uint64 = System.UInt64;
using uint128 = CityHash.UInt128;

namespace CityHash
{
    /// <summary>
    ///     Managed CityHash 1.0.3 implementation.
    /// </summary>
    public static class CityHash
    {
        // Some primes between 2^63 and 2^64 for various uses.
        private const uint64 K0 = 0xc3a5c85c97cb3127;
        private const uint64 K1 = 0xb492b66fbe98f273;
        private const uint64 K2 = 0x9ae16a3b2f90404f;
        private const uint64 K3 = 0xc949d7c7509e6557;
        // Magic numbers for 32-bit hashing. Copied from Murmur3.
        private const uint32 C1 = 0xcc9e2d51;
        private const uint32 C2 = 0x1b873593;
        public static bool BigEndian { get; set; }
        // Hash 128 input bits down to 64 bits of output.
        // This is intended to be a reasonably good hash function.
        private static uint64 Hash128To64(uint128 x)
        {
            // Murmur-inspired hashing.
            const ulong kMul = 0x9ddfea08eb382d69;
            ulong a = (Uint128Low64(x) ^ Uint128High64(x))*kMul;
            a ^= (a >> 47);
            ulong b = (Uint128High64(x) ^ a)*kMul;
            b ^= (b >> 47);
            b *= kMul;
            return b;
        }

        private static uint64 RotateByAtLeast1(uint64 val, int shift)
        {
            return (val >> shift) | (val << (64 - shift));
        }

        private static uint64 Uint128Low64(uint128 x)
        {
            return x.Low;
        }

        private static uint64 Uint128High64(uint128 x)
        {
            return x.High;
        }

        private static uint128 make_pair(uint64 low, uint64 high)
        {
            return new uint128(low, high);
        }

        private static uint64 UNALIGNED_LOAD64(byte[] p, int index)
        {
            return BitConverter.ToUInt64(p, index);
        }

        private static uint32 UNALIGNED_LOAD32(byte[] p, int index)
        {
            return BitConverter.ToUInt32(p, index);
        }

        private static uint bswap_32(uint x)
        {
            byte[] bytes = BitConverter.GetBytes(x);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        private static ulong bswap_64(ulong x)
        {
            byte[] bytes = BitConverter.GetBytes(x);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        private static uint uint32_in_expected_order(uint x)
        {
            return BigEndian ? bswap_32(x) : x;
        }

        private static ulong uint64_in_expected_order(ulong x)
        {
            return BigEndian ? bswap_64(x) : x;
        }

        private static uint64 Fetch64(byte[] p, int index)
        {
            return uint64_in_expected_order(UNALIGNED_LOAD64(p, index));
        }

        private static uint64 Fetch64(byte[] p, uint index)
        {
            return Fetch64(p, (int) index);
        }

        private static uint32 Fetch32(byte[] p, int index)
        {
            return uint32_in_expected_order(UNALIGNED_LOAD32(p, index));
        }

        private static uint32 Fetch32(byte[] p, uint index)
        {
            return Fetch32(p, (int) index);
        }

        // A 32-bit to 32-bit integer hash copied from Murmur3.
        private static uint32 Fmix(uint32 h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }

        private static uint32 Rotate32(uint32 val, int shift)
        {
            // Avoid shifting by 32: doing so yields an undefined result.
            return shift == 0 ? val : ((val >> shift) | (val << (32 - shift)));
        }

        private static void Permute3<T>(ref T a, ref T b, ref T c)
        {
            Swap(ref a, ref b);
            Swap(ref a, ref c);
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        private static uint32 Mur(uint32 a, uint32 h)
        {
            // Helper from Murmur3 for combining two 32-bit values.
            a *= C1;
            a = Rotate32(a, 17);
            a *= C2;
            h ^= a;
            h = Rotate32(h, 19);
            return h*5 + 0xe6546b64;
        }

        private static uint32 Hash32Len13To24(byte[] s)
        {
            uint len = (uint) s.Length;
            uint32 a = Fetch32(s, (len >> 1) - 4);
            uint32 b = Fetch32(s, +4);
            uint32 c = Fetch32(s, +len - 8);
            uint32 d = Fetch32(s, +(len >> 1));
            uint32 e = Fetch32(s, 0);
            uint32 f = Fetch32(s, +len - 4);
            uint32 h = len;
            return Fmix(Mur(f, Mur(e, Mur(d, Mur(c, Mur(b, Mur(a, h)))))));
        }

        private static uint32 Hash32Len0To4(byte[] s)
        {
            uint len = (uint) s.Length;
            uint32 b = 0;
            uint32 c = 9;
            for (int i = 0; i < len; i++)
            {
                b = b*C1 + (uint) ((sbyte) s[i]);
                c ^= b;
            }
            return Fmix(Mur(b, Mur(len, c)));
        }

        private static uint32 Hash32Len5To12(byte[] s)
        {
            uint len = (uint) s.Length;
            uint32 a = len, b = len*5, c = 9, d = b;
            a += Fetch32(s, 0);
            b += Fetch32(s, len - 4);
            c += Fetch32(s, ((len >> 1) & 4));
            return Fmix(Mur(c, Mur(b, Mur(a, d))));
        }

        // Hash function for a string. Most useful in 32-bit binaries.
        public static uint32 CityHash32(string s)
        {
            return CityHash32(s, Encoding.Default);
        }

        // Hash function for a string. Most useful in 32-bit binaries.
        public static uint32 CityHash32(string s, Encoding encoding)
        {
            return CityHash32(encoding.GetBytes(s));
        }

        public static uint32 CityHash32(byte[] s)
        {
            uint len = (uint) s.Length;
            if (len <= 24)
            {
                return len <= 12
                    ? (len <= 4 ? Hash32Len0To4(s) : Hash32Len5To12(s))
                    : Hash32Len13To24(s);
            }
            // len > 24
            uint32 h = len, g = C1*len, f = g;
            {
                uint32 a0 = Rotate32(Fetch32(s, len - 4)*C1, 17)*C2;
                uint32 a1 = Rotate32(Fetch32(s, len - 8)*C1, 17)*C2;
                uint32 a2 = Rotate32(Fetch32(s, len - 16)*C1, 17)*C2;
                uint32 a3 = Rotate32(Fetch32(s, len - 12)*C1, 17)*C2;
                uint32 a4 = Rotate32(Fetch32(s, len - 20)*C1, 17)*C2;
                h ^= a0;
                h = Rotate32(h, 19);
                h = h*5 + 0xe6546b64;
                h ^= a2;
                h = Rotate32(h, 19);
                h = h*5 + 0xe6546b64;
                g ^= a1;
                g = Rotate32(g, 19);
                g = g*5 + 0xe6546b64;
                g ^= a3;
                g = Rotate32(g, 19);
                g = g*5 + 0xe6546b64;
                f += a4;
                f = Rotate32(f, 19);
                f = f*5 + 0xe6546b64;
            }
            uint iters = (len - 1)/20;
            uint offset = 0;
            do
            {
                uint32 a0 = Rotate32(Fetch32(s, offset)*C1, 17)*C2;
                uint32 a1 = Fetch32(s, offset + 4);
                uint32 a2 = Rotate32(Fetch32(s, offset + 8)*C1, 17)*C2;
                uint32 a3 = Rotate32(Fetch32(s, offset + 12)*C1, 17)*C2;
                uint32 a4 = Fetch32(s, offset + 16);
                h ^= a0;
                h = Rotate32(h, 18);
                h = h*5 + 0xe6546b64;
                f += a1;
                f = Rotate32(f, 19);
                f = f*C1;
                g += a2;
                g = Rotate32(g, 18);
                g = g*5 + 0xe6546b64;
                h ^= a3 + a1;
                h = Rotate32(h, 19);
                h = h*5 + 0xe6546b64;
                g ^= a4;
                g = bswap_32(g)*5;
                h += a4*5;
                h = bswap_32(h);
                f += a0;
                Permute3(ref f, ref h, ref g);
                offset += 20;
            } while (--iters != 0);
            g = Rotate32(g, 11)*C1;
            g = Rotate32(g, 17)*C1;
            f = Rotate32(f, 11)*C1;
            f = Rotate32(f, 17)*C1;
            h = Rotate32(h + g, 19);
            h = h*5 + 0xe6546b64;
            h = Rotate32(h, 17)*C1;
            h = Rotate32(h + f, 19);
            h = h*5 + 0xe6546b64;
            h = Rotate32(h, 17)*C1;
            return h;
        }

        // Bitwise right rotate. Normally this will compile to a single
        // instruction, especially if the shift is a manifest constant.
        private static uint64 Rotate(uint64 val, int shift)
        {
            // Avoid shifting by 64: doing so yields an undefined result.
            return shift == 0 ? val : ((val >> shift) | (val << (64 - shift)));
        }

        private static uint64 ShiftMix(uint64 val)
        {
            return val ^ (val >> 47);
        }

        private static uint64 HashLen16(uint64 u, uint64 v)
        {
            return Hash128To64(new uint128(u, v));
        }

        private static uint64 HashLen16(uint64 u, uint64 v, uint64 mul)
        {
            // Murmur-inspired hashing.
            uint64 a = (u ^ v)*mul;
            a ^= (a >> 47);
            uint64 b = (v ^ a)*mul;
            b ^= (b >> 47);
            b *= mul;
            return b;
        }

        private static uint64 HashLen0To16(byte[] s, int offset)
        {
            int len = s.Length - offset;
            if (len > 8)
            {
                uint64 a = Fetch64(s, offset);
                uint64 b = Fetch64(s, offset + len - 8);
                return HashLen16(a, RotateByAtLeast1(b + (ulong) len, len)) ^ b;
            }
            if (len >= 4)
            {
                uint64 a = Fetch32(s, offset);
                return HashLen16((uint) len + (a << 3), Fetch32(s, offset + len - 4));
            }
            if (len > 0)
            {
                uint8 a = s[offset];
                uint8 b = s[offset + (len >> 1)];
                uint8 c = s[offset + (len - 1)];
                uint32 y = a + ((uint32) b << 8);
                uint32 z = (uint) len + ((uint32) c << 2);
                return ShiftMix(y*K2 ^ z*K3)*K2;
            }
            return K2;
        }

        // This probably works well for 16-byte strings as well, but it may be overkill
        // in that case.
        private static uint64 HashLen17To32(byte[] s)
        {
            uint len = (uint) s.Length;
            uint64 a = Fetch64(s, 0)*K1;
            uint64 b = Fetch64(s, 8);
            uint64 c = Fetch64(s, len - 8)*K2;
            uint64 d = Fetch64(s, len - 16)*K0;
            return HashLen16(Rotate(a - b, 43) + Rotate(c, 30) + d,
                a + Rotate(b ^ K3, 20) - c + len);
        }

        // Return a 16-byte hash for 48 bytes. Quick and dirty.
        // Callers do best to use "random-looking" values for a and b.
        private static uint128 WeakHashLen32WithSeeds(
            uint64 w, uint64 x, uint64 y, uint64 z, uint64 a, uint64 b)
        {
            a += w;
            b = Rotate(b + a + z, 21);
            uint64 c = a;
            a += x;
            a += y;
            b += Rotate(a, 44);
            return make_pair(a + z, b + c);
        }

        // Return a 16-byte hash for s[0] ... s[31], a, and b. Quick and dirty.
        private static uint128 WeakHashLen32WithSeeds(byte[] s, int offset, uint64 a, uint64 b)
        {
            return WeakHashLen32WithSeeds(Fetch64(s, offset),
                Fetch64(s, offset + 8),
                Fetch64(s, offset + 16),
                Fetch64(s, offset + 24),
                a,
                b);
        }

        // Return an 8-byte hash for 33 to 64 bytes.
        private static uint64 HashLen33To64(byte[] s)
        {
            uint len = (uint) s.Length;
            uint64 z = Fetch64(s, 24);
            uint64 a = Fetch64(s, 0) + (len + Fetch64(s, len - 16))*K0;
            uint64 b = Rotate(a + z, 52);
            uint64 c = Rotate(a, 37);
            a += Fetch64(s, 8);
            c += Rotate(a, 7);
            a += Fetch64(s, 16);
            uint64 vf = a + z;
            uint64 vs = b + Rotate(a, 31) + c;
            a = Fetch64(s, 16) + Fetch64(s, len - 32);
            z = Fetch64(s, len - 8);
            b = Rotate(a + z, 52);
            c = Rotate(a, 37);
            a += Fetch64(s, len - 24);
            c += Rotate(a, 7);
            a += Fetch64(s, len - 16);
            uint64 wf = a + z;
            uint64 ws = b + Rotate(a, 31) + c;
            uint64 r = ShiftMix((vf + ws)*K2 + (wf + vs)*K0);
            return ShiftMix(r*K0 + vs)*K2;
        }

        public static uint64 CityHash64(string s)
        {
            return CityHash64(s, Encoding.Default);
        }

        public static uint64 CityHash64(string s, Encoding encoding)
        {
            return CityHash64(encoding.GetBytes(s));
        }

        public static uint64 CityHash64(byte[] s)
        {
            int len = s.Length;
            if (len <= 32)
            {
                if (len <= 16)
                {
                    return HashLen0To16(s, 0);
                }
                return HashLen17To32(s);
            }
            if (len <= 64)
            {
                return HashLen33To64(s);
            }


            // For strings over 64 bytes we hash the end first, and then as we
            // loop we keep 56 bytes of state: v, w, x, y, and z.
            uint64 x = Fetch64(s, len - 40);
            uint64 y = Fetch64(s, len - 16) + Fetch64(s, len - 56);
            uint64 z = HashLen16(Fetch64(s, len - 48) + (ulong) len, Fetch64(s, len - 24));
            uint128 v = WeakHashLen32WithSeeds(s, len - 64, (ulong) len, z);
            uint128 w = WeakHashLen32WithSeeds(s, len - 32, y + K1, x);
            x = x*K1 + Fetch64(s, 0);

            // Decrease len to the nearest multiple of 64, and operate on 64-byte chunks.
            len = (s.Length - 1) & ~63;
            int offset = 0;
            do
            {
                x = Rotate(x + y + v.Low + Fetch64(s, offset + 8), 37)*K1;
                y = Rotate(y + v.High + Fetch64(s, offset + 48), 42)*K1;
                x ^= w.High;
                y += v.Low + Fetch64(s, offset + 40);
                z = Rotate(z + w.Low, 33)*K1;
                v = WeakHashLen32WithSeeds(s, offset, v.High*K1, x + w.Low);
                w = WeakHashLen32WithSeeds(s, offset + 32, z + w.High, y + Fetch64(s, offset + 16));
                Swap(ref z, ref x);
                offset += 64;
                len -= 64;
            } while (len != 0);
            return HashLen16(HashLen16(v.Low, w.Low) + ShiftMix(y)*K1 + z,
                HashLen16(v.High, w.High) + x);
        }

        // Hash function for a string. For convenience, a 64-bit seed is also
        // hashed into the result.
        public static uint64 CityHash64WithSeed(string s, uint64 seed)
        {
            return CityHash64WithSeed(s, seed, Encoding.Default);
        }

        // Hash function for a string. For convenience, a 64-bit seed is also
        // hashed into the result.
        public static uint64 CityHash64WithSeed(string s, uint64 seed, Encoding encoding)
        {
            return CityHash64WithSeed(encoding.GetBytes(s), seed);
        }

        public static uint64 CityHash64WithSeed(byte[] s, uint64 seed)
        {
            return CityHash64WithSeeds(s, K2, seed);
        }

        // Hash function for a byte array. For convenience, two seeds are also
        // hashed into the result.
        public static uint64 CityHash64WithSeeds(string s, uint64 seed0, uint64 seed1)
        {
            return CityHash64WithSeeds(s, seed0, seed1, Encoding.Default);
        }

        // Hash function for a string. For convenience, two seeds are also
        // hashed into the result.
        public static uint64 CityHash64WithSeeds(string s, uint64 seed0, uint64 seed1, Encoding encoding)
        {
            return CityHash64WithSeeds(encoding.GetBytes(s), seed0, seed1);
        }

        public static uint64 CityHash64WithSeeds(byte[] s, uint64 seed0, uint64 seed1)
        {
            return HashLen16(CityHash64(s) - seed0, seed1);
        }

        // A subroutine for CityHash128(). Returns a decent 128-bit hash for strings
        // of any length representable in signed long. Based on City and Murmur.
        private static uint128 CityMurmur(byte[] s, int offset, uint128 seed)
        {
            int len = s.Length - offset;
            uint64 a = Uint128Low64(seed);
            uint64 b = Uint128High64(seed);
            uint64 c;
            uint64 d;
            int l = len - 16;
            if (l <= 0)
            {
                // len <= 16
                a = ShiftMix(a*K1)*K1;
                c = b*K1 + HashLen0To16(s, offset);
                d = ShiftMix(a + (len >= 8 ? Fetch64(s, offset) : c));
            }
            else
            {
                // len > 16
                c = HashLen16(Fetch64(s, offset + len - 8) + K1, a);
                d = HashLen16(b + (ulong) len, c + Fetch64(s, offset + len - 16));
                a += d;
                int loopOffset = offset;
                do
                {
                    a ^= ShiftMix(Fetch64(s, loopOffset)*K1)*K1;
                    a *= K1;
                    b ^= a;
                    c ^= ShiftMix(Fetch64(s, loopOffset + 8)*K1)*K1;
                    c *= K1;
                    d ^= c;
                    loopOffset += 16;
                    l -= 16;
                } while (l > 0);
            }
            a = HashLen16(a, c);
            b = HashLen16(d, b);
            return new uint128(a ^ b, HashLen16(b, a));
        }

        // Hash function for a string. For convenience, a 128-bit seed is also
        // hashed into the result.
        public static uint128 CityHash128WithSeed(string s, uint128 seed)
        {
            return CityHash128WithSeed(s, seed, Encoding.Default);
        }

        // Hash function for a string. For convenience, a 128-bit seed is also
        // hashed into the result.
        public static uint128 CityHash128WithSeed(string s, uint128 seed, Encoding encoding)
        {
            return CityHash128WithSeed(encoding.GetBytes(s), 0, seed);
        }

        private static uint128 CityHash128WithSeed(byte[] s, int offset, uint128 seed)
        {
            uint len = (uint) (s.Length - offset);
            if (len < 128)
            {
                return CityMurmur(s, offset, seed);
            }
            // We expect len >= 128 to be the common case. Keep 56 bytes of state:
            // v, w, x, y, and z.
            uint128 v = new uint128();
            uint128 w = new uint128();
            uint64 x = Uint128Low64(seed);
            uint64 y = Uint128High64(seed);
            uint64 z = len*K1;
            v.Low = Rotate(y ^ K1, 49)*K1 + Fetch64(s, offset);
            v.High = Rotate(v.Low, 42)*K1 + Fetch64(s, offset + 8);
            w.Low = Rotate(y + z, 35)*K1 + x;
            w.High = Rotate(x + Fetch64(s, offset + 88), 53)*K1;
            // This is the same inner loop as CityHash64(), manually unrolled.
            int loopOffset = offset;
            do
            {
                x = Rotate(x + y + v.Low + Fetch64(s, loopOffset + 16), 37)*K1;
                y = Rotate(y + v.High + Fetch64(s, loopOffset + 48), 42)*K1;
                x ^= w.High;
                y ^= v.Low;
                z = Rotate(z ^ w.Low, 33);
                v = WeakHashLen32WithSeeds(s, loopOffset, v.High*K1, x + w.Low);
                w = WeakHashLen32WithSeeds(s, loopOffset + 32, z + w.High, y);
                Swap(ref z, ref x);
                loopOffset += 64;
                x = Rotate(x + y + v.Low + Fetch64(s, loopOffset + 16), 37)*K1;
                y = Rotate(y + v.High + Fetch64(s, loopOffset + 48), 42)*K1;
                x ^= w.High;
                y ^= v.Low;
                z = Rotate(z ^ w.Low, 33);
                v = WeakHashLen32WithSeeds(s, loopOffset, v.High*K1, x + w.Low);
                w = WeakHashLen32WithSeeds(s, loopOffset + 32, z + w.High, y);
                Swap(ref z, ref x);
                loopOffset += 64;
                len -= 128;
            } while (len >= 128);

            y += Rotate(w.Low, 37)*K0 + z;
            x += Rotate(v.Low + z, 49)*K0;

            // If 0 < len < 128, hash up to 4 chunks of 32 bytes each from the end of s.
            for (int tailDone = 0; tailDone < len;)
            {
                tailDone += 32;
                y = Rotate(y - x, 42)*K0 + v.High;
                w.Low += Fetch64(s, (int) len - tailDone + 16);
                x = Rotate(x, 49)*K0 + w.Low;
                w.Low += v.Low;
                v = WeakHashLen32WithSeeds(s, (int) len - tailDone, v.Low, v.High);
            }
            // At this point our 48 bytes of state should contain more than
            // enough information for a strong 128-bit hash.  We use two
            // different 48-byte-to-8-byte hashes to get a 16-byte final result.
            x = HashLen16(x, v.Low);
            y = HashLen16(y, w.Low);
            return new uint128(HashLen16(x + v.High, w.High) + y,
                HashLen16(x + w.High, y + v.High));
        }

        // Hash function for a string.
        public static uint128 CityHash128(string s)
        {
            return CityHash128(s, Encoding.Default);
        }

        // Hash function for a string.
        public static uint128 CityHash128(string s, Encoding encoding)
        {
            return CityHash128(encoding.GetBytes(s));
        }

        private static uint128 CityHash128(byte[] s)
        {
            int len = s.Length;
            return len >= 16
                ? CityHash128WithSeed(s, 16,
                    new uint128(Fetch64(s, 0), Fetch64(s, 8) + K0))
                : CityHash128WithSeed(s, 0, new uint128(K0, K1));
        }

        private static uint64 _mm_crc32_u64(uint64 crc, uint64 v)
        {
            throw new NotImplementedException();
            //const int polynomial = 0x1EDC6F41;
            //return crc + crc32c(v);
        }

        // Requires len >= 240.
        public static void CityHashCrc256Long(string s, uint32 seed, out UInt256 result)
        {
            CityHashCrc256Long(s, seed, Encoding.Default, out result);
        }

        // Requires len >= 240.
        public static void CityHashCrc256Long(string s, uint32 seed, Encoding encoding, out UInt256 result)
        {
            CityHashCrc256Long(encoding.GetBytes(s), seed, out result);
        }

        private static void CityHashCrc256Long(byte[] s, uint32 seed, out UInt256 result)
        {
            result = new UInt256();
            uint len = (uint) s.Length;
            uint64 a = Fetch64(s, 56) + K0;
            uint64 b = Fetch64(s, 96) + K0;
            uint64 c = result.Low.Low = HashLen16(b, len);
            uint64 d = result.Low.High = Fetch64(s, 120)*K0 + len;
            uint64 e = Fetch64(s, 184) + seed;
            uint64 f = seed;
            uint64 g = 0;
            uint64 h = 0;
            uint64 i = 0;
            uint64 j = 0;
            uint64 t = c + d;

            // 240 bytes of input per iter.
            uint iters = len/240;
            len -= iters*240;
            int offset = 0;
            Action<uint64, int> chunk =
                (multiplier, z) =>
                {
                    uint64 oldA = a;

                    a = Rotate(b, 41 ^ z)*multiplier + Fetch64(s, offset);
                    b = Rotate(c, 27 ^ z)*multiplier + Fetch64(s, offset + 8);
                    c = Rotate(d, 41 ^ z)*multiplier + Fetch64(s, offset + 16);
                    d = Rotate(e, 33 ^ z)*multiplier + Fetch64(s, offset + 24);
                    e = Rotate(t, 25 ^ z)*multiplier + Fetch64(s, offset + 32);
                    t = oldA;

                    f = _mm_crc32_u64(f, a);
                    g = _mm_crc32_u64(g, b);
                    h = _mm_crc32_u64(h, c);
                    i = _mm_crc32_u64(i, d);
                    j = _mm_crc32_u64(j, e);
                    offset += 40;
                };

            do
            {
                chunk(1, 1);
                chunk(K0, 0);
                chunk(1, 1);
                chunk(K0, 0);
                chunk(1, 1);
                chunk(K0, 0);
            } while (--iters > 0);

            while (len >= 40)
            {
                chunk(K0, 0);
                len -= 40;
            }
            if (len > 0)
            {
                offset += (int) len - 40;
                chunk(K0, 0);
            }
            j += i << 32;
            a = HashLen16(a, j);
            h += g << 32;
            b += h;
            c = HashLen16(c, f) + i;
            d = HashLen16(d, e + result.Low.Low);
            j += e;
            i += HashLen16(h, t);
            e = HashLen16(a, d) + j;
            f = HashLen16(b, c) + a;
            g = HashLen16(j, i) + c;
            result.Low.Low = e + f + g + h;
            a = ShiftMix((a + g)*K0)*K0 + b;
            result.Low.High += a + result.Low.Low;
            a = ShiftMix(a*K0)*K0 + c;
            result.High.Low = a + result.Low.High;
            a = ShiftMix((a + e)*K0)*K0;
            result.High.High = a + result.High.Low;
        }

        // Requires len < 240.
        public static void CityHashCrc256Short(string s, out UInt256 result)
        {
            CityHashCrc256Short(s, Encoding.Default, out result);
        }

        // Requires len < 240.
        public static void CityHashCrc256Short(string s, Encoding encoding, out UInt256 result)
        {
            CityHashCrc256Short(encoding.GetBytes(s), out result);
        }

        private static void CityHashCrc256Short(byte[] s, out UInt256 result)
        {
            int len = s.Length;
            byte[] buf = new byte[240];
            Array.Copy(s, 0, buf, 0, s.Length);
            CityHashCrc256Long(buf, ~(uint32) (len), out result);
        }

        public static void CityHashCrc256(string s, out UInt256 result)
        {
            CityHashCrc256(s, Encoding.Default, out result);
        }

        public static void CityHashCrc256(string s, Encoding encoding, out UInt256 result)
        {
            CityHashCrc256(encoding.GetBytes(s), out result);
        }

        private static void CityHashCrc256(byte[] s, out UInt256 result)
        {
            int len = s.Length;
            if (len >= 240)
            {
                CityHashCrc256Long(s, 0, out result);
            }
            else
            {
                CityHashCrc256Short(s, out result);
            }
        }

        public static uint128 CityHashCrc128WithSeed(string s, uint128 seed)
        {
            return CityHashCrc128WithSeed(s, seed, Encoding.Default);
        }

        public static uint128 CityHashCrc128WithSeed(string s, uint128 seed, Encoding encoding)
        {
            return CityHashCrc128WithSeed(encoding.GetBytes(s), seed);
        }

        private static uint128 CityHashCrc128WithSeed(byte[] s, uint128 seed)
        {
            int len = s.Length;
            if (len <= 900)
            {
                return CityHash128WithSeed(s, len, seed);
            }
            UInt256 result;
            CityHashCrc256(s, out result);
            uint64 u = Uint128High64(seed) + result.Low.Low;
            uint64 v = Uint128Low64(seed) + result.Low.High;
            return new uint128(HashLen16(u, v + result.High.Low),
                HashLen16(Rotate(v, 32), u*K0 + result.High.High));
        }

        public static uint128 CityHashCrc128(string s)
        {
            return CityHashCrc128(s, Encoding.Default);
        }

        public static uint128 CityHashCrc128(string s, Encoding encoding)
        {
            return CityHashCrc128(encoding.GetBytes(s));
        }

        private static uint128 CityHashCrc128(byte[] s)
        {
            int len = s.Length;
            if (len <= 900)
            {
                return CityHash128(s);
            }
            UInt256 result;
            CityHashCrc256(s, out result);
            return result.High;
        }
    }
}

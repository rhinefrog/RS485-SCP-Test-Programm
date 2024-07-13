using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SerialHex
{
    public class CRC32
    {
        private readonly uint[] ChecksumTable;
        private readonly uint Polynomial = 0xEDB88320;

        public CRC32()
        {
            ChecksumTable = new uint[0x100];

            for (uint index = 0; index < 0x100; ++index)
            {
                uint item = index;
                for (int bit = 0; bit < 8; ++bit)
                    item = ((item & 1) != 0) ? (Polynomial ^ (item >> 1)) : (item >> 1);
                ChecksumTable[index] = item;
            }
        }

        public byte[] ComputeHash(Stream stream)
        {
            uint result = 0xFFFFFFFF;

            int current;
            while ((current = stream.ReadByte()) != -1)
                result = ChecksumTable[(result & 0xFF) ^ (byte)current] ^ (result >> 8);

            byte[] hash = BitConverter.GetBytes(~result);
            Array.Reverse(hash);
            return hash;
        }

        public byte[] ComputeHash(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
                return ComputeHash(stream);
        }
    }

    public class ProtocolSDC
    {

        public string SDC_LongStatus_1_Read(uint cpuid)
        {
            int a;
            ushort checksumCalc;
            byte commandID = 0x01; // long status 1  
            byte[] dat = new byte[4];
            dat[0] = (byte)cpuid; // cpuid
            dat[1] =  commandID;  // command ID

            checksumCalc = 0xFFFF;                              // checksum Berechnung
            for (a = 0; a <= 2; a++)
            {
                checksumCalc = (ushort)crc16(dat[a], checksumCalc);
                dat[2] = (byte)(checksumCalc & 0xFF);          // LB checksum
                dat[3] = (byte)(checksumCalc >> 8 & 0xFF);     // HB checksum
            }
//            crc 16 bit checksum is the 2's complement of the sum from byte 0 to byte 2.

            string tes = ByteArrayToString(dat);
            return tes;
        }


        ushort crc16(ushort b, ushort c)
        {
            ushort i;

            c ^= b;
            for (i = 0; i < 8; i++)
            {
//                if (c & 0x01)
//                {
                    c >>= 1;
                    c ^= 0xA001;
//                }
//                else c >>= 1;
            }
            return (c);
        }

        public string SDC_LongStatus_1_Buffer(byte[] input)
        {
         //   int a;
         //   ushort checksumCalc;
            byte[] dat = new byte[140];
            dat = input;

            string tes = ByteArrayToString(dat);
            return tes;
        }

       
     //---tools-----------------------------------------------------------------------------
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}

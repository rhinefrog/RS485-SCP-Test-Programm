﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SCP
{
    public static class CRC16
    {
        public static UInt16[] cp_Crc16_CCITT =
        {
            0x0000,0x1021, 0x2042,0x3063,0x4084,0x50A5,0x60C6,0x70E7,
            0x8108,0x9129,0xA14A,0xB16B,0xC18C,0xD1AD,0xE1CE,0xF1EF,
            0x1231,0x0210,0x3273,0x2252,0x52B5,0x4294,0x72F7,0x62D6,
            0x9339,0x8318,0xB37B,0xA35A,0xD3BD,0xC39C,0xF3FF,0xE3DE,
            0x2462,0x3443,0x0420,0x1401,0x64E6,0x74C7,0x44A4,0x5485,
            0xA56A,0xB54B,0x8528,0x9509,0xE5EE,0xF5CF,0xC5AC,0xD58D,
            0x3653,0x2672,0x1611,0x0630,0x76D7,0x66F6,0x5695,0x46B4,
            0xB75B,0xA77A,0x9719,0x8738,0xF7DF,0xE7FE,0xD79D,0xC7BC,
            0x48C4,0x58E5,0x6886,0x78A7,0x0840,0x1861,0x2802,0x3823,
            0xC9CC,0xD9ED,0xE98E,0xF9AF,0x8948,0x9969,0xA90A,0xB92B,
            0x5AF5,0x4AD4,0x7AB7,0x6A96,0x1A71,0x0A50,0x3A33,0x2A12,
            0xDBFD,0xCBDC,0xFBBF,0xEB9E,0x9B79,0x8B58,0xBB3B,0xAB1A,
            0x6CA6,0x7C87,0x4CE4,0x5CC5,0x2C22,0x3C03,0x0C60,0x1C41,
            0xEDAE,0xFD8F,0xCDEC,0xDDCD,0xAD2A,0xBD0B,0x8D68,0x9D49,
            0x7E97,0x6EB6,0x5ED5,0x4EF4,0x3E13,0x2E32,0x1E51,0x0E70,
            0xFF9F,0xEFBE,0xDFDD,0xCFFC,0xBF1B,0xAF3A,0x9F59,0x8F78,
            0x9188,0x81A9,0xB1CA,0xA1EB,0xD10C,0xC12D,0xF14E,0xE16F,
            0x1080,0x00A1,0x30C2,0x20E3,0x5004,0x4025,0x7046,0x6067,
            0x83B9,0x9398,0xA3FB,0xB3DA,0xC33D,0xD31C,0xE37F,0xF35E,
            0x02B1,0x1290,0x22F3,0x32D2,0x4235,0x5214,0x6277,0x7256,
            0xB5EA,0xA5CB,0x95A8,0x8589,0xF56E,0xE54F,0xD52C,0xC50D,
            0x34E2,0x24C3,0x14A0,0x0481,0x7466,0x6447,0x5424,0x4405,
            0xA7DB,0xB7FA,0x8799,0x97B8,0xE75F,0xF77E,0xC71D,0xD73C,
            0x26D3,0x36F2,0x0691,0x16B0,0x6657,0x7676,0x4615,0x5634,
            0xD94C,0xC96D,0xF90E,0xE92F,0x99C8,0x89E9,0xB98A,0xA9AB,
            0x5844,0x4865,0x7806,0x6827,0x18C0,0x08E1,0x3882,0x28A3,
            0xCB7D,0xDB5C,0xEB3F,0xFB1E,0x8BF9,0x9BD8,0xABBB,0xBB9A,
            0x4A75,0x5A54,0x6A37,0x7A16,0x0AF1,0x1AD0,0x2AB3,0x3A92,
            0xFD2E,0xED0F,0xDD6C,0xCD4D,0xBDAA,0xAD8B,0x9DE8,0x8DC9,
            0x7C26,0x6C07,0x5C64,0x4C45,0x3CA2,0x2C83,0x1CE0,0x0CC1,
            0xEF1F,0xFF3E,0xCF5D,0xDF7C,0xAF9B,0xBFBA,0x8FD9,0x9FF8,
            0x6E17,0x7E36,0x4E55,0x5E74,0x2E93,0x3EB2,0x0ED1,0x1EF0,
            };

        const ushort polynomial = 0xA001;
        static readonly ushort[] table = new ushort[256];

        public static ushort ComputeChecksum(byte[] bytes)
        {
            ushort crc = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }
            return crc;
        }
        public static ushort CRC16CCITT(byte[] bytes, ushort initialValue)
        {
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort temp, a;
            ushort crc = initialValue;
            for (int i = 0; i < table.Length; ++i)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                        temp = (ushort)((temp << 1) ^ poly);
                    else
                        temp <<= 1;
                    a <<= 1;
                }
                table[i] = temp;
            }
            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }
            return crc;
        }
        public static ushort CRC16CCITT(byte v, byte[] bytes)
        {
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort initialValue = 0xffff;
            ushort temp, a;
            ushort crc = initialValue;
            for (int i = 0; i < table.Length; ++i)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                        temp = (ushort)((temp << 1) ^ poly);
                    else
                        temp <<= 1;
                    a <<= 1;
                }
                table[i] = temp;
            }
            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }
            return crc;
        }
        static CRC16()
        {
            ushort value;
            ushort temp;
            for (ushort i = 0; i < table.Length; ++i)
            {
                value = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                    {
                        value = (ushort)((value >> 1) ^ polynomial);
                    }
                    else
                    {
                        value >>= 1;
                    }
                    temp >>= 1;
                }
                table[i] = value;
            }
        }
    }

    class ProtocolSCP
    {

        //-------------------------------------------------------------------------------------
        public string ReadDP(uint cpuid, uint startdp)
        {
            int a;
            ushort checksumCalc;
            uint dpanzahl = 1;                                  //nur 1 Datenpunkt zum abfragen erlauben! beta-version
            byte[] dat = new byte[13];
            dat[0] = 69;                                        // 45h Blockrequest (like wib->At/wib->ebus)
            dat[1] = (byte)(cpuid & 0xFF);                      // LB CPU-ID
            dat[2] = (byte)(cpuid >> 8 & 0xFF);                 // HB CPU-ID
            dat[3] = 13;                                        // LB Nachrichtenlänge
            dat[4] = 0;                                         // HB Nachrichtenlänge
            dat[5] = 74;                                        // 4Ah innerer Bereich vom Blockrequest
            dat[6] = 2;                                         // Länge des Datenpunktes
            dat[7] = ((byte)(startdp & 0xFF));                  // LB Datenpunktadresse
            dat[8] = ((byte)(startdp >> 8 & 0xFF));             // HB Datenpunktadresse
            dat[9] = ((byte)(dpanzahl & 0xFF));                 // LB Datenpunkt Anzahl
            dat[10] = ((byte)(dpanzahl >> 8 & 0xFF));           // HB Datenpunkt Anzahl

            checksumCalc = 0xFFFF;                              // checksum Berechnung
            for (a = 0; a <= 10; a++)
            {
                checksumCalc = (ushort)crc16Add(dat[a], checksumCalc);
                dat[11] = (byte)(checksumCalc & 0xFF);          // LB checksum
                dat[12] = (byte)(checksumCalc >> 8 & 0xFF);     // HB checksum
            }
//            Console.WriteLine(checksumCalc.ToString());
//            Console.WriteLine(dat[11].ToString());
//            Console.WriteLine(dat[12].ToString());

            byte[] data = new byte[17];

			/*            data[0] = 2;                                        // zusammenbau ganzer Nachricht Array 17 benötigt
						data[1] = dat[0];
						data[2] = dat[1];
						data[3] = dat[2];
						data[4] = dat[3];
						data[5] = dat[4];
						data[6] = dat[5];
						data[7] = 27;                                       //1B hex escapesequence
						data[8] = dat[6];
						data[9] = dat[7];
						data[10] = 27;                                      //1B hex escapesequence
						data[11] = dat[8];
						data[12] = dat[9];
						data[13] = dat[10];
						data[14] = dat[11];
						data[15] = dat[12];
						data[16] = 3;                                       // ende des zusammenbaus
						*/
			data[0] = 2;                                        // zusammenbau ganzer Nachricht Array 17 benötigt
			data[1] = dat[0];
			data[2] = dat[1];
			data[3] = dat[2];
			data[4] = dat[3];
			data[5] = dat[4];
			data[6] = dat[5];
			data[7] = dat[6];                                       //1B hex escapesequence
			data[8] = dat[7];
			data[9] = dat[8];
			data[10] = dat[9];                                     //1B hex escapesequence
			data[11] = dat[10];
			data[12] = dat[11];
			data[13] = dat[12];
			data[14] = 3;

			string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        public string ReadDPDig(byte[] buffer, byte[] data)
        {
            data[0] = buffer[13]; // Wert
            string zur = ByteArrayToString(data); 
            return zur;
        }
        public string ReadDPAna(byte[] buffer, byte[] data)
        {
			string zuri = ByteArrayToString(buffer);
			StringBuilder builder = new StringBuilder(zuri);
			builder.Replace("1B", "");
			zuri = builder.ToString();
			byte[] neuBuf = new byte[20];

			data[0] = neuBuf[13]; // HB Wert
            data[1] = neuBuf[14]; // LB Wert

            string zur = ByteArrayToString(data);	
			return zur;
        }
        //-------------------------------------------------------------------------------------
        public string WriteDPTrans(uint cpuid, uint startdp, byte bitmap, uint value)
        {
            int a;
            ushort checksumCalc;
            uint dpanzahl = 1;                                  //nur 1 Datenpunkt zum abfragen erlauben! beta-version
            byte[] dat = new byte[19];
            byte[] data = new byte[19];
            dat[0] = 69;                                        // 45h Blockrequest (like wib->At/wib->ebus)
            dat[1] = ((byte)(cpuid & 0xFF));                    // LB CPU-ID
            dat[2] = ((byte)(cpuid >> 8 & 0xFF));               // HB CPU-ID
            dat[3] = 15;                                        // LB Nachrichtenlänge
            dat[4] = 0;                                         // HB Nachrichtenlänge
            dat[5] = 75;                                        // 4Bh innerer Bereich vom Blockrequest schreiben
            dat[6] = 2;                                         // Länge des Datenpunktes
            dat[7] = ((byte)(startdp & 0xFF));                  // LB Datenpunktadresse
            dat[8] = ((byte)(startdp >> 8 & 0xFF));             // HB Datenpunktadresse
            dat[9] = ((byte)(dpanzahl & 0xFF));                 // LB Datenpunkt Anzahl
            dat[10] = ((byte)(dpanzahl >> 8 & 0xFF));           // HB Datenpunkt Anzahl

            dat[11] = bitmap;                                   // access bitmap

            dat[12] = (byte)value;                                    // wert zum uebertragen

            checksumCalc = 0xFFFF;                              // checksum Berechnung
            for (a = 0; a <= 12; a++)
            {
                checksumCalc = (ushort)crc16Add(dat[a], checksumCalc);
                dat[13] = (byte)(checksumCalc & 0xFF);          // LB checksum
                dat[14] = (byte)(checksumCalc >> 8 & 0xFF);     // HB checksum
            }

            data[0] = 2;                                        // zusammenbau ganzer Nachricht Array 19 byte benötigt
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];
            data[7] = 27;                                       //1B hex escapesequence
            data[8] = dat[6];
            data[9] = dat[7];
            data[10] = 27;                                      //1B hex escapesequence
            data[11] = dat[8];
            data[12] = dat[9];
            data[13] = dat[10];
            data[14] = dat[11];
            data[15] = dat[12];
            data[16] = dat[13];
            data[17] = dat[14];
            data[18] = 3;                                       // ende des zusammenbaus

            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        uint crc16Add(byte bytes, uint crc)
        {
            return (ushort)((crc << 8) ^ CRC16.cp_Crc16_CCITT[((crc >> 8) ^ (0xff & bytes))]);
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

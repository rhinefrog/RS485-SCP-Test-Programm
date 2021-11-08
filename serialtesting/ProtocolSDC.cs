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

 /*       unsigned int checksubild(unsigned int distanz, BYTE* padr)
        {
            unsigned int ch_sum;
            unsigned int i;

            ch_sum = 0xFFFF;

            for (i = 0; i < distanz; i++)
                ch_sum = CRC16(padr[i] & 0x00FF, ch_sum);

            return (ch_sum);
        }
 */


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
/*
            dat[0] = ; // controller id 1..255 x x x x x
            dat[1] = ; //command id 1 x x x x x
            dat[2] = ; // number of following bytes 137 x x x x x
            dat[3] = ; // LB watertemperature -input - -50.0..+50.0 °C - 500..+500 x x x
            dat[4] = ; // HB %
            dat[5] = ; // LB return air temperature - input - 0.0..100.0°C 0..1000 x x x x x
            dat[6] = ; // HB %
            dat[7] = ; // LB supply air temperature -input - 0.0..100.0°C 0..1000 x x x
            dat[8] = ; // HB %
            dat[9] = ; // LB return air humidity - input - 0.0..100.0 % 0..1000 x x x x x
            dat[10] = ; // HB %
            dat[11] = ; // LB supply air humidity -input - 0.0..100.0 % 0..1000 x x x
            dat[12] = ; // HB %
            dat[13] = ; // LB outside air temperature -input - -50.0..+50.0 °C - 500..+500 x x x x
            dat[14] = ; // HB %
            dat[15] = ; // LB outside air humidity -input - 0.0..100.0 % 0..1000 x x x x
            dat[16] = ; // HB %
            dat[17] = ; // temperature setpoint shift(±12.7°C) ±127 - 127..+127 x x x
            dat[18] = ; // humidity setpoint shift(±12.7 %) ±127 - 127..+127 x x x
            dat[19] = ; // - reserved -
            dat[20] = ; // - reserved -
            dat[21] = ; // - reserved -
            dat[22] = ; // modul 1 - 4 digital out compressor 2 bit - coded see table x x
            dat[23] = ; // modul 1 - 4 digital in compressor 2 bit - coded see table x x
            dat[24] = ; // SW - Version x x x x
            dat[25] = ; // module 1 digital out status byte 1 bit - coded see table x x x x x
            dat[26] = ; // module 1 digital out status byte 2 bit - coded see table x x x x x
            dat[27] = ; // module 2 digital out status byte 1 bit - coded see table x x x x
            dat[28] = ; // module 2 digital out status byte 2 bit - coded see table x x x x
            dat[29] = ; // module 3 digital out status byte 1 bit - coded see table x x x
            dat[30] = ; // module 3 digital out status byte 2 bit - coded see table x x x
            dat[31] = ; // module 4 digital out status byte 1 bit - coded see table x x x
            dat[32] = ; // module 4 digital out status byte 2 bit - coded see table x x x
            dat[33] = ; // module 1 digital in status byte 1 bit - coded see table x x x x x
            dat[34] = ; // module 1 digital in status byte 2 bit - coded see table x x x x x
            dat[35] = ; // module 2 digital in status byte 1 bit - coded see table x x x x
            dat[36] = ; // module 2 digital in status byte 2 bit - coded see table x x x
            dat[37] = ; // module 3 digital in status byte 1 bit - coded see table x x x
            dat[38] = ; // module 3 digital in status byte 2 bit - coded see table x x x
            dat[39] = ; // module 4 digital in status byte 1 bit - coded see table x x x
            dat[40] = ; // module 4 digital in status byte 2 bit - coded see table x x x
            dat[41] = ; // analogue out GE / CW valve(module 1) 0 - 100 % 0..255 x x x x x
            dat[42] = ; // analogue out PWW heating(module 1) 0 - 100 % 0..255 x x x
            dat[43] = ; // analogue out humidifier(module 1) 0 - 100 % 0..255 x x x x
            dat[44] = ; // analogue out suction valve module 1 0 - 100 % 0..255 x x x
            dat[45] = ; // analogue out suction valve module 2 0 - 100 % 0..255 x x x
            dat[46] = ; // analogue out suction valve module 3 0 - 100 % 0..255 x x x
            dat[47] = ; // analogue out suction valve module 4 0 - 100 % 0..255 x x x
            dat[48] = ; // setpoint temperature 10.0 - 35.0°C 0..250 x x x x x
            dat[49] = ; // setpoint humidity 10 - 90 % 10..90 x x x x x
            dat[50] = ; // year 0..99 0..99 x x x x
            dat[51] = ; // month 1..12 1..12 x x x x
            dat[52] = ; // day 1..31 1..31 x x x x
            dat[53] = ; // hour 0..23 0..23 x x x x
            dat[54] = ; // minute 0..59 0..59 x x x x
            dat[55] = ; // return air temp.too high alarm 0..30°C 0..30 x x x x x
            dat[56] = ; // supply air temp.too high alarm 0..30°C 0..30 x x x
            dat[57] = ; // return air temp.too low alarm 0..30°C 0..30 x x x x x
            dat[58] = ; // supply air temp.too low alarm 0..30°C 0..30 x x x
            dat[59] = ; // water temp. too high alarm 0..50°C 0..50 x x x
            dat[60] = ; // water temp. too low alarm - 50..+30°C 0..80 x x x
            dat[61] = ; // return air humid.too high alarm 0..90 % 0..90 x x x x x
            dat[62] = ; // supply air humid.too high alarm 0..90 % 0..90 x x x
            dat[63] = ; // return air humid.too low alarm 0..90 % 0..90 x x x x x
            dat[64] = ; // supply air humid.too low alarm 0..90 % 0..90 x x x
                        //          ------------------------------------ - d a t a m o d u l e 1---------------------------------------- -
            dat[65] = ; // compressor start(setpoint +val) 0.0..10.0 K 0..100 x x x x x
            dat[66] = ; // compressor hysteresis 0.0..10.0 K 0..100 x x x x x
            dat[67] = ; // suction valve start 0.0..10.0 K 0..100 x x x
            dat[68] = ; // suction valve proportional band 0.0..10.0 K 0..100 x x x
            dat[69] = ; // drycooler start temperature 0..45°C 0..45 x x x
            dat[70] = ; // drycooler enable temperature 0..45°C 0..45 x ?
            dat[71] = ; // glycol - pump start temperature 0.0..10.0 K 0..100 x x x
            dat[72] = ; // GE / CW valve off temperature 0..35°C 0..35 x x x
            dat[73] = ; // GE / CW valve start temperature 0.0..10.0 K 0..100 x x x x
            dat[74] = ; // GE / CW valve proportional band 0.0..10.0 K 0..100 x x x x
            dat[75] = ; // reheat 1 start temperature 0.0..10.0 K 0..100 x x x x x
            dat[76] = ; // reheat 1 hysteresis 0.0..10.0 K 0..100 x x x x x
            dat[77] = ; // reheat 2 start temperature 0.0..10.0 K 0..100 x x x x
            dat[78] = ; // reheat 2 hysteresis 0.0..10.0 K 0..100 x x x x
            dat[79] = ; // reheat 3 start temperature 0.0..10.0 K 0..100 x x x
            dat[80] = ; // reheat 3 hysteresis 0.0..10.0 K 0..100 x x x
            dat[81] = ; // PWW valve start temperature 0.0..10.0 K 0..100 x x x
            dat[82] = ; // PWW valve proportional band 0.0..10.0 K 0..100 x x x
            dat[83] = ; // dehumidification start 0..90 % 0..90 x x x x x
            dat[84] = ; // dehumidification hysteresis 0..90 % 0..90 x x x x x
            dat[85] = ; // humidification start 0..90 % 0..90 x x x x x
            dat[86] = ; // humidification hysteresis 0..90 % 0..90 x x x x x
            dat[87] = ; // humidification start(analogue) 0..90 % 0..90 x x x x
            dat[88] = ; // humidification proportional band(analogue) 0..90 % 0..90 x x x x
                        //         -------------------------------------d a t a m o d u l e 2---------------------------------------- -
            dat[89] = ; // compressor start(setpoint +val) 0.0..10.0 K 0..100 x x x x
            dat[90] = ; // compressor hysteresis 0.0..10.0 K 0..100 x x x x
            dat[91] = ; // suction valve start 0.0..10.0 K 0..100 x x x
            dat[92] = ; // suction valve proportional band 0.0..10.0 K 0..100 x x x
            dat[93] = ; // drycooler start temperature 0..45°C 0..45 x x x
            dat[94] = ; // reheat 1 start temperature 0.0..10.0 K 0..100 x x x x
            dat[95] = ; // reheat 1 hysteresis 0.0..10.0 K 0..100 x x x x
            dat[96] = ; // reheat 2 start temperature 0.0..10.0 K 0..100 x x x x
            dat[97] = ; // reheat 2 hysteresis 0.0..10.0 K 0..100 x x x x
            dat[98] = ; // reheat 3 start temperature 0.0..10.0 K 0..100 x x x
            dat[99] = ; // reheat 3 hysteresis 0.0..10.0 K 0..100 x x x
            dat[100] = ; // dehumidification start 0..90 % 0..90 x x x x
            dat[101] = ; // dehumidification hysteresis 0..90 % 0..90 x x x x
            dat[102] = ; // humidification start 0..90 % 0..90 x x x x
            dat[103] = ; // humidification hysteresis 0..90 % 0..90 x x x x
                         //         -------------------------------------d a t a m o d u l e 3---------------------------------------- -
            dat[104] = ; // compressor start(setpoint +val) 0.0..10.0 K 0..100 x x x x
            dat[105] = ; // compressor hysteresis 0.0..10.0 K 0..100 x x x x
            dat[106] = ; // suction valve start 0.0..10.0 K 0..100 x x x
            dat[107] = ; // suction valve proportional band 0.0..10.0 K 0..100 x x x
            dat[108] = ; // drycooler start temperature 0..45°C 0..45 x x x
            dat[109] = ; // reheat 1 start temperature 0.0..10.0 K 0..100 x x x x
            dat[110] = ; // reheat 1 hysteresis 0.0..10.0 K 0..100 x x x x
            dat[111] = ; // reheat 2 start temperature 0.0..10.0 K 0..100 x x x x
            dat[112] = ; // reheat 2 hysteresis 0.0..10.0 K 0..100 x x x x
            dat[113] = ; // reheat 3 start temperature 0.0..10.0 K 0..100 x x x
            dat[114] = ; // reheat 3 hysteresis 0.0..10.0 K 0..100 x x x
            dat[115] = ; // dehumidification start 0..90 % 0..90 x x x x
            dat[116] = ; // dehumidification hysteresis 0..90 % 0..90 x x x x
            dat[117] = ; // humidification start 0..90 % 0..90 x x x x
            dat[118] = ; // humidification hysteresis 0..90 % 0..90 x x x x
                         //         -------------------------------------d a t a m o d u l e 4---------------------------------------- -
            dat[119] = ; // compressor start(setpoint +val) 0.0..10.0 K 0..100 x x x x
            dat[120] = ; // compressor hysteresis 0.0..10.0 K 0..100 x x x x
            dat[121] = ; // suction valve start 0.0..10.0 K 0..100 x x x
            dat[122] = ; // suction valve proportional band 0.0..10.0 K 0..100 x x x
            dat[123] = ; // drycooler start temperature 0..45°C 0..45 x x x
            dat[124] = ; // reheat 1 start temperature 0.0..10.0 K 0..100 x x x x
            dat[125] = ; // reheat 1 hysteresis 0.0..10.0 K 0..100 x x x x
            dat[126] = ; // reheat 2 start temperature 0.0..10.0 K 0..100 x x x x
            dat[127] = ; // reheat 2 hysteresis 0.0..10.0 K 0..100
            dat[128] = ; // reheat 3 start temperature 0.0..10.0 K 0..100
            dat[129] = ; // reheat 3 hysteresis 0.0..10.0 K 0..100
            dat[130] = ; // dehumidification start 0..90 % 0..90
            dat[131] = ; // dehumidification hysteresis 0..90 % 0..90
            dat[132] = ; // humidification start 0..90 % 0..90
            dat[133] = ; // humidification hysteresis 0..90 % 0..90
                         //         -------------------------------------g e n e r a l d a t a -----------------------------------------
            dat[134] = ; // general status byte 1 bit - coded see table
            dat[135] = ; // general status byte 2 bit - coded see table
            dat[136] = ; // error byte 1 bit - coded see table
            dat[137] = ; // error byte 2 bit - coded see table
                         //        dat[138 LB checksum 0..255
                         //        dat[139 HB checksum 0..255 

            checksumCalc = 0xFFFF;                              // checksum Berechnung
            for (a = 0; a <= 2; a++)
            {
                checksumCalc = (ushort)crc16(dat[a], checksumCalc);
                dat[138] = (byte)(checksumCalc & 0xFF);          // LB checksum
                dat[139] = (byte)(checksumCalc >> 8 & 0xFF);     // HB checksum
            }
//            The 16 bit checksum is the 2's complement of the sum from byte 0 to byte 137.
*/
            string tes = ByteArrayToString(dat);
            return tes;
        }

        /*
 
        Bit # General Status Byte 1 POS R / W C1001/2 C4000 C5000 C6000 C1010
        0 0 = PC-STOP(monitoring), 1 = on 0x01 R/W x x x x x
        1 0 = REMOTE STOP(contact), 1 = on 0x02 R x x x x x
        2 0 = LOCAL STOP(key), 1 = on 0x04 R x x x x x
        3 0 = TIMER-STOP(weekly oper.), 1 = on 0x08 R x x x
        4 Seq.Start/Stop(0=No, 1=Yes) 0x10 R all with MIB 6000
        5 not used 0x20 R
        6 not used 0x40 R
        7 not used 0x80 R
        NOTE: only if bits #0 to #3 are set to 1 the unit is in operation.
        If one of the bits #0,#1,#2,#3 is set to 0 the unit is in STOP condition.
        Bit # General Status Byte 2 POS R / W C1001/2 C4000 C5000 C6000 C1010
        0 not used 0x01 R
        1 not used 0x02 R
        2 not used 0x04 R
        3 not used 0x08 R
        4 not used 0x10 R
        5 not used 0x20 R
        6 not used 0x40 R
        7 not used 0x80 R
        Bit # module 1 - 4 digital OUT compressor 2 POS R / W C1001/2 C4000 C5000 C6000 C1010
        0 modul 1 compressor 2 0x01 R x x
        1 modul 2 compressor 2 0x02 R x x
        2 modul 3 compressor 2 0x04 R x x
        3 modul 4 compressor 2 0x08 R x x
        4 not used 0x10 R x x
        5 not used 0x20 R x x
        6 not used 0x40 R x x
        7 not used 0x80 R x x
        NOTE: if a bit is set to 1, than the corresponding component is in operation.
        Seite 8
        0x01 long status
        Bit # module x digital OUT status byte 1 POS R / W C1001/2 C4000 C5000 C6000 C1010
        0 reheat 1 0x01 R x x x x x
        1 compressor 1 0x02 R x x x x x
        2 humidification 1 0x04 R x x x x x
        3 dehumidification 1 (auch CW) 0x08 R x x x x x
        4 fan 1 0x10 R x x x x x
        5 drycooler 0x20 R x x x
        6 alarm relays #1; (1 = no alarm) 0x40 R x x x x x
        7 hot gas reheat / reheat 3 / PWW 0x80 R x x x
        NOTE: if a bit is set to 1, than the corresponding component is in operation.
        Bit # module x digital OUT status byte 2 POS R / W C1001/2 C4000 C5000 C6000 C1010
        0 reheat 2 0x01 R x x x x
        1 glycol pump 0x02 R x x x
        2 louver(0=closed, 1=open) 0x04 R x x x x x
        3 alarm relays #2; (1 = no alarm) 0x08 R x x x x
        4 alarm relays #3; (1 = no alarm) 0x10 R x ?
        5 alarm relays #4; (1 = no alarm) 0x20 R x ?
        6 alarm relays #5; (1 = no alarm) 0x40 R x ?
        7 glycol pump 1 / 2 select 0x80 R x x x
        NOTE: if a bit is set to 1, than the corresponding component is in operation.
        Bit # module 1 - 4 digital IN compressor 2 POS R / W C1001/2 C4000 C5000 C6000 C1010
        0 modul 1 compressor 2 low pressure 0x01 R x x
        1 modul 1 compressor 2 high pressure 0x02 R x x
        2 modul 2 compressor 2 low pressure 0x04 R x x
        3 modul 2 compressor 2 high pressure 0x08 R x x
        4 modul 3 compressor 2 low pressure 0x10 R x x
        5 modul 3 compressor 2 high pressure 0x20 R x x
        6 modul 4 compressor 2 low pressure 0x40 R x x
        7 modul 4 compressor 2 high pressure 0x80 R x x
        NOTE: if a bit is set to 1, than the corresponding alarm is active.
        Bit # module x digital IN status byte 1 POS R / W C1001/2 C4000 C5000 C6000 C1010
        0 compressor low pressure 0x01 R x x x x x
        1 compressor high pressure 0x02 R x x x x x
        2 reheat 1 failure 0x04 R x x x x x
        3 humidification failure 0x08 R x x x x
        4 air flow failure 0x10 R x x x x x
        5 filter clogged 0x20 R x x x x x
        6 aux.alarm #1 0x40 R x x x x
        7 reheat 2 failure 0x80 R x x x x
        NOTE: if a bit is set to 1, than the corresponding alarm is active.
        Bit # module x digital IN status byte 2 POS R / W C1001/2 C4000 C5000 C6000 C1010
        0 conductivity too high(< 5µS) 0x01 R x x x
        1 ultrasonic failure 0x02 R x x x
        2 glycol pump 1 failure 0x04 R x x x
        3 glycol pump 2 failure 0x08 R x x x
        4 drycooler failure 0x10 R x x x
        5 water detector 0x20 R x x?
        6 aux.alarm #2 0x40 R x x x
        7 aux.alarm #3 0x80 R x x x
        NOTE: if a bit is set to 1, than the corresponding alarm is active.
        Bit # error byte 1 POS R / W C1001/2 C4000 C5000 C6000 C1010
        0 return air temp.too high alarm 0x01 R x x x x x
        1 return air humid. too high alarm 0x02 R x x x x x
        2 supply air temp.too high alarm 0x04 R x x x
        3 supply air humid.too high alarm 0x08 R x x x
        4 water temp. too high alarm 0x10 R x x x
        5 return air temp. too low alarm 0x20 R x x x x x
        6 return air humid. too low alarm 0x40 R x x x x x
        7 supply air temp.too low alarm 0x80 R x x x
        NOTE: if a bit is set to 1, than the corresponding alarm is active.
        Seite 9
        0x01 long status
        Bit
        #error byte 2 POS R / W C1001/2 C4000 C5000 C6000 C1010
        0 supply air humid.too low alarm 0x01 R x x x
        1 water temp. too low alarm 0x02 R x x x
        2 supervisor failure 0x04 R x x x
        3 freeze alarm 0x08 R x x x x
        4 fire / smoke detector 0x10 R x x x x x
        5 sensor failure 0x20 R x x x x x
        6 controller failure 0x40 R x x x x x
        7 IO-board transmission failure 0x80 R x x x x
        NOTE: if a bit is set         */
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

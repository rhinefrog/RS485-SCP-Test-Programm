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
    {/*
    //--------------------------------------------
    // structs
    //--------------------------------------------
    public struct SDC_C001_RETURN_TYPE
    {
       char           status;
       ushort          error_nbr;
   
       ushort          water_temp;                                   // B 3,4    -500..+500
       ushort          return_air_temp;                              // B 5,6    0..1000
       ushort          supply_air_temp;                              // B 7,8    0..1000
       ushort          outside_air_temp;                             // B 13,14  -500..+500
       ushort          return_air_hum;                               // B 9,10   0..1000
       ushort          supply_air_hum;                               // B 11,12  0..1000

       char  ge_cw_valve;                                  // B 41     0..100 (!!! oder Ecocool-Jalousie)
       char  pww_heating;                                  // B 42     0..100
       char  humidifier;                                   // B 43     0..100
       char  suction_valve_m1;                             // B 44     0..100
       char  suction_valve_m2;                             // B 45     0..100
       char  suction_valve_m3;                             // B 46     0..100
       char  suction_valve_m4;                             // B 47     0..100
       
       char  water_temp_high;                              // B 59     0..50
       char  water_temp_low;                               // B 60   -50..50
       char  return_air_temp_high;                         // B 55     5..40
       char  return_air_temp_low;                          // B 57     5..40
       char  supply_air_temp_high;                         // B 56     5..40
       char  supply_air_temp_low;                          // B 58     5..40
       char  return_air_hum_high;                          // B 61     0..90
       char  return_air_hum_low;                           // B 63     0..90
       char  supply_air_hum_high;                          // B 62     0..90
       char  supply_air_hum_low;                           // B 64     0..90

       ushort sp_temperature;                               // B 48     100..350
       ushort sp_humidity;                                  // B 49     0..100

       char  d_out_status_mask [8][2];        // B 25-32
       char  d_in_status_mask [8][2];         // B 33-40
       char  general_status_mask [2];                      // B 134,135
       char  error_mask [2];                               // B 136,137
    };
    //--------------------------------------------
    public struct SDC_C001_CHILLER_RETURN_TYPE
    {
       char           status;
       ushort          error_nbr;
   
       ushort          water_temp;                                   // B 3,4    -500..+500
       ushort          fflow_temp;                                   // B 5,6    -500..+500
       ushort          rflow_temp;                                   // B 7,8    -500..+500
       ushort          outside_air_temp;                             // B 13,14  -500..+500

       char  ge_cw_valve;                                  // B 41     0..100 
       char  suction_valve_m1;                             // B 44     0..100
       char  suction_valve_m2;                             // B 45     0..100
       char  suction_valve_m3;                             // B 46     0..100
       char  suction_valve_m4;                             // B 47     0..100

       char  water_temp_high;                              // B 59     0..20
       char  water_temp_low;                               // B 60   -50..50
       char  fflow_temp_high;                              // B 55     5..25
       char  fflow_temp_low;                               // B 57     5..20
       char  rflow_temp_high;                              // B 56     5..30
       char  rflow_temp_low;                               // B 58     0..20

       short sp_fflow;                                     // B 48     40..200
       short sp_rflow;                                     // B 49     50..250

       char  d_out_status_mask [8][2];        // B 25-32
       char  d_in_status_mask [8][2];         // B 33-40
       char  general_status_mask [2];                      // B 134,135
       char  error_mask [2];                               // B 136,137
    };
    //--------------------------------------------
    public struct SDC_C005_RETURN_TYPE
    {
       char           status;
       ushort          error_nbr;
       ushort data;
    };
    //--------------------------------------------
    public struct SDC_C007_RETURN_TYPE
    {
       char          status;
       ushort         error_nbr;
       char          short_status;
    };
    //--------------------------------------------
    public struct SDC_C008_RETURN_TYPE
    {
       char          status;
       ushort         error_nbr;
    };
    //--------------------------------------------
    public struct SDC_C010_RETURN_TYPE
    {
       char          status;
       ushort         error_nbr;
       char sw_version;
       char hw_version;
       char unit_type;
    };
    //--------------------------------------------
    public struct SDC_C011_RETURN_TYPE
    {
       char           status;
       ushort          error_nbr;
       ushort          module_nbr;
       ushort module_config_mask [8];
       char  general_config_mask [2];
    };
    //--------------------------------------------
    public struct SDC_C012_RETURN_TYPE
    {
       char           status;
       ushort          error_nbr;
       char  current_index [6];
       ushort          value [6][96];
    };
    //--------------------------------------------
    public struct SDC_C128_RETURN_TYPE
    {
       char           status;
       ushort          error_nbr;
       short sdc_id;
       ulong  errmask;
       ulong  password_crc32;
    };
    //--------------------------------------------
    public struct SDC_C129_RETURN_TYPE
    {
       char           status;
       ushort          error_nbr;
       char  unit_status;
       char  year;
       char  month;
       char  day;
       char  hour;
       char  minute;
       char  d_in_status_mask [8][2];         // --> B 33-40 lstate
       char  general_status_mask [2];         // --> B 134,135 lstate
       char  error_mask [2];                  // --> B 136,137 lstate
    };
    //--------------------------------------------
    public struct SDC_C131_RETURN_TYPE
    {
       char           status;
       ushort          error_nbr;
       ulong  devmask;
    };
    //--------------------------------------------
    public struct SDC_RESPONSE_TYPE
    {
       SDC_C001_RETURN_TYPE         lstate;
       SDC_C001_CHILLER_RETURN_TYPE lstate_chiller;
       SDC_C005_RETURN_TYPE         eeprom;
       SDC_C007_RETURN_TYPE         devstate;
       SDC_C008_RETURN_TYPE         alarm_reset;
       SDC_C010_RETURN_TYPE         devtype;
       SDC_C011_RETURN_TYPE         devcfg;
       SDC_C012_RETURN_TYPE         scanval;        // Cmd 12: nur f. C5000/C6000
       SDC_C128_RETURN_TYPE         errmsg;
       SDC_C129_RETURN_TYPE         errstate;
       SDC_C131_RETURN_TYPE         password;
    };
    //--------------------------------------------
    SDC_C001_RETURN_TYPE Funktion1;
    SDC_C001_CHILLER_RETURN_TYPE FunktionCh1;
    SDC_C005_RETURN_TYPE Funktion2;
    SDC_C007_RETURN_TYPE Funktion3;
    SDC_C008_RETURN_TYPE Funktion4;
    SDC_C010_RETURN_TYPE Funktion5;
    SDC_C011_RETURN_TYPE Funktion6;
    SDC_C012_RETURN_TYPE Funktion7;
    SDC_C128_RETURN_TYPE Funktion8;
    SDC_C129_RETURN_TYPE Funktion9;
    SDC_C131_RETURN_TYPE Funktion10;
    SDC_RESPONSE_TYPE Funktion11;
    //--------------------------------------------
}
       public static crc3tab[] cp_Crc3_CCITT =
        {
//static long cr3tab [] =                   // CRC polynomial 0xedb88320 
//{ 
       0x00000000L, 0x77073096L, 0xee0e612cL, 0x990951baL, 0x076dc419L, 0x706af48fL, 0xe963a535L, 0x9e6495a3L,
       0x0edb8832L, 0x79dcb8a4L, 0xe0d5e91eL, 0x97d2d988L, 0x09b64c2bL, 0x7eb17cbdL, 0xe7b82d07L, 0x90bf1d91L,
       0x1db71064L, 0x6ab020f2L, 0xf3b97148L, 0x84be41deL, 0x1adad47dL, 0x6ddde4ebL, 0xf4d4b551L, 0x83d385c7L,
       0x136c9856L, 0x646ba8c0L, 0xfd62f97aL, 0x8a65c9ecL, 0x14015c4fL, 0x63066cd9L, 0xfa0f3d63L, 0x8d080df5L,
       0x3b6e20c8L, 0x4c69105eL, 0xd56041e4L, 0xa2677172L, 0x3c03e4d1L, 0x4b04d447L, 0xd20d85fdL, 0xa50ab56bL,
       0x35b5a8faL, 0x42b2986cL, 0xdbbbc9d6L, 0xacbcf940L, 0x32d86ce3L, 0x45df5c75L, 0xdcd60dcfL, 0xabd13d59L,
       0x26d930acL, 0x51de003aL, 0xc8d75180L, 0xbfd06116L, 0x21b4f4b5L, 0x56b3c423L, 0xcfba9599L, 0xb8bda50fL,
       0x2802b89eL, 0x5f058808L, 0xc60cd9b2L, 0xb10be924L, 0x2f6f7c87L, 0x58684c11L, 0xc1611dabL, 0xb6662d3dL,
       0x76dc4190L, 0x01db7106L, 0x98d220bcL, 0xefd5102aL, 0x71b18589L, 0x06b6b51fL, 0x9fbfe4a5L, 0xe8b8d433L,
       0x7807c9a2L, 0x0f00f934L, 0x9609a88eL, 0xe10e9818L, 0x7f6a0dbbL, 0x086d3d2dL, 0x91646c97L, 0xe6635c01L,
       0x6b6b51f4L, 0x1c6c6162L, 0x856530d8L, 0xf262004eL, 0x6c0695edL, 0x1b01a57bL, 0x8208f4c1L, 0xf50fc457L,
       0x65b0d9c6L, 0x12b7e950L, 0x8bbeb8eaL, 0xfcb9887cL, 0x62dd1ddfL, 0x15da2d49L, 0x8cd37cf3L, 0xfbd44c65L,
       0x4db26158L, 0x3ab551ceL, 0xa3bc0074L, 0xd4bb30e2L, 0x4adfa541L, 0x3dd895d7L, 0xa4d1c46dL, 0xd3d6f4fbL,
       0x4369e96aL, 0x346ed9fcL, 0xad678846L, 0xda60b8d0L, 0x44042d73L, 0x33031de5L, 0xaa0a4c5fL, 0xdd0d7cc9L,
       0x5005713cL, 0x270241aaL, 0xbe0b1010L, 0xc90c2086L, 0x5768b525L, 0x206f85b3L, 0xb966d409L, 0xce61e49fL,
       0x5edef90eL, 0x29d9c998L, 0xb0d09822L, 0xc7d7a8b4L, 0x59b33d17L, 0x2eb40d81L, 0xb7bd5c3bL, 0xc0ba6cadL,
       0xedb88320L, 0x9abfb3b6L, 0x03b6e20cL, 0x74b1d29aL, 0xead54739L, 0x9dd277afL, 0x04db2615L, 0x73dc1683L,
       0xe3630b12L, 0x94643b84L, 0x0d6d6a3eL, 0x7a6a5aa8L, 0xe40ecf0bL, 0x9309ff9dL, 0x0a00ae27L, 0x7d079eb1L,
       0xf00f9344L, 0x8708a3d2L, 0x1e01f268L, 0x6906c2feL, 0xf762575dL, 0x806567cbL, 0x196c3671L, 0x6e6b06e7L,
       0xfed41b76L, 0x89d32be0L, 0x10da7a5aL, 0x67dd4accL, 0xf9b9df6fL, 0x8ebeeff9L, 0x17b7be43L, 0x60b08ed5L,
       0xd6d6a3e8L, 0xa1d1937eL, 0x38d8c2c4L, 0x4fdff252L, 0xd1bb67f1L, 0xa6bc5767L, 0x3fb506ddL, 0x48b2364bL,
       0xd80d2bdaL, 0xaf0a1b4cL, 0x36034af6L, 0x41047a60L, 0xdf60efc3L, 0xa867df55L, 0x316e8eefL, 0x4669be79L,
       0xcb61b38cL, 0xbc66831aL, 0x256fd2a0L, 0x5268e236L, 0xcc0c7795L, 0xbb0b4703L, 0x220216b9L, 0x5505262fL,
       0xc5ba3bbeL, 0xb2bd0b28L, 0x2bb45a92L, 0x5cb36a04L, 0xc2d7ffa7L, 0xb5d0cf31L, 0x2cd99e8bL, 0x5bdeae1dL,
       0x9b64c2b0L, 0xec63f226L, 0x756aa39cL, 0x026d930aL, 0x9c0906a9L, 0xeb0e363fL, 0x72076785L, 0x05005713L,
       0x95bf4a82L, 0xe2b87a14L, 0x7bb12baeL, 0x0cb61b38L, 0x92d28e9bL, 0xe5d5be0dL, 0x7cdcefb7L, 0x0bdbdf21L,
       0x86d3d2d4L, 0xf1d4e242L, 0x68ddb3f8L, 0x1fda836eL, 0x81be16cdL, 0xf6b9265bL, 0x6fb077e1L, 0x18b74777L,
       0x88085ae6L, 0xff0f6a70L, 0x66063bcaL, 0x11010b5cL, 0x8f659effL, 0xf862ae69L, 0x616bffd3L, 0x166ccf45L,
       0xa00ae278L, 0xd70dd2eeL, 0x4e048354L, 0x3903b3c2L, 0xa7672661L, 0xd06016f7L, 0x4969474dL, 0x3e6e77dbL,
       0xaed16a4aL, 0xd9d65adcL, 0x40df0b66L, 0x37d83bf0L, 0xa9bcae53L, 0xdebb9ec5L, 0x47b2cf7fL, 0x30b5ffe9L,
       0xbdbdf21cL, 0xcabac28aL, 0x53b39330L, 0x24b4a3a6L, 0xbad03605L, 0xcdd70693L, 0x54de5729L, 0x23d967bfL,
       0xb3667a2eL, 0xc4614ab8L, 0x5d681b02L, 0x2a6f2b94L, 0xb40bbe37L, 0xc30c8ea1L, 0x5a05df1bL, 0x2d02ef8dL
    };
        const ulong polynomial = 0xedb88320;
        static readonly ulong[] table = new ulong[256];

        public static ulong ComputeChecksum(byte[] bytes)
        {
            ulong crc = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }
            return crc;
        }
        public static ushort CRC3CCITT(byte[] bytes, ushort initialValue)
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

//-------------------------------------------------------------------------------------
ushort sdc_write_password
(
        ushort           sdc_id,
        ulong            sdc_password,
        SDC_RESPONSE_TYPE        * response
)
{
        ushort           error;
        int             receive_byte_nbr;
        char            transmit_buffer [32],
                        receive_buffer [32];

        // Übertragungsdaten vorbereiten

        memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

        SETWORD(sdc_id, transmit_buffer);
        transmit_buffer [6] = (char) 131;
        SETLONG(sdc_password, &(transmit_buffer [7]));

        crc32_insert (transmit_buffer, 11);

        // Antwort in entsprechenden Record übertragen

        response->password.status    = receive_buffer [2];
        response->password.error_nbr = (short) GETWORD(&(receive_buffer [3]));
        response->password.devmask   = GETLONG(&(receive_buffer [7]));
   
        return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_write_devstate
(
        ushort       sdc_id,
        ushort                 device_id,
        ushort                 device_state,
        SDC_RESPONSE_TYPE     * response
)
{
	ushort	error;
   int	receive_byte_nbr;
   char	transmit_buffer [32],
		   receive_buffer [32];

   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

   SETWORD(sdc_id, transmit_buffer);

   transmit_buffer [5] = (char) device_id;
   transmit_buffer [6] = 7;
   transmit_buffer [7] = 3;

   if (device_state)
      transmit_buffer [8] = 1;
   else
      transmit_buffer [8] = 0;

   crc32_insert (transmit_buffer, 9);

   response->devstate.status       = receive_buffer [2];
   response->devstate.error_nbr    = (short) GETWORD(&(receive_buffer [3]));
   response->devstate.short_status = receive_buffer [8];

   return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_write_alarm_reset
(
   ushort      sdc_id,
   ushort               device_id,
   SDC_RESPONSE_TYPE * response
)
{
   ushort error;
   int   receive_byte_nbr;
   char  transmit_buffer [32],
         receive_buffer [32];

   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

   SETWORD(sdc_id, transmit_buffer);

   transmit_buffer [5] = (char) device_id;
   transmit_buffer [6] = 8;
   transmit_buffer [7] = 2;

   crc32_insert (transmit_buffer, 8);

   response->alarm_reset.status    = receive_buffer [2];
   response->alarm_reset.error_nbr = (short) GETWORD(&(receive_buffer [3]));

   return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_write_eeprom_temperature
(
   ushort      sdc_id,
   ushort               device_id,
   ushort               temperature,     // 0..250  <=> 10.0 .. 35.0
   SDC_RESPONSE_TYPE * response
)
{
   ushort            error;
   char    temp;
   EEPROM_DATA_TYPE result;

   // temp = (unsigned char) ((temperature - 100) & 0xff);
   temp = (char) (temperature & 0xff);

   response->eeprom.status    = result.status;
   response->eeprom.error_nbr = result.error_nbr;
   // response->eeprom.data      = ((unsigned short) (result.data)) + 100;
   response->eeprom.data      = ((ushort) (result.data));

   return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_write_eeprom_fflow_chiller
(
   ushort      sdc_id,
   ushort               device_id,
   ushort               fflow,           // 40..200  <=> 4.0 .. 20.0
   SDC_RESPONSE_TYPE * response
)
{
   ushort            error;
   char    f;
   EEPROM_DATA_TYPE result;

   f = (char) (fflow & 0xff);

   response->eeprom.status    = result.status;
   response->eeprom.error_nbr = result.error_nbr;
   response->eeprom.data      = ((ushort) (result.data));

   return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_write_eeprom_humidity
(
   ushort      sdc_id,
   ushort               device_id,
   ushort               humidity,     // 0..100  <=> 0 .. 100 [%]
   SDC_RESPONSE_TYPE * response
)
{
   ushort            error;
   char    hum;
   EEPROM_DATA_TYPE result;

   hum = (char) (humidity & 0xff);

   response->eeprom.status    = result.status;
   response->eeprom.error_nbr = result.error_nbr;
   response->eeprom.data      = (ushort) (result.data);

   return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_write_eeprom_rflow_chiller
(
   ushort      sdc_id,
   ushort               device_id,
   ushort               rflow,           // 50..250  <=> 5.0 .. 25.0
   SDC_RESPONSE_TYPE * response
)
{
   ushort            error;
   char    r;
   EEPROM_DATA_TYPE result;

   r = (char) (rflow & 0xff);

   response->eeprom.status    = result.status;
   response->eeprom.error_nbr = result.error_nbr;
   response->eeprom.data      = (ushort) (result.data);

   return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_query_devstate
(
   ushort      sdc_id,
   ushort               device_id,
   SDC_RESPONSE_TYPE * response
)
{
   short error;
   int   receive_byte_nbr;
   char  transmit_buffer [32],
         receive_buffer [32];

   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

   SETWORD(sdc_id, transmit_buffer);

   transmit_buffer [5] = (char) device_id;
   transmit_buffer [6] = 7;
   transmit_buffer [7] = 3;
   transmit_buffer [8] = 2; 

   crc32_insert (transmit_buffer, 9);

   response->devstate.status       = receive_buffer [2];
   response->devstate.error_nbr    = (short) GETWORD(&(receive_buffer [3]));
   response->devstate.short_status = receive_buffer [8];

   return (0);
}
//-------------------------------------------------------------------------------------
short SDC::sdc_query_eeprom_temperature
(
   ushort      sdc_id,
   ushort               device_id,
   SDC_RESPONSE_TYPE * response
)
{
   ushort            error;
   EEPROM_DATA_TYPE result;

   response->eeprom.status    = result.status;
   response->eeprom.error_nbr = result.error_nbr;
   response->eeprom.data      = ((ushort) (result.data)) + 100;

   return (0);
}
//-------------------------------------------------------------------------------------
short SDC::sdc_query_eeprom_humidity
(
   ushort      sdc_id,
   ushort               device_id,
   SDC_RESPONSE_TYPE * response
)
{
   ushort            error;
   EEPROM_DATA_TYPE result;


   response->eeprom.status    = result.status;
   response->eeprom.error_nbr = result.error_nbr;
   response->eeprom.data      = (ushort) (result.data);

   return (0);
}
//-------------------------------------------------------------------------------------
//****************************************************************
ushort sdc_query_devtype
// Kommando 10:
// MIB-Antwort = Controller Identification
//****************************************************************
(
   ushort      sdc_id,
   ushort               device_id,
   SDC_RESPONSE_TYPE * response
)
{
   ushort error;
   int   receive_byte_nbr;
   char  transmit_buffer [32],
         receive_buffer [32];

   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

   SETWORD(sdc_id, transmit_buffer);

   transmit_buffer [5] = (char) device_id;
   transmit_buffer [6] = 10;                                            // Kommando 10: Contr.-ident.
   transmit_buffer [7] = 2;

   crc32_insert (transmit_buffer, 8);

   response->devtype.status     = receive_buffer [2];
   response->devtype.error_nbr  = (short) GETWORD(&(receive_buffer [3]));
   response->devtype.sw_version = (char) (receive_buffer [8]);
   response->devtype.hw_version = (char) (receive_buffer [9]);
   response->devtype.unit_type  = (char) (receive_buffer [11]);

   return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_query_devcfg
(
   ushort      sdc_id,
   ushort               device_id,
   SDC_RESPONSE_TYPE * response
)
{
   ushort i, j, error;
   int   receive_byte_nbr;
   char  transmit_buffer [32],
         receive_buffer [32];

   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

   SETWORD(sdc_id, transmit_buffer);

   transmit_buffer [5] = (char) device_id;
   transmit_buffer [6] = 11;
   transmit_buffer [7] = 2;

   crc32_insert (transmit_buffer, 8);


   response->devcfg.status     = receive_buffer [2];
   response->devcfg.error_nbr  = (ushort) GETWORD(&(receive_buffer [3]));
   response->devcfg.module_nbr = ((ushort) (receive_buffer [8])) & 0x00ff;

   if (response->devcfg.module_nbr > MAX_MODULE_NBR)
      response->devcfg.module_nbr = MAX_MODULE_NBR;

   for (i = 0, j = 9; i < response->devcfg.module_nbr; i++, j += 2)
      response->devcfg.module_config_mask [i] = (ushort) GETWORD(&(receive_buffer [j]));

   for (; i < MAX_MODULE_NBR; i++)
      response->devcfg.module_config_mask [i] = 0;

   response->devcfg.general_config_mask [0] = (char) (receive_buffer [17]);
   response->devcfg.general_config_mask [1] = (char) (receive_buffer [18]);

   return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_query_scanval
(
   ushort      sdc_id,
   ushort               device_id,
   ushort               scan_code,       // 0..5, entspr. Raumtemperatur .. Zuluftfeuchte
   SDC_RESPONSE_TYPE * response
)
{
   ushort i, j,
         hi, lo,
         error;
   int   receive_byte_nbr;
   char  transmit_buffer [32],
         receive_buffer [256];

   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

   SETWORD(sdc_id, transmit_buffer);

   transmit_buffer [5] = (char) device_id;
   transmit_buffer [6] = 12;
   transmit_buffer [7] = 3;
   transmit_buffer [8] = (char) (scan_code & 0xff);

   crc32_insert (transmit_buffer, 9);

   response->scanval.status     = receive_buffer [2];
   response->scanval.error_nbr  = (ushort) GETWORD(&(receive_buffer [3]));

   response->scanval.current_index [scan_code] = (char) (receive_buffer [9]);

   for (i = 0, j = 10; i < MAX_C012_SCAN_COL_NBR; i++, j += 2)
   {
      hi = ((ushort) (receive_buffer [j]) & 0xff);
      lo = ((ushort) (receive_buffer [j+1]) & 0xff);
      
      response->scanval.value [scan_code][i] = (hi << 8) | lo;
      
      // response->scanval.value [scan_code][i] = (short) GETWORD(&(receive_buffer [j]));
   }

   return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_query_lstate
(
   ushort      sdc_id,
   ushort               device_id,
   SDC_RESPONSE_TYPE * response
)
{
   ushort i, k, error;
   int   receive_byte_nbr;
   char  transmit_buffer [32],
         receive_buffer [256];

   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

   SETWORD(sdc_id, transmit_buffer);

   transmit_buffer [5] = (char) device_id;
   transmit_buffer [6] = 1;
   transmit_buffer [7] = 2;

   crc32_insert (transmit_buffer, 8);

   response->lstate.status           = receive_buffer [2];
   response->lstate.error_nbr        = (ushort) GETWORD(&(receive_buffer [3]));

   response->lstate.water_temp       = (ushort) GETWORD(&(receive_buffer [8]));
   response->lstate.return_air_temp  = (ushort) GETWORD(&(receive_buffer [10]));
   response->lstate.supply_air_temp  = (ushort) GETWORD(&(receive_buffer [12]));
   response->lstate.outside_air_temp = (ushort) GETWORD(&(receive_buffer [18]));
   response->lstate.return_air_hum   = (ushort) GETWORD(&(receive_buffer [14]));
   response->lstate.supply_air_hum   = (ushort) GETWORD(&(receive_buffer [16]));

   response->lstate.ge_cw_valve      = (char) ((((((ushort) receive_buffer [46]) & 0xff) * 100) + 127) / 255);
   response->lstate.pww_heating      = (char) ((((((ushort) receive_buffer [47]) & 0xff) * 100) + 127) / 255);
   response->lstate.humidifier       = (char) ((((((ushort) receive_buffer [48]) & 0xff) * 100) + 127) / 255);
   response->lstate.suction_valve_m1 = (char) ((((((ushort) receive_buffer [49]) & 0xff) * 100) + 127) / 255);
   response->lstate.suction_valve_m2 = (char) ((((((ushort) receive_buffer [50]) & 0xff) * 100) + 127) / 255);
   response->lstate.suction_valve_m3 = (char) ((((((ushort) receive_buffer [51]) & 0xff) * 100) + 127) / 255);
   response->lstate.suction_valve_m4 = (char) ((((((ushort) receive_buffer [52]) & 0xff) * 100) + 127) / 255);

   response->lstate.water_temp_high      = (char) (receive_buffer [64]);
   response->lstate.water_temp_low       = (char) (receive_buffer [65]);  // 0..100 (!!! entspr. -50..+50)
   response->lstate.return_air_temp_high = (char) (receive_buffer [60]);
   response->lstate.return_air_temp_low  = (char) (receive_buffer [62]);
   response->lstate.supply_air_temp_high = (char) (receive_buffer [61]);
   response->lstate.supply_air_temp_low  = (char) (receive_buffer [63]);
   response->lstate.return_air_hum_high  = (char) (receive_buffer [66]);
   response->lstate.return_air_hum_low   = (char) (receive_buffer [68]);
   response->lstate.supply_air_hum_high  = (char) (receive_buffer [67]);
   response->lstate.supply_air_hum_low   = (char) (receive_buffer [69]);

   response->lstate.sp_temperature   = (((ushort) (receive_buffer [53])) & 0x00ff) + 100;
   response->lstate.sp_humidity      = ((ushort) (receive_buffer [54])) & 0x00ff;

   k = 30;

   for (i = 0; i < 4; i++)
   {
      response->lstate.d_out_status_mask [i][0] = (char) (receive_buffer [k++]);
      response->lstate.d_out_status_mask [i][1] = (char) (receive_buffer [k++]);
   }

   k = 38;

   for (i = 0; i < 4; i++)
   {
      response->lstate.d_in_status_mask [i][0] = (char) (receive_buffer [k++]);
      response->lstate.d_in_status_mask [i][1] = (char) (receive_buffer [k++]);
   }
   
   response->lstate.general_status_mask [0] = (char) (receive_buffer [139]);
   response->lstate.general_status_mask [1] = (char) (receive_buffer [140]);
   response->lstate.error_mask [0]          = (char) (receive_buffer [141]);
   response->lstate.error_mask [1]          = (char) (receive_buffer [142]);

   return (0);
}
//-------------------------------------------------------------------------------------
ushort sdc_query_lstate_chiller
(
   ushort      sdc_id,
   ushort               device_id,
   SDC_RESPONSE_TYPE * response
)
{
   ushort i, k, error;
   int   receive_byte_nbr;
   char  transmit_buffer [32],
         receive_buffer [256];

   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

   SETWORD(sdc_id, transmit_buffer);

   transmit_buffer [5] = (char) device_id;
   transmit_buffer [6] = 1;
   transmit_buffer [7] = 2;

   crc32_insert (transmit_buffer, 8);

   response->lstate_chiller.status           = receive_buffer [2];
   response->lstate_chiller.error_nbr        = (short) GETWORD(&(receive_buffer [3]));

   response->lstate_chiller.water_temp       = (short) GETWORD(&(receive_buffer [8]));
   response->lstate_chiller.fflow_temp       = (short) GETWORD(&(receive_buffer [10]));
   response->lstate_chiller.rflow_temp       = (short) GETWORD(&(receive_buffer [14]));
   response->lstate_chiller.outside_air_temp = (short) GETWORD(&(receive_buffer [18]));

   response->lstate_chiller.ge_cw_valve      = (char) ((((((ushort) receive_buffer [46]) & 0xff) * 100) + 127) / 255);
   response->lstate_chiller.suction_valve_m1 = (char) ((((((ushort) receive_buffer [49]) & 0xff) * 100) + 127) / 255);
   response->lstate_chiller.suction_valve_m2 = (char) ((((((ushort) receive_buffer [50]) & 0xff) * 100) + 127) / 255);
   response->lstate_chiller.suction_valve_m3 = (char) ((((((ushort) receive_buffer [51]) & 0xff) * 100) + 127) / 255);
   response->lstate_chiller.suction_valve_m4 = (char) ((((((ushort) receive_buffer [52]) & 0xff) * 100) + 127) / 255);

   response->lstate_chiller.water_temp_high = (char) (receive_buffer [64]);
   response->lstate_chiller.water_temp_low  = (char) (receive_buffer [65]);  // 0..80 (!!! entspr. -50..+30)
   response->lstate_chiller.fflow_temp_high = (char) (receive_buffer [60]);
   response->lstate_chiller.fflow_temp_low  = (char) (receive_buffer [62]);
   response->lstate_chiller.rflow_temp_high = (char) (receive_buffer [61]);
   response->lstate_chiller.rflow_temp_low  = (char) (receive_buffer [63]);

   response->lstate_chiller.sp_fflow        = ((ushort) (receive_buffer [53] +100)) & 0x00ff;
   response->lstate_chiller.sp_rflow        = ((ushort) (receive_buffer [54] +100)) & 0x00ff;

   k = 30;

   for (i = 0; i < 4; i++)
   {
      response->lstate_chiller.d_out_status_mask [i][0] = (char) (receive_buffer [k++]);
      response->lstate_chiller.d_out_status_mask [i][1] = (char) (receive_buffer [k++]);
   }

   k = 38;

   for (i = 0; i < 4; i++)
   {
      response->lstate_chiller.d_in_status_mask [i][0] = (char) (receive_buffer [k++]);
      response->lstate_chiller.d_in_status_mask [i][1] = (char) (receive_buffer [k++]);
   }
   
   response->lstate_chiller.general_status_mask [0] = (char) (receive_buffer [139]);
   response->lstate_chiller.general_status_mask [1] = (char) (receive_buffer [140]);
   response->lstate_chiller.error_mask [0]          = (char) (receive_buffer [141]);
   response->lstate_chiller.error_mask [1]          = (char) (receive_buffer [142]);

   return (0);
}
//-------------------------------------------------------------------------------------
//****************************************************************
ushort sdc_query_errmsg_bus
//****************************************************************
// Funktion:    Sendet das Kommando 128 auf den Bus und liest
//              danach die MIB-Antwort des Kommandos
//              aus(Fehlerbitmap/PWD).
// Aufruf:      Durch Prozess p_alarm_bus_scan_sdc nach
//              erfoglreicher Passwordübertragung
// Parameter:   Kanal, SDC-Id, MIB-Antwort
//      Änderungen: Bei fehlerhafter Kommunikation wird wiederholt
//                  Versucht(12x), die Daten zu bekommen.
//****************************************************************
(
        ushort         sdc_id, 
        SDC_RESPONSE_TYPE      * response
)
{
        ushort try, error;
        int   receive_byte_nbr;
        char  transmit_buffer [32],
               receive_buffer [32];

   error = 0;

   for (try = 0; try < 11; try++)	// 12x Versuchen, das Kommando 128 fehlerfrei zu senden/empfangen
   {
	   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

      SETWORD(sdc_id, transmit_buffer);

//      transmit_buffer [6] = 128;
//      transmit_buffer [6] = (const int)128;
      transmit_buffer [6] = (char)128;

      crc32_insert (transmit_buffer, 7);

      if (!error)
			break;

      Sleep (2000);
   }

   if (error)
      return (error);
		
   response->errmsg.status         = receive_buffer [2];
   response->errmsg.error_nbr      = (short) GETWORD(&(receive_buffer [3]));
   response->errmsg.sdc_id         = GETWORD(&(receive_buffer [0]));
   response->errmsg.errmask        = GETLONG(&(receive_buffer [7]));
   response->errmsg.password_crc32 = GETLONG(&(receive_buffer [11]));

	return (0);
}
//-------------------------------------------------------------------------------------
//****************************************************************
ushort sdc_query_errstate
//****************************************************************
// Funktion:    Sendet das Kommando 129 auf den Bus und liest
//              danach die MIB-Antwort des Kommandos
//              aus(Error/D-In/Unit-Status usw).
// Aufruf:      Durch Prozess p_alarm_bus_scan_sdc nach
//              erfoglreicher Passwordübertragung usw.
// Parameter:   Kanal, SDC-Id, MIB-Antwort
//      Änderungen: Bei fehlerhafter Kommunikation wird wiederholt
//                  Versucht(12x), die Daten zu bekommen.
//****************************************************************
(
   ushort       sdc_id,
   ushort                device_id,
   SDC_RESPONSE_TYPE    * response
)
{
   ushort  i, k, try, error;
   int    receive_byte_nbr;
   char   transmit_buffer [32],
          receive_buffer [32];

   error = 0;

   for (try = 0; try < 11; try++)	// 12x Versuchen, das Kommando 129 fehlerfrei zu senden/empfangen
   {
	   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

      SETWORD(sdc_id, transmit_buffer);

      transmit_buffer [6] = (char) 129;
      transmit_buffer [7] = (char) device_id;

      crc32_insert (transmit_buffer, 8);
   
      if (!error)
			break;

      Sleep (2000);
   }

   if (error)
      return (error);

   response->errstate.status      = receive_buffer [2];
   response->errstate.error_nbr   = (ushort) GETWORD(&(receive_buffer [3]));

   response->errstate.unit_status = (char) (receive_buffer [8]);
   response->errstate.year        = (char) (receive_buffer [9]);
   response->errstate.month       = (char) (receive_buffer [10]);
   response->errstate.day         = (char) (receive_buffer [11]);
   response->errstate.hour        = (char) (receive_buffer [12]);
   response->errstate.minute      = (char) (receive_buffer [13]);
   
   k = 18;

   for (i = 0; i < 4; i++)
   {
      response->errstate.d_in_status_mask [i][0] = (char) (receive_buffer [k++]);
      response->errstate.d_in_status_mask [i][1] = (char) (receive_buffer [k++]);
   }
   
   response->errstate.general_status_mask [0] = (char) (receive_buffer [14]);
   response->errstate.general_status_mask [1] = (char) (receive_buffer [15]);
   response->errstate.error_mask [0]          = (char) (receive_buffer [16]);
   response->errstate.error_mask [1]          = (char) (receive_buffer [17]);

   return (0);
		
}
//-------------------------------------------------------------------------------------
// --------------------------------- private Routinen ---------------------------------------------
static ushort write_eeprom_data
(
   ushort     sdc_id,
   ushort              device_id,
   char      eeprom_address,
   char      eeprom_data,
   EEPROM_DATA_TYPE * response
)
{
   ushort error;
   int   receive_byte_nbr;
   char  transmit_buffer [32],
         receive_buffer [32];

   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

   SETWORD(sdc_id, transmit_buffer);

   transmit_buffer [5] = (char) device_id;
   transmit_buffer [6] = 5;
   transmit_buffer [7] = 4;
   transmit_buffer [8] = (char) eeprom_address;
   transmit_buffer [9] = (char) eeprom_data;

   crc32_insert (transmit_buffer, 10);

   response->status    = receive_buffer [2];
   response->error_nbr = (short) GETWORD(&(receive_buffer [3]));
   response->data      = eeprom_data;

   return (0);
}
//-------------------------------------------------------------------------------------
static ushort query_eeprom_data
(
   ushort     sdc_id,
   ushort              device_id,
   char      eeprom_address,
   EEPROM_DATA_TYPE * response
)
{
   ushort error;
   int   receive_byte_nbr;
   char  transmit_buffer [32],
         receive_buffer [32];

   memset ((char *) transmit_buffer, 0, 32 * sizeof (char));

   SETWORD(sdc_id, transmit_buffer);

   transmit_buffer [5] = (char) device_id;
   transmit_buffer [6] = 4;
   transmit_buffer [7] = 3;
   transmit_buffer [8] = (char) eeprom_address;

   crc32_insert (transmit_buffer, 9);


   response->status    = receive_buffer [2];
   response->error_nbr = (ushort) GETWORD(&(receive_buffer [3]));
   response->data      = (char) (receive_buffer [8]);

   return (0);
}
//-------------------------------------------------------------------------------------
static ushort verify_response
(
   char * buffer,
   int    receive_byte_nbr,
   int    nominal_nbr
)
{
   ushort error;

   if (receive_byte_nbr != nominal_nbr)
      return (ERR_SDC_INCOMPLETE_RESPONSE);
   
   if (error = crc32_check (buffer, nominal_nbr-4))
      return (error);

   return (0);
}
//-------------------------------------------------------------------------------------
ushort crc32_check
(
   char * Data,
   int    num
) 
{
   long oldcrc = 0xFFFFFFFFL;
   long datacrc;
   int  i;

   for (i = 0; i < num; i++)
   {
      oldcrc = crc32 (Data[i], oldcrc);
   }
  
   datacrc = *((long *) &(Data [num]));

   if (datacrc != oldcrc)
      return (ERR_CRC32_CHECKSUM);

   return (0);
}
//----------------------------------
void crc32_insert
(
   char * buf,
   int    count
)
{
   long oldcrc = 0xFFFFFFFFL;
   int  i;

   for (i = 0; i < count; i++)
   {
      oldcrc = crc32 (buf [i], oldcrc);
   }

   *((long *)&buf [count]) = oldcrc;
}
//-------------------------------
static long crc32
(
   int  b,
   long c
)
{
   return (cr3tab [((int)c ^ b) & 0xff] ^ ((c >> 8) & 0x00FFFFFFL));
}*/
}
    }

/*
 internal sealed class Crc32
    {
        #region Constructor 

        /// 
 
        /// Constructs an instance of a Crc32 class. 
        /// 

        public 
        Crc32()
        {
            _crc32Value = Crc32StartValue;
        } 

        #endregion Constructor 
 
        #region Public properties
 
        /// 

        /// The Crc32 value of the bytes calculated
        /// thus far.
        /// 
 
        public UInt32 Crc32Value
        { 
            get 
            {
                return (~(_crc32Value)); 
            }
        }

        #endregion Public properties 

        #region Public methods 
 
        /// 

        /// Adds the specified bytes of data to the 
        /// already calculated crc32 value.  This
        /// method can be called as many times as needed
        /// and the Crc32 will continually keep updating.
        /// 
 
        /// 
        /// A byte array of data. 
        ///  
        public
        void 
        AddData(
            byte[]  data
            )
        { 
            _crc32Value = UpdateCRC32(data, _crc32Value);
        } 
 
        #endregion Public methods
 
        #region Private static helper methods

        /// 

        /// Updates the Crc32 value by adding the 
        /// supplied data bytes into the ongoing
        /// calculation. 
        /// 
 
        /// 
        /// The data bytes used to update value. 
        /// 
        /// 
        /// The Crc32 value prior to this data
        /// being added. 
        /// 
        ///  
        /// A new 32-bit unsigned integer representing 
        /// the new Crc32 value.
        ///  
        ///
        /// Critical    - This takes in raw bytes of a bit map
        /// TreatAsSafe - This does not keep the data it just uses it for a calculation.
        /// 
        [SecurityCritical, SecurityTreatAsSafe]
        private 
        static 
        UInt32
        UpdateCRC32( 
            byte[]  data,
            UInt32  oldCrc
            )
        { 
            int bytes = data.Length;
 
            if (bytes == 0) 
                return oldCrc;
 
            UInt32 newCrc = 0;

            for (int n = 0; n < bytes; n += 1)
            { 
                UInt32 idx = (byte)(oldCrc ^ (UInt32)data[n]);
                newCrc = Crc32Table[idx] ^ ((oldCrc >> 8) & 0x00FFFFFF); 
                oldCrc = newCrc; 
            }
 
            return newCrc;
        }

        #endregion Private static helper methods 

        #region Private data 
 
        private UInt32 _crc32Value;
 
        #endregion Private data

        #region Private static data
 
        /// 

        /// The starting Crc32 value. 
        /// 
 
        static
        UInt32 
        Crc32StartValue = 0xFFFFFFFF;

        /// 

        /// The Crc32 precalculated data table. 
        /// 

        static 
        UInt32[] 
        Crc32Table = {
            0x00000000, 0x77073096, 0xee0e612c, 0x990951ba, 0x076dc419, 0x706af48f, 0xe963a535, 0x9e6495a3, 
            0x0edb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988, 0x09b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91,
            0x1db71064, 0x6ab020f2, 0xf3b97148, 0x84be41de, 0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7,
            0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec, 0x14015c4f, 0x63066cd9, 0xfa0f3d63, 0x8d080df5,
            0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172, 0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b, 
            0x35b5a8fa, 0x42b2986c, 0xdbbbc9d6, 0xacbcf940, 0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59,
            0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423, 0xcfba9599, 0xb8bda50f, 
            0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924, 0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d, 
            0x76dc4190, 0x01db7106, 0x98d220bc, 0xefd5102a, 0x71b18589, 0x06b6b51f, 0x9fbfe4a5, 0xe8b8d433,
            0x7807c9a2, 0x0f00f934, 0x9609a88e, 0xe10e9818, 0x7f6a0dbb, 0x086d3d2d, 0x91646c97, 0xe6635c01, 
            0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e, 0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457,
            0x65b0d9c6, 0x12b7e950, 0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65,
            0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2, 0x4adfa541, 0x3dd895d7, 0xa4d1c46d, 0xd3d6f4fb,
            0x4369e96a, 0x346ed9fc, 0xad678846, 0xda60b8d0, 0x44042d73, 0x33031de5, 0xaa0a4c5f, 0xdd0d7cc9, 
            0x5005713c, 0x270241aa, 0xbe0b1010, 0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f,
            0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17, 0x2eb40d81, 0xb7bd5c3b, 0xc0ba6cad, 
            0xedb88320, 0x9abfb3b6, 0x03b6e20c, 0x74b1d29a, 0xead54739, 0x9dd277af, 0x04db2615, 0x73dc1683, 
            0xe3630b12, 0x94643b84, 0x0d6d6a3e, 0x7a6a5aa8, 0xe40ecf0b, 0x9309ff9d, 0x0a00ae27, 0x7d079eb1,
            0xf00f9344, 0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb, 0x196c3671, 0x6e6b06e7, 
            0xfed41b76, 0x89d32be0, 0x10da7a5a, 0x67dd4acc, 0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5,
            0xd6d6a3e8, 0xa1d1937e, 0x38d8c2c4, 0x4fdff252, 0xd1bb67f1, 0xa6bc5767, 0x3fb506dd, 0x48b2364b,
            0xd80d2bda, 0xaf0a1b4c, 0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55, 0x316e8eef, 0x4669be79,
            0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236, 0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f, 
            0xc5ba3bbe, 0xb2bd0b28, 0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31, 0x2cd99e8b, 0x5bdeae1d,
            0x9b64c2b0, 0xec63f226, 0x756aa39c, 0x026d930a, 0x9c0906a9, 0xeb0e363f, 0x72076785, 0x05005713, 
            0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0x0cb61b38, 0x92d28e9b, 0xe5d5be0d, 0x7cdcefb7, 0x0bdbdf21, 
            0x86d3d2d4, 0xf1d4e242, 0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1, 0x18b74777,
            0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c, 0x8f659eff, 0xf862ae69, 0x616bffd3, 0x166ccf45, 
            0xa00ae278, 0xd70dd2ee, 0x4e048354, 0x3903b3c2, 0xa7672661, 0xd06016f7, 0x4969474d, 0x3e6e77db,
            0xaed16a4a, 0xd9d65adc, 0x40df0b66, 0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
            0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605, 0xcdd70693, 0x54de5729, 0x23d967bf,
            0xb3667a2e, 0xc4614ab8, 0x5d681b02, 0x2a6f2b94, 0xb40bbe37, 0xc30c8ea1, 0x5a05df1b, 0x2d02ef8d 
        };
 
        #endregion Private static data 
    }
} 
*/
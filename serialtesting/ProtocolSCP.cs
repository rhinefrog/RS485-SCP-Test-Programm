using System;
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
/*
 void SendSCPStulzCom (task_data_struct *task_data, unsigned char num)
{	// Kommando auf RS485 IO-Bus Senden STX, ESC und ETX hinzufügen
	unsigned char a, dig;

	if (task_data->str_count+1 >= TASK_STR_NUM)
	{
		OS_WriteStringReturn ("SendStulzCom:!!!!");								// ÜÜÜÜÜÜÜÜ
		return;	// Kein Hilfsstring mehr übrig
	}
																															// Benötigt 2 Hilfsstrings !
	task_data->str[task_data->str_count+1][0] = STX;						// STX

	dig = 1;
	for (a = 0 ; a < num ; a++)
	{
		task_data->str[task_data->str_count+1][dig] = 						// Hilfsstring füllen
		task_data->str[task_data->str_count]	[a];

		switch (task_data->str[task_data->str_count+1][dig])
		{
			case STX:
			case ETX:
			case ESC:
				task_data->str[task_data->str_count+1][dig] = ESC;		// ESC einfügen
				dig++;
				task_data->str[task_data->str_count+1][dig] = 				// Original Zeichen wieder ran
				task_data->str[task_data->str_count]	[a];
				break;
		}
		dig++;
	}
				
	task_data->str[task_data->str_count+1][dig] = ETX;					// ETX anfügen
	task_data->str_count++;																			// Stringcounter einen hoch
	SendRS485_2 (task_data, dig+1);
	task_data->str_count--;																			// Stringcounter wieder runter
	OS_Delay (dig+1);																						// Task beenden bis Sendung raus
}
//**************************************************************************************************************

void RS4851SendToken (task_data_struct *task_data, unsigned char empf)				// Sendet Token an Empfänger
{
  unsigned int a, checksumCalc;

	io_bus_com.dyn_conf |= my_long_adress;													// Ich bin drin
	
	if (io_bus_com.al_reset)
	{
//		OS_WriteStringReturn ("save stat 2!!!");
		io_bus_com.stat_conf = io_bus_com.dyn_conf;								// Dynamische Buskonfiguration in statische übernehmen
		WriteRTC (RTC_STAT_BUS_CONF_1, (unsigned char) io_bus_com.stat_conf);			// und speichern
		WriteRTC (RTC_STAT_BUS_CONF_2, (unsigned char) (io_bus_com.stat_conf >> 8));
		WriteRTC (RTC_STAT_BUS_CONF_3, (unsigned char) (io_bus_com.stat_conf >> 16));

		io_bus_com.al_reset = 0;
	}
	
  task_data->str[task_data->str_count][0] = IOBUS_MSG_TOKEN;			// Kommando
  task_data->str[task_data->str_count][1] = empf;									// ID of adressed master
  task_data->str[task_data->str_count][2] = adresse.value;				// meine Bus id
  task_data->str[task_data->str_count][3] = IOBUS_TYPE_C7000IOC;	// Ich bin ein IOC
	task_data->str[task_data->str_count][4] = (UI_8) io_bus_com.dyn_conf;				// Dynamische Konfiguration
  task_data->str[task_data->str_count][5] = (UI_8)(io_bus_com.dyn_conf >> 8);
  task_data->str[task_data->str_count][6] = (UI_8)(io_bus_com.dyn_conf >> 16);
	task_data->str[task_data->str_count][7] = (UI_8) io_bus_com.stat_conf;			// Statische Konfiguration
  task_data->str[task_data->str_count][8] = (UI_8)(io_bus_com.stat_conf >> 8);
  task_data->str[task_data->str_count][9] = (UI_8)(io_bus_com.stat_conf >> 16);

	if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_TOKEN)
	{
		OS_WriteString ("\r\nme>");		// Bus 1 Send Bus Config Announce
		OS_WriteString (LongToString (&NULL_task_data,empf, 2));		// Wandelt ein long in string

		if (mon_timer_b1_level & _MON_LEVEL_TOKEN_DET)
		{
			WriteSpace();
			OS_WriteString (LongToStringBin (&NULL_task_data,io_bus_com.dyn_conf, 20));		// Wandelt ein long in string
			WriteSpace();
			OS_WriteString (LongToStringBin (&NULL_task_data,io_bus_com.stat_conf, 20));		// Wandelt ein long in string
			WriteSpace();
		}

		if (empf == adresse.value)																		// Sende an mich selber
			OS_WriteString ("\r\nToken lost!");
		OS_WriteReturn();
	}

	checksumCalc = 0xFFFF;		// initialize checksum
	for (a = 0 ; a <= 9 ; a++)
		checksumCalc = crc16Add (task_data->str[task_data->str_count][a], checksumCalc);
	task_data->str[task_data->str_count][10] = (UI_8)(checksumCalc) ;	// low byte of checksum
	task_data->str[task_data->str_count][11] = (UI_8)(checksumCalc >> 8) ;	// high byte of checksum

SendRS485_1(task_data, 12); // Senden
io_bus_com.sender = adresse.value;                                              // Ich habe gesendet
io_bus_com.recipient = empf;                                                            // Empfänger eintragen
io_bus_com.i_send_token = 1;                                                            // Eigenen Token faken
OS_Delay(12);																						// Task beenden bis Sendung raus
}
//***************************************************************************************

void RS4851SendNextToken (task_data_struct *task_data)			// Sendet Token oder Ping an den nächsten
{
	unsigned char a;
	unsigned long l;
	
	a = NextPingID();																					// Nächste Pingadresse holen
	if (!io_bus_com.ping_count &&															// Laut Ping Zähler darf ich
			 a != 0xFF)																						// Gibst es auch einen Ping Partner
	{
		io_bus_com.ping_count = 3;															// Ping Zähler wieder hochsetzen
		io_bus_com.next_ping_adr = a;														// Nächste Pingadresse speichern
		io_bus_com.i_send_token = 0;														// Ich habe gerade keinen Token gesendet damit die Ping auswertung funktioniert
		RS4851SendPing (task_data, io_bus_com.next_ping_adr);		// Ping senden
		
		if (RS4851MsgTimeOut (_PINGRESPONSETIME))									// Auf Antwort warten
		{
			if (CheckRS4851Input (task_data, RS485_1_receive_to_buf [RS485_1_receive_to_np]) ==
						IOBUS_MSG_PING_RES &&															// Ping respopnse bekommen
						io_bus_com.sender == io_bus_com.next_ping_adr)		// Absender stimmt auch
			{
				l = (unsigned long) 0x01 << io_bus_com.sender;				

				io_bus_com.dyn_conf |= l;															// Unit zur dynamischen Konfiguration zufügen

				io_bus_com.stat_conf |= l;														// Unit zur statischen Konfiguration zufügen
//				OS_WriteStringReturn ("save stat 3!!!");
				WriteRTC (RTC_STAT_BUS_CONF_1, (unsigned char) io_bus_com.stat_conf);			// Statische Buskonfiguration sichern
				WriteRTC (RTC_STAT_BUS_CONF_2, (unsigned char) (io_bus_com.stat_conf >> 8));
				WriteRTC (RTC_STAT_BUS_CONF_3, (unsigned char) (io_bus_com.stat_conf >> 16));

				unit[io_bus_com.sender].type.value = io_bus_com.type;	// Bustyp eintragen

				RS485_1_receive_nach_pointer = RS485_1_receive_to_buf [RS485_1_receive_to_np++];
				if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;
			}
		}
		a = NextIOID();																						// Nächste Token ID holen
		if (a != 0xFF)																						// Gültige Unit gefunden
			RS4851SendToken (task_data, a);													// Token an nächsten senden
	}
	else																											// Token ausgeben
	{
		if (io_bus_com.ping_count) io_bus_com.ping_count--;			// Ping counter runterzählen

		for (a = 0 ; a <= 19 ; a++)															// Alle möglichen Units durchgehen
		{
			unit[a].erreichb_bitmap <<= 1;												// Erreichbarkeitsbitmap 1 weiterschieen

			if (unit[a].send_token || a == adresse.value)					// Wenn Unit Token gesendet hat oder ich bin es selber
				unit[a].erreichb_bitmap |= 0x01;										// eine 1 rein

			unit[a].erreichb_bitmap &= 0xFFFFF;										// bits 20-31 auf 0 (0x00 bis 0xFFFFFFFF)

			unit[a].send_token = 0;																// Für nächsten durchlauf Merker löschen

			if (!unit[a].erreichb_bitmap)													// Unit nicht mehr erreichbar
			{
				io_bus_com.dyn_conf &= ~((unsigned long) 0x01 << a);	// Aus dynamischer Konfiguration entfernen
																															// Schmeisst sich auch selbst raus -> spielt aber keine Rolle
				if (adresse.value != a)
					unit[a].type.value = IOBUS_TYPE_UNKNOWN;					// Wenn ich es nicht selber bin, Typ löschen
			}
		}

		a = NextIOID();																						// Nächste Token ID holen
		if (a != 0xFF)																						// Gültige Unit gefunden
			RS4851SendToken (task_data, a);													// Token an nächsten senden
	}
}

//***************************************************************************************
void RS4851SendPing (task_data_struct *task_data, unsigned char empf)				// Sendet Ping an Empfänger
{
  unsigned int checksumCalc;

  task_data->str[task_data->str_count][0] = IOBUS_MSG_PING_REQ;			// Kommando
  task_data->str[task_data->str_count][1] = empf;										// ID of adressed master
  task_data->str[task_data->str_count][2] = adresse.value;					// meine Bus id

	if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_TOKEN)
	{
		OS_WriteString ("\r\nme-P1>");
		OS_WriteString (LongToString (&NULL_task_data,empf, 2));		// Wandelt ein long in string
	}

	checksumCalc = 0xFFFF;		// initialize checksum
	checksumCalc = crc16Add (task_data->str[task_data->str_count][0], checksumCalc);
	checksumCalc = crc16Add (task_data->str[task_data->str_count][1], checksumCalc);
	checksumCalc = crc16Add (task_data->str[task_data->str_count][2], checksumCalc);
	task_data->str[task_data->str_count][3] = (UI_8)(checksumCalc) ;	// low byte of checksum
	task_data->str[task_data->str_count][4] = (UI_8)(checksumCalc >> 8) ;	// high byte of checksum

	SendRS485_1 (task_data, 5); // Senden
	OS_Delay (5);																						// Task beenden bis Sendung raus
}

//***************************************************************************************
void RS4851SendPingRes (task_data_struct *task_data, unsigned char empf)				// Antworten auf Ping eines anderen
{
  unsigned int checksumCalc;

  task_data->str[task_data->str_count][0] = IOBUS_MSG_PING_RES;			// Kommando
  task_data->str[task_data->str_count][1] = empf;									// ID of adressed master
  task_data->str[task_data->str_count][2] = adresse.value;				// meine Bus id
  task_data->str[task_data->str_count][3] = IOBUS_TYPE_C7000IOC;		// Bus Typ IOC

	if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_TOKEN)
	{
		OS_WriteString ("\r\nme-P2>");
		OS_WriteString (LongToString (&NULL_task_data,empf, 2));		// Wandelt ein long in string
	}

	checksumCalc = 0xFFFF;		// initialize checksum
	checksumCalc = crc16Add (task_data->str[task_data->str_count][0], checksumCalc);
	checksumCalc = crc16Add (task_data->str[task_data->str_count][1], checksumCalc);
	checksumCalc = crc16Add (task_data->str[task_data->str_count][2], checksumCalc);
	checksumCalc = crc16Add (task_data->str[task_data->str_count][3], checksumCalc);
	task_data->str[task_data->str_count][4] = (UI_8)(checksumCalc) ;	// low byte of checksum
	task_data->str[task_data->str_count][5] = (UI_8)(checksumCalc >> 8) ;	// high byte of checksum

	SendRS485_1 (task_data, 6);
	OS_Delay (6);																						// Task beenden bis Sendung raus
}

//***************************************************************************************
// Diese Funktion wird als Antwort auf ein Data Block Response aufgerufen

void RS4851SendBlockRequestSL (task_data_struct *task_data, unsigned char empf, unsigned char command_id)
{
  unsigned int a, checksumCalc, xB; // xB = Extrabyte

  task_data->str[task_data->str_count][0] = IOBUS_MSG_DATARESP;		// Kommando (Slave -> Master)
  task_data->str[task_data->str_count][1] = empf;									// Empfänger
	task_data->str[task_data->str_count][2] = adresse.value;		// Sender
	task_data->str[task_data->str_count][3] = 11;										// Message länge LB
  task_data->str[task_data->str_count][4] = command_id;	// Kommando
	xB = 0;
	if (command_id == STULZBUS_DATA_BLOCKREQ)
	{
		task_data->str[task_data->str_count][5] = 2;										// Länge für DP Nummer
		xB = 1; 
	}
	task_data->str[task_data->str_count][5+xB] = (UI_8)(io_bus_com.adress & 0xFF);				// low byte of datapoint-adress
	task_data->str[task_data->str_count][6+xB] = (UI_8)(io_bus_com.adress >>8 & 0xFF);		// high byte of datapoint-adress
	task_data->str[task_data->str_count][7+xB] = (UI_8)(io_bus_com.number & 0xFF);				// low byte of datapoint anzahl
	task_data->str[task_data->str_count][8+xB] = (UI_8)(io_bus_com.number >> 8 & 0xFF);	// high byte of datapoint anzahl
	
	if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_MSG)
	{
		OS_WriteString ("\r\nB1 SreqS:");
		OS_WriteString (LongToString (&NULL_task_data,adresse.value, 2));		// Wandelt ein long in string
		WriteGroesser(); // ">"
		OS_WriteString (LongToString (&NULL_task_data,empf, 2));		// Wandelt ein long in string
		WriteDpkt(); // ":"
		OS_WriteString (LongToString (&NULL_task_data,io_bus_com.adress, 5));		// Wandelt ein long in string
		WriteDpkt(); // ":"
		OS_WriteStringReturn (LongToString (&NULL_task_data,io_bus_com.number, 3));		// Wandelt ein long in string
	}

	checksumCalc = 0xFFFF;		// initialize checksum
	for (a = 0 ; a <= (8+xB) ; a++)
		checksumCalc = crc16Add (task_data->str[task_data->str_count][a], checksumCalc);
	task_data->str[task_data->str_count][9+xB] = (UI_8)(checksumCalc & 0xFF) ;	// low byte of checksum
	task_data->str[task_data->str_count][10+xB] = (UI_8)(checksumCalc >> 8 & 0xFF) ;	// high byte of checksum
	SendRS485_1 (task_data, 11+xB);
	OS_Delay (11+xB);																						// Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************
// Diese Funktion wird als Antwort auf ein Data Block Response aufgerufen
// Stulz Bus

void RS4852SendBlockRequestSL (task_data_struct *task_data)
{
  unsigned int a, checksumCalc;

  task_data->str[task_data->str_count][0] = STULZBUS_MSG_DATARESP;				// Kommando
  task_data->str[task_data->str_count][1] = (UI_8)(stulz_bus_com.slave & 0xFF);				// low byte of datapoint-adress
	task_data->str[task_data->str_count][2] = (UI_8)(stulz_bus_com.slave >>8 & 0xFF);		// high byte of datapoint-adress
	task_data->str[task_data->str_count][3] = 13;										// Message länge LB
	task_data->str[task_data->str_count][4] = 0;										// Message länge HB
  task_data->str[task_data->str_count][5] = STULZBUS_DATA_BLOCKREQ;	// Kommando
  task_data->str[task_data->str_count][6] = 2;										// Länge für DP Nummer
	task_data->str[task_data->str_count][7] = (UI_8)(stulz_bus_com.adress & 0xFF);				// low byte of datapoint-adress
	task_data->str[task_data->str_count][8] = (UI_8)(stulz_bus_com.adress >>8 & 0xFF);		// high byte of datapoint-adress
	task_data->str[task_data->str_count][9] = (UI_8)(stulz_bus_com.number & 0xFF);				// low byte of datapoint anzahl
	task_data->str[task_data->str_count][10] = (UI_8)(stulz_bus_com.number >> 8 & 0xFF);	// high byte of datapoint anzahl

	if (mon_timer_b2)
	{
		OS_WriteString ("\r\nB2 SreqS:");
		OS_WriteString (LongToString (&NULL_task_data,stulz_bus_com.slave, 2));		// Wandelt ein long in string
		OS_WriteString (">:");
		OS_WriteString (LongToString (&NULL_task_data,stulz_bus_com.adress, 5));		// Wandelt ein long in string
		WriteDpkt(); // ":"
		OS_WriteStringReturn (LongToString (&NULL_task_data,stulz_bus_com.number, 3));		// Wandelt ein long in string
	}

	checksumCalc = 0xFFFF;		// initialize checksum
	for (a = 0 ; a <= 10 ; a++)
		checksumCalc = crc16Add (task_data->str[task_data->str_count][a], checksumCalc);
	task_data->str[task_data->str_count][11] = (UI_8)(checksumCalc & 0xFF) ;	// low byte of checksum
	task_data->str[task_data->str_count][12] = (UI_8)(checksumCalc >> 8 & 0xFF) ;	// high byte of checksum


SendSCPStulzCom(task_data, 13);	// Senden
}

//***************************************************************************************
//***************************************************************************************
// Diese Funktion wird aufgerufen wenn ich den Inhalt der DP von anderen wissen will

void RS4851SendBlockRequestMA(task_data_struct* task_data, unsigned char empf)
{
	unsigned int a, checksumCalc;

	task_data->str[task_data->str_count][0] = IOBUS_MSG_DATA;               // Kommando
	task_data->str[task_data->str_count][1] = empf;                                 // Empfänger
	task_data->str[task_data->str_count][2] = adresse.value;        // Sender
	task_data->str[task_data->str_count][3] = 11;                                       // Message länge LB
	task_data->str[task_data->str_count][4] = IOBUS_DATA_BLOCKREQ;  // Kommando
	task_data->str[task_data->str_count][5] = (UI_8)(io_bus_com.adress & 0xFF);             // low byte of datapoint-adress
	task_data->str[task_data->str_count][6] = (UI_8)(io_bus_com.adress >> 8 & 0xFF);        // high byte of datapoint-adress
	task_data->str[task_data->str_count][7] = (UI_8)(io_bus_com.number & 0xFF);             // low byte of datapoint anzahl
	task_data->str[task_data->str_count][8] = (UI_8)(io_bus_com.number >> 8 & 0xFF);    // high byte of datapoint anzahl

	if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_MSG)
	{
		OS_WriteString("\r\nB1 SreqM:");
		OS_WriteString(LongToString(&NULL_task_data, adresse.value, 2));        // Wandelt ein long in string
		WriteGroesser(); // ">"
		OS_WriteString(LongToString(&NULL_task_data, empf, 2));     // Wandelt ein long in string
		WriteDpkt(); // ":"
		OS_WriteString(LongToString(&NULL_task_data, io_bus_com.adress, 5));        // Wandelt ein long in string
		WriteDpkt(); // ":"
		OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.number, 3));      // Wandelt ein long in string
	}

	checksumCalc = 0xFFFF;      // initialize checksum
	for (a = 0; a <= 8; a++)
		checksumCalc = crc16Add(task_data->str[task_data->str_count][a], checksumCalc);
	task_data->str[task_data->str_count][9] = (UI_8)(checksumCalc & 0xFF);  // low byte of checksum
	task_data->str[task_data->str_count][10] = (UI_8)(checksumCalc >> 8 & 0xFF);    // high byte of checksum


	SendRS485_1(task_data, 11);
	OS_Delay(11);                                                                                       // Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************
// Diese Funktion wird aufgerufen wenn ich Daten bei anderen ändern will
// Als Antwort erwarte ich ein Data Block Request Slave

void RS4851SendBlockResponseMA(task_data_struct* task_data, unsigned char empf)
{
	unsigned int checksumCalc, i;
	unsigned int iData;                     // counts databytes in datagramm
	unsigned int iAccessbmp;                // counts bytes in access-bitmap datagramm
	unsigned char bitcounter;               // counts bits of a byte
	unsigned char dp_all_num;
	unsigned int byteofbits;                // indexpointer to the byte for the bitfield
	const unsigned char* cp;                            // Charpointer

	task_data->str[task_data->str_count][0] = IOBUS_MSG_DATA;               // Kommando
	task_data->str[task_data->str_count][1] = empf;                                 // Empfänger
	task_data->str[task_data->str_count][2] = adresse.value;                // Sender
																			// Länge wird hier nachgetragen
	task_data->str[task_data->str_count][4] = IOBUS_DATA_BLOCKRES;  // Kommando
	task_data->str[task_data->str_count][5] = (UI_8)(io_bus_com.adress & 0xFF);             // low byte of datapoint-adress
	task_data->str[task_data->str_count][6] = (UI_8)(io_bus_com.adress >> 8 & 0xFF);    // high byte of datapoint-adress
	task_data->str[task_data->str_count][7] = (UI_8)(io_bus_com.number & 0xFF);             // low byte of datapoint anzahl
	task_data->str[task_data->str_count][8] = (UI_8)(io_bus_com.number >> 8 & 0xFF);    // high byte of datapoint anzahl

	if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_MSG)
	{
		OS_WriteString("\r\nB1 SresM:");
		OS_WriteString(LongToString(&NULL_task_data, adresse.value, 2));        // Wandelt ein long in string
		WriteGroesser(); // ">"
		OS_WriteString(LongToString(&NULL_task_data, empf, 2));     // Wandelt ein long in string
		WriteDpkt(); // ":"
		OS_WriteString(LongToString(&NULL_task_data, io_bus_com.adress, 5));        // Wandelt ein long in string
		WriteDpkt(); // ":"
		OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.number, 3));      // Wandelt ein long in string
	}

	iAccessbmp = 9;                 // start of accessibility-bitmap;
	iData = 9 + (io_bus_com.number + 7) / 8;        // start of databytes after accessibility-bitmap;
	bitcounter = 0;
	// init access-bitmap
	for (i = iAccessbmp; i < iData; i++)
	{
		task_data->str[task_data->str_count][i] = 0;
	}

	OS_Use(&DP_all);                                                        // Einen DP all reservieren
	dp_all_num = BlockDPall();
	OS_Unuse(&DP_all);

	for (i = 0; io_bus_com.number > i; i++)
	{
		cp = DPAdr(io_bus_com.adress + i);

		if (cp != NULL)         // DP bekannt
		{
#if DEBUG == 1
OS_WriteString ("-a");		//
#endif
			GetDPnum(cp, dp_all_num);                                                   // Put DP in DPall

			if (*cp & DP_PROP_PHYS_UNIT &&                                          // DP hat physikalische Unit
					DP_ALL[dp_all_num].physUnit == PU_NV) ;                 // physikalische Unit ist üngültig
			else                                                                                                // DP hat keine physikalische Unit oder sie ist gültig
			{                                                                                                       // DP mit access Bit eintregen
				task_data->str[task_data->str_count][iAccessbmp + i / 8] |= 0x01 << (i % 8);    // sets validity in bitmap for transmission

				switch (*cp & 0x07)
				{
					case DT_BIT1:
						if (!bitcounter)
						{
							// we need a new byte :-)
							byteofbits = iData++;
							if (byteofbits < TASK_STR_LEN)
								task_data->str[task_data->str_count][byteofbits] = 0;
						}

						if (byteofbits < TASK_STR_LEN)
							task_data->str[task_data->str_count][byteofbits] |=
									(DP_ALL[dp_all_num].value.UI8 & 0x01) << bitcounter;

						bitcounter++;   // do not reset here!!
						if (bitcounter > 7) bitcounter = 0;
						break;

					case DT_SI8:
					case DT_UI8:
						if (iData < TASK_STR_LEN)
							task_data->str[task_data->str_count][iData++] = DP_ALL[dp_all_num].value.UI8;
						bitcounter = 0;
						break;

					case DT_SI16:
					case DT_UI16:
						if (iData < TASK_STR_LEN)
							task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI16 & 0xFF);
						if (iData < TASK_STR_LEN)
							task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI16 >> 8 & 0xFF);
						bitcounter = 0;
						break;

					case DT_SI32:
					case DT_UI32:
						if (iData < TASK_STR_LEN)
							task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 & 0xFF);
						if (iData < TASK_STR_LEN)
							task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 8 & 0xFF);
						if (iData < TASK_STR_LEN)
							task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 16 & 0xFF);
						if (iData < TASK_STR_LEN)
							task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 24 & 0xFF);
						bitcounter = 0;
						break;

					default:
						bitcounter = 0;
						break;
				}
			}
		}
	}
	RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben

	i = iData + 2;                                                                                          // Richtige länge ausrechnen
	task_data->str[task_data->str_count][3] = (UI_8)(i & 0xFF);                 // Message länge LB

	checksumCalc = 0xFFFF;      // initialize checksum
	for (i = 0; i < iData; i++)
		checksumCalc = crc16Add(task_data->str[task_data->str_count][i], checksumCalc);
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc & 0xFF);    // low byte of checksum
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc >> 8 & 0xFF);   // high byte of checksum

	SendRS485_1(task_data, iData);
	OS_Delay(iData);                                                                                        // Task beenden bis Sendung raus
}

//***************************************************************************************

//***************************************************************************************
// Diese Funktion wird aufgerufen wenn ich auf ein Block request mit meinen DP Antworte

void RS4851SendBlockResponseSL(task_data_struct* task_data, unsigned char empf, unsigned char command_id)
{
	unsigned int checksumCalc, i, xB; // Extrabyte
	unsigned int iData;                     // counts databytes in datagramm
	unsigned int iAccessbmp;                // counts bytes in access-bitmap datagramm
	unsigned char bitcounter;               // counts bits of a byte
	unsigned char dp_all_num;
	unsigned int byteofbits;                // indexpointer to the byte for the bitfield
	const unsigned char* cp;                            // Charpointer

	bitcounter = 0;

	task_data->str[task_data->str_count][0] = IOBUS_MSG_DATARESP;       // Kommando
	task_data->str[task_data->str_count][1] = empf;                                 // Empfänger
	task_data->str[task_data->str_count][2] = adresse.value;        // Sender
																	// Länge wird hier nachgetragen
	task_data->str[task_data->str_count][4] = command_id;   // Kommando
	xB = 0;
	if (command_id == STULZBUS_DATA_BLOCKRES)                                                   // Stulz Bus Kommando mit Länge der Adresse (2)
	{
		task_data->str[task_data->str_count][5] = 2;                                                                            // Länge der Adresse
		xB = 1;
	}
	task_data->str[task_data->str_count][5 + xB] = (UI_8)(io_bus_com.adress & 0xFF);                // low byte of datapoint-adress
	task_data->str[task_data->str_count][6 + xB] = (UI_8)(io_bus_com.adress >> 8 & 0xFF);   // high byte of datapoint-adress
	task_data->str[task_data->str_count][7 + xB] = (UI_8)(io_bus_com.number & 0xFF);                // low byte of datapoint anzahl
	task_data->str[task_data->str_count][8 + xB] = (UI_8)(io_bus_com.number >> 8 & 0xFF);   // high byte of datapoint anzahl
	iAccessbmp = 9 + xB;                                                // start of accessibility-bitmap;
	iData = 9 + xB + (io_bus_com.number + 7) / 8;       // start of databytes after accessibility-bitmap;

	if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_MSG)
	{
		OS_WriteString("\r\nB1 SresS:");
		OS_WriteString(LongToString(&NULL_task_data, adresse.value, 2));        // Wandelt ein long in string
		WriteGroesser(); // ">"
		OS_WriteString(LongToString(&NULL_task_data, empf, 2));     // Wandelt ein long in string
		WriteDpkt(); // ":"
		OS_WriteString(LongToString(&NULL_task_data, io_bus_com.adress, 5));        // Wandelt ein long in string
		WriteDpkt(); // ":"
		OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.number, 3));      // Wandelt ein long in string
	}

	// init access-bitmap
	for (i = iAccessbmp; i < iData; i++)
	{
		task_data->str[task_data->str_count][i] = 0;
	}

	OS_Use(&DP_all);                                                        // Einen DP all reservieren
	dp_all_num = BlockDPall();
	OS_Unuse(&DP_all);

	for (i = 0; io_bus_com.number > i; i++)
	{
		// copy data outof datapoint into pData or return error-message;
		// set bit in success-bitmap
		// Accessibility has to be checked;
		// Type of datapoint has influence on datagram-length!
		// calculate pointer once to avoid repetition of function-calling

		cp = DPAdr(io_bus_com.adress + i);

		if (cp != NULL)         // DP bekannt
		{
#if DEBUG == 1
OS_WriteString ("-b");		//
#endif
			GetDPnum(cp, dp_all_num);                   // Put DP in DPall

			if (*cp & DP_PROP_PHYS_UNIT &&                                          // DP hat physikalische Unit
					DP_ALL[dp_all_num].physUnit == PU_NV)                   // physikalische Unit ist üngültig
			{
			}
			else                                                                                                // DP hat keine physikalische Unit oder sie ist gültig
			{
				if (A_IO_RO & DP_ALL[dp_all_num].access)
				{
					// access allowed
					task_data->str[task_data->str_count][iAccessbmp + i / 8] |= 0x01 << (i % 8);    // sets validity in bitmap for transmission

					// following assignment should be done by separate function, because value is of type union
					//				switch (DPAdr (io_bus_com.adress + i)->dataType)
					switch (*cp & 0x07)
					{
						case DT_BIT1:
							// combine 8 datapoints in a row into one byte;
							// lowest datapoint becomes LSB
							if (!bitcounter)
							{
								// we need a new byte :-)
								byteofbits = iData++;
								if (byteofbits < TASK_STR_LEN)
									task_data->str[task_data->str_count][byteofbits] = 0;
							}
							if (byteofbits < TASK_STR_LEN)
								task_data->str[task_data->str_count][byteofbits] |= (DP_ALL[dp_all_num].value.UI8 & 0x01) << bitcounter;

							bitcounter++;   // do not reset here!!
							if (bitcounter > 7)
							{
								bitcounter = 0;
							}
							break;

						case DT_SI8:
						case DT_UI8:
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = DP_ALL[dp_all_num].value.UI8;
							bitcounter = 0;
							break;

						case DT_SI16:
						case DT_UI16:
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI16 & 0xFF);
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI16 >> 8 & 0xFF);
							bitcounter = 0;
							break;

						//					case DT_FP32:
						case DT_SI32:
						case DT_UI32:
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 & 0xFF);
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 8 & 0xFF);
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 16 & 0xFF);
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 24 & 0xFF);
							bitcounter = 0;
							break;

						default:
							bitcounter = 0;
							break;
					}
				}
			}
		}
	}
	RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben

	i = iData + 2;                                                                                          // Richtige länge ausrechnen
	task_data->str[task_data->str_count][3] = (UI_8)(i & 0xFF);                 // Message länge LB

	checksumCalc = 0xFFFF;      // initialize checksum
	for (i = 0; i < iData; i++)
		checksumCalc = crc16Add(task_data->str[task_data->str_count][i], checksumCalc);
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc & 0xFF);    // low byte of checksum
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc >> 8 & 0xFF);   // high byte of checksum

	SendRS485_1(task_data, iData);
	OS_Delay(iData);                                                                                        // Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************
// Diese Funktion wird aufgerufen wenn ich auf ein Block request mit meinen DP Antworte
// auf Stulz bus

void RS4852SendBlockResponseSL(task_data_struct* task_data)
{
	unsigned int checksumCalc, i;
	unsigned int iData;                     // counts databytes in datagramm
	unsigned int iAccessbmp;                // counts bytes in access-bitmap datagramm
	unsigned int byteofbits;                // indexpointer to the byte for the bitfield
	unsigned char bitcounter;               // counts bits of a byte
	unsigned char dp_all_num;
	const unsigned char* cp;                            // Pointer to struc

	bitcounter = 0;

	task_data->str[task_data->str_count][0] = STULZBUS_MSG_DATARESP;// Kommando
	task_data->str[task_data->str_count][1] = (UI_8)(stulz_bus_com.slave & 0xFF);               // low byte of datapoint-adress
	task_data->str[task_data->str_count][2] = (UI_8)(stulz_bus_com.slave >> 8 & 0xFF);  // high byte of datapoint-adress
																						// Länge wird hier nachgetragen
	task_data->str[task_data->str_count][5] = STULZBUS_DATA_BLOCKRES;   // Kommando
	task_data->str[task_data->str_count][6] = 2;                                        // Länge für DP Nummer
	task_data->str[task_data->str_count][7] = (UI_8)(stulz_bus_com.adress & 0xFF);              // low byte of datapoint-adress
	task_data->str[task_data->str_count][8] = (UI_8)(stulz_bus_com.adress >> 8 & 0xFF); // high byte of datapoint-adress
	task_data->str[task_data->str_count][9] = (UI_8)(stulz_bus_com.number & 0xFF);              // low byte of datapoint anzahl
	task_data->str[task_data->str_count][10] = (UI_8)(stulz_bus_com.number >> 8 & 0xFF);    // high byte of datapoint anzahl

	if (mon_timer_b2)
	{
		OS_WriteString("\r\nB2 SresS:");
		OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));      // Wandelt ein long in string
		OS_WriteString(">:");
		OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.adress, 5));     // Wandelt ein long in string
		WriteDpkt(); // ":"
		OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.number, 3));       // Wandelt ein long in string
	}

	iAccessbmp = 11;                    // start of accessibility-bitmap;
	iData = 11 + (stulz_bus_com.number + 7) / 8;        // start of databytes after accessibility-bitmap;

	// init access-bitmap
	for (i = iAccessbmp; i < iData; i++)
	{
		task_data->str[task_data->str_count][i] = 0;
	}

	OS_Use(&DP_all);                                                        // Einen DP all reservieren
	dp_all_num = BlockDPall();
	OS_Unuse(&DP_all);

	for (i = 0; stulz_bus_com.number > i; i++)
	{
		// copy data outof datapoint into pData or return error-message;
		// set bit in success-bitmap
		// Accessibility has to be checked;
		// Type of datapoint has influence on datagram-length!
		// calculate pointer once to avoid repetition of function-calling

		cp = DPAdr(stulz_bus_com.adress + i);

		if (cp != NULL)         // DP bekannt
		{
#if DEBUG == 1
OS_WriteString ("-c");		//
#endif
			GetDPnum(cp, dp_all_num);                                                               // Put DP in DPall

			if (*cp & DP_PROP_PHYS_UNIT &&                                              // DP hat physikalische Unit
					DP_ALL[dp_all_num].physUnit == PU_NV) ;                     // physikalische Unit ist üngültig
			else                                                                                                    // DP hat keine physikalische Unit oder sie ist gültig
			{
				if (A_IO_RO & DP_ALL[dp_all_num].access)
				{
					// access allowed
					task_data->str[task_data->str_count][iAccessbmp + i / 8] |= 0x01 << (i % 8);    // sets validity in bitmap for transmission

					// following assignment should be done by separate function, because value is of type union
					switch (*cp & 0x07)
					{
						case DT_BIT1:
							// combine 8 datapoints in a row into one byte;
							// lowest datapoint becomes LSB
							if (!bitcounter)
							{
								// we need a new byte :-)
								byteofbits = iData++;
								if (byteofbits < TASK_STR_LEN)
									task_data->str[task_data->str_count][byteofbits] = 0;
							}
							if (byteofbits < TASK_STR_LEN)
								task_data->str[task_data->str_count][byteofbits] |= (DP_ALL[dp_all_num].value.UI8 & 0x01) << bitcounter;

							bitcounter++;   // do not reset here!!
							if (bitcounter > 7)
							{
								bitcounter = 0;
							}
							break;
						case DT_SI8:
						case DT_UI8:
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = DP_ALL[dp_all_num].value.UI8;
							bitcounter = 0;
							break;
						case DT_SI16:
						case DT_UI16:
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI16 & 0xFF);
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI16 >> 8 & 0xFF);
							bitcounter = 0;
							break;
						//					case DT_FP32:
						case DT_SI32:
						case DT_UI32:
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 & 0xFF);
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 8 & 0xFF);
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 16 & 0xFF);
							if (iData < TASK_STR_LEN)
								task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 24 & 0xFF);
							bitcounter = 0;
							break;
						default:
							bitcounter = 0;
							break;
					}
				}
			}
		}
	}

	RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben

	i = iData + 2;                                                                                          // Richtige länge ausrechnen

	task_data->str[task_data->str_count][3] = (UI_8)(i & 0xFF);                 // Message länge LB
	task_data->str[task_data->str_count][4] = (UI_8)(i >> 8 & 0xFF);        // Message länge HB

	checksumCalc = 0xFFFF;      // initialize checksum
	for (i = 0; i < iData; i++)
		checksumCalc = crc16Add(task_data->str[task_data->str_count][i], checksumCalc);
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc & 0xFF);    // low byte of checksum
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc >> 8 & 0xFF);   // high byte of checksum

	//	for (i = 0 ; i < iData ; i++)
	//	{
	//		task_data->str_count++;																			// Stringcounter einen hoch
	//		OS_WriteString (LongToStringHex (task_data,task_data->str[task_data->str_count-1][i], 2));
	//		task_data->str_count--;																			// Stringcounter wieder runter
	//		WriteSpace();
	//	}
	//	OS_WriteReturn ();

	SendSCPStulzCom(task_data, iData);  // Senden
}

//***************************************************************************************
//***************************************************************************************

void RS4852SendAdrResponseSL(task_data_struct* task_data)
{
	unsigned int checksumCalc, i;
	unsigned int iData;                     // counts databytes in datagramm
	unsigned char dp_all_num;
	const unsigned char* cp;                            // Pointer to struc

	task_data->str[task_data->str_count][0] = STULZBUS_MSG_DATARESP;    // Direction
	task_data->str[task_data->str_count][1] = (UI_8)(stulz_bus_com.slave & 0xFF);               // low byte of datapoint-adress
	task_data->str[task_data->str_count][2] = (UI_8)(stulz_bus_com.slave >> 8 & 0xFF);  // high byte of datapoint-adress
																						// Länge wird hier nachgetragen
	task_data->str[task_data->str_count][5] = STULZBUS_DATA_ADRRES; // Addressed response
	task_data->str[task_data->str_count][6] = 2;                                        // Länge für DP Nummer

	if (mon_timer_b2)
	{
		OS_WriteString("\r\nB2 AdrResSl:");
		OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));        // Wandelt ein long in string
																							//		OS_WriteString (">:");
																							//		OS_WriteString (LongToString (&NULL_task_data,stulz_bus_com.adress, 5));		// Wandelt ein long in string
																							//		WriteDpkt(); // ":"
																							//		OS_WriteStringReturn (LongToString (&NULL_task_data,stulz_bus_com.number, 3));		// Wandelt ein long in string
	}

	iData = 7;                                                                                              // Hier beginnt die erste Adresse

	if (stulz_bus_com.address_num)                                                      // Sind überhaupt Adressen angegeben
	{
		OS_Use(&DP_all);                                                        // Einen DP all reservieren
		dp_all_num = BlockDPall();
		OS_Unuse(&DP_all);

		for (i = 0; stulz_bus_com.address_num > i; i++)             // Alle Adressen durchgehen
		{
			cp = DPAdr(stulz_bus_com.adress2[i]);                               // DP Adresse holen

			if (cp != NULL)                                                                             // DP bekannt
			{
#if DEBUG == 1
OS_WriteString ("-d");		//
#endif
				GetDPnum(cp, dp_all_num);                                                   // Put DP in DPall

				if (*cp & DP_PROP_PHYS_UNIT &&                                          // DP hat physikalische Unit
					DP_ALL[dp_all_num].physUnit == PU_NV) ;                     // physikalische Unit ist üngültig
				else                                                                                                // DP hat keine physikalische Unit oder sie ist gültig
				{
					if (A_IO_RO & DP_ALL[dp_all_num].access)                    // Ist der DP Leseberechtigt
					{
						switch (*cp & 0x07)                                                         // DP Typ unterscheidung
						{
							case DT_BIT1:
							case DT_SI8:
							case DT_UI8:
								if (iData < TASK_STR_LEN)                                       // Adresse davorschreiben
									task_data->str[task_data->str_count][iData++] = (UI_8)(stulz_bus_com.adress2[i] & 0xFF);                // low byte of datapoint-adress
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(stulz_bus_com.adress2[i] >> 8 & 0xFF);   // high byte of datapoint-adress
								if (iData < TASK_STR_LEN)                                       // Daten
									task_data->str[task_data->str_count][iData++] = DP_ALL[dp_all_num].value.UI8;
								break;

							case DT_SI16:
							case DT_UI16:
								if (iData < TASK_STR_LEN)                                       // Adresse davorschreiben
									task_data->str[task_data->str_count][iData++] = (UI_8)(stulz_bus_com.adress2[i] & 0xFF);                // low byte of datapoint-adress
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(stulz_bus_com.adress2[i] >> 8 & 0xFF);   // high byte of datapoint-adress
								if (iData < TASK_STR_LEN)                                       // Daten
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI16 & 0xFF);
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI16 >> 8 & 0xFF);
								break;

							case DT_SI32:
							case DT_UI32:
								if (iData < TASK_STR_LEN)                                       // Adresse davorschreiben
									task_data->str[task_data->str_count][iData++] = (UI_8)(stulz_bus_com.adress2[i] & 0xFF);                // low byte of datapoint-adress
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(stulz_bus_com.adress2[i] >> 8 & 0xFF);   // high byte of datapoint-adress
								if (iData < TASK_STR_LEN)                                       // Daten
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 & 0xFF);
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 8 & 0xFF);
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 16 & 0xFF);
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 24 & 0xFF);
								break;

							default:
								break;
						}   // switch (*cp & 0x07)
					}       // if (A_IO_RO & DP_ALL[dp_all_num].access)
				}           // else

			}               // if (cp != NULL)
		}                   // for (i = 0 ; stulz_bus_com.address_num > i ; i++)
		RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
	}                       // if (stulz_bus_com.address_num)
	i = iData + 2;                                                                                          // Richtige länge ausrechnen

	task_data->str[task_data->str_count][3] = (UI_8)(i & 0xFF);                 // Message länge LB
	task_data->str[task_data->str_count][4] = (UI_8)(i >> 8 & 0xFF);        // Message länge HB

	checksumCalc = 0xFFFF;      // initialize checksum
	for (i = 0; i < iData; i++)
		checksumCalc = crc16Add(task_data->str[task_data->str_count][i], checksumCalc);
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc & 0xFF);    // low byte of checksum
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc >> 8 & 0xFF);   // high byte of checksum

	//	for (i = 0 ; i < iData ; i++)
	//	{
	//		task_data->str_count++;																			// Stringcounter einen hoch
	//		OS_WriteString (LongToStringHex (task_data,task_data->str[task_data->str_count-1][i], 2));
	//		task_data->str_count--;																			// Stringcounter wieder runter
	//		WriteSpace();
	//	}
	//	OS_WriteReturn ();

	SendSCPStulzCom(task_data, iData);  // Senden
}

//***************************************************************************************
//***************************************************************************************

void RS4851SendAdrResponseSL(task_data_struct* task_data, unsigned char empf, unsigned char command_id)
{
	unsigned int checksumCalc, i;
	unsigned int iData;                     // counts databytes in datagramm
	unsigned char dp_all_num;
	const unsigned char* cp;                            // Pointer to struc

	task_data->str[task_data->str_count][0] = IOBUS_MSG_DATARESP;           // Slave -> Master
	task_data->str[task_data->str_count][1] = empf;                                     // Empfänger
	task_data->str[task_data->str_count][2] = adresse.value;                    // Sender
																				// Länge wird hier nachgetragen
	task_data->str[task_data->str_count][4] = command_id;   // Addressed response mit alten oder neuer Command ID

	if (command_id == STULZBUS_DATA_ADRRES)
	{
		task_data->str[task_data->str_count][5] = 2;                            // Länge der Adressen
		iData = 6;                                                                                              // Hier beginnt die erste Adresse
	}
	else
	{
		iData = 5;                                                                                              // Hier beginnt die erste Adresse
	}

	if (mon_timer_b2)
	{
		OS_WriteString("\r\nB1 AdrResSl:");
		OS_WriteString(LongToString(&NULL_task_data, adresse.value, 2));        // Wandelt ein long in string
		WriteGroesser(); // ">"
		OS_WriteStringReturn(LongToString(&NULL_task_data, empf, 2));       // Wandelt ein long in string
																			//		OS_WriteString (">:");
																			//		OS_WriteString (LongToString (&NULL_task_data,io_bus_com.adress, 5));		// Wandelt ein long in string
																			//		WriteDpkt(); // ":"
																			//		OS_WriteStringReturn (LongToString (&NULL_task_data,io_bus_com.number, 3));		// Wandelt ein long in string
	}

	if (io_bus_com.address_num)                                                     // Sind überhaupt Adressen angegeben
	{
		OS_Use(&DP_all);                                                        // Einen DP all reservieren
		dp_all_num = BlockDPall();
		OS_Unuse(&DP_all);

		for (i = 0; io_bus_com.address_num > i; i++)                // Alle Adressen durchgehen
		{
			cp = DPAdr(io_bus_com.adress2[i]);                              // DP Adresse holen

			if (cp != NULL)                                                                             // DP bekannt
			{
#if DEBUG == 1
OS_WriteString ("-e");		//
#endif
				GetDPnum(cp, dp_all_num);                                                   // Put DP in DPall

				if (*cp & DP_PROP_PHYS_UNIT &&                                          // DP hat physikalische Unit
					DP_ALL[dp_all_num].physUnit == PU_NV) ;                     // physikalische Unit ist üngültig
				else                                                                                                // DP hat keine physikalische Unit oder sie ist gültig
				{
					if (A_IO_RO & DP_ALL[dp_all_num].access)                    // Ist der DP Leseberechtigt
					{
						switch (*cp & 0x07)                                                         // DP Typ unterscheidung
						{
							case DT_BIT1:
							case DT_SI8:
							case DT_UI8:
								if (iData < TASK_STR_LEN)                                       // Adresse davorschreiben
									task_data->str[task_data->str_count][iData++] = (UI_8)(io_bus_com.adress2[i] & 0xFF);               // low byte of datapoint-adress
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(io_bus_com.adress2[i] >> 8 & 0xFF);  // high byte of datapoint-adress
								if (iData < TASK_STR_LEN)                                       // Daten
									task_data->str[task_data->str_count][iData++] = DP_ALL[dp_all_num].value.UI8;
								break;

							case DT_SI16:
							case DT_UI16:
								if (iData < TASK_STR_LEN)                                       // Adresse davorschreiben
									task_data->str[task_data->str_count][iData++] = (UI_8)(io_bus_com.adress2[i] & 0xFF);               // low byte of datapoint-adress
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(io_bus_com.adress2[i] >> 8 & 0xFF);  // high byte of datapoint-adress
								if (iData < TASK_STR_LEN)                                       // Daten
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI16 & 0xFF);
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI16 >> 8 & 0xFF);
								break;

							case DT_SI32:
							case DT_UI32:
								if (iData < TASK_STR_LEN)                                       // Adresse davorschreiben
									task_data->str[task_data->str_count][iData++] = (UI_8)(io_bus_com.adress2[i] & 0xFF);               // low byte of datapoint-adress
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(io_bus_com.adress2[i] >> 8 & 0xFF);  // high byte of datapoint-adress
								if (iData < TASK_STR_LEN)                                       // Daten
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 & 0xFF);
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 8 & 0xFF);
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 16 & 0xFF);
								if (iData < TASK_STR_LEN)
									task_data->str[task_data->str_count][iData++] = (UI_8)(DP_ALL[dp_all_num].value.UI32 >> 24 & 0xFF);
								break;

							default:
								break;
						}   // switch (*cp & 0x07)
					}       // if (A_IO_RO & DP_ALL[dp_all_num].access)
				}           // else

			}               // if (cp != NULL)
		}                   // for (i = 0 ; io_bus_com.address_num > i ; i++)
		RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
	}                       // if (io_bus_com.address_num)
	i = iData + 2;                                                                                          // Richtige länge ausrechnen

	task_data->str[task_data->str_count][3] = (UI_8)(i & 0xFF);                 // Message länge LB

	checksumCalc = 0xFFFF;      // initialize checksum
	for (i = 0; i < iData; i++)
		checksumCalc = crc16Add(task_data->str[task_data->str_count][i], checksumCalc);
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc & 0xFF);    // low byte of checksum
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc >> 8 & 0xFF);   // high byte of checksum

	//	for (i = 0 ; i < iData ; i++)
	//	{
	//		task_data->str_count++;																			// Stringcounter einen hoch
	//		OS_WriteString (LongToStringHex (task_data,task_data->str[task_data->str_count-1][i], 2));
	//		task_data->str_count--;																			// Stringcounter wieder runter
	//		WriteSpace();
	//	}
	//	OS_WriteReturn ();

	SendRS485_1(task_data, iData);
	OS_Delay(iData);                                                                                        // Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************

void RS4852SendAdrRequestSL(task_data_struct* task_data)
{
	unsigned int checksumCalc, i;
	unsigned int iData;                     // counts databytes in datagramm

	task_data->str[task_data->str_count][0] = STULZBUS_MSG_DATARESP;    // Direction
	task_data->str[task_data->str_count][1] = (UI_8)(stulz_bus_com.slave & 0xFF);               // low byte of datapoint-adress
	task_data->str[task_data->str_count][2] = (UI_8)(stulz_bus_com.slave >> 8 & 0xFF);  // high byte of datapoint-adress
																						// Länge wird hier nachgetragen
	task_data->str[task_data->str_count][5] = STULZBUS_DATA_ADRREQ; // Addressed response
	task_data->str[task_data->str_count][6] = 2;                                        // Länge für DP Nummer

	if (mon_timer_b2)
	{
		OS_WriteString("\r\nB2 AdrReqSl:");
		OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));        // Wandelt ein long in string
																							//		OS_WriteString (">:");
																							//		OS_WriteString (LongToString (&NULL_task_data,stulz_bus_com.adress, 5));		// Wandelt ein long in string
																							//		WriteDpkt(); // ":"
																							//		OS_WriteStringReturn (LongToString (&NULL_task_data,stulz_bus_com.number, 3));		// Wandelt ein long in string
	}

	iData = 7;                                                                                              // Hier beginnt die erste Adresse

	if (stulz_bus_com.address_num)                                                      // Sind überhaupt Adressen angegeben
	{
		for (i = 0; stulz_bus_com.address_num > i; i++)             // Alle Adressen durchgehen
		{
			if (iData < TASK_STR_LEN)                                       // Adresse davorschreiben
				task_data->str[task_data->str_count][iData++] = (UI_8)(stulz_bus_com.adress2[i] & 0xFF);                // low byte of datapoint-adress
			if (iData < TASK_STR_LEN)
				task_data->str[task_data->str_count][iData++] = (UI_8)(stulz_bus_com.adress2[i] >> 8 & 0xFF);   // high byte of datapoint-adress
		}                   // for (i = 0 ; stulz_bus_com.address_num > i ; i++)
	}                       // if (stulz_bus_com.address_num)
	i = iData + 2;                                                                                          // Richtige länge ausrechnen

	task_data->str[task_data->str_count][3] = (UI_8)(i & 0xFF);                 // Message länge LB
	task_data->str[task_data->str_count][4] = (UI_8)(i >> 8 & 0xFF);        // Message länge HB

	checksumCalc = 0xFFFF;      // initialize checksum
	for (i = 0; i < iData; i++)
		checksumCalc = crc16Add(task_data->str[task_data->str_count][i], checksumCalc);
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc & 0xFF);    // low byte of checksum
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc >> 8 & 0xFF);   // high byte of checksum

	//	for (i = 0 ; i < iData ; i++)
	//	{
	//		task_data->str_count++;																			// Stringcounter einen hoch
	//		OS_WriteString (LongToStringHex (task_data,task_data->str[task_data->str_count-1][i], 2));
	//		task_data->str_count--;																			// Stringcounter wieder runter
	//		WriteSpace();
	//	}
	//	OS_WriteReturn ();

	SendSCPStulzCom(task_data, iData);  // Senden
}

//***************************************************************************************
//***************************************************************************************

void RS4851SendAdrRequestSL(task_data_struct* task_data, unsigned char empf, unsigned char command_id)
{
	unsigned int checksumCalc, i;
	unsigned int iData;                     // counts databytes in datagramm

	task_data->str[task_data->str_count][0] = IOBUS_MSG_DATARESP;   // Direction
	task_data->str[task_data->str_count][1] = empf;                                     // Empfänger
	task_data->str[task_data->str_count][2] = adresse.value;                    // Sender
																				// Länge wird hier nachgetragen
	task_data->str[task_data->str_count][4] = command_id;   // Addressed request

	if (command_id == STULZBUS_DATA_ADRREQ)
	{
		task_data->str[task_data->str_count][5] = 2;                            // Länge der Adressen
		iData = 6;                                                                                              // Hier beginnt die erste Adresse
	}
	else
	{
		iData = 5;                                                                                              // Hier beginnt die erste Adresse
	}

	if (mon_timer_b2)
	{
		OS_WriteString("\r\nB2 AdrReqSl:");
		OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));        // Wandelt ein long in string
																							//		OS_WriteString (">:");
																							//		OS_WriteString (LongToString (&NULL_task_data,stulz_bus_com.adress, 5));		// Wandelt ein long in string
																							//		WriteDpkt(); // ":"
																							//		OS_WriteStringReturn (LongToString (&NULL_task_data,stulz_bus_com.number, 3));		// Wandelt ein long in string
	}

	if (stulz_bus_com.address_num)                                                      // Sind überhaupt Adressen angegeben
	{
		for (i = 0; stulz_bus_com.address_num > i; i++)             // Alle Adressen durchgehen
		{
			if (iData < TASK_STR_LEN)                                       // Adresse davorschreiben
				task_data->str[task_data->str_count][iData++] = (UI_8)(stulz_bus_com.adress2[i] & 0xFF);                // low byte of datapoint-adress
			if (iData < TASK_STR_LEN)
				task_data->str[task_data->str_count][iData++] = (UI_8)(stulz_bus_com.adress2[i] >> 8 & 0xFF);   // high byte of datapoint-adress
		}                   // for (i = 0 ; stulz_bus_com.address_num > i ; i++)
	}                       // if (stulz_bus_com.address_num)
	i = iData + 2;                                                                                          // Richtige länge ausrechnen

	task_data->str[task_data->str_count][3] = (UI_8)(i & 0xFF);                 // Message länge LB

	checksumCalc = 0xFFFF;      // initialize checksum
	for (i = 0; i < iData; i++)
		checksumCalc = crc16Add(task_data->str[task_data->str_count][i], checksumCalc);
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc & 0xFF);    // low byte of checksum
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc >> 8 & 0xFF);   // high byte of checksum

	//	for (i = 0 ; i < iData ; i++)
	//	{
	//		task_data->str_count++;																			// Stringcounter einen hoch
	//		OS_WriteString (LongToStringHex (task_data,task_data->str[task_data->str_count-1][i], 2));
	//		task_data->str_count--;																			// Stringcounter wieder runter
	//		WriteSpace();
	//	}
	//	OS_WriteReturn ();

	SendRS485_1(task_data, iData);
	OS_Delay(iData);                                                                                        // Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************

void FindVarDPStart(void)       // Findet Anfang und Ende der bit und analog varDP
{
	unsigned int a;
	unsigned char* cp;                          // Pointer to struc

	stulz_bus_com.first_vardp_bit = 0;                          // Alles auf Anfang
	stulz_bus_com.last_vardp_bit = CACHE_VARDP_START;
	stulz_bus_com.first_vardp_ana = 0;
	stulz_bus_com.last_vardp_ana = CACHE_VARDP_START;
	stulz_bus_com.last_vardp = CACHE_VARDP_START;

	for (a = CACHE_VARDP_START; a < CACHE_VARDP_END; a++)               // Ganzen Var DP Bereich durchgehen
	{
		if (var_dp[a - CACHE_VARDP_START].value != a)                                   // Var DP eingetragen
		{
			stulz_bus_com.last_vardp = a + 1;                                               // Als letzten Var DP merken

			cp = DPAdr(a);
			if (cp != NULL)                                                                             // DP ist bekannt
			{
				if ((*cp & 0x07) == DT_BIT1)                                                // Bit DP gefunden
				{
					if (!stulz_bus_com.first_vardp_bit)                             // Ersten Bit varDP gefunden
						stulz_bus_com.first_vardp_bit = a;
					stulz_bus_com.last_vardp_bit = a + 1;                                   // Letzten bit varDP eingetragen
				}
				else                                                                                                // Analog DP gefunden
				{
					if (!stulz_bus_com.first_vardp_ana)                             // Ersten Bit varDP gefunden
						stulz_bus_com.first_vardp_ana = a;
					stulz_bus_com.last_vardp_ana = a + 1;                                   // Letzten bit varDP eingetragen
				}
			}
		}
	}
	if (!stulz_bus_com.first_vardp_bit) stulz_bus_com.first_vardp_bit = CACHE_VARDP_START;  // Kein bit varDP gefunden
	if (!stulz_bus_com.first_vardp_ana) stulz_bus_com.first_vardp_ana = CACHE_VARDP_START;  // Kein ana varDP gefunden
}

//***************************************************************************************
//***************************************************************************************

void SendModbusError(task_data_struct* task_data, unsigned int slave, unsigned char err)    // Sendet einen Modbus Fehler
{
	unsigned int checksumCalc;

	task_data->str[task_data->str_count][0] = (UI_8)(slave);    // Meine globale Adresse wurde angesprochen
	task_data->str[task_data->str_count][1] = 0x80 | err;           // error & command
	task_data->str[task_data->str_count][2] = err;                      // error type

	// checksum calculation
	checksumCalc = 0xFFFF;
	checksumCalc = crc16Add2(task_data->str[task_data->str_count][0], checksumCalc);
	checksumCalc = crc16Add2(task_data->str[task_data->str_count][1], checksumCalc);
	checksumCalc = crc16Add2(task_data->str[task_data->str_count][2], checksumCalc);
	task_data->str[task_data->str_count][3] = (UI_8)(checksumCalc & 0xFF);  // low byte of checksum
	task_data->str[task_data->str_count][4] = (UI_8)(checksumCalc >> 8 & 0xFF); // high byte of checksum

	if (mon_timer_b2)
	{
		OS_WriteString("\r\nMod 1 res: ERROR ");
		OS_WriteStringReturn(LongToStringHex(&NULL_task_data, err, 2));
	}

	SendRS485_2(task_data, 5);                                                      // Daten Senden
	OS_Delay(5);                                                                                    // Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************

void RS4852SendModbus12Response(task_data_struct* task_data, unsigned char fkt)     // Sendet Antwort auf Modbus Funktion 1 Anfrage
{
	unsigned int a, checksumCalc, adr;
	unsigned char* cp;                          // Pointer to struc
	unsigned int iData;                     // counts databytes in datagramm
	unsigned char dp_all_num;
	unsigned char bitcounter;               // counts bits of a byte

	task_data->str[task_data->str_count][0] = (UI_8)(stulz_bus_com.slave);  // Meine globale Adresse wurde angesprochen
	task_data->str[task_data->str_count][1] = fkt - 10;                                             // Modbus Function 1 oder 2
	task_data->str[task_data->str_count][2] = (UI_8)((stulz_bus_com.modbus_datnum / 8) +
																									 (stulz_bus_com.modbus_datnum % 8 != 0));   // Anzahl der Bytes
	iData = 3;                                                                                              // Start der Daten im Telegramm
	bitcounter = 0;

	OS_Use(&DP_all);                                                                                // Einen DP all reservieren
	dp_all_num = BlockDPall();
	OS_Unuse(&DP_all);

	for (adr = stulz_bus_com.modbus_datstart; adr < stulz_bus_com.modbus_datstart + stulz_bus_com.modbus_datnum; adr++)
	{
		switch (datapointlist.value)                                                    // Welches Modbus Protokoll
		{
			case 1:                                                                                         // Modbus classic
				if (fkt == MODBUS_FUNCTION_1_READ_COIL_STATUS)      // Function 1
				{
					if (adr >= MODBUS_LAST_MAPPED_DP_1)
					{
						SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
						RELEASEDPALL(dp_all_num);                                           // DPall wieder freigeben
						return;
					}
					if (mod_func_1_map[adr] == -1)                                  // Datenpunkt bekannt aber hier nicht relevant
						cp = &null_bit_dp.dataType;                                     // Dieser DP ist 0, z.B. Modul 6, Kompressor 2
					else
						cp = DPAdr(mod_func_1_map[adr]);                            // Pointer auf DP holen
				}
				else                                                                                            // Function 2
				{
					if (adr >= MODBUS_LAST_MAPPED_DP_2)
					{
						SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
						RELEASEDPALL(dp_all_num);                                           // DPall wieder freigeben
						return;
					}
					if (mod_func_2_map[adr] == -1)                                  // Datenpunkt bekannt aber hier nicht relevant
						cp = &null_bit_dp.dataType;                                     // Dieser DP ist 0, z.B. Modul 6, Kompressor 2
					else
						cp = DPAdr(mod_func_2_map[adr]);                            // Pointer auf DP holen
				}
				break;

			case 2:                                                                                         // Modbus Full size
				cp = DPAdr(adr);
				break;

			case 3:                                                                                         // Modbus Custom
				if (adr >=                                                                              // Letzte Adresse ist grösser als bit varDP liste
						stulz_bus_com.last_vardp_bit - stulz_bus_com.first_vardp_bit)
				{
					SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
					RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
					return;
				}

				cp = DPAdr(var_dp[stulz_bus_com.first_vardp_bit - DP_VAR_START + adr].value);
				break;

			default:                                                                                // Keine gültige Datenpunktliste eingestellt
				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_DEVICE_FAIL);    // Sendet einen Modbus Fehler
				RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
				return;
		}

		if (cp == NULL)                                                                                 // DP ist unbekannt
		{
			SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
			RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
			return;
		}

		if (!bitcounter)                                                                                // Bit 0 ist dran -> ganzes Byte auf 0
			if (iData < TASK_STR_LEN)
				task_data->str[task_data->str_count][iData] = 0;

		if ((*cp & 0x07) == DT_BIT1)                                                        // DP ist ein Bit, ein muss bei Funktion 1, 2
		{
#if DEBUG == 1
OS_WriteString ("-f");		//
#endif
			GetDPnum(cp, dp_all_num);                                                       // Put DP in DPall

			if (!(A_IO_RO & DP_ALL[dp_all_num].access))                     // Zugriff verboten
			{
				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
				RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
				return;
			}

			if (!bitcounter)
				if (iData < TASK_STR_LEN)
					task_data->str[task_data->str_count][iData] = 0;

			if (DP_ALL[dp_all_num].value.UI8)                                   // Bit DP ist gesetzt
				if (iData < TASK_STR_LEN)
					task_data->str[task_data->str_count][iData] |= (1 << bitcounter);
		}
		else                                                                                                        // DP ist ein Analogwert
		{
			SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
			RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
			return;
		}

		bitcounter++;
		if (bitcounter >= 8)
		{
			bitcounter = 0;
			iData++;
		}
	}                               // for (adr = 
	RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben

	if (bitcounter) iData++;                                                                    // Byte ist angebrochen -> nächstes Byte

	// checksum calculation
	checksumCalc = 0xFFFF;
	for (a = 0; a < iData; a++)
		checksumCalc = crc16Add2(task_data->str[task_data->str_count][a], checksumCalc);
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc & 0xFF);    // low byte of checksum
	if (iData < TASK_STR_LEN)
		task_data->str[task_data->str_count][iData] = (UI_8)(checksumCalc >> 8 & 0xFF); // high byte of checksum

	if (mon_timer_b2)
	{
		OS_WriteString("\r\nMod 1 res:");
		for (a = 0; a <= iData; a++)
		{
			task_data->str_count++;                                                                         // Stringcounter einen hoch
			OS_WriteString(LongToStringHex(&NULL_task_data, task_data->str[task_data->str_count - 1][a], 2));
			task_data->str_count--;                                                                         // Stringcounter wieder runter
			WriteSpace();
		}
		OS_WriteReturn();
	}

	SendRS485_2(task_data, iData + 1);                                                      // Daten Senden
	OS_Delay(iData + 1);                                                                                    // Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************

//brief This function converts a datapoint to float format
float genpurpConvToFloat(unsigned int dp_all_num, unsigned char* cp)
{
	float valuefloat;

	switch (*cp & 0x07)
	{
		case DT_BIT1:
		case DT_UI8:
			valuefloat = (float)DP_ALL[dp_all_num].value.UI8;
			break;
		case DT_SI8:
			valuefloat = (float)DP_ALL[dp_all_num].value.SI8;
			break;
		case DT_UI16:
			valuefloat = (float)DP_ALL[dp_all_num].value.UI16;
			break;
		case DT_SI16:
			valuefloat = (float)DP_ALL[dp_all_num].value.SI16;
			break;
		case DT_UI32:
			valuefloat = (float)DP_ALL[dp_all_num].value.UI32;
			break;
		case DT_SI32:
			valuefloat = (float)DP_ALL[dp_all_num].value.SI32;
			break;
		default:
			// case not expected
			valuefloat = 0.0;
			break;
	} // switch

	return valuefloat;
}

//***************************************************************************************
//***************************************************************************************

void RS4852SendModbus34Response(task_data_struct* task_data, unsigned char fkt)     // Sendet Antwort auf Modbus Funktion 3, 4 Anfrage
{
	unsigned int a, checksumCalc, adr;
	unsigned char* cp;                          // Pointer to struc
	unsigned int iData;                     // counts databytes in datagramm
	unsigned char dp_all_num;
	//	unsigned int	i;								// counts datapoints
	float valueFloat;           // value of datapoint converted to float for transmission;

	// odd address or odd number of words to read
	if (stulz_bus_com.modbus_datstart & 1 || stulz_bus_com.modbus_datnum & 1)
	{
		SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
		return;
	}

	task_data->str[task_data->str_count][0] = (UI_8)(stulz_bus_com.slave);  // Meine globale Adresse wurde angesprochen
	task_data->str[task_data->str_count][1] = fkt - 10;                                             // Modbus Function 3 oder 4
	task_data->str[task_data->str_count][2] = (UI_8)(2 * stulz_bus_com.modbus_datnum);

	iData = 3;                                                                                              // Start der Daten im Telegramm
	adr = 0;

	OS_Use(&DP_all);                                                                                // Einen DP all reservieren
	dp_all_num = BlockDPall();
	OS_Unuse(&DP_all);

	for (adr = stulz_bus_com.modbus_datstart / 2; adr < stulz_bus_com.modbus_datstart / 2 + stulz_bus_com.modbus_datnum / 2; adr++)
	{
		switch (datapointlist.value)                                                        // Welches Modbus Protokoll
		{
			case 1:                                                                                             // Modbus classic
				if (fkt == MODBUS_FUNCTION_3_READ_HOLDING_REGISTER) // Function 3
				{
					if (adr >= MODBUS_LAST_MAPPED_DP_3_16)
					{
						SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
						RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
						return;
					}
					if (mod_func_3_16_map[adr] == -1)                                   // Datenpunkt bekannt, aber hier nicht relevant
						cp = &null_byte_dp.dataType;                                        // Dieser DP ist 0, z.B. Modul 6, Kompressor 2
					else
						cp = DPAdr(mod_func_3_16_map[adr]);                     // Pointer auf DP holen
				}
				else                                                                                                // Function 4
				{
					if (adr >= MODBUS_LAST_MAPPED_DP_4)
					{
						SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
						RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
						return;
					}
					if (mod_func_4_map[adr] == -1)                                      // Datenpunkt bekannt, aber hier nicht relevant
						cp = &null_byte_dp.dataType;                                        // Dieser DP ist 0, z.B. Modul 6, Kompressor 2
					else
						cp = DPAdr(mod_func_4_map[adr]);                                // Pointer auf DP holen
				}
				break;

			case 2:                                                                                             // Modbus Full size
				cp = DPAdr(adr);
				break;

			case 3:                                                                                             // Modbus Custom
				if (stulz_bus_com.first_vardp_ana + adr >=
						stulz_bus_com.last_vardp)
				{
					SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
					RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
					return;
				}

				cp = DPAdr(stulz_bus_com.first_vardp_ana + adr);
				break;

			default:                                                                                            // Keine gültige Datenpunktliste eingestellt
				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_DEVICE_FAIL);    // Sendet einen Modbus Fehler
				RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
				return;
		}

		if (cp == NULL)                                                                                 // Wenn DP unbekannt, Fehler
		{
			SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
			RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
			return;
		}

		if ((*cp & 0x07) != DT_BIT1)                                                        // DP ist kein Bit, ein muss bei Funktion 3, 4
		{
#if DEBUG == 1
OS_WriteString ("-g");		//
#endif
			GetDPnum(cp, dp_all_num);                                                       // Put DP in DPall

			if (!(A_IO_RO & DP_ALL[dp_all_num].access))                     // Zugriff verboten
			{
				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
				RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
				return;
			}

			if (DP_ALL[dp_all_num].physUnit == PU_NV)                           // DP bekannt aber gerade ungültig
			{
				valueFloat = 0;
			}
			else
			{
				valueFloat = genpurpConvToFloat(dp_all_num, cp);        // DP nach Float holen
				valueFloat /= GetPhysUnitFactor(DP_ALL[dp_all_num].physUnit);
			}

			if (iData < TASK_STR_LEN)
				task_data->str[task_data->str_count][iData++] = *(((unsigned char*)&valueFloat)+3);
if (iData < TASK_STR_LEN)
	task_data->str[task_data->str_count][iData++] = *(((unsigned char*)&valueFloat)+2);
if (iData < TASK_STR_LEN)
	task_data->str[task_data->str_count][iData++] = *(((unsigned char*)&valueFloat)+1);
if (iData < TASK_STR_LEN)
	task_data->str[task_data->str_count][iData++] = *(((unsigned char*)&valueFloat)  );
		}
		else                                                                                                        // DP ist ein Analogwert
{
	SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
	RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
	return;
}
	}								// for (i = 0 ; i < stulz_bus_com.modbus_datnum ; i += 2)

	RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben

// checksum calculation
checksumCalc = 0xFFFF;
for (a = 0; a < iData; a++)
	checksumCalc = crc16Add2(task_data->str[task_data->str_count][a], checksumCalc);
if (iData < TASK_STR_LEN)
	task_data->str[task_data->str_count][iData++] = (UI_8)(checksumCalc & 0xFF);    // low byte of checksum
if (iData < TASK_STR_LEN)
	task_data->str[task_data->str_count][iData] = (UI_8)(checksumCalc >> 8 & 0xFF); // high byte of checksum

if (mon_timer_b2)
{
	OS_WriteString("\r\nMod 1 res:");
	for (a = 0; a <= iData; a++)
	{
		task_data->str_count++;                                                                         // Stringcounter einen hoch
		OS_WriteString(LongToStringHex(&NULL_task_data, task_data->str[task_data->str_count - 1][a], 2));
		task_data->str_count--;                                                                         // Stringcounter wieder runter
		WriteSpace();
	}
	OS_WriteReturn();
}

SendRS485_2(task_data, iData + 1);                                                      // Daten Senden
OS_Delay(iData + 1);																					// Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************

void RS4852SendModbus5Response(task_data_struct* task_data)     // Sendet Antwort auf Modbus Funktion 5 Anfrage
{
	unsigned int a, checksumCalc;

	task_data->str[task_data->str_count][0] = (UI_8)(stulz_bus_com.slave);  // Meine globale Adresse wurde angesprochen
	task_data->str[task_data->str_count][1] = 5;                                                        // Modbus Function 5
	task_data->str[task_data->str_count][2] = (UI_8)(stulz_bus_com.modbus_datstart >> 8 & 0xFF);        // high byte of datapoint-adress
	task_data->str[task_data->str_count][3] = (UI_8)(stulz_bus_com.modbus_datstart & 0xFF);             // low byte of datapoint-adress
	task_data->str[task_data->str_count][4] = (UI_8)(stulz_bus_com.modbus_value >> 8 & 0xFF);       // high byte of datapoint-Wert
	task_data->str[task_data->str_count][5] = (UI_8)(stulz_bus_com.modbus_value & 0xFF);                // low byte of datapoint-Wert

	// checksum calculation
	checksumCalc = 0xFFFF;
	for (a = 0; a < 6; a++)
		checksumCalc = crc16Add2(task_data->str[task_data->str_count][a], checksumCalc);
	task_data->str[task_data->str_count][6] = (UI_8)(checksumCalc & 0xFF);  // low byte of checksum
	task_data->str[task_data->str_count][7] = (UI_8)(checksumCalc >> 8 & 0xFF); // high byte of checksum

	if (mon_timer_b2)
	{
		OS_WriteString("\r\nMod 5 res:");
		for (a = 0; a <= 7; a++)
		{
			task_data->str_count++;                                                                         // Stringcounter einen hoch
			OS_WriteString(LongToStringHex(&NULL_task_data, task_data->str[task_data->str_count - 1][a], 2));
			task_data->str_count--;                                                                         // Stringcounter wieder runter
			WriteSpace();
		}
		OS_WriteReturn();
	}

	SendRS485_2(task_data, 8);                                                      // Daten Senden
	OS_Delay(8);                                                                                    // Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************

void RS4852SendModbus8Response(task_data_struct* task_data)     // Sendet Antwort auf Modbus Funktion 5 Anfrage
{
	unsigned int a, checksumCalc;

	task_data->str[task_data->str_count][0] = (UI_8)(stulz_bus_com.slave);  // Meine globale Adresse wurde angesprochen
	task_data->str[task_data->str_count][1] = 8;                                                        // Modbus Function 8
	task_data->str[task_data->str_count][2] = (UI_8)(stulz_bus_com.modbus_diag_code >> 8 & 0xFF);       // high byte of diag code
	task_data->str[task_data->str_count][3] = (UI_8)(stulz_bus_com.modbus_diag_code & 0xFF);                // low byte of diag code
	task_data->str[task_data->str_count][4] = (UI_8)(stulz_bus_com.modbus_value >> 8 & 0xFF);       // high byte of datapoint-Wert
	task_data->str[task_data->str_count][5] = (UI_8)(stulz_bus_com.modbus_value & 0xFF);                // low byte of datapoint-Wert

	// checksum calculation
	checksumCalc = 0xFFFF;
	for (a = 0; a < 6; a++)
		checksumCalc = crc16Add2(task_data->str[task_data->str_count][a], checksumCalc);
	task_data->str[task_data->str_count][6] = (UI_8)(checksumCalc & 0xFF);  // low byte of checksum
	task_data->str[task_data->str_count][7] = (UI_8)(checksumCalc >> 8 & 0xFF); // high byte of checksum

	if (mon_timer_b2)
	{
		OS_WriteString("\r\nMod 8 res:");
		for (a = 0; a <= 7; a++)
		{
			task_data->str_count++;                                                                         // Stringcounter einen hoch
			OS_WriteString(LongToStringHex(&NULL_task_data, task_data->str[task_data->str_count - 1][a], 2));
			task_data->str_count--;                                                                         // Stringcounter wieder runter
			WriteSpace();
		}
		OS_WriteReturn();
	}

	SendRS485_2(task_data, 8);                                                      // Daten Senden
	OS_Delay(8);                                                                                    // Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************

void RS4852SendModbus16Response(task_data_struct* task_data)        // Sendet Antwort auf Modbus Funktion 16 Anfrage
{
	unsigned int a, checksumCalc;

	task_data->str[task_data->str_count][0] = (UI_8)(stulz_bus_com.slave);  // Meine globale Adresse wurde angesprochen
	task_data->str[task_data->str_count][1] = 16;                                                       // Modbus Function
	task_data->str[task_data->str_count][2] = (UI_8)(stulz_bus_com.modbus_datstart >> 8 & 0xFF);        // high byte of datapoint-adress
	task_data->str[task_data->str_count][3] = (UI_8)(stulz_bus_com.modbus_datstart & 0xFF);             // low byte of datapoint-adress
	task_data->str[task_data->str_count][4] = (UI_8)(stulz_bus_com.modbus_datnum >> 8 & 0xFF);      // high byte Anzahl der Wörter
	task_data->str[task_data->str_count][5] = (UI_8)(stulz_bus_com.modbus_datnum & 0xFF);               // low byte Anzahl der Wörter

	// checksum calculation
	checksumCalc = 0xFFFF;
	for (a = 0; a < 6; a++)
		checksumCalc = crc16Add2(task_data->str[task_data->str_count][a], checksumCalc);
	task_data->str[task_data->str_count][6] = (UI_8)(checksumCalc & 0xFF);  // low byte of checksum
	task_data->str[task_data->str_count][7] = (UI_8)(checksumCalc >> 8 & 0xFF); // high byte of checksum

	if (mon_timer_b2)
	{
		OS_WriteString("\r\nMod 16 res:");
		for (a = 0; a <= 7; a++)
		{
			task_data->str_count++;                                                                         // Stringcounter einen hoch
			OS_WriteString(LongToStringHex(&NULL_task_data, task_data->str[task_data->str_count - 1][a], 2));
			task_data->str_count--;                                                                         // Stringcounter wieder runter
			WriteSpace();
		}
		OS_WriteReturn();
	}

	SendRS485_2(task_data, 8);                                                      // Daten Senden
	OS_Delay(8);                                                                                    // Task beenden bis Sendung raus
}

//***************************************************************************************
//***************************************************************************************

unsigned char IsCommand(unsigned char com_id)   // Ist dies ein gültiges IO-Bus Kommando
{
	switch (com_id)
	{
		case IOBUS_MSG_TOKEN:               //	the token
		case IOBUS_MSG_DATA:                //	the data-message (Master -> Slave)
		case IOBUS_MSG_DATARESP:        //	the data-message (Slave -> Master)
		case IOBUS_MSG_PING_REQ:        // Ping Anfrage (Master -> Slave)
		case IOBUS_MSG_PING_RES:        // Ping Antwort (Slave -> Master)
			return (1);

		default:
			return (0);
	}
}

//***************************************************************************************
//***************************************************************************************

unsigned char CheckRS4851Input(task_data_struct* task_data, unsigned int com_end)
{   // Verarbeitet eingegangenes Kommando auf IO Bus
	unsigned int a, b, com_len, c, d, e, f, temp_adress;
	unsigned int checksumCalc;
	unsigned int iAccessbmp, iData, byteofbits;
	unsigned char bitcounter, Value;
	unsigned char dp_all_num;
	const unsigned char* dataType;
	unsigned int access_byte;           // Aktuelles Byte im Access Bitmap
	unsigned char access_bit;               // Aktuelles Bit im Access Byte
	unsigned char access;                       // Aktuelles Access Bit für diesen DP
	unsigned long l;

	if (io_bus_com.i_send_token)                                // Eigenen Token faken
	{
		io_bus_com.sender = adresse.value;                                      // Absender eintragen
		io_bus_com.type = IOBUS_TYPE_C7000IOC;                          // Bustyp eintragen
		return (IOBUS_MSG_TOKEN);
	}

	if (PointerDiff(RS485_1_receive_nach_pointer, com_end, MAX_485_1_RECEIVE_BUFF) < 4) return (0); // Zu kurz

	bitcounter = 0;

	// Startbyte überspringen oder auch nicht
	a = RS485_1_receive_nach_pointer;
	if (IsCommand(RS485_1_receive_buf[a]))      // Erstes Byte ist gültiges Kommando, STX verloren gegangen, Start schon gefunden
	{
		com_len = a;
		com_len++;
		if (com_len >= MAX_485_1_RECEIVE_BUFF) com_len = 0;
		if (IsCommand(RS485_1_receive_buf[com_len]))    // Nächstes Byte ist auch gültiges Kommando
			a = com_len;
	}
	else                                                                                // Erstes Byte ist kein gültiges Kommando (Startbyte)
	{
		a++;
		if (a >= MAX_485_1_RECEIVE_BUFF) a = 0;
	}

	if (!IsCommand(RS485_1_receive_buf[a]))                     // Hier muss das Kommando anfangen
		return (0);                                                                             // -> sonst abbruch

	// ESC rausnehmen und in Task_data string kopieren
	com_len = 0;                                                                                            // Stringlänge
	while (a != com_end && com_end < MAX_485_1_RECEIVE_BUFF)
	{
		//		if (RS485_1_receive_buf [a] == ESC)								// Wenn ESC
		//		{
		//			a++;
		//	 	 	if (a >= MAX_485_1_RECEIVE_BUFF) a = 0;
		//			if (a == com_end) break;												// Nicht übers Ende hinausschiessen
		//		}
		if (com_len < TASK_STR_LEN) task_data->str[task_data->str_count][com_len++] = RS485_1_receive_buf[a];
		a++;
		if (a >= MAX_485_1_RECEIVE_BUFF) a = 0;
		//		b++;
	}

	// Kommando Auswertung
	switch (task_data->str[task_data->str_count][0])        // Kommando Auswertung
	{
		//----------------------------------------------------------------------------------------
		case IOBUS_MSG_TOKEN:               // the token

			checksumCalc = 0xFFFF;      // initialize checksum
			for (a = 0; a <= 9; a++)
				checksumCalc = crc16Add(task_data->str[task_data->str_count][a], checksumCalc);

			if (task_data->str[task_data->str_count][10] != ((UI_8)(checksumCalc & 0xFF))) return (0);  // Falsche CS LB
			if (task_data->str[task_data->str_count][11] != ((UI_8)(checksumCalc >> 8 & 0xFF))) return (0); // Falsche CS HB

			io_bus_com.recipient = task_data->str[task_data->str_count][1]; // Empfänger eintragen
			io_bus_com.sender = task_data->str[task_data->str_count][2];    // Absender eintragen
			io_bus_com.type = task_data->str[task_data->str_count][3];  // Bustyp eintragen

			if (io_bus_com.sender == adresse.value) address_conflict.alarm.value = 1;           // Token mit meiner Adresse als Absender erhalten

			l = task_data->str[task_data->str_count][6];                                            // Dynamische Buskonfiguration lesen
			l <<= 8;
			l |= task_data->str[task_data->str_count][5];
			l <<= 8;
			l |= task_data->str[task_data->str_count][4];

			io_bus_com.dyn_conf = l | my_long_adress;                                                   // Empfangene dynamische Buskonfiguration mit mir drin übernehmen

			l = task_data->str[task_data->str_count][9];                                            // Statische Buskonfiguration lesen
			l <<= 8;                                                                                                                    // Diese muss ich eigentlich übernehmen
			l |= task_data->str[task_data->str_count][8];
			l <<= 8;
			l |= task_data->str[task_data->str_count][7];

			l |= io_bus_com.dyn_conf;                   // Positive Differenz aus der dynamischen Konfiguration bernehmen

			if (l != io_bus_com.stat_conf)                                                                      // Konfiguration ist abweichend
			{
				//				OS_WriteStringReturn ("save stat 1!!!");
				io_bus_com.stat_conf = l;                                                                               // Übernehmen
				WriteRTC(RTC_STAT_BUS_CONF_1, (unsigned char) io_bus_com.stat_conf);            // Statische Buskonfiguration sichern
WriteRTC(RTC_STAT_BUS_CONF_2, (unsigned char)(io_bus_com.stat_conf >> 8));
WriteRTC(RTC_STAT_BUS_CONF_3, (unsigned char)(io_bus_com.stat_conf >> 16));
			}
			
			if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_TOKEN)
{
	WriteSpace();
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
	WriteGroesser(); // ">"
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string

	if (mon_timer_b1_level & _MON_LEVEL_TOKEN_DET)
	{
		WriteSpace();
		OS_WriteString(LongToStringBin(&NULL_task_data, io_bus_com.dyn_conf, 20));      // Wandelt ein long in string
		WriteSpace();
		OS_WriteString(LongToStringBin(&NULL_task_data, io_bus_com.stat_conf, 20));     // Wandelt ein long in string
		WriteSpace();
	}

	if (io_bus_com.recipient == io_bus_com.sender)  // Token broadcast
		OS_WriteString("\r\nToken lost!");

	OS_WriteReturn();
}

return (IOBUS_MSG_TOKEN);
//----------------------------------------------------------------------------------------
		case IOBUS_MSG_PING_REQ:                // Ping request

checksumCalc = 0xFFFF;      // initialize checksum
checksumCalc = crc16Add(task_data->str[task_data->str_count][0], checksumCalc);
checksumCalc = crc16Add(task_data->str[task_data->str_count][1], checksumCalc);
checksumCalc = crc16Add(task_data->str[task_data->str_count][2], checksumCalc);
if (task_data->str[task_data->str_count][3] != ((UI_8)(checksumCalc & 0xFF))) return (0);   // Falsche CS LB
if (task_data->str[task_data->str_count][4] != ((UI_8)(checksumCalc >> 8 & 0xFF))) return (0);  // Falsche CS HB

io_bus_com.recipient = task_data->str[task_data->str_count][1]; // Empfänger eintragen
io_bus_com.sender = task_data->str[task_data->str_count][2];    // Absender eintragen

if (io_bus_com.recipient == adresse.value) address_conflict.alarm.value = 0;    // Ping an mich erhalten -> Adress Konflikt gelöst

if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_TOKEN)
{
	WriteSpace();
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
	OS_WriteString("-P1>");
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string
}

return (IOBUS_MSG_PING_REQ);
//----------------------------------------------------------------------------------------
		case IOBUS_MSG_PING_RES:                // Ping response

checksumCalc = 0xFFFF;      // initialize checksum
checksumCalc = crc16Add(task_data->str[task_data->str_count][0], checksumCalc);
checksumCalc = crc16Add(task_data->str[task_data->str_count][1], checksumCalc);
checksumCalc = crc16Add(task_data->str[task_data->str_count][2], checksumCalc);
checksumCalc = crc16Add(task_data->str[task_data->str_count][3], checksumCalc);
if (task_data->str[task_data->str_count][4] != ((UI_8)(checksumCalc & 0xFF))) return (0);   // Falsche CS LB
if (task_data->str[task_data->str_count][5] != ((UI_8)(checksumCalc >> 8 & 0xFF))) return (0);  // Falsche CS HB

io_bus_com.recipient = task_data->str[task_data->str_count][1]; // Empfänger eintragen
io_bus_com.sender = task_data->str[task_data->str_count][2];    // Absender eintragen
io_bus_com.type = task_data->str[task_data->str_count][3];  // Bustyp eintragen

if (io_bus_com.sender == adresse.value) address_conflict.alarm.value = 1;           // Token mit meiner Adresse als Absender erhalten

if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_TOKEN)
{
	WriteSpace();
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
	OS_WriteString("-P2>");
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string
}

return (IOBUS_MSG_PING_RES);
//----------------------------------------------------------------------------------------
		case IOBUS_MSG_DATA:                //	the data-message (Master -> Slave)

io_bus_com.len = task_data->str[task_data->str_count][3];   // Länge LB

if (io_bus_com.len > com_len) io_bus_com.len = com_len;         // Maximale Länge
if (io_bus_com.len < 3) return (0);

checksumCalc = 0xFFFF;      // initialize checksum
for (a = 0; a < io_bus_com.len - 2; a++)
	checksumCalc = crc16Add(task_data->str[task_data->str_count][a], checksumCalc);
if (task_data->str[task_data->str_count][io_bus_com.len - 2] != ((UI_8)(checksumCalc & 0xFF)))
	return (0); // Falsche CS LB
if (task_data->str[task_data->str_count][io_bus_com.len - 1] != ((UI_8)(checksumCalc >> 8 & 0xFF)))
	return (0); // Falsche CS HB

// Check auf Kommando Codes die von der WIB durchs AT durchgeleitet werden
// da ist dann noch die 2 für "Länge der folgenden Adressangabe" drin
// diese 2 muss raus
a = task_data->str[task_data->str_count][4];                        // Kommando Code holen

if (a == STULZBUS_DATA_BLOCKREQ ||                                      // Ist es ein WIB kommando
		a == STULZBUS_DATA_BLOCKRES ||
		a == STULZBUS_DATA_ADRREQ ||
		a == STULZBUS_DATA_ADRRES)
{
	for (a = 6; a < com_len; a++)                   // task_data->str[task_data->str_count][5] = 2 entfernen
		task_data->str[task_data->str_count][a - 1] = task_data->str[task_data->str_count][a];
}

io_bus_com.adress = task_data->str[task_data->str_count][6];
io_bus_com.adress <<= 8;
io_bus_com.adress |= task_data->str[task_data->str_count][5];

io_bus_com.number = task_data->str[task_data->str_count][8];
io_bus_com.number <<= 8;
io_bus_com.number |= task_data->str[task_data->str_count][7];

io_bus_com.sender = task_data->str[task_data->str_count][2];    // Absender eintragen
io_bus_com.recipient = task_data->str[task_data->str_count][1]; // Empfänger eintragen

if (io_bus_com.sender == adresse.value) address_conflict.alarm.value = 1;           // Token mit meiner Adresse als Absender erhalten

if (io_bus_com.recipient == io_bus_com.sender) io_bus_com.broadcast = 1;    // Broadcast
else io_bus_com.broadcast = 0;

switch (task_data->str[task_data->str_count][4])
{
	//--------------------------------------------------------------------------------------
	//--------------------------------------------------------------------------------------
	case STULZBUS_DATA_BLOCKREQ:    // Data block request Anfrage (Master -> Slave), Stulz Bus Command ID für WIB Anfragen über AT auf dem IO-Bus
	case IOBUS_DATA_BLOCKREQ:           // Data block request Anfrage (Master -> Slave), IO-Bus Command ID

		if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_MSG)
		{
			OS_WriteString("\r\nB1 RreqM:");
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
			WriteGroesser(); // ">"
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.adress, 5));        // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.number, 3));      // Wandelt ein long in string
		}

		if (task_data->str[task_data->str_count][4] == IOBUS_DATA_BLOCKREQ) // Anfrage mit redesign Command ID
			return (IOBUS_DATA_BLOCKREQ_MA);

		return (STULZBUS_DATA_BLOCKREQ_MA);     // Anfrage kommt von WIB und benutzt alte Kommandocodes
												//--------------------------------------------------------------------------------------
	case STULZBUS_DATA_BLOCKRES:    // Data block response (Master -> Slave), Stulz Bus Command ID für WIB Anfragen über AT auf dem IO-Bus
	case IOBUS_DATA_BLOCKRES:           // Data block response (Master -> Slave)

		if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_MSG)
		{
			OS_WriteString("\r\nB1 RresM:");
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
			WriteGroesser(); // ">"
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.adress, 5));        // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.number, 3));      // Wandelt ein long in string
		}

		if (io_bus_com.recipient == adresse.value ||        // Bin ich gemeint oder
				io_bus_com.broadcast)                                                       // Broadcast
		{

			bitcounter = 0;
			access_bit = 0;
			iData = 9 + (io_bus_com.number + 7) / 8;        // start of databytes after accessibility-bitmap;
			access_byte = 9;

			OS_Use(&DP_all);                                                        // Einen DP all reservieren
			dp_all_num = BlockDPall();
			OS_Unuse(&DP_all);

			// accessibility (bitmap) must be checked here!!!
			for (a = 0; a < io_bus_com.number; a++)
			{
				temp_adress = io_bus_com.adress + a;                    // Aktuelle Adresse merken

				//							dataType = DPAdr (io_bus_com.adress + a);
				dataType = DPAdr(temp_adress);

				access = task_data->str[task_data->str_count][access_byte] & (0x01 << access_bit);

				if (dataType != NULL)                                                   // Gibt es diesen DP überhaupt
				{
#if DEBUG == 1
OS_WriteString ("-h");
#endif
					GetDPnum(dataType, dp_all_num);                     // Put DP in DPall

					if (!access)                                                                // Access Bitmap war nicht gesetzt
					{                                                                                       // Bei Zonendaten die Einheit auf ungültig setzen, wenn eine ungültige Temperatur empfangen wurde
						if (temp_adress >= 36000 && temp_adress <= 36159 ||
								//temp_adress >= 36000 && temp_adress <= 36031 ||	// Unit Raumtemp
								//temp_adress >= 36032 && temp_adress <= 36063 ||	// Unit Raumfeuchte
								//temp_adress >= 36064 && temp_adress <= 36095 ||	// Unit Zulufttemp
								//temp_adress >= 36096 && temp_adress <= 36127 ||	// Unit Zuluftfeuchte
								//temp_adress >= 36128 && temp_adress <= 36159 ||	// Unit Lastzuschaltung Temp
								temp_adress >= 36224 && temp_adress <= 36255 || // Unit Aussentemp
								temp_adress >= 36640 && temp_adress <= 36671 || // Unit Differenz-Luftdruck
								temp_adress >= 36860 && temp_adress <= 36879 || // Unit Wassertemp
								temp_adress >= 37600 && temp_adress <= 37631)       // Unit Lastzuschaltung Feuchte
						{
							DP_ALL[dp_all_num].physUnit = PU_NV;    // Einheit ungültig
							PutDP(dataType, dp_all_num);                    // -> übernehmen
						}
					}
					else                                                                            // Access Bitmap war gesetzt
					{                                                                                   // Wert eintragen
						if (temp_adress >= 1300 && temp_adress <= 1467) // Sonderbehandlung Wochenprogramm
						{
							f = temp_adress - 1300;                                                                 // Offset abziehen
							b = f / 24;                                                                                             // Wochentag rausfinden, erstes Arrayfeld
							f -= b * 24;                                                                                            // Stunde rausfinden (0...23)
							c = f / 8;                                                                                              // Stunde Block 0,1,2 rausfinden
							d = f - c * 8;                                                                                      // Stunde innerhalb des Blockes rausfinden

							f = wprg[b][c].value;                                                                       // Original merken
							e = 0x03 << (d * 2);                                                                            // Positiv Bitmap zusammenbauen
							e = ~e;                                                                                                 // Negativ Bitmap erzeugen
							wprg[b][c].value &= e;                                                                  // Entsprechende 2 Bits für die Stunde löschen
							wprg[b][c].value |= (task_data->str[task_data->str_count][iData] & 0x03) << (d * 2);// Wert einfügen

							if (f != wprg[b][c].value)
								WriteFlashConfigS(&wprg[b][c].dataType);

							iData += 1;
							bitcounter = 0;
						}
						else if (temp_adress >= 1016 && temp_adress <= 1021)    // Sonderbehandlung Uhrzeit/Datum verstellen
						{
							Value = (unsigned char)task_data->str[task_data->str_count][iData];
			switch (temp_adress)
			{
				case 1016:                                                      // Jahr
					time_update |= RTC_CH_YEAR;
					RTC_jahr_tmp = Value;
					break;
				case 1017:                                                      // Monat
					time_update |= RTC_CH_MONTH;
					RTC_monat_tmp = Value;
					break;
				case 1018:                                                      // Tag
					time_update |= RTC_CH_DAY;
					RTC_tag_tmp = Value;
					break;
				case 1019:                                                      // Stunde
					time_update |= RTC_CH_HRS;
					RTC_stunde_tmp = Value;
					break;
				case 1020:                                                      // Minute
					time_update |= RTC_CH_MIN;
					RTC_min_tmp = Value;
					break;
				case 1021:                                                      // Sekunde
					time_update |= RTC_CH_SEC;
					RTC_sekunde_tmp = Value;
					break;
			}
			iData += 1;
			bitcounter = 0;
		}
		else
		{
			if (DP_ALL[dp_all_num].access & A_IO_WO)    // Wert darf beschrieben werden
			{
				// valid result received
				if (ActIsOn() && 0 == io_bus_com.broadcast)
				{
					ActParamsCheckChange(temp_adress);
				}

				if (0 == io_bus_com.broadcast &&
						(temp_adress >= DP_CWSUPPLY1ST) && (temp_adress <= DP_CWSUPPLYLAST))
				{
					cwSupplyChanged = true;
				}

				switch (*dataType & 0x07)
				{
					case DT_BIT1:
						// 8 datapoints are combined in one byte => split them;
						// lowest datapoint is LSB

						if (!bitcounter)
						{
							// we need a new byte :-)
							byteofbits = iData++;               // Am Anfang 12
						}

						if (task_data->str[task_data->str_count][byteofbits] & (0x01 << bitcounter))
						{
							if (DP_ALL[dp_all_num].value.UI8 != 1)          // Ist Wert schon gleich
							{
								DP_ALL[dp_all_num].value.UI8 = 1;
								PutDP(dataType, dp_all_num);
								WriteFlashConfigS(dataType);
							}
						}
						else
						{
							if (DP_ALL[dp_all_num].value.UI8 != 0)          // Ist Wert schon gleich
							{
								DP_ALL[dp_all_num].value.UI8 = 0;
								PutDP(dataType, dp_all_num);
								WriteFlashConfigS(dataType);
							}

							if (temp_adress == 1002)                    // Unit Local Stop
							{
								on_off_rem_ea_stop.value = 0;   // 1 = Fern Ein/Aus Stop
								on_off_timer_stop.value = 0;    // 1 = Timer Stop
								on_off_seq_stop.value = 0;  // 1 = Sequencing Stop

								on_off_monitoring_stop.value = 0;   // 1 = Monitoring Stop
								stoppedByFireAlarm.value = 0;
								bmsStop2.value = 0;

								WriteFlashConfigS(&on_off_monitoring_stop.dataType);    // Ins Flash speichern
								WriteFlashConfigS(&on_off_seq_stop.dataType);                   // Ins Flash speichern
								WriteFlashConfigS(&stoppedByFireAlarm.dataType);
								WriteFlashConfigS(&bmsStop2.dataType);

								on_off.value = 1;                                       // Gerät einschalten
							}
						}
						bitcounter++;   // do not reset here!!
						if (bitcounter > 7) bitcounter = 0;
						break;

					case DT_SI8:
					case DT_UI8:
						if (DP_ALL[dp_all_num].value.UI8 !=             // Ist Wert schon gleich
								(unsigned char)task_data->str[task_data->str_count][iData])
													{
							DP_ALL[dp_all_num].value.UI8 = task_data->str[task_data->str_count][iData];
							PutDP(dataType, dp_all_num);
							WriteFlashConfigS(dataType);
						}
						iData += 1;
						bitcounter = 0;
						break;

					case DT_SI16:
					case DT_UI16:
						if (DP_ALL[dp_all_num].value.UI16 !=                // Ist Wert schon gleich
							((unsigned int)task_data->str[task_data->str_count][iData] |
							 (unsigned int)task_data->str[task_data->str_count][iData + 1] << 8))
													{
							DP_ALL[dp_all_num].value.UI16 = (unsigned int)task_data->str[task_data->str_count][iData];
							DP_ALL[dp_all_num].value.UI16 |= (unsigned int)task_data->str[task_data->str_count][iData + 1] << 8;
							PutDP(dataType, dp_all_num);
							WriteFlashConfigS(dataType);
						}
						iData += 2;
						bitcounter = 0;
						break;

					case DT_SI32:
					case DT_UI32:
						if (temp_adress >= 35600 && // Zonen Seq Stop bitmap
								temp_adress <= 35631 &&
								my_zone.seq_switch_timer)               // Mache gerade eine Umschaltung
						{
							//													OS_WriteStringReturn ("DISCARDED!!!");					
						}
						// Also empfang des DP unterdrücken
						else if (DP_ALL[dp_all_num].value.UI32 !=               // Ist Wert schon gleich
							((unsigned long)task_data->str[task_data->str_count][iData] |
							((unsigned long)task_data->str[task_data->str_count][iData + 1] << 8)		|
							  ((unsigned long)task_data->str[task_data->str_count][iData + 2] << 16)	|
								(unsigned long)task_data->str[task_data->str_count][iData + 3] << 24))
													{
							DP_ALL[dp_all_num].value.UI32 = (unsigned long)task_data->str[task_data->str_count][iData];
							DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 1] << 8;
							DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 2] << 16;
							DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 3] << 24;
							PutDP(dataType, dp_all_num);
							WriteFlashConfigS(dataType);
						}
						iData += 4;
						bitcounter = 0;
						break;

				} // switch (DPAdr(adress + a)->dataType)

				if (temp_adress >= 36000 && temp_adress <= 36031)   // Unit Raumtemp
					unit[temp_adress - 36000].raum_temp.physUnit = PU_TEMP;// Raumtemp gültig

				if (temp_adress >= 36032 && temp_adress <= 36063)   // Unit Raumfeuchte
					unit[temp_adress - 36032].raum_feuchte.physUnit = PU_PERCENT_RH;// Raumfeuchte gültig

				if (temp_adress >= 36064 && temp_adress <= 36095)   // Unit Zulufttemp
					unit[temp_adress - 36064].zuluft_temp.physUnit = PU_TEMP;// Zulufttemp gültig

				if (temp_adress >= 36096 && temp_adress <= 36127)   // Unit Zuluftfeuchte
					unit[temp_adress - 36096].zuluft_feuchte.physUnit = PU_PERCENT_RH;// Zuluftfeuchte gültig

				if (temp_adress >= 36128 && temp_adress <= 36159)   // Unit Lastzuschaltung Temp
					unit[temp_adress - 36128].lastzu_temp.physUnit = PU_DELTATEMP;// Lastzuschaltung gültig

				if (temp_adress >= 36224 && temp_adress <= 36255)   // Unit Aussentemp
					unit[temp_adress - 36224].aussen_temp.physUnit = PU_TEMP;// Aussentemp gültig

				if (temp_adress >= 36640 && temp_adress <= 36640 + MAX_UNIT_ADR)    // Unit Differenz-Luftdruck
					unit[temp_adress - 36640].boden_druck.physUnit = PU_PA;                 // Differenz-Luftdruck gültig

				if (temp_adress >= 36860 && temp_adress <= 36879)   // Unit Wassertemp
					unit[temp_adress - 36860].wasser_temp_in_1.physUnit = PU_TEMP;// Wassertemp gültig

				if (temp_adress >= 37600 && temp_adress <= 37631)   // Unit Lastzuschaltung Feuchte
					unit[temp_adress - 37600].lastzu_feu.physUnit = PU_PERCENT_RH;// Lastzuschaltung gültig

				if (temp_adress >= 8700 && temp_adress <= LAST_EEV_DP)  // Änderung der EEV DP auch an VCM übertragen
				{
					switch (temp_adress)            // Welcher DP ist es genau
					{
						case 8701:
						case 8801:
							write_eev_bchrg = 1;    // Batterie laden gibt es nur im EEIO
							break;

						case 8716:
						case 8816:
							write_eev_bht = 1;  // Batteriehaltezeit gibt es nur im EEIO
							break;
					}
				}
				SetGLTSensor(temp_adress); // Änderung der Sensor GLT-Werte führt zur Aktivierung
			}   // if (DP_ALL[dp_all_num].access & A_IO_WO)
		}
}
							}		// if (dataType != NULL)

							access_bit++;                                                                   // Ein Bit im Access Bitmap weiterzählen
if (access_bit > 7)                                                     // Ende des Accessbytes
{
	access_bit = 0;                                                         // Wieder beim ersten Bit abfangen
	access_byte++;                                                          // Neues Byte anfangen
}
						}																								// for (a = 0; a < number; a++)
						RELEASEDPALL(dp_all_num);												// DPall wieder freigeben
					}																									// if (io_bus_com.recipient == adresse.value)		// Bin ich gemeint
					if (task_data->str[task_data->str_count][4] == IOBUS_DATA_BLOCKRES) // Anfrage mit redesign Command ID
	return (IOBUS_DATA_BLOCKRES_MA);

return (STULZBUS_DATA_BLOCKRES_MA);		// Anfrage kommt von WIB und benutzt alte Kommandocodes
//----------------------------------------------------------------------------------------
				case STULZBUS_DATA_ADRREQ:                                          // Adressed request (Master -> Slave) Stulz Bus Command ID für WIB Anfragen über AT auf dem IO-Bus
				case IOBUS_DATA_ADRREQ:                                                 // Adressed request (Master -> Slave) IO-Bus Command ID

if (io_bus_com.len < 7) return (0);
io_bus_com.address_num = (io_bus_com.len - 7) / 2;  // Anzahl der angefragten Adressen bei Adressanfrage

if (mon_timer_b2)
{
	OS_WriteString("\r\nB1 AdrReqM:");
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
	WriteGroesser(); // ">"
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string
	WriteDpkt(); // ":"
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.address_num, 2));       // Wandelt ein long in string
}

if (io_bus_com.address_num)
{
	for (a = 0; a < io_bus_com.address_num; a++)
	{
		io_bus_com.adress2[a] = task_data->str[task_data->str_count][6 + a * 2] * 256;  // Adressen einlesen HB
		io_bus_com.adress2[a] += task_data->str[task_data->str_count][5 + a * 2];           // Adressen einlesen LB

		if (mon_timer_b2)
		{
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.adress2[a], 5));      // Wandelt ein long in string
		}
	}
}

if (mon_timer_b2) OS_WriteReturn();

if (task_data->str[task_data->str_count][4] == IOBUS_DATA_ADRREQ)   // Anfrage mit redesign Command ID
	return (IOBUS_DATA_ADRREQ_MA);

return (STULZBUS_DATA_ADRREQ_MA);
//----------------------------------------------------------------------------------------
				case STULZBUS_DATA_ADRRES:                                          // Stulz Bus Command ID für WIB Anfragen über AT auf dem IO-Bus
				case IOBUS_DATA_ADRRES:                                                 // Adressed response (Master -> Slave) IO-Bus Command ID

if (io_bus_com.len < 7) return (0);

a = (io_bus_com.len - 7) / 2;                                           // Anzahl der angefragten Adressen bei Adressanfrage
io_bus_com.address_num = 0;                                     // Richtige Anzahl aller gültigen Adressen

if (mon_timer_b2)
{
	OS_WriteString("\r\nB1 AdrRresM:");
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
	WriteGroesser(); // ">"
	OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string
	WriteDpkt(); // ":"
	OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.address_num, 2));     // Wandelt ein long in string
}

if (io_bus_com.recipient == adresse.value ||        // Bin ich gemeint oder
		io_bus_com.broadcast)                                                       // Broadcast
{
	if (a)                                                                                  // Sind überhaupt Adressen angegeben
	{
		iData = 5;                                                                      // Erste Adresse

		OS_Use(&DP_all);                                                            // Einen DP all reservieren
		dp_all_num = BlockDPall();
		OS_Unuse(&DP_all);

		while (iData < 300 && iData < io_bus_com.len - 2)   // Bis zum Ende
		{
			io_bus_com.adress = task_data->str[task_data->str_count][iData + 1] * 256;  // Adresse einlesen HB
			io_bus_com.adress += task_data->str[task_data->str_count][iData];               // Adresse einlesen LB

			dataType = DPAdr(io_bus_com.adress);        // Adresse des DP holen

			if (dataType != NULL)
			{
#if DEBUG == 1
OS_WriteString ("-i");		//
#endif
				GetDPnum(dataType, dp_all_num);                 // DP holen

				//									if (DP_ALL[dp_all_num].access & A_IO_WO)	// Darf ich in diesen DP schreiben
				//									{
				io_bus_com.adress2[io_bus_com.address_num] = io_bus_com.adress; // Gültige Adresse merken
				io_bus_com.address_num++;                       // Anzahl gültiger Adressen hochzählen

				switch (*dataType & 0x07)                               // Unterscheidung nach DP Typ
				{
					case DT_BIT1:                                                   // 8 Bits
					case DT_SI8:
					case DT_UI8:
						if (DP_ALL[dp_all_num].value.UI8 != // Ist Wert schon gleich
							(unsigned char)task_data->str[task_data->str_count][iData + 2] &&
							DP_ALL[dp_all_num].access & A_IO_WO)	// Schreibberechtigt
												{
							DP_ALL[dp_all_num].value.UI8 = task_data->str[task_data->str_count][iData + 2]; // Wert übernehmen

							PutDP(dataType, dp_all_num);            // Wert zurückschreiben
							WriteFlashConfigS(dataType);            // Wert im Flash speichern
						}
						iData += 3;                                                 // Auf nächste Adresse Stellen
						break;

					case DT_SI16:                                                   // 16 bits
					case DT_UI16:
						if (DP_ALL[dp_all_num].value.UI16 !=    // Ist Wert schon gleich
							((unsigned int)task_data->str[task_data->str_count][iData + 2] |
							 (unsigned int)task_data->str[task_data->str_count][iData + 3] << 8) &&
							  DP_ALL[dp_all_num].access & A_IO_WO)	// Schreibberechtigt
												{
							DP_ALL[dp_all_num].value.UI16 = task_data->str[task_data->str_count][iData + 2];
							DP_ALL[dp_all_num].value.UI16 |= task_data->str[task_data->str_count][iData + 3] << 8;

							PutDP(dataType, dp_all_num);
							WriteFlashConfigS(dataType);
						}
						iData += 4;                                                 // Auf nächste Adresse stellen
						break;

					case DT_SI32:                                                   // 32 bits
					case DT_UI32:
						if (DP_ALL[dp_all_num].value.UI32 !=    // Ist Wert schon gleich
								((unsigned long)task_data->str[task_data->str_count][iData + 2] |
								((unsigned long)task_data->str[task_data->str_count][iData + 3] << 8)		|
								  ((unsigned long)task_data->str[task_data->str_count][iData + 4] << 16)	|
									 (unsigned long)task_data->str[task_data->str_count][iData + 5] << 24) &&
								  DP_ALL[dp_all_num].access & A_IO_WO)	// Schreibberechtigt
												{
							DP_ALL[dp_all_num].value.UI32 = (unsigned long)task_data->str[task_data->str_count][iData + 2];
							DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 3] << 8;
							DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 4] << 16;
							DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 5] << 24;

							PutDP(dataType, dp_all_num);
							WriteFlashConfigS(dataType);
						}
						iData += 6;                                                 // Auf nächste Adresse stellen
						break;

					default:                                                            // Ungültiger Typ -> Abbruch
						RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
						if (task_data->str[task_data->str_count][4] == IOBUS_DATA_ADRRES)   // Anfrage mit redesign Command ID
							return (IOBUS_DATA_ADRRES_MA);
						return (STULZBUS_DATA_ADRRES_MA);

				}                                                                           // switch (*dataType & 0x07)
																							//									}																					// if (DP_ALL[dp_all_num].access & A_IO_WO)
			}                                                                                       // if (dataType != NULL)
			else                                                                                // DP unbekannt -> Abbruch
			{
				RELEASEDPALL(dp_all_num);                                   // DPall wieder freigeben
				if (task_data->str[task_data->str_count][4] == IOBUS_DATA_ADRRES)   // Anfrage mit redesign Command ID
					return (IOBUS_DATA_ADRRES_MA);
				return (STULZBUS_DATA_ADRRES_MA);
			}
		}                                                                                           // while (iData < 300 && iData < stulz_bus_com.len-2)
		RELEASEDPALL(dp_all_num);                                           // DPall wieder freigeben
	}                                                                                               // if (stulz_bus_com.address_num)
}                                                                                                   // if (stulz_bus_com.recipient == adresse.value)		// Bin ich gemeint
if (task_data->str[task_data->str_count][4] == IOBUS_DATA_ADRRES)   // Anfrage mit redesign Command ID
	return (IOBUS_DATA_ADRRES_MA);
return (STULZBUS_DATA_ADRRES_MA);
			}																											// switch (task_data->str[task_data->str_count][5])
			return (0);
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
		case IOBUS_MSG_DATARESP:                //	the data-message (Slave -> Master)

io_bus_com.len = task_data->str[task_data->str_count][3];   // Länge LB

if (io_bus_com.len > com_len) io_bus_com.len = com_len;         // Maximale Länge
if (io_bus_com.len < 3) return (0);

checksumCalc = 0xFFFF;      // initialize checksum
for (a = 0; a < io_bus_com.len - 2; a++)
	checksumCalc = crc16Add(task_data->str[task_data->str_count][a], checksumCalc);
if (task_data->str[task_data->str_count][io_bus_com.len - 2] != ((UI_8)(checksumCalc & 0xFF)))
	return (0); // Falsche CS LB
if (task_data->str[task_data->str_count][io_bus_com.len - 1] != ((UI_8)(checksumCalc >> 8 & 0xFF)))
	return (0); // Falsche CS HB

// Check auf Kommando Codes die von der WIB durchs AT durchgeleitet werden
// da ist dann noch die 2 für "Länge der folgenden Adressangabe" drin
// diese 2 muss raus
a = task_data->str[task_data->str_count][4];                        // Kommando Code holen

if (a == STULZBUS_DATA_BLOCKREQ ||                                      // Ist es ein WIB kommando
		a == STULZBUS_DATA_BLOCKRES ||
		a == STULZBUS_DATA_ADRREQ ||
		a == STULZBUS_DATA_ADRRES)
{
	for (a = 6; a < com_len; a++)                   // task_data->str[task_data->str_count][5] = 2 entfernen
		task_data->str[task_data->str_count][a - 1] = task_data->str[task_data->str_count][a];
}

io_bus_com.adress = task_data->str[task_data->str_count][6];
io_bus_com.adress <<= 8;
io_bus_com.adress |= task_data->str[task_data->str_count][5];

io_bus_com.number = task_data->str[task_data->str_count][8];
io_bus_com.number <<= 8;
io_bus_com.number |= task_data->str[task_data->str_count][7];

if (!io_bus_com.number) return (0);                                 // Anzahl 0 geht nicht

io_bus_com.sender = task_data->str[task_data->str_count][2];    // Absender eintragen
io_bus_com.recipient = task_data->str[task_data->str_count][1]; // Empfänger eintragen

if (io_bus_com.sender == adresse.value) address_conflict.alarm.value = 1;           // Token mit meiner Adresse als Absender erhalten

if (io_bus_com.recipient == io_bus_com.sender) io_bus_com.broadcast = 1;    // Broadcast
else io_bus_com.broadcast = 0;

switch (task_data->str[task_data->str_count][4])
{
	//--------------------------------------------------------------------------------------
	case STULZBUS_DATA_BLOCKREQ:    // Data block request (Slave -> Master), Stulz Bus Command ID für WIB Anfragen über AT auf dem IO-Bus
	case IOBUS_DATA_BLOCKREQ:           // Data block request (Slave -> Master)

		if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_MSG)
		{
			OS_WriteString("\r\nB1 RreqS:");
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
			WriteGroesser(); // ">"
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.adress, 5));        // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.number, 3));      // Wandelt ein long in string
		}
		return (IOBUS_DATA_BLOCKREQ_SL);// Ein return reicht, das Kommando wird derzeit nicht benutzt
										//--------------------------------------------------------------------------------------
	case STULZBUS_DATA_BLOCKRES:    // Data block response (Slave -> Master), Stulz Bus Command ID für WIB Anfragen über AT auf dem IO-Bus
	case IOBUS_DATA_BLOCKRES:           // Data block response (Slave -> Master)

		// Bisher nur für 1 DP geschrieben
		// Für mehr macht es auch keinen Sinn da dies die Antwort auf einen Request ist
		// Ich muss hierbei keine Daten übernehmen, es geht nur darum diese Antwort anzuzeigen

		//#if DP_MON
		//					if (io_bus_com.adress == dp_mon)
		//					{
		//						for (a = 0 ; a <= io_bus_com.len ; a++)
		//						{
		//							OS_WriteString (LongToStringHex (&NULL_task_data, task_data->str[task_data->str_count][a], 2));
		//							WriteSpace();
		//						}
		//						OS_WriteReturn ();
		//					}
		//#endif

		if (mon_timer_b1 && mon_timer_b1_level & _MON_LEVEL_MSG)
		{
			OS_WriteString("\r\nB1 RresS:");
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
			WriteGroesser(); // ">"
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.adress, 5));        // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.number, 3));      // Wandelt ein long in string

			//						for (a = 0 ; a <= 14 ; a++)
			//						{
			//							OS_WriteString (LongToStringHex (&NULL_task_data,task_data->str[task_data->str_count][a], 2));		// Wandelt ein long in string
			//							WriteSpace();
			//						}
			//						OS_WriteReturn ();
		}

		dataType = DPAdr(io_bus_com.adress);

#if DEBUG == 1
OS_WriteString ("-H");		//
#endif
		if (dataType != NULL)
		{
#if DEBUG == 1
OS_WriteString ("-q");		//
#endif

			dp_all_num = GetDP(dataType);       // Original DP holen
			CopyDP(&io_bus_com.dp.dataType, &DP_ALL[dp_all_num].dataType);  // Kopiert DP Eigenschaften über Nummer nach io_bus_com (auch dataType)

			RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben

			if (io_bus_com.recipient == adresse.value || io_bus_com.broadcast != 0)
			{
				bitcounter = 0;
				iAccessbmp = 9;             // start of accessibility-bitmap;
				iData = 9 + (io_bus_com.number + 7) / 8;        // start of databytes after accessibility-bitmap;

				if (task_data->str[task_data->str_count][iAccessbmp] & 0x01 << bitcounter)  // Datum gültig
				{
					switch (*dataType & 0x07)
					{
						case DT_BIT1:
						case DT_UI8:
						case DT_SI8:
							io_bus_com.dp.value.UI8 = task_data->str[task_data->str_count][10];
							break;
						default:
						case DT_UI16:
						case DT_SI16:
							io_bus_com.dp.value.UI16 = task_data->str[task_data->str_count][11];
							io_bus_com.dp.value.UI16 <<= 8;
							io_bus_com.dp.value.UI16 |= task_data->str[task_data->str_count][10];
							break;
						case DT_UI32:
						case DT_SI32:
							io_bus_com.dp.value.UI32 = task_data->str[task_data->str_count][13];
							io_bus_com.dp.value.UI32 <<= 8;
							io_bus_com.dp.value.UI32 |= task_data->str[task_data->str_count][12];
							io_bus_com.dp.value.UI32 <<= 8;
							io_bus_com.dp.value.UI32 |= task_data->str[task_data->str_count][11];
							io_bus_com.dp.value.UI32 <<= 8;
							io_bus_com.dp.value.UI32 |= task_data->str[task_data->str_count][10];
					}

					// Sonderbehandlung Pumpenschrank 
					if (DP_CWREQACTIVE == io_bus_com.adress)
					{
						// Dieses Gerät ist in der Pumpenschrankzone enthalten 
						if ((io_bus_com.dp.value.UI32 & (1UL << adresse.value)) != 0)
						{
							cwSupplyError = (io_bus_com.dp.value.SI32 < 0);
						}
					}

					// Sonderbehandlung AE-Regelung 
					if (FacesIsActive())
					{
						switch (io_bus_com.adress)
						{
							case 6932:  // Lüfterdrehzahl empfangen 
								unit[io_bus_com.sender].fanSpeed.value = io_bus_com.dp.value.UI8;
								monitor(io_bus_com.sender, io_bus_com.adress, io_bus_com.dp.value.UI8);
								break;

							case 10408: // Öffnungsgrad Außenluftklappe empfangen 
								unit[io_bus_com.sender].freshLvVal.value = io_bus_com.dp.value.UI8;
								monitor(io_bus_com.sender, io_bus_com.adress, io_bus_com.dp.value.UI8);
								break;

							case 10608: // Öffnungsgrad Umluftklappe empfangen 
								unit[io_bus_com.sender].circleLvVal.value = io_bus_com.dp.value.UI8;
								monitor(io_bus_com.sender, io_bus_com.adress, io_bus_com.dp.value.UI8);
								break;

							case 10315: // AE-Betriebsart empfangen 
								unit[io_bus_com.sender].aeMode.value = io_bus_com.dp.value.UI8;
								monitor(io_bus_com.sender, io_bus_com.adress, io_bus_com.dp.value.UI8);
								break;
						}
					}

					// Sonderbehandlung GEp-Regelung mit Differenz-Luftdruckregelung 
					if (DPCisActive() && ge3_active != 0)
					{
						// Lüfterdrehzahl empfangen 
						if (6932 == io_bus_com.adress)
						{
							unit[io_bus_com.sender].fanSpeed.value = io_bus_com.dp.value.UI8;
							monitor(io_bus_com.sender, io_bus_com.adress, io_bus_com.dp.value.UI8);
						}
					}

					// Sonderbehandlung Integraloffset 
					if (1241 == io_bus_com.adress)
					{
						unit[io_bus_com.sender].integralOffset.value = io_bus_com.dp.value.SI16;
						unit[io_bus_com.sender].integralOffset.physUnit = PU_DELTATEMP;
					}
				}
				else
				{
					io_bus_com.dp.physUnit = PU_NV;                                 // ungültig setzen

					if (1241 == io_bus_com.adress)
					{
						unit[io_bus_com.sender].integralOffset.physUnit = PU_NV;
					}
				}
			}
		}

		return (IOBUS_DATA_BLOCKRES_SL);
	//----------------------------------------------------------------------------------------
	case STULZBUS_DATA_ADRREQ:                                                  // Adressed request (Slave -> Master), bei WIB Anfragen über AT
	case IOBUS_DATA_ADRREQ:                                                         // Adressed request (Slave -> Master)

		if (io_bus_com.len < 8) return (0);
		io_bus_com.address_num = (io_bus_com.len - 8) / 2;  // Anzahl der angefragten Adressen bei Adressanfrage

		if (mon_timer_b2)
		{
			OS_WriteString("\r\nB1 AdrRreqS:");
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
			WriteGroesser(); // ">"
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.address_num, 2));     // Wandelt ein long in string
		}
		return (IOBUS_DATA_ADRREQ_SL);                                      // Ein return reicht, das Kommando wird derzeit nicht benutzt
																			//----------------------------------------------------------------------------------------
	case STULZBUS_DATA_ADRRES:                                                  // Adressed response (Slave -> Master), bei WIB Anfragen über AT
	case IOBUS_DATA_ADRRES:                                                         // Adressed response (Slave -> Master)

		if (io_bus_com.len < 8) return (0);
		io_bus_com.address_num = (io_bus_com.len - 8) / 2;  // Anzahl der angefragten Adressen bei Adressanfrage

		if (mon_timer_b2)
		{
			OS_WriteString("\r\nB1 AdrRresS:");
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.sender, 2));        // Wandelt ein long in string
			WriteGroesser(); // ">"
			OS_WriteString(LongToString(&NULL_task_data, io_bus_com.recipient, 2));     // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, io_bus_com.address_num, 2));     // Wandelt ein long in string
		}
		return (IOBUS_DATA_ADRRES_SL);                                      // Ein return reicht, das Kommando wird derzeit nicht benutzt
}
return (0);
//----------------------------------------------------------------------------------------
default:

			if (mon_timer_b1)
{
	OS_WriteString("\r\nB1 ???:");
	a = 0;
	while (a < com_len)
	{
		OS_WriteString(LongToStringHexShort(&NULL_task_data, task_data->str[task_data->str_count][a]));     // Wandelt ein long in string
		WriteSpace();
		a++;
	}
	OS_WriteReturn();
}

return (0);												// Unbekanntes Kommando -> abbruch
	}																			// Ende Switch muss irgendwo return gemacht haben
	if (mon_timer_b1) OS_WriteStringReturn("\r\nNever used");

return (0);												// Unbekanntes Kommando -> abbruch
}

static void monitor(uint8_t sender, uint16_t address, uint8_t value)
{
#if MONITOR == 1
	OS_WriteString("\r\nB1 RresS:");
	OS_WriteString(LongToString(&NULL_task_data, sender, 2));
	WriteSpace();
	OS_WriteString(LongToString(&NULL_task_data, address, 5));
	OS_WriteString(": ");
	OS_WriteStringReturn(LongToString(&NULL_task_data, value, 3));
#endif
}

//***************************************************************************************
//***************************************************************************************

unsigned char CheckRS4852InputSCP(task_data_struct* task_data, unsigned int com_end)
{   // Verarbeitet eingegangenes Kommando auf Stulz Bus
	unsigned int a, b, temp_adress;
	unsigned int checksumCalc;
	unsigned int iAccessbmp, iData, byteofbits;
	unsigned char bitcounter, dp_all_num;
	const unsigned char* dataType;

	bitcounter = 0;

	// Startbyte überspringen oder auch nicht
	a = RS485_2_receive_nach_pointer;

	if (RS485_2_receive_buf[a] == STULZBUS_MSG_DATA ||      // Erstes Byte ist gültiges Kommando
			RS485_2_receive_buf[a] == STULZBUS_MSG_DATARESP)
	{
		b = a;
		b++;
		if (b >= MAX_485_2_RECEIVE_BUFF) b = 0;
		if (RS485_2_receive_buf[b] == STULZBUS_MSG_DATA ||  // Nächstes Byte ist auch gültiges Kommando
				RS485_2_receive_buf[b] == STULZBUS_MSG_DATARESP)
			a = b;
	}
	else                                                                                // Erstes Byte ist kein gültiges Kommando (Startbyte)
	{
		a++;
		if (a >= MAX_485_2_RECEIVE_BUFF) a = 0;
	}

	if (RS485_2_receive_buf[a] != STULZBUS_MSG_DATA &&                      // Hier muss das Kommando anfangen
			RS485_2_receive_buf[a] != STULZBUS_MSG_DATARESP)
	{
		return (0);                                                                             // -> sonst abbruch
	}

	if (PointerDiff(a, com_end, MAX_485_2_RECEIVE_BUFF) < 8) return (0);            //Telegramm zu kurz

	// ESC rausnehmen
	b = 0;                                                                                          // Stringlänge
	while (a != com_end && com_end < MAX_485_2_RECEIVE_BUFF)
	{
		if (RS485_2_receive_buf[a] == ESC)                      // Wenn ESC
		{
			a++;
			if (a >= MAX_485_2_RECEIVE_BUFF) a = 0;
			if (a == com_end) break;                                                // Nicht übers Ende hinausschiessen
		}
		if (b < TASK_STR_LEN) task_data->str[task_data->str_count][b++] = RS485_2_receive_buf[a];
		a++;
		if (a >= MAX_485_2_RECEIVE_BUFF) a = 0;
	}

	// Kommando Auswertung
	switch (task_data->str[task_data->str_count][0])        // Kommando Auswertung
	{
		//----------------------------------------------------------------------------------------
		case STULZBUS_MSG_DATA:             //	the data-message (Master -> Salve)

			if ((task_data->str[task_data->str_count][6]) != 0x02)
			{                   // Länge der folgenden Adresse immer 2
				return (0);
			}
			stulz_bus_com.len = task_data->str[task_data->str_count][4];    // Länge HB
			stulz_bus_com.len <<= 8;
			stulz_bus_com.len |= task_data->str[task_data->str_count][3];   // Länge LB

			if (stulz_bus_com.len > b) stulz_bus_com.len = b;           // Maximale Länge
			if (stulz_bus_com.len < 3) return (0);

			checksumCalc = 0xFFFF;      // initialize checksum
			for (a = 0; a < stulz_bus_com.len - 2; a++)
				checksumCalc = crc16Add(task_data->str[task_data->str_count][a], checksumCalc);
			if (task_data->str[task_data->str_count][stulz_bus_com.len - 2] != ((UI_8)(checksumCalc & 0xFF)))
				return (0); // Falsche CS LB
			if (task_data->str[task_data->str_count][stulz_bus_com.len - 1] != ((UI_8)(checksumCalc >> 8 & 0xFF)))
				return (0); // Falsche CS HB

			stulz_bus_com.adress = task_data->str[task_data->str_count][8];
			stulz_bus_com.adress <<= 8;
			stulz_bus_com.adress |= task_data->str[task_data->str_count][7];

			stulz_bus_com.number = task_data->str[task_data->str_count][10];
			stulz_bus_com.number <<= 8;
			stulz_bus_com.number |= task_data->str[task_data->str_count][9];

			stulz_bus_com.slave = task_data->str[task_data->str_count][2];  // HB of slave address
			stulz_bus_com.slave <<= 8;
			stulz_bus_com.slave |= task_data->str[task_data->str_count][1]; // LB of slave address

			switch (task_data->str[task_data->str_count][5])
			{
				//--------------------------------------------------------------------------------------
				//--------------------------------------------------------------------------------------
				case STULZBUS_DATA_BLOCKREQ:            // Data block request Anfrage (Master -> Slave)

					//	for (a = 0 ; a <= stulz_bus_com.len ; a++)
					//	{
					//		task_data->str_count++;
					//		OS_WriteString (LongToStringHex (task_data,task_data->str[task_data->str_count-1][a], 2));
					//		task_data->str_count--;
					//		WriteSpace();
					//	}
					//	OS_WriteReturn ();

					if (mon_timer_b2)
					{
						OS_WriteString("\r\nB2 RreqM:");
						OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));      // Wandelt ein long in string
						OS_WriteString(">:");
						OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.adress, 5));     // Wandelt ein long in string
						WriteDpkt(); // ":"
						OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.number, 3));       // Wandelt ein long in string
					}
					return (STULZBUS_DATA_BLOCKREQ_MA);
				//--------------------------------------------------------------------------------------
				case STULZBUS_DATA_BLOCKRES:            // Data block response (Master -> Slave)

					if (mon_timer_b2)
					{
						OS_WriteString("\r\nB2 RresM:");
						OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));      // Wandelt ein long in string
						OS_WriteString(">:");
						OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.adress, 5));     // Wandelt ein long in string
						WriteDpkt(); // ":"
						OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.number, 3));       // Wandelt ein long in string
					}

					if (stulz_bus_com.slave == glob_adresse.value ||        // Bin ich gemeint oder
							stulz_bus_com.slave == 0xFFFF)                                  // Broadcastadresse
					{
						//						iAccessbmp = 11;				// start of accessibility-bitmap;
						bitcounter = 0;
						iData = 11 + (stulz_bus_com.number + 7) / 8;                // start of databytes after accessibility-bitmap;

						b = 0;                                                                                  // Tatsächlich geschriebene DP

						OS_Use(&DP_all);                                                                // Einen DP all reservieren
						dp_all_num = BlockDPall();
						OS_Unuse(&DP_all);

						for (a = 0; a < stulz_bus_com.number; a++)
						{
							temp_adress = stulz_bus_com.adress + a;

							dataType = DPAdr(temp_adress);

							if (dataType != NULL)
							{
#if DEBUG == 1
OS_WriteString ("-j");		//
#endif
								GetDPnum(dataType, dp_all_num);                     // Put DP in DPall

								//								if (task_data->str[task_data->str_count][iAccessbmp + a/8] & (0x01 << (a%8)))
								if (DP_ALL[dp_all_num].access & A_IO_WO)
								{
									// valid result received

									switch (*dataType & 0x07)
									{
										case DT_BIT1:
											// 8 datapoints are combined in one byte => split them;
											// lowest datapoint is LSB

											if (!bitcounter)
											{
												// we need a new byte :-)
												byteofbits = iData++;               // Am Anfang 12
											}
											if (task_data->str[task_data->str_count][byteofbits] & (0x01 << bitcounter))
											{
												if (DP_ALL[dp_all_num].value.UI8 != 1)          // Ist Wert schon gleich
												{
													//													OS_WriteString ("BIT 2 changed 1:");					//
													//													OS_WriteString (LongToString (&NULL_task_data,stulz_bus_com.adress, 5));		// 
													//													if (*dataType & DP_PROP_FLASH_ADR)	// DP hat keine Flash Adresse
													//														OS_WriteStringReturn (" FLASH!!!");					//
													//													else
													//														OS_WriteReturn ();					//

													DP_ALL[dp_all_num].value.UI8 = 1;
													PutDP(dataType, dp_all_num);
													WriteFlashConfigS(dataType);
												}
											}
											else
											{
												if (DP_ALL[dp_all_num].value.UI8 != 0)          // Ist Wert schon gleich
												{
													//													OS_WriteString ("BIT 2 changed 0:");					//
													//													OS_WriteString (LongToString (&NULL_task_data,stulz_bus_com.adress, 5));		// 
													//													if (*dataType & DP_PROP_FLASH_ADR)	// DP hat keine Flash Adresse
													//														OS_WriteStringReturn (" FLASH!!!");					//
													//													else
													//														OS_WriteReturn ();					//

													DP_ALL[dp_all_num].value.UI8 = 0;
													PutDP(dataType, dp_all_num);
													WriteFlashConfigS(dataType);
												}
											}
											bitcounter++;   // do not reset here!!
											if (bitcounter > 7) bitcounter = 0;
											b++;                                                                    // DP tatsächlich geschrieben
											break;

										case DT_SI8:
										case DT_UI8:
											if (DP_ALL[dp_all_num].value.UI8 !=             // Ist Wert schon gleich
													(unsigned char)task_data->str[task_data->str_count][iData])
											{
	//												OS_WriteString ("UI8 2 changed:");					//
	//												OS_WriteString (LongToString (&NULL_task_data,stulz_bus_com.adress, 5));		// 
	//												if (*dataType & DP_PROP_FLASH_ADR)	// DP hat keine Flash Adresse
	//													OS_WriteStringReturn (" FLASH!!!");					//
	//												else
	//													OS_WriteReturn ();					//

	DP_ALL[dp_all_num].value.UI8 = task_data->str[task_data->str_count][iData];
	//												if (DP_ALL[dp_all_num].value.UI8 >= DP_ALL[dp_all_num].min.UI8 &&
	//														DP_ALL[dp_all_num].value.UI8 <= DP_ALL[dp_all_num].max.UI8)
	//												{
	PutDP(dataType, dp_all_num);
	WriteFlashConfigS(dataType);
	//												}
	//												else
	//													OS_WriteStringReturn ("Grenze UI8 2");
}
iData += 1;
bitcounter = 0;
b++;                                                                    // DP tatsächlich geschrieben
break;

										case DT_SI16:
										case DT_UI16:
if (DP_ALL[dp_all_num].value.UI16 !=                // Ist Wert schon gleich
	((unsigned int)task_data->str[task_data->str_count][iData] |
	 (unsigned int)task_data->str[task_data->str_count][iData + 1] << 8))
											{
	//												OS_WriteString ("UI16 2 changed:");					//
	//												OS_WriteString (LongToString (&NULL_task_data,stulz_bus_com.adress, 5));		// 
	//												if (*dataType & DP_PROP_FLASH_ADR)	// DP hat keine Flash Adresse
	//													OS_WriteStringReturn (" FLASH!!!");					//
	//												else
	//													OS_WriteReturn ();					//

	DP_ALL[dp_all_num].value.UI16 = task_data->str[task_data->str_count][iData];
	DP_ALL[dp_all_num].value.UI16 |= task_data->str[task_data->str_count][iData + 1] << 8;
	//												if (DP_ALL[dp_all_num].value.UI16 >= DP_ALL[dp_all_num].min.UI16 &&
	//														DP_ALL[dp_all_num].value.UI16 <= DP_ALL[dp_all_num].max.UI16)
	//												{
	PutDP(dataType, dp_all_num);
	WriteFlashConfigS(dataType);
	//												}
	//												else
	//													OS_WriteStringReturn ("Grenze UI16 2");
}
iData += 2;
bitcounter = 0;
b++;                                                                    // DP tatsächlich geschrieben
break;

										case DT_SI32:
										case DT_UI32:
if (DP_ALL[dp_all_num].value.UI32 !=                // Ist Wert schon gleich
		((unsigned long)task_data->str[task_data->str_count][iData] |
		((unsigned long)task_data->str[task_data->str_count][iData + 1] << 8)		|
		  ((unsigned long)task_data->str[task_data->str_count][iData + 2] << 16)	|
			(unsigned long)task_data->str[task_data->str_count][iData + 3] << 24))
											{
	//												OS_WriteString ("UI32 2 changed:");					//
	//												OS_WriteString (LongToString (&NULL_task_data,stulz_bus_com.adress, 5));		//
	//												if (*dataType & DP_PROP_FLASH_ADR)	// DP hat keine Flash Adresse
	//													OS_WriteStringReturn (" FLASH!!!");					//
	//												else
	//													OS_WriteReturn ();					//

	DP_ALL[dp_all_num].value.UI32 = (unsigned long)task_data->str[task_data->str_count][iData];
	DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 1] << 8;
	DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 2] << 16;
	DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 3] << 24;
	//												if (DP_ALL[dp_all_num].value.UI32 >= DP_ALL[dp_all_num].min.UI32 &&
	//														DP_ALL[dp_all_num].value.UI32 <= DP_ALL[dp_all_num].max.UI32)
	//												{
	PutDP(dataType, dp_all_num);
	WriteFlashConfigS(dataType);
	//												}
	//												else
	//													OS_WriteStringReturn ("Grenze UI32 2");
}
iData += 4;
bitcounter = 0;
b++;                                                                    // DP tatsächlich geschrieben
break;
									} // switch (DPAdr(adress + a)->dataType)

									SetGLTSensor(temp_adress);				// Änderung der Sensor GLT-Werte führt zur Aktivierung

								}																						// if (DP_ALL[dp_all_num].access & A_IO_WO)
							}																							// if (dataType != NULL)
						}																								// for (a = 0; a < number; a++)
						RELEASEDPALL(dp_all_num);                                               // DPall wieder freigeben
stulz_bus_com.number = b;												// tatsächlich geschriebene DP übernehmen
					}																									// if (stulz_bus_com.recipient == adresse.value)		// Bin ich gemeint
					return (STULZBUS_DATA_BLOCKRES_MA);
//----------------------------------------------------------------------------------------
				case STULZBUS_DATA_ADRREQ:                                                  // Adressed request (Master -> Slave)
if (stulz_bus_com.len < 9) return (0);
stulz_bus_com.address_num = (stulz_bus_com.len - 9) / 2;    // Anzahl der angefragten Adressen bei Adressanfrage

if (mon_timer_b2)
{
	OS_WriteString("\r\nB2 AdrReqM:");
	OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));      // Wandelt ein long in string
	WriteDpkt(); // ":"
	OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.address_num, 2));        // Wandelt ein long in string
}

if (stulz_bus_com.address_num)
{
	for (a = 0; a < stulz_bus_com.address_num; a++)
	{
		stulz_bus_com.adress2[a] = task_data->str[task_data->str_count][8 + a * 2] * 256;   // Adressen einlesen HB
		stulz_bus_com.adress2[a] += task_data->str[task_data->str_count][7 + a * 2];            // Adressen einlesen LB

		if (mon_timer_b2)
		{
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.adress2[a], 5));       // Wandelt ein long in string
		}
	}
}

if (mon_timer_b2) OS_WriteReturn();

return (STULZBUS_DATA_ADRREQ_MA);
//----------------------------------------------------------------------------------------
				case STULZBUS_DATA_ADRRES:                                                  // Adressed response (Master -> Slave)
if (stulz_bus_com.len < 9) return (0);

a = (stulz_bus_com.len - 9) / 2;                                            // Anzahl der angefragten Adressen bei Adressanfrage
stulz_bus_com.address_num = 0;                                      // Richtige Anzahl aller gültigen Adressen

if (mon_timer_b2)
{
	OS_WriteString("\r\nB2 AdrRresM:");
	OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));      // Wandelt ein long in string
	WriteDpkt(); // ":"
	OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.address_num, 2));      // Wandelt ein long in string
}

if (stulz_bus_com.slave == glob_adresse.value ||    // Bin ich gemeint oder
		stulz_bus_com.slave == 0xFFFF)                              // Broadcastadresse
{
	if (a)                                                                                  // Sind überhaupt Adressen angegeben
	{
		iData = 7;                                                                      // Erste Adresse

		OS_Use(&DP_all);                                                            // Einen DP all reservieren
		dp_all_num = BlockDPall();
		OS_Unuse(&DP_all);

		while (iData < 300 && iData < stulz_bus_com.len - 2)    // Bis zum Ende
		{
			stulz_bus_com.adress = task_data->str[task_data->str_count][iData + 1] * 256;   // Adresse einlesen HB
			stulz_bus_com.adress += task_data->str[task_data->str_count][iData];                // Adresse einlesen LB

			dataType = DPAdr(stulz_bus_com.adress);     // Adresse des DP holen

			if (dataType != NULL)
			{
#if DEBUG == 1
OS_WriteString ("-k");		//
#endif
				GetDPnum(dataType, dp_all_num);                 // DP holen

				//									if (DP_ALL[dp_all_num].access & A_IO_WO)	// Darf ich in diesen DP schreiben
				//									{
				stulz_bus_com.adress2[stulz_bus_com.address_num] = stulz_bus_com.adress;    // Gültige Adresse merken
				stulz_bus_com.address_num++;                        // Anzahl gültiger Adressen hochzählen

				switch (*dataType & 0x07)                               // Unterscheidung nach DP Typ
				{
					case DT_BIT1:                                                   // 8 Bits
					case DT_SI8:
					case DT_UI8:
						if (DP_ALL[dp_all_num].value.UI8 != // Ist Wert schon gleich
							(unsigned char)task_data->str[task_data->str_count][iData + 2] &&
							DP_ALL[dp_all_num].access & A_IO_WO)	// Schreibberechtigt
												{
							DP_ALL[dp_all_num].value.UI8 = task_data->str[task_data->str_count][iData + 2]; // Wert übernehmen

							PutDP(dataType, dp_all_num);            // Wert zurückschreiben
							WriteFlashConfigS(dataType);            // Wert im Flash speichern
						}
						iData += 3;                                                 // Auf nächste Adresse Stellen
						break;

					case DT_SI16:                                                   // 16 bits
					case DT_UI16:
						if (DP_ALL[dp_all_num].value.UI16 !=    // Ist Wert schon gleich
							((unsigned int)task_data->str[task_data->str_count][iData + 2] |
							 (unsigned int)task_data->str[task_data->str_count][iData + 3] << 8) &&
							  DP_ALL[dp_all_num].access & A_IO_WO)	// Schreibberechtigt
												{
							DP_ALL[dp_all_num].value.UI16 = task_data->str[task_data->str_count][iData + 2];
							DP_ALL[dp_all_num].value.UI16 |= task_data->str[task_data->str_count][iData + 3] << 8;

							PutDP(dataType, dp_all_num);
							WriteFlashConfigS(dataType);
						}
						iData += 4;                                                 // Auf nächste Adresse stellen
						break;

					case DT_SI32:                                                   // 32 bits
					case DT_UI32:
						if (DP_ALL[dp_all_num].value.UI32 !=    // Ist Wert schon gleich
								((unsigned long)task_data->str[task_data->str_count][iData + 2] |
								((unsigned long)task_data->str[task_data->str_count][iData + 3] << 8)		|
								  ((unsigned long)task_data->str[task_data->str_count][iData + 4] << 16)	|
									 (unsigned long)task_data->str[task_data->str_count][iData + 5] << 24) &&
								  DP_ALL[dp_all_num].access & A_IO_WO)	// Schreibberechtigt
												{
							DP_ALL[dp_all_num].value.UI32 = (unsigned long)task_data->str[task_data->str_count][iData + 2];
							DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 3] << 8;
							DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 4] << 16;
							DP_ALL[dp_all_num].value.UI32 |= (unsigned long)task_data->str[task_data->str_count][iData + 5] << 24;

							PutDP(dataType, dp_all_num);
							WriteFlashConfigS(dataType);
						}
						iData += 6;                                                 // Auf nächste Adresse stellen
						break;

					default:                                                            // Ungültiger Typ -> Abbruch
						RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
						return (STULZBUS_DATA_ADRRES_MA);

				}                                                                           // switch (*dataType & 0x07)
																							//									}																					// if (DP_ALL[dp_all_num].access & A_IO_WO)
			}                                                                                       // if (dataType != NULL)
			else                                                                                // DP unbekannt -> Abbruch
			{
				RELEASEDPALL(dp_all_num);                                   // DPall wieder freigeben
				return (STULZBUS_DATA_ADRRES_MA);
			}
		}                                                                                           // while (iData < 300 && iData < stulz_bus_com.len-2)
		RELEASEDPALL(dp_all_num);                                           // DPall wieder freigeben
	}                                                                                               // if (stulz_bus_com.address_num)
}                                                                                                   // if (stulz_bus_com.recipient == adresse.value)		// Bin ich gemeint
return (STULZBUS_DATA_ADRRES_MA);
//----------------------------------------------------------------------------------------
			}						// switch (task_data->str[task_data->str_count][5])
			return (0); // case STULZBUS_MSG_DATA:
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
		case STULZBUS_MSG_DATARESP:             //	the data-message (Slave -> Master)

if ((task_data->str[task_data->str_count][6]) != 0x02)
{                   // Länge der folgenden Adresse immer 2
	return (0);
}
stulz_bus_com.len = task_data->str[task_data->str_count][4];    // Länge HB
stulz_bus_com.len <<= 8;
stulz_bus_com.len |= task_data->str[task_data->str_count][3];   // Länge LB

if (stulz_bus_com.len > b) stulz_bus_com.len = b;           // Maximale Länge
if (stulz_bus_com.len < 3) return (0);

checksumCalc = 0xFFFF;      // initialize checksum
for (a = 0; a < stulz_bus_com.len - 2; a++)
	checksumCalc = crc16Add(task_data->str[task_data->str_count][a], checksumCalc);
if (task_data->str[task_data->str_count][stulz_bus_com.len - 2] != ((UI_8)(checksumCalc & 0xFF)))
	return (0); // Falsche CS LB
if (task_data->str[task_data->str_count][stulz_bus_com.len - 1] != ((UI_8)(checksumCalc >> 8 & 0xFF)))
	return (0); // Falsche CS HB

stulz_bus_com.adress = task_data->str[task_data->str_count][8];
stulz_bus_com.adress <<= 8;
stulz_bus_com.adress |= task_data->str[task_data->str_count][7];

stulz_bus_com.number = task_data->str[task_data->str_count][10];
stulz_bus_com.number <<= 8;
stulz_bus_com.number |= task_data->str[task_data->str_count][9];

if (!stulz_bus_com.number) return (0);                                  // Anzahl 0 geht nicht

stulz_bus_com.slave = task_data->str[task_data->str_count][2];  // Absender eintragen
stulz_bus_com.slave <<= 8;
stulz_bus_com.slave |= task_data->str[task_data->str_count][1]; // Empfänger eintragen

switch (task_data->str[task_data->str_count][5])
{
	//--------------------------------------------------------------------------------------
	case STULZBUS_DATA_BLOCKREQ:            // Data block request (Slave -> Master)
		if (mon_timer_b2)
		{
			OS_WriteString("\r\nB2 RreqS:");
			OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));      // Wandelt ein long in string
			OS_WriteString(">:");
			OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.adress, 5));     // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.number, 3));       // Wandelt ein long in string
		}
		return (STULZBUS_DATA_BLOCKREQ_SL);
	//--------------------------------------------------------------------------------------
	case STULZBUS_DATA_BLOCKRES:            // Data block response (Slave -> Master)

		if (mon_timer_b2)
		{
			OS_WriteString("\r\nB2 RresS:");
			OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));      // Wandelt ein long in string
			OS_WriteString(">:");
			OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.adress, 5));     // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.number, 3));       // Wandelt ein long in string
		}
		// Bisher nur für 1 DP geschrieben

		dataType = DPAdr(stulz_bus_com.adress);

#if DEBUG == 1
OS_WriteString ("-r");		//
#endif
		dp_all_num = GetDP(dataType);

		CopyDP(&stulz_bus_com.dp.dataType, &DP_ALL[dp_all_num].dataType);   // Kopiert DP Einheit , Zugriffsrechte usw.

		if (stulz_bus_com.slave == glob_adresse.value ||            // Bin ich gemeint oder
				stulz_bus_com.slave == 0xFFFF)                                      // Broadcastadresse
		{
			bitcounter = 0;
			iAccessbmp = 11;                // start of accessibility-bitmap;
			iData = 11 + (stulz_bus_com.number + 7) / 8;        // start of databytes after accessibility-bitmap;

			if (task_data->str[task_data->str_count][iAccessbmp] & 0x01 << bitcounter)  // Datum gültig
			{
				switch (*dataType & 0x07)
				{
					case DT_BIT1:
					case DT_UI8:
					case DT_SI8:
						stulz_bus_com.dp.value.UI8 = task_data->str[task_data->str_count][12];
						break;
					default:
					case DT_UI16:
					case DT_SI16:
						stulz_bus_com.dp.value.UI16 = task_data->str[task_data->str_count][13];
						stulz_bus_com.dp.value.UI16 <<= 8;
						stulz_bus_com.dp.value.UI16 |= task_data->str[task_data->str_count][12];
						break;
					case DT_UI32:
					case DT_SI32:
						stulz_bus_com.dp.value.UI32 = task_data->str[task_data->str_count][15];
						stulz_bus_com.dp.value.UI32 <<= 8;
						stulz_bus_com.dp.value.UI32 |= task_data->str[task_data->str_count][14];
						stulz_bus_com.dp.value.UI32 <<= 8;
						stulz_bus_com.dp.value.UI32 |= task_data->str[task_data->str_count][13];
						stulz_bus_com.dp.value.UI32 <<= 8;
						stulz_bus_com.dp.value.UI32 |= task_data->str[task_data->str_count][12];
						//								case DT_FP32:
						// Noch zu schreiben
						//									break;
				}
			}
			else
				stulz_bus_com.dp.physUnit = PU_NV;                                  // ungültig setzen
		}

		RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
		return (STULZBUS_DATA_BLOCKRES_SL);
	//----------------------------------------------------------------------------------------
	case STULZBUS_DATA_ADRREQ:                                                          // Adressed request (Slave -> Master)
		if (stulz_bus_com.len < 8) return (0);
		stulz_bus_com.address_num = (stulz_bus_com.len - 8) / 2;    // Anzahl der angefragten Adressen bei Adressanfrage

		if (mon_timer_b2)
		{
			OS_WriteString("\r\nB2 AdrRreqS:");
			OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));      // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.address_num, 2));      // Wandelt ein long in string
		}
		return (STULZBUS_DATA_ADRREQ_SL);
	//----------------------------------------------------------------------------------------
	case STULZBUS_DATA_ADRRES:                                                          // Adressed response (Slave -> Master)
		if (stulz_bus_com.len < 8) return (0);
		stulz_bus_com.address_num = (stulz_bus_com.len - 8) / 2;    // Anzahl der angefragten Adressen bei Adressanfrage

		if (mon_timer_b2)
		{
			OS_WriteString("\r\nB2 AdrRresS:");
			OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.slave, 2));      // Wandelt ein long in string
			WriteDpkt(); // ":"
			OS_WriteStringReturn(LongToString(&NULL_task_data, stulz_bus_com.address_num, 2));      // Wandelt ein long in string
		}
		return (STULZBUS_DATA_ADRRES_SL);
		//----------------------------------------------------------------------------------------
}   // switch (task_data->str[task_data->str_count][5])
break;
	}	// switch (task_data->str[task_data->str_count][0])		// Kommando Auswertung
	return (0);
}

//***************************************************************************************
//***************************************************************************************

unsigned char CheckRS4852InputModbus(task_data_struct* task_data, unsigned int com_end)
{   // Verarbeitet eingegangenes Kommando auf Stulz Bus
	unsigned int a, b, cs16;
	unsigned char* dataType, dp_all_num;
	float valueFloat;           // value of datapoint converted to float for transmission;
	tu_dtype tempvalue;

	if (PointerDiff(RS485_2_receive_nach_pointer, com_end, MAX_485_2_RECEIVE_BUFF) < 8)         //Telegramm zu kurz
		return (MODBUS_SKIP);

	// ------------------------------------------------------------------
	// copy from ring buffer
	a = RS485_2_receive_nach_pointer;
	b = 0;                                                                                          // Stringlänge

	while (a != com_end && com_end < MAX_485_2_RECEIVE_BUFF)
	{
		if (b < TASK_STR_LEN) task_data->str[task_data->str_count][b++] = RS485_2_receive_buf[a];
		a++;
		if (a >= MAX_485_2_RECEIVE_BUFF) a = 0;
	}

	stulz_bus_com.len = b;

	//-------------------------------------------------------------------
	// calculate checksum
	cs16 = 0xFFFF;
	for (a = 0; a <= stulz_bus_com.len - 3; a++)
		cs16 = crc16Add2(task_data->str[task_data->str_count][a], cs16);
	if (task_data->str[task_data->str_count][stulz_bus_com.len - 2] != ((UI_8)(cs16 & 0xFF)))
		return (MODBUS_SKIP);   // Falsche CS LB
	if (task_data->str[task_data->str_count][stulz_bus_com.len - 1] != ((UI_8)(cs16 >> 8 & 0xFF)))
		return (MODBUS_SKIP);   // Falsche CS HB

	//------------------------------------------------
	// copy necessary information
	stulz_bus_com.slave = task_data->str[task_data->str_count][0];  // Empfänger eintragen
	stulz_bus_com.modbus_datcmd = task_data->str[task_data->str_count][1];  // Modbus Function

	// ------------------------------------------------------------------
	// Message for me?
	if (stulz_bus_com.slave != glob_adresse.value)                      // Ich bin nicht gemeint
		return (MODBUS_SKIP);

	// ------------------------------------------------------------------
	// unit address is out of range
	if ((stulz_bus_com.slave <= 0) || (248 <= stulz_bus_com.slave))
		return (MODBUS_SKIP);

	// ------------------------------------------------------------------
	// modbus cannot return more than 127 words / 255 bytes due to the limited data byte count field
	if ((stulz_bus_com.modbus_datnum >= 128) && ((stulz_bus_com.modbus_datcmd == 3) || (stulz_bus_com.modbus_datcmd == 4)))
		return (MODBUS_ILL_ADDRESS);

	//this modbus implementation cannot return more than 256 bits = 32 bytes
	else if ((stulz_bus_com.modbus_datnum > 256) && ((stulz_bus_com.modbus_datcmd == 1) || (stulz_bus_com.modbus_datcmd == 2)))
		return (MODBUS_ILL_ADDRESS);

	// ---------------------------------------------------------------------
	if (mon_timer_b2)
	{
		OS_WriteString("B2 req:");
		OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.slave, 5));      // Absender
		OS_WriteString(",");
		OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.modbus_datstart, 2));        // Adresse
		OS_WriteString(",");
		OS_WriteString(LongToString(&NULL_task_data, stulz_bus_com.modbus_datnum, 2));      // Anzahl DP
		OS_WriteString(",");
	}
	// ---------------------------------------------------------------------
	// choose command type function
	switch (stulz_bus_com.modbus_datcmd)
	{
		case 1: // Function 1 : Read Coil Status
		case 2: // Function 2 : Read input Status
		case 3: // Function 3 : Read holding Register
		case 4: // Function 4 : Read input Registers
			stulz_bus_com.modbus_datstart = (task_data->str[task_data->str_count][2] * 256) // start address of datapoint block
																		+ task_data->str[task_data->str_count][3];
			stulz_bus_com.modbus_datnum = (task_data->str[task_data->str_count][4] * 256)   // no of datapoints
																		+ task_data->str[task_data->str_count][5];
			if (1 == stulz_bus_com.modbus_datcmd) return (MODBUS_FUNCTION_1_READ_COIL_STATUS);
			if (2 == stulz_bus_com.modbus_datcmd) return (MODBUS_FUNCTION_2_READ_INPUT_STATUS);
			if (3 == stulz_bus_com.modbus_datcmd) return (MODBUS_FUNCTION_3_READ_HOLDING_REGISTER);
			return (MODBUS_FUNCTION_4_READ_INPUT_REGISTER);
		//-----------------------------------------------------------------------------------------------
		case 5: // Function 5 : Force single Coil
			if (mon_timer_b2)
			{
				OS_WriteString("\r\n Modbus req 5:");
			}

			stulz_bus_com.modbus_datstart = (task_data->str[task_data->str_count][2] * 256) // start address of datapoint block
																		+ task_data->str[task_data->str_count][3];
			stulz_bus_com.modbus_value = (task_data->str[task_data->str_count][4] * 256)    // no of datapoints
																		+ task_data->str[task_data->str_count][5];

			switch (datapointlist.value)                                            // Welches Modbus Protokoll
			{
				case 1:                                                                                 // Modbus classic
					if (stulz_bus_com.modbus_datstart >= MODBUS_LAST_MAPPED_DP_5 ||     // Adresse zu gross
							mod_func_5_map[stulz_bus_com.modbus_datstart] == -1)                    // Adresse ungültig
					{
						SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
						return (MODBUS_SKIP);
					}
					dataType = DPAdr(mod_func_5_map[stulz_bus_com.modbus_datstart]);                            // Pointer auf DP holen

					SetGLTSensor(mod_func_5_map[stulz_bus_com.modbus_datstart]);                // Änderung der Sensor GLT-Werte führt zur Aktivierung
					break;

				case 2:                                                                                             // Modbus Full size
					dataType = DPAdr(stulz_bus_com.modbus_datstart);
					if ((*dataType & 0x07) != DT_BIT1)                                  // DP ist kein Bit
					{
						SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
						return (MODBUS_SKIP);
					}
					SetGLTSensor(stulz_bus_com.modbus_datstart);                // Änderung der Sensor GLT-Werte führt zur Aktivierung
					break;

				case 3:                                                                                             // Modbus Custom
					if (stulz_bus_com.modbus_datstart > stulz_bus_com.last_vardp - DP_VAR_START)
					{
						SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
						return (MODBUS_SKIP);
					}

					dataType = DPAdr(stulz_bus_com.first_vardp_bit + stulz_bus_com.modbus_datstart);

					if ((*dataType & 0x07) != DT_BIT1)                                  // DP ist kein Bit
					{
						SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
						return (MODBUS_SKIP);
					}
					SetGLTSensor(stulz_bus_com.first_vardp_bit + stulz_bus_com.modbus_datstart);                // Änderung der Sensor GLT-Werte führt zur Aktivierung
					break;

				default:                                                                                // Keine gültige Datenpunktliste eingestellt
					SendModbusError(task_data, stulz_bus_com.slave, MODBUS_DEVICE_FAIL);    // Sendet einen Modbus Fehler
					return (MODBUS_SKIP);
			}

			if (dataType == NULL)                                                                   // DP nicht belegt -> Fehler
			{
				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
				return (MODBUS_SKIP);
			}

#if DEBUG == 1
OS_WriteString ("-s");		//
#endif
			dp_all_num = GetDP(dataType);   // Put DP in DPall

			if (DP_ALL[dp_all_num].access & A_IO_WO)
			{

				if ((stulz_bus_com.modbus_value != 0xFF00) && (stulz_bus_com.modbus_value != 0x0000))   // wrong value (1, 0)
				{
					SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_VALUE);  // Sendet einen Modbus Fehler
					RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
					return (MODBUS_SKIP);
				}
				if (stulz_bus_com.modbus_value) stulz_bus_com.modbus_value = 1; // Zum DP beschreiben auf 1 setzen
				if (stulz_bus_com.modbus_value != DP_ALL[dp_all_num].value.UI8)
				{
					DP_ALL[dp_all_num].value.UI8 = (unsigned char)stulz_bus_com.modbus_value;
PutDP(dataType, dp_all_num);
WriteFlashConfigS(dataType);
				}
				if (stulz_bus_com.modbus_value) stulz_bus_com.modbus_value = 0xFF00;	// Ursprünglichen Wert wieder herstellen
			}
			else
{
	SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
	RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
	return (MODBUS_SKIP);
}

RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben

return (MODBUS_FUNCTION_5_FORCE_SINGLE_COIL);
//-----------------------------------------------------------------------------------------------
		case 8: // Funktion 8 : Loopback Test
			if (mon_timer_b2)
{
	OS_WriteString("\r\n Modbus req 8:");
}

stulz_bus_com.modbus_diag_code = (task_data->str[task_data->str_count][2] * 256)    // DIAG code
																+ task_data->str[task_data->str_count][3];
stulz_bus_com.modbus_value = (task_data->str[task_data->str_count][4] * 256)    // Wert
																+ task_data->str[task_data->str_count][5];

return (MODBUS_FUNCTION_8_LOOPBACK_TEST);
//-----------------------------------------------------------------------------------------------
		case 16: // Funktion 16 : Write multiple Registers
			if (mon_timer_b2)
{
	OS_WriteString("\r\n Modbus req 16:");
}

stulz_bus_com.modbus_datstart = (task_data->str[task_data->str_count][2] * 256) // start address of datapoint block
															+ task_data->str[task_data->str_count][3];
stulz_bus_com.modbus_datnum = (task_data->str[task_data->str_count][4] * 256)   // no of datapoints
															+ task_data->str[task_data->str_count][5];

if (stulz_bus_com.modbus_datstart & 1)                      // Adresse muss gerade sein
{
	SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
	return (MODBUS_SKIP);
}
if (stulz_bus_com.modbus_datnum != 2)                           // Wir schreiben nur einen DP = 2 Wörter
{
	SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
	return (MODBUS_SKIP);
}

switch (datapointlist.value)                                            // Welches Modbus Protokoll
{
	case 1:                                                                                 // Modbus classic
		if (stulz_bus_com.modbus_datstart / 2 >= MODBUS_LAST_MAPPED_DP_3_16 ||  // Adresse zu gross
				mod_func_3_16_map[stulz_bus_com.modbus_datstart / 2] == -1)                     // Adresse ungültig
		{
			SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
			return (MODBUS_SKIP);
		}
		dataType = DPAdr(mod_func_3_16_map[stulz_bus_com.modbus_datstart / 2]);                         // Pointer auf DP holen

		break;

	case 2:                                                                                             // Modbus Full size
		dataType = DPAdr(stulz_bus_com.modbus_datstart / 2);
		if (dataType == NULL)                                                               // DP unbekannt
		{
			SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
			return (MODBUS_SKIP);
		}
		break;

	case 3:                                                                                             // Modbus Custom
		if (stulz_bus_com.modbus_datstart / 2 > stulz_bus_com.last_vardp - DP_VAR_START)
		{
			SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
			return (MODBUS_SKIP);
		}

		dataType = DPAdr(stulz_bus_com.first_vardp_ana + stulz_bus_com.modbus_datstart / 2);

		if (dataType == NULL)                                                               // DP unbekannt
		{
			SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
			return (MODBUS_SKIP);
		}
		break;

	default:                                                                                // Keine gültige Datenpunktliste eingestellt
		SendModbusError(task_data, stulz_bus_com.slave, MODBUS_DEVICE_FAIL);    // Sendet einen Modbus Fehler
		return (MODBUS_SKIP);
}

if (dataType == NULL)                                                                   // DP nicht belegt -> Fehler
{
	SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
	return (MODBUS_SKIP);
}

#if DEBUG == 1
OS_WriteString ("-t");		//
#endif
dp_all_num = GetDP(dataType);   // Put DP in DPall

if (DP_ALL[dp_all_num].access & A_IO_WO)                            // Schreibberechtigt
{
	// ---------------------------------------------------------	
	// 1. sum up value
	*(((unsigned char*)&valueFloat)+3) = task_data->str[task_data->str_count][7];
	*(((unsigned char*)&valueFloat)+2) = task_data->str[task_data->str_count][8];
	*(((unsigned char*)&valueFloat)+1) = task_data->str[task_data->str_count][9];
	*(((unsigned char*)&valueFloat)  ) = task_data->str[task_data->str_count][10];

	// ---------------------------------------------------------
	// 2. Multiplications depending on physical unit:
	valueFloat *= GetPhysUnitFactor(DP_ALL[dp_all_num].physUnit);
	// ---------------------------------------------------------
	// 3. convert float to correct datapoint
	// 4. check range
	tempvalue.UI32 = 0;

	switch (*dataType & 0x07)
	{
		case DT_UI8:
			tempvalue.UI8 = (unsigned char)valueFloat;

			if (tempvalue.UI8 < DP_ALL[dp_all_num].min.UI8 ||
					tempvalue.UI8 > DP_ALL[dp_all_num].max.UI8)
			{
#if DEBUG == 1
OS_WriteStringReturn ("mod:AAA");		//
#endif
				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_VALUE);  // Sendet einen Modbus Fehler
				RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
				return (MODBUS_SKIP);
			}

			if (tempvalue.UI8 != DP_ALL[dp_all_num].value.UI8)
			{
#if DEBUG == 1
OS_WriteStringReturn ("mod:Mod 16 UI8 changed");		//
#endif
				DP_ALL[dp_all_num].value.UI8 = (unsigned char)tempvalue.UI8;
				PutDP(dataType, dp_all_num);
				WriteFlashConfigS(dataType);
			}
			break;

		case DT_SI8:
			tempvalue.SI8 = (char)valueFloat;

			if (tempvalue.SI8 < DP_ALL[dp_all_num].min.SI8 ||
					tempvalue.SI8 > DP_ALL[dp_all_num].max.SI8)
			{
#if DEBUG == 1
OS_WriteStringReturn ("mod:BBB");		//
#endif
				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_VALUE);  // Sendet einen Modbus Fehler
				RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
				return (MODBUS_SKIP);
			}

			if (tempvalue.SI8 != DP_ALL[dp_all_num].value.SI8)
			{
#if DEBUG == 1
OS_WriteStringReturn ("mod:Mod 16 SI8 changed");		//
#endif
				DP_ALL[dp_all_num].value.SI8 = (char)tempvalue.SI8;
				PutDP(dataType, dp_all_num);
				WriteFlashConfigS(dataType);
			}
			break;

		case DT_UI16:
			tempvalue.UI16 = (unsigned int)valueFloat;

			if (tempvalue.UI16 < DP_ALL[dp_all_num].min.UI16 ||
					tempvalue.UI16 > DP_ALL[dp_all_num].max.UI16)
			{
#if DEBUG == 1
OS_WriteStringReturn ("mod:ccc");		//
#endif
				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_VALUE);  // Sendet einen Modbus Fehler
				RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
				return (MODBUS_SKIP);
			}

			if (tempvalue.UI16 != DP_ALL[dp_all_num].value.UI16)
			{
#if DEBUG == 1
OS_WriteStringReturn ("mod:Mod 16 UI16 changed");		//
#endif
				DP_ALL[dp_all_num].value.UI16 = (unsigned int)tempvalue.UI16;
				PutDP(dataType, dp_all_num);
				WriteFlashConfigS(dataType);
			}
			break;

		case DT_SI16:
			tempvalue.SI16 = (int)valueFloat;

			if (tempvalue.SI16 < DP_ALL[dp_all_num].min.SI16 ||
					tempvalue.SI16 > DP_ALL[dp_all_num].max.SI16)
			{
#if DEBUG == 1
OS_WriteStringReturn ("mod:ddd:");		//
#endif

				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_VALUE);  // Sendet einen Modbus Fehler
				RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
				return (MODBUS_SKIP);
			}

			if (tempvalue.SI16 != DP_ALL[dp_all_num].value.SI16)
			{
#if DEBUG == 1
OS_WriteStringReturn ("Mod 16 SI16 changed");		//
#endif
				DP_ALL[dp_all_num].value.SI16 = (int)tempvalue.SI16;
				PutDP(dataType, dp_all_num);
				WriteFlashConfigS(dataType);
			}
			break;

		case DT_UI32:
			tempvalue.UI32 = (unsigned long)valueFloat;

			if (tempvalue.UI32 < DP_ALL[dp_all_num].min.UI32 ||
					tempvalue.UI32 > DP_ALL[dp_all_num].max.UI32)
			{
#if DEBUG == 1
OS_WriteStringReturn ("mod:eeee");		//
#endif
				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_VALUE);  // Sendet einen Modbus Fehler
				RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
				return (MODBUS_SKIP);
			}

			if (tempvalue.UI32 != DP_ALL[dp_all_num].value.UI32)
			{
#if DEBUG == 1
OS_WriteStringReturn ("Mod 16 UI32 changed");		//
#endif
				DP_ALL[dp_all_num].value.UI32 = (unsigned long)tempvalue.UI32;
				PutDP(dataType, dp_all_num);
				WriteFlashConfigS(dataType);
			}
			break;

		case DT_SI32:
			tempvalue.SI32 = (long)valueFloat;

			if (tempvalue.SI32 < DP_ALL[dp_all_num].min.SI32 ||
					tempvalue.SI32 > DP_ALL[dp_all_num].max.SI32)
			{
#if DEBUG == 1
OS_WriteStringReturn ("mod:ffff");		//
#endif
				SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_VALUE);  // Sendet einen Modbus Fehler
				RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
				return (MODBUS_SKIP);
			}

			if (tempvalue.SI32 != DP_ALL[dp_all_num].value.SI32)
			{
#if DEBUG == 1
OS_WriteStringReturn ("Mod 16 SI32 changed");		//
#endif
				DP_ALL[dp_all_num].value.SI32 = (long)tempvalue.SI32;
				PutDP(dataType, dp_all_num);
				WriteFlashConfigS(dataType);
			}
			break;

		default:
#if DEBUG == 1
OS_WriteStringReturn ("mod:ggg");		//
#endif
			OS_WriteStringReturn("modbus 16:unknown DP");       // ÜÜÜÜÜÜÜÜ

			SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
			RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
			return (MODBUS_SKIP);
	} // switch
}
else                    // if (DP_ALL[dp_all_num].access & A_IO_WO)							// Schreibberechtigt
{
	SendModbusError(task_data, stulz_bus_com.slave, MODBUS_ILL_ADDRESS);    // Sendet einen Modbus Fehler
	RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
	return (MODBUS_SKIP);
}

RELEASEDPALL(dp_all_num);                       // DPall wieder freigeben
return (MODBUS_FUNCTION_16_WRITE_MULTIPLE_REGISTERS);

//-----------------------------------------------------------------------------------------------

default:
			// illegal function call => respond error-message
			return (MODBUS_ILL_FUNCTION);
	}   // Ende Switch Funktionsauswahl

	return (0);
}

//***************************************************************************************
//***************************************************************************************

unsigned char NextIOID(void)                // Sucht den nächsten Busteilnehmer nach mir raus
{
	unsigned char a;
	unsigned long l;

	a = adresse.value + 1;                                                                      // Einen nach mir
	if (a > MAX_IOC_ADR) a = MIN_IOC_ADR;

	l = (unsigned long) 0x01 << a;

while (a != adresse.value && adresse.value <= MAX_IOC_ADR)
{
	if (l & io_bus_com.dyn_conf) return (a);                                    // Direkt nach mir ist eine Unit -> Gefunden

	if (a != adresse.value) a++;
	if (a > MAX_IOC_ADR) a = MIN_IOC_ADR;
	l = (unsigned long) 0x01 << a;
}
return (0xFF);
}

//***************************************************************************************
//***************************************************************************************

unsigned char NextPingID(void)              // Sucht den nächsten Busteilnehmer für Ping Kommando
{
	unsigned char a, b;
	unsigned long l;

	a = adresse.value + 1;                                                                      // Einen nach mir
	if (a > 19) a = 0;

	l = (unsigned long) 0x01 << a;
if (l & io_bus_com.dyn_conf) return (0xFF);                             // Direkt nach mir ist eine Unit -> Keinen Ping

b = io_bus_com.next_ping_adr + 1;                                                   // Nächste Ping Adresse suchen, es muss eine geben
if (b > 19) b = 0;                                                                              // Adresse nicht grösser als 19

l = (unsigned long) 0x01 << b;
if (l & io_bus_com.dyn_conf ||                                                      // Steht Ping Pointer jetzt auf einer Unit oder
		b == adresse.value)                                                                     // Bin ich bei mir selbst
	return (a);                                                                                         // Adresse nach mir zurückgeben

return (b);																								// Alles OK einfach nur 1 hochzählen
}

//***************************************************************************************
//***************************************************************************************

unsigned char RS4851MsgTimeOut(unsigned int time)       // Wartet auf Empfang=1, Kein Empfang=0 (Volle Message)
{                                                                                                   // Zeit in ms
	if (RS485_1_receive_to_np != RS485_1_receive_to_vp) return (1);

	if (OS_WaitEventTimed(0x01, time)) return (1);                      // Event eingetroffen, löscht alle Events

	return (0);                     // Kein Event eingetroffen
}

//***************************************************************************************
//***************************************************************************************

unsigned char RS4851TimeOut(unsigned int time)      // Wartet auf Empfang=1, Kein Empfang=0
{                                                                                                   // Zeit in ms
	if (RS485_1_receive_vor_pointer != RS485_1_receive_nach_pointer) return (1);    // Zeichen schon drin

	if (OS_WaitSingleEventTimed(0x02, time)) return (1);                        // Event eingetroffen, löscht nur Byte Event

	return (0);                     // Kein Event eingetroffen
}

//***************************************************************************************
//***************************************************************************************

unsigned char UnitsBetween(unsigned char send, unsigned char receive)
{   // Ermittelt die Anzahl der Units zwischen Sender und Empfänger
	unsigned long l;
	unsigned char a, b;

	if (send == receive && send == adresse.value)                           // Ich sende an mich selber
		return (0);

	b = 0;
	a = send + 1;                                                                                           // Nächste mögliche Unit
	if (a > MAX_IOC_ADR) a = MIN_IOC_ADR;

	while (a != adresse.value && adresse.value <= MAX_IOC_ADR)// Sind wir schon am Ende angekommen
	{
		l = (unsigned long) 0x01 << a;
if (io_bus_com.dyn_conf & l) b++;                                               // Vorhandene Unit gefunden
a++;
if (a > MAX_IOC_ADR) a = MIN_IOC_ADR;
	}
	return (b);
}

//***************************************************************************************
//***************************************************************************************

unsigned char BusParticipants(void)                                             // Ermittelt die Anzahl der Busteilnehmer
{   // Ermittelt die Anzahl der Busteilnehmer
	unsigned long l;
	unsigned char a, b;

	b = 0;

	for (a = MIN_IOC_ADR; a <= MAX_IOC_ADR; a++)
	{
		l = (unsigned long) 0x01 << a;
if (io_bus_com.dyn_conf & l) b++;												// Vorhandene Unit gefunden
	}
	return (b);
}

//***************************************************************************************
//***************************************************************************************

void SpreadDPIOC(task_data_struct* task_data, unsigned int msg)
{
	unsigned char a, try;

	a = adresse.value + 1;
	if (a > MAX_IOC_ADR) a = MIN_IOC_ADR;

	while (a != adresse.value && adresse.value <= MAX_IOC_ADR)
	{
		if (unit[a].type.value == IOBUS_TYPE_C7000IOC)      // Ist es ein IOC
		{
			for (try = 1; try <= _RESPONSEFAIL_MAX; try++)
			{
	io_bus_com.adress = msg;
	io_bus_com.number = 1;

	RS4851SendBlockResponseMA(task_data, a);            // Block Response senden

	if (RS4851MsgTimeOut(_RESPONSETIMEOUT))             // Auf Empfangs timeout warten
	{
		if (CheckRS4851Input(task_data, RS485_1_receive_to_buf[RS485_1_receive_to_np]) ==
				IOBUS_DATA_BLOCKREQ_SL &&                       // "request" bekommen
				io_bus_com.sender == a) // Absender stimmt auch
		{
			RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
			if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

			break;
		}
		else                                                    // Keine Antwort
		{
			RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
			if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;
		}
	}
}		// for (try = 1 ; try <= _RESPONSEFAIL_MAX ; try++)
		}
		if (a != adresse.value) a++;
if (a > MAX_IOC_ADR) a = MIN_IOC_ADR;
	}
}

//***************************************************************************************
//***************************************************************************************

void SpreadDPBroadMA(task_data_struct* task_data, unsigned int dp_adr, unsigned char num)
{
	io_bus_com.adress = dp_adr;
	io_bus_com.number = num;

	// Block Response an mich selbst senden und auf ende warten
	RS4851SendBlockResponseMA(task_data, adresse.value);
}

//***************************************************************************************
//***************************************************************************************

void SpreadDPBroadSL(task_data_struct* task_data, unsigned int dp_adr)
{
	io_bus_com.adress = dp_adr;
	io_bus_com.number = 1;

	// Block Response an mich selbst senden und auf ende warten
	RS4851SendBlockResponseSL(task_data, adresse.value, IOBUS_DATA_BLOCKRES);
}

//***************************************************************************************
//***************************************************************************************
void SpreadDPBroadSLmulti(TaskData* td, uint16_t dpFirst, uint8_t cnt)
{
	io_bus_com.adress = dpFirst;
	io_bus_com.number = cnt;
	RS4851SendBlockResponseSL(td, adresse.value, IOBUS_DATA_BLOCKRES);
}

//***************************************************************************************
//***************************************************************************************

// An jede Message wird am Anfang ein Startbyte 0x02 STX (Start of text) und
// am Ende ein Endbyte 0x03 ETX (End of text) angefügt.
// Wenn innerhalb der Message STX, ETX oder ESC vorkommen wird diesen Zeichen ein ESC vorangestellt
// Dies gilt nicht für das Startbyte STX am Anfang und nicht für das Endbyte ETX am Ende


// Token

// Byte value meaning
// 00 0x41 Command code
// 01 Address of the proposed next master
// 02 Address of the sending master
// 03 Bus type of sending master (IOC, AT)
// 04 Dynamic busconfig 1
// 05 Dynamic busconfig 2
// 06 Dynamic busconfig 3
// 07 Static busconfig 1
// 08 Static busconfig 2
// 09 Static busconfig 3
// 10 lowbyte of checksum
// 11 highbyte of checksum

//------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------

// Data Block Request Message (Master -> Slave), IO-Bus

// 0   0x82  Command Code of Datatransmission (Master -> Slave)
// 1   3     ID des Empfängers
// 2   1     ID des Senders
// 3   11    Länge der Message
// 4   0x8A  Command Code of Data Block Request
// 5   0xB1  DP Anfangs Adresse LB
// 6   0x04  DP Anfangs Adresse HB
// 7   1     Anzahl der DP LB
// 8   0     Anzahl der DP HB
// 9   0x04  CS LB
// 10  0x59  CS HB


// Data Block Response Message (Slave -> Master), IO-Bus

// 0   0x92  Command Code of Datatransmission (Slave -> Master)
// 1   3     ID des Empfängers
// 2   1     ID des Senders
// 3   13    Länge der Message
// 4   0x8B  Command Code of Data Block Response
// 5   0xB1  DP Anfangs Adresse LB
// 6   0x04  DP Anfangs Adresse HB
// 7   1     Anzahl der DP LB
// 8   0     Anzahl der DP HB
// 9   0x01  Access Bitmap Feld
// 10  0x01  Wert des DP selber
// 11  0x04  CS LB
// 12  0x59  CS HB

//------------------------------------------------------------------------------------------

// Data Block Response Message (Master -> Slave), IO-Bus

// 0   0x82  Command Code of Datatransmission (Master -> Slave)
// 1   3     ID des Empfängers
// 2   1     ID des Senders
// 3   13    Länge der Message
// 4   0x8B  Command Code of Data Block Response
// 5   0xB1  DP Anfangs Adresse LB
// 6   0x04  DP Anfangs Adresse HB
// 7   1     Anzahl der DP LB
// 8   0     Anzahl der DP HB
// 9   0x01  Access Bitmap Feld
// 10  0x01  Wert des DP selber
// 11  0x04  CS LB
// 12  0x59  CS HB


// Data Block Request Message (Slave -> Master), IO-Bus

// 0   0x92  Command Code of Datatransmission (Slave -> Master)
// 1   3     ID des Empfängers
// 2   1     ID des Senders
// 3   11    Länge der Message
// 4   0x8A  Command Code of Data Block Request
// 5   0xB1  DP Anfangs Adresse LB
// 6   0x04  DP Anfangs Adresse HB
// 7   1     Anzahl der DP LB
// 8   0     Anzahl der DP HB
// 9   0x04  CS LB
// 10  0x59  CS HB

//-----------------------------------------------------------------------------------

// Data Adressed Request Message (Master -> Slave), IO-Bus

// 0   0x82  Command Code of Datatransmission (Master -> Slave)
// 1   3     ID des Empfängers
// 2   1     ID des Senders
// 3   17    Länge der Message
// 4   0x8C  Command Code of Data Adressed Request
// 5   0xB1  Address 1, LB
// 6   0x04  Address 1, HB
// 7   0xB1  Address 2, LB
// 8   0x06  Address 2, HB
// 9   0x35  Address 3, LB
// 10  0x10  Address 3, HB
// 11  0xE7  Address 4, LB
// 12  0x90  Address 4, HB
// 13  0x04  Address 5, LB
// 14  0x38  Address 5, HB
// 15  0x04  CS LB
// 16  0x59  CS HB


// Data Addressed Response Message (Slave -> Master), IO-Bus

// 0   0x92  Command Code of Datatransmission (Slave -> Master)
// 1   3     ID des Empfängers
// 2   1     ID des Senders
// 3   20    Länge der Message
// 4   0x8D  Command Code of Data Adressed Response
// 5   0xB1  Address 1, LB
// 6   0x04  Address 1, HB
// 7   1     Data of adress 1
// 8   0xB1  Address 2, LB
// 9   0x06  Address 2, HB
// 10  20    Data of adress 2
// 11  5     Data of adress 2
// 12  0x04  Address 5, LB
// 13  0x38  Address 5, HB
// 14  3     Data of adress 5
// 15  198   Data of adress 5
// 16  38    Data of adress 5
// 17  24    Data of adress 5
// 18  0x04  CS LB
// 19  0x59  CS HB

//-----------------------------------------------------------------------------------

// Data Adressed Response Message (Master -> Slave), IO-Bus

// 0   0x82  Command Code of Datatransmission (Master -> Slave)
// 1   3     ID des Empfängers
// 2   1     ID des Senders
// 3   20    Länge der Message
// 4   0x8D  Command Code of Data Adressed Response
// 5   0xB1  Address 1, LB
// 6   0x04  Address 1, HB
// 7   1     Data of adress 1
// 8   0xB1  Address 2, LB
// 9   0x06  Address 2, HB
// 10  20    Data of adress 2
// 11  5     Data of adress 2
// 12  0x04  Address 5, LB
// 13  0x38  Address 5, HB
// 14  3     Data of adress 5
// 15  198   Data of adress 5
// 16  38    Data of adress 5
// 17  24    Data of adress 5
// 18  0x04  CS LB
// 19  0x59  CS HB


// Data Adressed Request Message (Slave -> Master), IO-Bus

// 0   0x92  Command Code of Datatransmission (Slave -> Master)
// 1   3     ID des Empfängers
// 2   1     ID des Senders
// 3   11    Länge der Message
// 4   0x8C  Command Code of Data Adressed Request
// 5   0xB1  Address 1, LB													Nur die Adressen die
// 6   0x04  Address 1, HB													mit dem Response auch
// 7   0xB1  Address 2, LB													tatsächlich beschrieben
// 8   0x06  Address 2, HB													werden konnten
// 9   0x35  Address 3, LB
// 10  0x10  Address 3, HB
// 11  0xE7  Address 4, LB
// 12  0x90  Address 4, HB
// 13  0x04  Address 5, LB
// 14  0x38  Address 5, HB
// 15  0x04  CS LB
// 16  0x59  CS HB

//-----------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------

// Data Block Request Message (BMS Master -> Slave), E-Bus, GLT-Bus, mit STX/ETX/ESC

// 0   0x45  Command Code of Datatransmission (BMS Master -> Slave)
// 1   3     lowbyte of slave adress
// 2   1     highbyte of slave adress
// 3   13    lowbyte of length (bytes of whole message)
// 4   0     highbyte of length (bytes of whole message)
// 5   0x4A  Command Code of Data Block Request
// 6   2     Länge der folgenden Adresse
// 7   0xB1  DP Anfangs Adresse LB
// 8   0x04  DP Anfangs Adresse HB
// 9   1     Anzahl der DP LB
// 10  0     Anzahl der DP HB
// 11  0x04  CS LB
// 12  0x59  CS HB


// Data Block Response Message (Slave -> BMS Master), E-Bus, GLT-Bus, mit STX/ETX/ESC

// 0   0x65  Command Code of Datatransmission (Slave -> BMS Master)
// 1   3     lowbyte of slave adress
// 2   1     highbyte of slave adress
// 3   15    lowbyte of length (bytes of whole message)
// 4   0     highbyte of length (bytes of whole message)
// 5   0x4B  Command Code of Data Block Response
// 6   2     Länge der folgenden Adresse
// 7   0xB1  DP Anfangs Adresse LB
// 8   0x04  DP Anfangs Adresse HB
// 9   1     Anzahl der DP LB
// 10  0     Anzahl der DP HB
// 11  0x01  Access Bitmap Feld
// 12  0x01  Wert des DP selber
// 13  0x04  CS LB
// 14  0x59  CS HB

//-----------------------------------------------------------------------------------

// Data Block Response Message (BMS Master -> Slave), E-Bus, GLT-Bus, mit STX/ETX/ESC

// 0   0x45  Command Code of Datatransmission (BMS Master -> Slave)
// 1   3     ID des Empfängers
// 2   1     ID des Senders
// 3   15    lowbyte of length (bytes of whole message)
// 4   0     highbyte of length (bytes of whole message)
// 5   0x4B  Command Code of Data Block Response
// 6   2     Länge der folgenden Adresse
// 7   0xB1  DP Anfangs Adresse LB
// 8   0x04  DP Anfangs Adresse HB
// 9   1     Anzahl der DP LB
// 10  0     Anzahl der DP HB
// 11  0x01  Access Bitmap Feld
// 12  0x01  Wert des DP selber
// 13  0x04  CS LB
// 14  0x59  CS HB


// Data Block Request Message (Slave -> BMS Master), E-Bus, GLT-Bus, mit STX/ETX/ESC

// 0   0x65  Command Code of Datatransmission (Slave -> BMS Master)
// 1   3     ID des Empfängers
// 2   1     ID des Senders
// 3   13    lowbyte of length (bytes of whole message)
// 4   0     highbyte of length (bytes of whole message)
// 5   0x4A  Command Code of Data Block Request
// 6   2     Länge der folgenden Adresse
// 7   0xB1  DP Anfangs Adresse LB
// 8   0x04  DP Anfangs Adresse HB
// 9   1     Anzahl der DP LB
// 10  0     Anzahl der DP HB
// 11  0x04  CS LB
// 12  0x59  CS HB

//-----------------------------------------------------------------------------------

// Data Adressed Request Message (BMS Master -> Slave), E-Bus, GLT-Bus, mit STX/ETX/ESC

// 0   0x45  Command Code of Datatransmission (BMS Master -> Slave)
// 1   3     lowbyte of slave adress
// 2   1     highbyte of slave adress
// 3   19    lowbyte of length (bytes of whole message)
// 4   0     highbyte of length (bytes of whole message)
// 5   0x4C  Command Code of Data Adressed Request
// 6   2     Länge der folgenden Adresse
// 7   0xB1  Address 1, LB
// 8   0x04  Address 1, HB
// 9   0xB1  Address 2, LB
// 10  0x06  Address 2, HB
// 11  0x35  Address 3, LB
// 12  0x10  Address 3, HB
// 13  0xE7  Address 4, LB
// 14  0x90  Address 4, HB
// 15  0x04  Address 5, LB
// 16  0x38  Address 5, HB
// 17  0x04  CS LB
// 18  0x59  CS HB


// Data Adressed Response Message (Slave -> BMS Master), E-Bus, GLT-Bus, mit STX/ETX/ESC

// 0   0x65  Command Code of Datatransmission (Slave -> BMS Master)
// 1   3     lowbyte of slave adress
// 2   1     highbyte of slave adress
// 3   22    lowbyte of length (bytes of whole message)
// 4   0     highbyte of length (bytes of whole message)
// 5   0x4D  Command Code of Data Adressed Response
// 6   2     Länge der folgenden Adresse
// 7   0xB1  Address 1, LB
// 8   0x04  Address 1, HB
// 9   1     Data of adress 1
// 10  0xB1  Address 2, LB
// 11  0x06  Address 2, HB
// 12  20    Data of adress 2
// 13  5     Data of adress 2
// 14  0x04  Address 5, LB
// 15  0x38  Address 5, HB
// 16  3     Data of adress 5
// 17  198   Data of adress 5
// 18  38    Data of adress 5
// 19  24    Data of adress 5
// 20  0x04  CS LB
// 21  0x59  CS HB

//-----------------------------------------------------------------------------------

// Data Adressed Response Message (BMS Master -> Slave), E-Bus, GLT-Bus, mit STX/ETX/ESC

// 0   0x45  Command Code of Datatransmission (BMS Master -> Slave)
// 1   3     lowbyte of slave adress
// 2   1     highbyte of slave adress
// 3   22    lowbyte of length (bytes of whole message)
// 4   0     highbyte of length (bytes of whole message)
// 5   0x4D  Command Code of Data Adressed Response
// 6   2     Länge der folgenden Adresse
// 7   0xB1  Address 1, LB
// 8   0x04  Address 1, HB
// 9   1     Data of adress 1
// 10  0xB1  Address 2, LB
// 11  0x06  Address 2, HB
// 12  20    Data of adress 2
// 13  5     Data of adress 2
// 14  0x04  Address 5, LB
// 15  0x38  Address 5, HB
// 16  3     Data of adress 5
// 17  198   Data of adress 5
// 18  38    Data of adress 5
// 19  24    Data of adress 5
// 20  0x04  CS LB
// 21  0x59  CS HB


// Data Adressed Request Message (Slave -> BMS Master), E-Bus, GLT-Bus, mit STX/ETX/ESC

// 0   0x65  Command Code of Datatransmission (Slave -> BMS Master)
// 1   3     lowbyte of slave adress
// 2   1     highbyte of slave adress
// 3   19    lowbyte of length (bytes of whole message)
// 4   0     highbyte of length (bytes of whole message)
// 5   0x4C  Command Code of Data Adressed Request
// 6   2     Länge der folgenden Adresse
// 7   0xB1  Address 1, LB													Nur die Adressen die
// 8   0x04  Address 1, HB													mit dem Response auch
// 9   0xB1  Address 2, LB													tatsächlich beschrieben
// 10  0x06  Address 2, HB													werden konnten
// 11  0x35  Address 3, LB
// 12  0x10  Address 3, HB
// 13  0xE7  Address 4, LB
// 14  0x90  Address 4, HB
// 15  0x04  Address 5, LB
// 16  0x38  Address 5, HB
// 17  0x04  CS LB
// 18  0x59  CS HB


/ --------------------------------------------------------------------
#define IOBUS_TYPE_UNKNOWN	0x00	// type not configured or unknown
#define IOBUS_TYPE_C4000		0x01	// C4000
#define IOBUS_TYPE_C1001		0x02	// C1001
#define IOBUS_TYPE_C1002		0x03	// C1002
#define IOBUS_TYPE_C5000		0x04	// C5000
#define IOBUS_TYPE_C6000		0x05	// C6000
#define IOBUS_TYPE_C1010		0x06	// C1010
#define IOBUS_TYPE_C7000IOC	0x07	// C7000-IO-Controller
#define IOBUS_TYPE_C7000AT	0x08	// C7000 AdvancedTerminal
#define IOBUS_TYPE_C5MSC		0x0A	// C5000 special software
//------------------------------------------------------------------------
//------------------------------------------------------------------------
// IO-Bus Kommandos
#define IOBUS_MSG_TOKEN					0x81	// The token (Master -> Slave)
//------------------------------------------------------------------------
#define IOBUS_MSG_PING_REQ			0x86	// Ping Anfrage (Master -> Slave)
#define IOBUS_MSG_PING_RES			0x96	// Ping Antwort (Slave -> Master)
//------------------------------------------------------------------------
#define IOBUS_MSG_DATA					0x82	// The data-message (Master -> Slave) Frame
// Wird im Protokoll gesendet
#define IOBUS_MSG_DATARESP			0x92	// The data-message (Slave -> Master) Frame
// Wird im Protokoll gesendet
//------------------------------------------------------------------------
#define IOBUS_DATA_BLOCKREQ			0x8A	// Data Block Request (with frame 0x82 or 0x92)
// Wird im Protokoll gesendet
#define IOBUS_DATA_BLOCKREQ_MA	0xAA	// Nur für meine interne Unterscheidung
// Data Block Request with frame 0x82 (Master -> Slave)
#define IOBUS_DATA_BLOCKREQ_SL	0xBA	// Nur für meine interne Unterscheidung
// Data Block Request with frame 0x92 (Slave -> Master)

#define IOBUS_DATA_BLOCKRES			0x8B	// Data Block Response (with frame 0x82 or 0x92)
// Wird im Protokoll gesendet
#define IOBUS_DATA_BLOCKRES_MA	0xAB	// Nur für meine interne Unterscheidung
// Data Block Response with frame 0x82 (Master -> Slave)
#define IOBUS_DATA_BLOCKRES_SL	0xBB	// Nur für meine interne Unterscheidung
// Data Block Response with frame 0x92 (Slave -> Master)
//----------------------------------------------------------------------------------------
#define IOBUS_DATA_ADRREQ				0x8C	// Adressed Request (with frame 0x82 or 0x92)
// Wird im Protokoll gesendet
#define IOBUS_DATA_ADRREQ_MA		0xAC	// Nur für meine interne Unterscheidung
// Adressed Request with frame 0x82 (Master -> Slave)
#define IOBUS_DATA_ADRREQ_SL		0xBC	// Nur für meine interne Unterscheidung
// Adressed Request with frame 0x92 (Slave -> Master)

#define IOBUS_DATA_ADRRES				0x8D	// Adressed Response (with frame 0x82 or 0x92)
// Wird im Protokoll gesendet
#define IOBUS_DATA_ADRRES_RED		0x8D	// Adressed Response (with frame 0x82 or 0x92) redesign
// Wird im Protokoll gesendet
#define IOBUS_DATA_ADRRES_MA		0xAD	// Nur für meine interne Unterscheidung
// Adressed Response with frame 0x82 (Master -> Slave)
#define IOBUS_DATA_ADRRES_SL		0xBD	// Nur für meine interne Unterscheidung
// Adressed Response with frame 0x92 (Slave -> Master)
//----------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------
#define STULZBUS_MSG_DATA				0x45		// The data-message (Master -> Slave) Frame
// Wird im Protokoll gesendet
#define STULZBUS_MSG_DATARESP		0x65		// The data-message (Slave -> Master) Frame
// Wird im Protokoll gesendet
//----------------------------------------------------------------------------------------
#define STULZBUS_DATA_BLOCKREQ		0x4A	// Data Block Request (with frame 0x45 or 0x65)
// Wird im Protokoll gesendet
#define STULZBUS_DATA_BLOCKREQ_MA	0xAE	// Nur für meine interne Unterscheidung
// Data Block Request with frame 0x45 (Master -> Slave)
#define STULZBUS_DATA_BLOCKREQ_SL	0xBE	// Nur für meine interne Unterscheidung
// Data Block Request with frame 0x65 (Slave -> Master)

#define STULZBUS_DATA_BLOCKRES		0x4B	// Data Block Response (with frame 0x45 or 0x65)
// Wird im Protokoll gesendet
#define STULZBUS_DATA_BLOCKRES_MA	0xAF	// Nur für meine interne Unterscheidung
// Data Block Response with frame 0x45 (Master -> Slave)
#define STULZBUS_DATA_BLOCKRES_SL	0xBF	// Nur für meine interne Unterscheidung
// Data Block Response with frame 0x65 (Slave -> Master)
//----------------------------------------------------------------------------------------
#define STULZBUS_DATA_ADRREQ			0x4C	// Data Block Request (with frame 0x45 or 0x65)
// Wird im Protokoll gesendet
#define STULZBUS_DATA_ADRREQ_MA		0xA0	// Nur für meine interne Unterscheidung
// Data Block Request with frame 0x45 (Master -> Slave)
#define STULZBUS_DATA_ADRREQ_SL		0xB0	// Nur für meine interne Unterscheidung
// Data Block Request with frame 0x65 (Slave -> Master)

#define STULZBUS_DATA_ADRRES			0x4D	// Data Block Response (with frame 0x45 or 0x65)
// Wird im Protokoll gesendet
#define STULZBUS_DATA_ADRRES_MA		0xA1	// Nur für meine interne Unterscheidung
// Data Block Response with frame 0x45 (Master -> Slave)
#define STULZBUS_DATA_ADRRES_SL		0xB1	// Nur für meine interne Unterscheidung
// Data Block Response with frame 0x65 (Slave -> Master)
//----------------------------------------------------------------------------------------
// ---------------- IIC Bus --------------------------
#define M10			0x0001			// Bit 0 im ICCON
#define RSC			0x0002			// Bit 1 im ICCON
#define BUM			0x0010			// Bit 4 im ICCON
#define ACKDIS	0x0020			// Bit 5 im ICCON
#define AIRDIS	0x0040			// Bit 6 im ICCON
#define TRX			0x0080			// Bit 7 im ICCON

#define AL		0x0002			// Bit 1 im ICST
#define LRB		0x0008			// Bit 3 im ICST
#define IRQD	0x0020			// Bit 5 im ICST
#define IRQP	0x0040			// Bit 6 im ICST
//-----------------------------------------------------
#define SOH      0x01
#define STX      0x02
#define ETX      0x03
#define EOT      0x04
#define ENQ      0x05
#define ACK      0x06
#define XON      0x11
#define XOFF     0x13
#define NAK      0x15
#define SYN      0x16
#define ESC      0x1B

// @name  Gerätetypen 
// @{ 
typedef enum {
	UNIT_DX = 1,
	UNIT_CW = 2,
	UNIT_GE1 = 6,
	UNIT_GE2 = 7,
	UNIT_2FLUID = 8,
	UNIT_CW2 = 9,
	UNIT_CPP = 13,
	UNIT_ENS = 16,
	UNIT_UNKNOWN = 255
}
unit_t;
// @} 

// @name  Geräte-Betriebsarten 
// @{ 
typedef enum {
	OP_NOP = 0,
	OP_CHILL = 1,
	OP_HEAT = 2,
	OP_HUMIDIFY = 3,
	OP_DEHUMIDIFY = 4
}
opMode_t;
// @} 

// @name  DFC-Betriebsarten 
// @{ 
typedef enum {
	DFC_NOP = 0,
	DFC_FC = 1,
	DFC_EFC = 2,
	DFC_MIX = 10,
	DFC_DX_WAIT = 20,
	DFC_DX_RUN = 21,
	DFC_CW2 = 30
}
dfcMode_t;
// @} 

// @name  Kühlbetriebsarten 
// @{ 
typedef enum {
	CMODE_NONE = 0,
	CMODE_FC = 1,
	CMODE_EFC = 2,
	CMODE_MIX = 3,
	CMODE_DX = 4,
	CMODE_CW = 5
}
coolMode_t;
// Structur für Antwort nach Auswertung eines IO-Bus Kommandos
typedef struct
{
	unsigned long dyn_conf;                 // Dynamische Buskonfiguration
unsigned long stat_conf;                // Statische Buskonfiguration
unsigned char ping_count;               // Ping Zähler, nach jedem dritten Token ein Ping
unsigned char al_reset;                 // Ein Alarmreset wurde bei mir ausgelöst, wenn ich das nächste Mal den Token
										// bekomme, muss ich den Busfehler löschen, bevor ich den Token weitergebe in dem
										// ich die dynamische Buskonfiguration in die statische übernehme
unsigned char address_num;          // Anzahl der angefragten Adressen bei Adressanfrage
unsigned int adress2[30];           // Startadresse der Angefrahten DPs für Adressabfrage
unsigned char i_send_token;         // Ich habe einen Token an jemand anderes gesendet, muss jetzt so tun als
									// hätte ich den Token auf dem Bus empfangen um alle Timings einzuhalten
									//	unsigned char i_send_token_receiver;	// Empfänger an den ich gerade den Token gesendet habe

unsigned char sender;                       // Unit ID des Senders der Nachricht
unsigned char recipient;                // Unit ID des Empfängers der Nachricht
unsigned int len;                           // Länge der Empfangenen Nachricht
dpDescr999 dp;                              // Wert des DP in Empfangener Nachricht (DPall)
unsigned char broadcast;                // Broadcast Message (Sender = Empfänger)
unsigned char type;                         // Unit typ
											//	unsigned char at_exist;					// Anzahl der ATs
unsigned int adress;                        // Startadresse der Angefragten DPs
unsigned int number;                        // Anzahl der abgefragten DPs

unsigned int msg_buf[MAX_IO_MSG + 1];   // DP Nummern die auf IO-Bus gesendet werden sollen
unsigned int rec_buf[MAX_IO_MSG + 1];   // Welche Art Empfänger soll die Nachricht erhalten
unsigned int num_buf[MAX_IO_MSG + 1];   // Welche Art Empfänger soll die Nachricht erhalten

unsigned char olli_empf;                // Nur zu Testzwecken
unsigned int olli_addres;
unsigned char olli_flag;                // Nur zu Testzwecken
unsigned int olli_anz;                  // Nur zu Testzwecken

unsigned char buf_vp;                       // Vor Pointer für IO-Bus Buffer
unsigned char buf_np;                       // Nach Pointer für IO-Bus Buffer

unsigned char next_ping_adr;        // Adresse für nächste Ping anfrage (0..19)

unsigned char token_hold_timer; // Zeit in der ich den Bus benutzen darf
unsigned char token_loop_back;	// Wenn 1 dann sende ich Token an mich selber,
																	// mache Broadcast und sende den Token weiter

} io_bus_com_struct;
//-----------------------------------------------------------------------------------------
// Structur für Antwort nach Auswertung eines Stulz-Bus Kommandos
typedef struct
{
	unsigned int slave;                     // Slave Adresse
											//	unsigned char sender;						// Unit ID des Senders der Nachricht
											//	unsigned char recipient;				// Unit ID des Empfängers der Nachricht
unsigned int len;                           // Länge der Empfangenen Nachricht
dpDescr999 dp;                              // Wert des DP in Empfangener Nachricht (DPall)
											//	unsigned char broadcast;				// Broadcast Massage (Sender = Empfänger)
											//	unsigned char type;							// Unit typ
											//	unsigned char no_smaller_unit;	// Anzahl konfigurierter Teilnehmer mit kleinerer ID als eigene
unsigned int adress;                        // Startadresse der Angefrahten DPs für Blockabfrage
unsigned char address_num;          // Anzahl der angefragten Adressen bei Adressanfrage
unsigned int adress2[30];           // Startadresse der Angefrahten DPs für Adressabfrage

unsigned int number;                        // Anzahl der abgefragten DPs
unsigned char modbus_datcmd;        // Modbus Funktion 1, 2, 3, 4, 5, 8, 16
unsigned int modbus_datstart;   // Modbus Startadresse
unsigned int modbus_datnum;     // Modbus Anzahl der DP
unsigned int modbus_value;          // Modbus Wert zum Beschreiben
unsigned int modbus_diag_code;  // Modbus DIAG code

unsigned int first_vardp_bit;                                                       // Start der bit varDP
unsigned int last_vardp_bit;                                                            // Ende der bit varDP
unsigned int first_vardp_ana;                                                       // Start der analog varDP
unsigned int last_vardp_ana;                                                            // Ende der analog varDP
unsigned int last_vardp;																	// Zeigt auf ersten undefinierten varDP nach den definierten

//	unsigned int	msg_buf	[MAX_IO_MSG+1];	// DP Nummern die auf IO-Bus gesendet werden sollen
//	unsigned int	rec_buf	[MAX_IO_MSG+1];	// Welche Art Empfänger soll die Nachricht erhalten

//	unsigned char olli_empf;				// Nur zu Testzwecken
//	unsigned int	olli_addres;
//	unsigned char olli_flag;				// Nur zu Testzwecken

//	unsigned char buf_vp;						// Vor Pointer für IO-Bus Buffer
//	unsigned char buf_np;						// Nach Pointer für IO-Bus Buffer

//	unsigned char token_hold_timer;	// Zeit in der ich den Bus benutzen darf
//	unsigned char token_loop_back;	// Wenn 1 dann sende ich Token an mich selber,
																	// mache Broadcast und sende den Token weiter
//	unsigned char last_token_adr;		// Unit die zuletzt einen Token gesendet hat

//	unsigned char kp_key8_15;		// Bits 8-15 of key pressed
//	unsigned char kp_key0_7;		// Bits 0-7 of key pressed

} stulz_bus_com_struct;
switch (msg)
{
	//-------------------------------------------------------------------------------------
	case IOBUS_MSG_TOKEN:           //	the token

		if (!io_bus_com.i_send_token)                       // Wenn token nicht von mir selber kommt
		{
			RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
			if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;
		}

		io_bus_com.i_send_token = 0;                        // Ende des Eigentoken fakes

		//-------------------------------------------------------------------------------------
		unit[io_bus_com.sender].send_token = 1; // Unit hat token gesendet
		unit[io_bus_com.sender].type.value = io_bus_com.type;   // Bustyp eintragen
																//-------------------------------------------------------------------------------------
		if (io_bus_com.recipient == io_bus_com.sender)  // Token broadcast
		{
			break;                                                                              // überspringen
		}
		//-------------------------------------------------------------------------------------
		if (io_bus_com.recipient != adresse.value)          // Ich bin nicht gemeint
		{
			if (RS4851TimeOut(UnitsBetween(io_bus_com.sender, io_bus_com.recipient) * _TOKENTIMEOUT + _TOKENHOLDTIME))
			{
				break;                                                                              // Eine Unit hat geantwortet -> weiter
			}
			else                                                                                        // Tokenverlust
			{
				io_bus_com.recipient = adresse.value;                   // Ich nehme Token auf
				io_bus_com.token_loop_back = 1;                             // Token an mich selbst senden  überflüssig???
			}
		}

		if (io_bus_com.recipient == adresse.value)      // Bin ich angesprochen (kein else)
		{
			if (io_bus_com.token_loop_back)
			{
				io_bus_com.token_loop_back = 0;                         // Token Loop beenden

				RS4851SendToken(&RS485_1_task_data, adresse.value);     // Token an mich selbst senden
				OS_Delay(10);                                                                                       // Timeout erzeugen
			}

			io_bus_com.token_hold_timer = _TOKENHOLDTIME;
			//----------------------------------------------------------------------------------------

			while (io_bus_com.token_hold_timer)     // Ich hab noch Zeit auf dem Bus
			{
				if (io_bus_com.buf_np != io_bus_com.buf_vp) // Message zu senden
				{
					switch (io_bus_com.rec_buf[io_bus_com.buf_np])
					{
						case IO_MSG_REC_IOC:                                        // Message an alle IOC
							SpreadDPIOC(&RS485_1_task_data, io_bus_com.msg_buf[io_bus_com.buf_np]);
							OS_Delay(BROAD_DELAY);                              // Time out Zeit auf Bus schaffen
							break;
						case IO_MSG_REC_BROAD_MA:                                   // Message als Broadcast an alle
							SpreadDPBroadMA(&RS485_1_task_data, io_bus_com.msg_buf[io_bus_com.buf_np], 1);
							OS_Delay(BROAD_DELAY);                              // Time out Zeit auf Bus schaffen
							break;
						case IO_MSG_REC_BROAD_SL:                                   // Message als Broadcast an alle
							SpreadDPBroadSL(&RS485_1_task_data, io_bus_com.msg_buf[io_bus_com.buf_np]);
							OS_Delay(BROAD_DELAY);                              // Time out Zeit auf Bus schaffen
							break;
						default:
							if (io_bus_com.rec_buf[io_bus_com.buf_np] >= IO_MSG_REC_ADR &&
									io_bus_com.rec_buf[io_bus_com.buf_np] <= IO_MSG_REC_ADR_END)
							{           // geziehlte Nachricht
								io_bus_com.adress = io_bus_com.msg_buf[io_bus_com.buf_np];
								io_bus_com.number = io_bus_com.num_buf[io_bus_com.buf_np];

								RS4851SendBlockResponseMA(&RS485_1_task_data,           // Block Response senden
									io_bus_com.rec_buf[io_bus_com.buf_np] - IO_MSG_REC_ADR);
							}
							break;
					}

					io_bus_com.buf_np++;
					if (io_bus_com.buf_np >= MAX_IO_MSG) io_bus_com.buf_np = 0;
				}
				else break;
			}                                               // while (io_bus_com.token_hold_timer)		// Ich hab noch Zeit auf dem Bus
															//------------------------------------------------------------------------------------
			if (io_bus_com.olli_flag)
			{
				io_bus_com.adress = io_bus_com.olli_addres;
				io_bus_com.number = io_bus_com.olli_anz;

				if (io_bus_com.olli_flag == 1) OS_WriteString("RS485_1 80:send block request -> ");
				if (io_bus_com.olli_flag == 1) OS_WriteStringReturn(LongToString(&RS485_1_task_data, io_bus_com.olli_empf, 2));     // Wandelt ein long in string

				RS4851SendBlockRequestMA(&RS485_1_task_data, io_bus_com.olli_empf);

				if (RS4851MsgTimeOut(_RESPONSETIMEOUT))             // Auf Empfangs timeout warten
																	//										if (RS4851MsgTimeOut (100))				// Auf Empfangs timeout warten
				{
					if (CheckRS4851Input(&RS485_1_task_data, RS485_1_receive_to_buf[RS485_1_receive_to_np]) ==
						IOBUS_DATA_BLOCKRES_SL &&                       // "request" bekommen
						io_bus_com.sender == io_bus_com.olli_empf)  // Absender stimmt auch
					{
						if (io_bus_com.olli_flag == 1)
						{
							OS_WriteString("RS485_1:");
							a = RS485_1_receive_nach_pointer;
							while (a != RS485_1_receive_to_buf[RS485_1_receive_to_np])
							{
								OS_WriteString(LongToStringHex(&RS485_1_task_data, RS485_1_receive_buf[a], 2));     // Wandelt ein long in string
								WriteSpace();
								a++;
								if (a >= MAX_485_1_RECEIVE_BUFF) a = 0;
							}
							OS_WriteReturn();
						}

						RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
						if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

						//												if (io_bus_com.physUnit == PU_NV) io_bus_com.physUnit = PU_NONE;

						if (io_bus_com.olli_flag == 1)
						{
							OS_WriteString("Sender:");
							OS_WriteStringReturn(LongToString(&RS485_1_task_data, io_bus_com.sender, 2));       // Wandelt ein long in string
							OS_WriteString("Empfaenger:");
							OS_WriteStringReturn(LongToString(&RS485_1_task_data, io_bus_com.recipient, 2));    // Wandelt ein long in string
							OS_WriteString("Adresse:");
							OS_WriteStringReturn(LongToString(&RS485_1_task_data, io_bus_com.adress, 0));       // Wandelt ein long in string
							OS_WriteString("Anzahl:");
							OS_WriteStringReturn(LongToString(&RS485_1_task_data, io_bus_com.number, 0));       // Wandelt ein long in string
							OS_WriteString("Wert:");
							OS_WriteStringReturn(DPToString(&RS485_1_task_data, &io_bus_com.dp.dataType, 0));
						}
						else
						{
							OS_WriteString("(");
							OS_WriteString(DPToString(&RS485_1_task_data, &io_bus_com.dp.dataType, 0));
							WriteKlZuReturn();
						}
					}
					else
					{
						OS_WriteString("RS485_1:");
						a = RS485_1_receive_nach_pointer;
						while (a != RS485_1_receive_to_buf[RS485_1_receive_to_np])
						{
							OS_WriteString(LongToStringHex(&RS485_1_task_data, RS485_1_receive_buf[a], 2));     // Wandelt ein long in string
							WriteSpace();
							a++;
							if (a >= MAX_485_1_RECEIVE_BUFF) a = 0;
						}
						OS_WriteReturn();

						OS_WriteString("Sender:");
						OS_WriteStringReturn(LongToString(&RS485_1_task_data, io_bus_com.sender, 2));       // Wandelt ein long in string
						OS_WriteString("Empfaenger:");
						OS_WriteStringReturn(LongToString(&RS485_1_task_data, io_bus_com.recipient, 2));        // Wandelt ein long in string
						OS_WriteString("Adresse:");
						OS_WriteStringReturn(LongToString(&RS485_1_task_data, io_bus_com.adress, 0));       // Wandelt ein long in string
						OS_WriteString("Anzahl:");
						OS_WriteStringReturn(LongToString(&RS485_1_task_data, io_bus_com.number, 0));       // Wandelt ein long in string
						OS_WriteString("Wert:");
						OS_WriteString2Return(DPToString(&RS485_1_task_data, &io_bus_com.dp.dataType, 0));

						RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
						if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

						OS_WriteStringReturn("Fehler:Falsche Antwort");
					}
				}
				else
				{
					WriteTextLangConstReturn(FLASH2_TEXT_0629); // "Fehler:Keine Antwort"
				}
				io_bus_com.olli_flag = 0;
			}
			//------------------------------------------------------------------------------------
			IOBusActValue(&RS485_1_task_data);          // Veränderte Werte über IO-Bus als Broadcast verteilen
														//------------------------------------------------------------------------------------
			RS4851SendNextToken(&RS485_1_task_data);                            // Sendet Token oder Ping an den nächsten
																				//--------------------------------------------------------------------------------------
		}   // if (io_bus_com.recipient == adresse.value)
		break;
	//-------------------------------------------------------------------------------------
	case IOBUS_MSG_PING_REQ:            //	Ping request

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		if (io_bus_com.recipient == adresse.value)          // Ich bin nicht gemeint
			RS4851SendPingRes(&RS485_1_task_data, io_bus_com.sender);

		break;
	//-------------------------------------------------------------------------------------
	case IOBUS_MSG_PING_RES:            //	Ping Response -> gleich überspringen

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		break;
	//-------------------------------------------------------------------------------------
	case IOBUS_DATA_BLOCKREQ_MA:    //	request to get continuous block of data from Master

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		if (io_bus_com.recipient == adresse.value)      // Bin ich gemeint
		{
			RS4851SendBlockResponseSL(&RS485_1_task_data, io_bus_com.sender, IOBUS_DATA_BLOCKRES);
		}
		break;
	//-------------------------------------------------------------------------------------
	case STULZBUS_DATA_BLOCKREQ_MA: // request from Master, Stulz Bus, bei WIB anfragen über AT

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		if (io_bus_com.recipient == adresse.value)      // Bin ich gemeint
		{
			RS4851SendBlockResponseSL(&RS485_1_task_data, io_bus_com.sender, STULZBUS_DATA_BLOCKRES);
		}
		break;
	//-------------------------------------------------------------------------------------
	case IOBUS_DATA_BLOCKRES_MA:    //	Data Block response from Master

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		if (io_bus_com.recipient == adresse.value)      // Bin ich gemeint
		{
			RS4851SendBlockRequestSL(&RS485_1_task_data, io_bus_com.sender, IOBUS_DATA_BLOCKREQ);
		}
		break;
	//-------------------------------------------------------------------------------------
	case STULZBUS_DATA_BLOCKRES_MA: //	Data Block response from Master, Stulz Bus, bei WIB anfragen über AT

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		if (io_bus_com.recipient == adresse.value)      // Bin ich gemeint
		{
			RS4851SendBlockRequestSL(&RS485_1_task_data, io_bus_com.sender, STULZBUS_DATA_BLOCKREQ);
		}
		break;
	//-------------------------------------------------------------------------------------
	case IOBUS_DATA_BLOCKRES_SL:    //	response on block-request

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		break;
	//-------------------------------------------------------------------------------------
	case IOBUS_DATA_BLOCKREQ_SL:    //	block-request from slace on block-response from master

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		break;
	//-------------------------------------------------------------------------------------
	case IOBUS_DATA_ADRREQ_MA:                                      // Addressed request from Master, IO-Bus

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		if (io_bus_com.recipient == adresse.value)      // Bin ich gemeint
		{
			RS4851SendAdrResponseSL(&RS485_1_task_data, io_bus_com.sender, IOBUS_DATA_ADRRES_RED);
		}
		break;
	//-------------------------------------------------------------------------------------
	case STULZBUS_DATA_ADRREQ_MA:                                   // Addressed request from Master, Stulz Bus, bei WIB anfragen über AT

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		if (io_bus_com.recipient == adresse.value)      // Bin ich gemeint
		{
			RS4851SendAdrResponseSL(&RS485_1_task_data, io_bus_com.sender, STULZBUS_DATA_ADRRES);
		}
		break;
	//-------------------------------------------------------------------------------------
	case IOBUS_DATA_ADRRES_MA:                                      // Addressed response from Master

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		if (io_bus_com.recipient == adresse.value)      // Bin ich gemeint
		{
			RS4851SendAdrRequestSL(&RS485_1_task_data, io_bus_com.sender, IOBUS_DATA_ADRREQ);
		}
		break;
	//-------------------------------------------------------------------------------------
	case STULZBUS_DATA_ADRRES_MA:                                       // Addressed response from Master, Stulz Bus, bei WIB anfragen über AT

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		if (io_bus_com.recipient == adresse.value)      // Bin ich gemeint
		{
			RS4851SendAdrRequestSL(&RS485_1_task_data, io_bus_com.sender, STULZBUS_DATA_ADRREQ);
		}
		break;
	//-------------------------------------------------------------------------------------
	case IOBUS_DATA_ADRREQ_SL:

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		break;
	//-------------------------------------------------------------------------------------
	case IOBUS_DATA_ADRRES_SL:

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;

		break;
	//-------------------------------------------------------------------------------------
	default:                                    // unbekanntes Kommando

		if (mon_timer_b1)
		{
			OS_WriteString("\r\nB1 ?:");
			a = RS485_1_receive_nach_pointer;
			while (a != RS485_1_receive_to_buf[RS485_1_receive_to_np])
			{
				OS_WriteString(LongToStringHex(&RS485_1_task_data, RS485_1_receive_buf[a], 2));     // Wandelt ein long in string
				WriteSpace();
				a++;
				if (a >= MAX_485_1_RECEIVE_BUFF) a = 0;
			}
			OS_WriteReturn();
		}

		RS485_1_receive_nach_pointer = RS485_1_receive_to_buf[RS485_1_receive_to_np++];
		if (RS485_1_receive_to_np >= MAX_485_1_RECEIVE_TO_BUFF) RS485_1_receive_to_np = 0;
		break;
		//-------------------------------------------------------------------------------------
}
			}									// while (RS485_1_receive_to_np != RS485_1_receive_to_vp)
		}										// if (RS4851MsgTimeOut (3000 + adresse.value*100))	// Auf Empfang warten
//-------------------------------------------------------------------------------------
*/
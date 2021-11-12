using System;
using System.Collections.Generic;
using System.Text;

namespace HOYA
{
    class hoya
    {

        //-------------------------------------------------------------------------------------
        public string RequestSTR(string address, string modulenumber, string value)
        {
        
            string f2 = value;
            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];// 0x31;// (byte)address;                             // address
            dat[1] = mod[0];// 0x34;// (byte)modulenumber;                        // module number
            dat[2] = 0x53;                                  // cmd byte 1
            dat[3] = 0x54;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                      // ETX

            int sum = 0;
            foreach (int number in dat)
            {
                sum += number;
            }
//            Console.WriteLine(sum);
            string hilf = sum.ToString("X");
            string crc1 = hilf.Substring(1, 1);
            string crc2 = hilf.Substring(2, 1);
//            Console.WriteLine(hilf);

            byte[] data = new byte[11];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = array[0];
            data[8] = array1[0];
            data[9] = 0x0D;                                      // Carriage Return
            data[10] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
        public string RequestADR(string address, string modulenumber, string value)
        {
            string f1 = address;
            string f3 = value;
            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = 0x40; // @
            dat[1] = mod[0];// 0x34;// (byte)modulenumber;                        // module number
            dat[2] = 0x41;                                  // cmd byte 1
            dat[3] = 0x44;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                      // ETX

            int sum = 0;
            foreach (int number in dat)
            {
                sum += number;
            }
//            Console.WriteLine(sum);
            string hilf = sum.ToString("X");
            string crc1 = hilf.Substring(1, 1);
            string crc2 = hilf.Substring(2, 1);
//            Console.WriteLine(hilf);

            byte[] data = new byte[11];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = array[0];
            data[8] = array1[0];
            data[9] = 0x0D;                                      // Carriage Return
            data[10] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
        public string RequestDMS(string address, string modulenumber, string value)
        {
          
            byte[] dat = new byte[9];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[2] = 0x53;                                  // cmd byte 1
            dat[3] = 0x54;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            if(value == "0")
            {// ausschalten modul 1
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;
            }
            if(value == "1")
            { // einschalten modul 1
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;
            }
            dat[9] = 0x03;                                      // ETX

            int sum = 0;
            foreach (int number in dat)
            {
                sum += number;
            }
//            Console.WriteLine(sum);
            string hilf = sum.ToString("X");
            string crc1 = hilf.Substring(1, 1);
            string crc2 = hilf.Substring(2, 1);
//            Console.WriteLine(hilf);

            byte[] data = new byte[15];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = dat[7];
            data[8] = dat[8];
            data[9] = dat[9];
            data[10] = dat[10];

            data[11] = array[0];
            data[12] = array1[0];
            data[13] = 0x0D;                                      // Carriage Return
            data[14] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }

        //-------------------------------------------------------------------------------------
        public string RequestDRS(string address, string modulenumber, string value)
        {
            byte[] dat = new byte[9];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[2] = 0x44;                                  // cmd byte 1
            dat[3] = 0x52;                                  // cmd byte 2
            dat[4] = 0x53;                                  // cmd byte 3
            if(value == "0")
            {// ausschalten modul 1
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;
            }
            if(value == "1")
            { // einschalten modul 1
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;
            }
            dat[9] = 0x03;                                      // ETX

            int sum = 0;
            foreach (int number in dat)
            {
                sum += number;
            }
//            Console.WriteLine(sum);
            string hilf = sum.ToString("X");
            string crc1 = hilf.Substring(1, 1);
            string crc2 = hilf.Substring(2, 1);
//            Console.WriteLine(hilf);

            byte[] data = new byte[15];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = dat[7];
            data[8] = dat[8];
            data[9] = dat[9];
            data[10] = dat[10];

            data[11] = array[0];
            data[12] = array1[0];
            data[13] = 0x0D;                                      // Carriage Return
            data[14] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
       //-------------------------------------------------------------------------------------
        public string RequestBMS(string address, string modulenumber, string value)
        {
            byte[] dat = new byte[9];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);
            
            dat[0] = adr[0];                                // address
            dat[2] = 0x42;                                  // cmd byte 1
            dat[3] = 0x4D;                                  // cmd byte 2
            dat[4] = 0x53;                                  // cmd byte 3
            if(value == "0")
            {// ausschalten modul 1
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;
            }
            if(value == "1")
            { // einschalten modul 1
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;
            }
            dat[9] = 0x03;                                      // ETX

            int sum = 0;
            foreach (int number in dat)
            {
                sum += number;
            }
//            Console.WriteLine(sum);
            string hilf = sum.ToString("X");
            string crc1 = hilf.Substring(1, 1);
            string crc2 = hilf.Substring(2, 1);
//            Console.WriteLine(hilf);

            byte[] data = new byte[15];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = dat[7];
            data[8] = dat[8];
            data[9] = dat[9];
            data[10] = dat[10];

            data[11] = array[0];
            data[12] = array1[0];
            data[13] = 0x0D;                                      // Carriage Return
            data[14] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
        public string RequestADS(string address, string modulenumber, string value)
        {
            byte[] dat = new byte[9];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[2] = 0x41;                                  // cmd byte 1
            dat[3] = 0x44;                                  // cmd byte 2
            dat[4] = 0x53;                                  // cmd byte 3
            if (value == "0")
            {// ausschalten modul 1
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;
            }
            if (value == "1")
            { // einschalten modul 1
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;
            }
            dat[9] = 0x03;                                      // ETX

            int sum = 0;
            foreach (int number in dat)
            {
                sum += number;
            }
            //            Console.WriteLine(sum);
            string hilf = sum.ToString("X");
            string crc1 = hilf.Substring(1, 1);
            string crc2 = hilf.Substring(2, 1);
            //            Console.WriteLine(hilf);

            byte[] data = new byte[15];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = dat[7];
            data[8] = dat[8];
            data[9] = dat[9];
            data[10] = dat[10];

            data[11] = array[0];
            data[12] = array1[0];
            data[13] = 0x0D;                                      // Carriage Return
            data[14] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------
        public string RequestData(string address, string modulenumber, string command)
        {
            string f = command;
            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            if(command == "ADR")
            {
                dat[0] = 0x40; // @
            }
            else
            {
                dat[0] = adr[0];// 0x31;// (byte)address;                             // address
            }

            dat[1] = mod[0];// 0x34;// (byte)modulenumber;                        // module number
            if(command == "STR")
            {
                dat[2] = 0x53;                                  // cmd byte 1
                dat[3] = 0x54;                                  // cmd byte 2
                dat[4] = 0x52;                                  // cmd byte 3
            }
            if (command == "DMS")
            {
                dat[2] = 0x53;                                  // cmd byte 1
                dat[3] = 0x54;                                  // cmd byte 2
                dat[4] = 0x52;                                  // cmd byte 3
            }
            if (command == "ADR")
            {
                dat[2] = 0x41;                                  // cmd byte 1
                dat[3] = 0x44;                                  // cmd byte 2
                dat[4] = 0x52;                                  // cmd byte 3
            }
            dat[5] = 0x03;                                      // ETX

            int sum = 0;
            foreach (int number in dat)
            {
                sum += number;
            }
//            Console.WriteLine(sum);
            string hilf = sum.ToString("X");
            string crc1 = hilf.Substring(1, 1);
            string crc2 = hilf.Substring(2, 1);
//            Console.WriteLine(hilf);

            byte[] data = new byte[11];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = array[0];
            data[8] = array1[0];
            data[9] = 0x0D;                                      // Carriage Return
            data[10] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
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
  

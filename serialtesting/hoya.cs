using System;
using System.Collections.Generic;
using System.Text;

namespace HOYA
{
    class hoya
    {
        //-------------------------------------------------------------------------------------
        public string RequestUVE(string address, string modulenumber, string value)
        {

            string f2 = value;
            byte[] dat = new byte[18];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x55;                                  // cmd byte 1
            dat[3] = 0x56;                                  // cmd byte 2
            dat[4] = 0x45;                                  // cmd byte 3

            if (f2 == "111") // switch all 3 modules on
            {
                //'000100010001' 
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;
                dat[12] = 0x31;

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = 0x30;
                dat[16] = 0x31;
            }
            if (f2 == "101")  // switch mod 1 and mod 3 on
            {
                //'000010000001' 
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;
                dat[12] = 0x30;

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = 0x30;
                dat[16] = 0x31;
            }
            if (f2 == "011")  // switch mod 2 and mod 3 on
            {
                //'000100010000' 
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;
                dat[12] = 0x31;

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = 0x30;
                dat[16] = 0x30;
            }
            if (f2 == "010") // switch mod 2 on
            {
                //'000000010000' 
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;
                dat[12] = 0x31;

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = 0x30;
                dat[16] = 0x30;
            }
            if (f2 == "100") // switch mod 1 on
            {
                //'000000000001' 
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;
                dat[12] = 0x30;

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = 0x30;
                dat[16] = 0x31;
            }
            if (f2 == "001") // switch mod 3 on
            {
                //'000100000000' 
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;
                dat[12] = 0x30;

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = 0x30;
                dat[16] = 0x30;
            }
            if (f2 == "000") // all modules off	
            {
                //'000000000000' 
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;
                dat[12] = 0x30;

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = 0x30;
                dat[16] = 0x31;
            }
            dat[17] = 0x03;                                  // ETX

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

            byte[] data = new byte[23];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = dat[6];
            data[8] = dat[7];
            data[9] = dat[8];
            data[10] = dat[9];

            data[11] = dat[10];
            data[12] = dat[11];
            data[13] = dat[12];
            data[14] = dat[13];

            data[15] = dat[14];
            data[16] = dat[15];
            data[17] = dat[16];
            data[18] = dat[17];

            data[19] = array[0];
            data[20] = array1[0];
            data[21] = 0x0D;                                      // Carriage Return
            data[22] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        public string RequestLVS(string address, string modulenumber, string value)
        {

            string f2 = value;
            string temp1 = f2.Substring(1, 3);               // string to dword mod 1
            string temp2 = f2.Substring(4, 3);               // string to dword mod 2
            string temp3 = f2.Substring(7, 3);               // string to dword mod 3
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);
            byte[] val1 = StringToByteArray(temp1);
            byte[] val2 = StringToByteArray(temp2);
            byte[] val3 = StringToByteArray(temp3);
            string tes = "";


            byte[] dat = new byte[18];
            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x4C;                                  // cmd byte 1
            dat[3] = 0x56;                                  // cmd byte 2
            dat[4] = 0x53;                                  // cmd byte 3

            dat[5] = 0x30;
            dat[6] = 0x30;
            dat[7] = val1[0];  // 10-100 %
            dat[8] = val1[1];  //

            dat[9] = 0x30;
            dat[10] = 0x30;
            dat[11] = val2[0];  // 10-100 %
            dat[12] = val2[1];  //

            dat[13] = 0x30;
            dat[14] = 0x30;
            dat[15] = val3[0];  // 10-100 %
            dat[16] = val3[1];  //
            if (Convert.ToInt32(temp1) < 16 && Convert.ToInt32(temp2) > 16 && Convert.ToInt32(temp3) > 16)
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = val1[0];  //

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = val2[0];  // 10-100 %
                dat[12] = val2[1];  //

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = val3[0];  // 10-100 %
                dat[16] = val3[1];  //
            }
            if (Convert.ToInt32(temp1) > 16 && Convert.ToInt32(temp2) < 16 && Convert.ToInt32(temp3) > 16)
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = val1[0];
                dat[8] = val1[1];  //

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;  // 10-100 %
                dat[12] = val2[0];  //

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = val3[0];  // 10-100 %
                dat[16] = val3[1];  //
            }
            if (Convert.ToInt32(temp1) < 16 && Convert.ToInt32(temp2) < 16 && Convert.ToInt32(temp3) < 16)
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = val1[0];  //

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;  // 10-100 %
                dat[12] = val2[0];  //

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = 0x30;  // 10-100 %
                dat[16] = val3[0];  //
            }
            if (Convert.ToInt32(temp1) < 16 && Convert.ToInt32(temp2) < 16 && Convert.ToInt32(temp3) > 16)
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = val1[0];  //

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;  // 10-100 %
                dat[12] = val2[0];  //

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = val3[0];  // 10-100 %
                dat[16] = val3[1];  //
            }
            if (Convert.ToInt32(temp1) > 16 && Convert.ToInt32(temp2) > 16 && Convert.ToInt32(temp3) > 16)
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = val1[0];
                dat[8] = val1[1];  //

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = val2[0];  // 10-100 %
                dat[12] = val2[1];  //

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = val3[0];  // 10-100 %
                dat[16] = val3[1];  //
            }

            dat[17] = 0x03;                                  // ETX

            int sum = 0;
            foreach (int number in dat)
            {
                sum += number;
            }
            string hilf = sum.ToString("X");
            string crc1 = hilf.Substring(1, 1);
            string crc2 = hilf.Substring(2, 1);
            byte[] data = new byte[23];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = dat[6];
            data[8] = dat[7];
            data[9] = dat[8];
            data[10] = dat[9];

            data[11] = dat[10];
            data[12] = dat[11];
            data[13] = dat[12];
            data[14] = dat[13];

            data[15] = dat[14];
            data[16] = dat[15];
            data[17] = dat[16];
            data[18] = dat[17];

            data[19] = array[0];
            data[20] = array1[0];
            data[21] = 0x0D;                                      // Carriage Return
            data[22] = 0x0A;                                     // Line Feed

            tes = ByteArrayToString(data);

            return tes;
        }
        //-------------------------------------------------------------------------------------
        public string RequestDMS(string address, string modulenumber, string value)
        {

            string f2 = value;
            byte[] dat = new byte[10];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x44;                                  // cmd byte 1
            dat[3] = 0x4D;                                  // cmd byte 2
            dat[4] = 0x53;                                  // cmd byte 3

            if (f2 == "1")
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;  // digital dimming mode for rs485
            }

            if (f2 == "0")
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;  // analog dimming mode (default) for IO control
            }
            dat[9] = 0x03;                                  // ETX

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

            data[7] = dat[6];
            data[8] = dat[7];
            data[9] = dat[8];
            data[10] = dat[9];

            data[11] = array[0];
            data[12] = array1[0];
            data[13] = 0x0D;                                      // Carriage Return
            data[14] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        public string RequestBMR(string address, string modulenumber)
        {

            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x42;                                  // cmd byte 1
            dat[3] = 0x4D;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                  // ETX

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
        public string RequestDMR(string address, string modulenumber)
        {

            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x44;                                  // cmd byte 1
            dat[3] = 0x4D;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                  // ETX

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
        public string RequestLGR(string address, string modulenumber, string value)
        {

            string f2 = value;
            byte[] dat = new byte[11];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);
            //	sData := CONCAT(sData, '10105'); // example
            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x4C;                                  // cmd byte 1
            dat[3] = 0x47;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3

            dat[5] = Convert.ToByte(f2.Substring(1, 1));
            dat[6] = Convert.ToByte(f2.Substring(2, 1));
            dat[7] = Convert.ToByte(f2.Substring(3, 1));
            dat[8] = Convert.ToByte(f2.Substring(4, 1));
            dat[9] = Convert.ToByte(f2.Substring(5, 1));


            dat[10] = 0x03;                                  // ETX

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

            byte[] data = new byte[16];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = dat[6];
            data[8] = dat[7];
            data[9] = dat[8];
            data[10] = dat[9];
            data[11] = dat[10];

            data[12] = array[0];
            data[13] = array1[0];
            data[14] = 0x0D;                                      // Carriage Return
            data[15] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        public string RequestLSR(string address, string modulenumber)
        {

            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x4C;                                  // cmd byte 1
            dat[3] = 0x53;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                  // ETX

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
        public string RequestERR(string address, string modulenumber)
        {

            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x45;                                  // cmd byte 1
            dat[3] = 0x52;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                  // ETX

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
        public string RequestTSR(string address, string modulenumber)
        {
            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x55;                                  // cmd byte 1
            dat[3] = 0x54;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                  // ETX

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
        public string RequestTHR(string address, string modulenumber)
        {
            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x54;                                  // cmd byte 1
            dat[3] = 0x48;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                  // ETX

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
        public string RequestLVR(string address, string modulenumber)
        {
            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x4C;                                  // cmd byte 1
            dat[3] = 0x56;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                  // ETX

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
        public string RequestSTR(string address, string modulenumber)
        {
            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x53;                                  // cmd byte 1
            dat[3] = 0x54;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                  // ETX

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
        public string RequestSVR(string address, string modulenumber)
        {
            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];                                // module number
            dat[2] = 0x53;                                  // cmd byte 1
            dat[3] = 0x56;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                  // ETX

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
        public string RequestADR(string address, string modulenumber)
        {
            string f1 = address;
            byte[] dat = new byte[6];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = 0x40;                                  // @
            dat[1] = mod[0];                                // module number
            dat[2] = 0x41;                                  // cmd byte 1
            dat[3] = 0x44;                                  // cmd byte 2
            dat[4] = 0x52;                                  // cmd byte 3
            dat[5] = 0x03;                                  // ETX

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
        public string RequestDRS(string address, string modulenumber, string value)
        {
            string f2 = value;
            byte[] dat = new byte[10];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];
            dat[2] = 0x44;                                  // cmd byte 1
            dat[3] = 0x52;                                  // cmd byte 2
            dat[4] = 0x53;                                  // cmd byte 3

            if (f2 == "000") // disable all mods
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;
            }
            if (f2 == "100") // enable mod 1
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;
            }
            if (f2 == "010") // enable mod 2
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x31;
                dat[8] = 0x30;
            }
            if (f2 == "110") // enable mod 1 + 2
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x31;
                dat[8] = 0x31;
            }
            if (f2 == "001") // enable mod 3
            {
                dat[5] = 0x30;
                dat[6] = 0x31;
                dat[7] = 0x30;
                dat[8] = 0x30;
            }
            if (f2 == "011") // enable mod 2 + 3
            {
                dat[5] = 0x30;
                dat[6] = 0x31;
                dat[7] = 0x31;
                dat[8] = 0x30;
            }
            if (f2 == "101") // enable mod 1 + 3
            {
                dat[5] = 0x30;
                dat[6] = 0x31;
                dat[7] = 0x30;
                dat[8] = 0x31;
            }
            if (f2 == "111") // enable mod 1 + 2 + 3
            {
                dat[5] = 0x30;
                dat[6] = 0x31;
                dat[7] = 0x31;
                dat[8] = 0x31;
            }

            dat[9] = 0x03;                                  // ETX

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

            data[7] = dat[6];
            data[8] = dat[7];
            data[9] = dat[8];
            data[10] = dat[9];

            data[11] = array[0];
            data[12] = array1[0];
            data[13] = 0x0D;                                     // Carriage Return
            data[14] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        public string RequestECS(string address, string modulenumber, string value)
        {
            string f2 = value;
            byte[] dat = new byte[18];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];
            dat[2] = 0x45;                                  // cmd byte 1
            dat[3] = 0x43;                                  // cmd byte 2
            dat[4] = 0x53;                                  // cmd byte 3

            if (f2 == "1") // switch on
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;
                dat[12] = 0x30;

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = 0x30;
                dat[16] = 0x31;
            }
            if (f2 == "0") // switch off
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;

                dat[9] = 0x30;
                dat[10] = 0x30;
                dat[11] = 0x30;
                dat[12] = 0x30;

                dat[13] = 0x30;
                dat[14] = 0x30;
                dat[15] = 0x30;
                dat[16] = 0x30;
            }
            dat[17] = 0x03;                                  // ETX

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

            byte[] data = new byte[23];
            byte[] array = Encoding.ASCII.GetBytes(crc1);
            byte[] array1 = Encoding.ASCII.GetBytes(crc2);

            data[0] = 0x02;
            data[1] = dat[0];
            data[2] = dat[1];
            data[3] = dat[2];
            data[4] = dat[3];
            data[5] = dat[4];
            data[6] = dat[5];

            data[7] = dat[6];
            data[8] = dat[7];
            data[9] = dat[8];
            data[10] = dat[9];

            data[11] = dat[10];
            data[12] = dat[11];
            data[13] = dat[12];
            data[14] = dat[13];

            data[15] = dat[14];
            data[16] = dat[15];
            data[17] = dat[16];
            data[18] = dat[17];

            data[19] = array[0];
            data[20] = array1[0];
            data[21] = 0x0D;                                     // Carriage Return
            data[22] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        public string RequestBMS(string address, string modulenumber, string value)
        {
            string f2 = value;
            byte[] dat = new byte[10];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];
            dat[2] = 0x42;                                  // cmd byte 1
            dat[3] = 0x4D;                                  // cmd byte 2
            dat[4] = 0x53;                                  // cmd byte 3

            if (f2 == "1") // 115200 baud
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x31;
            }
            if (f2 == "0") // 38400 baud
            {
                dat[5] = 0x30;
                dat[6] = 0x30;
                dat[7] = 0x30;
                dat[8] = 0x30;
            }

            dat[9] = 0x03;                                  // ETX

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
            data[13] = 0x0D;                                     // Carriage Return
            data[14] = 0x0A;                                     // Line Feed


            string tes = ByteArrayToString(data);
            return tes;
        }
        //-------------------------------------------------------------------------------------
        public string RequestADS(string address, string modulenumber, string value)
        {
            byte[] dat = new byte[9];
            byte[] adr = Encoding.ASCII.GetBytes(address);
            byte[] mod = Encoding.ASCII.GetBytes(modulenumber);

            dat[0] = adr[0];                                // address
            dat[1] = mod[0];
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
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
  

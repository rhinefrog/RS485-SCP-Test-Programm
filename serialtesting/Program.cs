using System;
//using System.IO.Ports;
using GodSharp.SerialPort;
using GodSharp.SerialPort.Extensions;
using CommandLine;
using coptions;
using OptionAttribute = coptions.OptionAttribute;


[ApplicationInfo(Help = "Example: serialtesting.exe -c com8 -a 1 -f 1 -s 1174")]
public class Options
{
    //	[Flag('s', "silent", Help = "Produce no output.")]
    //	public bool Silent;

    //	[Option('n', "name", "NAME", Help = "Name of user.")]
    //	public string Name
    //	{
    //		get { return _name; }
    //		set
    //		{
    //			if (String.IsNullOrWhiteSpace(value))
    //				throw new InvalidOptionValueException("Name must not be blank");
    //			_name = value;
    //		}
    //	}
    //	private string _name;

    [Option('c', "comport", "COMPORT", Help = "Comport Windows \"COM1\" or Linux \"/dev/ttySx\" ")]
    public string Comport
    {
        get { return _comport; }
        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new InvalidOptionValueException("Comport must not be blank");
            _comport = value;
        }
    }
    private string _comport;

    [Option('f', "function", "FUNCTION", Help = "ReadAnalog = 1 | WriteAnalog = 2 | ReadDigital = 3 | WriteDigital = 4 ")]
    public string Mfunction
    {
        get { return _mfunction; }
        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new InvalidOptionValueException("Comport must not be blank");
            _mfunction = value;
        }
    }
    private string _mfunction;

    [Option('a', "address", "DEVICE_ID", Help = "Choose a device address ")]
    public int Address
    {
        get { return _address; }
        set
        {
            _address = value;
        }
    }
    private int _address;

    [Option('s', "saddress", "STARTADDRESS", Help = "Choose a start address ")]
    public string Saddress
    {
        get { return _saddress; }
        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new InvalidOptionValueException("Comport must not be blank");
            _saddress = value;
        }
    }
    private string _saddress;

    [Option('w', "write", "WRITE", Help = "Value write")]
    public string DPValue
    {
        get { return _dpvalue; }
        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new InvalidOptionValueException("Comport must not be blank");
            _dpvalue = value;
        }
    }
    private string _dpvalue;
}


namespace GodSharp.NetCore.ConsoleSample
{

    class Program
    {
        static int Main(string[] args)
        {
            try {
                SCP.ProtocolSCP scp = new SCP.ProtocolSCP();
//                SDC.ProtocolSDC sdc = new SDC.ProtocolSDC();

                uint slaveAddress = 1;
                Options opt = CliParser.Parse<Options>(args);
                slaveAddress = Convert.ToUInt16(opt.Address);

                GodSerialPort gsp = new GodSerialPort(opt.Comport, 9600, 0, 8, 1, 0);

            gsp.UseDataReceived(true, (sp, bytes) =>
            {
                if (bytes != null && bytes.Length > 0)
                {
                    //                    string buffer = string.Join(" ", bytes);
                    //                    Console.WriteLine("receive data:" + buffer);

                    string test = SCP.ProtocolSCP.ByteArrayToString(bytes);
                    //                    Console.WriteLine("receive test:" + test);
                    if (opt.Mfunction == "1")
                    {
                        byte[] werte = new byte[2]; 
                        scp.ReadDPAna(bytes, werte);
                        string hexString = SCP.ProtocolSCP.ByteArrayToString(werte);
                        //                    Console.WriteLine("Data: " + hexString);
                        int num = Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
                        Console.WriteLine(num.ToString()); // Wert 
                    }
                    if (opt.Mfunction == "3")
                    {
                        byte[] werte = new byte[1]; 
                        scp.ReadDPDig(bytes, werte);
                        string hexString = SCP.ProtocolSCP.ByteArrayToString(werte);
                        //                    Console.WriteLine("Data: " + hexString);
                        int num = Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
                        Console.WriteLine(num.ToString()); // Wert 
                    }
                }
            });

            bool flag = gsp.Open();

            if (!flag)
            {
                Exit();
            }

            //SerialValueTest();
            if (opt.Mfunction == "1")
            {
                    // scp send read request
                    uint adresse = Convert.ToUInt16(opt.Address);
                    uint dp = Convert.ToUInt16(opt.Saddress);
                    string dd = scp.ReadDP(adresse, dp);
                    gsp.WriteHexString(dd);

                    System.Threading.Thread.Sleep(150);
            }
            if (opt.Mfunction == "2")
            {
                    // scp send write request
                    uint adresse = Convert.ToUInt16(opt.Address);
                    uint dp = Convert.ToUInt16(opt.Saddress);
                    uint val = Convert.ToUInt16(opt.DPValue);
                    byte bitmap = 0x01;
                    string dd = scp.WriteDPTrans(adresse, dp, bitmap, val);
                    gsp.WriteHexString(dd);

                    System.Threading.Thread.Sleep(150);
            }
            if (opt.Mfunction == "3")
            {
                    // scp send read request
                    uint adresse = Convert.ToUInt16(opt.Address);
                    uint dp = Convert.ToUInt16(opt.Saddress);
                    string dd = scp.ReadDP(adresse, dp);
                    gsp.WriteHexString(dd);

                    System.Threading.Thread.Sleep(150);
            }
            if (opt.Mfunction == "4")
            {
                    // scp send write request
                    uint adresse = Convert.ToUInt16(opt.Address);
                    uint dp = Convert.ToUInt16(opt.Saddress);
                    uint val = Convert.ToUInt16(opt.DPValue);
                    byte bitmap = 0x01;
                    string dd = scp.WriteDPTrans(adresse, dp, bitmap, val);
                    gsp.WriteHexString(dd);

                    System.Threading.Thread.Sleep(150);
            }

            gsp.Close();

            static void Exit()
            {
                Console.WriteLine("press any key to quit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
            static void SerialValueTest()
            {
                System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort();
                Console.WriteLine("PortName:" + port.PortName);
                Console.WriteLine("BaudRate:" + port.BaudRate);
                Console.WriteLine("Parity:" + port.Parity);
                Console.WriteLine("DataBits:" + port.DataBits);
                Console.WriteLine("StopBits:" + port.StopBits);
                Console.WriteLine("Handshake:" + port.Handshake);
                Console.WriteLine("ReadBufferSize:" + port.ReadBufferSize);
                Console.WriteLine("WriteBufferSize:" + port.WriteBufferSize);
            }
                return 0;
            }
            catch (CliParserExit)
            {
                // --help
                return 0;

            }
            catch (Exception e)
            {
                // unknown options etc...
                Console.Error.WriteLine("Fatal Error: " + e.Message);
                return 1;
            }
            string ToHexString(float f)
                {
                    var bytes = BitConverter.GetBytes(f);
                    var i = BitConverter.ToInt32(bytes, 0);
                    return "0x" + i.ToString("X8");
                }

                float FromHexString(string s)
                {
                    var i = Convert.ToInt32(s, 16);
                    var bytes = BitConverter.GetBytes(i);
                    return BitConverter.ToSingle(bytes, 0);
                }
            }
    }
}
                    // IEEE754 it works
                    //               var hexString = ToHexString(-10.5F);
                    //               var f = FromHexString(hexString);
                    //               Console.WriteLine(hexString.ToString());
                    //               Console.WriteLine(f.ToString());
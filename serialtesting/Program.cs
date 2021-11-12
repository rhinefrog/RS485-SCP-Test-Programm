using System;
using GodSharp.SerialPort;
using GodSharp.SerialPort.Extensions;
using CommandLine;
using coptions;
using OptionAttribute = coptions.OptionAttribute;
using System.Text;

[ApplicationInfo(Help = "Example: programname.exe -d com8 -c DRS -a 1 -m 3 -v 1")]
public class Options
{

    [Option('d', "comport", "COMPORT", Help = "Comport Windows \"COM1\" or Linux \"/dev/ttySx\" ")]
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

    [Option('c', "command", "CMD", Help = "Command STR/DRS/DMS/BMS ... ")]
    public string cmd
    {
        get { return _cmd; }
        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new InvalidOptionValueException("Command must not be blank");
            _cmd = value;
        }
    }
    private string _cmd;

    [Option('a', "address", "ADR", Help = "Address ")]
    public string address
    {
        get { return _address; }
        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new InvalidOptionValueException("Command must not be blank");
            _address = value;
        }
    }
    private string _address;

    [Option('m', "moduleno", "ModuleNo", Help = "Module Number")]
    public string moduleno
    {
        get { return _moduleno; }
        set
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new InvalidOptionValueException("Command must not be blank");
            _moduleno = value;
        }
    }
    private string _moduleno;

    [Option('v', "value", "VALUE", Help = "Value")]
    public string value
    {
        get { return _value; }
        set
        {
            _value = value;
        }
    }
    private string _value;
}

namespace GodSharp.NetCore.ConsoleSample
{

    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                HOYA.hoya hoya = new HOYA.hoya();

                Options opt = CliParser.Parse<Options>(args);

                //                GodSerialPort gsp = new GodSerialPort(opt.Comport, 115200, 0, 8, 1, 0);
                GodSerialPort gsp = new GodSerialPort(opt.Comport, 38400, 0, 8, 1, 0);

                gsp.UseDataReceived(true, (sp, bytes) =>
                {
                    if (bytes != null && bytes.Length > 0)
                    {
                        string buffer = string.Join(" ", bytes);
                        Console.WriteLine("receive data:" + buffer);

                        string test = ByteArrayToString(bytes);
                        Console.WriteLine("receive test:" + test);
                    //---------------

                }
                });

                bool flag = gsp.Open();

                if (!flag)
                {
                    Exit();
                }

                if (opt.cmd == "STR")
                {
                    string dd = hoya.RequestSTR(opt.address, opt.moduleno, opt.value);
                    gsp.WriteHexString(dd);
                    System.Threading.Thread.Sleep(150);
                }
                if (opt.cmd == "DMS")
                {
                    string dd = hoya.RequestDMS(opt.address, opt.moduleno, opt.value);
                    gsp.WriteHexString(dd);
                    System.Threading.Thread.Sleep(150);
                }
                if (opt.cmd == "DRS")
                {
                    string dd = hoya.RequestDRS(opt.address, opt.moduleno, opt.value);
                    gsp.WriteHexString(dd);
                    System.Threading.Thread.Sleep(150);
                }
                if (opt.cmd == "ADS")
                {
                    string dd = hoya.RequestADS(opt.address, opt.moduleno, opt.value);
                    gsp.WriteHexString(dd);
                    System.Threading.Thread.Sleep(150);
                }
                if (opt.cmd == "BMS")
                {
                    string dd = hoya.RequestBMS(opt.address, opt.moduleno, opt.value);
                    gsp.WriteHexString(dd);
                    System.Threading.Thread.Sleep(150);
                }

                //------------------
                gsp.Close();

                static void Exit()
                {
                    Console.WriteLine("press any key to quit.");
                    Console.ReadKey();
                    Environment.Exit(0);
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

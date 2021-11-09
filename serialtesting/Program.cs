using System;
//using System.IO.Ports;
using GodSharp.SerialPort;
using GodSharp.SerialPort.Extensions;
using CommandLine;
using coptions;
using OptionAttribute = coptions.OptionAttribute;


[ApplicationInfo(Help = "Example: programname.exe -d com8 -c 1 -v 1")]
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

    [Option('c', "commands", "CMD", Help = "STR | DMS | ERR ... ")]
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
    public string Svalue
    {
        get { return _svalue; }
        set
        {
            _svalue = value;
        }
    }
    private string _svalue;
}

namespace GodSharp.NetCore.ConsoleSample
{

    class Program
    {
        static int Main(string[] args)
        {
            try {
                   HOYA.hoya hoya = new HOYA.hoya();

                    Options opt = CliParser.Parse<Options>(args);

//            string test = hoya.RequestData("1", "4", "STR");
                    GodSerialPort gsp = new GodSerialPort(opt.Comport, 38400, 0, 8, 1, 0);

                    gsp.UseDataReceived(true, (sp, bytes) =>
                        {
                            if (bytes != null && bytes.Length > 0)
                            {
                                string buffer = string.Join(" ", bytes);
                                Console.WriteLine("receive data:" + buffer);

            //                    string test = HOYA.hoya.ByteArrayToString(bytes);
            //                    Console.WriteLine("receive test:" + test);
            /*                    if (opt.bDebug)
                                {
                                    string hexString = SCP.hoya.ByteArrayToString(bytes);
                                    Console.WriteLine("Debug response: "+hexString);
                                }
                                if (opt.Mfunction == "1")
                                {
                                    byte[] werte = new byte[2]; 
                                    scp.ReadDPAna(bytes, werte);
                                    string hexString = SCP.hoya.ByteArrayToString(werte);
                                    //                    Console.WriteLine("Data: " + hexString);
                                    int num = Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
                                    Console.WriteLine(num.ToString()); // Wert 
                                }
                                if (opt.Mfunction == "3")
                                {
                                    byte[] werte = new byte[1]; 
                                    scp.ReadDPDig(bytes, werte);
                                    string hexString = SCP.hoya.ByteArrayToString(werte);
                                    //                    Console.WriteLine("Data: " + hexString);
                                    int num = Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
                                    Console.WriteLine(num.ToString()); // Wert 
                                }*/
                            }
                        });

                    bool flag = gsp.Open();

                   if (!flag)
                   {
                        //Exit();
                   }

            //SerialValueTest();
                   if (opt.cmd == "STR")
                        {
                                //  send read request
                                string dd = hoya.RequestData(opt.address, opt.moduleno, opt.cmd); 
                                gsp.WriteHexString(dd);

                                System.Threading.Thread.Sleep(150);
                        }
                   if (opt.cmd == "DMS")
                        {
                            //  send read request
                            string dd = hoya.RequestData(opt.address, opt.moduleno, opt.cmd);
                            gsp.WriteHexString(dd);

                            System.Threading.Thread.Sleep(150);
                        }

                gsp.Close();
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
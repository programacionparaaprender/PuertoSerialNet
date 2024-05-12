using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO.Ports;
using System.Threading;

namespace PuertoSerial
{
    

    public class Program
    {
        static bool _resume;

        // Create a new SerialPort object
        static SerialPort _serialPortObj = new SerialPort();

        public static void Main()
        {
            Console.Read();
            //SerialPort port = new SerialPort("COM7", 9600, Parity.Even, 8, StopBits.One);
            SerialPort port = new SerialPort();
            port.PortName = "COM7";
            port.Handshake = Handshake.None;
            port.BaudRate = 9600;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.DataBits = 8;
            port.Open();
            bool isopen = port.IsOpen; //me muestraque esta conectado

            //byte[] iniciarScr = { 0xf2, 0x00, 0x03, 0x43, 0x30, 0x40, 0x88, 0x8c };
            //port.Write(iniciarScr, 0, iniciarScr.Length);
            //byte[] bufferRespuesta = new byte[9600];
            //var bytesRead = port.Read(bufferRespuesta, 0, bufferRespuesta.Length);
            //Console.WriteLine(bytesRead.ToString());


            string DataIn = String.Empty;
            //Thread.Sleep(1000);//Puedes darle un retardo si lo consideras necesario.
            while (true)
            {
                DataIn = port.ReadExisting();
                Console.WriteLine(DataIn);
            }
            DataIn = port.ReadExisting();

            Console.WriteLine(DataIn);
            Console.Read();
        }

        public static void Main2()
        {
            string userName;
            string inputMessage;
            Thread read_Thread = new Thread(Read);
            StringComparer string_Comparer = StringComparer.OrdinalIgnoreCase;

            // Set the required attributes.
            _serialPortObj.PortName = SetPort(_serialPortObj.PortName);
            _serialPortObj.Parity = SetParity(_serialPortObj.Parity);

            _serialPortObj.StopBits = SetStopBits(_serialPortObj.StopBits);
            _serialPortObj.Handshake = SetHandshake(_serialPortObj.Handshake);

            // Set the read/write timeouts
            _serialPortObj.ReadTimeout = 1000;
            _serialPortObj.WriteTimeout = 1000;

            _serialPortObj.Open();
            _resume = true;
            read_Thread.Start();

            Console.Write("Enter Your Name: ");
            userName = Console.ReadLine();

            Console.WriteLine("Type Exit to exit");

            while (_resume)
            {
                inputMessage = Console.ReadLine();

                if (string_Comparer.Equals("exit", inputMessage))
                {
                    _resume = false;
                }
                else
                {
                    _serialPortObj.WriteLine(String.Format("<{0}>: {1}", userName, inputMessage));
                }
            }

            read_Thread.Join();

            // Close the serial port
            _serialPortObj.Close();
        }

        public static void Read()
        {
            while (_resume)
            {
                try
                {
                    // read the received message
                    string _recerivedMessage = _serialPortObj.ReadLine();
                    Console.WriteLine(_recerivedMessage);
                }

                catch (TimeoutException)
                {
                }
            }
        }

        // setters for attributes
        public static string SetPort(string defaultPortName)
        {
            string newPortName;

            Console.WriteLine("Available Ports:");
            foreach (string a in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", a);
            }

            Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            newPortName = Console.ReadLine();

            if (newPortName == "" || !(newPortName.ToLower()).StartsWith("com"))
            {
                newPortName = defaultPortName;
            }
            return newPortName;
        }

        public static Parity SetParity(Parity defaultParity)
        {
            string parityValue;

            Console.WriteLine("Available Parity Values:");
            foreach (string a in Enum.GetNames(typeof(Parity)))
            {
                Console.WriteLine("   {0}", a);
            }

            Console.Write("Enter Parity value (Default: {0}):", defaultParity.ToString(), true);
            parityValue = Console.ReadLine();

            if (parityValue == "")
            {
                parityValue = defaultParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), parityValue, true);
        }

        public static StopBits SetStopBits(StopBits defaultStopBits)
        {
            string stopBitValue;

            Console.WriteLine("Available StopBits Values:");
            foreach (string a in Enum.GetNames(typeof(StopBits)))
            {
                Console.WriteLine("   {0}", a);
            }

            Console.Write(
                "Enter a StopBits value (ArgumentOutOfRangeException occurs for None value. Select another. \n (Default Value: {0}):",
                defaultStopBits.ToString());
            stopBitValue = Console.ReadLine();

            if (stopBitValue == "")
            {
                stopBitValue = defaultStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBitValue, true);
        }

        public static Handshake SetHandshake(Handshake defaultHandshake)
        {
            string handshakeValue;

            Console.WriteLine("Available Handshake Values:");
            foreach (string a in Enum.GetNames(typeof(Handshake)))
            {
                Console.WriteLine("   {0}", a);
            }

            Console.Write("Enter Handshake value (Default: {0}):", defaultHandshake.ToString());
            handshakeValue = Console.ReadLine();

            if (handshakeValue == "")
            {
                handshakeValue = defaultHandshake.ToString();
            }

            return (Handshake)Enum.Parse(typeof(Handshake), handshakeValue, true);
        }
    }
}

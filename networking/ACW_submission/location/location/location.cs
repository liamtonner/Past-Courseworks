//Demonstrate Sockets
using System;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Linq;
using System.Windows.Forms;
using xlocation;
using ConsoleRedirection;

public class Location
{
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Application.EnableVisualStyles();
            Application.Run(new clientUI());
        }
        try
        {
            String server = "localhost";
            int port = 43;
            string protocol = "whois";
            string username = null;
            string xLocation = null;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-h":
                        server = args[i + 1];
                        string flag = "-h";
                        string xserver = server;
                        var argsList = args.ToList();
                        argsList.Remove(flag);
                        argsList.Remove(xserver);
                        args = argsList.ToArray();
                        i--;
                        break;
                    case "-p":
                        port = int.Parse(args[i + 1]);
                        string xport = port.ToString();
                        flag = "-p";
                        argsList = args.ToList();
                        argsList.Remove(xport);
                        argsList.Remove(flag);
                        args = argsList.ToArray();
                        i--;
                        break;
                    case "-h9":
                        protocol = args[i];
                        flag = "-h9";
                        argsList = args.ToList();
                        argsList.Remove(flag);
                        args = argsList.ToArray();
                        i--;
                        break;
                    case "-h0":
                        protocol = args[i];
                        flag = "-h0";
                        argsList = args.ToList();
                        argsList.Remove(flag);
                        args = argsList.ToArray();
                        i--;
                        break;
                    case "-h1":
                        protocol = args[i];
                        flag = "-h1";
                        argsList = args.ToList();
                        argsList.Remove(flag);
                        args = argsList.ToArray();
                        i--;
                        break;
                    default:
                        if (username == null)
                        {
                            username = args[i];
                        }
                        else if (xLocation == null)
                        {
                            xLocation = args[i];
                        }
                        else
                        {
                            Console.WriteLine("Too many arguments");
                        }
                        break;
                }
            }
            if (args.Length != 0)
            {
                username = args[0];
                if (args.Length >= 2)
                {
                    xLocation = args[1];
                }
            }
            if (username == null)
            {
                Console.WriteLine("Too few arguments");
                return;
            }
            TcpClient client = new TcpClient();
            client.Connect(server, port);
            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());

            switch (protocol)
            {
                case "whois":
                    if (xLocation == null)
                    {
                        sw.WriteLine(username);
                        sw.Flush();
                        Console.WriteLine(username + " is " + sr.ReadLine());
                    }
                    else
                    {
                        sw.WriteLine(username + " " + xLocation);
                        sw.Flush();
                        String response = sr.ReadLine();
                        if (response == "OK")
                        {
                            Console.WriteLine(username + " location changed to be " + xLocation);
                        }
                        else
                        {
                            Console.WriteLine("error server info not changed " + response);
                        }
                    }
                    break;
                case "-h9":
                    if (xLocation == null)
                    {
                        sw.WriteLine("GET /" + username + "\r\n");
                        sw.Flush();
                        string line1 = sr.ReadLine();
                        line1 = sr.ReadLine();
                        line1 = sr.ReadLine();
                        Console.WriteLine(username + " is " + sr.ReadLine());
                    }
                    else
                    {
                        sw.WriteLine("PUT /" + username + "\r\n" + "\r\n" + xLocation + "\r\n");
                        sw.Flush();
                        String response = sr.ReadLine();
                        if (response.EndsWith("OK"))
                        {
                            Console.WriteLine(username + " location changed to be " + xLocation);
                        }
                        else
                        {
                            Console.WriteLine("error server info not changed " + response);
                        }
                    }
                    break;
                case "-h0":
                    if (xLocation == null)
                    {
                        sw.WriteLine("GET /?" + username + " " + "HTTP/1.0" + "\r\n" + "\r\n");
                        sw.Flush();
                        string line1 = sr.ReadLine();
                        line1 = sr.ReadLine();
                        line1 = sr.ReadLine();
                        Console.WriteLine(username + " is " + sr.ReadLine());
                    }
                    else
                    {
                        int j = xLocation.Length;
                        sw.WriteLine("POST /" + username + " " + "HTTP/1.0" + "\r\n" + "Content-Length: " + j + "\r\n" + "\r\n" + xLocation + "\r\n");
                        sw.Flush();
                        String response = sr.ReadLine();
                        if (response.EndsWith("OK"))
                        {
                            Console.WriteLine(username + " location changed to be " + xLocation);
                        }
                        else
                        {
                            Console.WriteLine("error server info not changed " + response);
                        }
                    }
                    break;
                case "-h1":
                    if (xLocation == null)
                    {
                        sw.WriteLine("GET /?name=" + username + " " + "HTTP/1.1" + "\r\n" + "Host: " + server + "\r\n");
                        sw.Flush();
                        String response = sr.ReadLine();
                        if (response.EndsWith("OK"))
                        {
                            String pResponse = sr.ReadLine();
                            while (pResponse != "")
                            {
                                pResponse = sr.ReadLine();
                            }
                            pResponse = sr.ReadLine();
                            Console.WriteLine(username + " is " + pResponse);
                        }
                        else
                        {
                            Console.WriteLine("error server info not found " + response);
                        }

                    }
                    else
                    {
                        String length = ("name=" + username + "&location=" + xLocation);
                        int i = length.Length;
                        sw.WriteLine("POST /" + " " + "HTTP/1.1" + "\r\n" + "Host: " + server + "\r\n" + "Content-Length: " + i + "\r\n" + "\r\n" + "name= " + username + "&location= " + xLocation + "\r\n");
                        sw.Flush();
                        String response = sr.ReadLine();
                        if (response.EndsWith("OK"))
                        {
                            Console.WriteLine(username + " location changed to be " + xLocation);
                        }
                        else
                        {
                            Console.WriteLine("error server info not changed " + response);
                        }

                    }
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.ToString());
        }
    }


    public void Run(string[] args)
    {
        try
        {
            String server = "localhost";
            int port = 43;
            string protocol = "whois";
            string username = null;
            string xLocation = null;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-h":
                        server = args[i + 1];
                        string flag = "-h";
                        string xserver = server;
                        var argsList = args.ToList();
                        argsList.Remove(flag);
                        argsList.Remove(xserver);
                        args = argsList.ToArray();
                        i--;
                        break;
                    case "-p":
                        port = int.Parse(args[i + 1]);
                        string xport = port.ToString();
                        flag = "-p";
                        argsList = args.ToList();
                        argsList.Remove(xport);
                        argsList.Remove(flag);
                        args = argsList.ToArray();
                        i--;
                        break;
                    case "-h9":
                        protocol = args[i];
                        flag = "-h9";
                        argsList = args.ToList();
                        argsList.Remove(flag);
                        args = argsList.ToArray();
                        i--;
                        break;
                    case "-h0":
                        protocol = args[i];
                        flag = "-h0";
                        argsList = args.ToList();
                        argsList.Remove(flag);
                        args = argsList.ToArray();
                        i--;
                        break;
                    case "-h1":
                        protocol = args[i];
                        flag = "-h1";
                        argsList = args.ToList();
                        argsList.Remove(flag);
                        args = argsList.ToArray();
                        i--;
                        break;
                    default:
                        if (username == null)
                        {
                            username = args[i];
                        }
                        else if (xLocation == null)
                        {
                            xLocation = args[i];
                        }
                        else
                        {
                            Console.WriteLine("Too many arguments");
                        }
                        break;
                }
            }
            if (args.Length != 0)
            {
                username = args[0];
                if (args.Length >= 2)
                {
                    xLocation = args[1];
                }
            }
            if (username == null)
            {
                Console.WriteLine("Too few arguments");
                return;
            }
            TcpClient client = new TcpClient();
            client.Connect(server, port);
            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());

            switch (protocol)
            {
                case "whois":
                    if (xLocation == null)
                    {
                        sw.WriteLine(username);
                        sw.Flush();
                        Console.WriteLine(username + " is " + sr.ReadLine());
                    }
                    else
                    {
                        sw.WriteLine(username + " " + xLocation);
                        sw.Flush();
                        String response = sr.ReadLine();
                        if (response == "OK")
                        {
                            Console.WriteLine(username + " location changed to be " + xLocation);
                        }
                        else
                        {
                            Console.WriteLine("error server info not changed " + response);
                        }
                    }
                    break;
                case "-h9":
                    if (xLocation == null)
                    {
                        sw.WriteLine("GET /" + username + "\r\n");
                        sw.Flush();
                        string line1 = sr.ReadLine();
                        line1 = sr.ReadLine();
                        line1 = sr.ReadLine();
                        Console.WriteLine(username + " is " + sr.ReadLine());
                    }
                    else
                    {
                        sw.WriteLine("PUT /" + username + "\r\n" + "\r\n" + xLocation + "\r\n");
                        sw.Flush();
                        String response = sr.ReadLine();
                        if (response.EndsWith("OK"))
                        {
                            Console.WriteLine(username + " location changed to be " + xLocation);
                        }
                        else
                        {
                            Console.WriteLine("error server info not changed " + response);
                        }
                    }
                    break;
                case "-h0":
                    if (xLocation == null)
                    {
                        sw.WriteLine("GET /?" + username + " " + "HTTP/1.0" + "\r\n" + "\r\n");
                        sw.Flush();
                        string line1 = sr.ReadLine();
                        line1 = sr.ReadLine();
                        line1 = sr.ReadLine();
                        Console.WriteLine(username + " is " + sr.ReadLine());
                    }
                    else
                    {
                        int j = xLocation.Length;
                        sw.WriteLine("POST /" + username + " " + "HTTP/1.0" + "\r\n" + "Content-Length: " + j + "\r\n" + "\r\n" + xLocation + "\r\n");
                        sw.Flush();
                        String response = sr.ReadLine();
                        if (response.EndsWith("OK"))
                        {
                            Console.WriteLine(username + " location changed to be " + xLocation);
                        }
                        else
                        {
                            Console.WriteLine("error server info not changed " + response);
                        }
                    }
                    break;
                case "-h1":
                    if (xLocation == null)
                    {
                        sw.WriteLine("GET /?name=" + username + " " + "HTTP/1.1" + "\r\n" + "Host: " + server + "\r\n");
                        sw.Flush();
                        String response = sr.ReadLine();
                        if (response.EndsWith("OK"))
                        {
                            String pResponse = sr.ReadLine();
                            while (pResponse != "")
                            {
                                pResponse = sr.ReadLine();
                            }
                            pResponse = sr.ReadLine();
                            Console.WriteLine(username + " is " + pResponse);
                        }
                        else
                        {
                            Console.WriteLine("error server info not found " + response);
                        }

                    }
                    else
                    {
                        String length = ("name=" + username + "&location=" + xLocation);
                        int i = length.Length;
                        sw.WriteLine("POST /" + " " + "HTTP/1.1" + "\r\n" + "Host: " + server + "\r\n" + "Content-Length: " + i + "\r\n" + "\r\n" + "name= " + username + "&location= " + xLocation + "\r\n");
                        sw.Flush();
                        String response = sr.ReadLine();
                        if (response.EndsWith("OK"))
                        {
                            Console.WriteLine(username + " location changed to be " + xLocation);
                        }
                        else
                        {
                            Console.WriteLine("error server info not changed " + response);
                        }

                    }
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.ToString());
        }
    }
}
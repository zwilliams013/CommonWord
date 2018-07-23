import java.io.*;
import java.net.*;
import java.util.*;

/*
 * Client to generate a ping requests over UDP.
 * Code has started in the PingServer.java
 */

public class PingClient
{
	private static final int MAX_TIMEOUT = 1000;	// milliseconds

	public static void main(String[] args) throws Exception
	{
		// Get command line arguments.
		if (args.length != 2) {
			System.out.println("Required arguments: server, port");
			return;
		}

		// Create server and pass in argument 
		InetAddress server;
		server = InetAddress.getByName(args[0]);

		// Create port integer and pass in argument
		int port = Integer.parseInt(args[1]);
		
		// Create a datagram socket for sending and receiving UDP packets through the port specified on the command line.
		DatagramSocket socket = new DatagramSocket();
		
		// Loop 10 times to send 10 packets
		int packetCount =  0;
		while (packetCount < 10) {
			
			// Find timestamp from when packet was sent
			Date now = new Date();
			long sentPacketTime = now.getTime();
			
			// Create a string to send to the server 
			String line = "PING " + packetCount + " " + sentPacketTime + now + " \n";
			
			//Put string into byte array
			byte[] buf = new byte[1024];
			buf = line.getBytes();
			
			// Create a new datagram packet and send to a server
			DatagramPacket ping = new DatagramPacket(buf, buf.length, server, port);
			socket.send(ping);
			
			// Try and receive the packet
			try {
				// Set up the timeout 1000 ms = 1 sec
				socket.setSoTimeout(MAX_TIMEOUT);
				// Create a packet for 
				DatagramPacket test = new DatagramPacket(new byte[1024], 1024);
				// Try to receive the response from the ping
				socket.receive(test);
				// If the response is received, the code will continue here, otherwise it will continue in the catch
				
				// Get timestamp of packet when received
				now = new Date();
				long getPacketTime = now.getTime();
				
				// Print the packet and its round trip time
				printData(test, getPacketTime - sentPacketTime);
				
			} catch (IOException e) {
				// Print packets that time out
				System.out.println("Packet" + packetCount + " timed out");
			}
			
			// Increment packet to do next packet 
			packetCount ++;
		}
	}

    
    //Print ping data to the standard output stream.  Added delayTime parameter
    
   private static void printData(DatagramPacket request, long delayTime) throws Exception
   {
      // Obtain references to the packet's array of bytes.
      byte[] buf = request.getData();

      // Wrap the bytes in a byte array input stream,
      // so that you can read the data as a stream of bytes.
      ByteArrayInputStream bais = new ByteArrayInputStream(buf);

      // Wrap the byte array output stream in an input stream reader,
      // so you can read the data as a stream of characters.
      InputStreamReader isr = new InputStreamReader(bais);

      // Wrap the input stream reader in a buferred reader,
      // so you can read the character data a line at a time.
      // (A line is a sequence of chars terminated by any combination of \r and \n.) 
      BufferedReader br = new BufferedReader(isr);

      // The message data is contained in a single line, so read this line.
      String line = br.readLine();

      // Print host address and data received from it.
      System.out.println(
         "Received from " + 
         request.getAddress().getHostAddress() + ": " + new String(line) + " Delay: " + delayTime );
   }
}


import java.io.*;
import java.net.*;
import java.util.*;

public class myClient {
	
	public static void main(String[] args) throws Exception
	{
		DatagramSocket ds = new DatagramSocket();
		
		byte [] b = "this is udp client".getBytes();
		
		InetAddress ip = InetAddress.getByName("localhost");
		int port = 2000;
		
		DatagramPacket dp = new DatagramPacket(b, b.length,ip, 2000); 
		
		
		ds.send(dp);
	}
}

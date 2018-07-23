using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace CodingTest
{
    public class Passenger
    {
        string passengerName;
        public string airlineName;
        public string flightNumber;
        public string seatNumber;
        public string passportNumber;
        public string firstName;
        public string lastName;
        public string title;
        public bool validSeat;

        public Passenger(string passengerName, string airlineName, string flightNumber, string seatNumber, string passportNumber)
        {
            this.passengerName = passengerName;
            this.airlineName = airlineName.Replace('-', ' ');
            this.flightNumber = flightNumber;
            this.seatNumber = seatNumber;
            this.passportNumber = passportNumber;

            string[] passengerNameSplit = passengerName.Split(' ');
            if (passengerNameSplit.Length == 2)
            {
                firstName = passengerNameSplit[0];
                lastName = passengerNameSplit[1];
            }
            else
            {
                title = passengerNameSplit[0];
                firstName = passengerNameSplit[1];
                lastName = passengerNameSplit[2];
            }
            validSeat = isValidSeat(this.seatNumber);
        }

        //Flight seats range from 1-6, A-F
        public bool isValidSeat(string seat)
        {
            Regex pattern = new Regex(@"[1-6][A-F]");
            Match match = pattern.Match(seat);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class Flight
    {
        string airlineName;
        string flightNumber;
        string origin;
        string destination;
        bool international;
        List<Passenger> standby = new List<Passenger>();
        List<Passenger> noFly = new List<Passenger>();
        List<Passenger> passengers = new List<Passenger>();

        public Flight(string airlineFlight, string origin, string destination)
        {
            string[] airlineSplit = airlineFlight.Split(' ');
            airlineName = airlineSplit[0].Replace('-', ' ');
            flightNumber = airlineSplit[1];
            this.origin = origin;
            this.destination = destination;
            isInternational();
        }

        public void isInternational()
        {
            string originCountry = (origin.Trim().Split(','))[1];
            string destCountry = (destination.Trim().Split(','))[1];
            if (originCountry != "USA" && destCountry != "USA")
            {
                international = true;
            }
        }

        //Print flight information based on sorting choice
        public void printInfo(int choice)
        {
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("{0} flight {1}", airlineName, flightNumber);
            Console.WriteLine("departing from {0} with service to {1}", origin, destination);
            Console.WriteLine("Number of passengers: {0}", passengers.Count);
            Console.WriteLine("Number of passengers on standby: {0}", standby.Count);
            Console.WriteLine("-----------------------------------------------------");

            if (choice == 1)
            {
                passengers = passengers.OrderBy(o => o.seatNumber).ToList();
            }
            else if (choice == 2)
            {
                passengers = passengers.OrderBy(o => o.lastName).ToList();
            }

            foreach (var item in passengers)
            {
                if (choice == 1)
                {
                    if (item.title != null)
                    {
                        Console.WriteLine("{0} - {1} {2}, {3}", item.seatNumber, item.lastName, item.firstName, item.title);
                    }
                    else
                    {
                        Console.WriteLine("{0} - {1}, {2}", item.seatNumber, item.lastName, item.firstName);
                    }
                }
                else if (choice == 2)
                {
                    if (item.title != null)
                    {
                        Console.WriteLine("{0}, {1} {2} - {3}", item.lastName, item.firstName, item.title, item.seatNumber);
                    }
                    else
                    {
                        Console.WriteLine("{0}, {1} - {2}", item.lastName, item.firstName, item.seatNumber);
                    }
                }
            }

            if (standby.Count > 0)
            {
                Console.Write("Standby: ");
                foreach (var item in standby)
                {
                    Console.Write("{0} {1}", item.firstName, item.lastName);
                    if (item != standby.Last())
                    {
                        Console.Write(", ");
                    }
                }
                Console.Write('\n');
            }
            if (international && noFly.Count > 0)
            {
                Console.Write("NO FLY: ");
                foreach (var item in noFly)
                {
                    Console.Write("{0} {1}", item.firstName, item.lastName);
                    if (item != noFly.Last())
                    {
                        Console.Write(", ");
                    }
                }
                Console.Write('\n');
            }
        }

        //Assign passengers to flights 
        public void getPassengers(List<Passenger> passengerFile)
        {

            foreach (var item in passengerFile)
            {
                if ((item.airlineName == airlineName) && (item.flightNumber == flightNumber))
                {
                    if (item.passportNumber == "" && international == true)
                    {
                        noFly.Add(item);
                    }
                    else if (item.seatNumber == "" || !item.isValidSeat(item.seatNumber))
                    {
                        standby.Add(item);
                    }
                    else
                    {
                        passengers.Add(item);
                    }
                }
            }
        }

        //Populate flight if not at capacity
        public void capacityCheck()
        {
            do
            {
                if (standby.Count > 0)
                {
                    Passenger passenger = standby[0];
                    do
                    {
                        passenger.seatNumber = randomSeat();

                    }
                    while (passengers.Exists(o => o.seatNumber == passenger.seatNumber));

                    passengers.Add(standby[0]);
                    standby.RemoveAt(0);
                }
            }
            while (passengers.Count < 36 && standby.Count != 0);
        }

        //Choose a random seat for a passenger if flight isn't at capacity 
        public string randomSeat()
        {
            string number = "123456";
            string letter = "ABCDEF";

            Random r = new Random();

            string randSeatNumber = "";
            randSeatNumber += number[r.Next(0, 6)];
            randSeatNumber += letter[r.Next(0, 6)];

            return randSeatNumber;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int sortChoice = 0;
            do
            {
                Console.WriteLine("Choose sorting scheme. \n1.Seat Number \n2.Last Name");
                try
                {
                    sortChoice = int.Parse(Console.ReadLine());
                    if (sortChoice != 1 && sortChoice != 2)
                    {
                        Console.WriteLine("ERROR: Enter 1 or 2 for sort choice");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            while (sortChoice != 1 && sortChoice != 2);

            List<Passenger> PassengerList = new List<Passenger>();
            List<Flight> FlightList = new List<Flight>();
            readFlight(FlightList);
            readPassenger(PassengerList);

            foreach (var item in FlightList)
            {
                item.getPassengers(PassengerList);
                item.capacityCheck();
            }

            foreach (var item in FlightList)
            {
                item.printInfo(sortChoice);
            }

            Console.Write("\nPress any key to exit");
            Console.ReadKey();
        }

        static List<Passenger> readPassenger(List<Passenger> PassengerList)
        {
            string line;
            List<string> values = new List<string>();
            try
            {
                StreamReader file = new StreamReader("passengers.txt");

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Equals("/*-*/"))
                    {
                        PassengerList.Add(new Passenger(values.ElementAt(0), values.ElementAt(1), values.ElementAt(2), values.ElementAt(3), values.ElementAt(4)));//convert element 2 to an int
                        values.Clear();
                        continue;
                    }
                    else
                    {
                        values.Add(line);
                    }
                }
                return PassengerList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        static List<Flight> readFlight(List<Flight> FlightList)
        {
            string line;
            List<string> values = new List<string>();
            try
            {
                StreamReader file = new StreamReader("flights.txt");

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Equals("/*-*/"))
                    {
                        FlightList.Add(new Flight(values.ElementAt(0), values.ElementAt(1), values.ElementAt(2)));//convert element 2 to an int
                        values.Clear();
                        continue;
                    }
                    else
                    {
                        values.Add(line);
                    }
                }
                return FlightList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
    }
}

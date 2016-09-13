﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Net.Http;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Net;

namespace DAL
{
    public class DataManager
    {
        //Create Tables
        public readonly string APIKey = "r51uf0pH7QBSEgzrvrGcsQCZipRnetWn";

        public void CreateTables()
        {
            ConnectionManager cManager = ConnectionManager.getInstance();

            String existCheck = "SELECT count(*) FROM sqlite_master WHERE type='table' AND (name='QueryCollection' OR name='QueryResults')";

            SQLiteCommand command = new SQLiteCommand(existCheck, cManager.dbConnection);
            command.CommandType = System.Data.CommandType.Text;
            int tableCount = Convert.ToInt32(command.ExecuteScalar());

            if(tableCount != 2)
            {
                String queryCollectionCmd = "CREATE TABLE QueryCollection (id INTEGER PRIMARY KEY AUTOINCREMENT, DepartingAirport VARCHAR(3), DestinationAirport VARCHAR(3), DepartureDate TEXT, ReturnDate TEXT, Passengers INTEGER, Currency VARCHAR(3))";
                SQLiteCommand createQueryCollection = new SQLiteCommand(queryCollectionCmd, cManager.dbConnection);
                createQueryCollection.ExecuteNonQuery();

                String queryResultsCmd = "CREATE TABLE QueryResults (id INTEGER PRIMARY KEY AUTOINCREMENT, GeneratedByQuery INTEGER, DepartingAirport VARCHAR(3),  DestinationAirport VARCHAR(3), DepartureDate TEXT, ReturnDate TEXT, InBoundFlights INTEGER, OutboundFlights INTEGER, Passengers INTEGER, Currency VARCHAR(3), TotalPrice REAL , FOREIGN KEY (GeneratedByQuery) REFERENCES QueryCollection(id))";
                SQLiteCommand createQueryResults = new SQLiteCommand(queryResultsCmd, cManager.dbConnection);
                createQueryResults.ExecuteNonQuery();
            }

        }

        public SQLiteDataReader ProcessQuery(String originAirport, String destAirport,
                                   DateTime departureDate, DateTime returnDate,
                                   String noOfPassengers, String currency)
        {
            ConnectionManager cManager = ConnectionManager.getInstance();

            StringBuilder fetchCount = new StringBuilder();
            StringBuilder fetchID = new StringBuilder();


            fetchCount.Append("SELECT count(*) FROM QueryCollection WHERE ");
            fetchID.Append("SELECT id FROM QueryCollection WHERE ");

            StringBuilder sb = new StringBuilder();

            //Add OriginAirport
            sb.Append("DepartingAirport = '" + originAirport + "' AND ");

            //Add DestinationAirport
            sb.Append("DestinationAirport = '" + destAirport + "' AND ");

            //Add DepartureDate
            sb.Append("DepartureDate = '" + departureDate.Year + "-" + departureDate.Month.ToString("00") + "-" + departureDate.Day.ToString("00") + "' AND ");

            //Add ReturnDate
            sb.Append("ReturnDate = '" + returnDate.Year + "-" + returnDate.Month.ToString("00") + "-" + returnDate.Day.ToString("00") + "' ");

            //Add number of passengers if inputted
            if (!(noOfPassengers == null || noOfPassengers == ""))
            {
                sb.Append("AND Passengers = '" + noOfPassengers + "' ");
            }

            //Add Currency
            sb.Append("AND Currency = '" + currency + "'");

            fetchID.Append(sb.ToString());
            fetchCount.Append(sb.ToString());

            SQLiteCommand command = new SQLiteCommand(fetchCount.ToString(), cManager.dbConnection);
            command.CommandType = System.Data.CommandType.Text;
            int rowCount = Convert.ToInt32(command.ExecuteScalar());
            if(rowCount > 0)
            {
                //The query has been processed already, fetch old data
                SQLiteCommand cmd = new SQLiteCommand(fetchID.ToString(), cManager.dbConnection);
                command.CommandType = System.Data.CommandType.Text;

                int queryID = Convert.ToInt32(cmd.ExecuteScalar());

                StringBuilder returnQuery = new StringBuilder();
                returnQuery.Append("SELECT * FROM QueryResults WHERE GeneratedByQuery = " + queryID);
                SQLiteCommand cmd2 = new SQLiteCommand(returnQuery.ToString(), cManager.dbConnection);

                return cmd2.ExecuteReader();
            }
            else
            {
                //New query, fetch new data

                using(SQLiteTransaction tr = cManager.dbConnection.BeginTransaction())
                {
                    //First insert new query into QueryCollection
                    //Then fetch data from API

                    StringBuilder newQuery = new StringBuilder();
                    newQuery.Append("INSERT INTO QueryCollection (DepartingAirport, DestinationAirport, DepartureDate, ReturnDate, Passengers, Currency) VALUES('" + originAirport + "', '" + destAirport + "', '" + departureDate.Year + "-" + departureDate.Month.ToString("00") + "-" + departureDate.Day.ToString("00") + "', '" + returnDate.Year + "-" + returnDate.Month.ToString("00") + "-" + returnDate.Day.ToString("00") + "'");

                    if (!(noOfPassengers == null || noOfPassengers == ""))
                    {
                        newQuery.Append(", '" + noOfPassengers + "'");
                    }
                    else
                    {
                        newQuery.Append(", null");
                    }


                    newQuery.Append(", '" + currency + "');");

                    using (SQLiteCommand cmd = cManager.dbConnection.CreateCommand())
                    {

                        cmd.Transaction = tr;
                        cmd.CommandText = newQuery.ToString();
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = fetchID.ToString();
                        int queryID = Convert.ToInt32(cmd.ExecuteScalar());

                        StringBuilder APIquery = new StringBuilder();
                        APIquery.Append("http://api.sandbox.amadeus.com/v1.2/flights/low-fare-search?origin=" +
                                        originAirport + "&destination=" + destAirport +
                                        "&departure_date=" + departureDate.Year + "-" + departureDate.Month.ToString("00") + "-" + departureDate.Day.ToString("00") +
                                        "&return_date=" + returnDate.Year + "-" + returnDate.Month.ToString("00") + "-" + returnDate.Day.ToString("00"));
                        if (!(noOfPassengers == null || noOfPassengers == ""))
                        {
                            APIquery.Append("&adults=" + noOfPassengers + "");
                        }

                        APIquery.Append("&currency=" + currency);
                        APIquery.Append("&apikey=" + APIKey);

                        //using (var client = new HttpClient())
                        //{
                        //    var responseString =  client.GetStringAsync(APIquery.ToString());
                        //    //var responseString = client.GetStringAsync("http://api.sandbox.amadeus.com/v1.2/flights/low-fare-search?apikey=r51uf0pH7QBSEgzrvrGcsQCZipRnetWn&origin=BOS&destination=LON&departure_date=2016-11-25");
                        //    responseString.Wait();
                        //    JObject json = JObject.Parse(responseString);


                        //}

                    }
                    tr.Commit();
                }

                return null; 
            }

        }

        public void FetchDataWithNoPreviousQuery(String originAirport, String destAirport,
                                   DateTime departureDate, DateTime returnDate,
                                   String noOfPassengers, String currency)
        {

        }

    }
}
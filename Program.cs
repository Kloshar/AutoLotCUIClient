using AutoLotConnectedLayer;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Common;
class Programm
{
    static void Main()
    {
        string cnStr = ConfigurationManager.ConnectionStrings["AutoLotSqlProvider"].ConnectionString;
        bool userDone = false;
        string userCommand = "";
        InventoryDAL invDaL = new InventoryDAL();
        invDaL.OpenConnection(cnStr);
        try
        {
            Showinstructions();
            do
            {
                Console.WriteLine("\nPlease enter your command:");
                userCommand = Console.ReadLine()!;
                Console.WriteLine();
                switch (userCommand!.ToUpper())
                {
                    case "I":
                        InsertNewCar(invDaL);
                        break;
                    case "U":
                        UpdateCarPetName(invDaL);
                        break;
                    case "D":
                        DeleteCar(invDaL);
                        break;
                    case "L":
                        ListInventory(invDaL);
                        break;
                    case "S":
                        Showinstructions();
                        break;
                    case "P":
                        LookUpPetName(invDaL);
                        break;
                    case "Q":
                        userDone = true;
                        break;
                    default:
                        Console.WriteLine("Bad data! Try again!");
                        break;
                }
            } while (!userDone);
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { invDaL.CloseConnection(); }
        Console.ReadKey();
    }
    static void Showinstructions()
    {
        Console.WriteLine($"I: Inserts a new car.\nU: Updates an existing car.\nD: Deletes an existings car.\nL: Lists current inventory.\nS: Shows these instructions.\nP: Looks up pet name.\nQ: Quits program.");
    }
    static void ListInventory(InventoryDAL invDAL)
    {
        DataTable dt = invDAL.GetAllInventoryAsDataTable();
        DisplayTable(dt);
    }
    static void DisplayTable(DataTable dt)
    {
        for(int c = 0; c < dt.Columns.Count; c++)
        {
            Console.Write(dt.Columns[c].ColumnName + "\t");
        }            
        Console.WriteLine("\n------------------------------------------------------");

        for(int r = 0; r < dt.Rows.Count; r++)
        {
            for(int c = 0; c < dt.Columns.Count; c++)
            {
                Console.Write(dt.Rows[r][c].ToString() + "\t");
            }
            Console.WriteLine();
        }        
    }
    static void ListInventoryViaList(InventoryDAL inventoryDAL)
    {
        List<NewCar> record = inventoryDAL.GetAllInventoryAsList();
        foreach (NewCar c in record)
        {
            Console.WriteLine($"CarID: {c.CarID}, Make: {c.Make}, Color: {c.Color}, PetName: {c.PetName}");
        }
    }
    static void DeleteCar(InventoryDAL invDAL)
    {
        Console.WriteLine("Enter ID of Car to delete: ");        
        try
        {
            int id = int.Parse(Console.ReadLine()!);
            invDAL.DeleteCar(id);
        }
        catch(Exception ex) { Console.WriteLine(ex.Message); }
    }
    static void InsertNewCar(InventoryDAL invDAL)
    {
        int newCarID = 0;
        string newCarMake, newCarColor, newCarPetName;
        
        while(newCarID.GetType().GetType() != typeof(int) && newCarID == 0)
        {
            Console.WriteLine("Enter Car ID:");
            try { newCarID = int.Parse(Console.ReadLine()!); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }            
        }

        Console.WriteLine("Enter Car Make:");
        newCarMake = Console.ReadLine()!;

        Console.WriteLine("Enter Car Color:");
        newCarColor = Console.ReadLine()!;

        Console.WriteLine("Enter Car Name:");
        newCarPetName = Console.ReadLine()!;

        //newCarID = 2;
        //newCarMake = "Lada";
        //newCarColor = "Green";
        //newCarPetName = "Largus";

        try
        {
            NewCar c = new NewCar { CarID = newCarID, Make = newCarMake, Color = newCarColor, PetName = newCarPetName };
            invDAL.InsertAuto(c);
        }
        catch(Exception ex) { Console.WriteLine(ex.Message); }

    }
    static void UpdateCarPetName(InventoryDAL invDAL)
    {
        Console.WriteLine("Enter Car ID: ");
        int CarID = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter New Pet Name: ");
        string newCarPetName = Console.ReadLine()!;
        invDAL.UpdateCarPetName(CarID, newCarPetName);
    }
    static void LookUpPetName(InventoryDAL invDAL)
    {
        Console.WriteLine("Enter ID of car to look up: ");
        try
        {
            int id = int.Parse(Console.ReadLine()!);
            Console.WriteLine($"Petname of {id} is {invDAL.LookUpPetName(id).TrimEnd()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
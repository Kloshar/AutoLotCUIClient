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
                userCommand = Console.ReadLine();
                Console.WriteLine();
                switch (userCommand.ToUpper())
                {
                    //case "I":
                    //    InsertNewCar(invDaL);
                    //    break;
                    //case "U":
                    //    UpdateCarPetName(invDaL);
                    //    break;
                    //case "D":
                    //    DeleteCar(invDaL);
                    //    break;
                    case "L":
                        ListInventory(invDaL);
                        break;
                    case "S":
                        Showinstructions();
                        break;
                    //case "P":
                    //    LookUpPetName(invDaL);
                    //    break;
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
        for(int c = 0; c < dt.Rows.Count; c++) Console.WriteLine(dt.Columns[c].ColumnName + "\t");
        Console.WriteLine("\n------------------------------------------------------");
        for(int r = 0; r < dt.Rows.Count; r++)
        {
            for(int c = 0; c < dt.Columns.Count; c++) Console.WriteLine(dt.Rows[r][c].ToString() + "\t");
        }
        Console.WriteLine();
    }
}
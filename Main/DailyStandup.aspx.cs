using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;



namespace DWD_DailyStandup.Main
{
  public partial class DailyStandup : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Calendar1.SelectedDate = DateTime.Now;
      fillGridview(DateTime.Now);
 

    }


    private void fillGridview(DateTime StartingDateDayOfWeek)
    {

      // Run the query
      SqlParameter p1 = new SqlParameter();
      SqlParameter p2 = new SqlParameter();
      //Create Parameters to Pass into Stored Procedure
      p1.Value = StartingDateDayOfWeek;
      p2.Value = StartingDateDayOfWeek.AddDays(1);

        DataSet ds = GetDataWithSP("dbo.pspGetDayInfo", p1, p2);

 
        if (ds.Tables.Count > 0)
        {
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {

        }
        }





    
    }

    DataSet GetDataWithSP(String sp, SqlParameter p1, SqlParameter p2)
    {

      // Retrieve the connection string stored in the Web.config file.
      String connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

      DataSet ds = new DataSet();

      try
      {
        // Connect to the database and run the query.
        SqlConnection connection = new SqlConnection(connectionString);
        SqlDataAdapter da = new SqlDataAdapter(sp, connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddWithValue("@DateStart", p1.Value);
        da.SelectCommand.Parameters.AddWithValue("@DateEnd", p2.Value);

        // Fill the DataSet.
        da.Fill(ds);

      }
      catch (Exception ex)
      {

        // The connection failed. Display an error message.
        Response.Write(ex);

      }

      return ds;
    }


    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
     //DateTime endDate = new DateTime();
      int intDatesSelected;
      intDatesSelected = Calendar1.SelectedDates.Count - 1;
      
    }

  }
}
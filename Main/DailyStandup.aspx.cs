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


    #region Events    //--------------------------------------------------------------------

    protected void Page_Load(object sender, EventArgs e)
    {


      if (!IsPostBack)
      {
        Calendar1.SelectedDate = DateTime.Now;
        lblDate.Text = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
        fillGridview(Calendar1.SelectedDate);
      }

    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
      //Update Date Label
      lblDate.Text = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
      //Update the Standup Page
      fillGridview(Calendar1.SelectedDate);

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

      AddNewStandUp();
    }



    protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!(ddlProjects.Text == "[choose project]"))
      {
        AllowStandupEntry(true);
      }
      else
      {
        AllowStandupEntry(false);
      }
    }


    #endregion    //--------------------------------------------------------------------

    private void AddNewStandUp()
    {

      try
      {
        //Calls the insert Stored Procedure

        // Setup Connection
        String connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
        //Setu Command
        SqlCommand command = new SqlCommand("dbo.pspInsNewStandup", connection);
        command.CommandType = CommandType.StoredProcedure;

        // Add the parameters to Pass into SP
        command.Parameters.AddWithValue("@Date", Calendar1.SelectedDate);
        command.Parameters.AddWithValue("@Yesterday", txtYesterday.Text);
        command.Parameters.AddWithValue("@Today", txtToday.Text);
        command.Parameters.AddWithValue("@Impediments", txtImpediments.Text);
        command.Parameters.AddWithValue("@ProjectName", ddlProjects.Text);

        // Connect to the database and run the Insert.
        using (connection)
        {
          connection.Open();
          command.ExecuteNonQuery();
        }
      }
      catch (Exception ex)
      {

        Response.Write(ex);
      }
    }


    private void fillGridview(DateTime StartingDateDayOfWeek)
    {

      // Run the query
      SqlParameter p1 = new SqlParameter();
      SqlParameter p2 = new SqlParameter();
      //Create Parameters to Pass into Stored Procedure
      p1.Value = StartingDateDayOfWeek;
      p2.Value = StartingDateDayOfWeek.AddDays(1);
      //Run the stored procedure storing result in new data set
      //The procedure only uses Date, so the time is 12AM for that day
      DataSet ds = GetDataWithSP("dbo.pspGetDayInfo", p1, p2);


      if (ds.Tables.Count > 0)
      {
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
          txtYesterday.Text = dt.Rows[0][0].ToString();
          txtToday.Text = dt.Rows[0][1].ToString();
          txtImpediments.Text = dt.Rows[0][2].ToString();
          Guid ProjectId = new Guid(dt.Rows[0]["ProjectID"].ToString());
          //ddlProjects.Items =ProjectId ;
        }
        else
        {
          txtYesterday.Text = "Yesterday I ";
          txtToday.Text = "Today I ";
          txtImpediments.Text = "My impediments are ";
          ddlProjects.Text = "[choose project]";
          AllowStandupEntry(false);
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



    private void AllowStandupEntry(Boolean Enable)
    {
      //Allow data entry only if a valid project has been selected
        txtYesterday.Enabled = Enable;
        txtToday.Enabled = Enable;
        txtImpediments.Enabled = Enable;
    }


  }
}
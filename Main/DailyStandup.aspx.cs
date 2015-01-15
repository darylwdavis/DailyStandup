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
    #region Private Members    //--------------------------------------------------------------------
    //Set default string if no Project is selected
    private string mDefaultProjectName = "[choose project]";
    #endregion                //--------------------------------------------------------------------



    #region Events    //--------------------------------------------------------------------

    protected void Page_Load(object sender, EventArgs e)
    {


      if (!IsPostBack)
      {
        //Set Behavior of Projects DropDownList
        ddlProjects.DataValueField = "ProjectID";
        ddlProjects.DataTextField = "Project";
        //Set The Calendar to today
        Calendar1.SelectedDate = DateTime.Now;
        lblDate.Text = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
        //Fill the Text Boxes
        UpdateTextBoxes(Calendar1.SelectedDate);

        //Set the New Project MultiView to default view
        MultiView1.ActiveViewIndex = 0;
      }

    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
      //Update Date Label
      lblDate.Text = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
      //Update the Standup Page
      UpdateTextBoxes(Calendar1.SelectedDate);

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

      AddNewStandUp();
    }



    protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
    {
      CheckForValidProject();

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


    private void UpdateTextBoxes(DateTime StartingDateDayOfWeek)
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
          txtYesterday.Text = dt.Rows[0]["Yesterday"].ToString();
          txtToday.Text = dt.Rows[0]["Today"].ToString();
          txtImpediments.Text = dt.Rows[0]["Impediments"].ToString();

          //Set the ddl SelectedValue to the Project Guid
          ddlProjects.SelectedValue = dt.Rows[0]["ProjectID"].ToString();

          //
          CheckForValidProject();

        }
        else
        {
          txtYesterday.Text = "Yesterday I ";
          txtToday.Text = "Today I ";
          txtImpediments.Text = "My impediments are ";
          //Set the ddl SelectedValue Guid by finding by Text
          ddlProjects.SelectedValue = ddlProjects.Items.FindByText(mDefaultProjectName).Value;
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

    private void CheckForValidProject()
    {
      if (!(ddlProjects.SelectedItem.Text == mDefaultProjectName))
      {
        AllowStandupEntry(true);
      }
      else
      {
        AllowStandupEntry(false);
      }
    }

    private void AllowStandupEntry(Boolean Enable)
    {
      //Allow data entry only if a valid project has been selected
      txtYesterday.Enabled = Enable;
      txtToday.Enabled = Enable;
      txtImpediments.Enabled = Enable;
    }

    protected void sdsProjects_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }

    protected void btnAddProject_Click(object sender, EventArgs e)
    {
      //Add Project Mode
      MultiView1.ActiveViewIndex = 1;

    }

    protected void btnAddProject_Click1(object sender, EventArgs e)
    {
      //Add project to db
      

      //Return to Normal Mode
      MultiView1.ActiveViewIndex = 0;

    }


  }
}
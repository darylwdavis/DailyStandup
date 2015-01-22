using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace DWD_DailyStandup.Main
{
  public partial class DailyStandup : System.Web.UI.Page
  {

    #region Public Properties    //--------------------------------------------------------------------



    #endregion                //--------------------------------------------------------------------


    #region Private Members    //--------------------------------------------------------------------
    //Set default string if no Project is selected
    private string mDefaultProjectName = "[choose project]";

    //ProjectID dictionary for selected Day
    private Dictionary<int, Guid> DayProjects;
    //ProjectID Index for dictionary
    private int DayProjectsIndex;

    const string VM = "View Mode";
    const string EM = "Edit Mode";


    private bool ProjectModeEdit = false;

    #endregion                //--------------------------------------------------------------------



    #region Events    //--------------------------------------------------------------------

    protected void Page_Load(object sender, EventArgs e)
    {

      if (!IsPostBack)
      {

        //Configure the sql DataSource obj for the Drop Down List
        SetupProjectsList();
        //Set Behavior of Projects DropDownList
        ddlProjects.DataValueField = "ProjectID";
        ddlProjects.DataTextField = "Project";
        //Set The Calendar to today
        Calendar1.SelectedDate = DateTime.Now;
        lblDate.Text = Calendar1.SelectedDate.ToString("yyyy-MM-dd");


        //Set the New Project MultiView to default view
        MultiView1.ActiveViewIndex = 0;

        this.Page.Header.Title = "Daily Stand-Up";
      }
      else
      {

        DayProjectsIndex = Convert.ToInt16(ViewState["DayProjectsIndex"]);
      }

    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
      //Set View Mode
      SetViewMode(true);
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
      //Set Standup to default
      ClearStandupBoxes();
      //Check for Non-default project
      CheckForValidProject();

    }


    protected void ddlProjects_DataBound(object sender, EventArgs e)
    {

      //Fill the Text Boxes
      UpdateTextBoxes(Calendar1.SelectedDate);
    }

    protected void btnNewProject_Click(object sender, EventArgs e)
    {
      //Add Project Mode
      MultiView1.ActiveViewIndex = 1;
    }

    protected void btnAddProject_Click(object sender, EventArgs e)
    {
      //Add project to db
      AddNewProject();
    }


    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
      ClearAddProject();
    }

    protected void btnMode_Click(object sender, EventArgs e)
    {
      ChangeMode(btnMode.Text);

    }

    protected void btnNextProject_Click(object sender, EventArgs e)
    {
      //Move to next Project
      DisplayNextProject();

    }
    #endregion    //--------------------------------------------------------------------

    private void AddNewProject()
    {

      try
      {
        //Calls the insert new Project Stored Procedure

        // Setup Connection
        String connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionString);
        //Setup Command
        SqlCommand command = new SqlCommand("dbo.pspInsNewProject", connection);
        command.CommandType = CommandType.StoredProcedure;

        // Add the parameters to Pass into SP
        command.Parameters.AddWithValue("@ProjectName", txtNewProject.Text);
        command.Parameters.AddWithValue("@Details", txtNewProjectDetails.Text);


        // Connect to the database and run the Insert.
        using (connection)
        {
          connection.Open();
          command.ExecuteNonQuery();
        }

        //Re-bind the ddlProjects
        ddlProjects.DataBind();

        //Data Saved, clear the txt boxes
        ClearAddProject();

      }
      catch (Exception ex)
      {

        Response.Write(ex);
      }
    }


    private void AddNewStandUp()
    {

      try
      {
        //Calls the insert Stored Procedure

        // Setup Connection
        String connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionString);
        //Setup Command
        SqlCommand command = new SqlCommand("dbo.pspInsNewStandup", connection);
        command.CommandType = CommandType.StoredProcedure;

        // Add the parameters to Pass into SP
        command.Parameters.AddWithValue("@Date", Calendar1.SelectedDate);
        command.Parameters.AddWithValue("@Yesterday", txtYesterday.Text);
        command.Parameters.AddWithValue("@Today", txtToday.Text);
        command.Parameters.AddWithValue("@Impediments", txtImpediments.Text);
        command.Parameters.AddWithValue("@ProjectID", ddlProjects.SelectedValue);

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


    private void ClearAddProject()
    {
      //Clear the txt boxes
      txtNewProject.Text = "";
      txtNewProjectDetails.Text = "";

      //Return to Normal Mode
      MultiView1.ActiveViewIndex = 0;
    }


    private void ClearStandupBoxes()
    {
      txtYesterday.Text = "Yesterday I ";
      txtToday.Text = "Today I ";
      txtImpediments.Text = "My impediments are ";

    }

    private void DisplayNextProject()
    {
      //Increment Project Pointer
      DayProjectsIndex = (DayProjectsIndex <= DayProjects.Count) ? DayProjectsIndex++ : 0;
      //Update Text Boxes
      UpdateTextBoxes(DayProjects[DayProjectsIndex]);
    }


    private void SetupProjectsList()
    {
      //Connects the dropdown list for all projects

      // Setup Connection
      String connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
      SqlConnection connection = new SqlConnection(connectionString);

      //remove existing datasource
      sdsProjects.SelectCommand = null;

      //Setup Command
      SqlCommand command = new SqlCommand("dbo.pspGetAllProjects", connection);
      command.CommandType = CommandType.StoredProcedure;
      sdsProjects.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
      sdsProjects.SelectCommand = command.CommandText;

    }

    private void SetupProjectsList(DateTime Date)
    {
      //Connects the dropdown list for all projects on Date
      //ddlProjects.Items.Clear();

      // Setup Connection
      String connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
      SqlConnection connection = new SqlConnection(connectionString);

      //remove existing datasource
      sdsProjects.SelectCommand = null;

      //Setup Command
      SqlCommand command = new SqlCommand("dbo.pspGetAllProjectsByStandupDate", connection);
      command.CommandType = CommandType.StoredProcedure;

      // Add the parameters to Pass into SP
      sdsProjects.SelectParameters.Add("date", Date.ToString());    //Not SqldataSource adds the @ parameter name, unlike the sqlcommand

      //Hook the datasource to the stored proc
      sdsProjects.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
      sdsProjects.SelectCommand = command.CommandText;
    }



    private void UpdateTextBoxes(Guid ProjectID)
    {
      // Run the query
      SqlParameter p1 = new SqlParameter();
      SqlParameter p2 = new SqlParameter();
      SqlParameter p3 = new SqlParameter();
      //Create Parameters to Pass into Stored Procedure
      p1.Value = Calendar1.SelectedDate;
      p2.Value = Calendar1.SelectedDate.AddDays(1);
      p3.Value = ProjectID;

      //Run the stored procedure storing result in new data set
      //The procedure only uses Date, so the time is 12AM for that day
      DataSet ds = GetDataWithSP("dbo.pspGetDayInfoByProjectID", p1, p2, p3);


      if (ds.Tables.Count > 0)
      {
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {

          txtYesterday.Text = dt.Rows[0]["Yesterday"].ToString();
          txtToday.Text = dt.Rows[0]["Today"].ToString();
          txtImpediments.Text = dt.Rows[0]["Impediments"].ToString();
        }
      }

    }


    private void UpdateTextBoxes(DateTime SelectedDate)
    {

      // Run the query
      SqlParameter p1 = new SqlParameter();
      SqlParameter p2 = new SqlParameter();
      SqlParameter p3 = new SqlParameter();
      //Create Parameters to Pass into Stored Procedure
      p1.Value = SelectedDate;
      p2.Value = SelectedDate.AddDays(1);
      //Run the stored procedure storing result in new data set
      //The procedure only uses Date, so the time is 12AM for that day
      DataSet ds = GetDataWithSP("dbo.pspGetDayInfo", p1, p2, p3);


      if (ds.Tables.Count > 0)
      {
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {

          txtYesterday.Text = dt.Rows[0]["Yesterday"].ToString();
          txtToday.Text = dt.Rows[0]["Today"].ToString();
          txtImpediments.Text = dt.Rows[0]["Impediments"].ToString();

          //Check Number of Projects On this Date
          if (dt.Rows.Count == 1)                                  //One Project reported on this day
          {
            //Disable Multi-Project Scrolling
            SetButtonNextState(false);
          }
          else if (dt.Rows.Count > 1)                   //Multiple Projects reported on this day                  && !(ProjectModeEdit)
          {
            //Enable Multi-Project Scrlling
            SetButtonNextState(true);
          }

          //Rebuild the Projects dd List for the selected date
          //DayProjects.Clear();  //Using a dictionary, currently onlyl storeing Guid
          //Instantiate DayProjects Dictionary
          DayProjects = new Dictionary<int, Guid>();
          for (int DayProjectsIndex = 0; DayProjectsIndex < dt.Rows.Count; DayProjectsIndex++)
          {
            DayProjects.Add(DayProjectsIndex, new Guid(dt.Rows[DayProjectsIndex]["ProjectID"].ToString()));
          }


          //Set the ddl SelectedValue to the Project Guid
          ddlProjects.SelectedValue = dt.Rows[0]["ProjectID"].ToString();   //View All: dt.Rows[0].ItemArray


          //
          CheckForValidProject();

        }
        else                                                        //No projects reported this day
        {
          //Disable Multi-Project Scrolling
          SetButtonNextState(false);
          //Clear Standup Boxes
          ClearStandupBoxes();
          //Set the ddl SelectedValue Guid by finding by Text
          if (ddlProjects.Items.Count > 1)
          {
            ddlProjects.SelectedValue = ddlProjects.Items.FindByText(mDefaultProjectName).Value;
          }
          AllowStandupEntry(false);
        }
      }
    }

    private void FillProjectDictionary(DateTime d)
    {

    }





    private void SetButtonNextState(Boolean enable)
    {
      //Set State and text of btnNext
      btnNextProject.Enabled = enable;
      btnNextProject.Text = enable ? ">>" : "--";
    }

    DataSet GetDataWithSP(String sp, SqlParameter p1, SqlParameter p2, SqlParameter p3)
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

      //Check for Default Project
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








    private void ChangeMode(string ModeName)
    {
      switch (ModeName)
      {
        case VM:
          //Change to Edit Mode
          ProjectModeEdit = true;
          btnMode.Text = EM;
          break;
        case EM:
          //Change to View Mode
          ProjectModeEdit = false;
          btnMode.Text = VM;
          break;

      }
    }


    private void SetViewMode(bool EditMode)
    {
      ProjectModeEdit = EditMode;
      if (EditMode == true)
      {
        btnMode.Text = EM;
      }
      else
      {
        btnMode.Text = VM;
      }
    }





  }
}
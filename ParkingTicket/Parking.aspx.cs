using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ParkingTicket
{
    public partial class Parking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadParkingRecords();
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                string carNumber = txtCarNumber.Text;
                DateTime inTime = DateTime.Parse(txtInTime.Text);
                DateTime outTime = DateTime.Parse(txtOutTime.Text);

                if (outTime <= inTime)
                {
                    lblResult.Text = "Out time must be later than in time.";
                    return;
                }

                TimeSpan duration = outTime - inTime;
                double fare = duration.TotalHours * 10; // ₹10 per hour

                lblResult.Text = $"Car Number: {carNumber}<br/>Duration: {duration.TotalHours:F2} hours<br/>Total Fare: ₹{fare:F2}";

                string connString = ConfigurationManager.ConnectionStrings["ParkingDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "INSERT INTO ParkingRecords (CarNumber, InTime, OutTime, Duration, Fare) VALUES (@CarNumber, @InTime, @OutTime, @Duration, @Fare)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarNumber", carNumber);
                        cmd.Parameters.AddWithValue("@InTime", inTime);
                        cmd.Parameters.AddWithValue("@OutTime", outTime);
                        cmd.Parameters.AddWithValue("@Duration", duration.TotalHours);
                        cmd.Parameters.AddWithValue("@Fare", fare);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadParkingRecords(); // Refresh grid after adding record
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error saving data: " + ex.Message;
            }
        }

        protected void btnLoadRecords_Click(object sender, EventArgs e)
        {
            LoadParkingRecords();
        }

        private void LoadParkingRecords(string carNumber = "")
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["ParkingDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT ID, CarNumber, InTime, OutTime, Duration, Fare FROM ParkingRecords";
                    if (!string.IsNullOrEmpty(carNumber))
                    {
                        query += " WHERE CarNumber = @CarNumber";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(carNumber))
                        {
                            cmd.Parameters.AddWithValue("@CarNumber", carNumber);
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            gvParkingRecords.DataSource = dt;
                            gvParkingRecords.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error loading data: " + ex.Message;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadParkingRecords(txtSearchCarNumber.Text);
        }

        protected void gvParkingRecords_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ParkingDB"].ConnectionString;

            if (e.CommandName == "DeleteRow")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "DELETE FROM ParkingRecords WHERE ID = @ID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadParkingRecords();
                }
                catch (Exception ex)
                {
                    lblResult.Text = "Error deleting record: " + ex.Message;
                }
            }
            else if (e.CommandName == "EditRow")
            {
                // Redirect to an Edit page with the selected ID
                Response.Redirect("EditParking.aspx?id=" + e.CommandArgument);
            }
        }
    }
}

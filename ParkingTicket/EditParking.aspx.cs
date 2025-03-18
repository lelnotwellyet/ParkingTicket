using System;
using System.Data.SqlClient;

namespace ParkingTicket
{
    public partial class EditParking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadParkingDetails();
            }
        }

        private void LoadParkingDetails()
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id)) return;

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ParkingDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT * FROM ParkingRecords WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtCarNumber.Text = reader["CarNumber"].ToString();
                        txtInTime.Text = Convert.ToDateTime(reader["InTime"]).ToString("yyyy-MM-ddTHH:mm");
                        txtOutTime.Text = Convert.ToDateTime(reader["OutTime"]).ToString("yyyy-MM-ddTHH:mm");
                    }
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string id = Request.QueryString["id"];
                string carNumber = txtCarNumber.Text;
                DateTime inTime = DateTime.Parse(txtInTime.Text);
                DateTime outTime = DateTime.Parse(txtOutTime.Text);

                if (outTime <= inTime)
                {
                    lblMessage.Text = "Out time must be later than in time.";
                    return;
                }

                TimeSpan duration = outTime - inTime;
                double fare = duration.TotalHours * 10; 

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ParkingDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "UPDATE ParkingRecords SET CarNumber=@CarNumber, InTime=@InTime, OutTime=@OutTime, Duration=@Duration, Fare=@Fare WHERE Id=@Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarNumber", carNumber);
                        cmd.Parameters.AddWithValue("@InTime", inTime);
                        cmd.Parameters.AddWithValue("@OutTime", outTime);
                        cmd.Parameters.AddWithValue("@Duration", duration.TotalHours);
                        cmd.Parameters.AddWithValue("@Fare", fare);
                        cmd.Parameters.AddWithValue("@Id", id);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                lblMessage.Text = "Record updated successfully!";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error updating record: " + ex.Message;
            }
        }
    }
}

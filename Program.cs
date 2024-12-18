using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace AcademicSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }

    public partial class MainForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();

            this.SuspendLayout();

            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(50, 50);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 23);
            this.txtUsername.Text = "Username";
            this.txtUsername.ForeColor = System.Drawing.Color.Gray;
            this.txtUsername.Enter += new EventHandler(RemovePlaceholder);
            this.txtUsername.Leave += new EventHandler(AddPlaceholder);

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(50, 90);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 23);
            this.txtPassword.Text = "Password";
            this.txtPassword.ForeColor = System.Drawing.Color.Gray;
            this.txtPassword.Enter += new EventHandler(RemovePlaceholder);
            this.txtPassword.Leave += new EventHandler(AddPlaceholder);

            // btnLogin
            this.btnLogin.Location = new System.Drawing.Point(50, 130);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);

            // MainForm
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Name = "MainForm";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            string role = AuthenticateUser(username, password);

            if (role == "Admin")
            {
                AdminForm adminForm = new AdminForm();
                adminForm.Show();
            }
            else if (role == "Lecturer")
            {
                LecturerForm lecturerForm = new LecturerForm();
                lecturerForm.Show();
            }
            else if (role == "Student")
            {
                StudentForm studentForm = new StudentForm(username);
                studentForm.Show();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string AuthenticateUser(string username, string password)
        {
            using (var conn = new SqliteConnection("Data Source=academic.db"))
            {
                conn.Open();
                string query = "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["Role"].ToString();
                        }
                    }
                }
            }
            return null;
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.ForeColor == System.Drawing.Color.Gray)
            {
                txt.Text = "";
                txt.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void AddPlaceholder(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                if (txt == txtUsername)
                    txt.Text = "Username";
                else if (txt == txtPassword)
                    txt.Text = "Password";

                txt.ForeColor = System.Drawing.Color.Gray;
            }
        }
    }

    public partial class AdminForm : Form
    {
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private ComboBox cmbRole;
        private Button btnCreateUser;

        public AdminForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtFirstName = new TextBox();
            this.txtLastName = new TextBox();
            this.cmbRole = new ComboBox();
            this.btnCreateUser = new Button();

            this.SuspendLayout();

            // txtFirstName
            this.txtFirstName.Location = new System.Drawing.Point(50, 50);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(200, 23);
            this.txtFirstName.Text = "Username";
            this.txtFirstName.ForeColor = System.Drawing.Color.Gray;
            this.txtFirstName.Enter += new EventHandler(RemovePlaceholder);
            this.txtFirstName.Leave += new EventHandler(AddPlaceholder);

            // txtLastName
            this.txtLastName.Location = new System.Drawing.Point(50, 90);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(200, 23);
            this.txtLastName.Text = "Lastname";
            this.txtLastName.ForeColor = System.Drawing.Color.Gray;
            this.txtLastName.Enter += new EventHandler(RemovePlaceholder);
            this.txtLastName.Leave += new EventHandler(AddPlaceholder);

            // cmbRole
            this.cmbRole.Location = new System.Drawing.Point(50, 130);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Items.AddRange(new string[] { "Admin", "Lecturer", "Student" });
            this.cmbRole.Size = new System.Drawing.Size(200, 23);

            // btnCreateUser
            this.btnCreateUser.Location = new System.Drawing.Point(50, 170);
            this.btnCreateUser.Name = "btnCreateUser";
            this.btnCreateUser.Size = new System.Drawing.Size(100, 23);
            this.btnCreateUser.Text = "Create User";
            this.btnCreateUser.Click += new EventHandler(this.btnCreateUser_Click);

            // AdminForm
            this.ClientSize = new System.Drawing.Size(300, 250);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.btnCreateUser);
            this.Name = "AdminForm";
            this.Text = "Admin Panel";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string role = cmbRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = firstName.ToLower();
            string password = lastName.ToLower();

            using (var conn = new SqliteConnection("Data Source=academic.db"))
            {
                conn.Open();
                string query = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show($"Usuario creado: {username} con contraseña: {password}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.ForeColor == System.Drawing.Color.Gray)
            {
                txt.Text = "";
                txt.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void AddPlaceholder(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                if (txt == txtFirstName)
                    txt.Text = "Username";
                else if (txt == txtLastName)
                    txt.Text = "Lastname";

                txt.ForeColor = System.Drawing.Color.Gray;
            }
        }
    }

    public partial class LecturerForm : Form
    {
        private ComboBox cmbUsers;
        private ComboBox cmbCourses;
        private TextBox txtGrade;
        private Button btnUpdateGrade;

        public LecturerForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.cmbUsers = new ComboBox();
            this.cmbCourses = new ComboBox();
            this.txtGrade = new TextBox();
            this.btnUpdateGrade = new Button();

            this.SuspendLayout();

            // cmbUsers
            this.cmbUsers.Location = new System.Drawing.Point(50, 20);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(200, 23);
            this.cmbUsers.DropDownStyle = ComboBoxStyle.DropDownList;
            LoadUsers();

            // cmbCourses
            this.cmbCourses.Location = new System.Drawing.Point(50, 60);
            this.cmbCourses.Name = "cmbCourses";
            this.cmbCourses.Size = new System.Drawing.Size(200, 23);
            this.cmbCourses.DropDownStyle = ComboBoxStyle.DropDownList;
            LoadCourses();

            // txtGrade
            this.txtGrade.Location = new System.Drawing.Point(50, 100);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.Size = new System.Drawing.Size(200, 23);
            this.txtGrade.Text = "Grade";

            // btnUpdateGrade
            this.btnUpdateGrade.Location = new System.Drawing.Point(50, 140);
            this.btnUpdateGrade.Name = "btnUpdateGrade";
            this.btnUpdateGrade.Size = new System.Drawing.Size(100, 23);
            this.btnUpdateGrade.Text = "Update Grade";
            this.btnUpdateGrade.Click += new EventHandler(this.btnUpdateGrade_Click);

            // LecturerForm
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.cmbUsers);
            this.Controls.Add(this.cmbCourses);
            this.Controls.Add(this.txtGrade);
            this.Controls.Add(this.btnUpdateGrade);
            this.Name = "LecturerForm";
            this.Text = "Lecturer Panel";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadUsers()
        {
            using (var conn = new SqliteConnection("Data Source=academic.db"))
            {
                conn.Open();
                string query = "SELECT Username FROM Users WHERE Role = 'Student'";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbUsers.Items.Add(reader["Username"].ToString());
                        }
                    }
                }
            }
        }

        private void LoadCourses()
        {
            using (var conn = new SqliteConnection("Data Source=academic.db"))
            {
                conn.Open();
                string query = "SELECT DISTINCT Name FROM Courses";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbCourses.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
        }

        private void btnUpdateGrade_Click(object sender, EventArgs e)
        {
            string selectedUser = cmbUsers.SelectedItem?.ToString();
            string selectedCourse = cmbCourses.SelectedItem?.ToString();
            string grade = txtGrade.Text;

            if (string.IsNullOrEmpty(selectedUser) || string.IsNullOrEmpty(selectedCourse) || string.IsNullOrWhiteSpace(grade))
            {
                MessageBox.Show("Por favor, seleccione un usuario, una asignatura y un grado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var conn = new SqliteConnection("Data Source=academic.db"))
            {
                conn.Open();

                // Obtener el StudentId y CourseId
                string getIdQuery = @"
                    SELECT Students.Id AS StudentId, Courses.Id AS CourseId 
                    FROM Users 
                    INNER JOIN Students ON Users.Username = Students.Username
                    INNER JOIN Courses ON Courses.Name = @CourseName
                    WHERE Users.Username = @Username";

                int studentId = -1, courseId = -1;

                using (var cmd = new SqliteCommand(getIdQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", selectedUser);
                    cmd.Parameters.AddWithValue("@CourseName", selectedCourse);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            studentId = Convert.ToInt32(reader["StudentId"]);
                            courseId = Convert.ToInt32(reader["CourseId"]);
                        }
                    }
                }

                if (studentId == -1 || courseId == -1)
                {
                    MessageBox.Show("Error al obtener IDs del estudiante o curso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Actualizar o insertar en Grades
                string updateOrInsertQuery = @"
                    INSERT INTO Grades (StudentId, CourseId, Grade)
                    VALUES (@StudentId, @CourseId, @Grade)
                    ON CONFLICT(StudentId, CourseId) 
                    DO UPDATE SET Grade = @Grade;";

                using (var cmd = new SqliteCommand(updateOrInsertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    cmd.Parameters.AddWithValue("@Grade", grade);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Grado actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public partial class StudentForm : Form
    {
        private string username;
        private ListBox lstGrades;

        public StudentForm(string username)
        {
            this.username = username;
            InitializeComponent();
            LoadGrades();
        }

        private void InitializeComponent()
        {
            this.lstGrades = new ListBox();

            this.SuspendLayout();

            // lstGrades
            this.lstGrades.Location = new System.Drawing.Point(20, 20);
            this.lstGrades.Name = "lstGrades";
            this.lstGrades.Size = new System.Drawing.Size(260, 200);

            // StudentForm
            this.ClientSize = new System.Drawing.Size(300, 250);
            this.Controls.Add(this.lstGrades);
            this.Name = "StudentForm";
            this.Text = $"Student Panel - {username}";
            this.ResumeLayout(false);
        }

        private void LoadGrades()
        {
            using (var conn = new SqliteConnection("Data Source=academic.db"))
            {
                conn.Open();
                string query = @"
                    SELECT Courses.Name AS CourseName, Grades.Grade AS Grade
                    FROM Grades
                    INNER JOIN Courses ON Grades.CourseId = Courses.Id
                    INNER JOIN Students ON Grades.StudentId = Students.Id
                    WHERE Students.Username = @Username";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string courseName = reader["CourseName"].ToString();
                            string grade = reader["Grade"].ToString();
                            lstGrades.Items.Add($"{courseName}: {grade}");
                        }
                    }
                }
            }
        }
    }
}

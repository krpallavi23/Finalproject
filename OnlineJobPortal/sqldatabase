use OnlineJobPortalDB;
--Server=LAPTOP-M4EE6G7H;Database=master;Trusted_Connection=True;
-- 1. User Table
CREATE TABLE [User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL, -- Note: Storing plain-text passwords is insecure
    Email VARCHAR(100) NOT NULL UNIQUE,
    UserType VARCHAR(20) NOT NULL CHECK (UserType IN ('Admin', 'JobSeeker', 'Employer'))
);
 
-- 2. JobSeeker Table
CREATE TABLE JobSeeker (
    JobSeekerID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES [User](UserID) ON DELETE CASCADE, -- Relationship to User
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE,
    Gender VARCHAR(10),
    AddressLine1 VARCHAR(255),
    AddressLine2 VARCHAR(255),
    City VARCHAR(100),
    State VARCHAR(100),
    Country VARCHAR(100),
    PostalCode VARCHAR(20),
    ResumePath VARCHAR(255),
    YearsOfExperience INT
);
 
-- 3. Employer Table
CREATE TABLE Employer (
    EmployerID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES [User](UserID) ON DELETE CASCADE,
    CompanyName VARCHAR(100) NOT NULL,
    AddressLine1 VARCHAR(255),
    AddressLine2 VARCHAR(255),
    City VARCHAR(100),
    State VARCHAR(100),
    Country VARCHAR(100),
    PostalCode VARCHAR(20),
    Email VARCHAR(100) NOT NULL UNIQUE,
    Website VARCHAR(255),
    ContactPerson VARCHAR(100),
    ContactNumber VARCHAR(20)
);
 
-- 4. JobPosting Table
CREATE TABLE JobPosting (
    JobID INT PRIMARY KEY IDENTITY(1,1),
    EmployerID INT NOT NULL FOREIGN KEY REFERENCES Employer(EmployerID) ON DELETE CASCADE,
    Title VARCHAR(100) NOT NULL,
    Description TEXT,
    Location VARCHAR(100),
    Salary DECIMAL(10, 2),
    ApplicationDeadline DATE,
    JobType VARCHAR(20) CHECK (JobType IN ('Full-Time', 'Part-Time', 'Contract', 'Freelance')),
    Status VARCHAR(20) CHECK (Status IN ('Active', 'Closed')),
    JobCategory VARCHAR(100) NOT NULL -- Direct field for job category
);
 
-- 5. JobApplication Table
CREATE TABLE JobApplication (
    ApplicationID INT PRIMARY KEY IDENTITY(1,1),
    JobSeekerID INT NOT NULL FOREIGN KEY REFERENCES JobSeeker(JobSeekerID) ,
    JobID INT NOT NULL FOREIGN KEY REFERENCES JobPosting(JobID) ,
    ApplicationDate DATE NOT NULL,
    Feedback TEXT,
    Status VARCHAR(20) CHECK (Status IN ('Applied', 'Interviewed', 'Rejected', 'Accepted'))
);
 
-- 6. SkillTag Table
CREATE TABLE SkillTag (
    SkillTagID INT PRIMARY KEY IDENTITY(1,1),
    SkillName VARCHAR(100) NOT NULL UNIQUE
);
 
-- 7. JobSkill Table
CREATE TABLE JobSkill (
    JobID INT NOT NULL FOREIGN KEY REFERENCES JobPosting(JobID) , -- Relationship to JobPosting
    SkillTagID INT NOT NULL FOREIGN KEY REFERENCES SkillTag(SkillTagID) , -- Relationship to SkillTag
    PRIMARY KEY (JobID, SkillTagID)
);
 
-- 8. JobSeekerSkill Table
CREATE TABLE JobSeekerSkill (
    JobSeekerID INT NOT NULL FOREIGN KEY REFERENCES JobSeeker(JobSeekerID) , -- Relationship to JobSeeker
    SkillTagID INT NOT NULL FOREIGN KEY REFERENCES SkillTag(SkillTagID) , -- Relationship to SkillTag
    SkillLevel INT CHECK (SkillLevel BETWEEN 1 AND 5), -- Assuming SkillLevel is rated between 1 and 5
    PRIMARY KEY (JobSeekerID, SkillTagID)
);
 
-- 9. AcademicDetails Table
CREATE TABLE AcademicDetails (
    AcademicDetailID INT PRIMARY KEY IDENTITY(1,1),
    JobSeekerID INT NOT NULL FOREIGN KEY REFERENCES JobSeeker(JobSeekerID) , -- Relationship to JobSeeker
    Degree VARCHAR(100) NOT NULL, -- Degree pursued by the Job Seeker
    EducationLevel VARCHAR(20) NOT NULL CHECK (EducationLevel IN ('10th', '12th', 'Graduation')),
    Marks DECIMAL(5, 2) NOT NULL,
    BoardOrUniversity VARCHAR(255),
    YearOfCompletion INT
);

CREATE TABLE ChatMessages (
    ChatMessageID INT IDENTITY(1,1) PRIMARY KEY,
    EmployerID INT NOT NULL,
    JobSeekerID INT NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    MessageTime DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (EmployerID) REFERENCES Employer(EmployerID) ,
    FOREIGN KEY (JobSeekerID) REFERENCES JobSeeker(JobSeekerID) 
);
 
-- 1. Insert into [User] Table
INSERT INTO [User] (Username, Password, Email, UserType) VALUES
('admin1', 'Password@123', 'admin1@example.com', 'Admin'),
('admin2', 'AdminPass@456', 'admin2@example.com', 'Admin'),
('john_doe', 'JohnPass@789', 'john.doe@example.com', 'JobSeeker'),
('jane_smith', 'JanePass@321', 'jane.smith@example.com', 'JobSeeker'),
('alice_jones', 'AlicePass@654', 'alice.jones@example.com', 'JobSeeker'),
('bob_brown', 'BobPass@987', 'bob.brown@example.com', 'JobSeeker'),
('employer1', 'EmpPass@111', 'contact@company1.com', 'Employer'),
('employer2', 'EmpPass@222', 'contact@company2.com', 'Employer'),
('employer3', 'EmpPass@333', 'contact@company3.com', 'Employer'),
('employer4', 'EmpPass@444', 'contact@company4.com', 'Employer');
GO

-- 2. Insert into JobSeeker Table
-- Assuming UserIDs for JobSeekers are 3 to 6
INSERT INTO JobSeeker (UserID, FirstName, LastName, DateOfBirth, Gender, AddressLine1, AddressLine2, City, State, Country, PostalCode, ResumePath, YearsOfExperience) VALUES
(3, 'John', 'Doe', '1990-05-15', 'Male', '123 Main St', 'Apt 4B', 'New York', 'NY', 'USA', '10001', '/resumes/john_doe.pdf', 5),
(4, 'Jane', 'Smith', '1988-08-22', 'Female', '456 Elm St', NULL, 'Los Angeles', 'CA', 'USA', '90001', '/resumes/jane_smith.pdf', 7),
(5, 'Alice', 'Jones', '1992-12-05', 'Female', '789 Oak St', 'Suite 12', 'Chicago', 'IL', 'USA', '60601', '/resumes/alice_jones.pdf', 3),
(6, 'Bob', 'Brown', '1985-03-30', 'Male', '321 Pine St', NULL, 'Houston', 'TX', 'USA', '77001', '/resumes/bob_brown.pdf', 10);
GO

-- 3. Insert into Employer Table
-- Assuming UserIDs for Employers are 7 to 10
INSERT INTO Employer (UserID, CompanyName, AddressLine1, AddressLine2, City, State, Country, PostalCode, Email, Website, ContactPerson, ContactNumber) VALUES
(7, 'Tech Innovators', '100 Tech Ave', 'Floor 5', 'San Francisco', 'CA', 'USA', '94105', 'hr@techinnovators.com', 'https://www.techinnovators.com', 'Emily Clark', '415-555-0101'),
(8, 'HealthPlus', '200 Wellness Blvd', NULL, 'Boston', 'MA', 'USA', '02108', 'contact@healthplus.com', 'https://www.healthplus.com', 'Michael Lee', '617-555-0202'),
(9, 'FinanceHub', '300 Money St', 'Suite 300', 'New York', 'NY', 'USA', '10005', 'jobs@financehub.com', 'https://www.financehub.com', 'Sarah Kim', '212-555-0303'),
(10, 'EduSmart', '400 Learning Rd', NULL, 'Chicago', 'IL', 'USA', '60602', 'info@edusmart.com', 'https://www.edusmart.com', 'David Park', '312-555-0404');
GO

-- 4. Insert into SkillTag Table
INSERT INTO SkillTag (SkillName) VALUES
('Java'),
('Python'),
('SQL'),
('Project Management'),
('Communication'),
('Machine Learning'),
('Data Analysis'),
('Leadership'),
('C#'),
('UI/UX Design');
GO

-- 5. Insert into JobPosting Table
-- Assuming EmployerIDs are 1 to 4
INSERT INTO JobPosting (EmployerID, Title, Description, Location, Salary, ApplicationDeadline, JobType, Status, JobCategory) VALUES
(1, 'Software Engineer', 'Develop and maintain software applications.', 'San Francisco, CA', 120000.00, '2024-12-31', 'Full-Time', 'Active', 'IT/Software'),
(1, 'Data Analyst', 'Analyze and interpret complex data sets.', 'San Francisco, CA', 90000.00, '2024-11-30', 'Full-Time', 'Active', 'Data Science'),
(2, 'Registered Nurse', 'Provide patient care and support.', 'Boston, MA', 80000.00, '2024-10-15', 'Full-Time', 'Active', 'Healthcare'),
(2, 'Healthcare Administrator', 'Manage healthcare facilities and staff.', 'Boston, MA', 95000.00, '2024-11-01', 'Full-Time', 'Active', 'Administration'),
(3, 'Financial Analyst', 'Assess financial data and trends.', 'New York, NY', 85000.00, '2024-09-30', 'Full-Time', 'Active', 'Finance'),
(3, 'Accountant', 'Manage financial records and reports.', 'New York, NY', 75000.00, '2024-10-20', 'Full-Time', 'Active', 'Finance'),
(4, 'Educational Consultant', 'Provide guidance on educational programs.', 'Chicago, IL', 70000.00, '2024-12-15', 'Contract', 'Active', 'Education'),
(4, 'Curriculum Developer', 'Design and develop educational curricula.', 'Chicago, IL', 68000.00, '2024-11-25', 'Contract', 'Active', 'Education'),
(1, 'Frontend Developer', 'Design user interfaces and experiences.', 'San Francisco, CA', 115000.00, '2024-12-10', 'Full-Time', 'Active', 'IT/Software'),
(2, 'Physical Therapist', 'Assist patients in physical recovery.', 'Boston, MA', 82000.00, '2024-10-05', 'Full-Time', 'Active', 'Healthcare');
GO

-- 6. Insert into JobSkill Table
-- Assuming SkillTagIDs are 1 to 10 and JobIDs are 1 to 10
INSERT INTO JobSkill (JobID, SkillTagID) VALUES
(1, 1), -- Software Engineer: Java
(1, 2), -- Software Engineer: Python
(2, 3), -- Data Analyst: SQL
(2, 7), -- Data Analyst: Data Analysis
(3, 4), -- Registered Nurse: Project Management
(4, 4), -- Healthcare Administrator: Project Management
(5, 3), -- Financial Analyst: SQL
(5, 7), -- Financial Analyst: Data Analysis
(6, 3), -- Accountant: SQL
(7, 5), -- Educational Consultant: Communication
(8, 10), -- Curriculum Developer: UI/UX Design
(9, 1), -- Frontend Developer: Java
(9, 10), -- Frontend Developer: UI/UX Design
(10, 5), -- Physical Therapist: Communication
(10, 9); -- Physical Therapist: C#
GO

-- 7. Insert into JobSeekerSkill Table
-- Assuming SkillTagIDs are 1 to 10 and JobSeekerIDs are 1 to 4
INSERT INTO JobSeekerSkill (JobSeekerID, SkillTagID, SkillLevel) VALUES
(1, 1, 5), -- John Doe: Java
(1, 3, 4), -- John Doe: SQL
(1, 5, 3), -- John Doe: Communication
(2, 2, 5), -- Jane Smith: Python
(2, 4, 4), -- Jane Smith: Project Management
(2, 7, 5), -- Jane Smith: Data Analysis
(3, 6, 4), -- Alice Jones: Machine Learning
(3, 8, 3), -- Alice Jones: Leadership
(4, 9, 5), -- Bob Brown: C#
(4, 10, 4), -- Bob Brown: UI/UX Design
(1, 2, 3), -- John Doe: Python
(2, 1, 4), -- Jane Smith: Java
(3, 3, 5), -- Alice Jones: SQL
(4, 5, 4), -- Bob Brown: Communication
(1, 7, 2), -- John Doe: Data Analysis
(2, 6, 4), -- Jane Smith: Machine Learning
(3, 4, 3), -- Alice Jones: Project Management
(4, 2, 3), -- Bob Brown: Python
(1, 10, 3), -- John Doe: UI/UX Design
(2, 9, 4); -- Jane Smith: C#
GO

-- 8. Insert into AcademicDetails Table
-- Assuming JobSeekerIDs are 1 to 4
INSERT INTO AcademicDetails (JobSeekerID, Degree, EducationLevel, Marks, BoardOrUniversity, YearOfCompletion) VALUES
(1, 'B.Sc. Computer Science', 'Graduation', 85.50, 'University of XYZ', 2012),
(1, 'High School', '12th', 88.00, 'State Board', 2008),
(2, 'B.A. Economics', 'Graduation', 90.00, 'University of ABC', 2010),
(2, 'High School', '12th', 92.00, 'State Board', 2006),
(3, 'M.Sc. Data Science', 'Graduation', 87.00, 'Institute of Data', 2015),
(3, 'B.Sc. Mathematics', 'Graduation', 82.00, 'University of DEF', 2013),
(4, 'B.A. Psychology', 'Graduation', 78.00, 'University of GHI', 2011),
(4, 'High School', '12th', 80.00, 'State Board', 2007),
(1, 'M.Sc. Computer Science', 'Graduation', 89.00, 'University of XYZ', 2014),
(2, 'M.A. Economics', 'Graduation', 91.00, 'University of ABC', 2012);
GO

-- 9. Insert into JobApplication Table
-- Assuming JobSeekerIDs are 1 to 4 and JobIDs are 1 to 10
INSERT INTO JobApplication (JobSeekerID, JobID, ApplicationDate, Feedback, Status) VALUES
(1, 1, '2024-09-20', 'Looking forward to this opportunity.', 'Applied'),
(2, 2, '2024-09-21', 'Excited about the role.', 'Interviewed'),
(3, 3, '2024-09-22', 'Interested in healthcare.', 'Rejected'),
(4, 4, '2024-09-23', 'Passionate about administration.', 'Accepted'),
(1, 5, '2024-09-24', 'Strong background in finance.', 'Applied'),
(2, 6, '2024-09-25', 'Experienced in financial analysis.', 'Interviewed'),
(3, 7, '2024-09-26', 'Keen on educational consulting.', 'Rejected'),
(4, 8, '2024-09-27', 'Looking to develop curricula.', 'Accepted'),
(1, 9, '2024-09-28', 'Skilled in frontend development.', 'Applied'),
(2, 10, '2024-09-29', 'Experienced in physical therapy.', 'Interviewed');
GO

-- -----------------------------------------------------
-- 4. Verification (Optional)
-- -----------------------------------------------------

-- To verify the inserted data, you can run SELECT statements like:

SELECT * FROM [User];
SELECT * FROM JobSeeker;
SELECT * FROM Employer;
SELECT * FROM SkillTag;
SELECT * FROM JobPosting;
SELECT * FROM JobApplication;
SELECT * FROM JobSkill;
SELECT * FROM JobSeekerSkill;
SELECT * FROM AcademicDetails;
USE master;
GO
ALTER DATABASE [OnlineJobPortal] MODIFY NAME = [OnlineJobPortalDB];
GO

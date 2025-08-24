CREATE TABLE users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    id uniqueidentifier,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone_number VARCHAR(20),
    date_of_birth DATETIME NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    is_active BIT DEFAULT 1
);

CREATE TABLE user_roles (
    role_id INT IDENTITY(1,1) PRIMARY KEY,
    id uniqueidentifier,
    role_name VARCHAR(50) UNIQUE NOT NULL,
    description VARCHAR(255),
    created_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE user_role_assignments (
    user_id INT,
    role_id INT,
    assigned_at DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES user_roles(role_id) ON DELETE CASCADE
);

CREATE TABLE medical_specialties (
    specialty_id INT IDENTITY(1,1) PRIMARY KEY,
    id uniqueidentifier,
    specialty_name VARCHAR(100) UNIQUE NOT NULL,
    description VARCHAR(255),
    created_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE doctors (
    doctor_id INT IDENTITY(1,1) PRIMARY KEY,
    id uniqueidentifier,
    user_id INT UNIQUE NOT NULL,
    license_number VARCHAR(50) UNIQUE NOT NULL,
    specialty_id INT NOT NULL,
    years_of_experience INT,
    consultation_fee DECIMAL(10,2),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (specialty_id) REFERENCES medical_specialties(specialty_id)
);

CREATE TABLE patients (
    patient_id INT IDENTITY(1,1) PRIMARY KEY,
    id uniqueidentifier,
    user_id INT UNIQUE NOT NULL,
    medical_record_number VARCHAR(50) UNIQUE NOT NULL,
    insurance_number VARCHAR(50),
    emergency_contact_name VARCHAR(100),
    emergency_contact_phone VARCHAR(20),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE
);

CREATE TABLE appointment_statuses (
    status_id INT IDENTITY(1,1) PRIMARY KEY,
    status_name VARCHAR(50) UNIQUE NOT NULL,
    description VARCHAR(255),
    created_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE appointments (
    appointment_id INT IDENTITY(1,1) PRIMARY KEY,
    id uniqueidentifier,
    patient_id INT NOT NULL,
    doctor_id INT NOT NULL,
    appointment_date DATETIME NOT NULL,
    duration_minutes INT DEFAULT 30, -- I'm not sure about, stays for now.
    status_id INT NOT NULL,
    reason_for_visit VARCHAR(500),
    notes VARCHAR(1000),
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (patient_id) REFERENCES patients(patient_id),
    FOREIGN KEY (doctor_id) REFERENCES doctors(doctor_id),
    FOREIGN KEY (status_id) REFERENCES appointment_statuses(status_id)
);

CREATE TABLE doctor_schedules (
    schedule_id INT IDENTITY(1,1) PRIMARY KEY,
    id uniqueidentifier,
    doctor_id INT NOT NULL,
    schedule_date DATETIME NOT NULL,
    duration_in_minutes INT NULL,
    is_available BIT DEFAULT 1,
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (doctor_id) REFERENCES doctors(doctor_id) ON DELETE CASCADE,
);


CREATE TABLE doctor_time_off (
    time_off_id INT IDENTITY(1,1) PRIMARY KEY,
    id uniqueidentifier,
    doctor_id INT NOT NULL,
    start_datetime DATETIME NOT NULL,
    end_datetime DATETIME NOT NULL,
    reason VARCHAR(255),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (doctor_id) REFERENCES doctors(doctor_id) ON DELETE CASCADE,
    CHECK (start_datetime < end_datetime)
);

INSERT INTO user_roles (role_name, description) VALUES
('admin', 'System administrator'),
('doctor', 'Medical doctor'),
('patient', 'Patient user'),
('receptionist', 'Front desk staff');
-- others roles? 

INSERT INTO appointment_statuses (status_name, description) VALUES
('scheduled', 'Appointment is scheduled'),
('confirmed', 'Appointment is confirmed'),
('in_progress', 'Appointment is currently happening'),
('completed', 'Appointment has been completed'),
('cancelled', 'Appointment was cancelled'),
('no_show', 'Patient did not show up');

CREATE INDEX IX_appointments_date ON appointments(appointment_date);
CREATE INDEX IX_appointments_doctor ON appointments(doctor_id);
CREATE INDEX IX_appointments_patient ON appointments(patient_id);
CREATE INDEX IX_doctor_schedules_doctor_day ON doctor_schedules(doctor_id, schedule_date);
CREATE INDEX IX_users_email ON users(email);

GO

CREATE PROCEDURE sp_InsertUser
    @Id uniqueidentifier,
    @FirstName VARCHAR(100),
    @LastName VARCHAR(100),
    @Email VARCHAR(255),
    @PhoneNumber VARCHAR(20) = NULL,
    @DateOfBirth DATETIME = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @InsertedUserId INT;
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @Now DATETIME = GETDATE();

        INSERT INTO [users] (
            [id],
            [first_name],
            [last_name],
            [email],
            [phone_number],
            [date_of_birth],
            [created_at],
            [updated_at],
            [is_active]
        )
        VALUES (
            @Id,
            @FirstName,
            @LastName,
            @Email,
            @PhoneNumber,
            @DateOfBirth,
            @Now,
            @Now,
            @IsActive
        );
        
        SET @InsertedUserId = SCOPE_IDENTITY();
        
        SELECT 
            [user_id] AS UserId,
            [id] AS Id,
            [first_name] AS FirstName,
            [last_name] AS LastName,
            [email] AS Email,
            [phone_number] AS PhoneNumber,
            [date_of_birth] AS DateOfBirth,
            [created_at] AS CreatedAt,
            [updated_at] AS UpdatedAt,
            [is_active] AS IsActive
        FROM [users] 
        WHERE [user_id] = @InsertedUserId;
        
        COMMIT TRANSACTION;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        SELECT 
            'ERROR' AS Status,
            @ErrorMessage AS ErrorMessage,
            @ErrorSeverity AS ErrorSeverity,
            @ErrorState AS ErrorState,
            ERROR_NUMBER() AS ErrorNumber,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE() AS ErrorLine;
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

CREATE PROCEDURE sp_CheckEmailExists
    @Email VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
    
    BEGIN TRY
        SELECT IIF(EXISTS(SELECT TOP 1 1 FROM [users] WHERE [email] = @Email), 1, 0) AS EmailExists;
        
    END TRY
    BEGIN CATCH
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        SELECT 
            'ERROR' AS Status,
            @ErrorMessage AS ErrorMessage,
            @ErrorSeverity AS ErrorSeverity,
            @ErrorState AS ErrorState,
            ERROR_NUMBER() AS ErrorNumber,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE() AS ErrorLine;
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

CREATE PROCEDURE sp_DeleteUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DELETE FROM [users] 
        WHERE [user_id] = @UserId;
        
        COMMIT TRANSACTION;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        SELECT 
            'ERROR' AS Status,
            @ErrorMessage AS ErrorMessage,
            @ErrorSeverity AS ErrorSeverity,
            @ErrorState AS ErrorState,
            ERROR_NUMBER() AS ErrorNumber,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE() AS ErrorLine;
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

CREATE PROCEDURE sp_UpdateUser
    @UserId INT,
    @FirstName VARCHAR(100),
    @LastName VARCHAR(100),
    @Email VARCHAR(255),
    @PhoneNumber VARCHAR(20) = NULL,
    @DateOfBirth DATETIME = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @Now DATETIME = GETDATE();

        UPDATE [users] 
        SET 
            [first_name] = @FirstName,
            [last_name] = @LastName,
            [email] = @Email,
            [phone_number] = @PhoneNumber,
            [date_of_birth] = @DateOfBirth,
            [updated_at] = @Now,
            [is_active] = @IsActive
        WHERE [user_id] = @UserId;
        
        SELECT 
            [user_id] AS UserId,
            [id] AS Id,
            [first_name] AS FirstName,
            [last_name] AS LastName,
            [email] AS Email,
            [phone_number] AS PhoneNumber,
            [date_of_birth] AS DateOfBirth,
            [created_at] AS CreatedAt,
            [updated_at] AS UpdatedAt,
            [is_active] AS IsActive
        FROM [users] 
        WHERE [user_id] = @UserId;
        
        COMMIT TRANSACTION;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        SELECT 
            'ERROR' AS Status,
            @ErrorMessage AS ErrorMessage,
            @ErrorSeverity AS ErrorSeverity,
            @ErrorState AS ErrorState,
            ERROR_NUMBER() AS ErrorNumber,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE() AS ErrorLine;
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
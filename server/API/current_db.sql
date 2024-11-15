CREATE TABLE "AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);


CREATE TABLE "AspNetUsers" (
    "Id" text NOT NULL,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "Email" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);


CREATE TABLE diseases (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    name text NOT NULL,
    severity text NOT NULL,
    CONSTRAINT diseases_pkey PRIMARY KEY (id)
);


CREATE TABLE doctors (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    name text NOT NULL,
    specialty text NOT NULL,
    years_experience integer,
    CONSTRAINT doctors_pkey PRIMARY KEY (id)
);


CREATE TABLE patients (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    name text NOT NULL,
    birthdate date NOT NULL,
    gender boolean NOT NULL,
    address text,
    CONSTRAINT patients_pkey PRIMARY KEY (id)
);


CREATE TABLE treatments (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    name text NOT NULL,
    cost double precision NOT NULL,
    CONSTRAINT treatments_pkey PRIMARY KEY (id)
);


CREATE TABLE "AspNetRoleClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "RoleId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);


CREATE TABLE "AspNetUserClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "UserId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


CREATE TABLE "AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


CREATE TABLE "AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


CREATE TABLE diagnoses (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    patient_id integer NOT NULL,
    disease_id integer NOT NULL,
    diagnosis_date timestamp with time zone DEFAULT (CURRENT_TIMESTAMP),
    doctor_id integer NOT NULL,
    CONSTRAINT diagnoses_pkey PRIMARY KEY (id),
    CONSTRAINT diagnoses_disease_id_fkey FOREIGN KEY (disease_id) REFERENCES diseases (id) ON DELETE CASCADE,
    CONSTRAINT diagnoses_doctor_id_fkey FOREIGN KEY (doctor_id) REFERENCES doctors (id) ON DELETE CASCADE,
    CONSTRAINT diagnoses_patient_id_fkey FOREIGN KEY (patient_id) REFERENCES patients (id) ON DELETE CASCADE
);


CREATE TABLE patient_treatments (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    patient_id integer NOT NULL,
    treatment_id integer NOT NULL,
    start_date timestamp with time zone DEFAULT (CURRENT_TIMESTAMP),
    end_date timestamp with time zone,
    CONSTRAINT patient_treatments_pkey PRIMARY KEY (id),
    CONSTRAINT patient_treatments_patient_id_fkey FOREIGN KEY (patient_id) REFERENCES patients (id) ON DELETE CASCADE,
    CONSTRAINT patient_treatments_treatment_id_fkey FOREIGN KEY (treatment_id) REFERENCES treatments (id) ON DELETE CASCADE
);


CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");


CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");


CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");


CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");


CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");


CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");


CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");


CREATE INDEX "IX_diagnoses_disease_id" ON diagnoses (disease_id);


CREATE INDEX "IX_diagnoses_doctor_id" ON diagnoses (doctor_id);


CREATE INDEX "IX_diagnoses_patient_id" ON diagnoses (patient_id);


CREATE INDEX "IX_patient_treatments_patient_id" ON patient_treatments (patient_id);


CREATE INDEX "IX_patient_treatments_treatment_id" ON patient_treatments (treatment_id);


#!/bin/bash
dotnet ef dbcontext scaffold \
  "Server=localhost;Database=LotteryDB;User Id=testuser;Password=testpass;" \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  --output-dir ./Models \
  --context-dir . \
  --context HospitalContext  \
  --no-onconfiguring \
  --force
# 🧩 Base image with .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

# Copy everything into container
COPY . .

# 🔹 Restore and build project
RUN dotnet restore RestfulBooker.Acceptance.csproj
RUN dotnet build RestfulBooker.Acceptance.csproj -c Release --no-restore

# 🔹 Create directory for test results
RUN mkdir -p /app/TestResults

# 🔹 Install Java (for Allure)
RUN apt-get update && apt-get install -y openjdk-17-jre wget unzip

# 🔹 Install Allure CLI once inside image
RUN wget https://github.com/allure-framework/allure2/releases/download/2.29.0/allure-2.29.0.zip && \
    unzip allure-2.29.0.zip -d /opt/ && \
    ln -s /opt/allure-2.29.0/bin/allure /usr/bin/allure

# 🔹 Environment variables
ENV JAVA_HOME=/usr/lib/jvm/java-17-openjdk-amd64
ENV PATH="$JAVA_HOME/bin:${PATH}"

# ✅ Container is now ready to run tests or generate reports
CMD ["dotnet", "--info"]

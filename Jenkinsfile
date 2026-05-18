pipeline {
    agent any

    environment {
        REPO_URL = 'https://github.com/vijay002/cartdotnetcore.git'
        BRANCH = 'main'
        CREDENTIALS_ID = 'github-ssh'

        PUBLISH_DIR = "publish"
        IIS_PATH = "C:\\inetpub\\wwwroot\\cartapp"   // change to your IIS site path
        IIS_SITE = "cartapp"                        // your IIS site name
		MSBUILD='"C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe"'
    }

    stages {

        stage('Checkout') {
            steps {
                git branch: "${BRANCH}",
                    credentialsId: "${CREDENTIALS_ID}",
                    url: "${REPO_URL}"
            }
        }

        stage('Restore') {
            steps {
               
				bat '"D:\\program files\\nuget\\nuget.exe" restore %SOLUTION%'
            }
        }

        stage('Build') {
            steps {
                
				bat '''
                %MSBUILD% %SOLUTION% --configuration Release
                '''
            }
        }

        stage('Publish') {
            steps {
                bat "dotnet publish -c Release -o %PUBLISH_DIR%"
            }
        }

        stage('Deploy to IIS') {
            steps {
                script {
                    // Stop IIS Site
                    bat "powershell Stop-WebSite -Name '${IIS_SITE}'"

                    // Clean old files
                    bat "powershell Remove-Item -Recurse -Force ${IIS_PATH}\\*"

                    // Copy new files
                    bat "xcopy ${PUBLISH_DIR}\\* ${IIS_PATH}\\ /E /H /C /I /Y"

                    // Start IIS Site
                    bat "powershell Start-WebSite -Name '${IIS_SITE}'"
                }
            }
        }
    }

    post {
        success {
            echo '✅ Build & IIS Deployment Successful!'
        }
        failure {
            echo '❌ Deployment Failed!'
        }
    }
}

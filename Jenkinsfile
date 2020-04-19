pipeline {
    agent none
    environment {
        IMAGE_NAME = "registry.gitlab.com/gabrielgio/hcrawler.api"
        VERSION = "0.0.${env.BUILD_NUMBER}"
    }
    stages {
        stage('Test') {
            agent {
                docker "mcr.microsoft.com/dotnet/core/sdk:3.1"
            }
            sh "dotnet test"
        }
        stage('Build') {
            agent any
            steps {
                sh "docker build -t ${env.IMAGE_NAME}:0.0.${env.BUILD_NUMBER} -t ${env.IMAGE_NAME}:latest ."
            }
        }
	    stage('Push') {
	        agent any
            steps {
                sh "docker push ${env.IMAGE_NAME}:0.0.${env.BUILD_NUMBER}"
		        sh "docker push ${env.IMAGE_NAME}:latest"
	        }
        }
    }
}

pipeline {
    agent any
    environment {
        IMAGE_NAME = "registry.gitlab.com/gabrielgio/hcrawler.api"
        VERSION = "0.0.${env.BUILD_NUMBER}"
    }
    stages {
        stage('Build') {
            steps {
                sh "docker build -t ${env.IMAGE_NAME}:0.0.${env.BUILD_NUMBER} -t ${env.IMAGE_NAME}:latest ."
            }
        }
	    stage('Push') {
            steps {
                sh "docker push ${env.IMAGE_NAME}:0.0.${env.BUILD_NUMBER}"
		        sh "docker push ${env.IMAGE_NAME}:latest"
	        }
        }
    }
}

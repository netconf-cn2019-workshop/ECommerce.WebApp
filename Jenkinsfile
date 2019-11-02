
def PROJECT_NAME = "ECommerce.WebApp"
def IMAGE_NAME = PROJECT_NAME.substring("ECommerce.".length()).toLowerCase().replace(".", "-")
def IMAGE_TAG="$IMAGE_NAME:$DEPLOY_SUFFIX-build-$BUILD_NUMBER"
node("dotnet") {
    dir("$PROJECT_NAME"){
        stage("Fetch Code") {
            git branch: 'master', url: "https://github.com/netconf-cn2019-workshop/${PROJECT_NAME}.git"
        }
        stage('Build App') {
            sh 'dotnet build';
        }
        // stage('Code Quality') {
        //     sh 'dotnet build';
        // }
    }
}

node("image-builder"){
    container('docker'){
        dir('dev-services'){
            git branch: 'master', url: 'https://github.com/netconf-cn2019-workshop/dev-services.git'
        }
        dir("$PROJECT_NAME"){
            git branch: 'master', url: "https://github.com/netconf-cn2019-workshop/${PROJECT_NAME}.git"
        }
        stage('Package') {
            dir("$PROJECT_NAME"){
                def DOCKER_REGISTRY = sh (script: "cat ../../dev-services/services/vars | grep REGISTRY_SERVER | cut -d '=' -f 2", returnStdout: true).trim() 
                def DOCKER_REGISTRY_HOST = DOCKER_REGISTRY.split('/')[0]
                sh "docker build . -t $DOCKER_REGISTRY$IMAGE_TAG";
                withCredentials([[ $class: 'UsernamePasswordMultiBinding', credentialsId: 'dockerRegistryCredential', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASSWORD']]){
                    sh "docker login -u $DOCKER_USER -p $DOCKER_PASSWORD $DOCKER_REGISTRY_HOST";
                }
                sh "docker push $DOCKER_REGISTRY$IMAGE_TAG";
            }
        }
        stage('Deploy to dev') {
            dir('dev-services'){
                git branch: 'master', url: 'https://github.com/netconf-cn2019-workshop/dev-services.git'
                writeFile file:'services/service-list', text: "$PROJECT_NAME:$IMAGE_TAG"
                sh './provision-services.sh --env dev --suffix $DEPLOY_SUFFIX';
            }
        }
    }
}


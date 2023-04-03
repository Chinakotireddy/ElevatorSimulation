# AUTHOR : Koti
# PARAMS
#1 env	=> [local | aks] 
#2 action	=> [build | deploy | buildanddeploy] (build: creation of an image in ACR) 
#                                                (deploy: deployment to AKS)
#                                                (build_and_deploy: creation of an image in ACR and deployment to AKS)
# Deployment Environments (param: env)
# local  : runs both the app and dapr instances on the worker processes
# docker : deploys to a local instance of docker
# aks    : creates an images in ACR and deploys the container to AKS

echo "First arg: $1"
echo "Second arg: $2"
BUILD="build"
DEPLOY="deploy"
BUILDNDEPLOY="build_and_deploy"

echo "Deploying into environment: $1"

build()
{
        cd ../../..
        ACR_NAME="acrstbgtwstg"
        az acr build -t stbgtw/proximity-control:"{{.Run.ID}}" -t stbgtw/proximity-control:dev1_C1_26032023.1 -r $ACR_NAME . -f "Dockerfile.Staging"
        echo "STEP 1: DONE"
        cd ElevatorSimulation/Deployments/C1
}

deploy()
{
        echo "STEP 2: Deploying the container into AKS"
        az aks get-credentials --resource-group RG-phase2_testing-stg-san-001 --name aks-stbgtwphse2teststg-san-001
        echo "Deploying Kurbernetes components."
        cd AksComponents
        # delete previous deployment so as to ensure that we're using the latest version.  In future this will be replaced by a form of rolling-updates
        kubectl delete -f deployment-configmap.yaml
        #kubectl.exe delete deployment proximity-control -n stbgw-connectedvideo
        kubectl delete -f deployment.yaml
        sleep 15

        kubectl get pods -n stbgw-connectedvideo
        sleep 5

        # spin up K8s resources
        kubectl apply -f deployment-namespace.yaml
        kubectl apply -f deployment-configmap.yaml
        sleep 3
        kubectl get cm -n stbgw-connectedvideo -oyaml

        kubectl apply -f deployment.yaml
        sleep 5
        kubectl get pods -n stbgw-connectedvideo
        echo "STEP 2: DONE"   
}

if [ "$1" == "aks" ]
then
        if [ "$2" == "$BUILD" ]
        then
                build
        fi
        if [ "$2" == "$DEPLOY" ]
        then
                deploy
        fi
        if [ "$2" == "$BUILDNDEPLOY" ]
        then
                build
                deploy
        fi
fi
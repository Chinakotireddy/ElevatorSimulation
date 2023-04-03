# PARAMS
#1 env	=> [local | aks] 
#2 action	=> [build | deploy] (build: creation of an image in ACR) (deploy: deployment to AKS)

# Deployment Environments (param: env)
# local  : runs both the app and dapr instances on the worker processes
# docker : deploys to a local instance of docker
# aks    : creates an images in ACR and deploys the container to AKS


param 
(
[Parameter(Mandatory=$true)][string]$env,
[Parameter(Mandatory=$false)][string]$action
)

Write-Host "Deploying into environment" $env

az account set --subscription "Tools and Data Intelligence"

Write-Host "Deploying into environment" $env

if ($env -eq "aks") {
if ($action -eq "build") 
{

#######################################
# Step 1: container deployment to ACR #
#######################################
Write-Host "STEP 1: Creating an image in ACR"
Set-Location ..\..\..

$ACR_NAME="acrstbgtwstg"

# Make sure that the bash command line folder context set to the root where dockerfile exists
# With dynamic version and latest tags 
az acr build -t stbgtw/proximity-control:"{{.Run.ID}}" -t stbgtw/proximity-control:dev1_C1_26032023 -r $ACR_NAME . -f "Dockerfile.Staging"
Write-Host "STEP 1: DONE"

Set-Location .\ElevatorSimulation\Deployments\C1
}

if ($action -eq "deploy")
{

#######################################
# Step 2: Deployment to Azure K8s env #
#######################################
Write-Host "STEP 2: Deploying the container into AKS"

# create auth into context on this execution
#az aks get-credentials --resource-group rg-sde_stbgw_proximity_c1_san-001 --name "managed-cluster"
az aks get-credentials --resource-group RG-phase2_testing-stg-san-001 --name aks-stbgtwphse2teststg-san-001
# aks-stbgtwphse2teststg-san-001

Write-Host "Deploying Kurbernetes components."
Set-Location .\AksComponents

# delete previous deployment so as to ensure that we're using the latest version.  In future this will be replaced by a form of rolling-updates
kubectl.exe delete -f deployment-configmap.yaml
#kubectl.exe delete deployment proximity-control -n stbgw-connectedvideo
kubectl.exe delete -f deployment.yaml

Start-Sleep -Seconds 20
kubectl.exe get pods -n stbgw-connectedvideo
Start-Sleep -Seconds 10

# spin up K8s resources
kubectl.exe apply -f deployment-namespace.yaml
kubectl.exe apply -f deployment-configmap.yaml
kubectl.exe apply -f deployment.yaml

Write-Host "STEP 2: DONE"

# Check the deployed resources
kubectl.exe get pods -n stbgw-connectedvideo
Set-Location ..
}
}
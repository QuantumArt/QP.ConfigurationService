# QP.ConfigurationService

## Table of contents
* [1. Introduction](#introduction)
* [2. Cluster setup](#setup)
* [3. Deployment](#deployment)
* [4. Application](#application)

## 1. Introduction <a name="introduction"></a>

QP.ConfigurationService is a service which porovides access to QP Customer Codes.

## 2. Cluster setup <a name="setup"></a>

Set local path as `/yaml`. Then execute commands:
```console
kubectl apply -f namespace.yaml
kubectl apply -f pv.yaml
kubectl apply -f pvc.yaml
kubectl apply -f service.yaml
kubectl apply -f ingress.yaml
```
After all run deployment as pointed in 3.

## 3. Deployment <a name="deployment"></a>

#### 3.1 Versioning

For publishing the image with new tag one should push to git a label with major version.

#### 3.2 Building

#### 3.2.1 Running the build

To buld the image and place it to registry one should run the build
[QP.ConfigurationService.Kubernetes](https://tfs.dev.qsupport.ru/tfs/QuantumartCollection/QP/_build/index?context=allDefinitions&path=%5C&definitionId=1130&_a=completed)

The minor version part added automatically.

#### 3.2.2 Checking the registry

After build finishes the image `qp-configuration-service` must be in registry images list:
[Docker registry](http://spbdocker03:5000/v2/_catalog).
Also avaliable image tags can be seen here:
[Image tags](http://spbdocker03:5000/v2/qp-configuration-service/tags/list).

#### 3.3 Running the release

Release [QP.ConfigurationService.Kubernetes](https://tfs.dev.qsupport.ru/tfs/QuantumartCollection/QP/_release?definitionId=16&_a=releases) runs automatically after build. It apply `deployment.yaml` to kubernetes cluster with current image tag. As a result application in cluster will be updated to latest version.

## 4. Application <a name="application"></a>

### 4.1 Endpoints

Application provides endpoind:
* [API](http://dpc-configuration-service.dev.qsupport.ru/swagger)

### 4.2 Configuring Customer Codes

All Customer Codes settings is stored on persistent volume in configuration file and availible via share `\\storage\msckubepv\qp_config\config.xml`
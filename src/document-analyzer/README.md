# Document Analyzer

![document-analyzer.png](images/document-analyzer.png)

This repository contains source code and guide how to build document analyzer with Function App, Form Recognizer, Logic App and Cosmos DB.

## Prepare resource group with Azure services

Below image presents azure services used in the solution:

![document-analyzer.png](images/document-analyzer1.png)


## Train Form Recognizer model

Please use [this](https://docs.microsoft.com/en-us/azure/cognitive-services/form-recognizer/quickstarts/label-tool) instructions to setup labeling tool and train Form Recognizer model.

Below steps show how Form Recognizer model was trained:

1. Upload test files to the Azure Blob Storage:


![document-analyzer.png](images/document-analyzer3.png)

![document-analyzer.png](images/document-analyzer4.png)


2. Setup Form Recognizer model:


![document-analyzer.png](images/document-analyzer5.png)

![document-analyzer.png](images/document-analyzer6.png)

![document-analyzer.png](images/document-analyzer7.png)

![document-analyzer.png](images/document-analyzer8.png)

![document-analyzer.png](images/document-analyzer9.png)

![document-analyzer.png](images/document-analyzer10.png)

![document-analyzer.png](images/document-analyzer11.png)

![document-analyzer.png](images/document-analyzer12.png)


## Setup Azure Logic App

Below steps show how Azure Logic App is configured:

![document-analyzer.png](images/document-analyzer13.png)

![document-analyzer.png](images/document-analyzer2.png)

![document-analyzer.png](images/document-analyzer14.png)
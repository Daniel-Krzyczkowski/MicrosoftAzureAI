# Call Center Talks Analysis

## Introduction

This repository contains source code and guide how to build call center talks analysis solution with Azure Function Apps, Azure Cognitive Services, Azure Video Indexer, Azure Cosmos DB and PowerBI.

## Business case

We need a solution to get insights related to call center talks.

1. We would like to improve contact with our customers based on analysis of recorded audio and video or chat history
2. We would like to know what is the average time of the conversation with customer in our call center
3. We would like to count how many customers are satisfied with help which is given during the conversation with call center's assistant
4. What are the most popular topics of conversations



## Solution

Below diagram presents solution build on the Microsoft Azure cloud that enables uploading PDF files with conversation history, or audio/video files of recorded conversation.

![call-center-talks-analysis.png](images/call-center-talks-analysis.png)

### Components used:

1. Azure Storage Account (Blob Storage)
2. Azure Durable Functions
3. Azure Cognitive Services Text Analytics
4. Azure Cognitive Services Computer Vision
5. Azure Video Indexer
6. Azure Cosmos DB
7. Power BI


![call-center-talks-analysis-video-indexer.PNG](images/call-center-talks-analysis-video-indexer.PNG)

![call-center-talks-analysis-powerbi.png](images/call-center-talks-analysis-powerbi.png)
#region (c) 2010-2013 Lokad EventStore - New BSD License 

// Copyright (c) Lokad 2010-2013 and contributors, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Lokad.EventStore.Azure1.Tests
{
    public abstract class fixture_with_cloud_blob_client
    {
        protected CloudBlobClient BlobClient { get; private set; }

        protected fixture_with_cloud_blob_client()
        {
            var setting = Environment.GetEnvironmentVariable("LOKAD_ES_AZURE1_ACCOUNT");
            if (string.IsNullOrEmpty(setting))
            {
                setting = ConfigurationManager.AppSettings["LOKAD_ES_AZURE1_ACCOUNT"];
            }
            BlobClient = CloudStorageAccount.Parse(setting).CreateCloudBlobClient();
        }
    }
}
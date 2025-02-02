﻿using System;
using System.Web;
using PholioVisualisation.ExceptionLogging;
using PholioVisualisation.RequestParameters;

namespace PholioVisualisation.Services
{
    public abstract class JsonBuilderBase
    {
        protected HttpContextBase Context { get; private set; }
        protected BaseParameters Parameters { get; set; }

        public string ServiceName { get; set; }

        protected JsonBuilderBase()
        {
        }

        protected JsonBuilderBase(HttpContextBase context)
        {
            Context = context;
        }

        public abstract string GetJson();

        public void Respond()
        {
            var response = Context.Response;

            try
            {
                if (Parameters.AreValid)
                {
                    string json = GetJson();

                    if (json != null)
                    {
                        response.Write(json);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.LogException(ex, Context.Request.Url.AbsoluteUri);
                Context.Response.Clear();
            }

            // No error message should be returned, absence of JSON interpreted as error by clients
            response.ContentType = "application/json";
            response.Flush();
        }

    }
}

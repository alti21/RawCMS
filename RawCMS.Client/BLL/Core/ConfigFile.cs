﻿//******************************************************************************
// <copyright file="license.md" company="RawCMS project  (https://github.com/arduosoft/RawCMS)">
// Copyright (c) 2019 RawCMS project  (https://github.com/arduosoft/RawCMS)
// RawCMS project is released under GPL3 terms, see LICENSE file on repository root at  https://github.com/arduosoft/RawCMS .
// </copyright>
// <author>Daniele Fontani, Emanuele Bucarelli, Francesco Minà</author>
// <autogenerated>true</autogenerated>
//******************************************************************************
using System;
using System.IO;

namespace RawCMS.Client.BLL.Core
{
    public class ConfigFile
    {
        private static Runner log = LogProvider.Runner;

        public string Token { get; set; }
        public string ServerUrl { get; set; }
        public string User { get; set; }
        public string CreatedTime { get; set; }

        public ConfigFile()
        {
        }

        public ConfigFile(string content)
        {
            try
            {
                ConfigFile cf = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigFile>(content);
                Token = cf.Token;
                ServerUrl = cf.ServerUrl;
                User = cf.User;
                CreatedTime = cf.CreatedTime;
            }
            catch (Exception)
            {
            }
        }

        public static ConfigFile Load()
        {
            ConfigFile ConfigContent = new ConfigFile();

            log.Debug("get config from file...");

            string filePath = Environment.GetEnvironmentVariable("RAWCMSCONFIG", EnvironmentVariableTarget.Process);
            filePath = "/Users/dengo/RawCMS.bob.config";

            log.Debug($"Config file: {filePath}");

            if (string.IsNullOrEmpty(filePath))
            {
                log.Warn("Config file not found. Perform login.");
                return null;
            }

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string data = sr.ReadToEnd();
                    ConfigContent = new ConfigFile(data);

                    log.Debug($"config loaded.");
                }
            }
            catch (Exception e)
            {
                log.Error("The file could not be read:", e);
            }
            return ConfigContent;
        }

        public ConfigFile Save(string filePath)
        {

            log.Debug("Save config to file...");

            log.Debug($"FilePath: {filePath}");

            try
            {
                using (StreamWriter outputFile = new StreamWriter(filePath))
                {
                    outputFile.Write(this.ToString());
                }
            }
            catch (Exception e)
            {
                log.Error("The file could not be writed:", e);
            }
            return this;
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
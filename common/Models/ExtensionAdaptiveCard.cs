﻿// Copyright (c) Microsoft Corporation and Contributors
// Licensed under the MIT license.

using System;
using System.Text.Json.Nodes;
using AdaptiveCards.ObjectModel.WinUI3;
using AdaptiveCards.Templating;
using DevHome.Logging;
using Microsoft.Windows.DevHome.SDK;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DevHome.Common.Models;
public class ExtensionAdaptiveCard : IExtensionAdaptiveCard
{
    public event EventHandler<AdaptiveCard>? UiUpdate;

    public string DataJson { get; private set; }

    public string State { get; private set; }

    public string TemplateJson { get; private set; }

    public ExtensionAdaptiveCard()
    {
        TemplateJson = new JsonObject().ToJsonString();
        DataJson = new JsonObject().ToJsonString();
        State = string.Empty;
    }

    public ProviderOperationResult Update(string templateJson, string dataJson, string state)
    {
        var template = new AdaptiveCardTemplate(templateJson ?? TemplateJson);
        var adaptiveCardString = template.Expand(JsonConvert.DeserializeObject<JObject>(dataJson ?? DataJson));
        var parseResult = AdaptiveCard.FromJsonString(adaptiveCardString);

        if (parseResult.AdaptiveCard is null)
        {
            GlobalLog.Logger?.ReportError($"PluginAdaptiveCard.Update(): AdaptiveCard is null - templateJson: {templateJson} dataJson: {dataJson} state: {state}");
            return new ProviderOperationResult(ProviderOperationStatus.Failure, new ArgumentNullException(null), "AdaptiveCard is null", $"templateJson: {templateJson} dataJson: {dataJson} state: {state}");
        }

        TemplateJson = templateJson ?? TemplateJson;
        DataJson = dataJson ?? DataJson;
        State = state ?? State;

        if (UiUpdate is not null)
        {
            UiUpdate.Invoke(this, parseResult.AdaptiveCard);
        }

        return new ProviderOperationResult(ProviderOperationStatus.Success, null, "IExtensionAdaptiveCard.Update succeeds", "IExtensionAdaptiveCard.Update succeeds");
    }
}

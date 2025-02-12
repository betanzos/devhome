﻿// Copyright (c) Microsoft Corporation and Contributors
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppExtensions;

namespace DevHome.Common.Services;
public interface IPluginService
{
    Task<IEnumerable<IPluginWrapper>> GetInstalledPluginsAsync(bool includeDisabledPlugins = false);

    Task<IEnumerable<IPluginWrapper>> GetInstalledPluginsAsync(Microsoft.Windows.DevHome.SDK.ProviderType providerType, bool includeDisabledPlugins = false);

    Task<IEnumerable<IPluginWrapper>> StartAllPluginsAsync();

    Task SignalStopPluginsAsync();

    Task<IEnumerable<AppExtension>> GetInstalledAppExtensionsAsync();

    public event EventHandler OnPluginsChanged;

    public void EnableExtension(string extensionUniqueId);

    public void DisableExtension(string extensionUniqueId);
}

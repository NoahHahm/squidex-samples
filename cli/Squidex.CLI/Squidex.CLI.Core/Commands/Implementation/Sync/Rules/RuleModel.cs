﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Squidex.ClientLibrary;

namespace Squidex.CLI.Commands.Implementation.Sync.Rules;

internal sealed class RuleModel
{
    public bool IsEnabled { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public RuleTriggerDto Trigger { get; set; }

    [Required]
    public DynamicRuleAction Action { get; set; }

    [JsonIgnore]
#pragma warning disable CS0618 // Type or member is obsolete
    public RuleActionDto TypedAction
#pragma warning restore CS0618 // Type or member is obsolete
    {
        set
        {
            Action = new DynamicRuleAction(JObject.FromObject(value));
        }
    }
}

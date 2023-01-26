using LanguageForge.Api.Entities;
using Microsoft.IdentityModel.Tokens;

namespace LanguageForge.WebApi.Services;

public static class FilterHelpers
{
    public static bool MatchesFilterAndInputSystem(this Entry e, string? filter, string? inputSystem)
    {
        if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(inputSystem))
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(inputSystem))
        {
            if (e.Location?.Value.ContainsLf(filter) ?? false)
            {
                return true;
            }
        }

        return (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(inputSystem))
            || e.Lexeme.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.CitationForm.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.Pronunciation.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.CVPattern.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.Tone.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.Etymology.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.EtymologyGloss.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.EtymologyComment.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.EtymologySource.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.Note.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.LiteralMeaning.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.Bibliography.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.Restrictions.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || e.SummaryDefinition.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || (e.Senses?.Any(s => s.MatchesFilterAndInputSystem(filter, inputSystem)) ?? false);
    }

    public static bool MatchesFilterAndInputSystem(this Sense s, string? filter, string? inputSystem)
    {
        if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(inputSystem))
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(inputSystem))
        {
            // We can match fields that don't have an input system
            if ((s.Pictures?.Any(p => p.FileName.ContainsLf(filter)) ?? false)
                || (s.PartOfSpeech?.Value.ContainsLf(filter) ?? false)
                || (s.SemanticDomain?.Values.Any(sd => sd.ContainsLf(filter)) ?? false)
                || (s.SenseType?.Value.ContainsLf(filter) ?? false)
                || (s.Status?.Value.ContainsLf(filter) ?? false))
            {
                return true;
            }
        }

        return s.Gloss.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.Definition.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.ScientificName.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.AnthropologyNote.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.Bibliography.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.DiscourseNote.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.EncyclopedicNote.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.GeneralNote.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.GrammarNote.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.PhonologyNote.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.Restrictions.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.SemanticsNote.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.SociolinguisticsNote.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || s.Source.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || (s.Examples?.Any(ex => ex.MatchesFilterAndInputSystem(filter, inputSystem)) ?? false);
    }

    public static bool MatchesFilterAndInputSystem(this Example ex, string? filter, string? inputSystem)
    {
        if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(inputSystem))
        {
            return true;
        }

        return ex.Sentence.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || ex.Translation.AnyMatchFilterAndInputSystem(filter, inputSystem)
            || ex.Reference.AnyMatchFilterAndInputSystem(filter, inputSystem);
    }

    public static bool MatchesSemanticDomain(this Entry e, string? semanticDomain)
    {
        return string.IsNullOrEmpty(semanticDomain) || (e.Senses != null &&
            e.Senses.Any(s => s.SemanticDomain?.Values.Any(sd => sd.ContainsLf(semanticDomain)) ?? false));
    }

    public static bool MatchesPartsOfSpeech(this Entry e, List<string>? partsOfSpeech)
    {
        return partsOfSpeech.IsNullOrEmpty() ||
            (e.Senses?.Any(s => !string.IsNullOrEmpty(s.PartOfSpeech?.Value) && partsOfSpeech.Contains(s.PartOfSpeech.Value)) ?? false);
    }

    public static bool AnyMatchFilterAndInputSystem(this IDictionary<string, ValueWrapper>? source, string? filter, string? inputSystem)
    {
        if (string.IsNullOrEmpty(filter) && string.IsNullOrEmpty(inputSystem))
        {
            return true;
        }

        if (source == null)
        {
            return false;
        }

        // If both filter and inputSystem are specified, we only match fields that match both
        return source.Any(kvp =>
            (string.IsNullOrEmpty(filter) || kvp.Value.Value.ContainsLf(filter)) &&
            (string.IsNullOrEmpty(inputSystem) || (kvp.Key.ContainsLf(inputSystem) &&
            // We're looking for entries that actually use the input-system, so empty values are not a match
            !string.IsNullOrEmpty(kvp.Value.Value))));
    }

    public static bool ContainsLf(this string? target, string value)
    {
        return target?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false;
    }
}
